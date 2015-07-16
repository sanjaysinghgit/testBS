mlm.
service('payoutRepository', function ($http) {
    
 
    //Get All Payouts
    this.getPayouts = function () {
        return $http.get(Url.resolve('Payouts/Payouts'));
    }
    //Get All Payouts by agentcode
    this.getPayoutsbyagcode = function (Id) {
        return $http.get(Url.resolve('Payouts/GetPayout/')+ Id);
    }


    
});