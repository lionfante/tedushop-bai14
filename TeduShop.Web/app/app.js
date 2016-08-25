/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop', ['tedushop.products','tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($sateProvider, $urlRouterProvider) {
        $sateProvider.state('home', {
            url: "/admin",
            templateUrl: "/app/components/home/homeView.html",
            controller: "homeController"
        });
        $urlRouterProvider.otherwise('/admin'); //Phương thức nếu không phải trường hợp nào sẽ trả về 'admin'
    }
})();