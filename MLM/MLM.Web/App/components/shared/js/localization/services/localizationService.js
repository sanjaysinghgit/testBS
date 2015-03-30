mlm.service('localization', function () {
    return {
        t: function (key) {
            if (resources[key])
                return resources[key];
            else {
                return "";
            }
        }
    };
});