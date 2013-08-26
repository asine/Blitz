namespace Blitz.Client.Core.EPPlus
{
    public class DataWriterModel
    {
        public static DataWriterModel Default
        {
            get { return new DataWriterModel(1, 1); }
        }

        public int StartColumn { get; private set; }

        public int StartRow { get; private set; }

        public int CurrentColumn { get; private set; }

        public int CurrentRow { get; private set; }

        public DataWriterModel(int startColumn, int startRow)
        {
            StartRow = startRow;
            StartColumn = startColumn;

            CurrentRow = StartRow;
            CurrentColumn = StartColumn;
        }

        public int NextRow()
        {
            CurrentRow++;
            return CurrentRow;
        }

        public int NextColumn()
        {
            CurrentColumn++;
            return CurrentColumn;
        }

        public int RestartColumns()
        {
            CurrentColumn = StartColumn;
            return CurrentColumn;
        }
    }
}