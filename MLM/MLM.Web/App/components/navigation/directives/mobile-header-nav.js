'use strict';

mlm.directive('mobileHeaderNav', ['$location', function ($location) {
      return {
          restrict: 'E',
          replace: true,
          templateUrl: '/App/components/navigation/partials/mobile-header-nav.html',
          link: function (scope, elm, attrs) {

              // elm.find('.toggle-bzng-nav').on('click', function () {
                  // $('.sidemenu').toggleClass('sidemenu--open');
              // });

          }
      }
  }]);
