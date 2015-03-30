"use strict";

mlm.directive('uservoiceFeedback', ['$rootScope', '$route', 'localization', 'userVoiceService', function ($rootScope, $route, localization, userVoiceService) {
    return {
        restrict: 'E',
        templateUrl: '/App/components/shared/partial/uservoiceFeedback.html',
        scope: {},
        replace: true,
        controller: function($scope){
            $scope.contactEmail = $route.current.locals.currentUser.contactEmail;
            $scope.developmentId = $route.current.params.developmentId;
        },
        link: function ($scope, element, attrs) {
            $scope.t = localization.t;
            var uv = document.createElement('script');
            uv.type = 'text/javascript';
            uv.async = true;
            uv.src = '//widget.uservoice.com/7UldpaLFlsIwfNDc6k6Rg.js';
            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(uv, s);
            
            //Show feedback widget 
            $scope.showWidget = function () {
                UserVoice = window.UserVoice || [];

                if (!$scope.ssoToken) {
                    userVoiceService.getToken($scope.developmentId).then(
                        function (tokenObject) {
                            $scope.ssoToken = tokenObject.token;
                            UserVoice.push(["setSSO", $scope.ssoToken]);
                            $scope.openWidget();
                        },
                        function () {
                            // go to feedback page without logging
                        });
                }
                else {
                    $scope.openWidget();
                }
            };
            $scope.openWidget = function () {
                UserVoice.push(['show'], {
                    position: 'top',
                    target: '#btnUserVoice',
                    email: $scope.contactEmail // doesn't work actually, may be work in the future
                });
            };
        }
    }


}]);