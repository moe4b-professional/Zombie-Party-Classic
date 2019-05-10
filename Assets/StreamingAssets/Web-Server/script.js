var playerName = document.getElementById("player name");
playerName.value = "Player";

var interact = document.getElementById("interact");
interact.value = "Join";
interact.onclick = onInteract;

var websocket;
function isConnected()
{
    if (websocket == undefined) return false;
    
    return websocket.readyState == websocket.OPEN;
}

var address = self.location.hostname;
var port = 8080;
function getURI()
{
    return "ws://" + address + ":" + 8080;
}

window.addEventListener("load", onLoad, false);
function onLoad() {

}

addEventListener("inputChanged", onInputChanged);
function onInputChanged(evt)
{
    if (isConnected())
    {
        var right = new Vector2(getAxis("d", "a"), getAxis("w", "s"));
        var left = new Vector2(0, 0);

        var input = new InputMessage(right, left);

        sendMessage(JSON.stringify(input));
		sendMessage("ready: true");
    }
}

function onInteract()
{
    if(isConnected())
    {
        websocket.close();
        interact.value = "Join";

        playerName.readOnly = false;
    }
    else
    {
        join();
    }
}

function join()
{
    websocket = new WebSocket(getURI());

    websocket.onopen = function (evt) { onOpen(evt) };
    websocket.onclose = function (evt) { onClose(evt) };
    websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };
}

function onOpen(evt) {
    log("socket opened");

    interact.value = "Leave";
    playerName.readOnly = true;

    sendMessage("Player Name: " + playerName.value);
}

function onMessage(evt) {
    log("recieved message: " + evt.data);

    var message = deserialize(evt.data, new InputMessage);

    log("input: " + message.right.x + ":" + message.right.y + " / " + message.left.x + ":" + message.left.y);

    message.right.x *= -1;
    message.right.y *= -1;

    message.left.x *= -1;
    message.left.y *= -1;

    sendMessage(JSON.stringify(message));
}

function sendMessage(message) {
    websocket.send(message);
}

function onError(evt) {
    log("Error: " + evt.data);
}

function onClose(evt) {
    log("socket closed");
}

//-----------------------------------------

function log(message) {
    document.getElementById("log").innerHTML += message + "<br/>";
}

function assign(source, destination)
{
    for (var prop in source) {
        if (destination.hasOwnProperty(prop)) {
            destination[prop] = source[prop];
        }
    }

    return destination;
}

function deserialize(json, destination)
{
    var source = JSON.parse(json);

    return assign(source, destination);
}

function Vector2(x, y)
{
    this.x = x;
    this.y = y;
}

function InputMessage(right, left)
{
    this.ID = 10;
    this.right = right;
    this.left = left;
}

function HealthMessage(value, max)
{
    this.ID = 11;
    this.value = value;
    this.max = max;
}