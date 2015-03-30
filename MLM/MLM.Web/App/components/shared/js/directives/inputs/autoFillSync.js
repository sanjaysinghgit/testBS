/*
When submitting a form in AngularJS and use the browser remember password functionality, 
and in a subsequent login attempt you let the browser fill in 
the login form with the username and password,
the $scope model won't be changed based on the autofill.

This is a known issue with Angular and is currently open
Issue link: https://github.com/angular/angular.js/issues/1460

Use solution provided in @blesh answer
http://stackoverflow.com/questions/14965968/angularjs-browser-autofill-workaround-by-using-a-directive
*/

'use strict';

mlm.directive('autoFillSync', ['$timeout', function ($timeout) {
    return {
        require: 'ngModel',
        link: function(scope, elem, attrs, ngModel) {
            var origVal = elem.val();
            $timeout(function() {
                var newVal = elem.val();
                if (ngModel.$pristine && origVal !== newVal) {
                    ngModel.$setViewValue(newVal);
                }
            }, 500);
        }
    };
}]);