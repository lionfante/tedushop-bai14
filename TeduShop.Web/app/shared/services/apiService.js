/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiService', apiService);  //Tạo service apiService

    apiService.$inject = ['$http', 'notificationService'];

    function apiService($http, notificationService) {
        return {
            get: get,
            post: post
        }

        function get(url, params, success, failure) {
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                failure(error);
            });
        }

        function post(url, data, success, failure) {
            $http.post(url, data).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === '401') {
                    notificationService.displayError('Authentication is required.');
                }else if(failure != null){
                    failure(error);
                }
                
                failure(error);
            });
        }
    }
})(angular.module('tedushop.common'));