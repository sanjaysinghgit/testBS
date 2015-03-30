'use strict'


mlm.directive('fileIcon', function () {

    return {
        restrict: 'E',
        templateUrl: '/App/components/shared/partial/fileIcon.html',
        replace: true,
        scope: {
            url: '@url',
            mimeType: '@'
        },
        link: function (scope, element, attrs) {

            var isRetina = (window.devicePixelRatio > 1.3);

            scope.width = isRetina ? 160 : 80;

            var fileTypes = [
                {label: '', dataFileType: 'image', extentions: ['.jpg', '.jpeg', '.png', '.bmp', '.gif'], mimeTypes: ['image/gif', 'image/jpeg', 'image/png', 'image/bmp', 'image/x-windows-bmp']},
                { label: 'PDF', dataFileType: 'pdf', extentions: ['.pdf'], mimeTypes: ['application/pdf'] },
                { label: 'TXT', dataFileType: 'txt', extentions: ['.txt'], mimeTypes: ['text/plain'] },
                { label: 'DOC', dataFileType: 'doc', extentions: ['.doc', '.docx'], mimeTypes: ['application/msword', 'application/word', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'] },
                { label: 'PPT', dataFileType: 'ppt', extentions: ['.ppt', '.pptx'], mimeTypes: ['application/ppt', 'application/mspowerpoint', 'application/powerpoint', 'application/vnd.ms-powerpoint', 'application/x-mspowerpoint', 'application/vnd.openxmlformats-officedocument.presentationml.presentation'] },
                { label: 'XLS', dataFileType: 'xls', extentions: ['.xls', '.xslx'], mimeTypes: ['application/excel', 'application/vnd.ms-excel', 'application/x-excel', 'application/x-msexcel', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'] }
            ];
            var fileTypeUnknown = { label: 'FILE', dataFileType: 'file' };

           
            function redefine() {
                var fileName = scope.url.replace(/^.*[\\\/]/, '');
                var extMatch = fileName.match(/\.[^.]+$/);
                var ext = extMatch ? extMatch.toString() : null;
                scope.fileType = _.find(fileTypes, function (fileType) {
                    var t = _.find(fileType.extentions, function(extention){
                        return extention == ext;
                    });
                    if (_.isUndefined(t)){
                        t = _.find(fileType.mimeTypes, function(mimeType){
                            return mimeType == scope.mimeType;
                        });
                    }
                    return !_.isUndefined(t);
                });
                if (_.isUndefined(scope.fileType)) {
                    scope.fileType = fileTypeUnknown;
                }
            };

            scope.$watch('url', function () {
                if (scope.url) {
                    redefine();
                }
            });

            if (!scope.url) {
                return;
            }
            else {
                redefine();
            }
        }
    }
});
