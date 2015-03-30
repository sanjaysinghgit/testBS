mlm.directive('autoExpanding', [function () {
    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, elem, attrs, ctrl) {

            if(!ctrl) return;

            // wrap the text area
            var container = angular.element('<div class="bz-autoexpanding"></div>');
            elem.wrap(container);
            elem.before('<pre><span></span><br></pre>');
          
            // store the mock field
            var mock = elem.parent().find('span');

            // specify how UI should be updated
            ctrl.$render = function() {
                elem.val(ctrl.$viewValue || '');
                mock.text(ctrl.$viewValue || '');
            }

            // sync
        	function sync() {
                ctrl.$setViewValue(elem.val());
                ctrl.$render();
            }

         	elem.bind('keyup', sync);
         	scope.$on('$destroy', function() {
         		elem.unbind('keyup', sync);
         	});

        }
   
    }
}]);