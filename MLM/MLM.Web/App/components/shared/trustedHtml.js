mlm.directive('bindTrustedHtml', ['$sce', function ($sce) {
    return {
        link: function (scope, element, attr) {
            var html = $sce.trustAsHtml(attr.bindTrustedHtml);
            var result = $sce.parseAsHtml(html);
            element.html(html);
        }
    };
}]);