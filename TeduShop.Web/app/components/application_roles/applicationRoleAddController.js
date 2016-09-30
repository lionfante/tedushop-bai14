(function (app) {
    'use strict';

    app.controller('applicationRoleAddController', applicationRoleAddController);

    applicationRoleAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function applicationRoleAddController($scope, apiService, notificationService, $state, commonService) {

        $scope.appRole = {
            ID: 0
        };

        $scope.addApplicationRole = addApplicationRole;

        function addApplicationRole() {
            apiService.post('api/applicationrole/add', $scope.appRole, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.appRole.Name + ' đã được thêm mới');
            $location.url('application_roles');
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
})(angular.module('tedushop.application_roles'));