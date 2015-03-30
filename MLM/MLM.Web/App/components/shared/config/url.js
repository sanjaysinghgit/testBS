var Url = {
    localizationPath : function (language) {
        return "/app/localization/" + language + ".json";
    },
    resolve: function (path) {
        if(path.indexOf("/")!=0)
            path = "/"+path;
        return MLM_CONFIG.SERVICE_BASE() + '/api' + path;
    },
    resolvePublic: function (path) {
        if (path.indexOf("/") != 0)
            path = "/" + path;
        return MLM_CONFIG.SERVICE_BASE() + '/api/public' + path;
    },
    resolveLocal: function (path) {
        return '/app' + path;
    }
}