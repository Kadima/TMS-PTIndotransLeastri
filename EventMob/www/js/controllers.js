var appControllers = angular.module('EventMob.controllers', [
    'ionic',
    'ngCordova.plugins.dialogs',
    'ngCordova.plugins.toast',
    'ngCordova.plugins.appVersion',
    'ngCordova.plugins.file',
    'ngCordova.plugins.fileTransfer',
    'ngCordova.plugins.fileOpener2',
    'ngCordova.plugins.datePicker',
    'ngMessages',
    'EventMob.directives',
    'EventMob.services'
]);

appControllers.controller('LoadingCtrl',
        ['$state', '$timeout',
        function ($state, $timeout) {
            $timeout(function () {
                $state.go('login', { 'CheckUpdate': 'Y' }, { reload: true });
            }, 2500);
        }]);

appControllers.controller('LoginCtrl',
        ['$scope', '$http', '$state', '$stateParams', '$ionicPopup', '$timeout', '$ionicLoading', '$cordovaToast', '$cordovaAppVersion', 'JsonServiceClient', 
        function ($scope, $http, $state, $stateParams, $ionicPopup, $timeout, $ionicLoading, $cordovaToast, $cordovaAppVersion, JsonServiceClient) {
            $scope.logininfo = {};
            if (undefined == $scope.logininfo.strPhoneNumber) {
                $scope.logininfo.strPhoneNumber = "";
            }
            $('#iPhoneNumber').on('keydown', function (e) {
                if (e.which === 9 || e.which === 13) {
                    $scope.login();
                }
            });
            if ($stateParams.CheckUpdate === 'Y') {
                var url = strWebSiteURL + '/update.json';
                $http.get(url)
                    .success(function (res) {
                        var serverAppVersion = res.version;
                        $cordovaAppVersion.getVersionNumber().then(function (version) {
                            if (version != serverAppVersion) {
                                $state.go('update', { 'Version': serverAppVersion });
                            }
                        });
                    })
                    .error(function (res) {});
            }
            $scope.checkUpdate = function () {
                var url = strWebServiceURL + strBaseUrl + '/update.json';
                $http.get(url)
                    .success(function (res) {
                        var serverAppVersion = res.version;
                        $cordovaAppVersion.getVersionNumber().then(function (version) {
                            if (version != serverAppVersion) {
                                $state.go('update', { 'Version': serverAppVersion });
                            } else {
                                var alertPopup = $ionicPopup.alert({
                                    title: "Already the Latest Version!",
                                    okType: 'button-assertive'
                                });
                                $timeout(function () {
                                    alertPopup.close();
                                }, 2500);
                            }
                        });
                    })
                    .error(function (res) {
                        var alertPopup = $ionicPopup.alert({
                            title: "Connect Update Server Error!",
                            okType: 'button-assertive'
                        });
                        $timeout(function () {
                            alertPopup.close();
                        }, 2500);
                    });
            };
            $scope.setConf = function () {
                $state.go('setting', {}, { reload: true });
            };
            $scope.login = function () {
                if (window.cordova && window.cordova.plugins.Keyboard) {
                    cordova.plugins.Keyboard.close();
                }
                if ($scope.logininfo.strPhoneNumber == "") {
                    var alertPopup = $ionicPopup.alert({
                        title: 'Please Enter Phone Number.',
                        okType: 'button-assertive'
                    });
                    $timeout(function () {
                        alertPopup.close();
                    }, 2500);
                    return;
                }
                $ionicLoading.show();
                var jsonData = { "PhoneNumber": $scope.logininfo.strPhoneNumber };
                var strUri = "/api/event/action/list/login";
                var onSuccess = function (response) {
                    $ionicLoading.hide();
                    sessionStorage.clear();
                    sessionStorage.setItem("strPhoneNumber", $scope.logininfo.strPhoneNumber);
                    sessionStorage.setItem("strDriverName", response.data.results);
                    $state.go('main', { 'blnForcedReturn': 'N' }, { reload: true });
                };
                var onError = function () {
                    $ionicLoading.hide();
                };
                JsonServiceClient.postToService(strUri, jsonData, onSuccess, onError);
            };
        }]);

