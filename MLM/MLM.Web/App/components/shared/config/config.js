var MLM_CONFIG;

function LoadConfig() {
    MLM_CONFIG = {
        UI_LANGUAGE: "en",
        SERVICE_BASE: function () {
            var base = window.location.protocol + "//" + window.location.host;
            return base;
        }
    };
}

LoadConfig();
