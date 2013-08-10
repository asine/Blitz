namespace Blitz.Client.Core.MVVM
{
    public interface IViewModel
    {
        string DisplayName { get; }

        void Activate();
        void DeActivate();
    }
}