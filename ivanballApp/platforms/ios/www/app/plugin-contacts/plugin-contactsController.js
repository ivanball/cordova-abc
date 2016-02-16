(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-contactsController",
        function () {
            var vm = this;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                // Empty
            };

            vm.createContact = function () {
                // create
                var contact = navigator.contacts.create();
                contact.displayName = "Plumber";
                contact.nickname = "Plumber";                 // specify both to support all devices
                var name = new ContactName();
                name.givenName = "Jane";
                name.familyName = "Doe";
                contact.name = name;

                // save
                contact.save(onSaveSuccess, onError);

                // clone
                var clone = contact.clone();
                clone.name.givenName = "John";
                alert("Original contact name = " + contact.name.givenName);
                alert("Cloned contact name = " + clone.name.givenName);

                // remove
                contact.remove(onRemoveSuccess, onError);
            };

            vm.findContacts = function () {
                // find all contacts with 'Bob' in any name field
                var options = new ContactFindOptions();
                options.filter = "Doe";
                options.multiple = true;
                var fields = ["displayName", "name"];
                navigator.contacts.find(fields, onFindSuccess, onError, options);
            };

            // onSuccess: Get a snapshot of the current contacts
            function onFindSuccess(contacts) {
                for (var i = 0; i < contacts.length; i++) {
                    alert("CONTACT" + "\n" +
                            "Display Name: " + contacts[i].displayName + "\n" +
                            "Formatted: " + contacts[i].name.formatted + "\n" +
                            "Family Name: " + contacts[i].name.familyName + "\n" +
                            "Given Name: " + contacts[i].name.givenName + "\n" +
                            "Middle Name: " + contacts[i].name.middleName + "\n" +
                            "Suffix: " + contacts[i].name.honorificSuffix + "\n" +
                            "Prefix: " + contacts[i].name.honorificPrefix);
                    if (contacts[i].addresses) {
                        for (var j = 0; j < contacts[i].addresses.length; j++) {
                            alert("CONTACT ADDRESS" + "\n" +
                                    "Pref: " + contacts[i].addresses[j].pref + "\n" +
                                    "Type: " + contacts[i].addresses[j].type + "\n" +
                                    "Formatted: " + contacts[i].addresses[j].formatted + "\n" +
                                    "Street Address: " + contacts[i].addresses[j].streetAddress + "\n" +
                                    "Locality: " + contacts[i].addresses[j].locality + "\n" +
                                    "Region: " + contacts[i].addresses[j].region + "\n" +
                                    "Postal Code: " + contacts[i].addresses[j].postalCode + "\n" +
                                    "Country: " + contacts[i].addresses[j].country);
                        }
                    }
                    if (contacts[i].phoneNumbers) {
                        for (var j = 0; j < contacts[i].phoneNumbers.length; j++) {
                            alert("CONTACT PHONE" + "\n" +
                                    "Type: " + contacts[i].phoneNumbers[j].type + "\n" +
                                    "Value: " + contacts[i].phoneNumbers[j].value + "\n" +
                                    "Preferred: " + contacts[i].phoneNumbers[j].pref);
                        }
                    }
                    if (contacts[i].organizations) {
                        for (var j = 0; j < contacts[i].organizations.length; j++) {
                            alert("CONTACT ORGANIZATION" + "\n" +
                                    "Pref: " + contacts[i].organizations[j].pref + "\n" +
                                    "Type: " + contacts[i].organizations[j].type + "\n" +
                                    "Name: " + contacts[i].organizations[j].name + "\n" +
                                    "Department: " + contacts[i].organizations[j].department + "\n" +
                                    "Title: " + contacts[i].organizations[j].title);
                        }
                    }
                }
            };

            // onSaveSuccess: Get a snapshot of the current contacts
            function onSaveSuccess(contact) {
                alert("Save Success");
            };

            // onRemoveSuccess: Get a snapshot of the current contacts
            function onRemoveSuccess(contacts) {
                alert("Removal Success");
            };

            // onError: Failed to get the contacts
            function onError(contactError) {
                alert("Error = " + contactError.code);
            };

        }
    );
}());