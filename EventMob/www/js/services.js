angular.module('EventMob.services', ['ionic'])
.service('JsonServiceClient', ['$http', '$ionicPopup', '$timeout', function ($http, $ionicPopup, $timeout) {
    function parseResponseStatus(status) {
        if (!status) return { isSuccess: true };
        var result =
        {
            isSuccess: status.meta.code === 200, // && status.meta.errors.code === 0,
            errorCode: status.meta.errors.code,
            message: status.meta.message,
            data: status.data.results,
            errorMessage: status.meta.errors.message//,
            //fieldErrors: [],
        };
        /*
        if (status.meta.errors.field) {
            for (var i = 0, len = status.FieldErrors.length; i < len; i++) {
                var err = status.FieldErrors[i];
                var error = { errorCode: err.ErrorCode, fieldName: err.FieldName, errorMessage: err.ErrorMessage || '' };
                result.fieldErrors.push(error);
                if (error.fieldName) {
                    result.fieldErrorMap[error.fieldName] = error;
                }
            }
        }
        */
        return result;
    }
    this.postToService = function (requestUrl, requestData, onSuccess, onError) {
        var strSignature = hex_md5(strBaseUrl + requestUrl + strSecretKey.replace(/-/ig, ""));
        $http({
            method: "POST",
            url: strWebServiceURL + strBaseUrl + requestUrl,
            data: requestData,
            headers: {
                "Signature": strSignature
            }
        }).success(function (response) {
            if (!response) {
                if (onSuccess) onSuccess(null);
                return;
            }
            var status = parseResponseStatus(response);
            if (status.isSuccess) {
                if (onSuccess) onSuccess(response);
            }
            else {
                var alertPopup = $ionicPopup.alert({
                    title: response.meta.message,
                    subTitle: response.meta.errors.message,
                    okType: 'button-assertive'
                });
                $timeout(function () {
                    alertPopup.close();
                }, 5000);
                if (onError) onError(response);
            }
        }).error(function (response) {
            try {
                if (onError) onError(response);
                var alertPopup = $ionicPopup.alert({
                    title: 'Connect to WebService failed.',
                    okType: 'button-assertive'
                });
                $timeout(function () {
                    alertPopup.close();
                }, 2500);
            }
            catch (e) { }
        });
    }
    this.getFromService = function (requestUrl, onSuccess, onError, onFinally) {
        var strSignature = hex_md5(strBaseUrl + requestUrl + "?format=json" + strSecretKey.replace(/-/ig, ""));
        $http({
            method: "GET",
            url: strWebServiceURL + strBaseUrl + requestUrl + "?format=json",
            headers: {
                "Signature": strSignature
            }
        }).success(function (response) {
            if (!response) {
                if (onSuccess) onSuccess(null);
                return;
            }
            var status = parseResponseStatus(response);
            if (status.isSuccess) {
                if (onSuccess) onSuccess(response);
            }
            else {
                if (onError) onError(response);
                var alertPopup = $ionicPopup.alert({
                    title: response.meta.message,
                    subTitle: response.meta.errors.message,
                    okType: 'button-assertive'
                });
                $timeout(function () {
                    alertPopup.close();
                }, 5000);
            }
        }).error(function (response) {
            try {
                var alertPopup = $ionicPopup.alert({
                    title: 'Connect to WebService failed.',
                    okType: 'button-assertive'
                });
                $timeout(function () {
                    alertPopup.close();
                }, 2500);
                if (onError) onError(response);
            }
            catch (e) { }
        }).finally(function () {
            if (onFinally) onFinally();
        });
    }
}]);