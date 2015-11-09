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
            $scope.logininfo = {
                strPhoneNumber: '',
                strCustomerCode: '',
                strJobNo: '',
                strRole: '',
                CurRole: '1',
                NewRole: '1'
            };
            $scope.roles = [
                { text: 'Driver/Ops', value: '1' },
                { text: 'Customer', value: '2' },
                { text: 'Transporter', value: '3' }
            ];
            $scope.funcChangeRole = function () {
                var myPopup = $ionicPopup.show({
                    template: '<ion-radio ng-repeat="role in roles" ng-value="role.value" ng-model="logininfo.NewRole">{{ role.text }}</ion-radio>',
                    title: 'Select Login Role',
                    scope: $scope,
                    buttons: [
                        {
                            text: 'Cancel',
                            onTap: function (e) {
                                $scope.logininfo.NewRole = $scope.logininfo.CurRole;
                            }
                        },
                        {
                            text: 'Save',
                            type: 'button-positive',
                            onTap: function (e) {
                                for (var r in $scope.roles) {
                                    if ($scope.logininfo.NewRole === $scope.roles[r].value) {
                                        $scope.logininfo.CurRole = $scope.logininfo.NewRole;
                                        $scope.logininfo.strRole = $scope.roles[r].text;
                                        if (window.cordova) {
                                            var data = 'BaseUrl=' + strBaseUrl + '##WebServiceURL=' + strWebServiceURL + '##WebSiteURL=' + strWebSiteURL + '##LoginRole=' + $scope.logininfo.strRole;
                                            var path = cordova.file.externalRootDirectory;
                                            var directory = "TmsApp";
                                            var file = directory + "/Config.txt";
                                            $cordovaFile.writeFile(path, file, data, true)
                                                .then(function (success) {
                                                    //
                                                }, function (error) {
                                                    $cordovaToast.showShortBottom(error);
                                                });
                                        }
                                    }
                                }
                            }
                        }
                    ]
                });
            };
            $scope.funcRoleJuage = function (roleType) {
                if (roleType === 1) {
                    if ($scope.logininfo.strRole === 'Driver/Ops') {
                        return true;
                    } else {
                        return false;
                    }
                }
                else if (roleType === 2) {
                    if ($scope.logininfo.strRole === 'Customer') {
                        return true;
                    } 
                    else if ($scope.logininfo.strRole === 'Transporter') {
                        return true;
                    } else {
                        return false;
                    }
                }
            };
            $scope.funcCheckUpdate = function () {
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
            $scope.funcSetConf = function () {
                $state.go('setting', {}, { reload: true });
            };
            $scope.funcLogin = function () {
                if (window.cordova && window.cordova.plugins.Keyboard) {
                    cordova.plugins.Keyboard.close();
                }
                var jsonData = { };
                var strUri = '';
                var onSuccess = null;
                var onError = function () {
                    $ionicLoading.hide();
                };
                if ($scope.logininfo.CurRole === '1') {
                    if ($scope.logininfo.strPhoneNumber === '') {
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
                    jsonData = { 'PhoneNumber': $scope.logininfo.strPhoneNumber, 'CustomerCode': '', 'JobNo': '' };
                    strUri = '/api/event/action/list/login';
                    onSuccess = function (response) {
                        $ionicLoading.hide();
                        sessionStorage.clear();
                        sessionStorage.setItem('strPhoneNumber', $scope.logininfo.strPhoneNumber);
                        sessionStorage.setItem('strDriverName', response.data.results);
                        sessionStorage.setItem('strCustomerCode', '');
                        sessionStorage.setItem('strJobNo', '');
                        sessionStorage.setItem('strRole', $scope.logininfo.strRole);
                        $state.go('main', { 'blnForcedReturn': 'N' }, { reload: true });
                    };
                } else {
                    if ($scope.logininfo.strCustomerCode === '') {
                        var alertPopup = $ionicPopup.alert({
                            title: 'Please Enter User ID.',
                            okType: 'button-assertive'
                        });
                        $timeout(function () {
                            alertPopup.close();
                        }, 2500);
                        return;
                    }
                    if ($scope.logininfo.strJobNo === '') {
                        var alertPopup = $ionicPopup.alert({
                            title: 'Please Enter Event Job No.',
                            okType: 'button-assertive'
                        });
                        $timeout(function () {
                            alertPopup.close();
                        }, 2500);
                        return;
                    }
                    $ionicLoading.show();
                    if ($scope.logininfo.CurRole === '2' || $scope.logininfo.CurRole === '3') {
                        jsonData = { 'PhoneNumber': '', 'CustomerCode': $scope.logininfo.strCustomerCode, 'JobNo': $scope.logininfo.strJobNo };
                        strUri = '/api/event/action/list/login';
                        onSuccess = function (response) {
                            $ionicLoading.hide();
                            sessionStorage.clear();
                            sessionStorage.setItem('strPhoneNumber', '');
                            sessionStorage.setItem('strDriverName', '');
                            sessionStorage.setItem('strCustomerCode', $scope.logininfo.strCustomerCode);
                            sessionStorage.setItem('strJobNo', $scope.logininfo.strJobNo);
                            sessionStorage.setItem('strRole', $scope.logininfo.strRole);                            
                            $state.go('list', { 'JobNo': $scope.logininfo.strJobNo }, { reload: true });
                        };
                    }                   
                }
                JsonServiceClient.postToService(strUri, jsonData, onSuccess, onError);
            };
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
                    .error(function (res) { });
            }
            if (window.cordova) {
                $cordovaFile.checkFile(path, file)
                    .then(function (success) {
                        $cordovaFile.readAsText(path, file)
                            .then(function (success) {
                                var arConf = success.split("##");
                                var arRole = arConf[3].split("=");
                                if (arRole[1].length > 0) {
                                    $scope.logininfo.strRole = arRole[1];
                                }
                            }, function (error) {
                                $cordovaToast.showShortBottom(error);
                            });
                    }, function (error) {
                        // If file not exists
                    });
            } else {
                $scope.logininfo.strRole = 'Driver/Ops';
            }
        }]);

