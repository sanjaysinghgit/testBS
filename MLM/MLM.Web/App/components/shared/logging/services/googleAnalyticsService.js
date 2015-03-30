loggingModule.factory('googleAnalyticsService', ['$log', '$rootScope', '$injector', 'authCache', function ($log, $rootScope, $injector, authCache) {

    // so tests don't fail ...
    var ga = ga || function () { };
    return {
        trackPageView: function (url) {
            // Universal Google Analytics, postponed because of beta stage.
            //	ga('send', 'pageview', url);
        },
        log: function (category, action, label, value, postValue) {
            if (category && action) {
                try {
                    if (value == undefined || value == null)
                        value = 1;
                    var $route = $injector.get('$route');
                    var eventInfo = category + '.' + action + (label ? '.' + label : '');
                    var developmentName = '';
                    // if current url matches one of the listed patterns, we do not need to try retrieving a 
                    // development name, because there is none
                    var needDevelopmentName = location.pathname.match(/^\/(login|logout)/) ? false : true;

                    if (needDevelopmentName) {
                        if (category == 'Development' && action == 'Selection') {
                            var gaDevelopmentCache = authCache.get("GA_Development");
                            if (gaDevelopmentCache) {
                                if (gaDevelopmentCache.indexOf(value) >= 0) return;
                                gaDevelopmentCache.push(value);
                            }
                            else {
                                gaDevelopmentCache = [value];
                            }
                            authCache.put("GA_Development", gaDevelopmentCache);

                            developmentName = value;
                        }
                        else if (!$route.current.locals.development || !$route.current.locals.development.displayName) {
                            var m = location.href.match('developments\/([0-9]+)');
                            developmentId = m && m.length > 1 ? m[1] : 0;

                            developmentName = 'Unknown';
                            if (developmentId > 0)
                                developmentName += ' - ' + developmentId;
                        } else {
                            developmentName = $route.current.locals.development.displayName;
                        }

                        var varSuccess = _gaq.push(['_setCustomVar', 1, 'Development', developmentName, 3]) == 0;
                        if (!varSuccess || !developmentName || developmentName.substr(0, 7) == 'Unknown') {
                            Logger.warn({
                                message: 'Development name is unknown',
                                object: { event: eventInfo, success: varSuccess, developmentName: developmentName, url: document.location.href }
                            });
                        }
                    }

                    var evtSuccess = _gaq.push(['_trackEvent', category, category + '.' + action, eventInfo, value]) == 0;
                    if (!evtSuccess)
                        Logger.warn({
                            message: 'Unsuccessful Google Analytics event.',
                            object: { event: eventInfo, developmentName: developmentName, success: evtSuccess, url: document.location.href }
                        });
                    // Temp
                    Logger.warn('GA ' + eventInfo + '@' + developmentName);
                }
                catch (err) {
                    console.log(err.message);
                }
            }
        }
    };
}]);
