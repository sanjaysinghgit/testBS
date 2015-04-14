var baseSiteUrlPath = $("base").first().attr("href");

var baseTemplateUrl = baseSiteUrlPath + "app/";

var Url = {
    localizationPath : function (language) {
        return baseTemplateUrl + "localization/" + language + ".json";
    },
    resolve: function (path) {
        if(path.indexOf("/")!=0)
            path = "/"+path;
        return baseSiteUrlPath + 'api' + path;
    },
    resolvePublic: function (path) {
        if (path.indexOf("/") != 0)
            path = "/" + path;
        return baseSiteUrlPath + 'api/public' + path;
    },
    resolveLocal: function (path) {
        return baseTemplateUrl + path;
    }
}