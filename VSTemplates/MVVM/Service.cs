using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace $rootnamespace$.$fileinputname$
{
    public interface I$fileinputname$Service : IService
    {
    }

    public class $fileinputname$Service : Service, I$fileinputname$Service
    {
        public $fileinputname$Service(ILog log)
	    : base(log)
        {
        }
    }
}