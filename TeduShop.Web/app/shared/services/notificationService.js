(function(app){
    app.factory("notificationService", notificationService);    //Thêm 1 service trong angularJS

    //Define service
    function notificationService(){
        toastr.options = {
            "debug": false,
            "positionClass": "toast-top-right",
            "onClick": null,
            "showDuration": 300,
            "hideDuration": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };
        return {
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        }

        function displaySuccess(message){
            toastr.success(message);
        }

        function displayError(error){
            if(Array.isArray(error)){
                error.each(function(err){
                    toastr.Error(err);
                })
            }else{
                toastr.error(error);
            }
        }

        function displayWarning(warning){
            toastr.warning(warning);
        }

        function displayInfo(info){
            toast.info(info);
        }
    }
})(angular.module('tedushop.common'));