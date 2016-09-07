(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope','apiService', 'notificationService', '$state', 'commonService'];

    function productAddController($scope, apiService, notificationService, $state, commonService) {
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

        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        $scope.AddProduct = AddProduct;

        function AddProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);
            apiService.post('api/product/create', $scope.product,
                function (result) {
                    notificationService.displaySuccess('Thêm ' + $scope.product.Name + ' thành công.');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công');
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

        $scope.moreImages = [];

        $scope.ChooseMoreImage = ChooseMoreImage;

        function ChooseMoreImage() {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {     //Bắt angular js force chạy cập nhật ngay ra view
                    $scope.moreImages.push(fileUrl);
                });
            }
            finder.popup();     //HIển thị ckfinder
        }


        getListProductCategories();
    }
 
})(angular.module('tedushop.products'));