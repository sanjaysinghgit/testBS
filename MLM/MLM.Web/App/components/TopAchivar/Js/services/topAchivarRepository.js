 mlm.
service('topAchivarRepository', function ($http) {
    

  

    //Create new record
    this.post = function (Topachivar) {
        //consol.log(Topachivar)

        var request = $http({
            method: "post",
            
            url: Url.resolve('TopAchivars/PostTopAchivar'),
            data: Topachivar
        });
        //consol.log(request);
        return request;
    }

    //Get Single Records
    //this.get = function (EmpNo) {
    //    return $http.get("/api/EmployeesAPI/" + EmpNo);
    //}

    //Get All Employees
    this.getTopAchivars = function () {
        return $http.get(Url.resolve('TopAchivars/GetTopAchivars'));
    }


    //Update the Record
    this.put = function (Id, TopAchivar) {
        var request = $http({
            method: "put",
            url: Url.resolve('TopAchivars/PutTopAchivar/') + Id,
            //url: "/api/EmployeesAPI/" + EmpNo,
            data: TopAchivar
        });
        return request;
    }
    ////Delete the Record
    this.delete = function (Id) {
        var request = $http({
            method: "delete",
            url: Url.resolve('TopAchivars/DeleteTopAchivar/') + Id
        });
        return request;
    }
});