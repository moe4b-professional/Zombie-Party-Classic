export class Glossary<TKey, TValue>
{
    keys: Map<TKey, TValue> = new Map<TKey, TValue>();
    values: Map<TValue, TKey> = new Map<TValue, TKey>();

    public GetValue(key: TKey): TValue | undefined
    {
        return this.keys.get(key);
    }

    public GetKey(value: TValue): TKey | undefined
    {
        return this.values.get(value);
    }

    public Add(key: TKey, value: TValue)
    {
        if (this.keys.has(key)) throw `Key ${key} Already Added to ${this}`;
        if (this.values.has(value)) throw `Value ${value} Already Added to ${this}`;

        this.keys.set(key, value);
        this.values.set(value, key);
    }

    public Clear(): void
    {
        this.keys.clear();
        this.values.clear();
    }

    constructor()
    {

    }
}