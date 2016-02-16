(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("hello-worldController", 
        function () {
            var vm = this;

            // Show a Hello World message
            vm.showAlert = function () {
                alert('Hello World!');
            };

        }
    );
}());