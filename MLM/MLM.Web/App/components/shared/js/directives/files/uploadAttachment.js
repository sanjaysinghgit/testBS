"use strict";

/// This directive is for cases when you need to create an attachment to message, or post, or whatever.

mlm.directive('uploadAttachment', ['$rootScope', 'localization', function ($rootScope, localization) {
    return {
        restrict: 'E',
        templateUrl: '/App/components/shared/partial/attachFile.html',
        scope: {
            todo: '&',
            submit: '@',
            fileName: '='
        },
        replace: true,
        link: function (scope, element, attrs) {
            var inputElement = $('[type=file]');
            scope.submitting = false;
            scope.fileName = '';
            scope.fileSelected = false;
            scope.t = localization.t;
            scope.submitting = false;

            inputElement.on('change',
                function (value) {
                    if (value.target.files && value.target.files.length > 0) {

                        if (value.target.files[0].name.length > 64) {
                            alert(localization.t('errors.fileNameIsTooLong'));
                            scope.clearFile();
                            return;
                        }

                        if (value.target.files[0].size > 10 * 1000000) {
                            alert(localization.t('errors.fileTooLarge'));
                            scope.clearFile();
                            return;
                        }

                        if (!$rootScope.checkFileType(value.target.files[0].type)) {
                            alert(localization.t('errors.unsupportedFileType'));
                            scope.clearFile();
                            return;
                        }

                        scope.$apply(function () {
                            scope.fileName = value.target.files[0].name;
                            scope.fileSelected = true;
                        });
                    }
                });

            scope.$on('submitDone', function () {

                scope.fileName = '';
                scope.fileSelected = false;
                scope.submitting = false;
                scope.clearFile();

            });

            scope.clearFile = function () {
                var inputElement = $('[type=file]');
                if (inputElement) {
                    inputElement.val('');
                }
            };

            scope.$on('submitStart', function () {
                scope.submitting = true;
            });

            scope.$watch('submit', function (value) {

                if (value == 'true') {
                    attrs.$set('disabled', 'disabled');
                }
                else {
                    element.removeAttr('disabled');
                }

            });

            scope.cancelFile = function () {
                if (!scope.submitting) {

                    scope.fileName = '';
                    scope.fileSelected = false;
                    var inputElement = $('[type=file]');
                    if (inputElement) {
                        inputElement.val('');
                    }

                }
            }
        }
    }

}]);

