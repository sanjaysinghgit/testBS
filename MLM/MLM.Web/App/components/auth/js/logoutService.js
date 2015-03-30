mlm.factory('LoginService', ['$timeout', '$http', 'cacheManager', '$q', '$location', '$rootScope', '$window', function ($timeout, $http, cacheManager, $q, $location, $rootScope, $window) {

    return {
        logout: function () {

            cacheManager.clearApplicationCache().then(function () {
                $rootScope.developmentCount = null;
                $http.post('/logout').success(function () {
                    $timeout(function () {
                        //$window.location.href = "/login";
                        console.log("TODO: set home or login page3");
                    }, 500);
                }).error(function () {
                    $timeout(function () {
                        //$window.location.href = "/login";
                        console.log("TODO: set home or login page4");
                    }, 500);
                });
            });
        }
    };
}]);
