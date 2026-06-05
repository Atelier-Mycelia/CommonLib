namespace AtMycelia
{
    public interface IHasItemId
    {
        object ItemId { get; set; }
    }

    public interface IHasItemId<T> : IHasItemId
    {
        new T ItemId { get; set; }
    }
}