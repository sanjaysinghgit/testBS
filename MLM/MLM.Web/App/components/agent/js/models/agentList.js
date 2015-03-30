mlm.factory("agentList", function () {
    var agentList = function (agentListData) {

        angular.extend(this, {
            items: populateList(agentListData.items),
            totalItems: parseInt(agentListData.totalItems),
            itemsPerPage: parseInt(agentListData.itemsPerPage),
            startIndex: parseInt(agentListData.startIndex),
        });
    };

    return agentListData;

    function populateList(items) {
        var agentListDataModelItems = [];
        _.each(items, function (item) {
            agentListDataModelItems.push(new agentListDataModel(item));
        });
        return agentListDataModelItems;
    }
});
