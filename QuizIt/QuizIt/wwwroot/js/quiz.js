"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/quizHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li); //Detta ska inte vara samma för alla frågor, varje fråga ska ha en egen messagelist
});

connection.start().then(function () {
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



