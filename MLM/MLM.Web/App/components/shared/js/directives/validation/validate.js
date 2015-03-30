/**
 * Piggy-backs on the bazinga form controller to give us more control over 
 * when we want to show / hide the error messages on a field, and concatenates 
 * the string of error messages together for us.
 */
mlm.directive('bzValidate', ['$compile', '$rootScope', 'localization', function ($compile, $rootScope, localization) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attr, ctrl) {

            // when the user enters the input field mark it as focused
            $(element).bind('focus', function () {
                ctrl.bzFocused = true;
                //scope.$apply();
            });

            // when the user leaves the input field mark it as visited and unfocused
            $(element).bind('blur', function () {
                ctrl.bzVisited = true;
                ctrl.bzFocused = false;
                //scope.$apply();
            });

            scope.$watch(
                // watch for changes in the validity of the input
                function() { return ctrl.$invalid },
                // update the error msg and show status
                function() {
                    formatErrors();
                    shouldShowError();
                }
            );

            scope.$watch(
                // watch for changes in the visited status
                function() { return ctrl.bzVisited },
                // update the error show status
                function() {
                    formatErrors();
                    shouldShowError();
                }
            );

            // a map to the error messages file
            // FIXME: move this out of here and into the views.
            var errorMsgs = {
                minlength: 'invalidLength',
                maxlength: 'invalidLength',
                required: 'emptyField',
                match: 'noMatch',
                nomatch: 'sameAsOld',
            };

            // make translation function available to this meeting
            scope.t = localization.t;

            /**
             * Concatenates all the error messages into a 
             * single string.
             */
            var formatErrors = function() {

                ctrl.bzErrorMsg = '';
                _.each(ctrl.$error, function(val, key, index) {
                    if (!val) return;
                    ctrl.bzErrorMsg += localization.t('errors.' + errorMsgs[key]) + ' ';
                });

            };

            /**
             * Determines whether the error should be visible
             */
            var shouldShowError = function () {

                if (ctrl.bzVisited && ctrl.$dirty && ctrl.$invalid) {
                    ctrl.bzShowError = true;
                } else {
                    ctrl.bzShowError = false;
                }

            };
        }
    };
}]);