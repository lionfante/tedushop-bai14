/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('applicationRoleListController', applicationRoleListController);

    applicationRoleListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function applicationRoleListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.getApplicationRoles = getApplicationRoles;
        
        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.deleteApplicationRole = deleteApplicationRole;
        $scope.selectAll = selectAll;
        $scope.deleteMulti = deleteMulti;

        function deleteMulti() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedApplicationRoles: JSON.stringify(listId) //Chuyển sang kiểu string
                }
            };
            apiService.del('api/applicationrole/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.')
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công.');
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.data, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.data, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("data", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteApplicationRole(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/applicationrole/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                });
            });
        }

        function search() {
            getApplicationRoles();
        }

        function getApplicationRoles(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    filter: $scope.keywork
                }
            };
            apiService.get("/api/applicationrole/getlistpaging", config, dataLoadCompleted, dataLoadFailed);

            function dataLoadCompleted(result) {
                $scope.data = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pageCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;

                if ($scope.filterExpression && $scope.filterExpression.length) {
                    notificationService.displayInfo(result.data.Items.length + ' được tìm thấy');
                }
            }

            function dataLoadFailed(response) {
                notificationService.displayError(response.data);
            }
        }

        function clearSearch() {
            $scope.filterExpression = '';
            search();
        }

        $scope.search();
    }
})(angular.module('tedushop.application_roles'));