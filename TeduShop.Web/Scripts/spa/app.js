/// <reference path="../plugins/angular/angular.js" />

//Khởi tạo module
var myApp = angular.module("myModule", []);

//declare
function myController($scope){
    $scope.message = "This is controller of AngularJS";
}

//Khởi tạo controller
myApp.controller("myController", myController);

myController.$inject = ['$scope'];