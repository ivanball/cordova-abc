(function () {
    "use strict";
    var ivanballApp = angular.module('ivanballApp', ['common.services', 'ui.router', 'ngResource']); //'ngRoute',

    ivanballApp.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', function ($stateProvider, $urlRouterProvider, $locationProvider) {
        //$urlRouterProvider.otherwise('/plugin-notification');
        $stateProvider.
            state("plugin-accelerometer", {
                url: '/plugin-accelerometer',
                templateUrl: 'app/plugin-accelerometer/plugin-accelerometer.html',
                controller: 'plugin-accelerometerController as vm'
            }).
            state("plugin-batterystatus", {
                url: '/plugin-batterystatus',
                templateUrl: 'app/plugin-batterystatus/plugin-batterystatus.html',
                controller: 'plugin-batterystatusController as vm'
            }).
            state("plugin-camera", {
                url: '/plugin-camera',
                templateUrl: 'app/plugin-camera/plugin-camera.html',
                controller: 'plugin-cameraController as vm'
            }).
            state("plugin-capture", {
                url: '/plugin-capture',
                templateUrl: 'app/plugin-capture/plugin-capture.html',
                controller: 'plugin-captureController as vm'
            }).
            state("plugin-compass", {
                url: '/plugin-compass',
                templateUrl: 'app/plugin-compass/plugin-compass.html',
                controller: 'plugin-compassController as vm'
            }).
            state("plugin-connection", {
                url: '/plugin-connection',
                templateUrl: 'app/plugin-connection/plugin-connection.html',
                controller: 'plugin-connectionController as vm'
            }).
            state("plugin-contacts", {
                url: '/plugin-contacts',
                templateUrl: 'app/plugin-contacts/plugin-contacts.html',
                controller: 'plugin-contactsController as vm'
            }).
            state("plugin-device", {
                url: '/plugin-device',
                templateUrl: 'app/plugin-device/plugin-device.html',
                controller: 'plugin-deviceController as vm'
            }).
            state("plugin-geolocation", {
                url: '/plugin-geolocation',
                templateUrl: 'app/plugin-geolocation/plugin-geolocation.html',
                controller: 'plugin-geolocationController as vm'
            }).
            state("plugin-notification", {
                url: '/plugin-notification',
                templateUrl: 'app/plugin-notification/plugin-notification.html',
                controller: 'plugin-notificationController as vm'
            }).
            state("hello-world", {
                url: '/hello-world',
                templateUrl: 'app/hello-world/hello-world.html',
                controller: 'hello-worldController as vm'
            });
        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    }]);
}());