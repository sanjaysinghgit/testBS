mlm.controller('manageAgencyCtrl',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'manageAgencyRepository',


 

function ($scope, $route, $location, cacheManager, manageAgencyRepository)
    {


   

    loadRecords();

    //Function to load all TopAchivars records
    function loadRecords() {
               
               
      
        var promiseGet = manageAgencyRepository.getAllagency()

        promiseGet.then(function (pl) { $scope.AllAgency = pl.data },
              function (errorPl) {
                  $log.error('failure loading Topachivars', errorPl);
              });
   }


    







        //Function For Fill Grid

       

        //var linkCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()">' +
        //                   '  <a href="{{row.getProperty(col.field)}}">Edit</a>' +
        //                   '</div>';

        $scope.mySelections = [];
        $scope.gridOptions = {
            data: 'AllAgency',

            rowHeight: 30,
            enableCellEdit: true,
            
            selectedItems: $scope.mySelections,
            multiSelect: false,
            columnDefs: [
               



                { field: 'Code', displayName: 'Agency Code',width: 100 },
                { field: 'Name', displayName: 'Name' ,width:200},
                
                 
                { field: 'SponsorCode', displayName: 'Spo Code', width: 100 },
                
                { field: 'IntroducerCode', displayName: 'Introd Code' ,width:100},
                 { field: 'Position' ,width:50},
              
                { field: 'LeftAgent', width: 100 },
                 { field: 'RightAgent', width: 100 },
                 
                   { field: 'SaveIncomeStatus', displayName: 'Save Incom', width: 50 },
                   
                     { field: 'ActivationDate', displayName: 'Activation Date',cellFilter:'date:\'dd-MM-yyyy\'' , width: 100 },
                      { field: 'Status', displayName: 'Active Status', width: 50 },
                      { field: 'VoucherStatus', displayName: 'Voucher Status', width: 50 },
                      { field: 'FatherName', displayName: 'FatherName', width: 200 },
                       { field: 'Address', displayName: 'Address', width: 400 },


               // {
                //     field: 'Edit',
                //     cellTemplate: '<div ><button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Edit</button> <button type="button"  ng-click="grid.appScope.delete(row.entity.Id)" >Delete</button></div>'
                //     //ellTemplate2: '<button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Delete</button> '
                // }
                 
                
            ]
        };


       

        $scope.topachivarEdit = function (data) {


            document.getElementById("upd").innerHTML = "Update";
            $scope.Id = data.Id
            $scope.name = data.name;
            $scope.location = data.location;

            $scope.AgencyCode = data.AgencyCode;
            $scope.Achivarprizename = data.Achivarprizename;
           
            
             
   
            };
       


        //    //$http.get('http://localhost/MLM.Web/api/TopAchivars/GetTopAchivar', id).
        //    //     success(function (data, status, headers, config) {


        //    //         TopAchivar= data;

        //    //     }).
        //    //     error(function (data, status, headers, config) {
        //    //         alert('error')


        //    //     });

        //};



       







        


    }]);
   