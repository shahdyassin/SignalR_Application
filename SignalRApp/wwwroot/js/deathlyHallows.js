var wandSpan = document.getElementById("wandCounter");
var cloakSpan = document.getElementById("cloakCounter");
var stoneSpan = document.getElementById("stoneCounter");
//Create Connection
var connectionDeathlyHallows = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/deathlyhallows").build();
//Connect To Methods that Hub Invokes "AKA" Recieve Notifications From Hub
connectionDeathlyHallows.on("updateDeathlyHallowsCount", (cloak, stone, wand) => {
    cloakSpan.innerText = cloak.toString();
    stoneSpan.innerText = stone.toString();
    wandSpan.innerText = wand.toString();
});

//Invoke Hub Method "AKA" Send Notification to Hub

//Start Connection
function fulfilled() {
    connectionDeathlyHallows.invoke("GetRaceStatus").then((raceCounter) => {
        cloakSpan.innerText = raceCounter.cloak.toString();
        stoneSpan.innerText = raceCounter.stone.toString();
        wandSpan.innerText = raceCounter.wand.toString();
    });
    console.log("Connection to User Hub Successfully!");

}
function rejected() {

}
connectionDeathlyHallows.start().then(fulfilled, rejected);