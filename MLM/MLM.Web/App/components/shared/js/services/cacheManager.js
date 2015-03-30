mlm.service('cacheManager', ['$q',
                                 '$timeout',
                                 'profileCache',
                                 'agentTreeCache',
                                 'tempCache',

    function ($q, $timeout, profileCache, agentTreeCache,  tempCache) {



        var clearGlobalCache = function () {

            var deferred = $q.defer();

            profileCache.removeAll();
            agentTreeCache.removeAll();
            tempCache.removeAll();

            // delay to make sure the cache is cleared
            $timeout(function () {
                deferred.resolve();
            }, 500);

            return deferred.promise;
        };

        var clearApplicationCache = function () {
            var deferred = $q.defer();
            clearGlobalCache();
            // delay to make sure the cache is cleared
            $timeout(function () {
                deferred.resolve();
            }, 500);
            return deferred.promise;
        };

        return {
            clearGlobalCache: clearGlobalCache,

            clearApplicationCache: clearApplicationCache
        };
    }
]);
