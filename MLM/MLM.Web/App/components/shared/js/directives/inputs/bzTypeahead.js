'use strict';

mlm.directive('bzTypeahead', 
[   '$timeout', 
    '$compile',
    '$rootScope', 
function (
    $timeout, 
    $compile,
    $rootScope) 
{
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            model:                  '=ngModel',
            matches:                '=bzTypeaheadMatches',
            updateMatches:          '&bzTypeaheadUpdateMatches',
            noMatchesText:          '=bzTypeaheadNoMatchesText',
            findingMatchesText:     '=bzTypeaheadFindingMatchesText',
            select:                 '&bzTypeaheadOnSelect',
            attribute:              '@bzTypeaheadAttribute',
            //optional params
            isFocused:              '=?bzTypeaheadIsFocused',
            showAllMatchesOnFocus:  '=?bzTypeaheadShowAllMatchesOnFocus'
        },
        link: function (scope, el, attrs, ctrl) {

            /**------------------------------------------------------
             * Set defaults
             ------------------------------------------------------*/

            var isFocused   = false,
                minLength   = 1,
                hot_keys    = {
                    up:     38,
                    down:   40,
                    tab:    9,
                    enter:  13,
                    esc:    27
                }

            // assign some important scope values from the parser
            scope.activeIndex = null;
            scope.maxResults = 8;
            scope.loading = false;

            /**------------------------------------------------------
             * Connect to ngModel - workaround so that 
             * it is still accessible with our isolate scope
             * http://stackoverflow.com/questions/15269737/why-is-ngmodel-setviewvalue-not-working-from/15272359#15272359
             ------------------------------------------------------*/

            // Bring in changes from outside: 
            scope.$watch('model', function () {
                scope.$eval(attrs.ngModel + ' = model');
            });

            // Send out changes from inside: 
            scope.$watch(attrs.ngModel, function (val) {
                scope.model = val;
            });


            /**------------------------------------------------------
             * Watch for value changes
             ------------------------------------------------------*/

            scope.$watch('model', function (newVal, oldVal) {
                if (scope.isFocused) scope.getMatches();
            });

            /**------------------------------------------------------
             * UI Bindings
             ------------------------------------------------------*/

            scope.modelIsValid = function() {
                return ctrl.$dirty 
                    && scope.model.length >= minLength;
            }

            var focus_handler = function () {
                scope.isFocused = true;
                if (scope.modelIsValid()) {
                    scope.showMatches();
                }
                if (scope.showAllMatchesOnFocus) {
                    scope.model = "";
                    scope.getMatches();
                }

                /* FIXME: refactor these UI bindings so that they leverage
                * Angular's built-in ui-events and are bound in the template's
                * DOM, so that we don't have to use safeApply. 
                * 
                * In this case, we are using safeApply so that the peoplePillbox 
                * directive doesn't cause a $digest in progress error when it 
                * autofocuses the input.
                */

                $rootScope.safeApply();
            }
            

            var blur_handler = function () {
                scope.isFocused = false;
                if (!_.isUndefined(attrs.bzTypeaheadStrict)) {
                    scope.model = '';
                }
                if (getMatchesTimeout) {
                    $timeout.cancel(getMatchesTimeout);
                }
                scope.hideMatches();
                scope.$apply();
            }
            

            /**
             * Handle Hot Keys
             */
            var hot_key_handler = function (evt) {

                switch (evt.keyCode) {
                    case hot_keys.up:
                        evt.preventDefault();
                        scope.prevItem();
                        break;
                    case hot_keys.down:
                        evt.preventDefault();
                        scope.nextItem();
                        break;
                    case hot_keys.enter:
                        evt.preventDefault();
                        scope.selectMatch();
                        break;
                    case hot_keys.esc:
                        el.blur();
                        break;
                }
                scope.$apply();
            }

            // bind event handlers
            $(el).bind('blur', blur_handler);
            $(el).bind('focus', focus_handler);
            $(el).bind('keydown', hot_key_handler);

            // unbind event handlers
            scope.$on('$destroy', function () {
                $(el).unbind('keydown', hot_key_handler);
                $(el).unbind('blur',    blur_handler);
                $(el).unbind('focus',   focus_handler);
            });

            /**------------------------------------------------------
             * Match Results Element
             ------------------------------------------------------*/

            // construct the element
            var matchesElement = angular.element(
                '<bz-typeahead-matches></bz-typeahead-matches>'
            );

            // apply scope values
            matchesElement.attr({
                loading: 'loading',
                matches: 'matches',
                'no-matches-text': 'noMatchesText',
                'finding-matches-text': 'findingMatchesText',
                attribute: 'attribute',
                'active-index': 'activeIndex',
                'select-match': 'selectMatch()',
                'max-results':  'maxResults'
            });

            // register custom item template
            if (angular.isDefined(attrs.bzTypeaheadTemplateUrl)) {
                matchesElement.attr('template-url', attrs.bzTypeaheadTemplateUrl);
            }

            // compile the matches and inject them into the page
            el.after($compile(matchesElement.css('display','none'))(scope));

            /**------------------------------------------------------
             * Match Results Behaviours
             ------------------------------------------------------*/

            /**
             * Gets the matches bazed on the current model value but is throttled
             * to do so only once every 300ms.
             */
            var getMatchesTimeout; 
            var actuallyRequestMatches = function() {
                scope.loading = true;                        
                scope.matches = [];
                scope.showMatches();
                if (!scope.model || 0 === scope.model.length) {
                    scope.maxResults = 25;
                }
                scope.updateMatches({
                    typeaheadInput: scope.model || ''
                });
            }

            scope.getMatches = function () {

                if (getMatchesTimeout) {
                    $timeout.cancel(getMatchesTimeout);
                }

                // when the show all on focus is true and the model is 
                // empty, we should request the matches "immediately"
                // ... we actually wait 1 millisecond just in case 
                // some changes to the query needed to propogate 
                if (scope.showAllMatchesOnFocus && scope.model == '') {
                    getMatchesTimeout = $timeout(actuallyRequestMatches, 1);
                } 
                // if the model has something in it, lets throttle for 
                // 300 seconds in case the user types more
                else if (scope.modelIsValid()) {
                    getMatchesTimeout = $timeout(actuallyRequestMatches, 300);
                } 
                // if all else fails, hide the matches
                else {
                    scope.hideMatches();
                }
            }

            scope.$on('bz-typeahead:loaded', function () {
                scope.loading = false;
            })

            scope.$on('bz-typeahead:updateList', function () {
                scope.getMatches();
            })

            /**
             * Shows the matches by making the matches element visible.
             */
            scope.showMatches = function () {
                scope.activeIndex = 0;
                matchesElement.show()
            }

            /**
             * Hides the matches by making the matches element not visible, and 
             * also resets the list of matches and the active index.
             */
            scope.hideMatches = function () {
                scope.activeIndex = null;
                matchesElement.hide();
            }

            /**------------------------------------------------------
             * UI Behaviours
             ------------------------------------------------------*/

            /**
             * Selects the next item in the list of matches by incrementing the 
             * active index.
             */
            scope.nextItem = function () {
                if (    scope.activeIndex < scope.matches.length-1 
                    &&  scope.activeIndex < scope.maxResults-1) 
                {
                    scope.activeIndex++;
                }
            }

            /**
             * Selects the previous item in the list of matches by decrementing the 
             * active index.
             */
            scope.prevItem = function () {
                if (scope.activeIndex > 0) {
                    scope.activeIndex--;
                }
            }

        	/**
             * Selects a match, then hides the matches and resets the state of the 
             * typeahead input's controller. It selects the match by calling the 
             * select expression that was passed into this scope. 
             *
             * By default, the match will be selected by the currently active index
             * unless an overriding index is passed as an optional param. 
             * 
             * @param  {integer} idx - an index of the match position in the matches array.
             */
        	scope.selectMatch = function (idx) {
                // grab the match
                var idx = idx || scope.activeIndex;
                var match = scope.matches[idx];

                // break if there is no match
                if (_.isUndefined(match)) return;

            	// update the model/view value
                if (!_.isUndefined(attrs.bzTypeaheadResetOnSelect)) {
                    scope.model = '';
                } else {
                    scope.model = scope.$eval('matches['+idx+'].'+scope.attribute);
                }

                if (!scope.showAllMatchesOnFocus) {
                    scope.hideMatches();
                } else {
                    scope.getMatches();
                }

                // reset the ngModelController state
                ctrl.$setPristine();

                // call the scope function and pass it the matchindex
                // and actual match object
                scope.select({
                    match: match
                });
            }

        }
    };
}])

