mlm.directive('permission', ['$location',
                                 '$rootScope',
                                 '$timeout',
                                 '$route',
                                 'permissionsRepository',
                                 function ($location,
                                           $rootScope,
                                           $timeout,
                                           $route,
                                           permissionsRepository) {
    return {
        restrict: 'A',
        link: function (scope, el, attrs) {

            scope.$on("permissions:reset", function () {
                checkPermissions(attrs.permission);
            });

            scope.$on("$routeChangeSuccess", function () {
                checkPermissions(attrs.permission);
            });
            
            // hide: explicitly set display none
            // if we use .hide(), .show() fails to 
            // restore display: table-cell items properly

            if (attrs.permission === "") {
                /*
                    permissions will = "" when the permission directive is applied to an ng-repeat, 
                    where only one item in the repeated list is given a premission claim
                    In this case we just return;
                */
                el.css('display', ''); // show: remove inline display value
                return;
            }

            checkPermissions();
            
            function checkPermissions() {

                if (!_.isUndefined($route.current.locals) &&
                !_.isUndefined($route.current.locals.permissions)) {

                    var permissions = $route.current.locals.permissions;

                    var result = permissionsRepository.validateClaim(attrs.permission);
                    if (result) {
                        el.css('display', '');
                    }
                    else {
                        el.css('display', 'none');
                        el.remove();
                    }
                } else {
                    el.css('display', 'none');
                }
            }
        }
    };
}]);
