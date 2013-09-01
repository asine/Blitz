﻿using System;
using System.Linq;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;
using Blitz.Common.Customer;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutViewModel : Workspace
    {
        private readonly IReportLayoutService _service;
        private readonly IViewService _viewService;
        private readonly Func<ReportLayoutItemViewModel> _reportLayoutItemViewModelFactory;

        public BindableCollection<ReportLayoutItemViewModel> Available { get; private set; }

        public BindableCollection<ReportLayoutItemViewModel> Rows { get; private set; }

        public BindableCollection<ReportLayoutItemViewModel> Columns { get; private set; }

        public ReportLayoutDropTarget ToRowsDropTarget { get; private set; }

        public ReportLayoutDropTarget ToColumnsDropTarget { get; private set; }

        public ReportLayoutDropTarget ToAvailableDropTarget { get; private set; }

        public DelegateCommand OkCommand { get; private set; }

        public ReportLayoutViewModel(ILog log, IDispatcherService dispatcherService,
            IReportLayoutService service, IViewService viewService, BindableCollectionFactory bindableCollectionFactory, 
            Func<ReportLayoutItemViewModel> reportLayoutItemViewModelFactory)
            : base(log, dispatcherService)
        {
            _service = service;
            _viewService = viewService;
            _reportLayoutItemViewModelFactory = reportLayoutItemViewModelFactory;

            DisplayName = "Layout";

            Available = bindableCollectionFactory.Get<ReportLayoutItemViewModel>();
            Rows = bindableCollectionFactory.Get<ReportLayoutItemViewModel>();
            Columns = bindableCollectionFactory.Get<ReportLayoutItemViewModel>();

            ToRowsDropTarget = new ReportLayoutDropTarget(x => x.Type == AttributeType.Dimension, Rows, Available, Columns);
            ToColumnsDropTarget = new ReportLayoutDropTarget(x => x.Type == AttributeType.Measure, Columns, Available, Rows);
            ToAvailableDropTarget = new ReportLayoutDropTarget(x => true, Available, Columns, Rows);

            OkCommand = new DelegateCommand(Close);
        }

        protected override void OnInitialise()
        {
            BusyAsync("... Loading Attributes ...")
                .Then(_ => _service.GetAttributes())
                .ThenDo(response =>
                {
                    Available.AddRange(response.Dimensions.Select(CreateDimension));
                    Available.AddRange(response.Measures.Select(CreateMeasure));
                })
                .LogException(Log)
                .CatchAndHandle(_ => _viewService.StandardDialogBuilder().Error("Error", "Problem initialising attributes"))
                .Finally(Idle);
        }

        private ReportLayoutItemViewModel CreateDimension(AttributeDto dimension)
        {
            var item = _reportLayoutItemViewModelFactory();
            item.DisplayName = dimension.Name;
            item.Type = AttributeType.Dimension;
            return item;
        }

        private ReportLayoutItemViewModel CreateMeasure(AttributeDto measure)
        {
            var item = _reportLayoutItemViewModelFactory();
            item.DisplayName = measure.Name;
            item.Type = AttributeType.Measure;
            return item;
        }
    }
}