using Sistran.Core.Framework.UIF2.Services.Security;
using Sistran.Core.Framework.UIF2.Services.Authentication;
using SEMO = Sistran.Core.Application.SecurityServices.Models;
using System;
using AutoMapper;
using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Application.UIF2.Providers
{
    public class SISE3GAuthenticationProvider : IAuthenticationProvider
    {
        public AuthenticationResult Autenthicate(string loginName, string password, string domain)
        {
            SEMO.AuthenticationResult authenticationResponse = DelegateService.authenticationProviders.Autenthicate(loginName, password, domain);
            var config = MapperCache.GetMapper<SEMO.AuthenticationResult, AuthenticationResult>(cfg =>
            {
                cfg.CreateMap<SEMO.AuthenticationResult, AuthenticationResult>();
            });
            AuthenticationResult authenticationResult = config.Map<SEMO.AuthenticationResult, AuthenticationResult>(authenticationResponse);

            switch (authenticationResult.Result)
            {
                case AuthenticationEnum.isInvalidPassword:
                    throw new Exception("Usuario o contraseña incorrecta");
                case AuthenticationEnum.isInvalidPasswordWithData:
                    throw new Exception(string.Format("Ha fallado en {0} ocasiones, después de {1} intentos su cuenta será bloqueada", authenticationResponse.data[0], authenticationResponse.data[1]));
                case AuthenticationEnum.isUserBlocked:
                    throw new Exception("Usuario bloqueado, Por favor contacte al administrador");
                case AuthenticationEnum.isUserBlockedWithTime:
                    throw new Exception(string.Format("Su usuario ha sido bloqueado en el sistema. Debe esperar {0} minutos antes de intentar ingresar nuevamente", authenticationResponse.data[0]));
                case AuthenticationEnum.IsUserExpired:
                    throw new Exception("Usuario / contraseña se encuentra vencida. Debe realizar el cambio.");
                default:
                    return authenticationResult;
            }
        }

        public int GetUserId(string loginName)
        {
            return DelegateService.authenticationProviders.GetUserId(loginName);
        }
    }
}