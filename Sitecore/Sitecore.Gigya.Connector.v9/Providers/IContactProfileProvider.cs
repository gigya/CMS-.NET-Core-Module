﻿using System.Collections.Generic;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Analytics.Model.Framework;
using Sitecore.Gigya.XConnect.Models;
//using Sitecore.Analytics.Tracking;
//using Sitecore.Gigya.Connector.Analytics.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace Sitecore.Gigya.Connector.Providers
{
    public interface IContactProfileProvider
    {
        AddressList Addresses { get; }
        Contact Contact { get; }
        EmailAddressList Emails { get; }
        GigyaFacet Gigya { get; }
        GigyaPiiFacet GigyaPii { get; }
        PersonalInformation PersonalInfo { get; }
        PhoneNumberList PhoneNumbers { get; }
        ConsentInformation ConsentInformation { get; }

        void SetFacet<T>(T facet, string facetName) where T : Sitecore.XConnect.Facet;
        Contact Flush();
    }
}