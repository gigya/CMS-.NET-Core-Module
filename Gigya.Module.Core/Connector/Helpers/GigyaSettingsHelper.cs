﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gigya.Module.Core.Data;
using Gigya.Module.Core.Connector.Encryption;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using Gigya.Module.Core.Mvc.Models;
using Newtonsoft.Json;
using System.Dynamic;
using Gigya.Module.Core.Connector.Common;

using System.Web.Mvc;
using Gigya.Module.Core.Connector.Logging;

namespace Gigya.Module.Core.Connector.Helpers
{
    public abstract class GigyaSettingsHelper
    {
        protected abstract string Language(IGigyaModuleSettings settings);
        protected abstract string ScriptPath(IGigyaModuleSettings settings);
        public abstract string CmsVersion { get; }
        public abstract string ModuleVersion { get; }

        /// <summary>
        /// Gets the Gigya module settings for the current site.
        /// </summary>
        /// <param name="decrypt">Whether to decrypt the application secret.</param>
        public abstract IGigyaModuleSettings GetForCurrentSite(bool decrypt = false);

        /// <summary>
        /// Deletes the Gigya module settings for the site with id of <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id of the site whose settings will be deleted.</param>
        public abstract void Delete(object id);

        protected abstract List<IGigyaModuleSettings> GetForSiteAndDefault(object id);

        protected abstract IGigyaModuleSettings EmptySettings(object id);

        /// <summary>
        /// Creates a view model for use in a view.
        /// </summary>
        /// <param name="settings">The settings for the current site.</param>
        /// <param name="urlHelper">UrlHelper for the current request.</param>
        public virtual GigyaSettingsViewModel ViewModel(IGigyaModuleSettings settings, UrlHelper urlHelper)
        {
            var scriptName = settings.DebugMode ? "gigya-cms.js" : "gigya-cms.min.js";

            var model = new GigyaSettingsViewModel
            {
                ApiKey = settings.ApiKey,
                DebugMode = settings.DebugMode,
                GigyaScriptPath = UrlUtils.AddQueryStringParam(ScriptPath(settings), "v=" + CmsVersion)
            };
            
            model.Settings = !string.IsNullOrEmpty(settings.GlobalParameters) ? JsonConvert.DeserializeObject<dynamic>(settings.GlobalParameters) : new ExpandoObject();
            model.Settings.lang = Language(settings);
            model.Settings.sessionExpiration = settings.SessionTimeout;
            model.SettingsJson = JsonConvert.SerializeObject(model.Settings);
            return model;
        }

        /// <summary>
        /// Gets Gigya Module settings for <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The Id of the site.</param>
        /// <param name="decrypt">Whether to decrypt the application secret.</param>
        public virtual IGigyaModuleSettings Get(object id, bool decrypt = false)
        {
            IGigyaModuleSettings settings = null;
            
            var siteSettingsAndGlobal = GetForSiteAndDefault(id);
            settings = siteSettingsAndGlobal.OrderByDescending(i => i.Id).FirstOrDefault() ?? EmptySettings(id);

            // decrypt application secret
            if (decrypt && !string.IsNullOrEmpty(settings.ApplicationSecret))
            {
                settings.ApplicationSecret = Encryptor.Decrypt(settings.ApplicationSecret);
            }

            return settings;
        }

        public void Validate(IGigyaModuleSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ApiKey))
            {
                throw new ArgumentException("API key is required");
            }

            if (string.IsNullOrEmpty(settings.ApplicationKey))
            {
                throw new ArgumentException("API key is required");
            }

            if (string.IsNullOrEmpty(settings.ApplicationSecret))
            {
                throw new ArgumentException("Application secret is required");
            }

            if (string.IsNullOrEmpty(settings.DataCenter))
            {
                throw new ArgumentException("DataCenter is required");
            }

            if (string.IsNullOrEmpty(settings.Language))
            {
                throw new ArgumentException("Language is required");
            }

            if (settings.Language == Constants.Languages.Auto && string.IsNullOrEmpty(settings.LanguageFallback))
            {
                throw new ArgumentException("Language fallback is required");
            }

            if (!string.IsNullOrEmpty(settings.GlobalParameters))
            {
                try
                {
                    JsonConvert.DeserializeObject<dynamic>(settings.GlobalParameters);
                }
                catch
                {
                    throw new ArgumentException("Couldn't deserialize global parameters. Check it's valid JSON.");
                }
            }
        }
    }
}
