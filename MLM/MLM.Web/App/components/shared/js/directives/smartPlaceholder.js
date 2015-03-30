mlm.directive('smartPlaceholder', [function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (!_.isUndefined(attr.smartPlaceholder) && attr.smartPlaceholder != '') {
                $(element).attr('placeholder', attr.smartPlaceholder);
            }
        }
    };
}]);