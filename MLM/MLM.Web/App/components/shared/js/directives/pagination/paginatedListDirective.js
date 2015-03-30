'use strict';

mlm.directive('paginatedList', 
    [
        '$timeout',
        '$rootScope',
        'localization',
        'permissionsRepository',
    function (
        $timeout,
        $rootScope,
        localization,
        permissionsRepository
    ) {
        return {
            restrict: 'E',
            transclude: true,
            replace: true,
            templateUrl: '/App/components/shared/partial/paginatedList.html', 
            scope: {
                paginator:  '=',
                itemClass:  '@',
                externals:  '='
            },
            link: function (scope, element, attr) {

                scope.t = localization.t;
                scope.util = util;

              

                scope.showSendMessage = function (person) {
                    var send = false;
                    var hasNonResidentRoles = false;

                    person.userRoles.forEach(function (userRole) {
                        if (userRole.role.type != 'resident') {
                            hasNonResidentRoles = true;
                        }
                    });

                    if (permissionsRepository
                                .validateClaim("can_message_residents can_create_message_thread")
                        || hasNonResidentRoles) {
                        send = true;
                    } else {
                        send = false;
                    }

                    return send;
                }

                // watch the other query attrs for changes
                var timeout = {}; 
                scope.$watch(function () {
                    return [
                        scope.paginator.query.sort,
                        scope.paginator.query.filter,
                        scope.paginator.query.top,
                        scope.paginator.query.skip,
                        scope.paginator.query.queryTextTokensStartsWith,
                        scope.paginator.query.queryText
                    ];
                }, function (newValue, oldValue) {

                    var sortChanged   = newValue[0] != oldValue[0];

                    // If anything but the skip value changed
                    // back to the start of the result set
                    var newSkip       = newValue[3],
                        oldSkip       = oldValue[3],
                        skipChanged   = newValue[3] != oldValue[3];

                    // determine if the query text has changed
                    // any list will only use queryTextTokensStartsWith 
                    // *or* queryText, never both.
                    var newText       = newValue[4] || newValue[5],
                        oldText       = oldValue[4] || oldValue[5],
                        textChanged = newText != oldText;

                  

                    if (textChanged) {
                        $timeout.cancel(timeout);
                        timeout = $timeout(function () {
                          
                            scope.paginator.filterByText();
                        }, 500);
                    } else if (sortChanged) {
                        scope.paginator.sort();
                    } else {
                        // unless this is a brand new paginator, we want to 
                        // reset the skip to 0 whenever it wasn't explicitly 
                        // changed
                        if (    !scope.paginator.fresh 
                            && (!skipChanged || newSkip < 0)) {
                            scope.paginator.query.skip = 0;
                        }
                      
                        scope.paginator.refreshList();
                    }
                    
                },true);
            }
        }
    }
]);
