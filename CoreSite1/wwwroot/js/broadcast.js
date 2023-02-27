// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    const encodedMsg = user + " says " + message;

    var li = '<div class="direct-chat-msg"><div class="direct-chat-text">';
    li = li + encodedMsg;
    li = li + '</div></div>';   
    document.getElementById("messagesList").innerHTML += li;
});

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(err => console.error(err));
    event.preventDefault();
});

connection.start().catch(err => console.error(err));