mlm.factory('profileCache', ['$angularCacheFactory', '$q',
    function ($angularCacheFactory, $q) {
        var cache = $angularCacheFactory('profileCache', {
            capacity: 1000,
            storageMode: 'localStorage',
            maxAge: 60 * 60 * 1000, // 1 hour,,
            aggressiveDelete: true,
            cacheFlushInterval: 3600000
        });

        // Add static properties
        cache = angular.extend(cache,
            {
                kKeyCurProfile: 'userProfile'
            });

        return cache;
    }
]);
