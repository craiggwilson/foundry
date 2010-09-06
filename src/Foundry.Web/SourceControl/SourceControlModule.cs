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

namespace Foundry.SourceControl
{
    public class SourceControlModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var catalog = new TypeCatalog(typeof(GitSourceControlProvider));
            builder.RegisterComposablePartCatalog(catalog);
        }
    }
}