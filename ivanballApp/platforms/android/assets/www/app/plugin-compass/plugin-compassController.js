(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-compassController",
        function () {
            var vm = this;
            vm.watch = true;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                startWatch();
            };

            function startWatch() {
                //navigator.compass.getCurrentHeading(onSuccess, onError);

                var options = { frequency: 3000 }; // Update compass every 3 seconds
                vm.watchID = navigator.compass.watchHeading(onSuccess, onError, options);
            };

            // onSuccess: Get the current heading
            function onSuccess(heading) {
                var element = document.getElementById('heading');
                element.innerHTML = 'Heading: ' + heading.magneticHeading;
            };

            vm.toggleWatch = function () {
                if (vm.watch) {
                    startWatch();
                }
                else {
                    if (vm.watchID != null) {
                        navigator.compass.clearWatch(vm.watchID);
                        vm.watchID = null;
                    }
                };
            };

            function onError(compassError) {
                alert('Compass error: ' + compassError.code);
            };

        }
    );
}());