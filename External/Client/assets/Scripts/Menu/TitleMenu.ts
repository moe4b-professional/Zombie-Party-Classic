import Menu from "./Menu";

import Game from "../Game";

const {ccclass, property} = cc._decorator;

@ccclass
export default class TitleMenu extends Menu
{
    static beenSeen : boolean = false;

    get game() { return Game.instance; }
    start ()
    {
        if(TitleMenu.beenSeen)
        {
            this.visibile = false;
            this.game.joinMenu.visibile = true;
        }
        else
        {
            TitleMenu.beenSeen = true;
            this.node.on("touchstart", this.onClick, this);
        }
    }

    onClick()
    {
        this.visibile = false;
        this.game.joinMenu.visibile = true;
    }
}