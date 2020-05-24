"use strict";

//NOTE:
//button names must be the same as properties on irrigationStatus

//connect to SignalR Hub
var connection = new signalR.HubConnectionBuilder().withUrl("/endUserHub").build();

//Disable all firstly
Array.from(document.getElementsByClassName("btnIrrigation")).forEach(btn => btn.disabled = true);

//react on message with status from signalR
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

//start connection and if successfull, enable buttons
connection.start().then(function () {
    informWebStarted();
    Array.from(document.getElementsByClassName("btnIrrigation")).forEach(btn => btn.disabled = false);
}).catch(function (err) {
    return console.error(err.toString());
});

//handle irrigation buttons click
Array.from(document.getElementsByClassName("btnIrrigation")).forEach(btn => btn.addEventListener("click", function (event) { handleIrrigationbutton(event) }));
document.getElementById("shutDown").addEventListener("click", function (event) { handleIrrigationbutton(event) });

function handleIrrigationbutton(event) {
    connection.invoke("IrrigationButtonClicked", event.target.id) //sends name of button to hub method
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function informWebStarted() {
    connection.invoke("WebStarted")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

