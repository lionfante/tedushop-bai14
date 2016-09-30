(function (app) {
    'use strict';

    app.controller('applicationGroupAddController', applicationGroupAddController);

    applicationGroupAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function applicationGroupAddController($scope, apiService, notificationService, $state, commonService) {

        $scope.appGroup = {
            ID: 0,
            Roles: []
        };

        $scope.addApplicationGroup = addApplicationGroup;

        function addApplicationGroup() {
            apiService.post('api/applicationgroup/add', $scope.appGroup, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.appGroup.Name + ' đã được thêm mới');
            $location.url('application_groups');
        }

        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        function loadRoles() {
            apiService.get('api/applicationrole/getlistall', null, function (response) {
                $scope.roles = response.data;
            }, function (error) {
                notificationService.displayError('Không tải được danh sách quyền.');
            });
        }

        loadRoles();

    }
})(angular.module('tedushop.application_groups'));