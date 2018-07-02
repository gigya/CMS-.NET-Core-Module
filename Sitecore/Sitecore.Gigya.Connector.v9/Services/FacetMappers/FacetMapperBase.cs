﻿using A = Sitecore.Analytics;
using Gigya.Module.Core.Connector.Logging;
using Sitecore.Gigya.Extensions.Abstractions.Analytics.Models;
using Sitecore.Gigya.Extensions.Abstractions.Services;
using Sitecore.Gigya.Connector.Providers;
using Sitecore.Pipelines;
using Sitecore.XConnect.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Gigya.Module.Models.Pipelines;

namespace Sitecore.Gigya.Connector.Services.FacetMappers
{
    public abstract class FacetMapperBase<T> where T: MappingBase
    {
        protected readonly Logger _logger;
        protected IContactProfileProvider _contactProfileProvider;

        public FacetMapperBase(IContactProfileProvider contactProfileProvider, Logger logger)
        {
            _contactProfileProvider = contactProfileProvider;
            _logger = logger;
        }

        public void Update(dynamic gigyaModel, T mapping)
        {
            if (mapping == null || !A.Tracker.IsActive)
            {
                return;
            }

            var args = new FacetMapperPipelineArgs<T>
            {
                GigyaModel = gigyaModel,
                Mapping = mapping
            };

            CorePipeline.Run("gigya.module.facetUpdating", args, false);

            if (!args.Aborted)
            {
                UpdateFacet(args.GigyaModel, args.Mapping);
            }

            CorePipeline.Run("gigya.module.facetUpdated", args, false);
        }

        protected abstract void UpdateFacet(dynamic gigyaModel, T mapping);
    }
}