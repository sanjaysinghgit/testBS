mlm.controller('agentCtrl',
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



        $scope.gridOptions = {
            data: 'agentData',

            rowHeight: 30,
            enableCellEdit: true,

            selectedItems: $scope.mySelections,
            multiSelect: false,
            columnDefs: [
                { field: 'Id', displayName: 'ID', width: 50 },
                //{ field: 'Code', displayName: 'Agency Code' },
                { field: 'SponsorCode', displayName: 'Spo Code' },

                //{ field: 'Achivarprizename', displayName: 'Prize' },

                //{ field: 'CreatedDate', displayName: 'Date' },
                // {
                //     field: 'Edit',
                //     cellTemplate: '<div ><button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Edit</button> <button type="button"  ng-click="grid.appScope.delete(row.entity.Id)" >Delete</button></div>'
                //     //ellTemplate2: '<button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Delete</button> '
                // }


            ]
        };





        getAgents();
            
}]);
