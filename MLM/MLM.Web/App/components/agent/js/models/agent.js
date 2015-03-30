mlm.factory("agent", [function () {
    var privateMessage = function (privateMessageModelData) {
        if (!privateMessageModelData)
            privateMessageModelData = {};
        angular.extend(this, {
            enableEmailNotification: privateMessageModelData.enableEmailNotification ? privateMessageModelData.enableEmailNotification : true
        });
        angular.extend(this, privateMessageModelData);
    };
    return privateMessage;
}]);