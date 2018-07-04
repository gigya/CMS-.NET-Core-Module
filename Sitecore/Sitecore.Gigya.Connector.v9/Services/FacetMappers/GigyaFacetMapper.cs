﻿using Gigya.Module.Core.Connector.Common;
using Gigya.Module.Core.Connector.Logging;
using Sitecore.Analytics.Model.Framework;
using Sitecore.Gigya.Connector.Providers;
using Sitecore.Gigya.Extensions.Abstractions.Analytics.Models;
using Sitecore.Gigya.Extensions.Abstractions.Services;
using Sitecore.Gigya.XConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using C = Sitecore.Gigya.Extensions.Abstractions.Analytics.Constants;

namespace Sitecore.Gigya.Connector.Services.FacetMappers
{
    public class GigyaFacetMapper : GigyaFacetMapperBase<GigyaFacet>
    {
        public GigyaFacetMapper(IContactProfileProvider contactProfileProvider, Logger logger) : base(contactProfileProvider, logger)
        {
        }

        protected override string FacetKey => C.FacetKeys.Gigya;

        protected override void SetFacet(GigyaFacet facet)
        {
            _contactProfileProvider.SetFacet(facet, FacetKey);
        }

        protected override GigyaFacet GetOrCreateFacet()
        {
            return _contactProfileProvider.Gigya ?? new GigyaFacet { Entries = new Dictionary<string, GigyaElement>() };
        }
    }
}