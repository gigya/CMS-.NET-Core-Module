﻿namespace Sitecore.Gigya.Module.Repositories
{
    using System;
    using System.Linq;
    using SC = Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Reflection;
    using System.Web;
    using Sitecore.Gigya.Extensions.Abstractions.Repositories;

    public class RenderingPropertiesRepository : IRenderingPropertiesRepository
    {
        public T Get<T>(SC.Mvc.Presentation.Rendering rendering)
        {
            var currentContext = rendering;
            var parameters = currentContext?.Properties["Parameters"];
            return Get<T>(parameters);
        }

        public T Get<T>(string parameters)
        {
            var obj = ReflectionUtil.CreateObject(typeof(T));
            if (parameters == null)
                return (T)obj;

            parameters = this.FilterEmptyParametrs(parameters);
            var nameValues = StringUtil.GetNameValues(parameters, '=', '&');

            try
            {
                foreach (string key in nameValues.Keys)
                {
                    var value = HttpUtility.UrlDecode(nameValues[key]);
                    ReflectionUtil.SetProperty(obj, key, value);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e, this);
            }

            return (T)obj;
        }

        protected virtual string FilterEmptyParametrs(string parameters)
        {
            var parametersList = parameters.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            var parameterString = string.Join("&", parametersList.Where(x => x.Contains("=")));
            return parameterString;
        }
    }
}