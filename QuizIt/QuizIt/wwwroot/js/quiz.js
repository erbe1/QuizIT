
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/quizHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("DisplayQuestion", function (question, answer, trackId, currentQuestion) {
    //document.getElementById("answerButton").disabled = ""; funkar ej..?
    document.getElementById("quizFinished").style.display = 'none';
    document.getElementById("playQuiz").style.display = 'block';
    document.getElementById("question").innerText = question;
    document.getElementById("questionNumber").innerText = currentQuestion;
    document.getElementById("resultList").innerText = "";

    let answerSection = document.getElementById("answer");
    let spotifyUrl = document.getElementById("spotifyUrl");

    //Om answerSection och SpotifyUrl inte är null/undefined
    if (answerSection) {
        answerSection.innerText = answer;
    }

    if (spotifyUrl) {
        spotifyUrl.innerHTML = `<iframe src="https://open.spotify.com/embed/track/${trackId}" width = "300" height = "80" frameborder = "0" allowtransparency = "true" allow = "encrypted-media" ></iframe >`;
    }

});

connection.on("ReceiveName", function (user) {
    var li = document.createElement("li");
    li.textContent = user;
    document.getElementById("joinedPlayers").appendChild(li);
});

connection.on("ReceiveMessage", function (user, message, result) {
    console.log(result);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("resultList").appendChild(li);
});

connection.on("QuizFinished", function () {
    document.getElementById("quizFinished").style.display = 'block';
    document.getElementById("quizFinished").innerHTML = '<h1>Quizet är slut!</h1></br><a href="/quiz/index">Tillbaka till quizen</a>';
    document.getElementById("playQuiz").style.display = 'none';
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
        event.srcElement.disabled = "disabled";
        var user = document.getElementById("userInput").value;

        connection.invoke("SendMessage", user).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}

let answerButton = document.getElementById("answerButton");

//Om "answerButton" är något vettigt, dvs inte null/undefined osv
if (answerButton) {
    answerButton.addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        connection.invoke("SendName", user).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}


