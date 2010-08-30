﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Mef;
using Foundry.Reporting.Infrastructure;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reporting.DomainEventHandlers;
using Foundry.Services;
using Foundry.Messaging.Infrastructure;
using Foundry.Reporting;
using System.ComponentModel.Composition.Hosting;
using Foundry.SourceControl;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

namespace Foundry.Services
{
    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MembershipService(c.Resolve<IBus>(), c.Resolve<IReportingRepository<UserReport>>())).As<IMembershipService>().HttpRequestScoped().PropertiesAutowired();
            builder.Register(c => new SourceControlManager(c.Resolve<IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>>>())).As<ISourceControlManager>().SingleInstance();

            var currentPath = Assembly.GetExecutingAssembly().CodeBase;
            var catalog = new DirectoryCatalog("bin\\Addins");
            builder.RegisterComposablePartCatalog(catalog);
        }
    }
}