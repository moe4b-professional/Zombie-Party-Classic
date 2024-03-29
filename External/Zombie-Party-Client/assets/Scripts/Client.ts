import MoeEvent from "../Plugins/MoeEvent";
import { NetworkMessage, ReadyClientMessage, RegisterClientMessage, RetryLevelMessage, PingMessage } from "./Tools/NetworkMessage";

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

        var message = new RegisterClientMessage(value);
        this.send(message);
    }
    public static get defaultName() { return "Player"; }
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

        var message = new ReadyClientMessage(value);
        this.send(message);

        this.readyEvent.invoke(this._ready);
    }

    rtt: number;

    ping()
    {
        var message = new PingMessage();
        this.send(message);
    }

    pong(message: PingMessage)
    {
        var now = Date.now();
        this.rtt = now - message.stamp;

        console.log(this.rtt);

        this.ping();
    }

    public send<T extends NetworkMessage>(message: T): void
    {
        var json = NetworkMessage.Stringify(message);

        this.socket.send(json);
    }

    public connectEvent = new MoeEvent();
    onConnected(event: Event): void
    {
        this.rtt = 0;

        this.ping();

        this.connectEvent.invoke(event);
    }

    public messageEvent = new MoeEvent();
    onMessage(event: MessageEvent): void
    {
        this.socket.send('ACKNOWLEDGE BYTES');

        var message = NetworkMessage.Parse(event.data);

        if (message instanceof RetryLevelMessage)
            this.ready = false;

        if (message instanceof PingMessage)
            this.pong(message);

        this.messageEvent.invoke(message);
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