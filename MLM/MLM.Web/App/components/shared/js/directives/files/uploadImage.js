'use strict';

/// This directive provides functionality for image uploading with preview and trash can.

mlm.directive('uploadImage', ['$rootScope', 'localization', function ($rootScope, localization) {
return {

    restrict: 'E',
    replace: true,

    scope: {
        imgDataUrl: '=',
        placeholderurl: '=',
        uploadText: '@',
        uploadAnotherText: '@',
        showRemoveButton: '@',
        imageClass: '@',
        imageStyle: '@',
        showButtonsAtBottom: '@',
        showSmallButtons: '@'
    },
    template: 
    '<div class="img-container">'+
        '<div class="btn-row" ng-if="!showButtonsAtBottom">' +
            '<div class="btn btn-small btn-file-input" ng-class="showSmallButtons ? \'btn-small\' : \'\'">' +
                '{{changeimagetitle}}'+
                '<input type="file" id="file-upload" name="file-upload" '+
                    'bz-file-input file="imgDataUrl" on-file-change="onFileChange"'+
                '/>'+
            '</div>'+
            '<a ng-if="!showRemoveButton" class="btn btn-small pull-right" id="btn-delete" ' +
                    'ng-click="deleteImage()"'+
                    'ng-disabled="imgDataUrl==\'\'">'
                +'<span class="ico-trash"></span>'+
            '</a>' +
        '</div>'+
        '<div class="img-preview">'+
            '<img ng-show="imgSrc!= \'\'" id="img-preview" ng-src="{{imgSrc}}" ' +
                'class="{{imageClass}}" style={{imageStyle}}/>'+
        '</div>' +
        '<div class="btn-bottom-row" ng-if="showButtonsAtBottom">' +
            '<div class="btn btn-file-input" ng-class="showSmallButtons ? \'btn-small\' : \'\'">' +
                '{{changeimagetitle}}' +
                '<input type="file" id="file-upload" name="file-upload" ' +
                    'bz-file-input file="imgDataUrl" on-file-change="onFileChange"' +
                '/>' +
            '</div>' +
            '<a ng-if="!showRemoveButton" class="btn" id="btn-delete" ' +
                    'ng-click="deleteImage()"' +
                    'ng-disabled="imgDataUrl==\'\'">'
                + '<span class="ico-trash"></span>' +
            '</a>' +
        '</div>' +
    '</div>',

    link: function (scope, element, attrs) {
        // some IE speciific stuff
        var IE = window.FileReader == undefined;
        var IE10 = navigator.userAgent.indexOf('MSIE 10') > -1;

        scope.oldImageUrl = scope.imgDataUrl;
        scope.t = localization.t;

        // Configure button labels
        scope.title         = (scope.uploadText == undefined) ? 
                                    localization.t('imageEdit.changeImage') :
                                    scope.uploadText;

        // Label for 'another image'                                    
        scope.uploadAnother  = (scope.uploadAnotherText == undefined) ? 
                                    localization.t('imageEdit.changeAnotherImage') :
                                    scope.uploadAnotherText;

        // Not sure what's happening here
        if (scope.imgDataUrl == undefined || scope.imgDataUrl == '') {
            scope.changeimagetitle = scope.title;
            scope.imgDataUrl = '';
        }
        else {
            scope.imgDataUrl += '?w=604&autorotate=true';
            scope.changeimagetitle = scope.uploadAnother;
        }

        // set the image src for the preview
        scope.imgSrc = scope.imgDataUrl || scope.placeholderurl || '';

        scope.$on('formCanceled', function () {
            scope.imgDataUrl = scope.oldImageUrl;
        });

        // watch the image url for changes
        scope.$watch('imgDataUrl', function (newValue) {
            if (newValue == '') {
                scope.changeimagetitle = scope.title;
            }
            else {
                scope.changeimagetitle = scope.uploadAnother;
            }
        });

        /**
         * Watch for when the file changes
         * @return {[type]} [description]
         */
        scope.onFileChange = function (files) {

            scope.$emit('imageChanged');

            if (IE) {
                scope.imgSrc = "file:///" + scope.imgDataUrl;
            }
            else {
                var files = files;
                var Url = window.URL || window.webkitURL;
              
                if (!files[0].type.match('image.*')) {
                    alert(localization.t('imageEdit.onlyImageWarning'));
                    return;
                }
                if (files[0].size > 10 * 1000000) {
                    alert(localization.t('imageEdit.bigImageError'));
                    return;
                }
                else {
                    var reader = new FileReader();
                    reader.readAsArrayBuffer(files[0]);
                    scope.imgDataUrl = scope.imgSrc = Url.createObjectURL(files[0]);
                }
            }

            scope.$apply();
            
        }

        /**
         * [deleteImage description]
         * @return {[type]} [description]
         */
        scope.deleteImage = function() {
            if (!confirm(localization.t('imageEdit.deleteConfirmation'))) {
                return;
            } else {
                scope.$emit('imageChanged');
                scope.imgSrc = scope.placeholderurl ? scope.placeholderurl : '';
                scope.imgDataUrl = '';
            }

        
        }
    }
}}]);