.directive('bzTypeaheadMatches', 
[   '$route', 
    'localization', 
function (
    $route, 
    localization) 
{
    return {
        restrict:'E',
        replace: true,
        scope:{
            matches:    '=',
            activeIndex:'=',
            loading:    '=',
            attribute:  '=',
            maxResults: '=',
            templateUrl:'@',
            selectMatch: '&',
            noMatchesText: '=?noMatchesText',
            findingMatchesText: '=?findingMatchesText'
        },
        templateUrl: '/App/components/shared/partial/bzTypeaheadMatches.html',
        link: function (scope, element, attrs, bzTypeahead) {

            // if noMatchesText is not supplied, use default, else use supplied
            if (_.isUndefined(scope.noMatchesText)) {
                scope.noMatchesTextToDisplay = localization.t('bzTypeahead.noMatches');
            } else {
                scope.noMatchesTextToDisplay = scope.noMatchesText;
            }

            // if findingMatchesText is not supplied, use default
            if (_.isUndefined(scope.findingMatchesText)) {
                scope.findingMatchesTextToDisplay = localization.t('bzTypeahead.findingMatches');
            } else {
                scope.findingMatchesTextToDisplay = scope.findingMatchesText;
            }

            scope.setActive = function (matchIdx) {
                scope.activeIndex = matchIdx;
            };

            scope.isActive = function (matchIdx) {
                return scope.activeIndex == matchIdx;
            };
        }
    };
}])

.directive('bzTypeaheadMatch', 
[   '$http', 
    '$templateCache', 
    '$compile', 
    '$parse', 
function (
    $http, 
    $templateCache, 
    $compile, 
    $parse) 
{
    return {
        restrict: 'E',
        replace: true,
        link: function (scope, element, attrs) {
            var defaultTemplateUrl = '/App/components/shared/partial/bzTypeaheadMatch.html';
            // calling scope.$parent, because this is on an ng-repeat that
            // isolates the scope
            var customTemplateUrl = scope.$parent.templateUrl;
            var tplUrl = customTemplateUrl || defaultTemplateUrl;
            $http.get(tplUrl, { cache: $templateCache })
                .success(function (tplContent) {
                    element.replaceWith($compile(tplContent.trim())(scope));
                });

            scope.label = scope.$eval('match.' + scope.$parent.attribute);
        }
    }
}])
