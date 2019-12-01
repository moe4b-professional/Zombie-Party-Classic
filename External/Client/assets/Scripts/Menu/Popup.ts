import Menu from "./Menu";

const {ccclass, property} = cc._decorator;

@ccclass
export default class Popup extends Menu
{
    @property(cc.Label)
    label : cc.Label = null;
    set text(value) { this.label.string = value; }

    @property(cc.Button)
    button : cc.Button = null;
    buttonLabel : cc.Label;
    set interactable(value : boolean) { this.button.node.active = value; }
    initButton()
    {
        this.buttonLabel = this.button.getComponentInChildren(cc.Label);

        var eventHandler = new cc.Button.EventHandler();

        eventHandler.component = "Popup";
        eventHandler.handler = "onButtonClick";
        eventHandler.target = this.node;

        this.button.clickEvents.push(eventHandler);
    }

    init()
    {
        this.initButton();

        this.visibile = false;
    }

    action : CallableFunction = null;
    onButtonClick()
    {
        if(this.action != null) this.action();
    }

    display(text, action:CallableFunction, buttonText)
    {
        this.text = text;

        if(action == null)
        {
            this.interactable = false;
        }
        else
        {
            this.action = action;
            this.interactable = true;
            this.buttonLabel.string = buttonText;
        }

        if(this.visibile == false)
            this.visibile = true;
    }
}