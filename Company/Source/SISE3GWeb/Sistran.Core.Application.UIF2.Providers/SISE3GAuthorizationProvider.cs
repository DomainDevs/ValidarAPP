using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Framework.UIF2.Services.Security;
using SEMO = Sistran.Core.Application.SecurityServices.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Application.UIF2.Providers
{
    public class SISE3GAuthorizationProvider : IAuthorizationProvider
    {
        public IList<ControlSecurity> GetControlsSecurity(string viewID, string userName)
        {
            IList<SEMO.ControlSecurity> controlsSecurity = DelegateService.authorizationProviders.GetControlsSecurity(viewID, userName);
            var config = MapperCache.GetMapper<SEMO.ControlSecurity, ControlSecurity>(cfg =>
            {
                cfg.CreateMap<SEMO.ControlSecurity, ControlSecurity>();
            });
            return config.Map<IList<SEMO.ControlSecurity>, IList<ControlSecurity>>(controlsSecurity);
        }

        public IList<Module> GetModules(string userName)
        {
            IList<SEMO.Module> modules = DelegateService.authorizationProviders.GetModules(userName);

            
            var config = MapperCache.GetMapper<SEMO.Module, Module>(cfg =>
            {
                cfg.CreateMap<SEMO.Module, Module>();
            });
            return config.Map<IList<SEMO.Module>, IList<Module>>(modules);
        }
    }
}