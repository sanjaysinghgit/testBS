mlm.config(
[   '$provide', 
    '$httpProvider', 
    '$routeProvider', 
    '$locationProvider', 
    '$sceProvider', 
    '$sceDelegateProvider', 
    '$compileProvider', 
function (
    $provide, 
    $httpProvider, 
    $routeProvider, 
    $locationProvider, 
    $sceProvider, 
    $sceDelegateProvider, 
    $compileProvider) 
{

        var defaultDependencies = {
            currentUser: Resolve.getCurrentUser,
            currentUserRoles: Resolve.getCurrentUserRoles,
        };

        var agentDependencies = {
            currentUser: Resolve.getCurrentUser,
            currentUserRoles: Resolve.getCurrentUserRoles,
            agentTree: Resolve.getAgentTree,

        };


        $locationProvider.html5Mode({
            enabled: true,
            requireBase: true
        });
        
        $sceDelegateProvider.resourceUrlWhitelist([
            'self',
            'http:\/\/.*\.url.com/*',
            'http:\/\/my-image**',
            'http:\/\/url/img**'
        ]);
        
        $compileProvider.imgSrcSanitizationWhitelist(/^\s*(https?|http?|blob):/);
        
        $provide.decorator("$q", ['$rootScope', '$delegate', function ($rootScope, $delegate) {
            var defer = $delegate.defer;
            $delegate.defer = function() {
                var deferred = defer();
                deferred.promise.noRedirectOn404 = function () {
                    $rootScope.redirectOn404 = false;
                    return deferred.promise;
                };
                deferred.promise.noRedirectOnAnyError = function () {
                    $rootScope.redirectOnAnyError = false;
                    return deferred.promise;
                };
                return deferred;
            };
            return $delegate;
        }]);

    $routeProvider
            // Home page
            .when('/', {
                templateUrl: Url.resolveLocal('components/shared/partial/main.html'),
                controller: 'mainCtrl',
                resolve: defaultDependencies,
                cache: true
            })
            // Business Plan
            .when('/auth/businessPlan', {
                templateUrl: Url.resolveLocal('components/shared/partial/bplan.html'),
                controller: 'bPlanCtrl'
            })
            .when('/auth/agent/agentsList', {
                templateUrl: Url.resolveLocal('components/agent/partial/agentList.html'),
                controller: 'agentCtrl',
                //resolve: agentDependencies,
                //navSection: "weeklySection",
                //permissionContext: 'home',
                //cache: true
            })
            .when('/auth/agent/Tree/Binary', {
                templateUrl: Url.resolveLocal('components/agent/partial/agentBTree.html'),
                controller: 'agentBTreeCtrl',
                resolve: agentDependencies,
                cache: true
            })
            .when('/auth/agent/Tree/Generation', {
                templateUrl: Url.resolveLocal('components/agent/partial/agentGTree.html'),
                controller: 'agentGTreeCtrl',
                resolve: agentDependencies,
                cache: true
            })
        .when('/auth/agent/Details', {
            templateUrl: Url.resolveLocal('components/agent/partial/agentDetails.html'),
            controller: 'agentDetailsCtrl',
            resolve: agentDependencies,
            cache: true
        })
        ///////////////////// Payout routes /////////////////
        .when('/auth/payout/list', {
            templateUrl: Url.resolveLocal('components/payout/partial/payoutList.html'),
            controller: 'payoutCtrl',
            //resolve: agentDependencies,
            //cache: true
        })


        /////////////////////////////////
        // Handle incoming email links //
        /////////////////////////////////

        .when('/auth/fromEmail', {
            //provide an empty template. all we are doing here is redirecting
            template: '<span></span>', 
            controller: FromEmailCtrl
        });
                
    $routeProvider.otherwise({ redirectTo: '/' });
}]);
