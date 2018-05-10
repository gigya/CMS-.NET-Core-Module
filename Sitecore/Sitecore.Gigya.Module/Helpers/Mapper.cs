﻿using Gigya.Module.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SC = Sitecore.Gigya.Module.Models;

using Core = Gigya.Module.Core;

namespace Sitecore.Gigya.Module.Helpers
{
    public class Mapper
    {
        public static SC.GigyaSettingsViewModel Map(Core.Mvc.Models.GigyaSettingsViewModel source)
        {
            return new SC.GigyaSettingsViewModel
            {
                ApiKey = source.ApiKey,
                DataCenter = source.DataCenter,
                DebugMode = source.DebugMode,
                ErrorMessage = source.ErrorMessage,
                GigyaScriptPath = source.GigyaScriptPath,
                Id = source.Id,
                IsGetInfoRequired = source.IsGetInfoRequired,
                IsLoggedIn = source.IsLoggedIn,
                LoggedInRedirectUrl = source.LoggedInRedirectUrl,
                LogoutUrl = source.LogoutUrl,
                RenderScript = source.RenderScript,
                Settings = source.Settings,
                SettingsJson = source.SettingsJson
            };
        }
    }
}