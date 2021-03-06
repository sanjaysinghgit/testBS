﻿mlm.controller('payoutCtrlAgent',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'payoutRepository',




function ($scope, $route, $location, cacheManager, payoutRepository) {


  

    loadRecords($route.current.locals.currentUser.AgentInfo.Code);

    console.log($route.current.locals.currentUser.AgentInfo.Code);

    //Function to load all TopAchivars records
    function loadRecords(Code) {

        console.log(Code);
        
        var promiseGet = payoutRepository.getPayoutsbyagcode(Code); //The MEthod Call from service

        promiseGet.then(function (pl) { $scope.payouts = pl.data },
              function (errorPl) {
                  $log.error('failure loading Topachivars', errorPl);
              });
    }




    $scope.mySelections = [];
    $scope.gridOptions = {
        data: 'payouts',

        rowHeight: 30,
        enableCellEdit: true,

        selectedItems: $scope.mySelections,
        multiSelect: false,
        columnDefs: [
            { field: 'Id', displayName: 'Voucher No', width: 50 },
            { field: 'VoucherDate', displayName: 'Voucher Date',cellFilter:'date:\'dd-MM-yyyy\'' ,width:100 },
            { field: 'AgentCode', displayName: 'Agency Code', width: 100 },
            //{ field: 'Name', displayName: 'Name', width: 100 },

            //{ field: 'FatherName', displayName: 'Father Name', width: 100 },

            { field: 'TotalLeftPair', displayName: 'Total Left',width:100 },

            { field: 'TotalRightPair', displayName: 'Total Right', width: 100 },
            { field: 'PairsInThisPayout', displayName: 'Pairs Payout', width: 100 },
            { field: 'TDS', displayName: 'TDS', width: 100 },
            { field: 'PairIncome', displayName: 'Pair Income', width: 100 },
            { field: 'SaveIncome', displayName: 'SaveIncome', width: 100 },
             { field: 'NetIncome', displayName: 'NetIncome', width: 100 },
                { field: 'DispatchedAmount', displayName: 'DispatchedAmount', width: 150 }
            //{ field: 'CreatedDate', displayName: 'Date' },
            // {
            //     field: 'Edit',
            //     cellTemplate: '<div ><button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Edit</button> <button type="button"  ng-click="grid.appScope.delete(row.entity.Id)" >Delete</button></div>'
            //     //ellTemplate2: '<button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Delete</button> '
            // }


        ]
    };


            


        //console.log("Shobhit");
        //console.log();
        //getPayouts($route.current.locals.currentUser.AgentInfo.Code, "01-01-1900", "01-01-1900");
        






        
        //  getPayouts("", "1-1-2015 0:00", "4-25-2015 23:59");  
}]);
