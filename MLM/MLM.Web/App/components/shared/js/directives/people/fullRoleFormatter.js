mlm.directive('fullRoleFormatter',[ 'permissionsRepository', function (permissionsRepository) {
    return {
        restrict: 'E',
        templateUrl: '/App/components/shared/partial/fullRole.html',
        link: function ($scope, element, attr) {

            $scope.canReadAllMemberInfo = permissionsRepository.validateClaim("can_read_all_member_info");

            var ROLE_HIERARCHY = [
                "Developer",
                "Property Manager",
                "Staff",
                "Strata",
                "Resident - Owner",
                "Resident - Renter"
            ];

            
        }
    };
}]);