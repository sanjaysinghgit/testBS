mlm.controller('TopAchiversCtrl',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'topAchivarRepository',


 

function ($scope, $route, $location, cacheManager,topAchivarRepository)
    {


    $scope.IsNewRecord = 1; //The flag for the new record

    loadRecords();

    //Function to load all TopAchivars records
    function loadRecords() {
               
               
        Clear();
        var promiseGet = topAchivarRepository.getTopAchivars(); //The MEthod Call from service

        promiseGet.then(function (pl) { $scope.Topachivars = pl.data },
              function (errorPl) {
                  $log.error('failure loading Topachivars', errorPl);
              });
   }


    console.log($scope.Id);



    //The Save scope method use to define the Employee object.
    //In this method if IsNewRecord is not zero then Update Employee else 
    //Create the Employee information to the server
   $scope.save = function () {
       var Topachivar = {
           Id: $scope.Id,
           location: $scope.location,
           name: $scope.name,
           AgencyCode: $scope.AgencyCode,
           Achivarprizename: $scope.Achivarprizename
           
       };
       //If the flag is 1 the it si new record
       if ($scope.Id === 0) {
           var promisePost = topAchivarRepository.post(Topachivar);
         
           console.log("t1");
           promisePost.then(function (data, status, headers, config) {
               //,status, headers, config
              
               console.log("TopAchivar Ctrl Save :---" + err);
               //$scope.Id = data.Id;
               
               loadRecords();
               Clear();
               //$scope.loading = false;
               //$scope.loadMoreLoading = false;
              
           },
           
           function (err) {
               
               console.log("TopAchivar Ctrl Error" + err);
               
               loadRecords();
               Clear();
               //$scope.loading = false;
               //$scope.loadMoreLoading = false;
           });
       }

       else { //Else Edit the record
           console.log("Call Edit Ctrl");
           var promisePut = topAchivarRepository.put($scope.Id, Topachivar);
           promisePut.then(function (pl) {
               $scope.Message = "Updated Successfuly";
               loadRecords();
           }, function (err) {
               console.log("Err" + err);
               loadRecords();
               Clear();
               document.getElementById("upd").innerHTML = "Save";
           });
       }



   };




    //Method to Delete
   $scope.delete = function (Id) {
       var promiseDelete = topAchivarRepository.delete(Id);
       promiseDelete.then(function (pl) {
           $scope.Message = "Deleted Successfuly";
           Clear();
           loadRecords();
       }, function (err) {
           console.log("Err" + err);
           Clear();
           loadRecords();
       });
   }

   // //Method to Get Single Employee based on EmpNo
   //$scope.get = function (Emp) {
   //    var promiseGetSingle = crudService.get(Emp.EmpNo);
 
   //    promiseGetSingle.then(function (pl) {
   //        var res = pl.data;
   //        $scope.EmpNo = res.EmpNo;
   //        $scope.EmpName = res.EmpName;
   //        $scope.Salary = res.Salary;
   //        $scope.DeptName = res.DeptName;
   //        $scope.Designation = res.Designation;
 
   //        $scope.IsNewRecord = 0;
   //    },
   //              function (errorPl) {
   //                  console.log('failure loading Employee', errorPl);
   //              });
   //}
    //Clear the Scopr models
   function Clear () {
       //$scope.IsNewRecord = 1;
       $scope.Id = 0;
       $scope.name = "";
       $scope.location = "";
       $scope.AgencyCode = "";
       $scope.Achivarprizename = "";
   }



        //Function For Fill Grid

       

        //var linkCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()">' +
        //                   '  <a href="{{row.getProperty(col.field)}}">Edit</a>' +
        //                   '</div>';

        $scope.mySelections = [];
        $scope.gridOptions = {
            data: 'Topachivars',

            rowHeight: 30,
            enableCellEdit: true,
            
            selectedItems: $scope.mySelections,
            multiSelect: false,
            columnDefs: [
                { field: 'Id', displayName: 'ID', width:50 },
                { field: 'name', displayName: 'Name' },
                { field: 'location', displayName: 'Location' },
                
                { field: 'Achivarprizename', displayName: 'Prize' },
               
                { field: 'CreatedDate', displayName: 'Date' },
                 {
                     field: 'Edit',
                     cellTemplate: '<div ><button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Edit</button> <button type="button"  ng-click="grid.appScope.delete(row.entity.Id)" >Delete</button></div>'
                     //ellTemplate2: '<button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Delete</button> '
                 }
                 
                
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
   