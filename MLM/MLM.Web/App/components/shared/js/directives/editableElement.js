mlm.directive('editable', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var checkEditMode = function () {
                if (scope.editMode) {
                    attrs.editable == 'view' ? element.hide() : element.show();
                }
                else {
                    attrs.editable == 'edit' ? element.hide() : element.show();
                }
            };
            if (scope.editMode == undefined) {
                scope.editMode = false;
            }
            checkEditMode();

            scope.$on('toggleEditMode', function (e, editMode) {
                checkEditMode();
            });
        }
    }
});

mlm.directive('editSwitch', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            // scope[scope.action];
            var eventName = 'click';

            if (attrs.type == 'submit') {
                eventName = 'goAhead';
                scope.$on(eventName, function () {
                    toggleMode();
                })
            };
            element.on(eventName, function () {
                toggleMode();
            });
            function toggleMode() {
                if (attrs.editSwitch == 'both') {
                    if (scope.editMode == undefined) {
                        scope.editMode = false;
                    }
                    scope.editMode = !scope.editMode;
                }
                else if (attrs.editSwitch == 'on') {
                    scope.editMode = true;
                }
                else if (attrs.editSwitch == 'off') {
                    scope.editMode = false;
                }
                if (scope.editMode) {
                    scope.copyData = {};
                    scope.$apply(angular.copy(scope.editableData, scope.copyData));
                }
                scope.$broadcast('toggleEditMode', scope.editMode);
            }
        }
    }
});

mlm.directive('editCancel', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            if (attrs.editCancel != undefined) {
                scope.copyObject = JSON.parse(attrs.editCancel);
            }
            element.on('click', function () {
                scope.$apply(angular.copy(scope.copyData, scope.editableData));
                scope.$broadcast("formCanceled");
            });
        }
    }
});


mlm.directive('editableToggle',['$rootScope', '$compile', function ($rootScope, $compile) {
    var setViewModeElement = function (scope, element, attrs) {
        element.empty();
        var viewContent = $("<" + attrs.viewtag + ">");
        viewContent = $("<div>");
        var c = angular.element("<div>zxcc</div>");
        element.append(c);
    }
           
    return {
        restrict: "E",
        scope: true,
        transclude: true,
        compile: function (tElement, tAttrs, transclude) {
            
            return function (scope, element, attrs) {
                var c = angular.element("<div>zxcc</div>");
                element.append(c);
            }
        }
    };
}]);

