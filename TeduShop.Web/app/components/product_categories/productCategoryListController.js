/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope','apiService','notificationService'];

    function productCategoryListController($scope, apiService,notificationService) {
        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pageCount = 0;

        $scope.getProductCategories = getProductCategories;
        $scope.keyword = '';

        $scope.search = search;

        function search() {
            getProductCategories();
        }

        function getProductCategories(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            };
            apiService.get("/api/productcategory/getall", config, function (result) {
                if(result.data.TotalCount == 0){
                    notificationService.displayWarning('Không có bản ghi nào!');
                } else {
                    notificationService.displaySuccess('Tìm thấy ' + result.data.TotalCount + ' bản ghi.');
                }
                $scope.productCategories = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load productcategory failed');
            });
        }

        $scope.getProductCategories();
    }
})(angular.module('tedushop.product_categories'));