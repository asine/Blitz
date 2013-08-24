using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.ModernUI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link : Displayable
    {
        public DelegateCommand Command { get; set; }
    }
}