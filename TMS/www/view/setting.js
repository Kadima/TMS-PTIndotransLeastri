'use strict';
  app.controller('SettingCtrl', ['ENV', '$scope', '$state', '$ionicHistory', '$ionicPopup', '$cordovaToast', '$cordovaFile',
    function(ENV, $scope, $state, $ionicHistory, $ionicPopup, $cordovaToast, $cordovaFile) {
      $scope.Setting = {
        Version: ENV.version,
        WebApiURL: ENV.api.replace('http://', ''),
        WebSiteUrl: ENV.website.replace('http://', ''),
        };
      $scope.returnLogin = function() {
        // if ($ionicHistory.backView()) {
        //     $ionicHistory.goBack();
        // }else{
        $state.go('login', {
          'CheckUpdate': 'Y'
        }, {
          reload: true
        });
        // }
      };
      $scope.saveSetting = function() {
        if (is.not.empty($scope.Setting.WebApiURL)) {
          ENV.api = onStrToURL($scope.Setting.WebApiURL);
        } else {
          $scope.Setting.WebApiURL = ENV.website.replace('http://', '');
        }
        if (is.not.empty($scope.Setting.WebSiteUrl)) {
          ENV.website = onStrToURL($scope.Setting.WebSiteUrl);
        } else {
          $scope.Setting.WebSiteUrl = ENV.api.replace('http://', '');
        }
              if (!ENV.fromWeb) {
          // var data = 'website=' + ENV.website + '##api=' + ENV.api + '##map=' + ENV.mapProvider;
          var data = 'website=' + ENV.website + '##api=' + ENV.api;
          var path = cordova.file.externalRootDirectory;
          var file = ENV.rootPath + '/' + ENV.configFile;
          $cordovaFile.writeFile(path, file, data, true)
            .then(function(success) {
              $state.go('index.login', {}, {
                reload: true
              });
            }, function(error) {
              $cordovaToast.showShortBottom(error);
            });
        } else {
          $state.go('login', {}, {
            reload: true
          });
        }
      };

      $scope.delSetting = function () {
          var path = cordova.file.externalRootDirectory;
          var directory = ENV.rootPath;
          var file = directory + "/" + ENV.configFile;
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

    }
  ]);
