 mlm.
service('manageAgencyRepository', function ($http) {
    

  

    

    //Get All Employees
    this.getAllagency = function () {
        return $http.get(Url.resolve('Agent/AllAgents'));
    }


   
});