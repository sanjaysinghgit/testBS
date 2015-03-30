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
            agentTree: Resolve.getAgentTree,

        };

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
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
                templateUrl: Url.resolveLocal('/components/shared/partial/main.html'),
                controller: 'mainCtrl',
                resolve: defaultDependencies,
                cache: true
            })
            // Business Plan
            .when('/businessPlan', {
                templateUrl: Url.resolveLocal('/components/shared/partial/bplan.html'),
                controller: 'bPlanCtrl'
            })
            // Tree Report
            .when('/weeklySection/treeReport', {
                templateUrl: Url.resolveLocal('/components/weeklySection/partial/treeReport.html'),
                controller: 'treeReportCtrl',
                resolve: defaultDependencies,
                //navSection: "weeklySection",
                //permissionContext: 'home',
                //cache: true
            })
            .when('/agent/agentsList', {
                templateUrl: Url.resolveLocal('/components/agent/partial/agentList.html'),
                controller: 'agentCtrl',
                //resolve: defaultDependencies,
                //navSection: "weeklySection",
                //permissionContext: 'home',
                //cache: true
            })
            .when('/agent/registration', {
                templateUrl: Url.resolveLocal('/components/agent/partial/agentList.html'),
                controller: 'agentCtrl',
                //resolve: defaultDependencies,
                //navSection: "weeklySection",
                //permissionContext: 'home',
                //cache: true
            })
            //.when('/agent/Tree/:typeMode', {
            //    templateUrl: Url.resolveLocal('/components/agent/partial/agentTree.html'),
            //    controller: 'agentTreeCtrl',
            //    resolve: defaultDependencies,
            //    cache: false
            //})
            .when('/agent/Tree/Binary', {
                templateUrl: Url.resolveLocal('/components/agent/partial/agentBTree.html'),
                controller: 'agentBTreeCtrl',
                resolve: defaultDependencies,
                cache: true
            })
            .when('/agent/Tree/Generation', {
                templateUrl: Url.resolveLocal('/components/agent/partial/agentGTree.html'),
                controller: 'agentGTreeCtrl',
                resolve: defaultDependencies,
                cache: true
            })
        .when('/agent/Details', {
            templateUrl: Url.resolveLocal('/components/agent/partial/agentDetails.html'),
            controller: 'agentDetailsCtrl',
            resolve: defaultDependencies,
            cache: true
        })

        /////////////////////////////////
        // Handle incoming email links //
        /////////////////////////////////

        .when('/fromEmail', {
            //provide an empty template. all we are doing here is redirecting
            template: '<span></span>', 
            controller: FromEmailCtrl
        });
                
    $routeProvider.otherwise({ redirectTo: '/' });
}]);
