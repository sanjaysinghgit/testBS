mlm.factory("treeReport", [function () {
    var treeReport = function (treeReportData) {
        if (!treeReportData)
            treeReportData = {};
        angular.extend(this, {
            enableEmailNotification: treeReportData.enableEmailNotification ? treeReportData.enableEmailNotification : true
        });
        angular.extend(this, treeReportData);
    };
    return treeReport;
}]);