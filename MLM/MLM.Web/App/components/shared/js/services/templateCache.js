mlm.factory('templateCache', ['$angularCacheFactory', '$q', 'bLog',
    function ($angularCacheFactory, $q, $location, bLog) {

        var cache = $angularCacheFactory('templateCache', {
            capacity: 1000,
            storageMode: 'localStorage',
            maxAge: 3600000,
            aggressiveDelete: true,
            cacheFlushInterval: 3600000
        });

        return cache;
    }
]);

