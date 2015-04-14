mlm.controller('mainCtrl',
    ['$scope',
     '$location',
     '$route',
     '$window',
     '$rootScope',
     '$q',
     'profileCache',
     'cacheManager',
     'agentRepository',
     'bLog',

    function ($scope,
             $location,
             $route,
             $window,
             $rootScope,
             $q,
             profileCache,
             cacheManager,
             agentRepository,
             bLog
             ) {
        //console.log("inMain");
        $scope.CurrentUser = $route.current.locals.currentUser;
        console.log($route.current.locals.currentUser);
    }]);