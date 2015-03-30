mlm
    .factory("agentTreeList", ['agentListModel', function (agentListModel) {
    var agentList = function (agentListData) {

        angular.extend(this, {
            items: populateList(agentListData.items),
            totalItems: parseInt(agentListData.totalItems),
        });
    };

    return agentList;

    function populateList(items) {
        var agentListModelItems = [];
        _.each(items, function (item) {
            agentListModelItems.push(new agentListModel(item));
        });
        return agentListModelItems;
    }
}]);

mlm.
    factory("agentListModel", [
        function (agent) {
    var agentListModel = function (agentListModelData) {

        return {
            name: agentListModelData.Code,
            parent: agentListModelData.SponsorCode,
        };
    };

    return agentListModel;
}]);