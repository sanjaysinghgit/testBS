'use strict';
/**
 * A directive that evaluates whether this input's value matches 
 * the value of the input[name] provided.
 *
 * Huge props to this custom validation blog post:
 * http://www.benlesh.com/2012/12/angular-js-custom-validation-via.html
 * 
 * @param  {[bzMatch]} the name of the input to match against
 * @return {[type]}
 */
mlm.directive('bzMatch', function () {
    return {

        restrict: 'A',

        require: 'ngModel',

        // ctrl = the controller for ngModel
        link: function (scope, element, attrs, ctrl) {

            // the input to match against
            var inputToMatch = $('input[name='+attrs.bzMatch+']');

            // add a parser that will process each time the value is 
            // parsed into the model when the user updates it.
            ctrl.$parsers.unshift(function(value) {

                // test and set the validity after update.
                var valid = inputToMatch.val() == value;
                ctrl.$setValidity('match', valid);
                
                // if it's valid, return the value to the model, 
                // otherwise return undefined.
                return valid ? value : undefined;

            });

            // add a formatter that will process each time the value 
            // is updated on the DOM element.
            ctrl.$formatters.unshift(function(value) {
                // validate.
                ctrl.$setValidity('match', inputToMatch.val() == value);
                
                // return the value or nothing will be written to the DOM.
                return value;
            });

        }
    };
});

/**
 * The opposite of bzMatch - identifies when two fields are unique
 */
mlm.directive('bzNoMatch', function () {
    return {

        restrict: 'A',

        require: 'ngModel',

        // ctrl = the controller for ngModel
        link: function (scope, element, attrs, ctrl) {

            // the input to match against
            var inputToMatch = $('input[name='+attrs.bzNoMatch+']');

            // add a parser that will process each time the value is 
            // parsed into the model when the user updates it.
            ctrl.$parsers.unshift(function(value) {

                // test and set the validity after update.
                var valid = inputToMatch.val() != value;
                ctrl.$setValidity('nomatch', valid);
                
                // if it's valid, return the value to the model, 
                // otherwise return undefined.
                return valid ? value : undefined;

            });

            // add a formatter that will process each time the value 
            // is updated on the DOM element.
            ctrl.$formatters.unshift(function(value) {
                // validate.
                ctrl.$setValidity('nomatch', inputToMatch.val() != value);
                
                // return the value or nothing will be written to the DOM.
                return value;
            });

        }
    };
});