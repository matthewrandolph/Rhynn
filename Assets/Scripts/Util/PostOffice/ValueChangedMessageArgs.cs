namespace Util.PostOffice
{
    public class ValueChangedMessageArgs<T> : MessageArgs<T>
    {
        public T New { get; }
        public T Old { get; }
        
        public ValueChangedMessageArgs(T oldValue, T newValue)
        {
            Old = oldValue;
            New = newValue;
        }
    }
}