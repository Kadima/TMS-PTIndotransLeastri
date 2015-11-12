var app = angular.module('EventMob', [
    'ionic',
    'ngCordova.plugins.toast',
    'ngCordova.plugins.file',
    'EventMob.controllers'
]);
    
app.run(['$ionicPlatform', '$rootScope', '$state', '$location', '$timeout', '$ionicPopup', '$ionicHistory', '$ionicLoading', '$cordovaToast', '$cordovaFile',
    function ($ionicPlatform, $rootScope, $state, $location, $timeout, $ionicPopup, $ionicHistory, $ionicLoading, $cordovaToast, $cordovaFile) {
        $ionicPlatform.ready(function () {
            // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
            // for form inputs)
            if (window.cordova && window.cordova.plugins.Keyboard) {
                cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
                //
                var data = 'BaseUrl=' + strBaseUrl + '##WebServiceURL=' + strWebServiceURL + '##WebSiteURL=' + strWebSiteURL;
                var path = cordova.file.externalRootDirectory;
                var directory = "TmsApp";
                var file = directory + "/Config.txt";
                $cordovaFile.createDir(path, directory, false)
                    .then(function (success) {
                        $cordovaFile.writeFile(path, file, data, true)
                            .then(function (success) {
                                //
                                if (strBaseUrl.length > 0) {
                                    strBaseUrl = "/" + strBaseUrl;
                                }
                                if (strWebServiceURL.length > 0) {
                                    strWebServiceURL = "http://" + strWebServiceURL;
                                }
                                if (strWebSiteURL.length > 0) {
                                    strWebSiteURL = "http://" + strWebSiteURL;
                                }
                            }, function (error) {
                                $cordovaToast.showShortBottom(error);
                            });
                    }, function (error) {
                        // If an existing directory exists
                        $cordovaFile.checkFile(path, file)
                            .then(function (success) {
                                $cordovaFile.readAsText(path, file)
                                    .then(function (success) {
                                        var arConf = success.split("##");
                                        var arBaseUrl = arConf[0].split("=");
                                        if (arBaseUrl[1].length > 0) {
                                            strBaseUrl = arBaseUrl[1];
                                        }
                                        var arWebServiceURL = arConf[1].split("=");
                                        if (arWebServiceURL[1].length > 0) {
                                            strWebServiceURL = arWebServiceURL[1];
                                        }
                                        var arWebSiteURL = arConf[2].split("=");
                                        if (arWebSiteURL[1].length > 0) {
                                            strWebSiteURL = arWebSiteURL[1];
                                        }
                                        //
                                        if (strBaseUrl.length > 0) {
                                            strBaseUrl = "/" + strBaseUrl;
                                        }
                                        if (strWebServiceURL.length > 0) {
                                            strWebServiceURL = "http://" + strWebServiceURL;
                                        }
                                        if (strWebSiteURL.length > 0) {
                                            strWebSiteURL = "http://" + strWebSiteURL;
                                        }
                                    }, function (error) {
                                        $cordovaToast.showShortBottom(error);
                                    });
                            }, function (error) {
                                // If file not exists
                                $cordovaFile.writeFile(path, file, data, true)
                                    .then(function (success) {
                                        //
                                        if (strBaseUrl.length > 0) {
                                            strBaseUrl = "/" + strBaseUrl;
                                        }
                                        if (strWebServiceURL.length > 0) {
                                            strWebServiceURL = "http://" + strWebServiceURL;
                                        }
                                        if (strWebSiteURL.length > 0) {
                                            strWebSiteURL = "http://" + strWebSiteURL;
                                        }
                                    }, function (error) {
                                        $cordovaToast.showShortBottom(error);
                                    });
                            });
                    });
            } else {
                if (strBaseUrl.length > 0) {
                    strBaseUrl = "/" + strBaseUrl;
                }
                if (strWebServiceURL.length > 0) {
                    strWebServiceURL = "http://" + strWebServiceURL;
                }
                if (strWebSiteURL.length > 0) {
                    strWebSiteURL = "http://" + strWebSiteURL;
                }
            }
            if (window.StatusBar) {
                // org.apache.cordova.statusbar required
                StatusBar.styleDefault();
            }
        });
        $ionicPlatform.registerBackButtonAction(function (e) {
            e.preventDefault();
            // Is there a page to go back to?  $state.include ??
            if ($state.includes('main') || $state.includes('login') || $state.includes('loading')) {
                if ($rootScope.backButtonPressedOnceToExit) {
                    ionic.Platform.exitApp();
                } else {
                    $rootScope.backButtonPressedOnceToExit = true;
                    $cordovaToast.showShortBottom('Press again to exit.');
                    setTimeout(function () {
                        $rootScope.backButtonPressedOnceToExit = false;
                    }, 2000);
                }
            } else if ($state.includes('setting')) {
                $state.go('login', { 'CheckUpdate': 'Y' }, { reload: true });
            } else if ($state.includes('update') || $state.includes('listDirect')) {
                $state.go('login', { 'CheckUpdate': 'N' }, { reload: true });
            } else if ($state.includes('list')) {
                $state.go('main', { 'blnForcedReturn': 'Y' }, { reload: true });
            } else if ($ionicHistory.backView()) {
                $ionicHistory.goBack();
            } else {
                // This is the last page: Show confirmation popup
                $rootScope.backButtonPressedOnceToExit = true;
                $cordovaToast.showShortBottom('Press again to exit.');
                setTimeout(function () {
                    $rootScope.backButtonPressedOnceToExit = false;
                }, 2000);
            }
            return false;
        }, 101);
    }]);

app.config(['$stateProvider', '$urlRouterProvider', '$ionicConfigProvider',
    function ($stateProvider, $urlRouterProvider, $ionicConfigProvider) {
        $ionicConfigProvider.backButton.previousTitleText(false);
        $stateProvider
            .state('loading', {
                url: '/loading',
                cache: 'false',
                templateUrl: 'view/loading.html',
                controller: 'LoadingCtrl'
            })
            .state('login', {
                url: '/login/:CheckUpdate',
                cache: 'false',
                templateUrl: 'view/login.html',
                controller: 'LoginCtrl'
            })
            .state('setting', {
                url: '/setting',
                //cache: 'false',
                templateUrl: 'view/setting.html',
                controller: 'SettingCtrl'
            })
            .state('update', {
                url: '/update/:Version',
                cache: 'false',
                templateUrl: 'view/update.html',
                controller: 'UpdateCtrl'
            })
            .state('main', {
                url: "/main/:blnForcedReturn",
                //cache: 'false',
                templateUrl: "view/main.html",
                controller: 'MainCtrl'
            })
            .state('list', {
                url: '/list/:JobNo',
                //cache: 'false',
                templateUrl: 'view/list.html',
                controller: 'ListCtrl'
            })
            .state('listDirect', {
                url: '/list/:JobNo',
                //cache: 'false',
                templateUrl: 'view/list.html',
                controller: 'ListCtrl'
            })
            .state('detail', {
                url: '/detail/:ContainerNo/:JobNo/:JobLineItemNo/:LineItemNo/:Description/:Remark/:DoneFlag',
                //cache: 'false',
                templateUrl: 'view/detail.html',
                controller: 'DetailCtrl'
            });
        $urlRouterProvider.otherwise('/loading');
    }]);

app.constant('$ionicLoadingConfig', {
    template: 'Loading...'
});

app.constant('ApiEndpoint', {
    url: strWebServiceURL + "/" + strBaseUrl
});