'use strict';
mlm.directive('peoplePillboxTypeahead',
['$rootScope',
    '$route',
    '$q',
    '$location',
    '$timeout',
    'peopleService',
    'peopleListQuery',
    'profileRepository',
    'permissionsRepository',
    'localization',
function (
    $rootScope,
    $route,
    $q,
    $location,
    $timeout,
    peopleService,
    peopleListQuery,
    profileRepository,
    permissionsRepository,
    localization) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            source: '=',
            submitting: "=",
            excludeList: "=",
            isFocused: "=?",
            addRecipients: "=?",
            placeholder:'@',
            showAllMatchesOnFocus: "=?",
            noMatchesText: "=",
            findingMatchesText: "="
        },
        templateUrl: '/App/components/shared/partial/peoplePillboxTypeahead.html',
        link: function (scope, element, attrs) {

            scope.t = localization.t;
           

            // are we only allowed to choose one person for this instance?
            attrs.single === "" ? scope.single = true : scope.single = false;

            var loggedInUser = $route.current.locals.currentUser;
            var developmentId = $route.current.params.developmentId;
            var can_message_residents
                        = permissionsRepository.validateClaim('can_message_residents');

            var can_read_all_member_info
                        = permissionsRepository.validateClaim('can_read_all_member_info');

            // To exclude residents from the typeahead results we can 
            // add the exclude-residents attribute. otherwise, including residents
            // will depend on the permissions for the user.
            scope.includeResidents
                = permissionsRepository.validateClaim('can_message_residents');

            var excludeResidents = function () {
                if (!_.isUndefined(attrs.excludeResidents)) {
                    return true;
                } else {
                    return scope.includeResidents ? false : true;
                }
            }();

            if (!_.isUndefined(attrs.autofocus)) {
                $("#typeaheadInput").focus();
            }

            /**
             * Bind Hot Keys
             */
            var hot_keys = {
                del: 46,
                backspace: 8
            };

            scope.onKeydown = function (evt) {
                if (// the current input is empty
                    (!scope.new_value || scope.new_value.length < 1)
                        &&
                    // the delete key was hit
                        (evt.keyCode == hot_keys.del || evt.keyCode == hot_keys.backspace)) {
                    // delete the last user from the list
                    scope.source.pop();
                    scope.$broadcast('bz-typeahead:updateList');
                }
            };

            scope.getPeople = function (matchString) {

                // collect the ids of people to exclude from the list 
                var peopleIdsToExclude = _.pluck(_.union(scope.source, scope.excludeList), 'id');
                if (attrs.excludeSelf) {
                    peopleIdsToExclude.push(loggedInUser.id);
                }

                var queryMap = {
                    textFields: ['name/first', 'name/last'],
                    exclude: ['id']
                };

                if (can_read_all_member_info) {
                    queryMap.textFields.push('units/number');
                }

                var peopleListQueryTop = scope.showAllMatchesOnFocus && 0 === matchString.length ? 25 : 8;

                var query = new peopleListQuery({
                    top: peopleListQueryTop,
                    exclude: [peopleIdsToExclude],
                    queryTextTokensStartsWith: matchString
                });

                if (!can_message_residents || excludeResidents) {
                    query.exclude.push(['resident']);
                    queryMap.exclude.push('userRoles/role/type');
                }
               
                if (!_.isUndefined(attrs.excludeUnclaimedUsers)) {
                    query.filter = query.filter || [];
                    queryMap.filter = queryMap.filter || [];
                    query.filter.push('1');
                    queryMap.filter.push('profileType');
                }
                
                peopleService
                    .getList(developmentId, query, queryMap)
                    .then(function (pillboxPeopleListData) {
                 //       console.log('Looking... matchString = ' + pillboxPeopleListData.matchString + '; currentValue = ' + scope.new_value);
                        if (pillboxPeopleListData.matchString == scope.new_value) {
                            scope.people = pillboxPeopleListData.items;
                            scope.$broadcast('bz-typeahead:loaded');
                        }
                    });
            };

            // This adds the new tag to the tags array
            scope.add = function (person) {
                scope.source.push(person);
            };

            // This is the ng-click handler to remove an item
            scope.remove = function (idx) {
                scope.source.splice(idx, 1);
            };

        }
    };
}]);
