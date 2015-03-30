mlm.directive('bzPaginationStatus', 
	[
		'localization',
    function (
    	localization
    ) {
        return {
            restrict:   'E',
            replace:    true,
            transclude: true,
            template:   '<span class="pagination-status">'+
                            '<span class="pagination-status-populated"'+
                                '  ng-if="!p.fresh">'+
                                '{{p.firstRecord}}-{{p.lastRecord}}'+
                                ' {{t("common.of")}} '+
                                '{{p.totalRecords}} {{p.label}}'+
                            '</span>'+
                            '<span class="pagination-status-fresh"'+
                                '  ng-if="p.fresh">'+
                                '{{t("common.finding_items").format(p.label)}}'+
                            '</span>'+
                        '</span>', 
            scope: {
            	p: 	'=paginator'
            },
            link: function (scope, element, attr) {
            	scope.t = localization.t;
            }
        }
    }
]);