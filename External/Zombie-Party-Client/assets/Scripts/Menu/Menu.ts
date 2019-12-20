import MoeEvent from "../../Plugins/MoeEvent";

const {ccclass, property} = cc._decorator;

@ccclass
export default class Menu extends cc.Component
{
    get visibile() { return this.getVisible(); }
    getVisible() { return this.node.activeInHierarchy; }

    set visibile(value : boolean) { this.setVisible(value); }
    setVisible(value : boolean)
    {
        if(value)
            this.show();
        else
            this.hide();
    }

    onVisibilityChanged = new MoeEvent();

    onShow = new MoeEvent();
    show()
    {
        this.node.active = true;

        this.onShow.invoke(null);
        this.onVisibilityChanged.invoke(true);
    }

    onHide = new MoeEvent();
    hide()
    {
        this.node.active = false;

        this.onShow.invoke(null);
        this.onVisibilityChanged.invoke(false);
    }

    toggle()
    {
        this.visibile = !this.visibile;
    }
}