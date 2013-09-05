namespace Blitz.Client.Core.TPL
{
    public class Unit
    {
        private static readonly Unit _default = new Unit();

        public static Unit Default
        {
            get { return _default; }
        }
    }
}