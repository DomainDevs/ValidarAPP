using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class UsersSessionHelper
    {
        /// <summary>
        /// Sesión de usuarios
        /// </summary>
        private static List<UniqueUserSession> usersSession;

        private static object BlockObject = new object();

        /// <summary>
        /// Obtiene o establece la sesion del usuario
        /// </summary>    
        public static List<UniqueUserSession> UsersSession
        {
            get
            {
                lock (BlockObject)
                {
                    if (usersSession == null)
                    {
                        usersSession = new List<UniqueUserSession>();
                    }

                    return usersSession;
                }
            }

            set
            {
                usersSession = value;
            }
        }
        public static void Add(UniqueUserSession user)
        {
            lock (BlockObject)
            {
                UsersSession.Add(user);
            }
        }
        public static void Remove(UniqueUserSession user)
        {
            lock (BlockObject)
            {
                UsersSession.Remove(user);
            }
        }

        public static UniqueUserSession GetBySessionId(string SessionID)
        {
            try
            {
                UniqueUserSession model = UsersSession?.FirstOrDefault(x => x != null && x.SessionId == SessionID);
                if (model == null)
                {
                    model = Services.DelegateService.uniqueUserService.GetUserInSessionBySessionId(SessionID);
                    if (model == null)
                        return null;
                    Add(model);
                }
                return model;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}