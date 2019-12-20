import MoeEvent from "../MoeEvent";

const {ccclass, property} = cc._decorator;

@ccclass
export default class VirtualJoystick extends cc.Component
{
    @property(cc.Sprite)
    body : cc.Sprite = null;
    initialPosition : cc.Vec2;

    @property(cc.Sprite)
    pointer : cc.Sprite = null;

    _value = cc.Vec2.ZERO;
    onValueChanged = new MoeEvent();
    get value() : cc.Vec2
    {
        return this._value;
    }
    set value(value : cc.Vec2)
    {
        if(value.mag() > 1) value = value.normalize();

        this._value = value;

        this.onValueChanged.invoke(this.value);
    }

    start()
    {
        this.initialPosition = this.body.node.position;

        this.node.on("touchstart", this.onTouchStart, this);
        this.node.on("touchmove", this.onTouchMove, this);
        this.node.on("touchend", this.onTouchEnd, this);
        this.node.on("touchcancel", this.onTouchCancel, this);
    }

    touchID : number = null;

    onTouchStart(event : cc.Event.EventTouch)
    {
        if(this.touchID != null) return;

        let location = event.getLocation();
        this.body.node.position = this.node.convertToNodeSpaceAR(location);

        this.touchID = event.getID();
    }

    onTouchMove(event : cc.Event.EventTouch)
    {
        if(event.getID() != this.touchID) return;

        let location = event.getLocation();
        this.pointer.node.position = this.body.node.convertToNodeSpaceAR(location);

        var size = this.body.node.getContentSize();
        var length = (size.width + size.height) / 4;

        this.pointer.node.position = VirtualJoystick.clampVector(this.pointer.node.position, length);

        this.value = VirtualJoystick.divideVector(this.pointer.node.position, length);
    }

    onTouchEnd(event : cc.Event.EventTouch)
    {
        if(event.getID() != this.touchID) return;

        this.reset();
    }
    onTouchCancel(event : cc.Event.EventTouch)
    {
        this.onTouchEnd(event);
    }

    reset()
    {
        if(this.initialPosition != null)
            this.body.node.position = this.initialPosition;
            
        this.pointer.node.position = cc.Vec2.ZERO;

        this.value = this.pointer.node.position;

        this.touchID = null;
    }

    onDisable()
    {
        this.reset();
    }

    public static clampVector(vector : cc.Vec2, length : number)
    {
        if(vector.mag() > length)
        {
            vector = vector.normalize();

            vector.x *= length;
            vector.y *= length;
        }

        return vector;
    }

    public static divideVector(vector : cc.Vec2, number : number)
    {
        vector.x /= number;
        vector.y /= number;

        return vector;
    }
}