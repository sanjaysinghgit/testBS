'use strict';
/**
 * A directive that set focus to element.
 */
mlm.directive('bzFocus', ['$timeout', function ($timeout) {
    return {
        link: function (scope, element, attrs, controller) {
            $timeout(function () {
                element[0].focus();
            });
        }
    };
}]);

/**
 * A directive that set focus to element. On a parameter. 
 * If parameter is true it set focus.
 * Also on blur its sets parameters variable to false.
  */
mlm.directive('bzFocusMe', ['$timeout', '$parse', function ($timeout, $parse) {
    return {
        link: function (scope, element, attrs) {
            var model = $parse(attrs.bzFocusMe);
            scope.$watch(model, function (value) {
                if (value === true) {
                    $timeout(function () {
                        element[0].focus();
                    });
                }
            });

            element.bind('blur', function () {
                scope.$apply(model.assign(scope, false));
            });
        }
    };
}]);