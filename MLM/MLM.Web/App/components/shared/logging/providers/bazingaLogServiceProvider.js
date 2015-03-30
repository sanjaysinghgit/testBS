loggingModule.provider('bazingaLogServiceProvider', function () {

    this.$get = ['$rootScope', '$location', '$injector', function ($rootScope, $location, $injector) {
        
        //var local = $location.host() === "localhost" ? true : false;
             
        var gaLogger = $injector.get('googleAnalyticsService');

    	this.logToGoogle = function(category, action, label,value) {
            if (type && details && !local) {
                gaLogger.log(category, action, label,value);
                return;
            }
        };

        // GOOGLE PAGEVIEW TRACKER
        $rootScope.$on('$locationChangeSuccess', function () {
            var url = $location.path();
            url ? gaLogger.trackPageView(url) : angular.noop();
        });

        return {
            logToGoogle: this.logToGoogle
        };
    }];
});
