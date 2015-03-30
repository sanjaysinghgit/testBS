'use strict'

mlm.directive('fileList', ['$rootScope', '$route', '$timeout', 'documentsRepository', 'fileUploadService', '$sce', 'localization', 
    function ($rootScope, $route, $timeout, devDocs, uploader, $sce, localization) {
        return {
            restrict: 'E',
            templateUrl: '/App/components/shared/partial/fileList.html',
            replace: true,
            scope: {
                context: '@',
                containerid: '@',
                breadcrumblink: '@',
                breadcrumbtext: '@',
                containername: '@',
                parameters: '@' // additional parameters for file urls
            },
            link: function (scope, element, attrs) {

                scope.t = localization.t;
                scope.developmentId = $route.current.params.developmentId;
                scope.showuploadButton = attrs.noupload != undefined ? false : true;

                scope.updateFilesInfo = function(files) {
                    scope.files = [];
                    scope.submitting = false;
                    scope.containerName = scope.containername ? scope.containername : files.displayName;
                    scope.files = files.documents;
                    scope.folderId = files.id; // 
                };

                scope.submitting = true;
                
              
                devDocs.getFiles(scope.context, scope.containerid).then(function (files) {
                    scope.updateFilesInfo(files);
                }, function () {
                    scope.submitting = false;
                    $rootScope.showErrorMessage(localization.t('errors.default_error_message'));
                });

             
                scope.showNewFileInfo = function() {
                    var elementInput = $('#fileupload') && $('#fileupload').length ? 
                        $('#fileupload')[0] : null;
                    if (elementInput && elementInput.files && elementInput.files.length > 0) {
                        scope.fileName = elementInput.files[0].name;
                        if (scope.fileName.length > 64) {
                            scope.hideNewFileInfo();
                            alert(localization.t('errors.fileNameIsTooLong'));
                            return;
                        }
                        if (elementInput.files[0].size > 10000000) {
                            scope.hideNewFileInfo();
                            alert(localization.t('errors.fileTooLarge'));
                            return;
                        }
                        if (!$rootScope.checkFileType(elementInput.files[0].type)) {
                            scope.hideNewFileInfo();
                            alert(localization.t('errors.unsupportedFileType'));
                            return;
                        }
                    }
                    scope.newFile = true;
                    try{
                        scope.$apply();
                    }
                    catch (e) { }
                };

                scope.hideNewFileInfo = function() {
                    var elementInput = $('#fileupload');
                    if (elementInput) {
                        scope.fileName = null;
                        $('#fileUploadForm')[0].reset();
                    }
                    scope.newFile = false;
                };

                /**
             * Returns a link to the file detail page for the file 
             * id provided 
             * 
             * @param  {[string]} id - the id of the file
             * @return {[string]} a formatted path to the file detail page
             */
                scope.buildFileDetailLink = function(id) {
                    // Because angular decodes URLs before matching them with routes
                    // we need to double encode the container name to account for
                    // special chars like '/'.
                    var containerName =
                        encodeURIComponent(encodeURIComponent(scope.containerName));
                    var filePath = '/developments/'
                        + scope.developmentId
                        + '/documents/'
                        + scope.containerid
                        + '/' + scope.context + '/'
                        + id
                        + '?container='
                        + containerName;
                    if (scope.parameters) {
                        filePath += (filePath.indexOf('?') < 0 ? '?' : '&') + scope.parameters;
                    }
                    return filePath;
                };

                scope.uploadFile = function() {
                    scope.submitting = true;
                    uploader.doUpload('fileUploadForm', { id: scope.folderId }, false, function (fileUploadedData) {
                        scope.newFile = false;
                        if (fileUploadedData && !fileUploadedData.imageTooLarge) {
                            scope.flash = true;
                            scope.flashMessageClass = 'flash-message';
                            scope.flashMessage = $sce.trustAsHtml(fileUploadedData.items[0].displayName + localization.t('documents.file_upload_success'));
                            scope.flashLink = scope.buildFileDetailLink(fileUploadedData.items[0].id);
                         
                            devDocs.getFiles(scope.context, scope.containerid).then(function(files) {
                                scope.updateFilesInfo(files);
                                scope.submitting = false;
                            });
                    
                        } else {
                            scope.submitting = false;
                            scope.flash = false;
                            scope.hideNewFileInfo();
                            scope.$apply();
                            if (fileUploadedData && fileUploadedData.imageTooLarge)
                                alert(localization.t('errors.imageTooLarge'));
                            else
                                alert(localization.t('errors.fileUpload'));
                        }
                    });
                };

                scope.updateFileName = function(id, fileName) {
                    var fileId = id;
                    // update the filename
                    devDocs.updateFile(id, fileName).then(function (updatedFileData) {
                        // then update the list so that our file is alphabetically organized    
                        devDocs.getFiles(scope.context, scope.containerid).then(function(files) {
                            scope.updateFilesInfo(files);
                        }, function() {
                        });
                    });
                };

                var fileState = {};
                scope.editFile = function(id, state) {
                    if (_.isUndefined(state)) {
                        return fileState[id];
                    } else {
                        fileState[id] = state;
                    }
                };

                scope.deleteFile = function (fileObject) {

                    scope.flash = false;

                    if (confirm(localization.t('documents.delete_file_confirm'))) {
                        scope.submitting = true;
                        devDocs.deleteFile(fileObject.id).then(function() {
                            fileObject.deleted = true;
                            
                            scope.flash = true;
                            scope.flashMessageClass = 'flash-message';
                            scope.flashMessage = $sce.trustAsHtml(fileObject.displayName + " "+ localization.t("common.was_deleted"));

                            scope['editFile' + fileObject.id] = false;

                            devDocs.getFiles(scope.context, scope.containerid).then(function (files) {

                                scope.updateFilesInfo(files);
                                scope.submitting = false;

                            }, function (err) {
                                // handleError
                            });

                           
                        }, function() {
                            alert(localization.t('errors.fileNotDeleted'));
                            scope.submitting = false;
                        });
                    }
                };
            }
        };
    }]);
