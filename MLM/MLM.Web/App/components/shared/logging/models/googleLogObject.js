mlm.factory("googleLogObject", [function () {

    return function (category, action, label, value, postValue) {

        var logObject = {
            category: category,
            action: action,
            label: label,
            value: value,
            postValue: postValue
        };

        return logObject;
    };

}]);