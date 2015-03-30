mlm.factory('tempCache', ['$angularCacheFactory', '$q',
    function ($angularCacheFactory, $q) {
        var cache = $angularCacheFactory('tempCache', {
            capacity: 1000,
            storageMode: 'localStorage',
            maxAge: 3600000,
            aggressiveDelete: true,
            cacheFlushInterval: 3600000
        });
        return cache;
    }
]);