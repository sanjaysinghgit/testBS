mlm.
factory('webItemsRepository', ['$resource', '$q', 'bLog', '$route', 'oDataQueryProvider', function ($resource, $q, bLog, $route, oDataQueryProvider) {

    var getTopachiversResource = $resource(Url.resolve('TopAchivars/Topachivers'), null,
        { get: { method: 'GET', isArray: true } }
    );

    return {

        getTopachivers: function () {

            //var requestParams = { agentcode: agentCode, startDate: startDate, endDate: endDate };

            var deferred = $q.defer();

            getTopachiversResource.get(function (res) {
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