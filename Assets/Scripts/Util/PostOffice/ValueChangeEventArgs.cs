namespace Util.PostOffice
{
    public class ValueChangeEventArgs<T> : EventArgs<T>
    {
        public T New { get; }
        public T Old { get; }
        
        public ValueChangeEventArgs(T oldValue, T newValue)
        {
            Old = oldValue;
            New = newValue;
        }
    }
}