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
    response: ko.observable()
};

ko.applyBindings(viewModel);

function followLink(data, link) {
    $.ajax({
        url: link.href,
        success: loadResponse,
        fail: logResponse
    });
}

function loadResponse(data) {
    viewModel.response(data);
}

function logResponse(data) {
    console.log(data);
};

(function () {
    $.ajax({
        url: "http://localhost:5000",
        success: loadResponse,
        fail: logResponse
    });
}());