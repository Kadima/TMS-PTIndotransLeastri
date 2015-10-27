function JsonServiceClient() {
    'use strict';
    return;
}
JsonServiceClient.id = 0;
JsonServiceClient.parseResponseStatus_ = function (status) {
    if (!status) return { isSuccess: true };
    var result =
    {
        isSuccess: status.meta.errors === undefined || status.meta.errors === null,
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
};
JsonServiceClient.prototype.send = function (webMethod, requestUrl, requestData, strSignature, onSuccess, onError) {
    var startCallTime = new Date();
    var id = JsonServiceClient.id++;
    $http({
        method: webMethod,
        url: requestUrl,
        data: requestData,
        headers: {
            "Signature": strSignature
        }
    }).success(function (response) {
        var endCallTime = new Date();
        var callDuration = endCallTime.getTime() - startCallTime.getTime();
        if (!response) {
            if (onSuccess) onSuccess(null);
            return;
        }
        var status = JsonServiceClient.parseResponseStatus_(response);
        if (status.isSuccess) {
            if (onSuccess) onSuccess(response);
        }
        else {
            if (onError) onError(status);
        }
    }).error(function (response) {
        var endCallTime = new Date();
        var callDuration = endCallTime.getTime() - startCallTime.getTime();
        try {
            if (onError) onError(response);
        }
        catch (e) { }
    });
}
//Sends a HTTP 'GET' request on the QueryString
JsonServiceClient.prototype.getFromService = function (requestUrl, requestData, strSignature, onSuccess, onError) {
    this.send("GET", requestUrl, requestData, strSignature, onSuccess, onError);
};
//Sends a HTTP 'POST' request as key value pair formData
JsonServiceClient.prototype.postToService = function (requestUrl, requestData, strSignature, onSuccess, onError) {
    this.send("POST", requestUrl, requestData, strSignature, onSuccess, onError);
};
//Sends a HTTP 'PUT' request as JSON @requires jQuery
JsonServiceClient.prototype.putToService = function (requestUrl, requestData, strSignature, onSuccess, onError) {
    //var jsonRequest = JsonServiceClient.toJSON(request);
    //this.send(webMethod, jsonRequest, onSuccess, onError, { type: "PUT", processData: false, contentType: "application/json; charset=utf-8" });
};
//Sends a HTTP 'DELETE' request as JSON @requires jQuery
JsonServiceClient.prototype.deleteFromService = function (requestUrl, requestData, strSignature, onSuccess, onError) {
    //var jsonRequest = JsonServiceClient.toJSON(request);
    //this.send(webMethod, jsonRequest, onSuccess, onError, { type: "DELETE", processData: false, contentType: "application/json; charset=utf-8" });
};
