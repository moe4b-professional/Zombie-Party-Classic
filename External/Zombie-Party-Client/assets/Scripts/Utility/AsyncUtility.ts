export class AsyncUtility
{
    static sleep(ms: number): Promise<unknown>
    {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}