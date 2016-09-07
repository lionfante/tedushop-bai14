(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];

    //$state là 1 đôi tượng dùng để điều hướng trong angularJS

    function productEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {

        $scope.product = {
        };

        $scope.moreImages = [];

        $scope.UpdateProduct = UpdateProduct;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        function loadProductDetail() {
            apiService.get('api/product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;
                $scope.moreImages = JSON.parse($scope.product.MoreImages);
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);
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
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                });
                
            }
            ckfinder.popup();
        }

        $scope.ChooseMoreImage = function () {
            var ckfinder = new CKFinder();
            ckfinder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                });
                
            }
            ckfinder.popup();
        }


        getListProductCategories();
        loadProductDetail();
    }
})(angular.module('tedushop.products'));