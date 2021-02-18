export default class MoeEvent
{
    callbacks: MoeCallback[];

    constructor()
    {
        this.callbacks = new Array();
    }

    public add(callback: CallableFunction, ref: any): MoeCallback
    {
        var item = new MoeCallback(callback, ref);

        this.callbacks.push(item);

        return item;
    }

    public remove(callback): void
    {
        for (let index = 0; index < this.callbacks.length; index++)
        {
            const element = this.callbacks[index];

            if (element.callback == callback)
            {
                this.callbacks.splice(index, 1);
                break;
            }
        }
    }

    public invoke(value)
    {
        for (let index = 0; index < this.callbacks.length; index++)
            this.callbacks[index].invoke(value);
    }
}

class MoeCallback
{
    callback: CallableFunction;
    bind: CallableFunction;

    invoke(value)
    {
        this.bind(value);
    }

    constructor(callback: CallableFunction, reference: any)
    {
        this.callback = callback;

        this.bind = callback.bind(reference);
    }
}