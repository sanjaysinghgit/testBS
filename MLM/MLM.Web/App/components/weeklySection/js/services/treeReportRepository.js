mlm.
factory('treeReportRepository', ['$resource', '$q', 'bLog', '$route', 'oDataQueryProvider', function ($resource, $q, bLog, $route, oDataQueryProvider) {
    var treeByAgentIdResource = $resource(Url.resolve('treeReportRepository') + '/:agentId?:query',
        { agentId: '@agentId', query: '@query' },
        { get: { method: 'GET', isArray: false } }
    );

    var treeBySponsorCodeResource = $resource(Url.resolve('treeReportRepository') + '/:sponsorCode?:query',
    { sponsorCode: '@sponsorCode', query: '@query' },
    { get: { method: 'GET', isArray: false } }
);


    return {
        getTreeByAgentId: function (agentId, query) {

            var requestParams = { agentId: agentId, query: oDataQueryProvider.generateQueryString(query) };
            var deferred = $q.defer();

            treeReportResource.get(requestParams, function (reprotData) {
                deferred.resolve(reprotData);
            },
                function (err) {
                    deferred.reject(err);
                    bLog.serverError("Server Error", err);
                });

            return deferred.promise;
        },

        getTreeBySponsorCode: function (sponsorCode, query) {

            var requestParams = { sponsorCode: sponsorCode, query: oDataQueryProvider.generateQueryString(query) };
            var deferred = $q.defer();

            treeBySponsorCodeResource.get(requestParams, function (reprotData) {
                deferred.resolve(reprotData);
            },
                function (err) {
                    deferred.reject(err);
                    bLog.serverError("Server Error", err);
                });

            return deferred.promise;
        }

    };
}]);