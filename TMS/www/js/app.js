var app = angular.module('EventMob', [

  'ionic-datepicker',
  'jett.ionic.filter.bar',
  'ionic.ion.headerShrink',
  'ngCordova.plugins.sms',
  'ngCordova.plugins.actionSheet',
  'ionic',
  'ngCordova.plugins.dialogs',
  'ngCordova.plugins.toast',
  'ngCordova.plugins.appVersion',
  'ngCordova.plugins.file',
  'ngCordova.plugins.fileTransfer',
  'ngCordova.plugins.fileOpener2',
  'ngCordova.plugins.datePicker',
  'ngMessages',
  'EventMob.services',
  'EventMob.factories',
  'EventMob.config'
]);

app.run(['ENV', '$ionicPlatform', '$rootScope', '$state', '$location', '$timeout', '$ionicPopup',
  '$ionicHistory', '$ionicLoading', '$cordovaToast', '$cordovaFile', 'GEO_CONSTANT',
  function(ENV, $ionicPlatform, $rootScope, $state, $location, $timeout, $ionicPopup,
    $ionicHistory, $ionicLoading, $cordovaToast, $cordovaFile, GEO_CONSTANT) {
    $ionicPlatform.ready(function() {
      // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
      // for form inputs)
      if (window.cordova && window.cordova.plugins.Keyboard) {
        ENV.fromWeb = false;
        cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
        cordova.plugins.Keyboard.disableScroll(true);
        /*
        if(window.plugins.jPushPlugin){
            // Add JPush
            window.plugins.jPushPlugin.init();
            //window.plugins.jPushPlugin.setDebugMode(true);
            window.plugins.jPushPlugin.setLatestNotificationNum(5);
            //window.plugins.jPushPlugin.openNotificationInAndroidCallback = function(data);
            //window.plugins.jPushPlugin.receiveMessageInAndroidCallback = function(data);
        }
        */
        var data = 'website=' + ENV.website + '##api=' + ENV.api;
        var path = cordova.file.externalRootDirectory;
        var directory = ENV.rootPath;
        var file = directory + "/" + ENV.configFile;
        $cordovaFile.createDir(path, directory, false)
          .then(function(success) {
            $cordovaFile.writeFile(path, file, data, true)
              .then(function(success) {
                //
              }, function(error) {
                $cordovaToast.showShortBottom(error);
              });
          }, function(error) {
            // If an existing directory exists
            $cordovaFile.checkFile(path, file)
              .then(function(success) {
                $cordovaFile.readAsText(path, file)
                  .then(function(success) {
                    var arConf = success.split('##');
                    var arWebServiceURL = arConf[0].split('=');
                    if (is.not.empty(arWebServiceURL[1])) {
                      ENV.website = arWebServiceURL[1];
                    }
                    var arWebSiteURL = arConf[1].split('=');
                    if (is.not.empty(arWebSiteURL[1])) {
                      ENV.api = arWebSiteURL[1];
                    }

                  }, function(error) {
                    $cordovaToast.showShortBottom(error);
                  });
              }, function(error) {
                // If file not exists
                $cordovaFile.writeFile(path, file, data, true)
                  .then(function(success) {
                    //

                  }, function(error) {
                    $cordovaToast.showShortBottom(error);
                  });
              });
          });
      }
      if (window.StatusBar) {
        // org.apache.cordova.statusbar required
        StatusBar.styleDefault();
      }
    });

    $ionicPlatform.registerBackButtonAction(function(e) {
      e.preventDefault();
      // Is there a page to go back to?  $state.include ??
      if ($state.includes('index.main') || $state.includes('index.login') || $state.includes('loading')) {
        if ($rootScope.backButtonPressedOnceToExit) {
          ionic.Platform.exitApp();
        } else {
          $rootScope.backButtonPressedOnceToExit = true;
          $cordovaToast.showShortBottom('Press again to exit.');
          setTimeout(function() {
            $rootScope.backButtonPressedOnceToExit = false;
          }, 2000);
        }
      } else if ($state.includes('index.setting')) {
        if ($ionicHistory.backView()) {
          $ionicHistory.goBack();
        } else {
          $state.go('index.login', {
            'CheckUpdate': 'Y'
          }, {
            reload: true
          });
        }
      } else if ($state.includes('index.update') || $state.includes('listDirect')) {
        if ($ionicHistory.backView()) {
          $ionicHistory.goBack();
        } else {
          $state.go('index.login', {
            'CheckUpdate': 'N'
          }, {
            reload: true
          });
        }
      } else if (
        $state.includes('list')
      ) {
        $state.go('index.main', {
          'blnForcedReturn': 'Y'
        }, {
          reload: true
        });
      } else if ($ionicHistory.backView()) {
        $ionicHistory.goBack();
      } else {
        // This is the last page: Show confirmation popup
        $rootScope.backButtonPressedOnceToExit = true;
        $cordovaToast.showShortBottom('Press again to exit.');
        setTimeout(function() {
          $rootScope.backButtonPressedOnceToExit = false;
        }, 2000);
      }
      return false;
    }, 101);
    //GEO
    GEO_CONSTANT.init();
  }
]);
app.config(['$httpProvider', '$stateProvider', '$urlRouterProvider', '$ionicConfigProvider', '$ionicFilterBarConfigProvider',
  function($httpProvider, $stateProvider, $urlRouterProvider, $ionicConfigProvider, $ionicFilterBarConfigProvider) {
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
        cache: 'false',
        templateUrl: "view/main.html",
        controller: 'MainCtrl'
      })
      .state('list', {
        url: '/list/:JobNo',
        cache: 'false',
        templateUrl: 'view/list.html',
        controller: 'ListCtrl'
      })
      .state('listDirect', {
        url: '/list/:JobNo',
        cache: 'false',
        templateUrl: 'view/list.html',
        controller: 'ListCtrl'
      })
      .state('detail', {
        url: '/detail/:Type/:ContainerNo/:JobNo/:JobLineItemNo/:LineItemNo/:Description/:Remark/:DoneFlag',
        cache: 'false',
        templateUrl: 'view/detail.html',
        controller: 'DetailCtrl'
      });
    $urlRouterProvider.otherwise('/loading');
  }
]);

app.constant('$ionicLoadingConfig', {
  template: '<div class="loader"><svg class="circular"><circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"/></svg></div>'
});

// app.constant('ApiEndpoint', {
//     url: strWebServiceURL + "/" + strBaseUrl
// });
