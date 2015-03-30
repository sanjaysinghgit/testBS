'use strict';

mlm.service('fileUploadService', ['$rootScope', '$http', '$q', '$resource', 'postFileService', 'bLog',
    function ($rootScope, $http, $q, $resource, postFileService, bLog) {
    var self = this;
    self.imageId = 0;
    self.scope = null;

    self.doUpload = function (formId, parentObject, reverse, callBack) {
        self.parentObject = parentObject;
        var input = $("input[type=file]");
        var inputValue = input ? input.val() : '';
        self.resultUpload = null;
        if (inputValue) {
            postFileService.post(formId, function (fileUploadedData) {
                self.resultUpload = fileUploadedData;
                postFileService.createRelationships(fileUploadedData.items[0].id, self.parentObject.id, reverse).then(function () {
                    // okay
                    callBack(self.resultUpload);
                    return;
                }, function () {
                    callBack(null); //self.resultUpload); // change to return null if failed when API is ready.
                });
                $rootScope.$apply();
            }, function (obj) {
                callBack(obj); // change to return null if failed when API is ready.
            });
        }
        else {
            callBack(false);
        }
    };

    self.uploadFile = function (formId, callback) {
        var input = $("input[type=file]");
        var inputValue = input ? input.val() : '';
        self.resultUpload = null;
        if (inputValue) {
            postFileService.post(formId, function (fileUploadedData) {
                callback(fileUploadedData);
            }, function () {
                callback(arguments.length > 0 ? arguments[0] : null);
            });
        }
        else {
            callback(false);
        }
    };

    self.removeFile = function (fileId) {
        return postFileService.removeFile(fileId);
    };

    self.createEntityWithAttachment = function (formId, reverseRelationship, logObject,
        functionToCreateEntity,
        functionSucceedWhenFileUploaded, // This function may takes 2 arguments: file upload object, and entity object
        functionSucceedAnyway, // This function may take result object after creating entity.
        functionFail,
        functionToRollback, // Usually it's deleting the entity from database
        articlesForm, // This flag means that instead of using object.id we should use object.articles[0].id to create a relatioships
        functionLookForParentObjectId // If it's provided use it to look for is of object that an image should be attached to
        ) {
        self.uploadFile(formId, function (selfFileUploadedData) {
            self.fileData = selfFileUploadedData;
            if (selfFileUploadedData == null || selfFileUploadedData.imageTooLarge) {
                functionFail ? functionFail(selfFileUploadedData) : angular.noop();
            }
            functionToCreateEntity().then(function (createEntityResult) {
                if (createEntityResult.status && createEntityResult.status !== 201 && createEntityResult.status !== "captured") {
                    functionFail ? functionFail() : angular.noop();
                    return;
                }
                // Log
                if (logObject) {
                    bLog.logGoogleEvent(logObject);
                }

                if (self.fileData instanceof Object && !self.fileData.imageTooLarge) {
                    var parentId = 0;
                    if (!_.isUndefined(functionLookForParentObjectId)) {
                        parentId = functionLookForParentObjectId(createEntityResult);
                    }
                    else {
                        parentId = articlesForm ? createEntityResult.articles[0].id : createEntityResult.id;
                    }
                    postFileService.createRelationships(self.fileData.items[0].id, parentId, reverseRelationship).then(function () {
                        functionSucceedWhenFileUploaded ? functionSucceedWhenFileUploaded(self.fileData, createEntityResult) : angular.noop();
                        functionSucceedAnyway ? functionSucceedAnyway(createEntityResult) : angular.noop();
                    }, function () {
                        // Here we need to rollback the creating of entity.
                        if (functionToRollback) {
                            functionToRollback(createEntityResult).then(
                                functionFail ? functionFail() : angular.noop()
                            );
                        } else {
                            // If there is no rollback function it may mean that there is no way to remove already created entity. So what we 
                            // can to do is just to try creating relationship again.
                            postFileService.createRelationships(self.fileData.items[0].id, createEntityResult.id, reverseRelationship).then(function () {
                                functionSucceedWhenFileUploaded ? functionSucceedWhenFileUploaded(self.fileData, createEntityResult) : angular.noop();
                                functionSucceedAnyway ? functionSucceedAnyway(createEntityResult) : angular.noop();
                            }, function () {
                                functionFail ? functionFail() : angular.noop();
                            });
                        }                           
                    });
                }
                else if (self.fileData.imageTooLarge) {
                    angular.noop();
                }
                else if (self.fileData == null) {
                    // Error during file upload
                    functionFail ? functionFail() : angular.noop();
                }
                else {
                    // There is no any file to attach so just simply do 'anyway' function
                    functionSucceedAnyway ? functionSucceedAnyway(createEntityResult) : angular.noop();
                }

            }, function(){
                functionFail ? functionFail() : angular.noop();
            });
        });
    };

}]);