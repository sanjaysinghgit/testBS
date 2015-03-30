mlm.directive('attachment', ['$route', '$rootScope', 'documentsRepository', '$sce', 'localization', 'securedFileUrl',
    function ($route, $rootScope, documentsRepository, $sce, localization, securedFileUrl) {
        return {
            restrict: 'E',
            templateUrl: '/App/components/shared/partial/attachment.html',
            replace: true,
            scope: { item: '=' },
            link: function (scope, element, attr) {

                scope.t = localization.t;

                scope.$watch('item', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        scope.updateItem();
                    }
                });

                scope.updateItem = function() {
                    if (scope.item && scope.item.images && scope.item.images.length > 0) {
                        var attachmentsList = [];
                        attachmentsList.push({
                            $href: scope.item.images[0].url,
                            $objectType: scope.item.images[0].$objectType,
                            displayName: scope.item.images[0].displayName ? scope.item.images[0].displayName : 'No name',
                            url: scope.item.images[0].url,
                            id: scope.item.images[0].id
                        });
                        angular.extend(scope.item, {
                            attachments: attachmentsList,
                            attachmentMode: ''
                        });
                    } else {
                        angular.extend(scope.item, {
                            attachmentMode: ''
                        });
                    }
                };

                scope.showAttachment = function (item) {
                   
                        if (item.attachments[0].$objectType == 'file') {
                            item.attachmentMode = 'doc';
                            documentsRepository.getFile(item.attachments[0].id).then(function (sessionObject) {

                                sessionObject.url = securedFileUrl.getUrl(sessionObject.url);
                                item.attachments[0].docSrc = sessionObject.mimeType == 'text/plain' ? $sce.trustAsResourceUrl(sessionObject.url) : $sce.trustAsResourceUrl(sessionObject.externalSystemProxy.externalSystemEndpointURL);

                            },
                                function () {
                                    $rootScope.showErrorMessage(scope.t('errors.document_error'));
                                });
                        }
                        else {
                            item.attachmentMode = 'image';
                        }
                   
                }

                scope.hideAttachment = function (item) {
                  
                        item.attachmentMode = '';
                  
                }
                scope.updateItem();
            }
        };
    }]);