'use strict';

mlm.service('developmentImageService', ['$http', '$q', '$resource', function ($http, $q, $resource) {
    var self = this;
    self.scope = null;

    self.DevelopmentImageResource = $resource(Url.resolve('objects/:objectId/relationships'), { objectId: '@Id' }, {
        update: { method: 'POST', isArray: false }
    });

    self.upload = function (requestUrl, controllerScope, callbackSuccess, callbackError) {
        $.ajaxFileUpload
                ({
                    url: requestUrl,
                    method: 'POST',
                    secureuri: false,
                    dataType: 'json',
                    beforeSend: function () {
                        //   $("#loading").show();
                    },
                    complete: function () {
                        //  $("#loading").hide();
                    },
                    success: function (ajaxFileUploadData, status) {
                        callbackSuccess(ajaxFileUploadData);
                    },
                    error: function (ajaxFileUploadData, status, e) {
                        callbackError();
                    }
                });
    };
    //relationship between development image and developmentid
    self.createDevelopmentImageRelation = function (uploadedImageID, developmentID) {
        //self.DevelopmentImageResource.update({ objectId: uploadedImageID });
        var target = { target: { id: uploadedImageID } };
        var deferred = $q.defer();
        self.DevelopmentImageResource.update({ objectId: developmentID }, target, function (result) {
            deferred.resolve(null);
        }, function (result) {
            deferred.reject(null);
        });
        return deferred.promise;
    };
    self.importNewDevelopmentImage = function (objectScope, postExportEvent) {
        var deferred = $q.defer();
        var requestUrl = "/data/InsertDevelopmentImage/" + objectScope.developmentID;
        self.upload(requestUrl, objectScope,
            function (developmentImageUploadData) {
                self.createDevelopmentImageRelation(developmentImageUploadData.items[0].id, objectScope.developmentID).then(function () {
                });
                //objectScope.$apply(postExportEvent(developmentImageUploadData.id));
                deferred.resolve(developmentImageUploadData);
            },
            function (developmentImageUploadData) {
                objectScope.$apply();
                deferred.reject(null);
            });
        return deferred.promise;
    };


}]);