(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-deviceController",
        function () {
            var vm = this;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                getDeviceProperties();
            };

            function getDeviceProperties() {
                var element = document.getElementById('deviceProperties');
                element.innerHTML = 'Device Name: ' + device.name + '<br />' +
                                    'Device Cordova: ' + device.cordova + '<br />' +
                                    'Device Platform: ' + device.platform + '<br />' +
                                    'Device UUID: ' + device.uuid + '<br />' +
                                    'Device Model: ' + device.model + '<br />' +
                                    'Device Version: ' + device.version + '<br />';
            };

        }
    );
}());