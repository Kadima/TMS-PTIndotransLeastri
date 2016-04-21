'use strict';
app.controller('MainCtrl', ['ENV', '$scope', '$http', '$state', '$stateParams', '$ionicLoading', '$ionicPopup', '$timeout', 'WebApiService',
  function(ENV, $scope, $http, $state, $stateParams, $ionicLoading, $ionicPopup, $timeout, WebApiService) {
    $scope.returnLogin = function() {
      $state.go('login', {
        'CheckUpdate': 'N'
      }, {
        reload: true
      });
    };
    var strDriverName = sessionStorage.getItem('strDriverName');
    var strPhoneNumber = sessionStorage.getItem('strPhoneNumber');
    if (strDriverName != null && strDriverName.length > 0) {
      $scope.strName = strDriverName;
    } else {
      $scope.strName = "Driver";
    }
    if (strPhoneNumber === null) {
      strPhoneNumber = '5888865';
    }
    $scope.strItemsCount = "loading...";
    $scope.showList = function(strJobNo) {
      $state.go('list', {
        'JobNo': strJobNo
      }, {
        reload: true
      });
    };
    var funcShowList = function() {
      $scope.Jobs = [{
        JobNo: '',
        ContainerCounts: '',
        TaskDoneCounts: ''
      }];
      var strUri = '/api/event/action/list/jobno?PhoneNumber=' + strPhoneNumber;
          WebApiService.GetParam(strUri, true).then(function success(result) {
        console.log(result);
        $scope.Jobs = result.data.results;
        if (result.data.results.length === 1 && $stateParams.blnForcedReturn === 'N') {
          $state.go('list', {
            'JobNo': result.data.results[0].JobNo
          }, {
            reload: true
          });
        } else if (result.data.results.length < 1) {
          var alertPopup = $ionicPopup.alert({
            title: 'No Tasks.',
            okType: 'button-calm'
          });
          return;

        }

      });
    };
    funcShowList();
  }
]);
