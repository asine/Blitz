using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Blitz.Client.Common.DynamicReportData
{
    public class DynamicGridColumnBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof (IEnumerable<DynamicColumn>), typeof (DynamicGridColumnBehavior), new PropertyMetadata(default(IEnumerable<DynamicColumn>), ColumnsPropertyChangedCallback));

        private static void ColumnsPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as DynamicGridColumnBehavior;
            if (behavior == null) return;

            behavior.InitialiseColumns();
        }

        public IEnumerable<DynamicColumn> Columns
        {
            get { return (IEnumerable<DynamicColumn>) GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        private void InitialiseColumns()
        {
            AssociatedObject.Columns.Clear();

            foreach (var column in Columns.Where(x => x.IsVisible))
            {
                var dataGridColumn = new DataGridTextColumn
                {
                    Header = column.HeaderName,
                    Binding = new Binding(column.PropertyName)
                };

                AssociatedObject.Columns.Add(dataGridColumn);
            }
        }
    }
}