using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Autofac;
using Autofac.Integration.Web;



namespace Foundry.Reporting
{
    public class ReportingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new UserReportManager()).As<IUserReportManager>().HttpRequestScoped();
        }
    }
}
