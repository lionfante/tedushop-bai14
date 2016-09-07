(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function productEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {

        $scope.product = {
        };

        $scope.UpdateProduct = UpdateProduct;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        function loadProductDetail() {
            apiService.get('api/product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateProduct() {
            apiService.put('api/product/update', $scope.product,
                function (result) {
                    notificationService.displaySuccess('Cập nhật ' + $scope.product.Name + ' thành công.');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công');
                });
        }

        $scope.parentCategories = [];

        function getListProductCategories() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function (error) {
                console.log('Get product category failed.');
            });
        }

        $scope.ChooseImage = function () {
            var ckfinder = new CKFinder();
            ckfinder.selectActionFunction = function (fileUrl) {
                $scope.product.Image = fileUrl;
            }
            ckfinder.popup();
        }

        getListProductCategories();
        loadProductDetail();
    }
})(angular.module('tedushop.products'));