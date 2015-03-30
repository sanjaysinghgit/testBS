'use strict';

mlm.factory('postFileService',
    ['$http', '$q', '$resource', '$rootScope', 'localization', '$route', 'authCache',
    function ($http, $q, $resource, $rootScope, localization, $route,  authCache) {

    var resourceUpdateParent = $resource(Url.resolve('objects/:objectId/relationships'), { objectId: '@Id' }, {
        update: { method: 'POST', isArray: false }
    });
    
    var ResourceDelete = $resource(Url.resolve('files/:fileId'), { fileId: '@Id' }, {
        remove: { method: 'DELETE', isArray: false }
    });

    return {
        // Send the current form content (where input type = file) to the server
        post: function (formId, callbackSuccess, callbackError) {
            var headers = {}, authHeader;
            ///########################################################
            /// Injecting Authorization Token in header
            ///########################################################
            var authorizationToken = authCache.get("AuthorizationToken");
            if (authorizationToken) {
                headers['AuthorizationToken'] = authorizationToken;
            }
            
            var requestUrl = '/data/files/';
            var ajaxOptions = {
                url: requestUrl,
                method: 'POST',
                secureuri: false,
                formId: formId,
                dataType: 'json',
                beforeSend: function() {
                },
                complete: function() {
                },
                success: function(data, status) {
                    callbackSuccess(data);
                },
                error: function(data, status, e) {
                    callbackError(data);
                },
            };            
            
            ///########################################################
            /// look for development id in route.
            ///########################################################
            if ($route && $route.current && $route.current.params && $route.current.params.developmentId) {
                authHeader = jQuery.extend({}, headers, { 'X-Bazinga-DevelopmentId': $route.current.params.developmentId });
                $.ajaxFileUpload(jQuery.extend({}, ajaxOptions, {
                    headers: authHeader
                }));
            } else {
                ///########################################################
                /// If development id is not defined in route.
                ///########################################################
                ////////developmentRepository.getCurrentDevelopment().then(function (currentDevelopment) {
                ////////    if (currentDevelopment && currentDevelopment.id) {
                ////////        authHeader = jQuery.extend({}, headers, { 'X-Bazinga-DevelopmentId': currentDevelopment.id });
                ////////        $.ajaxFileUpload(jQuery.extend({}, ajaxOptions, {
                ////////            headers: authHeader
                ////////        }));
                ////////    } else {
                ////////        $.ajaxFileUpload(jQuery.extend({}, ajaxOptions, headers));
                ////////    }
                ////////});
            }
        },

        createRelationships: function (uploadedFileId, parentObjectId, reverse) {
            var targetId = reverse ? parentObjectId : uploadedFileId;
            var sourceId = reverse ? uploadedFileId : parentObjectId;
            var target = { target: { id: targetId } };
            var deferred = $q.defer();
            var res = resourceUpdateParent.update({ objectId: sourceId}, target, function (data) {
                deferred.resolve(true); 
            }, function (err) {
                deferred.reject(null);
            });
            return deferred.promise;
        },

        saveUser: function (developmentId, userId, userData) {
            var deferred = $q.defer();
            personResource.save({ developmentId: developmentId, userId: userId }, userData, function (data) {
                deferred.resolve(data);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        },
        
        removeFile: function (fileId) {
            var deferred = $q.defer();
            ResourceDelete.remove({ fileId: fileId }, function (result) {
                deferred.resolve(200);
            }, function (result) {
                deferred.resolve(400);
            });
            return deferred.promise;
        }
    };
}]);