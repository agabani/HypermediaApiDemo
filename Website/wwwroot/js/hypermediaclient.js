"use strict";

ko.bindingHandlers.foreachprop = {
    transformObject: function(obj) {
        var properties = [];
        for (var key in obj) {
            if (obj.hasOwnProperty(key)) {
                properties.push({ key: key, value: obj[key] });
            }
        }
        return properties;
    },
    init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            properties = ko.bindingHandlers.foreachprop.transformObject(value);
        ko.applyBindingsToNode(element, { foreach: properties }, bindingContext);
        return { controlsDescendantBindings: true };
    }
};

ko.virtualElements.allowedBindings.foreachproperty = true;

var viewModel = {
    response: ko.observable(),
    headers: {}
};

ko.applyBindings(viewModel);

function followLink(data, link) {
    $.ajax({
        url: link.href,
        headers: viewModel.headers,
        success: handleSuccessResponse,
        fail: handleFailResponse
    });
}

function followAction(data, action) {
    console.log(action);

    $.ajax({
        url: action.href,
        headers: viewModel.headers,
        method: action.method,
        data: transformRequest(action.fields),
        contentType: action.type,
        success: handleSuccessResponse,
        fail: handleFailResponse
    });
}

function transformRequest(fields) {
    var fieldsArray = [];

    for (var index in fields) {
        if (fields.hasOwnProperty(index)) {
            var field = fields[index];

            fieldsArray.push(field.name + "=" + field.value);
        }
    }

    return fieldsArray.join("&");
}

function handleSuccessResponse(data) {
    handleHttpClass(data);
    viewModel.response(data);
}

function handleHttpClass(data) {
    for (var entity in data.entities) {
        if (data.entities.hasOwnProperty(entity)) {
            if ($.inArray("http", data.entities[entity].class) >= 0) {
                addHttpHeadersToViewModel(data.entities[entity]);
            }
        }
    }
}

function addHttpHeadersToViewModel(data) {
    for (var property in data.properties) {
        if (data.properties.hasOwnProperty(property)) {
            viewModel.headers[property] = data.properties[property];
        }
    }
}

function handleFailResponse(data) {
    console.log(data);
};

(function() {
    $.ajax({
        url: "http://localhost:5000",
        success: handleSuccessResponse,
        fail: handleFailResponse
    });
}());