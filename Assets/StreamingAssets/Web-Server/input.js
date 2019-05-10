var keys = [];

function isKeyDown(key)
{
    for (var i = 0; i < keys.length; i++) {
        if(keys[i] == key)
            return true;
    }

    return false;
}

function getAxis(positive, negative)
{
    for (var i = 0; i < keys.length; i++) {
        if (keys[i] == positive) return 1;
        if (keys[i] == negative) return -1;
    }

    return 0;
}

var inputChanged = new Event("inputChanged");

window.addEventListener("keydown", OnKeyDown, true);
function OnKeyDown(event) {
    if (event.preventDefaulted) return;

    for (var i = 0; i < keys.length; i++) {
        if (keys[i] == event.key)
            return;
    }

    keys.push(event.key);
    dispatchEvent(inputChanged);
}

window.addEventListener("keyup", OnKeyUp, true);
function OnKeyUp(event) {
    if (event.preventDefaulted) return;

    for (var i = 0; i < keys.length; i++) {
        if (keys[i] == event.key)
        {
            keys.splice(i, 1);
            dispatchEvent(inputChanged);
            break;
        }
    }
}