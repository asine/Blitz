using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $rootnamespace$
{
    [UseView(typeof(ReportRunnerView))]
    public class $fileinputname$ReportRunnerViewModel : ReportRunnerViewModel<$fileinputname$ParameterViewModel, $fileinputname$ReportRunnerService, $fileinputname$ReportRunnerRequest, $fileinputname$ReportRunnerResponse>
    {
        public $fileinputname$ReportRunnerViewModel(ILog log, IViewService viewService, IDispatcherService dispatcherService, IToolBarService toolBarService, 
            $fileinputname$ReportParameterViewModel reportParameterViewModel, $fileinputname$ReportRunnerService reportRunnerService) 
            : base(log, viewService, dispatcherService, toolBarService, reportParameterViewModel, reportRunnerService)
        {
        }
    }
}