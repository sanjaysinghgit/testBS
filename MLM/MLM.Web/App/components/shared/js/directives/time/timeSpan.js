mlm.directive('bzTimeSpan', ['localization', function (localization) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.spans = [
             {
                 "id": "00:00:00",
                 "name": localization.t('datetime.spans.none')
             },
             {
                 "id": "00:30:00",
                 "name": "30 " + localization.t('datetime.spans.minutes')
             },
             {
                  "id": "00:45:00",
                  "name": "45 " + localization.t('datetime.spans.minutes')
             },
             {
                 "id": "01:00:00",
                 "name": "1 " + localization.t('datetime.spans.hour')
             }
            ];
            for (var i = 2; i <= 24; i++) {
                var item = {
                    "id": (i < 10 ? "0" + i : i) + ":00:00",
                    "name": i.toString() + " " + localization.t('datetime.spans.hours')
                };
                scope.spans.push(item);
            }
        }
    };
}]);