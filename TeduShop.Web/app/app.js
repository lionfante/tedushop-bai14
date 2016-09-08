/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop',
        ['tedushop.products',
         'tedushop.product_categories',
         'tedushop.common'])
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($sateProvider, $urlRouterProvider) {
        $sateProvider
            .state('base', {
                url: '',
                templateUrl: '/app/shared/views/baseView.html',
                abstract: true, //template chính , các template con kế thừa từ đây
            })
            .state('login', {
                url: "/login",
                templateUrl: "/app/components/login/loginView.html",
                controller: "loginController"
            })
            .state('home', {
                url: "/admin",
                parent: 'base',
                templateUrl: "/app/components/home/homeView.html",
                controller: "homeController"
            });
        $urlRouterProvider.otherwise('/login'); //Phương thức nếu không phải trường hợp nào sẽ trả về 'login'
    }
})();