using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Marines.MarineBusinessService
{
    [ServiceContract]
    public interface IMarineBusinessService
    {
        #region Marine        

        #region GetMakes   

        /// <summary>
        /// Retorna el listado de marcas
        /// </summary>
        /// <returns>Listado de marcas</returns>
        [OperationContract]
        List<Make> GetMakes();
        #endregion
        #region GetModelByMakeId
        /// <summary>
        /// Lista los modelos a partir de la marca
        /// </summary>
        /// <param name="makeId">Identificador de la marca</param>
        /// <returns>Listado de modelos</returns>
        [OperationContract]
        List<Model> GetModelByMakeId(int makeId);
        #endregion
        #region GetOperators
        /// <summary>
        /// Retorna un listado de explotadores de aeronaves
        /// </summary>
        /// <returns>Listado de explotadores de aeronaves</returns>
        [OperationContract]
        List<Operator> GetOperators();
        #endregion
        #region  GetRegisters
        /// <summary>
        /// Retorna el listado de matrículas que se encuentran registradas
        /// </summary>
        /// <returns>Listado de matrículas</returns>
        [OperationContract]
        List<Register> GetRegisters();

        #endregion
        #region GetMarineTypes
        /// <summary>
        /// Retorna un listado de tipos de aeronaves por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de tipos de aeronaves</returns>
        [OperationContract]
        List<MarineType> GetMarineTypes(int prefixId);
        #endregion
        #region GetTypesByPrefixId
        /// <summary>
        /// Retorna un listado de tipos de aeronaves por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de tipos de aeronaves</returns>
        [OperationContract]
        List<MarineTypePrefix> GetTypesByPrefixId(/*int prefixId*/);
        [OperationContract]
        List<Use> GetUseByusessByPrefixId(int prefixId);
        #endregion
        #region GetUses
        /// <summary>
        /// Retorna un listado de usos de aeronave 
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de usos de aeronave</returns>
        [OperationContract]
        List<Use> GetUses(int prefixId);
        #endregion
        #region GetUsesByPrefixId
        /// <summary>
        /// Retorna un listado de usos de aeronave por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de usos de aeronave</returns>
        [OperationContract]
        List<UsePrefix> GetUsesByPrefixId(int prefixId);
        #endregion

        #endregion

        [OperationContract]
        List<Marine> GetMarinesByEndorsementIdModuleType(int endorsementIds);
    }
}