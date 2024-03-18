(function ($) {

    $.ajaxPostJson = function (options) {
        options = options || {};
        options.type = 'POST';

        ajaxJson(options);
    };

    $.ajaxGetJson = function (options) {
        options = options || {};
        options.type = 'GET';
        options.traditional = true;

        ajaxJson(options);
    };

    $.ajaxJson = function (options) {
        options = options || {};
        options.traditional = true;

        ajaxJson(options);
    };

    var ajaxJson = function (options) {
        var settings = {
            dataType: 'json',
            crossDomain: true,
            contentType: 'application/json; charset=utf-8'
        };

        $.extend(true, settings, options);

        ajax(settings);
    };

    $.ajaxPostFormData = function (options) {
        var settings = {
            type: "POST",
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
        };

        $.extend(true, settings, options);

        ajax(settings);
    }

    var ajax = function (options) {
        var settings = {
            type: 'GET',
            cache: false,
            //dataType: 'json',
            //crossDomain: true,
            enableErrorHandling: true,
            enableRequestAbortedErrorHandling: true,
            ignoreError404: false
        };

        $.extend(true, settings, options);

        if (settings.url === undefined) { throw new Error("Missing param: url"); }


        jQuery.support.cors = true;

        settings.success = function (response, status, xhr) {
            if (status === "success") {
                if (typeof (options.success) === "function") {
                    options.success(response, status, xhr);
                }
            }
            else {
                if (typeof (options.error) === "function") {
                    options.error(response, status, xhr);
                } else {
                    $.errorModal('Sorry, it seems that an error occured.');
                }
            }
        };

        settings.error = function (xhr, status, error) {

            if (settings.enableErrorHandling == true) {

                if (xhr.status === 400) { // 400 = bad request

                    var errorsObj = null;

                    if (xhr.responseJSON && xhr.responseJSON.modelState) {
                        errorsObj = xhr.responseJSON.modelState;
                    } else if (xhr.responseJSON && xhr.responseJSON.errors) {
                        errorsObj = xhr.responseJSON.errors;
                    } else if (typeof xhr.responseJSON === 'object') {
                        errorsObj = xhr.responseJSON;
                    } 

                    if (errorsObj) {
                        var message = '';

                        $.each(errorsObj, function (key, value) {
                            message += value + '\r\n';
                        });

                        $.errorModal(message);
                        return;
                    }

                    if (xhr.responseText) {
                        $.errorModal(xhr.responseText);
                        return;
                    }

                    $.errorModal(error);
                    return;
                }

                if (xhr.status === 401) {
                    $.errorModal("Access denied!");
                    //window.location.href = "Login.aspx";
                    return;
                }

                if (xhr.status === 404) {
                    if (!settings.ignoreError404) {
                        $.errorModal("404. The requested item was not found!");
                    }
                    return;
                }

                if (xhr.status === 409) { // 409 = conflict

                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        $.errorModal(xhr.responseJSON.message);
                        return;
                    }

                    if (xhr.responseText) {
                        $.errorModal(xhr.responseText);
                        return;
                    }

                    $.errorModal(error);
                    return;
                }

                if (xhr.status === 0 && xhr.getAllResponseHeaders) { // 0 = destination not reached
                    var responseHeaders = xhr.getAllResponseHeaders();

                    if (responseHeaders == null || responseHeaders == '') {
                        if (settings.enableRequestAbortedErrorHandling == true) {
                            $.errorModal("Request aborted.");
                        }

                        return;
                    }
                }
            }

            // Call the error callback if specified.
            if (typeof (options.error) === "function") {
                options.error(xhr, status, error);
            } else if (settings.enableErrorHandling == true) {
                $.errorModal('Sorry, it seems that an error occured.');
            }
        };

        settings.complete = function (xhr, status) {
            // Call the error callback if specified.
            if (typeof (options.complete) === "function") {
                options.complete(xhr, status);
            }
        };

        $.ajax(settings);
    };
})(jQuery);