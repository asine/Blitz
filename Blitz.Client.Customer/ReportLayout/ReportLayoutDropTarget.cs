using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using GongSolutions.Wpf.DragDrop;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutDropTarget : IDropTarget
    {
        private readonly ObservableCollection<ReportLayoutItemViewModel> _target;
        private List<ObservableCollection<ReportLayoutItemViewModel>> _sources;

        public ReportLayoutDropTarget(ObservableCollection<ReportLayoutItemViewModel> target, 
            params ObservableCollection<ReportLayoutItemViewModel>[] sources)
        {
            _target = target;
            _sources = sources.ToList();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var item = dropInfo.Data as ReportLayoutItemViewModel;
            if (item == null) return;

            foreach (var source in _sources)
            {
                source.Remove(item);
            }

            _target.Add(item);
        }
    }
}