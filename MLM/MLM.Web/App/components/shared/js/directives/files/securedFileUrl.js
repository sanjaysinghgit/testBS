mlm.service('securedFileUrl', [function () {
    var self = this;
    self.getUrl = function (documentUrl) {
        if (documentUrl.indexOf('blob.core.windows.net') > -1) {
            return documentUrl;
        }
        var startPosition = documentUrl.search("signed/files");
        var partOfURL = documentUrl.slice(startPosition, documentUrl.length);
        return  "/data/securedFile/" + partOfURL; 
    };
}]);