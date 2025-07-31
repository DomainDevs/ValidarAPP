using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.SecurityServices.EEProvider.Helper;
using Sistran.Core.Application.SecurityServices.Enums;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.UniqueUser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CCOM = Sistran.Core.Application.CommonService.Models;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using UUMO = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Application.SecurityServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao de Usuario
    /// </summary>
    public class UserDAO
    {

        /// <summary>
        /// Obtener Usuario por Nombre y Clave
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// PRPERR_PASSWORD_INVALID
        /// or
        /// PRPERR_PASSWORD_MUST_BE_CHANGED
        /// or
        /// PRPERR_PASSWORD_EXPIRED
        /// </exception>
        /// <exception cref="AuthenticationException">
        /// ENTERR_USERLOGIN_MISSING
        /// or
        /// ENTERR_USERLOGIN_MISSING
        /// </exception>
        public AuthenticationResult GetUserByLoginByPassword(string login, string password)
        {
            AuthenticationResult authenticationResult = new AuthenticationResult();

            UUMO.User user = DelegateService.uniqueUserServiceCore.GetUserByLogin(login);

            if (user == null)
            {
                throw new AuthenticationException("ENTERR_USERLOGIN_MISSING");
            }
            else
            {
                try
                {
                    //valido que no haya expirado
                    if (user.ExpirationDate != null && !user.IsExpirationDateNull)
                    {
                        var date1 = DateTime.ParseExact(user.ExpirationDate.GetValueOrDefault().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var date2 = DateTime.ParseExact(BusinessServices.GetDate().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (date1 <= date2)
                        {
                            // throw new UserExpiredException();
                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.IsUserExpired };
                        }
                    }
                    ValidateValidUser(user);
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.OpenParenthesis();
                    filter.Property("UserId");
                    filter.Equal();
                    filter.Constant(user.UserId);
                    filter.CloseParenthesis();
                    UUEN.UniqueUserLogin UserLogin = (UUEN.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().List(typeof(UUEN.UniqueUserLogin), filter.GetPredicate()).FirstOrDefault();
                    if (UserLogin == null)
                    {
                        throw new AuthenticationException("ENTERR_USERLOGIN_MISSING");
                    }
                    if (user.AuthenticationTypeCode == (int)AuthenticationCodeType.Standard)
                    {
                        List<object> data = new List<object>();
                        List<int> parametersId = new List<int>();

                        parametersId.Add((int)ParametersTypes.DaysBeforePasswordExpires);
                        parametersId.Add((int)ParametersTypes.TimeUserBlocked);
                        parametersId.Add((int)ParametersTypes.WarningAttempts);
                        parametersId.Add((int)ParametersTypes.MaxAttempts);


                        List<CCOM.Parameter> parameters = DelegateService.commonServiceCore.GetParametersByIds(parametersId);

                        //Debe coincidir el password...
                        CryptoEngine ce = new CryptoEngine();
                        ce.EncryptionKey = password;
                        ce.CryptedText = UserLogin.Password;
                        ce.Decrypt();

                        if (UserLogin.LockPassword != null && (bool)UserLogin.LockPassword)
                        {
                            int lockTime = parameters.Where(x => x.Id == (int)ParametersTypes.TimeUserBlocked).First().NumberParameter.GetValueOrDefault();
                            if (lockTime > 0)
                            {
                                if (UserLogin.LockPasswordDate != null && DateTime.Now < UserLogin.LockPasswordDate.GetValueOrDefault().AddMinutes(lockTime))
                                {
                                    data.Add(lockTime);
                                    return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlockedWithTime, data = data };
                                }
                                DelegateService.uniqueUserServiceCore.UnlockPassword(UserLogin.UserId);
                            }
                            else
                            {
                                return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlocked };
                            }
                        }

                        if (MustBeReplaced.Cryptography.EncryptText(password) != ce.InClearText)
                        {
                            if (DelegateService.uniqueUserServiceCore.GetHashSha256(password, UserLogin.Salt) != UserLogin.Password)
                            {
                                int tryAdvert = parameters.Where(x => x.Id == (int)ParametersTypes.WarningAttempts).First().NumberParameter.GetValueOrDefault();
                                int trylock = parameters.Where(x => x.Id == (int)ParametersTypes.MaxAttempts).First().NumberParameter.GetValueOrDefault();
                                if (trylock > 0)
                                {
                                    int attempts = 0;
                                    var locked = DelegateService.uniqueUserServiceCore.UpdateFailedPassword(UserLogin.UserId, out attempts);
                                    if (locked)
                                    {
                                        int lockTime = parameters.Where(x => x.Id == (int)ParametersTypes.TimeUserBlocked).First().NumberParameter.GetValueOrDefault();
                                        if (lockTime > 0)
                                        {
                                            data.Add(lockTime);
                                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlockedWithTime, data = data };
                                        }
                                        else
                                        {
                                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlocked };
                                        }
                                    }
                                    if (tryAdvert == 0 || attempts < tryAdvert)
                                    {
                                        return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isInvalidPassword };
                                    }
                                    data.Add(attempts);
                                    data.Add(trylock);
                                    return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isInvalidPasswordWithData, data = data };
                                }
                                return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isInvalidPassword };
                            }
                        }

                        DelegateService.uniqueUserServiceCore.UnlockPassword(UserLogin.UserId);


                        //No debe exigir el cambio de password ...
                        if (UserLogin.MustChangePassword==true)
                        {
                            return new AuthenticationResult { isAuthenticated = true, Result = AuthenticationEnum.MustChangePasssword, UserId = UserLogin.UserId };
                        }

                        //El password no debe haber expirado ...
                        if ((UserLogin.PasswordExpirationDate != null) &&
                            UserLogin.PasswordExpirationDate <= BusinessServices.GetDate())
                        {
                            return new AuthenticationResult { isAuthenticated = true, Result = AuthenticationEnum.isPasswordExpired, UserId = UserLogin.UserId };
                        }
                        //Está cerca a expirar
                        int daysToExpire = parameters.Where(x => x.Id == (int)ParametersTypes.DaysBeforePasswordExpires).First().NumberParameter.GetValueOrDefault();
                        if ((UserLogin.PasswordExpirationDate != null) &&
                            UserLogin.PasswordExpirationDate.Value.Subtract(BusinessServices.GetDate()).Days <= daysToExpire)
                        {

                            data.Add(UserLogin.PasswordExpirationDate.Value.Subtract(BusinessServices.GetDate()).Days);
                            return new AuthenticationResult { isAuthenticated = true, Result = AuthenticationEnum.isPasswordNearToExpire, data = data, UserId = UserLogin.UserId };
                        }
                        authenticationResult.UserId = UserLogin.UserId;
                        authenticationResult.isAuthenticated = true;
                        authenticationResult.Result = AuthenticationEnum.isAuthenticated;
                    }
                    else
                    {
                        authenticationResult.isAuthenticated = false;
                        authenticationResult.Result = AuthenticationEnum.isInvalidPassword;
                    }
                }
                catch (UserExpiredException ueEx)
                {
                    authenticationResult.isAuthenticated = false;
                    authenticationResult.Result = AuthenticationEnum.IsUserExpired;
                }
                catch (System.Exception ex)
                {
                    authenticationResult.isAuthenticated = false;
                    authenticationResult.Result = AuthenticationEnum.isInvalidPassword;
                    throw;

                }
            }
            return authenticationResult;
        }

        /// <summary>
        /// Obtener Usuario por Nombre y Clave
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <param name="SessionID">The session</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// PRPERR_PASSWORD_INVALID
        /// or
        /// PRPERR_PASSWORD_MUST_BE_CHANGED
        /// or
        /// PRPERR_PASSWORD_EXPIRED
        /// </exception>
        /// <exception cref="AuthenticationException">
        /// ENTERR_USERLOGIN_MISSING
        /// or
        /// ENTERR_USERLOGIN_MISSING
        /// </exception>
        public AuthenticationResult GetUserByLoginByPasswordR2(string login, string password, string SessionID)
        {
            AuthenticationResult authenticationResult = new AuthenticationResult();

            UUMO.User user = DelegateService.uniqueUserServiceCore.GetUserByLogin(login);

            if (user == null)
            {
                throw new AuthenticationException("ENTERR_USERLOGIN_MISSING");
            }
            else
            {
                try
                {
                    //valido que no haya expirado
                    if (user.ExpirationDate != null && !user.IsExpirationDateNull)
                    {
                        var date1 = DateTime.ParseExact(user.ExpirationDate.GetValueOrDefault().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var date2 = DateTime.ParseExact(BusinessServices.GetDate().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (date1 <= date2)
                        {
                            // throw new UserExpiredException();
                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.IsUserExpired };
                        }
                    }
                    ValidateValidUser(user);
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.OpenParenthesis();
                    filter.Property("UserId");
                    filter.Equal();
                    filter.Constant(user.UserId);
                    filter.CloseParenthesis();
                    UUEN.UniqueUserLogin UserLogin = (UUEN.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().List(typeof(UUEN.UniqueUserLogin), filter.GetPredicate()).FirstOrDefault();
                    if (UserLogin == null)
                    {
                        throw new AuthenticationException("ENTERR_USERLOGIN_MISSING");
                    }
                    if (user.AuthenticationTypeCode == (int)AuthenticationCodeType.Standard)
                    {
                        List<object> data = new List<object>();
                        List<int> parametersId = new List<int>();

                        parametersId.Add((int)ParametersTypes.DaysBeforePasswordExpires);
                        parametersId.Add((int)ParametersTypes.TimeUserBlocked);
                        parametersId.Add((int)ParametersTypes.WarningAttempts);
                        parametersId.Add((int)ParametersTypes.MaxAttempts);


                        List<CCOM.Parameter> parameters = DelegateService.commonServiceCore.GetParametersByIds(parametersId);

                        //Debe coincidir el password...
                        CryptoEngine ce = new CryptoEngine();
                        ce.EncryptionKey = password;
                        ce.CryptedText = UserLogin.Password;
                        ce.Decrypt();

                        if (UserLogin.LockPassword != null && (bool)UserLogin.LockPassword)
                        {
                            int lockTime = parameters.Where(x => x.Id == (int)ParametersTypes.TimeUserBlocked).First().NumberParameter.GetValueOrDefault();
                            if (lockTime > 0)
                            {
                                if (UserLogin.LockPasswordDate != null && DateTime.Now < UserLogin.LockPasswordDate.GetValueOrDefault().AddMinutes(lockTime))
                                {
                                    data.Add(lockTime);
                                    return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlockedWithTime, data = data };
                                }
                                DelegateService.uniqueUserServiceCore.UnlockPassword(UserLogin.UserId);
                                DelegateService.uniqueUserServiceCore.UnlockPasswordR2(UserLogin.UserId, SessionID);
                            }
                            else
                            {
                                return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlocked };
                            }
                        }

                        if (MustBeReplaced.Cryptography.EncryptText(password) != ce.InClearText)
                        {
                            if (DelegateService.uniqueUserServiceCore.GetHashSha256(password, UserLogin.Salt) != UserLogin.Password)
                            {
                                int tryAdvert = parameters.Where(x => x.Id == (int)ParametersTypes.WarningAttempts).First().NumberParameter.GetValueOrDefault();
                                int trylock = parameters.Where(x => x.Id == (int)ParametersTypes.MaxAttempts).First().NumberParameter.GetValueOrDefault();
                                if (trylock > 0)
                                {
                                    int attempts = 0;
                                    var locked = DelegateService.uniqueUserServiceCore.UpdateFailedPasswordR2(UserLogin.UserId, out attempts, SessionID);
                                    if (locked)
                                    {
                                        int lockTime = parameters.Where(x => x.Id == (int)ParametersTypes.TimeUserBlocked).First().NumberParameter.GetValueOrDefault();
                                        if (lockTime > 0)
                                        {
                                            data.Add(lockTime);
                                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlockedWithTime, data = data };
                                        }
                                        else
                                        {
                                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isUserBlocked };
                                        }
                                    }
                                    if (tryAdvert == 0 || attempts < tryAdvert)
                                    {
                                        return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isInvalidPassword };
                                    }
                                    data.Add(attempts);
                                    data.Add(trylock);
                                    return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isInvalidPasswordWithData, data = data };
                                }
                                return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.isInvalidPassword };
                            }
                        }

                        DelegateService.uniqueUserServiceCore.UnlockPassword(UserLogin.UserId);
                        DelegateService.uniqueUserServiceCore.UnlockPasswordR2(UserLogin.UserId, SessionID);

                        // Valida si tiene un perfil el usuario                        
                        filter.Clear();
                        filter.PropertyEquals(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId, typeof(UniqueUser.Entities.ProfileUniqueUser).Name, user.UserId);
                        BusinessCollection UserProfile = DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.ProfileUniqueUser), filter.GetPredicate());
                        if (UserProfile.Count == 0)
                        {
                            return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.IsUserWithoutProfiles };
                        }

                        //Valida si el perfil que tiene el usuario esta habilitado
                        
                        List<UniqueUser.Entities.ProfileUniqueUser> entityProfileUniqueUsers = EntityAssembler.CreateProfileUniqueUser(UserProfile);
                        foreach (UniqueUser.Entities.ProfileUniqueUser ProfileUniqueUser in entityProfileUniqueUsers)
                        {
                            ObjectCriteriaBuilder filterProfile = new ObjectCriteriaBuilder();
                            filterProfile.PropertyEquals(UniqueUser.Entities.Profiles.Properties.ProfileId, typeof(UniqueUser.Entities.Profiles).Name, ProfileUniqueUser.ProfileId);
                            BusinessCollection profile = DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.Profiles), filterProfile.GetPredicate());
                            List<UniqueUser.Entities.Profiles> entityProfileUser = EntityAssembler.CreateProfile(profile);
                            if (entityProfileUser.FirstOrDefault(x => x.ProfileId == entityProfileUser[0].ProfileId).Enabled == false)
                            {
                                return new AuthenticationResult { isAuthenticated = false, Result = AuthenticationEnum.IsProfileDisabled };
                            }
                        }
                        
                        //No debe exigir el cambio de password ...
                        if (UserLogin.MustChangePassword == true)
                        {
                            return new AuthenticationResult { isAuthenticated = true, Result = AuthenticationEnum.MustChangePasssword, UserId = UserLogin.UserId };
                        }

                        //El password no debe haber expirado ...
                        if ((UserLogin.PasswordExpirationDate != null) &&
                            UserLogin.PasswordExpirationDate <= BusinessServices.GetDate())
                        {
                            return new AuthenticationResult { isAuthenticated = true, Result = AuthenticationEnum.isPasswordExpired, UserId = UserLogin.UserId };
                        }
                        //Está cerca a expirar
                        int daysToExpire = parameters.Where(x => x.Id == (int)ParametersTypes.DaysBeforePasswordExpires).First().NumberParameter.GetValueOrDefault();
                        if ((UserLogin.PasswordExpirationDate != null) &&
                            UserLogin.PasswordExpirationDate.Value.Subtract(BusinessServices.GetDate()).Days <= daysToExpire)
                        {

                            data.Add(UserLogin.PasswordExpirationDate.Value.Subtract(BusinessServices.GetDate()).Days);
                            return new AuthenticationResult { isAuthenticated = true, Result = AuthenticationEnum.isPasswordNearToExpire, data = data, UserId = UserLogin.UserId };
                        }
                        authenticationResult.UserId = UserLogin.UserId;
                        authenticationResult.isAuthenticated = true;
                        authenticationResult.Result = AuthenticationEnum.isAuthenticated;
                    }
                    else
                    {
                        authenticationResult.isAuthenticated = false;
                        authenticationResult.Result = AuthenticationEnum.isInvalidPassword;
                    }
                }
                catch (UserExpiredException)
                {
                    authenticationResult.isAuthenticated = false;
                    authenticationResult.Result = AuthenticationEnum.IsUserExpired;
                }
                catch (System.Exception)
                {
                    authenticationResult.isAuthenticated = false;
                    authenticationResult.Result = AuthenticationEnum.isInvalidPassword;
                    throw;

                }
            }
            return authenticationResult;
        }

        /// <summary>
        /// Implementación de reglas de validación sobre un objeto User.
        /// </summary>
        /// <param name="user">
        /// Objeto User a validar.
        /// </param>
        /// <exception 
        /// cref="Sistran.Core.Application.UniqueUser.Exceptions.UserDisabledException">
        /// El usuario no está habilitado.
        /// </exception>
        /// <exception 
        /// cref="Sistran.Core.Application.UniqueUser.Exceptions.UserDisabledException">
        /// La fecha de expiración del usuario coincide o es anterior a la
        /// fecha del día.
        /// </exception>
        /// <exception 
        /// cref="Sistran.Core.Application.UniqueUser.Exceptions.UserLockedException">
        /// El usuario está bloqueado.
        /// </exception>
        /// <exception 
        /// cref="Sistran.Core.Application.UniqueUser.Exceptions.BusinessException">
        /// El usuario no existe.
        /// </exception>
        private void ValidateValidUser(UUMO.User user)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-CO");
            //Valido que me hayan enviado el usuario
            if (user != null)
            {
                //valido si esta habilitado
                if (!user.IsDisabledDateNull)
                {
                    if (user.DisableDate <= BusinessServices.GetDate())
                    {
                        throw new BusinessException("ENTERR_USER_DOESNT_ENABLED");
                    }
                }
                //valido que no este bloqueado
                if (!user.IsLockDateNull)
                {
                    if (user.LockDate <= BusinessServices.GetDate())
                    {
                        throw new BusinessException("ENTERR_USER_IS_BLOCKED");
                    }
                }
                //valido que no este bloqueado
                if (user.LockPassword)
                {
                    throw new BusinessException("ENTERR_USER_IS_BLOCKED");
                }
            }
            else
            {
                throw new BusinessException("ENTERR_USER_DOESNT_EXIST");
            }
        }
    }
}
