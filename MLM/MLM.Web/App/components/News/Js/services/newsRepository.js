 mlm.
service('newsRepository', function ($http) {
    

    //Create new record
    this.post = function (newsModel) {
        var request = $http({
            method: "post",
            // url: Url.resolve('NewsModels/PostNewsModel?newsModel='+newsModel),
            url: Url.resolve('NewsModels/PostNewsModel'),
            data: newsModel
        });
       // console.log("Abc : " + newsModel.NewsTitle + " - " + newsModel.NewsDetails);
        return request;
    }
    
    //Get Single Records
    //this.get = function (EmpNo) {
    //    return $http.get("/api/EmployeesAPI/" + EmpNo);
    //}

    //Get All Employees
    this.getNews = function () {
        return $http.get(Url.resolve('NewsModels/GetNewsModels'));
    }


    //Update the Record
    this.put = function (Id, newsModel) {
        var request = $http({
            method: "put",
            url: Url.resolve('NewsModels/PutNewsModel/') + Id,
            //url: "/api/EmployeesAPI/" + EmpNo,
            data: newsModel
        });
        return request;
    }
    ////Delete the Record
    this.delete = function (Id) {
        var request = $http({
            method: "delete",
            url: Url.resolve('NewsModels/DeleteNewsModel/') + Id
        });
        return request;
    }
});