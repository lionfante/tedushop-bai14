/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />
(function (app) {
    'use strict';

    app.controller('applicationUserEditController', applicationUserEditController);

    applicationUserEditController.$inject = ["$scope", "apiService", "notificationService", "$ngBootbox", "$location", "$stateParams"];

    function applicationUserEditController($scope, apiService, notificationService, $ngBootbox, $location, $stateParams) {

        $scope.appUser = {
            Groups:[]
        };

        //$scope.editUser = editUser;

        function loadDetail() {
            apiService.get('api/applicationusers/detail/' + $stateParams.id, null, function (response) {
                $scope.appUser = response.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        
        function loadGroups() {
            apiService.get('api/applicationgroup/getlistall', null, function (response) {
                $scope.groups = response.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        loadDetail();
        loadGroups();
    }
})(angular.module('tedushop.application_users'));