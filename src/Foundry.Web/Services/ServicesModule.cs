using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Mef;
using Foundry.Services;
using Foundry.Messaging.Infrastructure;
using System.ComponentModel.Composition.Hosting;
using Foundry.SourceControl;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using Foundry.SourceControl.GitIntegration;
using Foundry.Services.Security;
using Foundry.Services.SourceControl;
using Foundry.Security;
using Foundry.Domain;

namespace Foundry.Services
{
    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SourceControlManager>().As<ISourceControlManager>().PropertiesAutowired();

            builder.RegisterType<MembershipService>().As<IMembershipService>().HttpRequestScoped().PropertiesAutowired();
            builder.RegisterType<AuthorizationService>().As<IAuthorizationService>().HttpRequestScoped();
        }
    }
}