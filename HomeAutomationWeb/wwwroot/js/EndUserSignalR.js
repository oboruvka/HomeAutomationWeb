"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/endUserHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("IrrigationStatusUpdate", function (status) {
    console.log(status);
    var irrigationStatus = JSON.parse(status);
    Array.from(document.getElementsByClassName("btnIrrigation")).forEach(el => el.classList.remove("btn-outline-warning", "btn-warning"))
    for (const irrigationKey of Object.keys(irrigationStatus)) {
        console.log(irrigationKey, irrigationStatus[irrigationKey]);
        var element = document.getElementById(irrigationKey);
        if (element !== null) {
            if (irrigationStatus[irrigationKey])
                document.getElementById(irrigationKey).classList.add("btn-warning");
            else
                document.getElementById(irrigationKey).classList.add("btn-outline-warning");
        }
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;//TODO enable-disable all
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("pump").addEventListener("click", function (event) { setBtn("btnValve") });

Array.from(document.getElementsByClassName("btnIrrigation")).forEach(valve => valve.addEventListener("click", function (event) {
    btnClicked(event);
}))

function setBtnsAsStatus(className, irrigationStatus) {
    Array.from(document.getElementsByClassName(className)).forEach(el => el.classList.replace("btn-outline-warning", "btn-warning"))    
};

function btnClicked(event) {
    connection.invoke("IrrigationButtonClicked", event.target.id).catch(function (err) {
        return console.error(err.toString());
    });
};