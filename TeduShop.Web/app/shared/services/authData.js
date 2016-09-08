/// <reference path="E:\Cong viec\projects\tedushop-bai14\TeduShop.Web\Assets/admin/libs/angular/angular.js" />
(function (app) {
    'use strict';
    app.factory('authData', [
        function () {
            var authDataFactory = {};

            var authentication = {
                IsAuthenticated: false,
                userName: ""
            };
            authDataFactory.authenticationData = authentication;

            return authDataFactory;
        }
    ]);
})(angular.module('tedushop.common'));

//Cách viết trên có thể viết lại như sau:
//(function (app) {
//    'use strict';
//    app.factory('authData', authData);

//    function authData() {
//        var authDataFactory = {};

//        var authentication = {
//            IsAuthenticated: false,
//            userName: ""
//        };
//        authDataFactory.authenticationData = authentication;

//        return authDataFactory;
//    }
//})(angular.module('tedushop.common'));