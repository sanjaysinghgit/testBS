mlm.controller('TopAchiversCtrl',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'webItemsRepository',
    function (
        $scope,
        $route,
        $location,
        cacheManager,
        webItemsRepository
    ) {
        $scope.topachivers = {
            items: [],
            totalItems: 0,
        };
        function getTopachivers() {
            console.log("start getTopachivers method:");

            webItemsRepository.getTopachivers().then(function (data) {
                $scope.loading = false;
                $scope.loadMoreLoading = false;

                $scope.topachivers.items.push.apply($scope.topachivers.items, data);

                console.log($scope.topachivers.items[0])

                $scope.achiversData = $scope.topachivers.items;
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

        getTopachivers();

    }]);
