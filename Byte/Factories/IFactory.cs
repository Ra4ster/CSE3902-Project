namespace Byte.Factory
{
    /// <typeparam name="TData">Serialized description of the object</typeparam>
    /// <typeparam name="TObject">The object to return upon creation</typeparam>
    public interface IFactory<in TData, out TObject>
    {
        TObject Create(TData data);
    }
}