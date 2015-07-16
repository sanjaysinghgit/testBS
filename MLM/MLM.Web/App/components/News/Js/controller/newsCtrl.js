mlm.controller('newsCtrl',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'newsRepository',


 

function ($scope, $route, $location, cacheManager, newsRepository)
    {


    //$scope.IsNewRecord = 1; //The flag for the new record

    loadRecords();

    //Function to load all TopAchivars records
   function loadRecords() {
       var promiseGet = newsRepository.getNews(); //The MEthod Call from service

        promiseGet.then(function (pl) { $scope.Newses = pl.data },
              function (errorPl) {
                  $log.error('failure loading News', errorPl);
              });
        Clear();
   }


    //The Save scope method use to define the Employee object.
    //In this method if IsNewRecord is not zero then Update Employee else 
    //Create the Employee information to the server
   $scope.save = function () {
       var newsModel = {
           Id: $scope.Id,
           NewsTitle: $scope.NewsTitle,
           NewsDetails: $scope.NewsDetails
       };

       
       //consol.log(newsModel);
       //If the flag is 1 the it si new record
       if ($scope.Id === 0) {
           console.log("Call Save Ctrl");
           
           var promisePost = newsRepository.post(newsModel);
          
           console.log(newsModel.Id);
           console.log(newsModel.NewsTitle);
           //console.log($scope.Id);
           promisePost.then(function (data) {
               $scope.Id = data.Id;
               loadRecords();
               Clear();
               $scope.loading = false;
               $scope.loadMoreLoading = false;
              
           },

           function (err) {
               console.log("News Ctrl" + err);
               loadRecords();
               Clear();
               $scope.loading = false;
               $scope.loadMoreLoading = false;
           });
       }

       else { //Else Edit the record
           console.log("Call Edit Ctrl");
           var promisePut = newsRepository.put($scope.Id, newsModel);
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
       var promiseDelete = newsRepository.delete(Id);
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
      
       $scope.Id = 0;
       $scope.NewsTitle = "";
       $scope.NewsDetails = "";
       
   }



        //Function For Fill Grid


        $scope.mySelections = [];
        $scope.gridOptions = {
            data: 'Newses',

            rowHeight: 30,
            enableCellEdit: true,
            
            selectedItems: $scope.mySelections,
            multiSelect: false,
            columnDefs: [
                { field: 'Id', displayName: 'ID', width:50 },
                { field: 'NewsTitle', displayName: 'Title' },
                { field: 'NewsDetails', displayName: 'Detail' },
                {
                     field: 'Edit',
                     cellTemplate: '<div ><button type="button" ng-click="grid.appScope.Edit(row.entity)" >Edit</button> <button type="button"  ng-click="grid.appScope.delete(row.entity.Id)" >Delete</button></div>'
                     //ellTemplate2: '<button type="button" ng-click="grid.appScope.topachivarEdit(row.entity)" >Delete</button> '
                 }
                 
                
            ]
        };


       

        $scope.Edit = function (data) {


            document.getElementById("upd").innerHTML = "Update";
            $scope.Id = data.Id
            $scope.NewsTitle = data.NewsTitle;
            $scope.NewsDetails = data.NewsDetails;
            };
       



    }]);
   