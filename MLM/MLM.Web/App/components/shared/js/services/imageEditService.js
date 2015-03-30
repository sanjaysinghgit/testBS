'use strict';

mlm.service('imageEditService', ['$http', '$q', '$resource', '$rootScope', 'localization', 'postFileService',
    function ($http, $q, $resource, $rootScope, localization, postFileService) {
    var self = this;
    self.imageId = 0;
    self.scope = null;

    self.ResourceUpdateParent = $resource(Url.resolve('objects/:objectId/relationships'), { objectId: '@Id' }, {
        update: { method: 'POST', isArray: false }
    });

    self.updateImage = function (clear) {
        if (clear != undefined && clear) {
            self.scope.imageListener();
        }
        else {
            self.scope.imageListener = self.scope.$watch(function () {
                return self.imageObject.images[0].url;
            }, function (newUrl, oldUrl) {
                if (self.extractUrl(newUrl) != self.extractUrl(oldUrl)) {
                    self.scope.imageUrlChanged = true;
                    if (self.functionOnImageUpdate) {
                        self.functionOnImageUpdate();
                    }
                }
            });
        }
    };

    // Returns parameterless url from the source url
    self.extractUrl = function (url) {
        var parStart = url.indexOf('?');
        if (parStart > -1) {
            url = url.substring(0, parStart);
        }
        return url;
    };

    self.clearImageSetup = function () {
        self.updateImage(true);
    };

    self.initService = function (controllerScope, parentImageObject, functionOnImageUpdate) {
        self.scope = controllerScope;
        self.imageObject = parentImageObject;
        self.imageUrlChanged = false;
        self.functionOnImageUpdate = functionOnImageUpdate;
        self.updateImage();
    }

    self.clearupImagesList = function () {
        var deferred = $q.defer();
        if (self.imageObject.images.length > 0) {
            if (self.imageObject.images[0].id){
                postFileService.removeFile(self.imageObject.images[0].id).then(function () {
                    var tmpList = _.reject(self.imageObject.images, function (item) { return item.id == self.imageObject.images[0].id; });
                    if (tmpList.length == 0) {
                        tmpList = [{ url: "", id: "" }];
                    }
                    self.imageObject.images = tmpList;
                    self.clearupImagesList();
                    deferred.resolve(true);
                });
            }
            else {
                deferred.resolve(true);
            }
        }
        else {
            deferred.resolve(true);
        }
        return deferred.promise;
    };

    self.saveObjectWithImages = function (objectScope, formObject, parentObject, functionToSaveObject) {
        var valid = formObject != null ? formObject.$valid : true;
            if (valid) {
                if (objectScope.imageUrlChanged) {
                    if (self.imageObject.images[0].url != '') {
                        var formId = formObject != undefined ? (formObject.$name != undefined ? formObject.$name : null) : null;

                        postFileService.post(formId, function (fileUploadedData) {

                            // after upload clean up the images list
                            self.clearupImagesList().then(function () {
                                if (fileUploadedData.ErrorMessage) {
                                    if (fileUploadedData.ErrorMessage != '') {
                                        objectScope.errorMessage = localization.t('errors.imageNotSaved') + fileUploadedData.ErrorMessage;
                                    }
                                    $rootScope.safeApply(functionToSaveObject());
                                }
                                else {
                                    var resultData = fileUploadedData;
                                    postFileService.createRelationships(fileUploadedData.items[0].id, self.imageObject.id, false).then(function () {
                                        if (resultData) {
                                            if (resultData.items && resultData.items.length > 0) {
                                                objectScope.imageUrl = resultData.items[0].url;
                                            }
                                            else {
                                                objectScope.imageUrl = resultData.url;
                                            }
                                            objectScope.imageUrlChanged = true;
                                            $rootScope.safeApply(functionToSaveObject());
                                            objectScope.imageUrlChanged = false;
                                        }
                                        return;
                                    }, function (err) {
                                        // Error handler
                                    });
                                   // objectScope.$apply();
                                }

                            });

                            
                        },
                        function (error, status) {
                            // Save issue anyway
                            $rootScope.safeApply(functionToSaveObject(error));
                        });
                    }
                    else {
                        // image should be deleted
                        if (self.imageObject.images[0] && self.imageObject.images[0].id != 0) {

                            postFileService.removeFile(self.imageObject.images[0].id).then(function (fileRemovedData) {
                                $rootScope.safeApply(functionToSaveObject);
                                objectScope.imageUrlChanged = false;
                            },
                            function (fileRemovedData) {
                                functionToSaveObject();
                            });
                        }
                    }
                }
                else {
                    $rootScope.safeApply(functionToSaveObject());
                }
            }

    };
}]);