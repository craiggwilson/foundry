using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Mef;
using Foundry.Reports.Infrastructure;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reports.DomainEventHandlers;
using Foundry.Services;
using Foundry.Messaging.Infrastructure;
using Foundry.Reports;
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

namespace Foundry.Services
{
    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MembershipService(c.Resolve<IBus>(), c.Resolve<IReportingRepository<UserReport>>())).As<IMembershipService>().HttpRequestScoped().PropertiesAutowired();
            builder.Register(c => new SourceControlManager(c.Resolve<IBus>(), c.Resolve<IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>>>())).As<ISourceControlManager>().PropertiesAutowired();
            builder.Register(c => new AuthorizationService(c.Resolve<IReportingRepository<UserPermissionsReport>>())).As<IAuthorizationService>();
        }
    }
}