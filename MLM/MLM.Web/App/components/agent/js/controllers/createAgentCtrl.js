mlm.controller('createAgentCtrl',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'agentRepository',
    function (
        $scope,
        $route,
        $location,
        cacheManager,
        agentRepository
    ) {
        $scope.agents = {
            items: [],
            totalItems: 0,
        };
        function getAgents(devId, query) {
            console.log("Before method:");
            agentRepository.getAgents().then(function (agentData) {
                $scope.loading = false;
                $scope.loadMoreLoading = false;

                $scope.agents.items.push.apply($scope.agents.items, agentData);

                //if ($scope.pageLoadError) {
                //    $scope.agents.totalItems = agentData.totalItems;
                //    $scope.pageLoadError = false;
                //}
            }, function (error) {
                console.log("error in agent CTRL: " + error)
                $scope.loading = false;
                $scope.loadMoreLoading = false;
            });
        }

        getAgents();
            
}]);
