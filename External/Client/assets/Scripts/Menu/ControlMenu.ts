import Game from "../Game";

import Menu from "./Menu";

import VirtualJoystick from "../../Plugins/Virtual Joystick/VirtualJoystick";

const {ccclass, property} = cc._decorator;

@ccclass
export default class ControlMenu extends Menu
{
    @property(Menu)
    initial : Menu = null;

    @property(Menu)
    HUD : Menu = null;

    @property(Menu)
    death : Menu = null;

    setActiveMenu(target : Menu)
    {
        this.initial.visibile = target == this.initial;
        this.HUD.visibile = target == this.HUD;
        this.death.visibile = target == this.death;
    }

    @property(VirtualJoystick)
    leftStick : VirtualJoystick = null;

    @property(VirtualJoystick)
    rightStick : VirtualJoystick = null;

    @property(cc.ProgressBar)
    healthBar : cc.ProgressBar = null;

    @property(cc.Label)
    healthLabel : cc.Label = null;

    get game() { return Game.instance; }
    get client() { return this.game.client; }
    get popup() { return this.game.popup; }

    onEnable()
    {
        this.setActiveMenu(this.initial);
        this.healthBar.node.active = false;

        this.client.disconnectEvent.add(this.onDisconnect, this);
        this.client.commandEvent.add(this.onCommand, this);
        this.client.networkMessageEvent.add(this.onNetworkMessage, this);

        this.leftStick.onValueChanged.add(this.updateInput, this);
        this.rightStick.onValueChanged.add(this.updateInput, this);
    }

    updateInput()
    {
        var message = new InputMessage(this.leftStick.value, this.rightStick.value);

        this.client.send(message.getJSON());
    }

    onCommand(text : string)
    {
        if(text == "start")
        {
            this.setActiveMenu(this.HUD);
        }

        if(text == "retry")
        {
            this.setActiveMenu(this.initial);
            this.healthBar.node.active = false;
        }
    }

    onNetworkMessage(data : any)
    {
        if(data["ID"] == 11)
        {
            var msg = NetworkMessage.Parse(data, new HealthMessage());

            this.healthBar.progress = msg.value / msg.max;
            this.healthBar.node.active = true;
            this.healthLabel.string = msg.value.toFixed(1) + "/" + msg.max.toFixed(1);

            if(msg.value != msg.max)
            {
                
            }

            if(msg.value == 0) this.onDeath();
        }
    }

    onDeath()
    {
        this.setActiveMenu(this.death);
    }

    onDisconnect()
    {
        this.client.disconnectEvent.remove(this.onDisconnect);

        this.popup.display("Disconnected", this.game.reload, "Return");
    }
}

import NetworkMessage from "../Tools/NetworkMessage";

class InputMessage extends NetworkMessage
{
    ID = 10;
    left : cc.Vec2
    right : cc.Vec2;

    constructor(left : cc.Vec2, right : cc.Vec2)
    {
        super(10);
        this.left = left;
        this.right = right;
    }
}

class HealthMessage extends NetworkMessage
{
    value : number;
    max : number;
    
    constructor()
    {
        super(11);
    }
}