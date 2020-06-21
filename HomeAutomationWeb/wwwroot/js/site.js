// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

Array.from(document.getElementsByClassName("irrigation-pump-type")).forEach(btn => btn.addEventListener("click", function (event) { handleIrrigationPumpType(event) }));

function handleIrrigationPumpType(event) {
    $("#pumpLabel").text(event.target.textContent);
}

Array.from(document.getElementsByClassName("irrigation-time-span")).forEach(btn => btn.addEventListener("click", function (event) { handleIrrigationTimeSpan(event) }));

function handleIrrigationTimeSpan(event) {
    $("#timeLabel").text(event.target.textContent);
}