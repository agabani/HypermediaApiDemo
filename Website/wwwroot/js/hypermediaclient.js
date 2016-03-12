var myViewModel = {
    personName: ko.observable("Bob"),
    personAge: ko.observable(123)
};

ko.applyBindings(myViewModel);

(function () {

    function loadRootResponse(data) {
        console.log(data);
    };

    function alertContent() {
        
    };

    $.ajax({
        url: "http://localhost:5000",
        success: loadRootResponse,
        fail: alertContent
    });

}());