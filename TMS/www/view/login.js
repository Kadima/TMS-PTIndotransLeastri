'use strict';
app.controller('LoginCtrl', ['ENV', '$scope', '$rootScope', '$http', '$state', '$stateParams', '$ionicPopup', '$timeout', '$ionicLoading', '$cordovaToast',
  '$cordovaAppVersion', 'WebApiService', '$cordovaFile', 'SALES_ORM',
  function(ENV, $scope, $rootScope, $http, $state, $stateParams, $ionicPopup, $timeout, $ionicLoading, $cordovaToast,
    $cordovaAppVersion, WebApiService, $cordovaFile, SALES_ORM) {
    var path = '';
    var directory = ENV.rootPath;
    var file = directory + "/" + ENV.configFile;
    $scope.logininfo = {
      strPhoneNumber: '',
      strCustomerCode: '',
      strJobNo: '',
      strRole: '',
      CurRole: '1',
      NewRole: '1'
    };
    $scope.roles = [{
      text: 'Driver/Ops',
      value: '1'
    }, {
      text: 'Customer',
      value: '2'
    }, {
      text: 'Transporter',
      value: '3'
    }];
    $scope.funcChangeRole = function() {
      var myPopup = $ionicPopup.show({
        template: '<ion-radio ng-repeat="role in roles" ng-value="role.value" ng-model="logininfo.NewRole">{{ role.text }}</ion-radio>',
        title: 'Select Login Role',
        scope: $scope,
        buttons: [{
          text: 'Cancel',
          onTap: function(e) {
            $scope.logininfo.NewRole = $scope.logininfo.CurRole;
          }
        }, {
          text: 'Save',
          type: 'button-positive',
          onTap: function(e) {
            for (var r in $scope.roles) {
              if ($scope.logininfo.NewRole === $scope.roles[r].value) {
                $scope.logininfo.CurRole = $scope.logininfo.NewRole;
                $scope.logininfo.strRole = $scope.roles[r].text;
                if (window.cordova) {
                  path = cordova.file.externalRootDirectory;
                  var data = 'website=' + ENV.website.replace('http://', '') + '##api=' + ENV.api.replace('http://', '') + '##role=' + $scope.logininfo.strRole;
                  $cordovaFile.writeFile(path, file, data, true)
                    .then(function(success) {
                      //
                    }, function(error) {
                      $cordovaToast.showShortBottom(error);
                    });
                }
              }
            }
          }
        }]


      });
    };
    $scope.funcRoleJuage = function(roleType) {
      if (roleType === 1) {
        if ($scope.logininfo.strRole === 'Driver/Ops') {
          return true;
        } else {
          return false;
        }
      } else if (roleType === 2) {
        if ($scope.logininfo.strRole === 'Customer') {
          return true;
        } else if ($scope.logininfo.strRole === 'Transporter') {
          return true;
        } else {
          return false;
        }
      }
    };
    $scope.funcCheckUpdate = function() {
      var url = ENV.website + '/update.json';
      $http.get(url)
        .success(function(res) {
          var serverAppVersion = res.version;
          $cordovaAppVersion.getVersionNumber().then(function(version) {
            if (version != serverAppVersion) {
              $state.go('update', {
                'Version': serverAppVersion
              });
            } else {
              var alertPopup = $ionicPopup.alert({
                title: "Already the Latest Version!",
                okType: 'button-assertive'
              });
              $timeout(function() {
                alertPopup.close();
              }, 2500);
            }
          });
        })
        .error(function(res) {
          var alertPopup = $ionicPopup.alert({
            title: "Connect Update Server Error!",
            okType: 'button-assertive'
          });
          $timeout(function() {
            alertPopup.close();
          }, 2500);
        });
    };
    $scope.funcSetConf = function() {
      $state.go('setting', {}, {
        reload: true
      });
    };

    $scope.funcLogin = function() {
      if (window.cordova && window.cordova.plugins.Keyboard) {
        cordova.plugins.Keyboard.close();
      }
      var jsonData = {};
      var strUri = '';
      var onSuccess = null;
      var onError = function() {
        $ionicLoading.hide();
      };
      if ($scope.logininfo.CurRole === '1') {
        if ($scope.logininfo.strPhoneNumber === '') {
          var alertPopup = $ionicPopup.alert({
            title: 'Please Enter Phone Number.',
            okType: 'button-assertive'
          });
          $timeout(function() {
            alertPopup.close();
          }, 2500);
          return;
        }
        $ionicLoading.show();
        strUri = '/api/event/login/check?PhoneNumber=' + $scope.logininfo.strPhoneNumber;
        WebApiService.GetParam(strUri, true).then(function success(result) {
          console.log(result);
          $ionicLoading.hide();
          sessionStorage.clear();
          sessionStorage.setItem('strPhoneNumber', $scope.logininfo.strPhoneNumber);
          sessionStorage.setItem('strDriverName', result.data.results);
          sessionStorage.setItem('strCustomerCode', '');
          sessionStorage.setItem('strJobNo', '');
          sessionStorage.setItem('strRole', $scope.logininfo.strRole);
          $state.go('main', {
            'blnForcedReturn': 'N'
          }, {
            reload: true
          });
        });
      } else {
        if ($scope.logininfo.strCustomerCode === '') {
          var alertPopup = $ionicPopup.alert({
            title: 'Please Enter User ID.',
            okType: 'button-assertive'
          });
          $timeout(function() {
            alertPopup.close();
          }, 2500);
          return;
        }
        if ($scope.logininfo.strJobNo === '') {
          var alertPopup = $ionicPopup.alert({
            title: 'Please Enter Event Job No.',
            okType: 'button-assertive'
          });
          $timeout(function() {
            alertPopup.close();
          }, 2500);
          return;
        }
        $ionicLoading.show();
        if ($scope.logininfo.CurRole === '2' || $scope.logininfo.CurRole === '3') {
          strUri = '/api/event/login/check?CustomerCode=' + $scope.logininfo.strCustomerCode + '&JobNo=' + $scope.logininfo.strJobNo;
          WebApiService.GetParam(strUri, true).then(function success(result) {
            $ionicLoading.hide();
            sessionStorage.clear();
            sessionStorage.setItem('strPhoneNumber', '');
            sessionStorage.setItem('strDriverName', '');
            sessionStorage.setItem('strCustomerCode', $scope.logininfo.strCustomerCode);
            sessionStorage.setItem('strJobNo', $scope.logininfo.strJobNo);
            sessionStorage.setItem('strRole', $scope.logininfo.strRole);
            $state.go('listDirect', {
              'JobNo': $scope.logininfo.strJobNo
            }, {
              reload: true
            });
          });
        }
      }

    };

    $('#iPhoneNumber').on('keydown', function(e) {
      if (e.which === 9 || e.which === 13) {
        $scope.funcLogin();
      }
    });
    if ($stateParams.CheckUpdate === 'Y') {
      var url = ENV.website + '/update.json';
      $http.get(url)
        .success(function(res) {
          var serverAppVersion = res.version;
          $cordovaAppVersion.getVersionNumber().then(function(version) {
            if (version != serverAppVersion) {
              $state.go('update', {
                'Version': serverAppVersion
              });
            }
          });
        })
        .error(function(res) {});
    }
    if (window.cordova) {
      path = cordova.file.externalRootDirectory;
      $cordovaFile.checkFile(path, file)
        .then(function(success) {
          $cordovaFile.readAsText(path, file)
            .then(function(success) {
              var arConf = success.split("##");
              if (arConf.length > 3) {
                var arRole = arConf[3].split("=");
                if (arRole[1].length > 0) {
                  $scope.logininfo.strRole = arRole[1];
                  if ($scope.logininfo.strRole === 'Customer') {
                    $scope.logininfo.CurRole = '2';
                    $scope.logininfo.NewRole = $scope.logininfo.CurRole;
                  } else if ($scope.logininfo.strRole === 'Transporter') {
                    $scope.logininfo.CurRole = '3';
                    $scope.logininfo.NewRole = $scope.logininfo.CurRole;
                  }
                }
              } else {
                $scope.logininfo.strRole = "Driver/Ops";
              }
            }, function(error) {
              $cordovaToast.showShortBottom(error);
            });
        }, function(error) {
          $scope.logininfo.strRole = "Driver/Ops";
        });
    } else {
      $scope.logininfo.strRole = 'Driver/Ops';
    }
  }
]);
