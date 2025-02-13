const { error } = require("jquery");

//Create Connection
var connectionUserCount = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/userCount", signalR.HttpTransportType.WebSockets).build();
//Connect To Methods that Hub Invokes "AKA" Recieve Notifications From Hub
connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});
connectionUserCount.on("updateTotalUsers", (value) => {
    var newCountSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();
});
//Invoke Hub Method "AKA" Send Notification to Hub
function newWindowLoadedOnClient() {
    connectionUserCount.send("NewWindowLoaded");
}
//Start Connection
function fulfilled() {
    console.log("Connection to User Hub Successfully!");
    newWindowLoadedOnClient();
}
function rejected() {

}

connectionUserCount.start().then(fulfilled, rejected);