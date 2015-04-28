'use strict';

//DEFINE GLOBAL VARS FOR EACH MODULE
/////////////////////////////////////////////
var deficiency_module = angular.module('product', []);
var errorHandler = angular.module('errorHandler', []);
var loggingModule = angular.module('logger', []);



//MAIN MODULE
/////////////////////////////////////////////
   
var mlm = angular.module('mlm',
        [      
         'ngResource',
         'ngRoute',
         'ngAnimate',
         'ngCookies',
         'ngSanitize',
         'ui.grid',
         'angular-cache',
         'product',
         'errorHandler',
         'logger'
        ]
     );

mlm.run(['$rootScope',
    '$http',
    '$location',
    '$route',
    '$timeout',
    'bLog',
    'LoginService',
    'profileCache',
    'cacheManager',
    '$sce',
    'localization',
    function (scope,
        $http,
        $location,
        $route,
        $timeout,
        bLog,
        LoginService,
        profileCache,
        cacheManager,
        $sce,
        localization

        ) {

        console.log("in mlm run");
        cacheManager.clearApplicationCache().then(function () {
            profileCache.put(profileCache.kKeyCurProfile, null);
        });

        

        scope.redirectOn404 = true;
        scope.redirectOnAnyError = true;
        // logout method on the rootScope 
        // so we can call logout from any page in the app ...
        scope.logout = LoginService.logout;
        scope.isHandsetDevice = (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(navigator.userAgent.toLowerCase())) == true ? true : false;
        
        // show page loading spinner
        scope.$on("$routeChangeStart", function (event, next, current) {
            scope.changingPages = true;
            var currentPath = current;
            var nextPath = next;

            console.log('Starting to leave %s to go to %s', currentPath, nextPath);
            // TODO: WE MUST find a better way of authorizing routes! This is VERY insecure.
            // Stop people from visiting the profile page of unregistered users directly.
            if (next && next.$$route && next.$$route.stopVisitsIfUnregisteredProfileAndUserHasNoPermissions) {

                var userHasPermissions = true;
                //var development = developmentCache.get('currentDevelopment');
                //var permissions = permissionsCache.get(development.id);

                switch (next.$$route.stopVisitsIfUnregisteredProfileAndUserHasNoPermissions.page) {
                    case "profileEdit":
                        userHasPermissions = permissionsRepository.validateClaim("can_manage_profiles", permissions)
                    case "amenityEdit":
                        userHasPermissions = permissionsRepository.validateClaim("can_manage_tickets", permissions);
                }
               
                //var isARegisteredProfile = _.contains(_.pluck(current.locals.currentUser.userRoles, "status"), "active");

                if (!userHasPermissions) {
                    event.preventDefault();
                    //$location.path("/notfound");
                    console.log("TODO: set not found page here 1");
                }
            }
            // ---------------------------
            
        });

        scope.$on("$locationChangeStart", function (event, next, current) {
            console.log('Starting to leave %s to go to %s', current, next);
            scope.backUrl = current.replace(/^(?:\/\/|[^\/]+)*\//, "");
            //if (_gaq) {
            //    var evtSuccess = _gaq.push(['_trackPageview', next]);
            //    Logger.warn('GA _trackPageview ' + next);
            //}
        });

        scope.$on("$routeChangeSuccess", function (event, next, current) {

            // add an attibute to the body to proivde css hook
            var pathClass = $location.path().replace(/\//g, "-");
            $('body').attr('data-bloc', pathClass.substr(1, pathClass.length - 1));
    
            if (next && next.$$route) {
                // determine the correct section and subsection for the nav ...
                scope.currentSection = next.$$route ? next.$$route.navSection : current.$$route.navSection;
                scope.subSection = next.$$route ? next.$$route.subTemplate : current.$$route.subTemplate;
                scope.activeSubSection = next.$$route ? next.$$route.navSubSection : current.$$route.navSubSection;
            }

            //// count developments; if there are more than one then show the development picker
            //if (!_.isUndefined($route.current.locals.developments)) {
            //    scope.developmentCount = $route.current.locals.developments.length;
            //}

            //if ($route.current.locals.development) {
            //    scope.developmentName = $route.current.locals.development.displayName;
            //}
        
            // hide the changing pages spinner.
            scope.changingPages = false;

        });



        // generic event that we can use to redirect to the login page
        //scope.$on('event:loginRequired', function (event, args) {
        //    scope.layout = "login";
        //    $location.path('/login');
        //});

    ///////////////////////////////////////////////////////////////////////////////////////////
    //UTILITIES
    ///////////////////////////////////////////////////////////////////////////////////////////

    
    scope.trustedHtml = function (html) {
        var nhtml = $sce.trustAsHtml(html);
        return nhtml;
    }

    scope.util = util;

    scope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };

    scope.isMobileDevice = scope.isMobileDevice == undefined ? (function () {
        var check = false;
        (function (a, b) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true })(navigator.userAgent || navigator.vendor || window.opera);
        return check;})()
     : scope.isMobileDevice;

    scope.showErrorMessage = function (message) {
        scope.$broadcast("pageLoadErrorEvent", { message: message });
    };

    scope.checkFileType = function (fileType) {
        if (fileType) {
            return fileType.match('image.*') ||
            fileType.match('application/pdf') ||
            fileType.match('text/plain') ||
            fileType.match('application/word') ||
            fileType.match('application/msword') ||
            fileType.match('application/vnd.ms-excel') ||
            fileType.match('application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') ||
            fileType.match('application/ppt') ||
            fileType.match('application/vnd.ms-powerpoint') ||
            fileType.match('application/vnd.openxmlformats-officedocument.presentationml.presentation') ||
            fileType.match('application/vnd.openxmlformats-officedocument.wordprocessingml.document');
        }
        else {
            return true;
        }
    };

    scope.Constants = {
        Loop: {
            MessageCreated: "MessageCreated",
            MessageCreationFailed: "MessageCreationFailed"
        },
        Profile: {
            PhoneContactMethod: "phone"
        }
    };

        customizeTimeStamps();

}]);

function customizeTimeStamps()
{
    moment.lang('en', {
        relativeTime: {
            future: "in %s",
            past: "%s",
            s: "just now",
            m: "1m",
            mm: "%dm",
            h: "1h",
            hh: "%dh",
            d: "1",
            dd: "%dd",
            M: "a month",
            MM: "%d months",
            y: "a year",
            yy: "%d years"
        }
    });
}

function createMediaQuery(widthType, breakpointName){
//    width type can be either max or min
    var widthElement = widthType.toLowerCase().concat('-width');

//    breakpoint can be 'handset', 'tablet' or 'desktop'
    var handsetBreakpoint = '560px';
    var tabletBreakpoint = '768px';
    var desktopBreakpoint = '1030px';

    var breakpoint = '0px';

    switch (breakpointName){
        case 'handset':
            breakpoint = handsetBreakpoint;
            break;
        case 'tablet':
            breakpoint = tabletBreakpoint;
            break;
        case 'desktop':
            breakpoint = desktopBreakpoint;
            break;
        default:
            break;
    }

    var mediaQuery = '(' + widthElement + ': ' + breakpoint + ')';
    return mediaQuery;

}









