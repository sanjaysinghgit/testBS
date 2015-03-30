'use strict';
//Use this directive when ngMaxlength directive is not working i.e. for textArea
mlm.directive('bzMaxlength', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            var maxlength = Number(attrs.bzMaxlength);
            function fromUser(text) {
                if (text && text.length && text.length > maxlength) {
                    var transformedInput = text.substring(0, maxlength);
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                    return transformedInput;
                }
                return text;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});