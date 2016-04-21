var appService = angular.module('EventMob.services', [
  'ionic',
  'ngCordova.plugins.toast',
  'ngCordova.plugins.file',
  'ngCordova.plugins.fileTransfer',
  'ngCordova.plugins.fileOpener2',
  'ngCordova.plugins.inAppBrowser',
  'EventMob.config'
]);

appService.service('WebApiService', ['$q', 'ENV', '$http', '$ionicLoading', '$ionicPopup', '$timeout',
  function($q, ENV, $http, $ionicLoading, $ionicPopup, $timeout) {
    this.Post = function(requestUrl, requestData, blnShowLoad) {
      if (blnShowLoad) {
        $ionicLoading.show();
      }
      var deferred = $q.defer();
      // var strSignature = hex_md5(requestUrl + ENV.appId.replace(/-/ig, ""));
      var url = ENV.api + requestUrl;
      // log4web.log(url);
      var config = {
        'Content-Type': 'application/json'
      };
      $http.post(url, requestData, config).success(function(data, status, headers, config, statusText) {
        if (blnShowLoad) {
          $ionicLoading.hide();
        }
        deferred.resolve(data);
      }).error(function(data, status, headers, config, statusText) {
        if (blnShowLoad) {
          $ionicLoading.hide();
        }
        deferred.reject(data);
        // log4web.log(data);
      });
      return deferred.promise;
    };
    this.Get = function(requestUrl, blnShowLoad) {
      if (blnShowLoad) {
        $ionicLoading.show();
      }
      var deferred = $q.defer();
      // var strSignature = hex_md5(requestUrl + "?format=json" + ENV.appId.replace(/-/ig, ""));
      var url = ENV.api + requestUrl + "?format=json";
      // log4web.log(url);
      $http.get(url).success(function(data, status, headers, config, statusText) {
        if (blnShowLoad) {
          $ionicLoading.hide();
        }
        deferred.resolve(data);
      }).error(function(data, status, headers, config, statusText) {
        if (blnShowLoad) {
          $ionicLoading.hide();
        }
        deferred.reject(data);
        // log4web.log(data);
      });
      return deferred.promise;
    };
    this.GetParam = function(requestUrl, blnShowLoad) {
      if (blnShowLoad) {
        $ionicLoading.show();
      }
      var deferred = $q.defer();
      // var strSignature = hex_md5(requestUrl + "&format=json" + ENV.appId.replace(/-/ig, ""));
      var url = ENV.api + requestUrl + "&format=json";
      // log4web.log(url);
      $http.get(url).success(function(data, status, headers, config, statusText) {
        if (blnShowLoad) {
          $ionicLoading.hide();
        }
        deferred.resolve(data);
      }).error(function(data, status, headers, config, statusText) {
        if (blnShowLoad) {
          $ionicLoading.hide();
        }
        deferred.reject(data);
        // log4web.log(data);
      });
      return deferred.promise;
    };
  }
]);

appService.service('DownloadFileService', ['ENV', '$http', '$timeout', '$ionicLoading', '$cordovaToast', '$cordovaFile', '$cordovaFileTransfer', '$cordovaFileOpener2',
  function(ENV, $http, $timeout, $ionicLoading, $cordovaToast, $cordovaFile, $cordovaFileTransfer, $cordovaFileOpener2) {
    this.Download = function(url, fileName, fileType, onPlatformError, onCheckError, onDownloadError) {
      $ionicLoading.show({
        template: "Download  0%"
      });
      var blnError = false;
      if (ENV.fromWeb) {
        $cordovaFile.checkFile(cordova.file.externalRootDirectory + '/' + ENV.rootPath, fileName)
          .then(function(success) {
            //
          }, function(error) {
            blnError = true;
          }).catch(function(ex) {
            console.log(ex);
          });
        var targetPath = cordova.file.externalRootDirectory + '/' + ENV.rootPath + '/' + fileName;
        var trustHosts = true;
        var options = {};
        if (!blnError) {
          $cordovaFileTransfer.download(url, targetPath, trustHosts, options).then(function(result) {
            $ionicLoading.hide();
            $cordovaFileOpener2.open(targetPath, fileType).then(function() {
              // success
            }, function(err) {
              // error
            }).catch(function(ex) {
              console.log(ex);
            });
          }, function(err) {
            $cordovaToast.showShortCenter('Download faild.');
            $ionicLoading.hide();
            if (onDownloadError) onDownloadError();
          }, function(progress) {
            $timeout(function() {
              var downloadProgress = (progress.loaded / progress.total) * 100;
              $ionicLoading.show({
                template: "Download  " + Math.floor(downloadProgress) + "%"
              });
              if (downloadProgress > 99) {
                $ionicLoading.hide();
              }
            })
          }).catch(function(ex) {
            console.log(ex);
          });
        } else {
          $ionicLoading.hide();
          $cordovaToast.showShortCenter('Check file faild.');
          if (onCheckError) onCheckError();
        }
      } else {
        $ionicLoading.hide();
        if (onPlatformError) onPlatformError(url);
      }
    };
  }
]);

appService.service('OpenUrlService', ['ENV', '$cordovaInAppBrowser',
  function(ENV, $cordovaInAppBrowser) {
    this.Open = function(url) {
      if (!ENV.fromWeb) {
        var options = {
          location: 'yes',
          clearcache: 'yes',
          toolbar: 'no'
        };
        $cordovaInAppBrowser.open(url, '_system', options)
          .then(function(event) {
            // success
          })
          .catch(function(event) {
            // error
            $cordovaInAppBrowser.close();
          });
      } else {
        window.open(url);
      }
    };
  }
]);

appService.service('GeoService', ['$q',
  function($q) {
    this.BaiduGetCurrentPosition = function() {
      var deferred = $q.defer();
      var geolocation = new BMap.Geolocation();
      geolocation.getCurrentPosition(function(r) {
        if (this.getStatus() == BMAP_STATUS_SUCCESS) {
          deferred.resolve(r.point);
          var pos = {
            type: 'Baidu',
            lat: r.point.lat,
            lng: r.point.lng
          };
          log4web.log(pos);
        } else {
          deferred.reject(this.getStatus());
          log4web.log(this.getStatus());
        }
      }, {
        maximumAge: 60000,
        timeout: 5000,
        enableHighAccuracy: true
      })
      return deferred.promise;
    };
    this.GoogleGetCurrentPosition = function() {
      var deferred = $q.defer();
      // Try HTML5 geolocation.
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
          deferred.resolve(position);
          var pos = {
            type: 'Google',
            lat: position.coords.latitude,
            lng: position.coords.longitude
          };
          log4web.log(pos);
        }, function() {
          deferred.reject('The Geolocation service failed.');
          log4web.log('The Geolocation service failed.');
        }, {
          maximumAge: 60000,
          timeout: 5000,
          enableHighAccuracy: true
        });
      } else {
        deferred.reject('Browser does not support Geolocation');
        log4web.log('Browser does not support Geolocation');
      }
      return deferred.promise;
    };
  }
]);
