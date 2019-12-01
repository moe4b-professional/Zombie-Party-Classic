import Game from "../Game";

const {ccclass, property} = cc._decorator;

@ccclass
export default class ReadyOnButton extends cc.Component
{
    button : cc.Button;
    background : cc.Sprite;
    label : cc.Label;

    @property
    readyColor : cc.Color = cc.Color.GREEN;

    @property
    unreadyColor : cc.Color = cc.Color.RED;

    get game() { return Game.instance; }
    get client() { return this.game.client; }

    start ()
    {
        this.button = this.getComponent(cc.Button);
        this.background = this.getComponentInChildren(cc.Sprite);
        this.label = this.getComponentInChildren(cc.Label);

        var event = new cc.Button.EventHandler();
        event.target = this.node;
        event.component = "ReadyOnButton";
        event.handler = "onButtonClick";
        this.button.clickEvents.push(event);

        this.client.readyEvent.add(this.onReadyChanged, this);

        this.UpdateState();
    }

    onButtonClick()
    {
        this.client.ready = !this.client.ready;
    }

    onReadyChanged()
    {
        this.UpdateState();
    }

    UpdateState()
    {
        if(this.client.ready)
        {
            this.label.string = "Ready";
            this.background.node.color = this.readyColor;
        }
        else
        {
            this.label.string = "UnReady";
            this.background.node.color = this.unreadyColor;
        }
    }
}