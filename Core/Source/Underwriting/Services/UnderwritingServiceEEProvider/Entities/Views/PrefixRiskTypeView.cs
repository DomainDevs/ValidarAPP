using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    /// <summary>
    /// Esta clase permite modelar una relacion entre Prefix,
    /// PrefixLineBusiness, LineBusiness, CoveredRiskType.
    /// </summary>
    [Serializable()]
	public class PrefixRiskTypeView : BusinessView
	{

		/// <summary>
		/// Constructor
		/// </summary>
		public PrefixRiskTypeView()
		{}
		
		/// <summary>
		/// Colección de elementos de la entidad 
		/// <see cref="Sistran.Core.Application.Common.Entities
		/// .Prefix">
		/// Prefix</see>
		/// </summary>
		/// <value>
		/// Coleccion de Prefix
		/// </value>
		public BusinessCollection PrefixList
		{
			get
			{
				return this["Prefix"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad <see 
		/// cref="Sistran.Core.Application.Common.Entities.PrefixLineBusiness">
		/// PrefixLineBusiness</see>
		/// </summary>
		/// <value>
		/// Coleccion de PrefixLineBusiness
		/// </value>
		public BusinessCollection PrefixLineBusinessList
		{
			get
			{
				return this["PrefixLineBusiness"];
			}
		}
	
		/// <summary>
		/// Colección de elementos de la entidad <see cref="Sistran.Core
		/// .Application.Common.Entities.LineBusiness">LineBusiness</see>
		/// </summary>
		public BusinessCollection LineBusinessList
		{
			get
			{
				return this["LineBusiness"];
			}
		}

        /// <summary>
        /// Colección de elementos de la entidad <see cref="Sistran.Core
        /// .Application.Common.Entities.SubLineBusiness">SubLineBusiness</see>
        /// </summary>
        public BusinessCollection SubLineBusinessList
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }

		/// <summary>
		/// Colección de elementos de la entidad <see cref="Sistran.Core
		/// .Application.Common.Entities.CoveredRiskType">CoveredRiskType</see>
		/// </summary>
		public BusinessCollection CoveredRiskTypeList
		{
			get
			{
				return this["CoveredRiskType"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad <see cref="Sistran.Core
		/// .Application.Common.Entities.LineBusinessCoveredRiskType">LineBusinessCoveredRiskType</see>
		/// </summary>
		public BusinessCollection LineBusinessCoveredRiskTypeList
		{
			get
			{
				return this["LineBusinessCoveredRiskType"];
			}
		}

		/**********************************************************************
		* RELACIÓN : Prefix - PrefixLineBusiness
		**********************************************************************/
		/// <summary>
		/// Obtiene el objeto de la entidad Prefix relacionado a un objeto de
		/// la entidad PrefixLineBusiness.
		/// </summary>
		/// <param name="prefixLineBusiness">
		/// Objeto de la entidad PrefixLineBusiness a partir del que se desea 
		/// obtener 
		/// el objeto de la entidad Prefix asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad Prefix.
		/// </returns>
		public COMMEN.Prefix GetPrefixByPrefixLineBusiness(
           COMMEN.PrefixLineBusiness prefixLineBusiness)
		{
			return (COMMEN.Prefix) this.GetObjectByRelation(
				"PrefixPrefixLineBusiness", prefixLineBusiness, true);
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad PrefixLineBusiness 
		/// relacionado a un objeto de la entidad Prefix.
		/// </summary>
		/// <param name="prefix">
		/// Objeto de la entidad Prefix a partir del que se desea obtener 
		/// una colección de objetos de la entidad PrefixLineBusiness asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad PrefixLineBusiness.
		/// </returns>
		public BusinessCollection GetPrefixLineBusinessListByPrefix(
			COMMEN.Prefix prefix)
		{
			return this.GetObjectsByRelation(
				"PrefixPrefixLineBusiness", prefix, false);
		}

		/**********************************************************************
		* RELACIÓN : LineBusiness - PrefixLineBusiness
		**********************************************************************/
		/// <summary>
		/// Obtiene el objeto de la entidad LineBusiness relacionado a un 
		/// objeto de la entidad PrefixLineBusiness.
		/// </summary>
		/// <param name="prefixLineBusiness">
		/// Objeto de la entidad PrefixLineBusiness a partir del que se desea
		/// obtener el objeto de la entidad LineBusiness asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad LineBusiness.
		/// </returns>
		public COMMEN.LineBusiness GetLineBusinessByPrefixLineBusiness(
			COMMEN.PrefixLineBusiness prefixLineBusiness)
		{
			return (COMMEN.LineBusiness) this.GetObjectByRelation(
				"LineBusinessPrefixLineBusiness", prefixLineBusiness, true);
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad PrefixLineBusiness
		/// relacionadoa un objeto de la entidad LineBusiness.
		/// </summary>
		/// <param name="lineBusiness">
		/// Objeto de la entidad LineBusiness a partir del que se desea 
		/// obtener una colección de objetos de la entidad PrefixLineBusiness 
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad PrefixLineBusiness.
		/// </returns>
		public BusinessCollection GetPrefixLineBusinessListByLineBusiness(
			COMMEN.LineBusiness lineBusiness)
		{
			return this.GetObjectsByRelation(
				"LineBusinessPrefixLineBusiness", lineBusiness, false);
		}

		/**********************************************************************
		* RELACIÓN : CoveredRiskType - LineBusiness
		**********************************************************************/
		/// <summary>
		/// Obtiene el objeto de la entidad CoveredRiskType relacionado a un 
		/// objeto de la entidad LineBusiness.
		/// </summary>
		/// <param name="lineBusiness">
		/// Objeto de la entidad LineBusiness a partir del que se desea 
		/// obtener el objeto de la entidad CoveredRiskType asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad CoveredRiskType.
		/// </returns>
		public BusinessCollection GetCoveredRiskTypeListByLineBusiness(COMMEN.LineBusiness lineBusiness)
		{

			BusinessCollection returnList = new BusinessCollection();
			foreach (COMMEN.LineBusinessCoveredRiskType lineBusinessCoveredRiskType
						 in this.GetObjectsByRelation("LineBusinessLineBusinessCoveredRiskType", lineBusiness, false))
			{
				returnList.AddRange(this.GetObjectsByRelation(
					"LineBusinessCoveredRiskTypeCoveredRiskType", 
					lineBusinessCoveredRiskType, 
					false));
			}

			return returnList;
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad LineBusiness 
		/// relacionado a un objeto de la entidad CoveredRiskType.
		/// </summary>
		/// <param name="coveredRiskType">
		/// Objeto de la entidad CoveredRiskType a partir del que se desea 
		/// obtener una colección de objetos de la entidad LineBusiness
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad LineBusiness.
		/// </returns>
		public BusinessCollection GetLineBusinessListByCoveredRiskType(PARAMEN.CoveredRiskType coveredRiskType)
		{

			BusinessCollection returnList = new BusinessCollection();

			foreach (COMMEN.LineBusinessCoveredRiskType lineBusinessCoveredRiskType in	this.LineBusinessCoveredRiskTypeList)
			{
				if (lineBusinessCoveredRiskType.CoveredRiskTypeCode == coveredRiskType.CoveredRiskTypeCode)
				{
					PrimaryKey key = COMMEN.LineBusiness.CreatePrimaryKey(lineBusinessCoveredRiskType.LineBusinessCode);
					if (!returnList.Contains(key))
					{
						if (this.LineBusinessList.Contains(key))
						{
							returnList.Add(this.LineBusinessList[key]);
						}
					}
				}
			}

			return returnList;

		}

        /**********************************************************************
           * RELACIÓN : LineBusiness - SubLineBusiness
           **********************************************************************/
        /// <summary>
        /// Obtiene el objeto de la entidad SubLineBusiness relacionado a un 
        /// objeto de la entidad LineBusiness.
        /// </summary>
        /// <param name="SubLineBusiness">
        /// Objeto de la entidad LineBusiness a partir del que se desea
        /// obtener el objeto de la entidad SubLineBusiness asociado.
        /// </param>
        /// <returns>
        /// Objeto de la entidad SubLineBusiness.
        /// </returns>
        public BusinessCollection GetSubLineBusinessListByLineBusiness(
            COMMEN.LineBusiness lineBusiness)
        {
            return this.GetObjectsByRelation(
                "LineBusinessSubLineBusiness", lineBusiness, false);
        }

        public BusinessCollection GetLineBusinessListByPrefix(COMMEN.Prefix prefix)
        {
            BusinessCollection resultList = new BusinessCollection();
            foreach (COMMEN.PrefixLineBusiness prefixLineBusiness in this.PrefixLineBusinessList)
            {
                if (prefixLineBusiness.PrefixCode == prefix.PrefixCode)
                {
                    PrimaryKey lineBusinessKey = COMMEN.LineBusiness.CreatePrimaryKey(prefixLineBusiness.LineBusinessCode);
                    if (this.LineBusinessList.Contains(lineBusinessKey))
                    {
                        resultList.Add(this.LineBusinessList[lineBusinessKey]);
                    }
                }
            }
            return resultList;
        }

	}
}
