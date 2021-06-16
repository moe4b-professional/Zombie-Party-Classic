import { Glossary } from "../Utility/UtilityCollections";

export abstract class NetworkMessage
{
    public ID: number;

    constructor(type: any)
    {
        this.ID = NetworkMessage.GetID(type);
    }

    //Static Utility

    public static Stringify<T extends NetworkMessage>(message: T)
    {
        return JSON.stringify(message);
    }

    public static Parse(json: string): NetworkMessage
    {
        var jObject = JSON.parse(json);

        var id = jObject["ID"];

        var constructor = NetworkMessage.GetType(id);

        var message = new constructor() as NetworkMessage;

        for (var property in jObject)
            message[property] = jObject[property]

        return message;
    }

    static glossary = new Glossary<number, any>();

    static Register(id: number, type: any)
    {
        NetworkMessage.glossary.Add(id, type);
    }

    public static GetType(id: number): any
    {
        var type = NetworkMessage.glossary.GetValue(id);

        if (type == undefined) throw `ID ${id} not Registerd with Any Type`;

        return type;
    }

    public static GetID(type: any): number
    {
        var id = NetworkMessage.glossary.GetKey(type);

        if (id == undefined) throw `Type ${type} Not Registered as NetworkMessage`;

        return id;
    }

    static Configure()
    {
        let index = 0;

        function Add(type: any): void
        {
            NetworkMessage.Register(index, type);

            index += 1;
        }

        Add(RegisterClientMessage);
        Add(ReadyClientMessage);

        Add(StartLevelMessage);
        Add(RetryLevelMessage);

        Add(PlayerInputMessage);
        Add(PlayerHealthMessage);

        Add(HitMarkerMessage);

        Add(PingMessage);

        Add(ScoreMessage);
    }
}

//#region Client
export class RegisterClientMessage extends NetworkMessage
{
    name: string;

    constructor(name: string)
    {
        super(RegisterClientMessage);

        this.name = name;
    }
}

export class ReadyClientMessage extends NetworkMessage
{
    value: boolean;

    constructor(value: boolean)
    {
        super(ReadyClientMessage);

        this.value = value;
    }
}
//#endregion

//#region Level
export class StartLevelMessage extends NetworkMessage
{
    constructor()
    {
        super(StartLevelMessage);
    }
}

export class RetryLevelMessage extends NetworkMessage
{
    constructor()
    {
        super(RetryLevelMessage);
    }
}
//#endregion

//#region Player
export class PlayerInputMessage extends NetworkMessage
{
    left: cc.Vec2
    right: cc.Vec2;

    constructor(left: cc.Vec2, right: cc.Vec2)
    {
        super(PlayerInputMessage);

        this.left = left;
        this.right = right;
    }
}

export class PlayerHealthMessage extends NetworkMessage
{
    value: number;
    max: number;

    constructor()
    {
        super(PlayerHealthMessage);
    }
}
//#endregion

export class HitMarkerMessage extends NetworkMessage
{
    pattern: number[];

    hit: boolean;

    constructor()
    {
        super(HitMarkerMessage);
    }
}

export class PingMessage extends NetworkMessage
{
    stamp: number;

    constructor()
    {
        super(PingMessage);

        this.stamp = Date.now();
    }
}

export class ScoreMessage extends NetworkMessage
{
    value: number;

    constructor()
    {
        super(ScoreMessage);
    }
}

NetworkMessage.Configure();