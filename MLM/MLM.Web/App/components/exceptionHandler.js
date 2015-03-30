mlm.factory('$exceptionHandler', ['$injector', 
    function ($injector) {
    return function (exception) {
        try {
            // IE - Angular bug that will be resolved with new release of Angular
            if (exception.stack.indexOf('interpolateFnWatchAction') >= 0) {
                return;
            }

            var location = $injector.get('$location');
            var rootScope = $injector.get('$rootScope');

            if (location.host().match(/(localhost|-dev\.)/))
                console.error(exception.message, exception.stack);

            //Logger.error(exception); 
            console.error(exception);

            if (location.search().backurl) {
                location.path($location.search().backurl);
            }
                /*
            else if (!_.isUndefined(rootScope.backUrl)) {
                location.path(rootScope.backUrl);
            }
            */
            //rootScope.showErrorMessage(localization.t('errors.serverError'));
            console.error("errors.serverError");

            rootScope.changingPages = false;
        }
        catch (ex) {

            console.log(ex.message, ex.stack);    // We should not be in the infinite loop.
        } 
    };
}]);
