mlm.config(['$httpProvider', function ($httpProvider) {
    var headerInterceptor = [
        '$q',
        '$injector',
        'authCache',
        function (
            $q,
            $injector,
            authCache
        ) {
            var interceptorInstance = {
                request: function (config) {
                    ///########################################################
                    /// Injecting Authrization token in header
                    ///########################################################
                    var authorizationToken = authCache.get("AuthorizationToken");
                    if (authorizationToken) {
                        config.headers['AuthorizationToken'] = authorizationToken;
                    }

                    ///########################################################
                    // Site was becoming unresposive if we were only using developmentRepository.getCurrentDevelopment().
                    ///########################################################
                    /// look for development id in route.
                    ///########################################################
                    var $route = $injector.get('$route');
                    if ($route && $route.current && $route.current.params && $route.current.params.developmentId) {
                        config.headers['X-Bazinga-DevelopmentId'] = $route.current.params.developmentId;
                    } else {
                        ///########################################################
                        // If development id is not defined in route.
                        ///########################################################
                        // We have to use to $injector, otherwise will get error: 
                        // Uncaught Error: [$injector:cdep] Circular dependency found: developmentRepository <- $http
                        ///########################################################
                        ////////////var developmentRepository = $injector.get('developmentRepository');
                        ////////////developmentRepository.getCurrentDevelopment().then(function (currentDevelopment) {
                        ////////////    if (currentDevelopment && currentDevelopment.id) {
                        ////////////        config.headers['X-Bazinga-DevelopmentId'] = currentDevelopment.id;
                        ////////////    }
                        ////////////});
                    }

                    return config || $q.when(config);
                }
            };
            return interceptorInstance;
        }];

    $httpProvider.interceptors.push(headerInterceptor);
}]);