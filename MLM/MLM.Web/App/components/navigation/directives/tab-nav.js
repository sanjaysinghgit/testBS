mlm.directive('tabNav', ['$rootScope', '$route', '$location', 'invitationService', 'invitationListQuery', 'localization', function ($rootScope, $route, $location, invitationService, invitationListQuery, localization) {
      return {
          restrict: 'E',
          templateUrl: "/App/components/navigation/partials/tab-nav.html",
          replace: true,
          // because there could be more than one instance on a page
          scope: true,
          link: function (scope, elem, attrs) {

              var developmentId = $route.current.params.developmentId;

              scope.getInvitesCount = function() {
                  var query = angular.copy(new invitationListQuery($location.search()));
                  query.queryText = null;
                  query.filter = null;
                  query.top = 0;
                  query.skip = 0;
                  invitationService.getCount(developmentId, query, { textFields: ['name/first', 'name/last', 'emailid', 'role'], filter: ["role/type"] })
                      .then(function (count) {
                      scope.invitesCount = count.count;
                  }).then(function() {
                      scope.setLinks();
                  });
              };
             
              scope.setLinks = function() {
                  if (attrs.navfor === "community") {
                      var invitesTitle = localization.t('people.invitations');
                      // if (scope.invitesCount > 0) {
                      //     invitesTitle = invitesTitle + " ({0})".format(scope.invitesCount);
                      // }
                      
                      scope.links = [
                          { title: localization.t('people.people'), link: "/developments/{0}/community/people".format(developmentId) },
                          { title: invitesTitle, link: "/developments/{0}/community/invitations".format(developmentId), permission: "can_manage_profiles" }
                      ];
                  }

                          
                  if (attrs.navfor === "concierge") {
                      scope.links = [
                          // no links for this section 
                          { title: localization.t('concierge.report') },
                          { title: localization.t('concierge.request') }
                      ];
                  }

                  if (_.some(scope.links, 'link')) {
                      // sets the active link based on the url
                      for (var i = 0; i < scope.links.length; i++) {
                          if (scope.links[i].link == $location.path()) {
                              scope.links[i].active = true;
                          }
                      }
                  } else {
                      // the active link is set by clicking
                      scope.links[0].active = true;
                      
                      scope.setActive = function(index) {
                          _.each(scope.links, function(link) {
                              link.active = false;
                          });
                          scope.links[index].active = true;
                          $rootScope.$broadcast('tabs:change', scope.links[index].title);
                      };

                  }
              };

              //Set initial 
              scope.setLinks();
              //Only for community
              // if (attrs.navfor === "community") {
              //     scope.getInvitesCount();
              // }

              // scope.$on('inviteUpdated', function () {
              //     scope.getInvitesCount();
              // });
          }
      };
  }]);
