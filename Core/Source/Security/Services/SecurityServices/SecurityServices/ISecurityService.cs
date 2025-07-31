using Sistran.Core.Application.SecurityServices.Models;
using System.ServiceModel;

namespace Sistran.Core.Application.SecurityServices
{
    [ServiceContract]
    public interface ISecurityService
    {
        /// <summary>
        /// Autenticación de Usuario.
        /// </summary>
        /// <param name="loginName">Nombre de usuario.</param>
        /// <param name="password">Contraseña.</param>
        /// <param name="domain">Dominio.</param>
        /// <returns></returns>
        [OperationContract]
        AuthenticationResult Autenthicate(string loginName, string password, string domain);

        /// <summary>
        /// Devuelde el identificador de un usuario.
        /// </summary>
        /// <param name="name">Nombre de usuario.</param>
        /// <returns></returns>
        [OperationContract]
        int GetUserId(string name);


    }
}
