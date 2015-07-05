mlm.controller('payoutCtrlAgent',
    [
    '$scope',
    '$route',
    '$location',
    'profileCache',
    'cacheManager',
    'agentRepository',
    'payoutRepository',
    function (
        $scope,
        $route,
        $location,
        profileCache,
        cacheManager,
        agentRepository,
        payoutRepository
    ) {
        $scope.payouts = {
            items: [],
            totalItems: 0,
        };

        function getPayouts(agentCode, startDate, endDate) {
            console.log("start getPayouts method:");

            payoutRepository.getPayouts(agentCode, startDate, endDate).then(function (data) {
                $scope.loading = false;
                $scope.loadMoreLoading = false;

                $scope.payouts.items.push.apply($scope.payouts.items, data);

                console.log($scope.payouts.items[0])

                $scope.gridData = $scope.payouts.items;
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
        console.log("Shobhit");
        console.log($route.current.locals.currentUser.AgentInfo.Code);
        getPayouts($route.current.locals.currentUser.AgentInfo.Code, "01-01-1900", "01-01-1900");
        
        
        //  getPayouts("", "1-1-2015 0:00", "4-25-2015 23:59");  
}]);