appControllers.controller('SettingCtrl',
        ['$scope', '$state', '$timeout', '$ionicLoading', '$ionicPopup', '$cordovaToast', '$cordovaFile',
        function ($scope, $state, $timeout, $ionicLoading, $ionicPopup, $cordovaToast, $cordovaFile) {
            $scope.Setting = {};
            $scope.Setting.WebServiceURL = strWebServiceURL.replace('http://', '');
            $scope.Setting.BaseUrl = strBaseUrl.replace('/', '');
            $scope.returnLogin = function () {
                $state.go('login', { 'CheckUpdate': 'Y' }, { reload: true });
            };
            $scope.saveSetting = function () {
                if ($scope.Setting.WebServiceURL.length > 0) {
                    strWebServiceURL = $scope.Setting.WebServiceURL;
                    if (strWebServiceURL.length > 0) {
                        strWebServiceURL = "http://" + strWebServiceURL;
                    }
                } else { $scope.Setting.WebServiceURL = strWebServiceURL }
                if ($scope.Setting.BaseUrl.length > 0) {
                    strBaseUrl = $scope.Setting.BaseUrl;
                    if (strBaseUrl.length > 0) {
                        strBaseUrl = "/" + strBaseUrl;
                    }
                } else { $scope.Setting.BaseUrl = strBaseUrl }
                if ($scope.Setting.WebSiteUrl.length > 0) {
                    strWebSiteURL = $scope.Setting.WebSiteUrl;
                    if (strWebSiteURL.length > 0) {
                        strWebSiteURL = "http://" + strWebSiteURL;
                    }
                } else { $scope.Setting.WebSiteUrl = strWebSiteURL }
                var data = 'BaseUrl=' + $scope.Setting.BaseUrl + '##WebServiceURL=' + $scope.Setting.WebServiceURL + '##WebSiteURL=' + strWebSiteURL;
                var path = cordova.file.externalRootDirectory;
                var directory = "TmsApp";
                var file = directory + "/Config.txt";
                $cordovaFile.writeFile(path, file, data, true)
                    .then(function (success) {
                        $state.go('login', { 'CheckUpdate': 'Y' }, { reload: true });
                    }, function (error) {
                        $cordovaToast.showShortBottom(error);
                    });
            };
            $scope.delSetting = function () {
                var path = cordova.file.externalRootDirectory;
                var directory = "TmsApp";
                var file = directory + "/Config.txt";
                $cordovaFile.removeFile(path, file)
                    .then(function (success) {
                        var alertPopup = $ionicPopup.alert({
                            title: 'Delete Config File Success.',
                            okType: 'button-calm'
                        });
                        $timeout(function () {
                            alertPopup.close();
                        }, 2500);
                    }, function (error) {
                        $cordovaToast.showShortBottom(error);
                    });
            };
        }]);

appControllers.controller('UpdateCtrl',
        ['$scope', '$stateParams', '$state', '$timeout', '$ionicLoading', '$cordovaToast', '$cordovaFile', '$cordovaFileTransfer', '$cordovaFileOpener2',
        function ($scope, $stateParams, $state, $timeout, $ionicLoading, $cordovaToast, $cordovaFile, $cordovaFileTransfer, $cordovaFileOpener2) {
            $scope.strVersion = $stateParams.Version;
            $scope.returnLogin = function () {
                $state.go('login', { 'CheckUpdate': 'N' }, { reload: true });
            };
            $scope.upgrade = function () {
                $ionicLoading.show({
                    template: "Download  0%"
                });
                var url = strWebServiceURL + strBaseUrl + "/TMS.apk";
                var blnError = false;
                $cordovaFile.checkFile(cordova.file.externalRootDirectory, "TMS.apk")
                .then(function (success) {
                    //
                }, function (error) {
                    blnError = true;
                });
                var targetPath = cordova.file.externalRootDirectory + "TMS.apk";
                var trustHosts = true;
                var options = {};
                if (!blnError) {
                    $cordovaFileTransfer.download(url, targetPath, options, trustHosts).then(function (result) {
                        $ionicLoading.hide();
                        $cordovaFileOpener2.open(targetPath, 'application/vnd.android.package-archive'
                        ).then(function () {
                            // success
                        }, function (err) {
                            // error
                        });
                    }, function (err) {
                        $cordovaToast.showShortCenter('Download faild.');
                        $ionicLoading.hide();
                        $state.go('login', { 'CheckUpdate': 'N' }, { reload: true });
                    }, function (progress) {
                        $timeout(function () {
                            var downloadProgress = (progress.loaded / progress.total) * 100;
                            $ionicLoading.show({
                                template: "Download  " + Math.floor(downloadProgress) + "%"
                            });
                            if (downloadProgress > 99) {
                                $ionicLoading.hide();
                            }
                        })
                    });
                } else {
                    $ionicLoading.hide();
                    $cordovaToast.showShortCenter('Check APK file faild.');
                    $state.go('login', { 'CheckUpdate': 'N' }, { reload: true });
                }
            };
        }]);

