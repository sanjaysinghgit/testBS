mlm.factory('agentTreeCache', ['$angularCacheFactory', '$q', 'bLog',
    function ($angularCacheFactory, $q, $location, bLog) {

        var cache = $angularCacheFactory('agentTreeCache', {
            capacity: 1000,
            storageMode: 'localStorage',
            maxAge: 60 * 60 * 1000, // 1 hour,,
            aggressiveDelete: true,
            cacheFlushInterval: 3600000
        });

        return cache;
    }
]);

