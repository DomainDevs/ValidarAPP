using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService
{
    [ServiceContract]
    public interface IAircraftBusinessService
    {
        #region Aircraft

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
        #region GetAircraftTypes
        /// <summary>
        /// Retorna un listado de tipos de aeronaves por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de tipos de aeronaves</returns>
        [OperationContract]
        List<AircraftType> GetAircraftTypes(int prefixId);
        #endregion
        #region GetTypesByPrefixId
        /// <summary>
        /// Retorna un listado de tipos de aeronaves por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de tipos de aeronaves</returns>

        [OperationContract]
        List<AircraftType> GetTybeByTypesByPrefixId(int prefixId);
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
        List<Use> GetUseByusessByPrefixId(int prefixId);

        #endregion
        #endregion

        #region Claims

        [OperationContract]
        List<Aircraft> GetRiskAircraftsByEndorsementId(int endorsemetId);

        [OperationContract]
        Aircraft GetRiskAircraftByRiskId(int riskId);

        [OperationContract]
        List<Aircraft> GetRiskAircraftsByInsuredId(int insuredId);

        #endregion
    }
}