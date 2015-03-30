mlm.directive('pageErrorMessage', ['$timeout', 'localization', function ($timeout, localization) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$on('pageLoadErrorEvent', function (event, messageObject) {
                scope.errorMessage = messageObject.message || localization.t('errors.default_error_message');
                $(element).show();
                $timeout(function () {
                    $(element).hide();
                }, 5000);
            });
        }
    };
}]);