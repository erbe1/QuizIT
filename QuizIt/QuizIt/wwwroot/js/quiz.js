﻿
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/quizHub").build();

//När en ny fråga visas körs nedanstående
connection.on("DisplayQuestion", function (question, answer, trackId, currentQuestion) {
    document.getElementById("quizFinished").style.display = 'none';
    document.getElementById("playQuiz").style.display = 'block';
    document.getElementById("question").innerText = question;
    document.getElementById("questionNumber").innerText = currentQuestion;
    document.getElementById("resultList").innerText = "";

    let answerSection = document.getElementById("answer");
    let spotifyUrl = document.getElementById("spotifyUrl");
    let answerButton = document.getElementById("answerButton");
    let playerButtons = document.getElementById("playerButtons");

    //Om answerSection och SpotifyUrl inte är null/undefined
    if (answerSection) {
        answerSection.innerText = answer;
    }

    if (spotifyUrl) {
        spotifyUrl.innerHTML = `<iframe src="https://open.spotify.com/embed/track/${trackId}" width = "300" height = "80" frameborder = "0" allowtransparency = "true" allow = "encrypted-media" ></iframe >`;
    }

    if (answerButton) {
        answerButton.disabled = "";
    }

    if (playerButtons) {
        playerButtons.innerText = "";
    }

});

//När en ny spelare sparat sitt namn läggs det till i dictionairy och skrivs ut enligt nedan
connection.on("ReceiveName", function (userScores) {
    console.log("userScores", userScores);
    let jp = document.getElementById("joinedPlayers");
    jp.innerHTML = "";

    for (let userScore of userScores) {
        let userName = userScore.name;
        let score = userScore.score;
        jp.innerHTML += `<li>${userName} ${score} poäng</li>`;
    }

});

//När en spelare tryckt på "Jag kan svaret" skrivs det ut enligt nedan
connection.on("ReceiveMessage", function (user, message, result) {
    console.log(result);
    let msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let encodedMsg = user + msg;
    let li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("resultList").appendChild(li);

    //När spelaren tryckt på kan svaret skapas en knapp med spelarens namn på
    let playerButtons = document.getElementById("playerButtons");
    if (playerButtons) {

        let button = document.createElement("button");
        button.style.color = "black";
        button.innerHTML = user;
        button.className = "scoreButton";

        button.addEventListener("click", function (event) {
            event.srcElement.disabled = "disabled";
            connection.invoke("UpdateScore", user).catch(function (err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });

        playerButtons.appendChild(button);
    }
});

//När quizet är slut skrivs nedanstående ut
connection.on("QuizFinished", function (userScores) {
    //document.getElementById("quizFinished").style.display = 'block';
    //document.getElementById("quizFinished").innerHTML = '<h1>Quizet är slut!</h1></br><a href="/quiz/index">Tillbaka till quizen</a>';
    document.getElementById("playQuiz").style.display = 'none';

    let quizFinished = document.getElementById("quizFinished");
    quizFinished.style.display = 'block';
    quizFinished.innerHTML = '<h1>Quizet är slut!</h1></br><h1>Resultat</h1>';

    for (let userScore of userScores) {
        let userName = userScore.name;
        let score = userScore.score;
        quizFinished.innerHTML += `<li>${userName} ${score} poäng</li>`;
    }

    quizFinished.innerHTML += '</br > <a href="/quiz/index">Tillbaka till quizen</a>';
});


//När sidan laddas skapas kopplingen som sedan kallar på funktionen DisplayQuestion
connection.start().then(function () {

    connection.invoke("DisplayQuestion").catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});


//Varje knapp som skapas lyssnar efter klick, då går den in i score-metoden
for (let x of document.getElementsByClassName("scoreButton")) {
    x.addEventListener("click", function (event) {
        event.srcElement.disabled = "disabled";
        var user = document.getElementById("userInput").value;

        connection.invoke("UpdateScore", user).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}

let answerButton = document.getElementById("answerButton");

//Om "answerButton" är något vettigt, dvs inte null/undefined osv,
//Då lyssnar den efter klick och kallas på SendMessage-funktionen
if (answerButton) {
    answerButton.addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        event.srcElement.disabled = "disabled";
        connection.invoke("SendMessage", user).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}


//nameButton sparar spelarens namn
let nameButton = document.getElementById("nameButton");

//Om "nameButton" är något vettigt, dvs inte null/undefined osv
if (nameButton) {
    nameButton.addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        event.srcElement.disabled = "disabled";
        connection.invoke("SendName", user).catch(function (err) {
            alert("namnet '" +user+ "' upptaget!")
            event.srcElement.disabled = "";
            //return console.error(err.toString());
        });
        let userEntersName = document.getElementById("userEntersName");
        if (userEntersName) {
            userEntersName.style.display = "none";
        }
        event.preventDefault();
    });
}

