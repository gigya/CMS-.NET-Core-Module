﻿using Gigya.Module.Core.Connector.Events;
using Gigya.Module.Core.Connector.Logging;
using Gigya.Module.Core.Connector.Models;
using Gigya.Module.Core.Data;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Gigya.Module.Pipelines
{
    public class AccountInfoMergeCompletedPipelineArgs : GetAccountInfoPipelineArgs
    {
        public AccountInfoMergeCompletedEventArgs EventArgs { get; set; }
    }
}