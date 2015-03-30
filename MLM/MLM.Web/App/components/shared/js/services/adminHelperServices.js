mlm.service('adminHelperServices', ['$resource', '$q', 'bLog',
    function ($resource, $q, bLog) {

        var permissionResource = $resource(Url.resolve('users/:userId/objects/:objectId/permissions'),
            { userId: '@user', objectId: '@context' });

        var profileResource = $resource(Url.resolve('users/self'), {}, {
            get: { method: 'GET', isArray: false },
            patch: { method: 'PATCH', isArray: false }
        });

        return {
          
            getPermissions: function(contextId, userId) {

                var deferred = $q.defer();
                    permissionResource.get({ userId: userId, objectId: contextId }, function (permissions) {
                        deferred.resolve(permissions.items);
                    }, function (res) {
                        deferred.reject(res);
                    });
                return deferred.promise; 
            },
            getCurrentUser: function () {
                var deferred = $q.defer();
                profileResource.get(function (profileData) {
                    deferred.resolve(profileData);
                }, function (error) {
                    deferred.reject(error);
                });

                return deferred.promise;
            }
    }
}]);

