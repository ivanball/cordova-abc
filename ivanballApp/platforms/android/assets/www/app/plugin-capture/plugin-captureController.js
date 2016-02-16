(function () {
    "use strict";
    var ivanballApp = angular.module("ivanballApp");

    ivanballApp.controller("plugin-captureController",
        function () {
            var vm = this;
            var downloadDirectory;
            //var fileSystem;
            //var localPath;

            // Wait for device API libraries to load
            document.addEventListener("deviceready", onDeviceReady, false);

            function onDeviceReady() {
                downloadDirectory = cordova.file.externalDataDirectory;
                if (!downloadDirectory) downloadDirectory = cordova.file.dataDirectory;
                ////request a file system	
                //window.requestFileSystem(LocalFileSystem.PERSISTENT, 0,
                //    function (fs) {
                //        fileSystem = fs;
                //        fileSystem.root.getDirectory("Download", { create: true, exclusive: false }, function (dirEntry) {
                //            dirEntry.getFile("myfile.mp3", { create: true, exclusive: false }, function (fileEntry) {
                //                localPath = fileEntry.fullPath;
                //                if (device.platform === "Android" && localPath.indexOf("file://") === 0) {
                //                    localPath = localPath.substring(7);
                //                }
                //            }, function (getFileError) {
                //                // getFile Error
                //                alert('failed on getFile');
                //            })
                //        }, function (getDirectoryError) {
                //            // getDirectory Error
                //            alert('failed on getDirectoryError');
                //        })
                //    }, function (error) {
                //        alert('failed to get fs');
                //        alert(JSON.stringify(error));
                //    });
            };

            // Called when capture operation is finished
            function captureSuccess(mediaFiles) {
                var i, len;
                for (i = 0, len = mediaFiles.length; i < len; i += 1) {
                    uploadFile(mediaFiles[i]);
                }
            };

            // Called if something bad happens.
            function captureError(error) {
                var msg = 'An error occurred during capture: ' + error.code;
                navigator.notification.alert(msg, null, 'Uh oh!');
            };

            // A button will call this function
            vm.captureAudio = function () {
                // Launch device audio recording application, allowing user to capture up to 2 audio clips
                navigator.device.capture.captureAudio(captureSuccess, captureError, { limit: 2 });
            };

            // A button will call this function
            vm.captureImage = function () {
                // Launch device camera application, allowing user to capture up to 2 images
                navigator.device.capture.captureImage(captureSuccess, captureError, { limit: 2 });
            };

            // A button will call this function
            vm.captureVideo = function () {
                // Launch device video recording application, allowing user to capture up to 2 video clips
                navigator.device.capture.captureVideo(captureSuccess, captureError, { limit: 2 });
            };

            // A button will call this function
            vm.downloadFile = function () {
                var sourceFile = encodeURI("http://archive.org/download/Kansas_Joe_Memphis_Minnie-When_Levee_Breaks/Kansas_Joe_and_Memphis_Minnie-When_the_Levee_Breaks.mp3");
                var targetFile = downloadDirectory + sourceFile.substr(sourceFile.lastIndexOf("/"));

                var ft = new FileTransfer();
                ft.onprogress = function (progressEvent) {
                    var element = document.getElementById('ft-status');
                    if (progressEvent.lengthComputable) {
                        var perc = Math.floor(progressEvent.loaded / progressEvent.total * 100);
                        document.getElementById("ft-prog").value = perc;
                        element.innerHTML = perc + "% downloaded... (" + sourceFile + ")";
                    } else {
                        if (element.innerHTML == "") {
                            element.innerHTML = "Downloading " + sourceFile;
                        } else {
                            element.innerHTML += ".";
                        }
                    }
                };

                ft.download(
                    sourceFile,
                    targetFile,
                    downloadSuccess,
                    downloadError,
                    false,
                    {
                        headers: {
                            "Authorization": "Basic dGVzdHVzZXJuYW1lOnRlc3RwYXNzd29yZA=="
                        }
                    }
                );
            };

            function downloadSuccess(entry) {
                document.getElementById("ft-prog").value = 100;
                var element = document.getElementById('ft-success');
                element.innerHTML += "File Name = " + entry.toURL() + '<br />' +
                                     "Full Path = " + entry.fullPath + '<br />';
                var media = new Media(entry.fullPath, null, function (e) { alert(JSON.stringify(e)); });
                media.play();
            }

            function downloadError(error) {
                alert("An error has occurred: Code = " + error.code);
                console.log("Upload error source " + error.source);
                console.log("Upload error target " + error.target);
            }

            function uploadSuccess(entry) {
                document.getElementById("ft-prog").value = 100;
                var element = document.getElementById('ft-success');
                element.innerHTML += "File Name = " + entry.response.substr(entry.response.indexOf("["), entry.response.indexOf("]") - entry.response.indexOf("[") + 1) + '<br />' +
                                     "Bytes Sent = " + entry.bytesSent + '<br />';
            }

            function uploadError(error) {
                alert("An error has occurred: Code = " + error.code);
                console.log("Upload error source " + error.source);
                console.log("Upload error target " + error.target);
            }

            // Upload files to server
            function uploadFile(mediaFile) {

                var uri = encodeURI("http://www.ivanball.com/Web/api/FileTransfer/UploadFile");

                var options = new FileUploadOptions();
                //options.fileKey = "file";
                options.fileName = mediaFile.name;
                //options.mimeType = "text/plain";
                //options.chunkedMode = false;

                //var headers = { 'headerParam': 'headerValue' };
                //options.headers = headers;

                var ft = new FileTransfer();
                ft.onprogress = function (progressEvent) {
                    var element = document.getElementById('ft-status');
                    if (progressEvent.lengthComputable) {
                        var perc = Math.floor(progressEvent.loaded / progressEvent.total * 100);
                        document.getElementById("ft-prog").value = perc;
                        element.innerHTML = perc + "% uploaded... (" + mediaFile.name + ")";
                    } else {
                        if (element.innerHTML == "") {
                            element.innerHTML = "Uploading " + mediaFile.name;
                        } else {
                            element.innerHTML += ".";
                        }
                    }
                };
                ft.upload(mediaFile.fullPath, uri, uploadSuccess, uploadError, options, true);
            };

        }
    );
}());