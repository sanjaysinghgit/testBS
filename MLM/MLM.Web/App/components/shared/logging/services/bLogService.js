loggingModule.service('bLog', ['bazingaLogServiceProvider', 'googleAnalyticsService',  function (bazingaLogServiceProvider, googleAnalyticsService) {

    function logLevelIs(level) {
        var levels = [ 'Debug', 'Info', 'Warn', 'Error', 'Fatal'];
        var currentLevel = bazingaLogServiceProvider.logLevel;
        return (levels.indexOf(currentLevel) < levels.indexOf(level));
    }


    return {
       trackUIEvent: function (type, details) {
           if (logLevelIs('Info')) {
               //bazingaLogServiceProvider.logToAzure(type, details);
           };
        },
        authException: function (type, details) {
            if (logLevelIs('Error')) {
                //bazingaLogServiceProvider.logToAzure(type, details);
            };
        },
        pageNotFound: function(type, details){
            if (logLevelIs('Error')) {
               // bazingaLogServiceProvider.logToAzure(type, details);
            };
        },
        serverError: function (type, details) {
            if (logLevelIs('Fatal')) {
               // bazingaLogServiceProvider.logToAzure(type, details);
            }
        }, 
        loginSuccess: function (type, details) {
            if (logLevelIs('Info')) {
               // bazingaLogServiceProvider.logToAzure(type, details);
            };
        },
       trackTimingEvent: function(action, label, value) {
           if (logLevelIs('Info')) {
               bazingaLogServiceProvider.logToGoogle('Timing', action, label, value);
           }
       },

       logGoogleEvent: function (googleLogObject) {
           googleAnalyticsService.log(googleLogObject.category, googleLogObject.action, googleLogObject.label, googleLogObject.value, googleLogObject.postValue);
       },

       loginEvent: function (result) {
           googleAnalyticsService.log('Authentication', 'Login', result ? 'Success': 'Fail');
       },

       bookAmenityEvent: function (result) {
           googleAnalyticsService.log('Amenities', 'Booking', result ? 'Success' : 'Fail');
       }
    };

}]);



