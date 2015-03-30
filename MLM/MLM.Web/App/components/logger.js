// We need to be able to use Logger out of Angular context.
// Therefore it needs to be accessible globally.

Logger = {
    isTraceEnabled: false,
    isDebugEnabled: false,
    isInfoEnabled: true,
    isWarnEnabled: true,
    isErrorEnabled: true,
    isFatalEnabled: true,
    refreshPeriod: 60,
    defaults: { userId: 0, sessionStart: '' },

    getJQuerySelector: function (obj) {
        if (!obj || obj.tagName == 'BODY')
            return '';
        if (obj.id != '')
            return '#' + obj.id;
        var siblings = Array.prototype.slice.call(obj.parentElement.getElementsByTagName(obj.tagName), 0);
        var selector = (Logger.getJQuerySelector(obj.parentElement) + ' ' + obj.tagName.toLowerCase()).trim();
        if (siblings.length > 1)
            selector += ':eq(' + siblings.indexOf(obj) + ')';
        return selector;
    },
    getXPathSelector: function (obj) {
        if (!obj || obj.tagName == 'BODY')
            return '//html/body';
        var siblings = [];
        var list = obj.parentElement.getElementsByTagName(obj.tagName);
        for (var i = 0; i < list.length; i++)
            if (list[i].parentElement == obj.parentElement)
                siblings.push(list[i]);
        var selector = (Logger.getXPathSelector(obj.parentElement) + '/' + obj.tagName.toLowerCase()).trim();
        if (siblings.length > 1)
            selector += '[' + (siblings.indexOf(obj) + 1) + ']';
        return selector;
    },
    getElementByXPath: function (xpath) {
        return document.evaluate(xpath, document, null, 9, null).singleNodeValue;
    },
    getUxStack: function () {
        var stack = JSON.parse(localStorage.getItem('Logger_Ux_Stack')) || [];
        var offset = 0;
        var count = stack.length;
        if (arguments.length > 1) {
            offset = parseInt(arguments[0]);
            count = parseInt(arguments[1]);
        }
        else if (arguments.length == 1) {
            count = parseInt(arguments[0]);
            offset = stack.length - count;
        }
        offset = Math.max(0, Math.min(stack.length - 1, offset));
        count = Math.max(1, Math.min(stack.length, count));
        return stack.slice(offset, offset + count);
    },
    enableUxMonitor: function () {
        if (typeof Logger.enableUxMonitor.logEvent == 'undefined')
            Logger.enableUxMonitor.logEvent = function (e) {
                try {
                    Logger.logUxEvent(e.type + '("' + Logger.getXPathSelector(e.target) + '")');
                    if (location.href != localStorage.getItem('Logger_Ux_Url')) {
                        Logger.logUxEvent('location("' + location.href + '")');
                        localStorage.setItem('Logger_Ux_Url', location.href);
                    }
                }
                catch (exc) { }
            };
        $(window).on({
            click: Logger.enableUxMonitor.logEvent,
            touchend: Logger.enableUxMonitor.logEvent
        });
    },
    disableUxMonitor: function () {
        $(window).off({
            click: Logger.enableUxMonitor.logEvent,
            touchend: Logger.enableUxMonitor.logEvent
        });
    },
    logUxEvent: function (evt) {
        try {
            evt = (new Date()).toISOString() + ': ' + evt;
            var stack = Logger.getUxStack();
            stack.push(evt);
            if (stack.length > 20)
                stack = stack.slice(1);
            localStorage.setItem('Logger_Ux_Stack', JSON.stringify(stack));
            localStorage.setItem('Logger_Ux_SendCount', 0);
        }
        catch (e) { }
    },
    trace: function (entry) {
        if (Logger.isTraceEnabled)
            Logger.log(entry, 'trace');
    },
    debug: function (entry) {
        if (Logger.isDebugEnabled)
            Logger.log(entry, 'debug');
    },
    info: function (entry) {
        if (Logger.isInfoEnabled)
            Logger.log(entry, 'info');
    },
    warn: function (entry) {
        if (Logger.isWarnEnabled)
            Logger.log(entry, 'warn');
    },
    error: function (entry) {
        if (Logger.isErrorEnabled)
            Logger.log(entry, 'error');
    },
    fatal: function (entry) {
        if (Logger.isFatalEnabled)
            Logger.log(entry, 'fatal');
    },
    log: function (entry) {
        try {
            if (entry === undefined || entry === null) return;
            var defLevel = arguments.length > 1 ? arguments[1].toLowerCase() : 'info';
            var entryObj;
            if (typeof entry == 'object')
                entryObj = entry.message && entry.stack ? Logger.convertToLogEntry(entry) : entry;
            else
                entryObj = { message: entry };

            if (!entryObj.level)
                entryObj.level = defLevel;
            else
                entryObj.level = entryObj.level.toLowerCase();

            var index = ['trace', 'debug', 'info', 'warn', 'error', 'fatal'].indexOf(entryObj.level);
            if (index < 0) {
                entryObj.level = 'info';
                index = 2;
            }

            switch (index) {
                case 0: if (!Logger.isTraceEnabled) return; break;
                case 1: if (!Logger.isDebugEnabled) return; break;
                case 2: if (!Logger.isInfoEnabled) return; break;
                case 3: if (!Logger.isWarnEnabled) return; break;
                case 4: if (!Logger.isErrorEnabled) return; break;
                case 5: if (!Logger.isFatalEnabled) return; break;
            }

            var uxStack = '';
            var sendCount = parseInt(localStorage.getItem('Logger_Ux_SendCount')) || 0;
            if (sendCount < 5) {
                localStorage.setItem('Logger_Ux_SendCount', sendCount + 1);
                uxStack = Logger.getUxStack().join('\r\n');
            }

            entryObj.object = JSON.stringify({
                uxStack: uxStack,
                sessionStart: Logger.defaults.sessionStart,
                data: entryObj.object
            });

            if (!entryObj.userId) {
                var profile = JSON.parse(localStorage.getItem("angular-cache.caches.profileCache.data.userProfile"));
                entryObj.userId = profile && profile.value && profile.value.id ? profile.value.id : Logger.defaults.userId;
            }
            if (!entryObj.developmentId) {
                var m = location.href.match('developments\/([0-9]+)');
                entryObj.developmentId = m && m.length > 1 ? m[1] : 0;
            }
            if (!entryObj.stackTrace)
                entryObj.stackTrace = Logger.getStackTrace();
            if (!entryObj.requestUrl)
                entryObj.requestUrl = document.location.href;

            if (Logger.log.sendRequest)
                Logger.log.sendRequest(entryObj);
        }
        catch (e) {
            // swallow the exception and avoid infinite loop
        }
    },
    convertToLogEntry: function (exc) {
        var skip = arguments.length > 1 ? Math.max(0, parseInt(arguments[1])) : 0;
        var lines = exc.stack.split('\n');
        var message = exc.message ? exc.message : 'Unhandled exception';
        var exception = lines && lines.length ? lines[0].replace('<', '[').replace('>', ']') : exc.message;
        var arr = [];
        var len = lines.length;
        for (var i = skip + 1; i < len; i++)
            arr.push(lines[i].replace('at ', '').replace('<', '[').replace('>', ']').trim());
        return { message: message, exception: exception, stackTrace: arr.join('\r\n') };
    },
    getStackTrace: function () {
        try {
            raise.an.exception++;
        } catch (e) {
            var temp = Logger.convertToLogEntry(e, 1);
            return temp.stackTrace;
        }
        return '';
    },

    init: function () {
        Logger.log.shouldRefreshSettings = function () {
            try {
                var diff = (new Date() - Logger.log.lastRefreshTime);
                return !Logger.log.lastRefreshTime || (new Date() - Logger.log.lastRefreshTime) > Logger.refreshPeriod * 1000;
            }
            catch (e) { return false; }
        }

        Logger.log.refreshSettings = function () {
            try {
                if (Logger.log.shouldRefreshSettings()) {
                    Logger.log.lastRefreshTime = new Date();
                    Logger.log.sendRequest(null);
                }
            }
            catch (e) { }
            setTimeout(Logger.log.refreshSettings, (Math.min(3600, Math.max(10, Logger.refreshPeriod)) + Math.random() * 10 - 5) * 1000);
        },

        Logger.log.applySettings = function (data) {
            if (!data) return;
            if (data.hasOwnProperty('IsTraceEnabled'))
                Logger.isTraceEnabled = data.IsTraceEnabled;
            if (data.hasOwnProperty('IsDebugEnabled'))
                Logger.isDebugEnabled = data.IsDebugEnabled;
            if (data.hasOwnProperty('IsInfoEnabled'))
                Logger.isInfoEnabled = data.IsInfoEnabled;
            if (data.hasOwnProperty('IsWarnEnabled'))
                Logger.isWarnEnabled = data.IsWarnEnabled;
            if (data.hasOwnProperty('IsErrorEnabled'))
                Logger.isErrorEnabled = data.IsErrorEnabled;
            if (data.hasOwnProperty('IsFatalEnabled'))
                Logger.isFatalEnabled = data.IsFatalEnabled;
            if (data.hasOwnProperty('RefreshPeriod'))
                Logger.refreshPeriod = Math.min(3600, Math.max(10, data.RefreshPeriod));
        };

        Logger.log.sendRequest = function (entryObj) {
            var info = !entryObj || Logger.log.shouldRefreshSettings() ? '?info' : '';
            $.ajax('/log' + info, {
                async: true,
                type: 'POST',
                data: entryObj == null ? null : JSON.stringify(entryObj),
                contentType: 'application/json',
                error: function () { },
                success: Logger.log.applySettings
            });
        };

        Logger.enableUxMonitor();
        Logger.logUxEvent('pageLoad(' + location.href + ')');
        setTimeout(Logger.log.refreshSettings, Logger.refreshPeriod * 1000);
    }
};

$(function () {
    Logger.init();
});
