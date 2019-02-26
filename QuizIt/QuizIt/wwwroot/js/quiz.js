﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/quizHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("DisplayQuestion", function (question) {
    document.getElementById("question").innerText = question;
    document.getElementById("resultList").innerText = "";
});

document.getElementById("displayQuestion").addEventListener("click", function (event) {
    connection.invoke("DisplayQuestion").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("ReceiveMessage", function (user, message, result) {
    console.log(result);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("resultList").appendChild(li); //Detta ska inte vara samma för alla frågor, varje fråga ska ha en egen result div
});

connection.start().then(function () {

    connection.invoke("DisplayQuestion").catch(function (err) {
        return console.error(err.toString());
    });


   // document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


for (let x of document.getElementsByClassName("sendButton")) {
    x.addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;

        connection.invoke("SendMessage", user).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}



