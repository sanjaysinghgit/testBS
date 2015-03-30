mlm.directive('bzListUtilities', 
	[
		'localization',
    function (
    	localization
    ) {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: '/App/components/shared/partial/bzListUtilities.html', 
            scope: {
                paginator: '=',
                actionButtonText: '=',
                action: '&',
                actionPermission: '='
            },
            link: function (scope, element, attr) {
                scope.t = localization.t;
                scope.showActionButton = !_.isUndefined(scope.actionButtonText) && !_.isUndefined(attr.action);
            }
        }
    }
]);