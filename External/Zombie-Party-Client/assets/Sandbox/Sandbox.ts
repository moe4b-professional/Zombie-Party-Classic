const { ccclass, property } = cc._decorator;

@ccclass
export default class Sandbox extends cc.Component
{
    start()
    {
        navigator.vibrate([100, 100, 100]);
    }
}