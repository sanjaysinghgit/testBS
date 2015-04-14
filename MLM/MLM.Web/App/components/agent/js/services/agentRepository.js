mlm.
factory('agentRepository', ['$resource', '$q', 'bLog', 'agentTreeCache', 'profileCache', '$route', 'oDataQueryProvider', function ($resource, $q, bLog, agentTreeCache, profileCache, $route, oDataQueryProvider) {
    
    var getAgentsResource = $resource(Url.resolve('Agent/Agents'), null,
        { get: { method: 'GET', isArray: true } }
    );
    var getAgentTreeResource = $resource(Url.resolve('Agent/Tree/:agentcode'),
        { agentcode: '@agentcode' },
    { get: { method: 'GET', isArray: true } }
    );
    var getAgentDetailResource = $resource(Url.resolve('Agent/Details/:agentcode'),
    { agentcode: '@agentcode' },
    { get: { method: 'GET', isArray: false } }
    );
    var getCurrentUserResource = $resource(Url.resolve('ApplicationUser/GetCurrentUser'), null,
        { get: { method: 'GET', isArray: false } }
    );
    var getCurrentUserRolesResource = $resource(Url.resolve('ApplicationUser/GetCurrentUserRoles'), null,
        { get: { method: 'GET', isArray: true } }
    );

    return {
        
        getAgentTree: function (agentcode) {

            var deferred = $q.defer();

            // attempt to get cached permission by contextId
            var agentTree = agentTreeCache.get(agentcode);

            var requestParams = { agentcode: agentcode };

            // if there is a cached copy then resolve with it
            if (agentTree) {
                deferred.resolve(agentTree);
            } else {
                getAgentTreeResource.get(requestParams, function (res) {
                    agentTreeCache.put(agentcode, res);
                    deferred.resolve(res);
                },
                    function (err) {
                        deferred.reject(err);
                        //bLog.serverError("Server Error", err);
                        console.log("Error: " + err);
                    });
            }
            return deferred.promise;
        },
        getAgentDetails: function (agentcode) {

            var deferred = $q.defer();

            var requestParams = { agentcode: agentcode };


                getAgentDetailResource.get(requestParams, function (res) {
                    deferred.resolve(res);
                },
                    function (err) {
                        deferred.reject(err);
                        //bLog.serverError("Server Error", err);
                        console.log("Error: " + err);
                    });
            return deferred.promise;
        },
        getAllAgents: function () {

            var requestParams = {};
            var deferred = $q.defer();

            getAgentsResource.get(requestParams, function (res) {
                deferred.resolve(res);
            },
                function (err) {
                    deferred.reject(err);
                    //bLog.serverError("Server Error", err);
                    console.log("Error: " + err);
                });

            return deferred.promise;
        },
        getCurrentUserRoles: function () {

            var requestParams = {};
            var deferred = $q.defer();

            getCurrentUserRolesResource.get(requestParams, function (res) {
                deferred.resolve(res);
            },
                function (err) {
                    deferred.reject(err);
                    //bLog.serverError("Server Error", err);
                    console.log("Error: " + err);
                });

            return deferred.promise;

        },
        getCurrentUser: function () {

            var requestParams = {};
            var deferred = $q.defer();

            getCurrentUserResource.get(requestParams, function (res) {
                deferred.resolve(res);
            },
                function (err) {
                    deferred.reject(err);
                    //bLog.serverError("Server Error", err);
                    console.log("Error: " + err);
                });

            return deferred.promise;


            //var currentProfile =
            //   profileCache.get(profileCache.kKeyCurProfile)

            //if (currentProfile) {
            //    deferred.resolve(currentProfile);
            //} else {

            //    getCurrentUserResource.get(requestParams, function (res) {
            //        profileCache.put(profileCache.kKeyCurProfile, res);
            //        deferred.resolve(res);
            //    },
            //        function (err) {
            //            deferred.reject(err);
            //            //bLog.serverError("Server Error", err);
            //            console.log("Error: " + err);
            //        });
            //}
            //return deferred.promise;
        },

    };
}]);