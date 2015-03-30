'use strict';
mlm.service('importService', ['$http', '$q', '$resource', 'authCache', '$rootScope', function ($http, $q, $resource, authCache, $rootScope) {
    var developmentResource = $resource(Url.resolve('/exceptiondetails/:developmentId'), { developmentId: '@Id' }, {
        post: { method: 'POST', isArray: false }
    });
    var continueimportedResource = $resource(Url.resolve('/commands/onboarding/batch-import?:apiParameter&action=import&filePattern=*.xlsx'),
    { developmentId: '@developmentId', 'invitationText': '@invitationText', apiParameter: '@apiParameter' }, {
        post: { method: 'POST', isArray: false }
    });
    var count = 0;
    var self = this;
    self.developmentId = 0;
    self.scope = null;
    self.upload = function (developmentId, imgdataurl, controllerScope, callbackSuccess, callbackError) {
        var requestUrl = "/data/commands/onboarding/batch-upload?action=import&filePattern=*.xlsx&developmentId=" + developmentId + "&" + null + "&" + "invitationText=" + controllerScope.invitationText;
        var headers = {}, authHeader;

        ///########################################################
        /// Injecting Authorization Token in header
        ///########################################################
        var authorizationToken = authCache.get("AuthorizationToken");
        if (authorizationToken) {
            if (headers['AuthorizationToken'] == undefined && count == 0) {
                headers['AuthorizationToken'] = authorizationToken;
                count = count + 1;
            }
        }
        if ($rootScope.isExpotStarted != undefined && $rootScope.isExpotStarted == true) {
            if (authorizationToken) {
                if (headers['AuthorizationToken'] == undefined) {
                    headers['AuthorizationToken'] = authorizationToken;
                    $rootScope.isExpotStarted = false;
                }
            }
        }
        authHeader = jQuery.extend({}, headers, { 'X-Bazinga-DevelopmentId': developmentId });

        var ajaxOptions = {
            url: requestUrl,
            method: 'POST',
            secureuri: false,
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (ajaxFileUploadData, status) {
                callbackSuccess(ajaxFileUploadData);
            },
            error: function (ajaxFileUploadData, status, e) {
                callbackError();
            },
        };

        $.ajaxFileUpload(jQuery.extend({}, ajaxOptions, {
            headers: authHeader
        }));

        //$.ajaxFileUpload
        //        ({
        //            headers: headers,
        //            url: requestUrl,
        //            method: 'POST',
        //            secureuri: false,
        //            dataType: 'json',
        //            beforeSend: function () {
        //                //   $("#loading").show();
        //            },
        //            complete: function () {
        //                //  $("#loading").hide();
        //            },
        //            success: function (ajaxFileUploadData, status) {
        //                callbackSuccess(ajaxFileUploadData);
        //            },
        //            error: function (ajaxFileUploadData, status, e) {
        //                callbackError();
        //            }
        //        });
    };
    self.initService = function (controllerScope, parentImageObject) {
        self.scope = controllerScope;
        self.imageObject = parentImageObject;
        self.imageUrlChanged = false;
        self.updateImage();
    };
    self.importDevelopment = function (objectScope, postExportEvent, postErrorEvent) {
        var deferred = $q.defer();
        self.upload(objectScope.developmentId, objectScope.file.imgdataurl, objectScope,
            function (selfFileUploadedData) {
                if (selfFileUploadedData.errorCode == 4002) {
                    objectScope.$apply(postErrorEvent(selfFileUploadedData));
                } else {
                    objectScope.$apply(postExportEvent(selfFileUploadedData));
                }
            },
            function (selfFileUploadedData) {
                // objectScope.isExportError = true;
                objectScope.$apply();
            });
    },
   self.sentException = function (d) {
       var deferred = $q.defer();
       developmentResource.save({ id: d.developmentId }, d,
           function (res) {
               deferred.resolve(res);
           }, function (error) {
               deferred.reject(error);
               bLog.serverError(error, error);
           });
       return deferred.promise;
   },
     self.continueImportedFile = function (objectScope) {
         var apiParameter = objectScope.apiParameter;
         var deferred = $q.defer();
         continueimportedResource.post({ developmentId: objectScope.developmentId, invitationText: objectScope.invitationText, apiParameter: objectScope.apiParameter },
            {},
             function (res) {
                 deferred.resolve(res);

             }, function (error) {
                 deferred.reject(error);
                 bLog.serverError(error, error);
             });
         return deferred.promise;
     }
}]);