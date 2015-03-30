mlm.service('batchRepository', ['$resource', '$q', 'bLog',
    function ($resource, $q, bLog) {
        var self = this;

        var batchResource = $resource(Url.resolve('batches/:batchId'), { batchId: '@Id' }, {
            get: { method: 'GET', isArray: false }
      });
        var InvitationDuplicateEmailsResource = $resource(Url.resolve('feedback/:batchId/duplicateemails'), { batchId: '@Id' }, {
            get: { method: 'GET', isArray: false }
        });
        var InvitationExistingEmailsResource = $resource(Url.resolve('feedback/:batchId/existinguseremails'), { batchId: '@Id' }, {
            get: { method: 'GET', isArray: false }
        });

        return {
            getBatchById: function (id) {
                var deferred = $q.defer();
                batchResource.get({ batchId: id }, function (batchData) {
                    deferred.resolve(batchData);
                }, function (error) {
                    //alert(error.data.errorCode);
                    deferred.reject(error);
                });
                return deferred.promise;
            },
            getInvitationDuplicateEmails: function (id) {
            var deferred = $q.defer();
            InvitationDuplicateEmailsResource.get({ batchId: id }, function (batchData) {
                deferred.resolve(batchData);
            }, function (error) {
                deferred.reject(error);
                bLog.serverError(error, error);
            });
            return deferred.promise;
            },
            getInvitationExistingEmails: function (id) {
            var deferred = $q.defer();
            InvitationExistingEmailsResource.get({ batchId: id }, function (batchData) {
                deferred.resolve(batchData);
            }, function (error) {
                deferred.reject(error);
                bLog.serverError(error, error);
            });
            return deferred.promise;
        }

        };
    }]);