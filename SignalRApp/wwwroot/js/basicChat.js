﻿var connectionChat = new signalR.HubConnectionBuilder().withUrl("/hubs/basicChat").build();

document.getElementById("sendMessage").disabled = true;

connectionChat.on("MessageRecieve", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} - ${message}`;
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var message = document.getElementById("chatMessage").value;
    var reciever = document.getElementById("receiverEmail").value;


    if (reciever.length > 0) {
        connectionChat.send("SendMessageToReciever", sender, reciever, message).catch(function (err) {
            return console.error(err.toString());
        });
    } else {

        connectionChat.send("SendMessageToAll", sender, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
})

connectionChat.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
});