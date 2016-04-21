'use strict';
app.controller('UpdateCtrl', ['$scope', '$stateParams', '$state', '$timeout', '$ionicLoading', '$cordovaToast', '$cordovaFile', '$cordovaFileTransfer', '$cordovaFileOpener2',
  function($scope, $stateParams, $state, $timeout, $ionicLoading, $cordovaToast, $cordovaFile, $cordovaFileTransfer, $cordovaFileOpener2) {
    $scope.strVersion = $stateParams.Version;
    $scope.returnLogin = function() {
      onError();
      // $state.go('login', { 'CheckUpdate': 'N' }, { reload: true });
    };
    var onError = function() {
      $state.go('login', {}, {
        reload: true
      });
    };
    $scope.upgrade = function() {
      DownloadFileService.Download(ENV.website + '/TMS.apk', 'application/vnd.android.package-archive', null, onError, onError);
    };
  }
]);
