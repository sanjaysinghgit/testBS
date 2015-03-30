mlm.directive('shortRoleFormatter', ['$route', 'localization', 'permissionsRepository',
    function ($route, localization, permissionsRepository) {
        return {
            restrict: 'E',
            templateUrl: '/App/components/shared/partial/shortRole.html',
            scope: {
                person: '='
            },
            replace: true,
            link: function (scope, element, attr) {

                var personInfo = scope.person;

                var role = personInfo.userRoles ? personInfo.userRoles[0] : null;

                /**
                * Buildings are resolved on every route, atm
                * TODO: make the cache object available directly to 
                * directives, so that this sort of logic is handled more 
                * gracefully
                */

                try {
                    var buildingCount = $route.current.locals && $route.current.locals.buildings ? $route.current.locals.buildings.length : 0;
                } catch (e) {
                   /**
                   * Eat this exception. Errors here seem to be caused by slower systems, 
                   * when the link function is called multiple times (during $digest),
                   * $route.current.locals may be momentarily undefined throwing an exception
                   * and crashing the app. FIXME: better understand and prevent this situation.
                   */
                }


                // should this be a link?
                if (attr.link != undefined) {
                    scope.link = true;
                } else {
                    scope.link = false;
                }

                scope.profileLink = "/developments/" + $route.current.params.developmentId + "/users/" + personInfo.id;

                // if the person has no roles
                if (!personInfo.userRoles || personInfo.userRoles.length === 0) {
                    var formerCommunityMemberText = localization.t('people.former_community_member');
                    if (personInfo.name) {
                        scope.roleName = personInfo.name.first + ' ' + personInfo.name.last;
                        scope.roleTitle = formerCommunityMemberText;
                    } else {
                        scope.roleName = formerCommunityMemberText;
                        scope.roleTitle = "";
                    }
                    scope.roleContext = "";
                    scope.removedUser = true;
                    scope.link = false;
                    return;
                }

                // if this is a unit based role
                if (role.role.requiredContextType === 'unit') {
                    var roleDisplayName = permissionsRepository.validateClaim("can_read_all_member_info")
                        ? role.role.displayName
                        : localization.t('canonical_roles.' + role.role.type);
                    
                    scope.roleName = (personInfo.name) ? personInfo.name.first + ' ' + personInfo.name.last : roleDisplayName;
                    scope.roleTitle = role.context.number || "";
                    scope.roleContext = buildingCount < 2 ? "" : role.context.building.displayName;
                }
                    // if it is a development based role
                else {
                    if (personInfo.name) {
                        scope.roleName = personInfo.name.first + ' ' + personInfo.name.last;
                        scope.roleTitle = role.displayName ? role.displayName : role.role.displayName;
                    } else {
                        scope.roleName = role.displayName ? role.displayName : role.role.displayName;
                        scope.roleTitle = "";
                    }

                    scope.roleContext = "";
                }

            }
        }
    }]);