'use strict';
app.controller('DetailCtrl',
        ['ENV','$scope', '$stateParams', '$state', '$http', '$timeout', '$ionicLoading', '$ionicPopup', 'WebApiService',
        function (ENV,$scope, $stateParams, $state, $http, $timeout, $ionicLoading, $ionicPopup, WebApiService) {
            $scope.detail = {
                Type            : $stateParams.Type,
                ContainerNo     : $stateParams.ContainerNo,
                JobNo           : $stateParams.JobNo,
                JobLineItemNo   : $stateParams.JobLineItemNo,
                LineItemNo      : $stateParams.LineItemNo,
                Description     : $stateParams.Description,
                DoneFlag        : $stateParams.DoneFlag
            };
            var currentDate = new Date();
            $scope.Update = {
                ContainerNo : $scope.detail.ContainerNo,
                remark      : $stateParams.Remark,
                datetime    : currentDate,
                strDatetime : currentDate.getTime()
            };
            if ($scope.detail.Type === 'OPEN') {
                $scope.strDoneOrUpdateTitle = 'Detail Infos';
                $scope.strDoneOrUpdate = '';
            }
            else if ($scope.detail.Type === 'UPDATE') {
                $scope.strDoneOrUpdateTitle = 'Update Remark';
                $scope.strDoneOrUpdate = 'Update';
            }
            else {
              $scope.strDoneOrUpdateTitle = 'Detail Infos';
                $scope.strDoneOrUpdate = 'Done';
            }
            $scope.returnList = function () {
                $state.go('list', { 'JobNo': $scope.detail.JobNo }, { reload: true });
            };
            $scope.update = function () {
                $ionicLoading.show();
                currentDate.setFullYear($scope.Update.datetime.getFullYear());
                currentDate.setMonth($scope.Update.datetime.getMonth());
                currentDate.setDate($scope.Update.datetime.getDate());
                currentDate.setHours($scope.Update.datetime.getHours());
                currentDate.setMinutes($scope.Update.datetime.getMinutes());
                var jsonData = null;
                  var strUri =null;
                if ($scope.detail.Type === 'UPDATE') {
                  // jsonData = { "JobNo": $scope.detail.JobNo, "JobLineItemNo": $scope.detail.JobLineItemNo, "LineItemNo": $scope.detail.LineItemNo, "DoneFlag": 'N', "Remark": $scope.Update.remark, "ContainerNo": $scope.Update.ContainerNo };
            strUri="/api/event/action/update/done?JobNo="+$scope.detail.JobNo+"&JobLineItemNo="+$scope.detail.JobLineItemNo+"&LineItemNo="+$scope.detail.LineItemNo+"&DoneFlag="+'N'+"&Remark="+$scope.Update.remark+ "&ContainerNo"+$scope.Update.ContainerNo ;
                } else if ($scope.detail.Type === 'DONE') {
                    // jsonData = { "JobNo": $scope.detail.JobNo, "JobLineItemNo": $scope.detail.JobLineItemNo, "LineItemNo": $scope.detail.LineItemNo, "DoneFlag": 'Y', "DoneDatetime": currentDate, "Remark": $scope.Update.remark, "ContainerNo": $scope.Update.ContainerNo };
                    strUri="/api/event/action/update/done?JobNo="+$scope.detail.JobNo+"&JobLineItemNo="+$scope.detail.JobLineItemNo+"&LineItemNo="+$scope.detail.LineItemNo+"&DoneFlag="+'Y'+"&DoneDatetime"+currentDate+"&Remark="+$scope.Update.remark+ "&ContainerNo"+$scope.Update.ContainerNo ;
                }
                WebApiService.GetParam(strUri, true).then(function success(result){
                      console.log(result);
                  $scope.tasks = result.data.results;
                  if (result.data.results.length == 0) {
                      var alertPopup = $ionicPopup.alert({
                          title: 'Connect to WebService failed.',
                          okType: 'button-assertive'
                      });
                      $timeout(function () {
                          alertPopup.close();
                      }, 2500);
                  }
                  else {
                    $ionicLoading.hide();
                    $state.go('list', { 'JobNo': $scope.detail.JobNo }, { reload: true });
                  }
              });

            };
        }]);
