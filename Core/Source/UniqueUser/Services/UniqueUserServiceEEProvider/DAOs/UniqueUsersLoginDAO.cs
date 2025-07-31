using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.EEProvider.Helper;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Helper;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.UniqueUser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes;
using CCOM = Sistran.Core.Application.CommonService.Models;
using System.Linq;
using System.Collections;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao UniqueUsersLogin
    /// </summary>
    public class UniqueUsersLoginDAO
    {
        /// <summary>
        /// Save UniqueUsersLogin
        /// </summary>
        /// <param name="uniqueUsersLogin">uniqueUsersLogin</param>
        public void CreateUniqueUsersLogin(UniqueUserLogin uniqueUsersLogin)
        {
            PrimaryKey key = UniqueUser.Entities.UniqueUserLogin.CreatePrimaryKey(uniqueUsersLogin.UserId);
            UniqueUser.Entities.UniqueUserLogin entity = (UniqueUser.Entities.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entity == null)
            {
                entity = EntityAssembler.CreateUniqueUserLogin(uniqueUsersLogin);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            }
            else
            {
                entity.MustChangePassword = uniqueUsersLogin.MustChangePassword;
                entity.PasswordExpirationDate = uniqueUsersLogin.ExpirationDate;
                entity.PasswordExpirationDays = uniqueUsersLogin.ExpirationsDays;
                if (uniqueUsersLogin.Password != null)
                {
                    //entity.Salt = SecurityHelper.CreateSalt();
                    //entity.Password = SecurityHelper.GetHashSha256(uniqueUsersLogin.Password, entity.Salt);

                    entity.Salt = null;
                    entity.Password = CryptoEngineR1(uniqueUsersLogin.Password);

                }
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
            }
        }

        /// <summary>
        /// Actualización de password
        /// </summary>
        /// <param name="UniqueUserssLogin"></param>
        public void UpdatePassword(int userId, string password, int expirationDays)
        {
            PrimaryKey key = UniqueUser.Entities.UniqueUserLogin.CreatePrimaryKey(userId);
            UniqueUser.Entities.UniqueUserLogin entity = (UniqueUser.Entities.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            entity.PasswordExpirationDate = DateTime.Now.AddDays(expirationDays);

            //entity.Salt = SecurityHelper.CreateSalt();
            //entity.Password = SecurityHelper.GetHashSha256(password, entity.Salt);

            entity.Salt = null;
            entity.Password = CryptoEngineR1(password);

            entity.MustChangePassword = false;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static bool UpdateFailedPassword(int UserId, out int attempts)
        {
            bool response = false;
            List<string> parametersDescription = new List<string>();

            parametersDescription.Add("NUMERO MAXIMO INTENTOS");

            List<CCOM.Parameter> parameters = DelegateService.commonServiceCore.GetParametersByDescriptions(parametersDescription);

            PrimaryKey key = UniqueUser.Entities.UniqueUserLogin.CreatePrimaryKey(UserId);

            UniqueUser.Entities.UniqueUserLogin entity = (UniqueUser.Entities.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            var val = parameters.Find(x => x.Description == "NUMERO MAXIMO INTENTOS").NumberParameter.GetValueOrDefault();
            if (entity.LoginAttempts == 0)
            {
                entity.LoginAttempts = 1;
            }
            else
            {
                entity.LoginAttempts++;
            }
            attempts = (int)entity.LoginAttempts;
            if (entity.LoginAttempts >= val)
            {
                response = true;
                entity.LockPassword = true;
                entity.LockPasswordDate = DateTime.Now;
            }

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="attempts"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public static bool UpdateFailedPasswordR2(int UserId, out int attempts, string SessionID)
        {
            bool response = false;
            bool insertObject = false;
            List<string> parametersDescription = new List<string>();

            parametersDescription.Add("NUMERO MAXIMO INTENTOS");

            List<CCOM.Parameter> parameters = DelegateService.commonServiceCore.GetParametersByDescriptions(parametersDescription);

            //KeyR1
            PrimaryKey keyR1 = UniqueUser.Entities.LogPasswordFailed.CreatePrimaryKey(UserId, SessionID);
            //KeyR2
            PrimaryKey keyR2 = UniqueUser.Entities.UniqueUserLogin.CreatePrimaryKey(UserId);

            //Reintentos R1
            UniqueUser.Entities.LogPasswordFailed entityR1 = (UniqueUser.Entities.LogPasswordFailed)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyR1);

            if (entityR1 == null)
            {
                insertObject = true;

                entityR1 = new UUEN.LogPasswordFailed(UserId, SessionID)
                {
                    UserId = UserId,
                    SessionId = SessionID,
                    Attempts = 0,
                    LockDate = null
                };
            }

            //Reintentos R2
            UniqueUser.Entities.UniqueUserLogin entityR2 = (UniqueUser.Entities.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyR2);

            var val = parameters.Find(x => x.Description == "NUMERO MAXIMO INTENTOS").NumberParameter.GetValueOrDefault();

            if (entityR1.Attempts == 0)
            {
                //EntityR1
                entityR1.Attempts = 1;
                //EntityR2
                entityR2.LoginAttempts = 1;
            }
            else
            {
                //EntityR1
                entityR1.Attempts++;
                //EntityR2
                entityR2.LoginAttempts = entityR1.Attempts;
            }

            attempts = (int)entityR1.Attempts;

            if (entityR1.Attempts >= val)
            {
                response = true;
                //EntityR1
                entityR1.LockDate = DateTime.Now;
                //EntityR2
                entityR2.LockPassword = true;
                entityR2.LockPasswordDate = DateTime.Now;
            }

            if (insertObject)
            {
                //EntityR1
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityR1);
            }
            else
            {
                //EntityR1
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityR1);
            }

            //EntityR2
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityR2);

            return response;
        }

        /// <summary>
        /// controla los intentos de autenticación correctos
        /// </summary>
        /// <param name="UserId">UserId</param>
        public static void UnlockPassword(int UserId)
        {
            List<string> parametersDescription = new List<string>();

            PrimaryKey key = UniqueUser.Entities.UniqueUserLogin.CreatePrimaryKey(UserId);

            UniqueUser.Entities.UniqueUserLogin entity = (UniqueUser.Entities.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            entity.LoginAttempts = 0;
            entity.LockPassword = false;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
        }

        /// <summary>
        /// controla los intentos de autenticación correctos
        /// </summary>
        /// <param name="UserId">UserId</param>
        public static void UnlockPasswordR2(int UserId, string SessionID)
        {
            PrimaryKey keyR1 = UniqueUser.Entities.LogPasswordFailed.CreatePrimaryKey(UserId, SessionID);

            UniqueUser.Entities.LogPasswordFailed entityR1 = (UniqueUser.Entities.LogPasswordFailed)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyR1);

            if (entityR1 != null)
            {
                entityR1.Attempts = 0;
                entityR1.LockDate = null;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityR1);
            }                   
        }

        /// <summary>
        /// Realiza las validaciones paramétricas para el cambio de contraseña 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="errors"></param>
        /// <returns>Verdadero sis cambió la contraseña</returns>
        public bool ChangePassword(bool changePassword, string login, string oldPassword, string newPassword, out Dictionary<int, int> errors)
        {
            int val = 0;
            string regExp = "";
            Regex reg;
            errors = new Dictionary<int, int>();

            #region Password acttual incorrecto
            UserDAO userDAO = new UserDAO();
            Models.User user = userDAO.GetUserByLogin(login);
            if (user == null)
            {
                throw new AuthenticationException("ENTERR_USERLOGIN_MISSING");
            }
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter = new ObjectCriteriaBuilder();
            filter.OpenParenthesis();
            filter.Property("UserId");
            filter.Equal();
            filter.Constant(user.UserId);
            filter.CloseParenthesis();
            UUEN.UniqueUserLogin UserLogin = (UUEN.UniqueUserLogin)DataFacadeManager.Instance.GetDataFacade().List(typeof(UUEN.UniqueUserLogin), filter.GetPredicate()).FirstOrDefault();

            if (changePassword)
            {
                if (UserLogin == null)
                {
                    throw new AuthenticationException("ENTERR_USERLOGIN_MISSING");
                }
                if (user.AuthenticationTypeCode == (int)AuthenticationCodeType.Standard)
                {
                    //Debe coincidir el password...
                    CryptoEngine ce = new CryptoEngine();
                    ce.EncryptionKey = oldPassword;
                    ce.CryptedText = UserLogin.Password;
                    ce.Decrypt();

                    if (MustBeReplaced.Cryptography.EncryptText(oldPassword) != ce.InClearText)
                    {
                        if (SecurityHelper.GetHashSha256(oldPassword, UserLogin.Salt) != UserLogin.Password)
                        {
                            errors.Add((int)ChangePasswordErrors.OldPassswordDoesntMatch, 0);
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Obtención de parámetros
            List<int> parametersId = new List<int>();

            parametersId.Add((int)ParametersTypes.KeyCoincidencePercentage);
            parametersId.Add((int)ParametersTypes.MaxRecordsHistory);
            parametersId.Add((int)ParametersTypes.MinKeyLeng);
            parametersId.Add((int)ParametersTypes.MinNumbersAmount);
            parametersId.Add((int)ParametersTypes.MinLowerAmount);
            parametersId.Add((int)ParametersTypes.MinUpperAmount);
            parametersId.Add((int)ParametersTypes.MinSpecialsAmount);
            parametersId.Add((int)ParametersTypes.CharactersToValidateInSecuence);
            parametersId.Add((int)ParametersTypes.NotAllowedSecuences);
            parametersId.Add((int)ParametersTypes.DAYS_EXPIRATION_PASSWORD);

            List<CCOM.Parameter> parameters = DelegateService.commonServiceCore.GetParametersByIds(parametersId);
            #endregion

            #region password contiene login
            if (newPassword.ToUpper().Contains(login.ToUpper()))
            {
                errors.Add((int)ChangePasswordErrors.ContainsUserName, 0);
            }
            #endregion

            #region longitud mínima
            val = parameters.Where(x => x.Id == (int)ParametersTypes.MinKeyLeng).First().NumberParameter.GetValueOrDefault();

            if (val > 0)
            {
                if (newPassword.Length < val)
                {
                    errors.Add((int)ChangePasswordErrors.minLenght, val);
                }
            }
            #endregion

            #region cantidad mínima de números
            val = parameters.Where(x => x.Id == (int)ParametersTypes.MinNumbersAmount).First().NumberParameter.GetValueOrDefault();
            regExp = "(?=";
            for (int i = 0; i < val; i++)
            {
                regExp += ".*[0-9]";
            }
            regExp += ")";

            reg = new Regex(regExp);

            if (!reg.IsMatch(newPassword))
            {
                errors.Add((int)ChangePasswordErrors.MustHaveNumber, val);
            }
            #endregion

            #region cantidad mínima de minúsculas
            val = parameters.Where(x => x.Id == (int)ParametersTypes.MinLowerAmount).First().NumberParameter.GetValueOrDefault();

            if (val > 0)
            {
                regExp = "(?=";
                for (int i = 0; i < val; i++)
                {
                    regExp += ".*[a-z]";
                }
                regExp += ")";

                reg = new Regex(regExp);

                if (!reg.IsMatch(newPassword))
                {
                    errors.Add((int)ChangePasswordErrors.MustHaveLower, val);
                }
            }
            #endregion

            #region cantidad mínima de mayúsculas
            val = parameters.Where(x => x.Id == (int)ParametersTypes.MinUpperAmount).First().NumberParameter.GetValueOrDefault();

            if (val > 0)
            {
                regExp = "(?=";
                for (int i = 0; i < val; i++)
                {
                    regExp += ".*[A-Z]";
                }
                regExp += ")";

                reg = new Regex(regExp);

                if (!reg.IsMatch(newPassword))
                {
                    errors.Add((int)ChangePasswordErrors.MustHaveUpper, val);
                }
            }
            #endregion

            #region cantidad mínima de especiales
            val = parameters.Where(x => x.Id == (int)ParametersTypes.MinSpecialsAmount).First().NumberParameter.GetValueOrDefault();

            if (val > 0)
            {
                regExp = "(?=";
                for (int i = 0; i < val; i++)
                {
                    regExp += @".*[!@#$&+\-*\\?/<>='\.\[\]\|{}]";
                }
                regExp += ")";

                reg = new Regex(regExp);

                if (!reg.IsMatch(newPassword))
                {
                    errors.Add((int)ChangePasswordErrors.MustHaveSpecial, val);
                }
            }
            #endregion

            #region Secuencias
            val = parameters.Where(x => x.Id == (int)ParametersTypes.CharactersToValidateInSecuence).First().NumberParameter.GetValueOrDefault();

            if (val > 0)
            {
                string secuences = parameters.Where(x => x.Id == (int)ParametersTypes.NotAllowedSecuences).First().TextParameter;                

                if (VerifySecuences(val, secuences, newPassword))
                {
                    errors.Add((int)ChangePasswordErrors.CantHaveSecuence, 0);
                }
            }
            #endregion

            #region procentaje de coincidencia
            filter = new ObjectCriteriaBuilder();
            filter.Property("UserId");
            filter.Equal();
            filter.Constant(user.UserId);

            val = parameters.Where(x => x.Id == (int)ParametersTypes.KeyCoincidencePercentage).First().NumberParameter.GetValueOrDefault();
            int numHistorial = parameters.Where(x => x.Id == (int)ParametersTypes.MaxRecordsHistory).First().NumberParameter.GetValueOrDefault();

            if (val > 0)
            {
                var historicalPasswords = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(UUEN.UniqueUserLoginHistory), filter.GetPredicate());

                List<UUEN.UniqueUserLoginHistory> entityUniqueUserLoginHistory = historicalPasswords.Cast<UUEN.UniqueUserLoginHistory>().OrderByDescending(x => x.ChangeDate).Take(numHistorial).ToList();

                CommonUtilitiesUser util = new CommonUtilitiesUser();

                foreach (UUEN.UniqueUserLoginHistory history in entityUniqueUserLoginHistory)
                {
                    //Comparacion R2
                    if (history.Ecryptedpassword != null)
                    {
                        if (!string.IsNullOrEmpty(history.Ecryptedpassword.Trim()))
                        {
                            int numToCompare = util.Decrypt(history.Ecryptedpassword).Length * val / 100;

                            if (VerifySecuences(numToCompare, util.Decrypt(history.Ecryptedpassword), newPassword))
                            {
                                errors.Add((int)ChangePasswordErrors.SimilarTohistorical, 0);
                                break;
                            }
                        }
                    }

                    //Comparacion R1
                    if (history.Password != null)
                    {
                        if (!string.IsNullOrEmpty(history.Password.Trim()))
                        {
                            int numToCompare = history.Password.Length * val / 100;

                            if (VerifySecuences(numToCompare, history.Password, CryptoEngineR1(newPassword)))
                            {
                                errors.Add((int)ChangePasswordErrors.SimilarTohistorical, 0);
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Primer caracter no numérico
            if (char.IsNumber(newPassword, 0))
            {
                errors.Add((int)ChangePasswordErrors.FirstMustNonNumber, 0);
            }
            #endregion

            #region Último caracter no numérico
            if (char.IsNumber(newPassword, newPassword.Length - 1))
            {
                errors.Add((int)ChangePasswordErrors.LastMustNotNumber, 0);
            }
            #endregion

            if (errors.Count == 0)
            {
                #region guardar password
                val = parameters.Where(x => x.Id == (int)ParametersTypes.DAYS_EXPIRATION_PASSWORD).First().NumberParameter.GetValueOrDefault();
                UpdatePassword(UserLogin.UserId, newPassword, val);
                #endregion

                if (changePassword)
                {
                    #region guardar historial
                    //CommonUtilitiesUser util = new CommonUtilitiesUser();
                    //var encText = util.Encrypt(oldPassword);

                    UUEN.UniqueUserLoginHistory newHistory = new UUEN.UniqueUserLoginHistory(UserLogin.UserId, DateTime.Now)
                    {
                        //Salt = UserLogin.Salt,
                        //Password = UserLogin.Password,
                        //Ecryptedpassword = encText

                        Salt = " ",
                        Password = CryptoEngineR1(oldPassword),
                        Ecryptedpassword = " "
                    };

                    DataFacadeManager.Instance.GetDataFacade().InsertObject(newHistory);
                    #endregion
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica si cotiene las secuencias
        /// </summary>
        /// <param name="numberOfCharacthers"></param>
        /// <param name="secuences"></param>
        /// <param name="password"></param>
        /// <returns>Verdadero si contiene alguna secuencia, falso si no</returns>
        private bool VerifySecuences(int numberOfCharacthers, string secuences, string password)
        {
            List<string> secuenceList = secuences.Split('|').ToList();
            foreach (string sec in secuenceList)
            {
                for (int i = 0; i <= (sec.Length - numberOfCharacthers); i++)
                {
                    string compareSec = sec.Substring(i, numberOfCharacthers);
                    if (password.ToUpper().Contains(compareSec.ToUpper()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Method of encryption of password
        /// </summary>
        /// <param name="passwordd"></param>
        /// <returns>encryption password</returns>
        private string CryptoEngineR1(string password)
        {
            CryptoEngine ce = new CryptoEngine();
            ce.EncryptionKey = password;
            ce.CryptedText = password;
            ce.Decrypt();
            return ce.CryptedText;
        }
    }
}
