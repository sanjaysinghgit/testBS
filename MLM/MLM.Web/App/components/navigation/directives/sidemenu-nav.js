'use strict';

mlm.directive('sidemenu', 
[   '$timeout', 
    '$location', 
    '$route', 
    'localization',
    'permissionsRepository',
    'development',
function (
    $timeout, 
    $location, 
    $route, 
    localization,
    permissionsRepository,
    development)
{

return {
    restrict: 'E',
    replace: true,
    require: '^sidemenuManager',
    templateUrl: '/App/components/navigation/partials/sidemenu-nav.html',
   
    link: function (scope, elm, attrs, sidemenuManager) {
        
        // register the sidemenu with the manager          
        sidemenuManager.register(attrs.name, scope);
        scope.t = localization.t;

        /**
         *  Scope functions
         */
        
        scope.onDocumentClick = function (e) {
            var targetParents = $(e.target).parents();
            // if the menu wasn't the target, or a parent of the target
            if (e.target != elm[0] && !$(targetParents).is(elm)) {
                e.preventDefault();
                scope.closeMenu();    
            }
        };

        scope.closeMenu = function () {
            elm.removeClass('sidemenu--open');
            $('body').unbind('click touchstart', scope.onDocumentClick);
        };

        scope.openMenu = function () {
            elm.addClass('sidemenu--open');
            // wait so that we don't immediately fire forceClose
            $timeout(function () { 
                $('body').bind('click touchstart', scope.onDocumentClick) 
            }, 50);
        };

        scope.toggleMenu = function () {
            if (elm.hasClass('sidemenu--open')) {
                scope.closeMenu();
            } else {
                scope.openMenu();
            }
        };

        scope.navClicked = function (section) {
            scope.currentSection = section;
            $location.search('');
            scope.closeMenu();
        }

        scope.isManager = function(){
            return permissionsRepository.validateClaim("can_manage_tickets");
        }

        scope.reloadNav = function (unitArg) {

            if (_.isUndefined($route.current.$$route)) return;

            scope.currentSection = $route.current.$$route.navSection;
         
            try {
                scope.user = $route.current.locals.currentUser;
                scope.development = $route.current.locals.development;
                scope.developmentId = scope.development.id;
                scope.amenities = { count: $route.current.locals.amenityListData.items.length }
            } catch (err) {
                console.log('Current User is not yet ready:'+ err);
            }            

            if (unitArg) {
                scope.units = unitArg;
            } else {
                scope.units = $route.current.locals.unitData ? 
                                $route.current.locals.unitData.units : "";
            }

            if (scope.units && scope.units.length > 0) {
                scope.myHome = true;
                scope.myHomeUrl = '/developments/' + scope.developmentId + '/myhome';
                if (scope.units.length == 1) {
                    scope.myHomeUrl += '/' + scope.units[0].id;
                }
            } else {
                scope.myHome = false;
            }
        }

        /**
         *  Handle events
         */
        
        elm.on('mouseenter', function () {
            scope.openMenu()
        });
        elm.on('mouseleave', function () {
            scope.closeMenu()
        });

           // handle route change events
        scope.$on("$routeChangeSuccess", function () {
            scope.reloadNav(); 
        });

        // get the units when permissions are reset. (emit from
        // profileController)
        scope.$on("check:units", function(e, units) {
            scope.reloadNav(units);
        });

    }

}

}]);
