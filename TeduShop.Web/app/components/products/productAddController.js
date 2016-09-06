(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope','apiService', 'notificationService', '$state'];

    function productAddController($scope, apiService, notificationService, $state) {
        $scope.productCategories = [];

        $scope.getListProductCategories = getListProductCategories;

        function getListProductCategories() {
            apiService.get('api/productcategory/getallparents',null, function (result) {
                $scope.productCategories = result.data;
            }, function (error) {
                console.log('Get product category failed.');
            });
        }

        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        };

        $scope.product = {
            CreatedDate: new Date(),
            Status: true,
        };

        $scope.AddProduct = AddProduct;

        function AddProduct() {
            apiService.post('api/product/create', $scope.product,
                function (result) {
                    notificationService.displaySuccess('Thêm ' + $scope.product.Name + ' thành công.');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công');
                });
        }

        getListProductCategories();
    }
 
})(angular.module('tedushop.products'));