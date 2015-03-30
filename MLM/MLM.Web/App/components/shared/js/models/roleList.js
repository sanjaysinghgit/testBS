mlm.factory("roleList", ['roleListModel', function (roleListModel) {
    var roleList = function (roleListData) {
        angular.extend(this, {
            items: populateList(roleListData.items),
            totalItems: parseInt(roleListData.totalItems),
        });
        angular.extend(this, roleListData.items);
    };
    return roleList;

    function populateList(items) {
        var roleItems = [];
        _.each(items, function (item) {
            roleItems.push(new roleListModel(item));
        });
        return roleItems;
    }
}]);

mlm.factory("roleListModel", function () {
    var role = function (roleListModelData) {
        angular.extend(this, {
           
        });
        angular.extend(this, roleListModelData);
    };
    return role;
});
