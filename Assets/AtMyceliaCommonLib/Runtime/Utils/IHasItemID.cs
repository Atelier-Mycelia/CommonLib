namespace AtMycelia
{
    public interface IHasItemID
    {
        object ItemId { get; set; }
    }

    public interface IHasItemId<T> : IHasItemID
    {
        new T ItemId { get; set; }
    }
}