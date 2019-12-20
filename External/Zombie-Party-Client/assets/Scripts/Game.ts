import Menu from "./Menu/Menu";

import TitleMenu from "./Menu/TitleMenu";
import JoinMenu from "./Menu/JoinMenu";
import ControlMenu from "./Menu/ControlMenu";

import Popup from "./Menu/Popup";

import Client from "./Client";

const { ccclass, property } = cc._decorator;

@ccclass
export default class Game extends cc.Component
{
    static instance: Game;

    @property(cc.Canvas)
    canvas: cc.Canvas = null;

    @property(Menu)
    titleMenu: TitleMenu = null;

    @property(Menu)
    joinMenu: JoinMenu = null;

    @property(ControlMenu)
    controlMenu: ControlMenu = null;

    @property(Popup)
    popup: Popup = null;

    client: Client = null;

    onLoad()
    {
        Game.instance = this;

        this.titleMenu.visibile = false;
        this.joinMenu.visibile = false;
        this.controlMenu.visibile = false;
        this.popup.init();

        this.popup.display("Retrieving Resources", null, null);
        Request("RSC", "Server Port", this.onServerPortRecieved, this);

        this.client = new Client();
    }

    port: number;
    onServerPortRecieved(request: XMLHttpRequest)
    {
        if (request.readyState == 4 && request.status == 200)
        {
            this.popup.hide();

            this.port = request.response as number;

            this.titleMenu.visibile = true;
        }
        else
        {
            this.popup.display("Failure Retrieving Resources", this.reload, "Retry");
        }
    }

    start()
    {

    }

    reload()
    {
        cc.director.loadScene(cc.director.getScene().name);
    }
}

function Request(method: string, path: string, callback: CallableFunction, context: object)
{
    var request = cc.loader.getXMLHttpRequest();

    request.open("RSC", "Server Port", true);

    request.onreadystatechange = function ()
    {
        callback.bind(context)(request);
    };

    request.send();
}