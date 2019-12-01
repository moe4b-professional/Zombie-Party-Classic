export default class NetworkMessage
{
    ID : number;

    public getJSON() : string
    {
        return JSON.stringify(this);
    }

    public static Parse<T>(data : any, msg : T) : T
    {
        for (var property in data)
            msg[property] = data[property]
        
        return msg;
    }

    constructor(ID : number)
    {
        this.ID = ID;
    }
}