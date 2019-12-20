const {ccclass, property} = cc._decorator;

@ccclass("DataStructure")
export class DataStructure
{
    @property(Number)
    number : number = 10;

    @property(cc.Label)
    label : cc.Label = null;
}

@ccclass
export default class Sandbox extends cc.Component
{
    @property(DataStructure)
    data : DataStructure = new DataStructure();

    start()
    {
        cc.log(this.data.number);
    }
}