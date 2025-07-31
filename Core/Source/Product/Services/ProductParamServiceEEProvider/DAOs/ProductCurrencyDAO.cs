using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.ProductParamService.EEProvider.DAOs
{
    public class ProductCurrencyDAO
    {
        /// <summary>
        /// crea el tipo de moneda para un producto
        /// </summary>
        /// <param name="productId">id del prodcuto</param>
        /// <param name="currency">moneda para el producto</param>
        //public static void CreateProductCurrency(int productId, CommonModel.Currency currency)
        //{
        //    PrimaryKey key = ProductCurrency.CreatePrimaryKey(productId, currency.Id);
        //    ProductCurrency ProductCurrencyEntities = (ProductCurrency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //    if (ProductCurrencyEntities == null)
        //    {
        //        SaveProductCurrency(productId, currency);
        //    }
        //}

        /// <summary>
        /// guarda el tipo de moneda para un producto
        /// </summary>
        /// <param name="productId">id del prodcuto</param>
        /// <param name="currency">moneda para el producto</param>
        //private static void SaveProductCurrency(int productId, CommonModel.Currency currency)
        //{
        //    ProductCurrency item = EntityAssembler.CreateProductCurrency(currency, productId);
        //    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //}
    }
}
