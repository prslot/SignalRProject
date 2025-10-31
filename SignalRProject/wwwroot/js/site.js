"use strict";

var SignalRConn = new signalR.HubConnectionBuilder().withUrl("/hub").build();

SignalRConn.on("SendRegisterHub", function (id, app_name) {

    if (document.querySelector('[data-appname="' + app_name + '"]')) {
        var existingElement = document.querySelector('[data-appname="' + app_name + '"]');
        existingElement.id = id;
        updateTableElement(id, app_name, " --> last seen at: ")
    }
    else {
        createTableElement(id, app_name, " --> registered at: ")
    }
});

SignalRConn.on("SendPollHub", function (id, app_name) {

    if (document.getElementById(id)) {
        updateTableElement(id, app_name, " --> last seen at: ")
    } else {
        createTableElement(id, app_name, " --> registered at: ")
    }

});

SignalRConn.on("DisconnectedHub", function (id) {

    if (document.getElementById(id)) {
        var tr = document.getElementById(id);
        var td = tr.firstElementChild;

        var d = new Date();
        td.textContent = tr.dataset.appname + " --> offline sinds: " + d.toLocaleDateString() + " " + d.toLocaleTimeString();
    }

});

SignalRConn.start().then(function () {
    SignalRConn.invoke("AddToManagerGroup").catch(function (ex) {
        return console.error(ex.toString());
    });
}).catch(function (ex) {
    return console.error(ex.toString());
});


function createTableElement(id, application, action) {
    var table = document.getElementById('tblConnectedApps');
    var tr = document.createElement('tr');
    tr.id = id;
    tr.dataset.appname = application;
    table.appendChild(tr);

    var td = document.createElement('td');
    tr.appendChild(td);
    var d = new Date();
    td.textContent = application + action + d.toLocaleDateString() + " " + d.toLocaleTimeString();
}

function updateTableElement(id, application, action) {
    var td = document.getElementById(id).firstElementChild;

    var d = new Date();
    td.textContent = application + action + d.toLocaleDateString() + " " + d.toLocaleTimeString();
}
