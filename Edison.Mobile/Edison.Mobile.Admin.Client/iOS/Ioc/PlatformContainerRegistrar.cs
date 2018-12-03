﻿using System;
using Autofac;
using Edison.Mobile.Admin.Client.iOS.Shared;
using Edison.Mobile.Common.Auth;
using Edison.Mobile.Common.Ioc;
using Microsoft.Identity.Client;

namespace Edison.Mobile.Admin.Client.iOS.Ioc
{
    public class PlatformContainerRegistrar : IContainerRegistrar
    {
        public void Register(ContainerBuilder builder)
        {
            var publicClientApplication = new PublicClientApplication(Constants.ClientId, AuthConfig.Authority)
            {
                RedirectUri = AuthConfig.UserRedirectUri,
            };

            builder.RegisterInstance(publicClientApplication)
                   .As<IPublicClientApplication>();
        }
    }
}
