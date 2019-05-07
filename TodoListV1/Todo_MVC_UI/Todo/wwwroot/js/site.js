// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var status = document.getElementById("status");
    if (status != null) {
        $('#status').delay(2000).slideUp(300);
    }
});


$(function () {
    $("#Date").datepicker();
});

