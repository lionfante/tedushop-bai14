(function (app) {
    'use strict';

    app.controller('applicationGroupEditController', applicationGroupEditController);

    applicationGroupEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location','$stateParams', 'commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function applicationGroupEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {

        $scope.appGroup = {
            Roles: []
        };

        $scope.editApplicationGroup = editApplicationGroup;

        function editApplicationGroup() {
            apiService.put('api/applicationgroup/update', $scope.appGroup, addSuccessed, addFailed);
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

        function getDetail() {
            apiService.get('api/applicationgroup/detail/' + $stateParams.id, null, function (result) {
                $scope.appGroup = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        getDetail();
        loadRoles();

    }
})(angular.module('tedushop.application_groups'));