(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-accelerometerController",
        function () {
            var vm = this;
            vm.watch = true;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                startWatch();
            };

            function startWatch() {
                //navigator.accelerometer.getCurrentAcceleration(onSuccess, onError);

                var options = { frequency: 3000 }; // Update acceleration every 3 seconds
                vm.watchID = navigator.accelerometer.watchAcceleration(onSuccess, onError, options);
            };

            // onSuccess: Get a snapshot of the current acceleration
            function onSuccess(acceleration) {
                var element = document.getElementById('accelerometer');
                element.innerHTML = 'Acceleration X: ' + acceleration.x + '<br />' +
                                    'Acceleration Y: ' + acceleration.y + '<br />' +
                                    'Acceleration Z: ' + acceleration.z + '<br />' +
                                    'Timestamp: ' + acceleration.timestamp + '<br />';
            };

            vm.toggleWatch = function () {
                if (vm.watch) {
                    startWatch();
                }
                else {
                    if (vm.watchID != null) {
                        navigator.accelerometer.clearWatch(vm.watchID);
                        vm.watchID = null;
                    }
                };
            };

            function onError() {
                alert('onError!');
            };

        }
    );
}());