import Game from "../Game";

import Client from "../Client";

import Menu from "./Menu";

const {ccclass, property} = cc._decorator;

@ccclass
export default class JoinMenu extends Menu
{
    @property(cc.EditBox)
    playerName : cc.EditBox = null;
    initPlayerName()
    {
        var changeEvent = new cc.EditBox.EventHandler();
        changeEvent.component = "JoinMenu";
        changeEvent.handler = "onPlayerNameEdit";
        changeEvent.target = this.node;
        this.playerName.textChanged.push(changeEvent);

        var editEndEvent = new cc.EditBox.EventHandler();
        editEndEvent.component = "JoinMenu";
        editEndEvent.handler = "onPlayerNameEditEnd";
        editEndEvent.target = this.node;
        this.playerName.editingDidEnded.push(editEndEvent);

        var value = cc.sys.localStorage.getItem("player name");

        if(value == null) value = Client.defaultName;

        this.playerName.string = value;

        this.onPlayerNameEdit();
    }

    @property(cc.Button)
    button : cc.Button = null;
    initButton()
    {
        var event = new cc.Button.EventHandler();
        event.component = "JoinMenu";
        event.handler = "onButtonClick";
        event.target = this.node;
        this.button.clickEvents.push(event);

        this.button.node.on("touchstart", this.onButtonTouched, this);
    }

    get game() { return Game.instance; }
    get client() { return this.game.client; }
    get popup() { return this.game.popup; }
    
    get hostName()
    {
        if(location.hostname == "") return "localhost";

        return location.hostname;
    }
    get address() { return "ws://" + this.hostName + ":" + this.game.port; }

    start ()
    {
        this.initButton();

        this.initPlayerName();
    }

    onButtonClick()
    {
        this.client.connectEvent.add(this.onConnected, this);
        this.client.disconnectEvent.add(this.onDisconnected, this);

        this.popup.display("Connecting", null, null);

        this.client.connect(this.address);
    }
    onButtonTouched()
    {
        if(Client.isValidName(this.playerName.string) == false)
        {
            this.popup.display("Invalid Name", this.popup.hide, "Close");
        }
    }

    onPlayerNameEdit()
    {
        this.button.interactable = Client.isValidName(this.playerName.string);
    }
    onPlayerNameEditEnd()
    {
        cc.sys.localStorage.setItem("player name", this.playerName.string);

        this.game.reload();
    }

    onConnected()
    {
        this.client.connectEvent.remove(this.onConnected);
        this.client.disconnectEvent.remove(this.onDisconnected);

        this.popup.visibile = false;

        this.game.canvas.fitWidth = true;
        this.game.canvas.fitHeight = false;

        this.client.name = this.playerName.string;

        this.visibile = false;
        this.game.controlMenu.visibile = true;
    }

    onDisconnected()
    {
        this.client.connectEvent.remove(this.onConnected);
        this.client.disconnectEvent.remove(this.onDisconnected);

        this.popup.display("Connection\nFailed", this.popup.hide, "Close");
    }
}