mlm.controller('agentDetailsCtrl',
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
        console.log("In agentDetailsCtrl");
        $scope.CurrentUser = $route.current.locals.currentUser;
        
        function getAgentDetails(agentCode) {

            var deferred = $q.defer();

            agentRepository.getAgentDetails(agentCode).then(function (agentData) {
                console.log(agentData);
                if (agentData) {
                    $scope.AgentDetails = agentData;
                    $scope.AgentDetails.TotalPair = agentData.TotalRight > agentData.TotalLeft ? agentData.TotalRight : agentData.TotalLeft;
                    $scope.AgentDetails.CarryLeft = agentData.TotalLeft > agentData.TotalRight ? (agentData.TotalLeft - agentData.TotalRight) : 0;
                    $scope.AgentDetails.CarryRight = agentData.TotalRight > agentData.TotalLeft ? (agentData.TotalRight - agentData.TotalLeft) : 0;

                    deferred.resolve(agentData);
                }
            }, function (error) {
                deferred.reject(error);
                console.log("error in agent CTRL: " + error)
            });
            // Set the promise to the member variable to be accessed by other
            // functions that have it as a dependency
            $scope.AgentDetails = deferred.promise;
            return deferred.promise;
        }
        //console.log($route.current.locals.currentUser.AgentInfo.Code);
        getAgentDetails($route.current.locals.currentUser.AgentInfo.Code);



    }]);