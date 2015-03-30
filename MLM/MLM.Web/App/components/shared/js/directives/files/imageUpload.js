'use strict'

mlm.
  directive('imageUpload', function () {
      return {
          restrict: 'A',
          link: function ($scope, elm, attrs) {
              var imageElement = null;
              var imgHolderWidth = 0, imgHolderHeight = 0;
              if (attrs.title != undefined) {
                  elm.append($('<p>' + attrs.title + '</p>'));
              }
              if (attrs.imagepreview != undefined && attrs.imagepreview == 'true') {
                  var block = $('<div style="display: block; text-align: center;">');
                  elm.append(block);

                  // Setup placeholder image
                  if (attrs.placeholderpath != undefined && attrs.placeholderpath != '') {
                      //imageElement = $('<img />');
                      
                      //imageElement.attr("src", attrs.placeholderpath);
                      imageElement = new Image();
                      imageElement.src = attrs.placeholderpath;
                      imageElement.onload = function () {
                          imgHolderWidth = imageElement.width;
                          imgHolderHeight = imageElement.height;
                      };
                      block.append(imageElement);
                  }
              }
              var fileElement = $('<input type="file" id="files" class="btn" ng-model="fileName" ng-model-instant />');
              elm.append(fileElement);
              
              if (fileElement != null && fileElement != undefined) {
                  fileElement.on('change', function (eventData) {

                      var files = eventData.target.files;
                      var Url = window.URL || window.webkitURL;
                      if (!files[0].type.match('image.*')) {
                          alert('You can upload only images!');
                          return;
                      }
                      var reader = new FileReader();
                      reader.onload = (function (theFile) {
                          return function (e) {
                              $scope.item.fileContent = e.currentTarget.result; //ArrayBuffer
                          };
                      })(files[0]);

                      // Set up the image
                      if (imageElement != null && imageElement != undefined) {
                          imageElement.onload = null;
                          var img = new Image();

                          img.onload = function () {
                              var h, w;
                              var diffWidth = this.width - imgHolderWidth;
                              var diffHeight = this.height - imgHolderHeight;
                              var heightBased = false; // Flag to know what dimentions to start the calculation with.

                              if (diffHeight > 0 && diffWidth > 0) {
                                  if (diffHeight > diffWidth) {
                                      heightBased = true;
                                  }
                                  else {
                                      heightBased = false;
                                  }
                              }
                              else if (diffHeight > 0 && diffWidth < 0) {
                                  heightBased = true;
                              }
                              else if (diffWidth > 0 && diffHeight < 0) {
                                  heightBased = false;
                              }
                              if (heightBased) {
                                  h = imgHolderHeight;
                                  w = (h * this.width) / this.height;
                              }
                              else {
                                  w = imgHolderWidth;
                                  h = (w * this.height) / this.width;
                              }
                              imageElement.width = w;//css("width", w);
                              imageElement.height = h; //css("height", h);
                              imageElement.src = img.src; //attr("src", img.src);
                          };
                      }
                      // Read in the image file as a data URL.
                      img.src = Url.createObjectURL(files[0]);
                      reader.readAsArrayBuffer(files[0]);

                  });
              }
          }
      }
  });