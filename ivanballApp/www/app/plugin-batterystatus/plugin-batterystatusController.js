(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-batterystatusController",
        function () {
            var vm = this;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                window.addEventListener("batterystatus", onBatteryStatus, false);
                window.addEventListener("batterycritical", onBatteryCritical, false);
                window.addEventListener("batterylow", onBatteryLow, false);
            };

            function onBatteryStatus(info) {
                var element = document.getElementById('batterystatus');
                element.innerHTML = "Level: " + info.level + " isPlugged: " + info.isPlugged;
            };

            function onBatteryCritical(info) {
                alert("Battery Level Critical " + info.level + "%\nRecharge Soon!");
            };

            function onBatteryLow(info) {
                alert("Battery Level Low " + info.level + "%");
            };
        }
    );
}());