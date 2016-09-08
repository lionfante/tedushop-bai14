(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    productCategoryEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state','$stateParams','commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function productCategoryEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {

        $scope.productCategory = {
            CreatedDate: new Date(),
        };

        $scope.UpdateProductCategory = UpdateProductCategory;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        function loadProductCategoryDetail() {
            apiService.get('api/productCategory/getbyid/' + $stateParams.id, null, function (result) {
                $scope.productCategory = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateProductCategory() {
            apiService.put('api/productcategory/update', $scope.productCategory,
                function (result) {
                    notificationService.displaySuccess('Cập nhật ' + $scope.productCategory.Name + ' thành công.');
                    $state.go('product_categories');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công');
                });
        }

        $scope.parentCategories = [];

        function loadParentCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;
            }, function () {
                console.log('Cannot get list parent.');
            });
        }

        loadParentCategory();
        loadProductCategoryDetail();
    }
})(angular.module('tedushop.product_categories'));