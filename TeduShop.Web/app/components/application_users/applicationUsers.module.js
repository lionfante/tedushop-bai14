(function () {
    angular.module("tedushop.application_users", ["tedushop.common"]).config(config);

    config.$inject = ["$stateProvider", "$urlRouterProvider"];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('application_users', {
            url: "/application_users",
            parent: "base",
            templateUrl: "/app/components/application_users/applicationUserListView.html",
            controller: "applicationUserListController"
        }).state('add_application_user', {
            url: "/add_application_user",
            parent: "base",
            templateUrl: "/app/components/application_users/applicationUserAddView.html",
            controller: "applicationUserAddController"
        }).state("edit_application_user", {
            url: "/edit_application_user",
            parent: "base",
            templateUrl: "/app/components/application_users/applicationUserEditView.html",
            controller: "applicationUserEditController"
        });
    }
})();