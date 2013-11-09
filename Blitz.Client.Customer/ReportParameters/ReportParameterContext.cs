﻿using System;

using Naru.WPF.Wizard;

namespace Blitz.Client.Customer.ReportParameters
{
    public class ReportParameterContext : WizardContext
    {
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                CanFinish = true;
            }
        }
    }
}