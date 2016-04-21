'use strict';
app.controller('ListCtrl', ['ENV', '$scope', '$state', '$stateParams', '$http', '$ionicPopup', '$timeout', '$ionicLoading', '$cordovaDialogs', 'WebApiService',
  function(ENV, $scope, $state, $stateParams, $http, $ionicPopup, $timeout, $ionicLoading, $cordovaDialogs, WebApiService) {
    $scope.JobNo = $stateParams.JobNo;
    var strJobNo = $scope.JobNo;
    var strPhoneNumber = sessionStorage.getItem("strPhoneNumber");
    var strCustomerCode = sessionStorage.getItem("strCustomerCode");
    var strRole = sessionStorage.getItem("strRole");
    if (strCustomerCode === null) {
      strCustomerCode = '';
    }
    if (strPhoneNumber === null) {
      strPhoneNumber = '5888865';
    }
    if (strRole === null) {
      strRole = 'Driver/Ops';
    }
    //var strJobNo = sessionStorage.getItem("strJobNo").toString();
    $scope.shouldShowDelete = false;
    if (strRole === 'Driver/Ops') {
      $scope.listCanSwipe = true;
    } else {
      $scope.listCanSwipe = false;
    }
    $scope.returnMain = function() {
      if (strRole === 'Driver/Ops') {
        $state.go('main', {
          'blnForcedReturn': 'Y'
        }, {
          reload: true
        });
      } else {
        $state.go('login', {
          'CheckUpdate': 'N'
        }, {
          reload: true
        });
      }
    };
    $scope.funcShowRole = function(roleType) {
      if (roleType === 1) {
        if (strRole === 'Driver/Ops') {
          return true;
        } else {
          return false;
        }
      } else if (roleType === 2) {
        if (strRole === 'Customer') {
          return true;
        } else {
          return false;
        }
      } else if (roleType === 3) {
        if (strRole === 'Transporter') {
          return true;
        } else {
          return false;
        }
      }
    };
    $scope.funcShowTruckType = function(task) {
      if (task.JobType === 'IM') {
        return 'In';
      } else if (task.JobType === 'EX' || task.JobType === 'TP') {
        return 'Out';
      } else {
        return '';
      }
    };
    $scope.funcShowLoadType = function(task) {
      if (task.JobType === 'IM') {
        return 'Unload';
      } else if (task.JobType === 'EX' || task.JobType === 'TP') {
        return 'Load';
      } else {
        return '';
      }
    };
    $scope.funcShowDatetime = function(utc) {
      // if (typeof(utc) === 'undefined') return ''
      // var utcDate = Number(utc.substring(utc.indexOf('(') + 1, utc.lastIndexOf('-')));
      // var newDate = new Date(utcDate);
      // if (newDate.getUTCFullYear() < 2166 && newDate.getUTCFullYear() > 1899) {
      //     return newDate.Format('yyyy-MM-dd hh:mm');
      // } else {
      //     return '';
      // }
      if (typeof(utc) === 'undefined') {
        return '';
      } else {
        return moment(utc).format('YYYY-MMM-DD HH:mm');
      }
    };
    var getData = function() {
      var strUri = '';
      var onSuccess = null;
      if (strCustomerCode.length > 0) {
        strUri = "/api/event/action/list/jmjm6/?JobNo=" + strJobNo;
        WebApiService.GetParam(strUri, true).then(function success(result) {
          console.log(result);
          $scope.tasks = result.data.results;
          $scope.$broadcast('scroll.refreshComplete');
          if (result.data.results.length == 0) {
            var alertPopup = $ionicPopup.alert({
              title: 'No Tasks.',
              okType: 'button-calm'
            });
            $timeout(function() {
              alertPopup.close();
            }, 2500);
          }

        });
      } else {
        strUri = "/api/event/action/list/container/?phoneNumber=" + strPhoneNumber + "&JobNo=" + strJobNo;
        WebApiService.GetParam(strUri, true).then(function success(result) {
          console.log(result);
          $scope.tasks = result.data.results;
          $scope.$broadcast('scroll.refreshComplete');
          if (result.data.results.length == 0) {
            var alertPopup = $ionicPopup.alert({
              title: 'No Tasks.',
              okType: 'button-calm'
            });
            $timeout(function() {
              alertPopup.close();
            }, 2500);
          }
        });
      }
    };
    // var getTasks = function() {
    //   $ionicLoading.show();
    //   getData();
    // };
    $scope.doRefresh = function() {
      getData();
      $scope.$apply();
    };
    $scope.showContainerNo = function(task) {
      if (typeof(task.ContainerNo) === 'undefined') {
        return false;
      } else {
        if (task.ContainerNo.length > 0) {
          return true;
        } else {
          return false;
        }
      }
    };
    var checkEventOrder = function(task) {
      for (var i = 0; i <= $scope.tasks.length - 1; i++) {
        if ($scope.tasks[i].JobLineItemNo < task.JobLineItemNo && $scope.tasks[i].AllowSkipFlag != 'Y') {
          if ($scope.tasks[i].DoneFlag != 'Y') {
            return false;
          }
        }
      }
      return true;
    };
    $scope.slideDone = function(task, type) {
      if (type === 'OPEN') {
        $state.go('detail', {
          'Type': 'OPEN',
          'ContainerNo': task.ContainerNo,
          'JobNo': task.JobNo,
          'JobLineItemNo': task.JobLineItemNo,
          'LineItemNo': task.LineItemNo,
          'Description': task.Description,
          'Remark': task.Remark,
          'DoneFlag': task.DoneFlag
        });
      } else if (type === 'UPDATE') {
        $state.go('detail', {
          'Type': 'UPDATE',
          'ContainerNo': task.ContainerNo,
          'JobNo': task.JobNo,
          'JobLineItemNo': task.JobLineItemNo,
          'LineItemNo': task.LineItemNo,
          'Description': task.Description,
          'Remark': task.Remark,
          'DoneFlag': task.DoneFlag
        });
      } else {
        // if (checkEventOrder(task)) {
        $state.go('detail', {
          'Type': 'DONE',
          'ContainerNo': task.ContainerNo,
          'JobNo': task.JobNo,
          'JobLineItemNo': task.JobLineItemNo,
          'LineItemNo': task.LineItemNo,
          'Description': task.Description,
          'Remark': task.Remark,
          'DoneFlag': task.DoneFlag
        });
        // } else {
        //     var alertPopup = $ionicPopup.alert({
        //         title: 'Previous event not Done.<br/>Not allow to do this one.',
        //         okType: 'button-assertive'
        //     });
        //     $timeout(function () {
        //         alertPopup.close();
        //     }, 2500);
        // }
      }
    };
    getData();
  }
]);
