/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiService', apiService);  //Tạo service apiService

    apiService.$inject = ['$http'];

    function apiService($http) {
        return {
            get: get
        }

        function get(url, params, success, failure) {
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                failure(error);
            });
        }
    }
})(angular.module('tedushop.common'));