appControllers.controller('SettingCtrl',
        ['$scope', '$state', '$timeout', '$ionicLoading', '$ionicPopup', '$cordovaToast', '$cordovaFile',
        function ($scope, $state, $timeout, $ionicLoading, $ionicPopup, $cordovaToast, $cordovaFile) {
            $scope.Setting = {};
            $scope.Setting.WebServiceURL = strWebServiceURL.replace('http://', '');
            $scope.Setting.BaseUrl = strBaseUrl.replace('/', '');
            $scope.Setting.WebSiteUrl = strWebSiteURL.replace('http://', '');
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
        ['$scope', '$http', '$state', '$stateParams', '$ionicLoading', '$ionicPopup', '$timeout', 'JsonServiceClient',
        function ($scope, $http, $state, $stateParams, $ionicLoading, $ionicPopup, $timeout, JsonServiceClient) {
            var strDriverName = sessionStorage.getItem("strDriverName").toString();
            var strPhoneNumber = sessionStorage.getItem("strPhoneNumber").toString();
            if (strDriverName.length > 0) {
                $scope.strName = strDriverName;
            } else {
                $scope.strName = "Driver";
            }
            $scope.strItemsCount = "loading...";
            $scope.showList = function (strJobNo) {
                $state.go('list', { 'JobNo': strJobNo }, { reload: true });
            };
            var funcShowList = function () {
                $ionicLoading.show();
                var strUri = '/api/event/action/list/jobno/';
                var onSuccess = function (response) {
                        if (response.data.results.length === 1 && $stateParams.blnForcedReturn === 'N') {
                            $state.go('list', { 'JobNo': response.data.results[0].JobNo }, { reload: true });
                        }
                        $scope.Jobs = response.data.results;
                    };
                var onError = function () {
                    $ionicLoading.hide();
                };
                JsonServiceClient.getFromService(strUri + strPhoneNumber, onSuccess, onError);
            };
            funcShowList();
        }]);

appControllers.controller('ListCtrl',
        ['$scope', '$state', '$stateParams', '$http', '$ionicPopup', '$timeout', '$ionicLoading', '$cordovaDialogs', 'JsonServiceClient',
        function ($scope, $state, $stateParams, $http, $ionicPopup, $timeout, $ionicLoading, $cordovaDialogs, JsonServiceClient) {       
            $scope.shouldShowDelete = false;
            $scope.listCanSwipe = true;
            $scope.JobNo = $stateParams.JobNo;
            var strJobNo = $scope.JobNo;
            var strPhoneNumber = sessionStorage.getItem("strPhoneNumber").toString();
            var strCustomerCode = sessionStorage.getItem("strCustomerCode").toString();
            //var strJobNo = sessionStorage.getItem("strJobNo").toString();
            var strRole = sessionStorage.getItem("strRole").toString();
            $scope.returnMain = function () {
                $state.go('main', { 'blnForcedReturn': 'Y' }, { reload: true });
            };
            $scope.funcShowRole = function (roleType) {
                if (roleType === 1) {
                    if (strRole === 'Driver/Ops') {
                        return true;
                    } else {
                        return false;
                    }
                }
                else if (roleType === 2) {
                    if (strRole === 'Customer') {
                        return true;
                    } else {
                        return false;
                    }
                } 
                else if (roleType === 3) {
                    if (strRole === 'Transporter') {
                        return true;
                    } else {
                        return false;
                    }
                }
            };
            $scope.funcShowTruckType = function (task) {
                if (task.JobType === 'IM') {
                    return 'In';
                } else if (task.JobType === 'EX' || task.JobType === 'TP') {
                    return 'Out';
                } else {
                    return '';
                }
            };
            $scope.funcShowTruckDatetime = function (utc) {
                var utcDate = Number(utc.substring(utc.indexOf('(') + 1, utc.lastIndexOf('-')));
                var newDate = new Date(utcDate);
                if (newDate.getUTCFullYear() < 2166 && newDate.getUTCFullYear() > 1899) {
                    return newDate.Format('yyyy-MM-dd hh:mm');
                } else {
                    return '';
                }
            };
            var strUri = '';
            var onSuccess = null;
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
            if (strCustomerCode.length > 0) {
                strUri = "/api/event/action/list/jmjm6/" + strJobNo;
                onSuccess = function (response) {
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
            } else {
                strUri = "/api/event/action/list/container/" + strPhoneNumber + "/" + strJobNo;
                onSuccess = function (response) {
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
            }
            $scope.doRefresh = function () {
                JsonServiceClient.getFromService(strUri, onSuccess, onError, onFinally);
                $scope.$apply();
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
            //
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