/// <reference path="../plugins/angular/angular.js" />

//Khởi tạo module
var myApp = angular.module("myModule", []);

//Khởi tạo controller
myApp.controller("schoolController", schoolController);

myApp.service("ValidatorService", ValidatorService);

myApp.directive("teduShopDirective", teduShopDirective);

schoolController.$inject = ['$scope', 'ValidatorService'];

//declare
function schoolController($scope, ValidatorService) {
    $scope.checkNumber = function () {
        $scope.message = ValidatorService.checkNumber($scope.num);
    }
    $scope.num = 1;
    
}
//myController.$inject = ['$scope'];

//Khai báo service (sau này tách riêng ra 1 file script servier riêng)
function ValidatorService($window) {
    return {
        checkNumber: checkNumber
    }
    function checkNumber(input) {
        if (input % 2 == 0) {
            //$window.alert("This is even");
            return "This is even";
        }
        else{
            //$window.alert("This is odd");
            return "This is odd";
        }
    }
}

function teduShopDirective() {
    return {
        //template: "<h1>This is my first custom directive</h1>"
        restrict: "A",
        templateUrl: "/Scripts/spa/teduShopDirective.html"
    }
}



