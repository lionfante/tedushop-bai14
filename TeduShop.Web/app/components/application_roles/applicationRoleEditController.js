(function (app) {
    'use strict';

    app.controller('applicationRoleEditController', applicationRoleEditController);

    applicationRoleEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams', 'commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function applicationRoleEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {

        $scope.appRole = {

        };

        $scope.editApplicationRole = editApplicationRole;

        function editApplicationRole() {
            apiService.put('api/applicationrole/update', $scope.appRole, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.appRole.Name + ' đã được chỉnh sửa');
            $location.url('application_roles');
        }

        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayError(response);
        }

        function loadDetail() {
            apiService.get('api/applicationrole/detail/' + $stateParams.id, null, function (result) {
                $scope.appRole = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function loadRoles() {
            apiService.get('api/applicationrole/getlistall', null, function (response) {
                $scope.roles = response.data;
            }, function (error) {
                notificationService.displayError('Không tải được danh sách quyền.');
            });
        }

        loadDetail();
        loadRoles();

    }
})(angular.module('tedushop.application_roles'));