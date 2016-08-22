/// <reference path="../plugins/angular/angular.js" />

//Khởi tạo module
var myApp = angular.module("myModule", []);

//Khởi tạo controller
myApp.controller("schoolController", schoolController);


myApp.service("Validator", Validator);

schoolController.$inject = ['$scope', 'Validator'];

//declare
function schoolController($scope, Validator) {
    $scope.checkNumber = function () {
        $scope.message = Validator.checkNumber($scope.num);
    }
    $scope.num = 1;
    
}
//myController.$inject = ['$scope'];

//Khai báo service (sau này tách riêng ra 1 file script servier riêng)
function Validator($window) {
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



