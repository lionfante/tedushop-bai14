(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    productCategoryAddController.$inject = ['$scope', 'apiService','notificationService','$state'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function productCategoryAddController($scope, apiService, notificationService,$state) {

        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true,
        };

        $scope.AddProductCategory = AddProductCategory;

        function AddProductCategory() {
            apiService.post('api/productcategory/create',$scope.productCategory,
                function (result) {
                    notificationService.displaySuccess('Thêm ' + $scope.productCategory.Name + ' thành công.');
                    $state.go('product_categories');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công');
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
    }
})(angular.module('tedushop.product_categories'));