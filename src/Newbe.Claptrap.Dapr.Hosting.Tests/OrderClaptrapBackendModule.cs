using OrderClaptrap.EntityFrameworkCore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Newbe.Claptrap.Dapr.Hosting.Tests
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(OrderClaptrapEntityFrameworkCoreModule)
        //typeof(AbpSwashbuckleModule),
        //typeof(AbpAspNetCoreMvcModule)
        )]
    public class OrderClaptrapBackendModule : AbpModule
    {
    }
}