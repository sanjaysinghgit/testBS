'use strict'

/**
 * This directive manages communication between sidemenus and any
 * sidemenu controls that target them.
 *
 * You ask: Why have this when there is only one sidemenu?
 * Answer: Because the specs down the road call for two sidemenus 
 * that have the same behavior. One on the left and one on the right.
 * We thought it would serve us well to abstract this functionality 
 * out and allow for any number of controls to be registered for 
 * toggling those side menus.
 */
mlm.directive('sidemenuManager', [ function() {
      	return {
      		restrict: 'A',
      		controller: function ($scope) {

      			this.sidemenus = {};

      			/**
      			 * Execute the toggle command on the sidemenu
      			 * targetted
      			 * @param  {[string]} sidemenuName - the name of the sidemenu to toggle
      			 */
      			this.toggle = function (sidemenuName) {
      				this.sidemenus[sidemenuName].toggleMenu();
      			}

      			/**
      			 * [register description]
      			 * @param  {[type]} 	sidemenuName - the name to register the sidemenu under
      			 * @param  {[object]} 	sidemenu - the sidemenu object itself
      			 */
      			this.register = function (sidemenuName, sidemenuScope) {
      				this.sidemenus[sidemenuName] = sidemenuScope;
      			}

      		}
      	}
  	}]);