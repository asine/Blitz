using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $rootnamespace$
{
    public class $fileinputname$ReportRunnerService : ReportRunnerService<$fileinputname$ReportParameterViewModel, $fileinputname$ReportRunnerRequest, $fileinputname$ReportRunnerResponse>
    {
        public $fileinputname$ReportRunnerService(IToolBarService toolBarService, ILog log) : base(toolBarService, log)
        {
        }

        public override Task ConfigureParameterViewModelAsync($fileinputname$ReportParameterViewModel viewModel)
        {
            return TaskEx.Completed();
        }

        public override $fileinputname$ReportRunnerRequest CreateRequest($fileinputname$ReportParameterViewModel reportParameterViewModel)
        {
            return new $fileinputname$ReportRunnerRequest();
        }

        public override Task<$fileinputname$ReportRunnerResponse> GenerateAsync($fileinputname$ReportRunnerRequest request)
        {
            return TaskEx.Completed<$fileinputname$ReportRunnerResponse>();
        }

        public override Task<List<IViewModel>> GenerateDataViewModelsAsync($fileinputname$ReportRunnerResponse response)
        {
            return TaskEx.Completed<List<IViewModel>>();
        }
    }
}