import Game from "../Game";

const { ccclass, property } = cc._decorator;

@ccclass
export default class PingLabel extends cc.Component
{
    @property(cc.Label)
    label: cc.Label = null;

    get game() { return Game.instance; }
    get client() { return this.game.client; }

    update()
    {
        this.label.string = `RTT: ${this.client.rtt}`;
    }
}