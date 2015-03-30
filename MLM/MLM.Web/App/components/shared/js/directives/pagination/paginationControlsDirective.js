mlm.directive('paginationControls', 
    [   '$rootScope', 
        '$document', 
        '$timeout',
        'localization', 
    function (
        $rootScope, 
        $document, 
        $timeout,
        localization) {
    return {
        restrict:       'E',
        replace:        true,
        templateUrl:    '/App/components/shared/partial/paginationControls.html', 
        scope: {
            paginator:  '='
        },
        link: function (scope, element, attr) {

            // map the translate function into this isolated scope
            scope.t = localization.t;

            /**
             * Handles the left and right arrow keys within this
             * view to control next/prev page
             * @param  {[type]} e [description]
             * @return {[type]}   [description]
             */
            var handleKeyboardShortcuts = function (e) {    

                switch (e.keyCode) {
                    case 37:
                        if (scope.paginator.searching) return;
                        scope.paginator.prevPage();
                        scope.$apply();
                        break;
                    case 39:
                        if (scope.paginator.searching) return;
                        scope.paginator.nextPage();
                        scope.$apply();
                        break;
                }
                
            }
            $document.bind('keyup', handleKeyboardShortcuts);

            // clean up the DOM
            scope.$on('$destroy', function() {
                $document.unbind('keyup', handleKeyboardShortcuts);                
            });

        }
    }
}]);