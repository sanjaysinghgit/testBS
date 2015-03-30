mlm.service('adminImportRepository',
['$resource',
    '$q',
    'bLog',
function ($resource,
            $q,
            bLog) {

    var buildingResource = $resource(
        Url.resolve('developments/:developmentId/buildings/'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        }
    );
    var unitsResource = $resource(
        Url.resolve('developments/:developmentId/units/'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var rolesResource = $resource(
        Url.resolve('developments/:developmentId/roles/'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var tagsResource = $resource(
        Url.resolve('developments/:developmentId/tags/'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var amenitiesResource = $resource(
        Url.resolve('developments/:developmentId/amenities/'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var developmentcustomfieldsResource = $resource(
        Url.resolve('developments/:developmentId/developmentcustomfields'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var floorplansResource = $resource(
        Url.resolve('developments/:developmentId/floorplans'),
        { developmentId: '@Id' }, {
            get: { method: 'GET', isArray: false }
        });
    var PackageResource = $resource(
        Url.resolve('developments/:developmentId/packages'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var keyfobResource = $resource(
        Url.resolve('developments/:developmentId/keyfobs'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var EntrycodesResource = $resource(
        Url.resolve('developments/:developmentId/entrycodes'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var ParkingStallResource = $resource(
        Url.resolve('developments/:developmentId/parkingstalls'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var BikeStallResource = $resource(
        Url.resolve('developments/:developmentId/bikestalls'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var StorageLockersResource = $resource(
        Url.resolve('developments/:developmentId/storagelockers'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var UnitCustomFieldsResource = $resource(
        Url.resolve('developments/:developmentId/unitcustomfields'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var UserTagsResource = $resource(
        Url.resolve('developments/:developmentId/usertags'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var ConciergeTemplatesResource = $resource(
        Url.resolve('developments/:developmentId/conciergetemplates'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var InvitesResource = $resource(
        Url.resolve('developments/:developmentId/people'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var InvitesFeedBackResource = $resource(
        Url.resolve('developments/:developmentId/Invitations'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
        var permissionsResource = $resource(
        Url.resolve('permissions/:developmentId'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var rolepermissionsResource = $resource(
        Url.resolve('claimtypepermissions/:developmentId'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false },
        });
    var rolespermissionsResource = $resource(
        Url.resolve('claimtypepermissions/:developmentId'),
        { developmentId: '@Id' },
        {
            put: { method: 'PUT', isArray: false }
        });
    var unitswithpackagesResource = $resource(
        Url.resolve('unitswithassetspackage/:developmentId'),
        { developmentId: '@Id' },
        {
            get: { method: 'GET', isArray: false }
        });
    var unitswithfloorplansResource = $resource(
       Url.resolve('unitswithfloorplans/:developmentId'),
       { developmentId: '@Id' },
       {
           get: { method: 'GET', isArray: false }
       });
    var batchresponseResource = $resource(
     Url.resolve('/developmentbatch/:developmentId'),
     { developmentId: '@Id' },
     {
         get: { method: 'GET', isArray: false }
     });
    var sendInvitations = $resource(Url.resolve('commands/onboarding/invite-all'),
       { developmentId: '@developmentId', 'invitationText': '@invitationText' }, {
           post: { method: 'POST', isArray: false }
       });

    return {
        getBuildings: function (developmentId) {
            var deferred = $q.defer();
            buildingResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getUnits: function (developmentId) {
            var deferred = $q.defer();
            unitsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getRoles: function (developmentId) {
            var deferred = $q.defer();
            rolesResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getTags: function (developmentId) {
            var deferred = $q.defer();
            tagsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getAmenities: function (developmentId) {
            var deferred = $q.defer();
            amenitiesResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getDevelopmentCustomFields: function (developmentId) {
            var deferred = $q.defer();
            developmentcustomfieldsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getFloorPlans: function (developmentId) {
            var deferred = $q.defer();
            floorplansResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getPackages: function (developmentId) {
            var deferred = $q.defer();
            PackageResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getKeyFobs: function (developmentId) {
            var deferred = $q.defer();
            keyfobResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getEntryCodes: function (developmentId) {
            var deferred = $q.defer();
            EntrycodesResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getParkingstalls: function (developmentId) {
            var deferred = $q.defer();
            ParkingStallResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getBikeStalls: function (developmentId) {
            var deferred = $q.defer();
            BikeStallResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getStorageLockers: function (developmentId) {
            var deferred = $q.defer();
            StorageLockersResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getUnitCustomFields: function (developmentId) {
            var deferred = $q.defer();
            UnitCustomFieldsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getUserTags: function (developmentId) {
            var deferred = $q.defer();
            UserTagsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getConciergeTemplates: function (developmentId) {
            var deferred = $q.defer();
            ConciergeTemplatesResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getInvites: function (developmentId) {
            var deferred = $q.defer();
            InvitesResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getInvitesFeedBack: function (developmentId) {
            var deferred = $q.defer();
            InvitesFeedBackResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getpermissions: function (developmentId) {
            var deferred = $q.defer();
            permissionsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        getrolepermissions: function (developmentId) {
            var deferred = $q.defer();
            rolepermissionsResource.get({ developmentId: developmentId },
                function (buildingsData) {
                    deferred.resolve(buildingsData);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        },
        saveRolePermissionSettings: function (Id, d) {
            var deferred = $q.defer();
            rolespermissionsResource.put({ developmentId: Id }, d,
                function (res) {
                    deferred.resolve(res);
                }, function (error) {
                    deferred.reject(error);
                });
            return deferred.promise;
        },
        getUnitsWithPackages: function (developmentId,apiparameter) {
            var deferred = $q.defer();
            unitswithpackagesResource.get({ developmentId: developmentId}, function (buildingsData) {
                deferred.resolve(buildingsData);
            }, function (error) {


                deferred.reject(error);
                bLog.serverError(error, error);
            });
            return deferred.promise;
        },
        getUnitsWithFloorPlan: function (developmentId) {
            var deferred = $q.defer();
            unitswithfloorplansResource.get({ developmentId: developmentId }, function (buildingsData) {
                deferred.resolve(buildingsData);
            }, function (error) {
                deferred.reject(error);
                bLog.serverError(error, error);
            });
            return deferred.promise;
        },
        getBatchResponse: function (developmentId) {
            var deferred = $q.defer();
            batchresponseResource.get({ developmentId: developmentId }, function (buildingsData) {
                deferred.resolve(buildingsData);
            }, function (error) {
                deferred.reject(error);
                bLog.serverError(error, error);
            });
            return deferred.promise;
        },
        sendInvitations: function (developmentId,invitationText) {
               var deferred = $q.defer();
               sendInvitations.post({ developmentId: developmentId, invitationText: invitationText },
               {},
                function (res) {
                    deferred.resolve(res);

                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
            return deferred.promise;
        }
    };
}]);