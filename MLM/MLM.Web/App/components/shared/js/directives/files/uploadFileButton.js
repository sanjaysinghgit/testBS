'use strict';
/// This directive is used when you need to provide only button for file upload.

mlm.directive('uploadFileButton', ['$rootScope', function ($rootScope) {
    return {
        restrict: 'A',
        scope: {
            todo: '&',
            submit: '@'
        },
        link: function (scope, element, attrs) {
            var inputElement = $('#' + attrs.id);
            
            inputElement.on('change',
                function (value) {
                    scope.todo();
                });

            scope.$watch('submit', function (value) {
            
                if (value == 'true') {
                    attrs.$set('disabled', 'disabled');
                }
                else {
                    element.removeAttr('disabled');
                }});
            
           
        }
    }
}]);
