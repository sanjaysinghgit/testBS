mlm.factory('authCache', ['$angularCacheFactory', '$q',
    function ($angularCacheFactory, $q) {
        var cache = $angularCacheFactory('authCache', {
            capacity: 1000,
            storageMode: 'localStorage',
            maxAge: 3600000,
            aggressiveDelete: true,
            cacheFlushInterval: 3600000
        });
        return cache;
    }
]);