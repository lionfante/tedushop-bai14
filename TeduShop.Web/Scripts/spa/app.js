/// <reference path="../plugins/angular/angular.js" />

//Khởi tạo module
var myApp = angular.module("myModule", []);

//Khởi tạo controller
myApp.controller("schoolController", schoolController);
myApp.controller("studentController", studentController);
myApp.controller("teacherController", teacherController);

//declare
function schoolController($scope) {
    $scope.message = "Annountment from school";
}
function studentController($scope){
    $scope.message = "This is message from student";
}
function teacherController($scope) {
    $scope.message = "This is message from teacher";
}
//myController.$inject = ['$scope'];



