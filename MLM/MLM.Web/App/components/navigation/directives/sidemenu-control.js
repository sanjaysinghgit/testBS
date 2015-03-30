'use strict'

mlm.directive('sidemenuControl', [function() {
      	return {
      		restrict: 'A',
          require: '^sidemenuManager',
      		link: function($scope, elm, attrs, sidemenuManager) {
      			elm.on('click', function () {
      				sidemenuManager.toggle(attrs.sidemenuControl);
      			});
	    	  }
      	}
  	}]);