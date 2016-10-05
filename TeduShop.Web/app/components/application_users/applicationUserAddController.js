(function (app) {
    'use strict';

    app.controller("applicationUserAddController", applicationUserAddController);

    applicationUserAddController.$inject = ["$scope", "apiService", "notificationService", "$ngBootbox", "$location"];

    function applicationUserAddController($scope, apiService, notificationService, $ngBootbox, $location) {
        $scope.appUser = {
            Groups: []
        };

        $scope.loadGroups = loadGroups;

        $scope.addUser = addUser;

        function addUser() {
            apiService.post("api/applicationusers/add", $scope.appUser, dataAddCompleted, dataAddFailed);
        }

        function dataAddCompleted(response) {
            notificationService.displaySuccess($scope.appUser.Name + " đã được thêm mới.");
            $location.url("application_users");
        }

        function dataAddFailed(error) {
            notificationService.displayError(error.data);
        }

        function loadGroups() {
            apiService.get("api/applicationgroup/getlistall", null, function (response) {
                $scope.groups = response.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        loadGroups();
    }
})(angular.module('tedushop.application_users'));