// -----------------------------------------------------------------------
// <copyright file="IUnderwritingParamServiceWeb.cs" company="SISTRAN">
using System.ServiceModel;

namespace Sistran.Core.Application.ProductParamService
{

    /// <summary>
    /// Interfaz para UnderwritingParamService
    /// </summary>
    [ServiceContract]
    public interface IProductParamServiceCore
    {

        /// <summary>
        /// Crear Producto 
        /// </summary>
        /// <param name="product">ProductModelo de Producto</param>
        /// <returns></returns>
        //[OperationContract]
        //Product CreateFullProduct(Product product);
    }
}
