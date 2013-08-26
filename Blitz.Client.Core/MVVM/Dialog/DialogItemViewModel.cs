namespace Blitz.Client.Core.MVVM.Dialog
{
    public class DialogItemViewModel<T>
    {
        public T Response { get; set; }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }
    }
}