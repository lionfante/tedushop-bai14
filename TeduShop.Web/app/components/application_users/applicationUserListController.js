(function (app) {
    'use strict';

    app.controller("applicationUserListController", applicationUserController);

    applicationUserController.$inject = ["$scope", "apiService", "notificationService","$ngBootbox", "$filter"];

    function applicationUserController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.dataUsers = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;
        $scope.filterExpression = '';

        $scope.search = search;
        $scope.getUsers = getUsers;
        $scope.search = search;

        function search() {
            getUsers();
        }

        function getUsers(page) {
            page = page || 0;
            $scope.loading = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.filterExpression
                }
            };

            apiService.get("/api/applicationusers/getlistpaging", config, dataLoadCompleted, dataLoadFailed);

            function dataLoadCompleted(result) {
                $scope.dataUsers = result.data.Items,
                $scope.page = result.data.page,
                $scope.pageCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;

                if ($scope.filterExpression && $scope.filterExpression.length) {
                    notificationService.displayInfo(result.data.Items.length + ' items found');
                }
            }

            function dataLoadFailed(error) {
                notificationService.displayError(error.data);
            }

            function clearSearch() {
                $scope.filterExpression = '';
                search();
            }
        }

        $scope.search();
    }
})(angular.module('tedushop.application_users'));