appControllers.controller('MainCtrl',
        ['$scope', '$http', '$state', '$stateParams', '$ionicPopup', '$timeout', 'JsonServiceClient',
        function ($scope, $http, $state, $stateParams, $ionicPopup, $timeout, JsonServiceClient) {
            var strDriverName = sessionStorage.getItem("strDriverName");
            var strPhoneNumber = sessionStorage.getItem("strPhoneNumber");
            if (strDriverName === null || strDriverName === "") {
                $scope.strDriverName = "Driver";
            } else {
                $scope.strDriverName = strDriverName;
            }
            $scope.strItemsCount = "loading...";
            $scope.showList = function (strJobNo) {
                $state.go('list', { 'JobNo': strJobNo }, { reload: true });
            };
            var strUri = "/api/event/action/list/jobno/";
            var onSuccess = function (response) {
                if (response.data.results.length === 1 && $stateParams.blnForcedReturn === 'N') {
                    $state.go('list', { 'JobNo': response.data.results[0].JobNo }, { reload: true });
                }
                $scope.Jobs = response.data.results;
            };
            var onError = function () {
            };
            JsonServiceClient.getFromService(strUri + strPhoneNumber, onSuccess, onError);
        }]);

appControllers.controller('ListCtrl',
        ['$scope', '$state', '$stateParams', '$http', '$ionicPopup', '$timeout', '$ionicLoading', '$cordovaDialogs', 'JsonServiceClient',
        function ($scope, $state, $stateParams, $http, $ionicPopup, $timeout, $ionicLoading, $cordovaDialogs, JsonServiceClient) {
            $scope.shouldShowDelete = false;
            $scope.listCanSwipe = true;
            $scope.JobNo = $stateParams.JobNo;
            var strPhoneNumber = sessionStorage.getItem("strPhoneNumber");
            var strJobNo = $scope.JobNo;
            var strUri = "/api/event/action/list/container/" + strPhoneNumber + "/" + strJobNo;
            var onSuccess = function (response) {
                $ionicLoading.hide();
                $scope.tasks = response.data.results;
                if (response.data.results.length == 0) {
                    var alertPopup = $ionicPopup.alert({
                        title: 'No Tasks.',
                        okType: 'button-calm'
                    });
                    $timeout(function () {
                        alertPopup.close();
                    }, 2500);
                }
            };
            var onError = function () {
                $ionicLoading.hide();
            };
            var onFinally = function () {
                $scope.$broadcast('scroll.refreshComplete');
            };
            var getTasks = function () {
                $ionicLoading.show();
                getData();
            };
            var getData = function () {                
                JsonServiceClient.getFromService(strUri, onSuccess, onError);
            };
            $scope.doRefresh = function () {
                JsonServiceClient.getFromService(strUri, onSuccess, onError, onFinally);
                $scope.$apply();
            };
            $scope.returnMain = function () {
                $state.go('main', { 'blnForcedReturn': 'Y' }, { reload: true });
            };
            $scope.showContainerNo = function (task) {
                var to = typeof (task.ContainerNo);
                var strUndefined = 'undefined';
                if (to === strUndefined) {
                    return false;
                } else {
                    if (to.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }                
            };
            var checkEventOrder = function (task) {
                for (var i = 0; i <= $scope.tasks.length - 1; i++) {
                    if ($scope.tasks[i].JobLineItemNo < task.JobLineItemNo && $scope.tasks[i].AllowSkipFlag != 'Y') {
                        return false;
                    }
                }
                return true;
            };
            $scope.slideDone = function (task, strDoneFlag) {
                if (strDoneFlag === 'N') {
                    $state.go('detail', { 'ContainerNo': task.ContainerNo, 'JobNo': task.JobNo, 'JobLineItemNo': task.JobLineItemNo, 'LineItemNo': task.LineItemNo, 'Description': task.Description, 'Remark': task.Remark, 'DoneFlag': strDoneFlag });
                } else {
                    if (checkEventOrder(task)) {
                        $state.go('detail', { 'ContainerNo': task.ContainerNo, 'JobNo': task.JobNo, 'JobLineItemNo': task.JobLineItemNo, 'LineItemNo': task.LineItemNo, 'Description': task.Description, 'Remark': task.Remark, 'DoneFlag': strDoneFlag });
                    } else {
                        var alertPopup = $ionicPopup.alert({
                            title: 'Previous event not Done.<br/>Not allow to do this one.',
                            okType: 'button-assertive'
                        });
                        $timeout(function () {
                            alertPopup.close();
                        }, 2500);
                    }
                }
            };
            getTasks();
        }]);

appControllers.controller('DetailCtrl',
        ['$scope', '$stateParams', '$state', '$http', '$timeout', '$ionicLoading', '$ionicPopup', 'JsonServiceClient',
        function ($scope, $stateParams, $state, $http, $timeout, $ionicLoading, $ionicPopup, JsonServiceClient) {
            $scope.strContainerNo = $stateParams.ContainerNo;
            $scope.strJobNo = $stateParams.JobNo;
            $scope.strJobLineItemNo = $stateParams.JobLineItemNo;
            $scope.strLineItemNo = $stateParams.LineItemNo;
            $scope.strDescription = $stateParams.Description;
            $scope.Update = {};
            $scope.Update.ContainerNo = $scope.strContainerNo;
            $scope.Update.remark = $stateParams.Remark;
            $scope.strDoneFlag = $stateParams.DoneFlag;
            if ($scope.strDoneFlag === 'N') {
                $scope.strDoneOrUpdateTitle = 'Update Remark';
                $scope.strDoneOrUpdate = 'Update';
            } else {
                $scope.strDoneOrUpdateTitle = 'Detail Infos';
                $scope.strDoneOrUpdate = 'Done';
            }
            var currentDate = new Date();
            $scope.Update.datetime = currentDate;
            $scope.Update.strDatetime = currentDate.getTime();
            $scope.returnList = function () {
                $state.go('list', { 'JobNo': $scope.strJobNo }, { reload: true });
            };
            $scope.update = function () {
                $ionicLoading.show();
                currentDate.setFullYear($scope.Update.datetime.getFullYear());
                currentDate.setMonth($scope.Update.datetime.getMonth());
                currentDate.setDate($scope.Update.datetime.getDate());
                currentDate.setHours($scope.Update.datetime.getHours());
                currentDate.setMinutes($scope.Update.datetime.getMinutes());
                var jsonData = { "JobNo": $scope.strJobNo, "JobLineItemNo": $scope.strJobLineItemNo, "LineItemNo": $scope.strLineItemNo, "DoneFlag": $scope.strDoneFlag, "DoneDatetime": currentDate, "Remark": $scope.Update.remark, "ContainerNo": $scope.Update.ContainerNo };
                var strUri = "/api/event/action/update/done";
                var onSuccess = function (response) {
                    $ionicLoading.hide();
                    $state.go('list', { 'JobNo': $scope.strJobNo }, { reload: true });
                };
                var onError = function () {
                    $ionicLoading.hide();
                    var alertPopup = $ionicPopup.alert({
                        title: 'Connect to WebService failed.',
                        okType: 'button-assertive'
                    });
                    $timeout(function () {
                        alertPopup.close();
                        $state.go('list', { 'JobNo': $scope.strJobNo }, { reload: true });
                    }, 2500);
                };
                JsonServiceClient.postToService(strUri, jsonData, onSuccess, onError);
            };
        }]);