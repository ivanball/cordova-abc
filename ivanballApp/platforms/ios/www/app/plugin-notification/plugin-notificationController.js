(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-notificationController", 
        function () {
            var vm = this;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                // Empty
            };

            function alertDismissed() {
                // do something
            };

            // Show a custom alertDismissed
            vm.showAlert = function () {
                navigator.notification.alert(
                    'You are the winner!',  // message
                    alertDismissed,         // callback
                    'Game Over',            // title
                    'Done'                  // buttonName
                );
            };

            function onConfirm(buttonIndex) {
                alert('You selected button ' + buttonIndex);
            };

            // Show a custom confirmation dialog
            vm.showConfirm = function () {
                navigator.notification.confirm(
                    'You are the winner!', // message
                     onConfirm,            // callback to invoke with index of button pressed
                    'Game Over',           // title
                    'Restart,Exit'         // buttonLabels
                );
            };

            function onPrompt(results) {
                alert("You selected button number " + results.buttonIndex + " and entered " + results.input1);
            };

            // Show a custom prompt dialog
            vm.showPrompt = function () {
                navigator.notification.prompt(
                    'Please enter your name',  // message
                    onPrompt,                  // callback to invoke
                    'Registration',            // title
                    ['Ok', 'Exit'],             // buttonLabels
                    'Jane Doe'                 // defaultText
                );
            };

            // Beep three times
            vm.playBeep = function () {
                navigator.notification.beep(3);
            };

            // Vibrate for 2 seconds
            vm.vibrate = function () {
                navigator.notification.vibrate(2000);
            };

        }
    );
}());