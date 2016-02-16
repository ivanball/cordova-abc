(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-geolocationController",
        function () {
            var vm = this;
            vm.watch = true;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                startWatch();
            };

            function startWatch() {
                //navigator.geolocation.getCurrentPosition(onSuccess, onError);

                //var options = { timeout: 30000 }; // Throw an error if no update is received every 30 seconds
                var options = { enableHighAccuracy: true }; // Get the most accurate position updates available on the device.
                vm.watchID = navigator.geolocation.watchPosition(onSuccess, onError, options);
            };

            // onSuccess Geolocation
            function onSuccess(position) {
                var element = document.getElementById('geolocation');
                element.innerHTML = 'Lat: ' + position.coords.latitude + ' Lon: ' + position.coords.longitude + '<br />' +
                                    'Alt: ' + position.coords.altitude + ' ' + 'Timestamp: ' + position.timestamp + '<br />' +
                                    'Heading: ' + position.coords.heading + ' ' + 'Speed: ' + position.coords.speed + '<br />' +
                                    'Accuracy: ' + position.coords.accuracy + ' ' + 'Alt Acc: ' + position.coords.altitudeAccuracy + '<br />';
                                    //'<hr />' + 
                                    //element.innerHTML;
            };

            vm.toggleWatch = function () {
                if (vm.watch) {
                    startWatch();
                }
                else {
                    if (vm.watchID != null) {
                        navigator.geolocation.clearWatch(vm.watchID);
                        vm.watchID = null;
                    }
                };
            };

            function onError(error) {
                alert('code: ' + error.code + '\n' +
                      'message: ' + error.message + '\n');
            };

        }
    );
}());