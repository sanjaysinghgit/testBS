mlm.directive('bzDateTimePeriods', ['localization', function (localization) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.periods = [
             {
                 "id": "hours",
                 "name": localization.t('datetime.periods.hours')
             },
             {
                 "id": "days",
                 "name": localization.t('datetime.periods.days')
             },
             {
                 "id": "months",
                 "name": localization.t('datetime.periods.months')
             }
            ];
        }
    };
}]);