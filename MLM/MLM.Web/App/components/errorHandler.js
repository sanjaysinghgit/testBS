errorHandler.factory('errorHandlerInterceptor',
    ['$q', '$rootScope', '$window', '$location', 'cacheManager',
        function ($q, $rootScope, $window, $location, cacheManager) {

    var currentRequest;

    return {
        request: function (config) {
            currentRequest = config.method;
            return config || $q.when(config);
        },
        requestError: function(request){

        },
        response: function (response) {

            if (response && response.status !== 200) {
               return $q.reject(response);
            }
            return response || $q.when(response);
        },
        responseError: function (response) {
            if (!$rootScope.redirectOnAnyError) {
                redirectOnAnyError = true;
                return $q.reject(response);
            }
            if (response && response.status === 404) {
                if ($rootScope.redirectOn404) {
                    //$location.path('/notfound');
                    console.log("TODO: set not found page here 2");
                }
                $rootScope.redirectOn404 = true;
            }

            if (response && (response.status == 401 || response.status == 403)) {
                //Clear cache and redirect to login page
                cacheManager.clearApplicationCache().then(function () {
                    //$window.location.href = "/login";
                    console.log("TODO: set home or login page1");
                });
            }
            return $q.reject(response);
        }
    };
}]);

errorHandler.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('errorHandlerInterceptor');
}]);