using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using GongSolutions.Wpf.DragDrop;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutDropTarget : IDropTarget
    {
        private readonly Func<ReportLayoutItemViewModel, bool> _predicate;
        private readonly ObservableCollection<ReportLayoutItemViewModel> _target;
        private readonly List<ObservableCollection<ReportLayoutItemViewModel>> _sources;

        public ReportLayoutDropTarget(Func<ReportLayoutItemViewModel, bool> predicate, 
            ObservableCollection<ReportLayoutItemViewModel> target, 
            params ObservableCollection<ReportLayoutItemViewModel>[] sources)
        {
            _predicate = predicate;
            _target = target;
            _sources = sources.ToList();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var item = dropInfo.Data as ReportLayoutItemViewModel;
            if (item == null) return;

            if (!_predicate(item)) return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var item = dropInfo.Data as ReportLayoutItemViewModel;
            if (item == null) return;

            var existing = _target.FirstOrDefault(x => x.Id == item.Id);
            if (existing != null) return;

            foreach (var source in _sources)
            {
                source.Remove(item);
            }

            _target.Add(item);
        }
    }
}