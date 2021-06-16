import Game from "../Game";

import Menu from "./Menu";

import VirtualJoystick from "../../Plugins/Virtual Joystick/VirtualJoystick";
import { NetworkMessage, PlayerHealthMessage, PlayerInputMessage, StartLevelMessage, RetryLevelMessage, HitMarkerMessage, ScoreMessage } from "../Tools/NetworkMessage";
import { AsyncUtility } from "../Utility/AsyncUtility";

const { ccclass, property } = cc._decorator;

@ccclass
export default class ControlMenu extends Menu
{
    @property(Menu)
    initial: Menu = null;

    @property(Menu)
    HUD: Menu = null;

    @property(Menu)
    death: Menu = null;

    setActiveMenu(target: Menu)
    {
        this.initial.visibile = target == this.initial;
        this.HUD.visibile = target == this.HUD;
        this.death.visibile = target == this.death;
    }

    @property(VirtualJoystick)
    leftStick: VirtualJoystick = null;

    @property(VirtualJoystick)
    rightStick: VirtualJoystick = null;

    @property(cc.ProgressBar)
    healthBar: cc.ProgressBar = null;

    @property(cc.Label)
    healthLabel: cc.Label = null;

    @property(cc.AudioClip)
    hitmarker: cc.AudioClip = null;

    @property(cc.Label)
    scoreLabel: cc.Label = null;

    get game() { return Game.instance; }
    get client() { return this.game.client; }
    get popup() { return this.game.popup; }

    start()
    {
        this.setActiveMenu(this.initial);
        this.healthBar.node.active = false;

        this.client.disconnectEvent.add(this.onDisconnect, this);
        this.client.messageEvent.add(this.onMessage, this);

        this.leftStick.onValueChanged.add(this.updateInput, this);
        this.rightStick.onValueChanged.add(this.updateInput, this);
    }

    updateInput()
    {
        var message = new PlayerInputMessage(this.leftStick.value, this.rightStick.value);
        this.client.send(message);
    }

    onMessage(message: NetworkMessage)
    {
        if (message instanceof PlayerHealthMessage)
        {
            this.healthBar.progress = message.value / message.max;
            this.healthBar.node.active = true;
            this.healthLabel.string = message.value.toFixed(1) + "/" + message.max.toFixed(1);

            if (message.value == 0) this.onDeath();

            return;
        }

        if (message instanceof ScoreMessage)
        {
            this.scoreLabel.string = "Score: " + message.value;
            return;
        }

        if (message instanceof HitMarkerMessage)
        {
            navigator.vibrate(message.pattern);

            if (message.hit)
                cc.audioEngine.play(this.hitmarker, false, 1);

            return;
        }

        if (message instanceof StartLevelMessage)
        {
            this.setActiveMenu(this.HUD);

            return;
        }

        if (message instanceof RetryLevelMessage)
        {
            this.setActiveMenu(this.initial);
            this.healthBar.node.active = false;

            return;
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