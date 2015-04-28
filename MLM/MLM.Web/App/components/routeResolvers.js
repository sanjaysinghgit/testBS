var Resolve = mlm.controller('Resolve', [function () {

    //----------------------------------------------------------------------
    // Member variables
    //----------------------------------------------------------------------

    //must have for every route
    this.agentTree = {};
    this.currentUser = {};
    this.currentUserRoles = {};

    //----------------------------------------------------------------------
    // Member functions
    //----------------------------------------------------------------------
}]);

Resolve.getAgentTree = ['$q',
                          '$rootScope',
                          'profileCache',
                          'agentRepository',
                          function ($q,
                                    $rootScope,
                                    profileCache,
                                    agentRepository) {

                              var deferred = $q.defer();

                              // Array of dependent promises that must resolve before we can continue
                              var dependencies = [this.currentUser];

                              // Wait for dependencies to resolve
                              $q.all(dependencies).then(function(values) {

                                  if (!values[0]) {
                                      console.log('Missing user data.');
                                      return;
                                  }
                                  
                                  var currentUserCode = values[0].AgentInfo.Code;

                                  console.log("Current user code: " + currentUserCode);
                                  agentRepository.getAgentTree(currentUserCode).then(function (agentData) {
                                            console.log("in  tree: " + agentData)
                                              if (agentData) {
                                                  deferred.resolve(agentData);
                                              }
                                          }, function (error) {
                                              deferred.reject(error);
                                              console.log("error in agent CTRL: " + error)
                                          });

                              },
                                function (error) {
                                    deferred.reject(error);
                                });
                              // Set the promise to the member variable to be accessed by other
                              // functions that have it as a dependency
                              this.agentTree = deferred.promise;
                              return deferred.promise;
                          }];



Resolve.getCurrentUser = ['$q',
                          '$rootScope',
                          'profileCache',
                          'agentRepository',
                          function ($q,
                                    $rootScope,
                                    profileCache,
                                    agentRepository) {

                              var deferred = $q.defer();
                              console.log("in resolve");
                              var User =
                                     profileCache.get(profileCache.kKeyCurProfile)

                              if (User) {
                                  deferred.resolve(User);
                              }
                              else {

                                  agentRepository.getCurrentUser().then(function (agentData) {
                                      profileCache.put(profileCache.kKeyCurProfile, agentData);
                                      if (agentData) {
                                          deferred.resolve(agentData);
                                      }
                                  }, function (error) {
                                      deferred.reject(error);
                                      console.log("error in agent CTRL: " + error)
                                  });

                              }
                              // Set the promise to the member variable to be accessed by other
                              // functions that have it as a dependency
                              this.currentUser = deferred.promise;
                              return deferred.promise;
                          }];


Resolve.getCurrentUserRoles = ['$q',
                          '$rootScope',
                          'profileCache',
                          'agentRepository',
                          function ($q,
                                    $rootScope,
                                    profileCache,
                                    agentRepository) {

                              var deferred = $q.defer();
                              //console.log("in resolve getCurrentUserRoles");
                              var User =
                                     profileCache.get(profileCache.kKeyCurRoles)

                              if (User) {
                                  deferred.resolve(User);
                              }
                              else {

                                  agentRepository.getCurrentUserRoles().then(function (agentData) {
                                      profileCache.put(profileCache.kKeyCurRoles, agentData);
                                      if (agentData) {
                                          deferred.resolve(agentData);
                                      }
                                  }, function (error) {
                                      deferred.reject(error);
                                      console.log("error in getCurrentUserRoles: " + error)
                                  });

                              }
                              // Set the promise to the member variable to be accessed by other
                              // functions that have it as a dependency
                              this.currentUserRoles = deferred.promise;
                              return deferred.promise;
                          }];


