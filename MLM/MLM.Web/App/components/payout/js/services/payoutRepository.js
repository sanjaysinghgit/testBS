mlm.
factory('payoutRepository', ['$resource', '$q', 'bLog', '$route', 'oDataQueryProvider', function ($resource, $q, bLog, $route, oDataQueryProvider) {
    
    var getPayoutsResource = $resource(Url.resolve('Payout/Payouts'), null,
        { get: { method: 'GET', isArray: true } }
    );

    return {
        
        getPayouts: function (agentCode, startDate, endDate) {

            var requestParams = { agentcode: agentCode, startDate: startDate, endDate: endDate };

            var deferred = $q.defer();

            getPayoutsResource.get(requestParams, function (res) {
                deferred.resolve(res);
            },
                function (err) {
                    deferred.reject(err);
                    //bLog.serverError("Server Error", err);
                    console.log("Error: " + err);
                });

            return deferred.promise;
        },

    };
}]);