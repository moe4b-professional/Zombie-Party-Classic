import MoeEvent from "../Plugins/MoeEvent";

const { ccclass, property } = cc._decorator;

@ccclass
export default class Client
{
    socket: WebSocket;

    public get isConnected(): boolean
    {
        if (this.socket == null) return false;

        return this.socket.readyState == this.socket.OPEN;
    }

    connect(URL: string): void
    {
        this.socket = new WebSocket(URL);

        this.socket.onopen = this.onConnected.bind(this);
        this.socket.onmessage = this.onMessage.bind(this);
        this.socket.onerror = this.onError.bind(this);
        this.socket.onclose = this.onDisconnect.bind(this);
    }

    _name: string;
    public get name(): string { return this._name; }
    public set name(value: string)
    {
        this._name = value;
        this.send("player name: " + this._name);
    }
    public static get defaultName() { return "Player Name"; }
    public static isValidName(value: string): boolean
    {
        if (value == null) return false;

        if (value.length < 1) return false;

        return true;
    }

    public readyEvent = new MoeEvent();
    _ready: boolean = false;
    public get ready(): boolean { return this._ready; }
    public set ready(value: boolean)
    {
        this._ready = value;

        this.send("ready: " + this._ready);

        this.readyEvent.invoke(this._ready);
    }

    public send(data: string): void
    {
        this.socket.send(data);
    }

    public connectEvent = new MoeEvent();
    onConnected(event: Event): void
    {
        this.connectEvent.invoke(event);
    }

    public messageEvent = new MoeEvent();
    onMessage(event: MessageEvent): void
    {
        this.messageEvent.invoke(event);

        if (typeof event.data === 'string')
        {
            var text = event.data as string;

            if (text.length > 0)
            {
                if (text.charAt(0) == '#')
                    this.onCommand(text.slice(1).toLowerCase());
                else if (text.includes("\"ID\":"))
                    this.onNetworkMessage(text);
            }
        }
    }

    public commandEvent = new MoeEvent();
    onCommand(value: string)
    {
        if (value == "retry")
            this.ready = false;

        this.commandEvent.invoke(value);
    }

    public networkMessageEvent = new MoeEvent();
    onNetworkMessage(value: string)
    {
        var msg = JSON.parse(value);

        this.networkMessageEvent.invoke(msg);
    }

    public errorEvent = new MoeEvent();
    onError(event: Event): void
    {
        this.errorEvent.invoke(event);
    }

    public disconnectEvent = new MoeEvent();
    onDisconnect(event: CloseEvent): void
    {
        this.disconnectEvent.invoke(event);
    }
}