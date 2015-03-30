mlm.config(['$httpProvider', function ($httpProvider) {
    var requestInterceptor = ['$q', '$rootScope', '$location', 'authCache', '$window', 'cacheManager', function ($q, $rootScope, $location, authCache, $window, cacheManager) {
        var intervalInMinutesToBeAuthenticated = 60; // 60 for one hour
        var intervalInMinutesToConsiderAlmostExpire = 50;

        var checkExpiration = function () {
            var expTimeCookie = authCache.get('SessionExpireTime');
            if (expTimeCookie) {
                var exp = new Date(expTimeCookie);
                var diff = (exp - new Date()) / 60000;
                return diff < intervalInMinutesToConsiderAlmostExpire;
            }
            return true;
        };
        var interceptorInstance = {
            request: function (config) {
                // Do this call only for server requests and not for refreshToken request.
                if (config.url.indexOf('refreshtoken') == -1 && config.url.indexOf('authorize') == -1 && config.url.indexOf('.html') == -1) {
                    if (checkExpiration()) {
                        $.ajax({
                            url: '/refreshtoken',
                            async: false,
                            type: 'GET',
                            headers: {
                                'RefreshToken': authCache.get("RefreshToken")
                            },
                            dataType: "json",
                        }).done(function (data, status, header) {
                            authCache.put("AuthorizationToken", data.AuthorizationToken);
                            authCache.put("RefreshToken", data.RefreshToken);
                            authCache.put('SessionExpireTime', moment().add('minutes', intervalInMinutesToBeAuthenticated));
                            return config || $q.when(config);
                        }).fail(function () {
                            cacheManager.clearApplicationCache().then(function () {
                                //$window.location.href = "/login";
                                console.log("TODO: set home or login page2");
                            });
                        });
                    }
                }
                return config || $q.when(config);
            }
        };
        return interceptorInstance;
    }];

    //$httpProvider.interceptors.push(requestInterceptor);
}]);