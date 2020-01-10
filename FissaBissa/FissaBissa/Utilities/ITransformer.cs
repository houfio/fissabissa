namespace FissaBissa.Utilities
{
    public interface ITransformer<T>
    {
        T Transform();

        void Copy(T data, bool create);
    }
}
