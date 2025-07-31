namespace Sistran.Core.Application.ProductParamService.EEProvider
{
    using Sistran.Core.Application.ProductParamService;
    using Sistran.Core.Application.Utilities.Error;
    using System.ServiceModel;

    /// <summary>
    /// Provider para ProductParamService
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProductParamServiceEEProviderCore : IProductParamServiceCore
    {
        //public ErrorModel ErrorTypeSevice { get; private set; }
        ///// <summary>
        ///// Crear Producto 
        ///// </summary>
        ///// <param name="product">Modelo de Producto</param>
        ///// <returns></returns>
        //public Model.Product CreateFullProduct(Model.Product product)
        //{
        //    try
        //    {
        //        ProductDAO productDAO = new ProductDAO();
        //        return productDAO.CreateFullProduct(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        ///// <summary>
        ///// Obtiene informacion principal del producto 
        ///// </summary>
        ///// <param name="productId">Id producto</param>
        ///// <returns>Producto</returns>
        //public List<Model.Product> GetMainProductByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        //{
        //    try
        //    {
        //        ProductDAO productDAO = new ProductDAO();
        //        return productDAO.GetMainProductByPrefixIdDescriptionProductId(prefixId, description, productId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        ///// <summary>
        ///// Obtiene Datos adicionales relacionados al Id del Producto
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public Model.Product GetDataAditionalByProductId(int productId)
        //{
        //    try
        //    {
        //        ProductDAO productDAO = new ProductDAO();
        //        return productDAO.GetDataAditionalByProductId(productId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }

        //}

        ///// <summary>
        ///// generar Archivo del Producto
        ///// </summary>
        ///// <param name="id">Identificador del producto</param>
        ///// <param name="fileName">Nombre Archivo</param>
        ///// <returns></returns>
        ///// <exception cref="BusinessException"></exception>
        //public string GenerateFileToProduct(int id, string fileName)
        //{
        //    try
        //    {
        //        var ProductDAO = new ProductDAO();
        //        return ProductDAO.GenerateFileToProduct(id, fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}
    }
}
