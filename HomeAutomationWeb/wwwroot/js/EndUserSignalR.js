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
    Array.from(document.getElementsByClassName("pump-sign")).forEach(el => el.classList.remove("badge-success", "badge-danger"))
    for (const irrigationKey of Object.keys(irrigationStatus)) {
        console.log(irrigationKey, irrigationStatus[irrigationKey]);
        var element = document.getElementById(irrigationKey);
        if (element !== null) {
            //buttons
            if (element.classList.contains("btnIrrigation")) 
                if (irrigationStatus[irrigationKey])
                    element.classList.add("btn-warning");
                else
                    element.classList.add("btn-outline-warning");
            
            //signs for pump
            if (element.classList.contains("pump-sign"))
                if (irrigationStatus[irrigationKey])
                    element.classList.add("badge-success");
                else
                    element.classList.add("badge-danger");

            //signs for pump
            if (element.classList.contains("irrigation-progress"))
                element.value = irrigationStatus[irrigationKey];              

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
document.getElementById("IrrigationStop").addEventListener("click", function (event) { handleIrrigationbutton(event) });
document.getElementById("IrrigationStart").addEventListener("click", function (event) { handleIrrigationStartbutton(event) });

function handleIrrigationbutton(event) {
    connection.invoke("IrrigationButtonClicked", event.target.id) //sends name of button to hub method
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function handleIrrigationStartbutton(event) { 
    //textContent
    var pump = document.getElementById("pumpLabel").textContent;
    var time = document.getElementById("timeLabel").textContent;
    connection.invoke("StartIrrigation", pump, time) //sends info for start
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

