'use strict'

mlm.directive('bzTimeSpanControl', ['localization', function (localization) {

    return {
        restrict: 'E',
        template: '<div class="timespan">' +
          '<input type="text" class="input-small" ng-model="number" />' + 
          '<div class="bz-select bz-select-small">' +
          '<select bz-booking-units ng-model="units" ng-options="item.id as item.name for item in bookingUnits">' +
          '</select></div> ',
        replace: true,
        scope: {
            timespan: '='
        },
        link: function (scope, element, attrs) {

            scope.units = '';
            scope.number = -1;
            scope.element = element;
            scope.$watch('timespan', function () {
                if (scope.timespan != undefined) {
                    extractNumberFromAdvanceBooking(scope.timespan);
                   // scope.$apply(function () {
                        scope.element.find("ul").find("li").each(function (index, li) {
                            if ($(li).onclick == null) {
                                $(li).on("click", updateTimeSpan);
                            }
                        });
                  //  });
                }
            });

            scope.$watch('number', function () {
                if (scope.number != -1) {
                    updateTimeSpan();
                }
            });

            scope.$watch('units', function () {
                if (scope.number != -1) {
                    updateTimeSpan();
                }
            });

         
            function updateTimeSpan() {
                if (scope.units == 'days') {
                    scope.timespan = scope.number < 10 ?
                        '0' + scope.number + '.00:00:00' :
                        scope.number.toString() + '.00:00:00';
                }
                else if (scope.units == 'hours') {
                    scope.timespan = scope.number < 10 ?
                        '0.00:0' + scope.number + ':00' :
                        '0.00:' + scope.number + ':00';
                }
            }

            function extractNumberFromAdvanceBooking(val) {
                var number = 0;
                var units = "";
                if (val != undefined) {
                    try {
                        var parts = val.split(':');
                        for (var i = 0; i < parts.length; i++) {
                            var num = Number(parts[i]);
                            if (num > 0) {
                                number = num;
                                break;
                            }
                        }
                        switch (i) {
                            case 0:
                                units = localization.t('datetime.periods.days');
                                break;
                            case 1:
                                units = localization.t('datetime.periods.hours');
                                break;
                            default:
                                units = '';
                                break;
                        }
                        scope.number = number;
                        scope.units = units;
                    }
                    catch (error) {
                    }
                }

            }
        }
    }
}]);
