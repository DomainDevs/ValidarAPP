using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    /// <summary>
    /// Esta clase permite modelar una relación entre TechnicalPlan y las 
    /// entidades relacionadas.
    /// </summary>
    [Serializable()]
	public class TechnicalPlanRelatedEntitiesView : BusinessView
	{

		private BusinessCollection _individualDirectory = 
			new BusinessCollection();

		/// <summary>
		/// Constructor
		/// </summary>
		public TechnicalPlanRelatedEntitiesView()
		{
			this["NonRelatedAllyCoverageList"] = new BusinessCollection();
			this["AlliedTechnicalPlanCoverage"] = new BusinessCollection();
		}
		
		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de TechnicalPlanes
		/// </value>
		public BusinessCollection TechnicalPlanList
		{
			get
			{
				return this["TechnicalPlan"];
			}
		}
        public BusinessCollection PrefixLineBusiness
        {
            get
            {
                return this["PrefixLineBusiness"];
            }
        }

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de PRODEN.TechnicalPlanCoverage (aliadas)
		/// </value>
		public BusinessCollection AlliedTechnicalPlanCoverageList
		{
			get
			{
				return this["AlliedTechnicalPlanCoverage"];
			}
		}
		
		/// <summary>
		/// Colección de elementos de la entidad <see cref="Sistran.Core.Application.Common.Entities.CoveredRiskType">CoveredRiskType</see>
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad State
		/// </value>
		public BusinessCollection CoveredRiskTypeList
		{
			get
			{
				return this["CoveredRiskType"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad PRODEN.TechnicalPlanCoverage
		/// </value>
		public BusinessCollection TechnicalPlanCoverageList
		{
			get
			{
				return this["PRODEN.TechnicalPlanCoverage"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad Coverage
		/// </value>
		public BusinessCollection CoverageList
		{
			get
			{
				return this["Coverage"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad AllyCoverage
		/// </value>
		public BusinessCollection AllyCoverageList
		{
			get
			{
				return this["AllyCoverage"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad AllyCoverage NO RELACIONADOS
		/// CON EL PLAN TÉCNICO.
		/// </value>
		public BusinessCollection NonRelatedAllyCoverageList
		{
			get
			{
				return this["NonRelatedAllyCoverageList"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad <see 
		/// cref="Sistran.Core.Application.Common.
		///Entities.LineBusiness">LineBusiness</see>
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad LineBusiness
		/// </value>
		public BusinessCollection LineBusinessList
		{
			get
			{
				return this["LineBusiness"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad <see 
		/// cref="Sistran.Core.Application.Common.
		///Entities.LineBusiness">SubLineBusiness</see>
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad SubLineBusiness
		/// </value>
		public BusinessCollection SubLineBusinessList
		{
			get
			{
				return this["SubLineBusiness"];
			}
		}

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad Peril
		/// </value>
		public BusinessCollection PerilList
		{
			get
			{
				return this["Peril"];
			}
		}


		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Coleccion de objetos de la entidad InsuredObject
		/// </value>
		public BusinessCollection InsuredObjectList
		{
			get
			{
				return this["InsuredObject"];
			}
		}

		/// <summary>
		/// Obtener las coberturas aliadas no relacionadas al plan técnico, que
		/// estén relacionadas a una cobertura específica.
		/// </summary>
		/// <param name="coverageId">
		/// Identificador de la cobertura cuyas aliadas no relacionadas se 
		/// desea obtener.
		/// </param>
		/// <returns>
		/// Colección de coberturas aliadas.
		/// </returns>
		public BusinessCollection GetNonRelatedAllyCoverage(int coverageId)
		{
			BusinessCollection resultado = new BusinessCollection();

			foreach (QUOEN.AllyCoverage item in this.NonRelatedAllyCoverageList)
			{
				if (item.CoverageId == coverageId)
				{
					resultado.Add(item);
				}
			}

			return resultado;
		}

		/// <summary>
		/// Obtener las coberturas aliadas relacionadas al plan técnico, que
		/// estén relacionadas a una cobertura específica.
		/// </summary>
		/// <param name="coverageId">
		/// Identificador de la cobertura cuyas aliadas relacionadas se 
		/// desea obtener.
		/// </param>
		/// <returns>
		/// Colección de coberturas aliadas.
		/// </returns>
		public BusinessCollection GetRelatedAllyCoverage(int coverageId)
		{
			BusinessCollection resultado = new BusinessCollection();

			foreach (QUOEN.AllyCoverage item in this.AllyCoverageList)
			{
				if (item.CoverageId == coverageId)
				{
					resultado.Add(item);
				}
			}

			return resultado;
		}

		/**************************************************************************
		* RELACIÓN : CoverageInsuredObject
		**************************************************************************/
		/// <summary>
		/// Obtiene un InsuredObject a partir de una Coverage
		/// </summary>
		/// <param name="coverage">
		/// Coverage a la que esta relacionado el InsuredOject
		/// </param>
		/// <returns>
		/// Devuelve un InsuredObject relacionado con el Coverage
		/// </returns>
		public QUOEN.InsuredObject GetInsuredObjectByCoverage(
            QUOEN.Coverage coverage)
		{
			return (QUOEN.InsuredObject)this.GetObjectByRelation(
				"CoverageInsuredObject", 
				coverage, 
				false);
		}

		/**************************************************************************
		* RELACIÓN : CoveragePerfil
		**************************************************************************/
		/// <summary>
		/// Devuelve un Peril a partir de una Coverage
		/// </summary>
		/// <param name="coverage">Coverage a la que esta asociado el Peril</param>
		public QUOEN.Peril GetPerilByCoverage(
            QUOEN.Coverage coverage)
		{
			return (QUOEN.Peril)this.GetObjectByRelation(
				"CoveragePerfil", 
				coverage, 
				false);
		}

		/**************************************************************************
		* RELACIÓN : TechnicalPlanCoveredRiskType
		**************************************************************************/
		/// <summary>
		/// Obtiene el objeto CoveredRiskType relacionado a un 
		/// objeto TechnicalPlan.
		/// </summary>
		/// <param name="technicalPlan">
		/// Objeto TechnicalPlan a partir del que se desea obtener el objeto
		/// CoveredRiskType asociado.</param>
		/// <returns>
		/// Objeto CoveredRiskType.
		/// </returns>
        public PARAMEN.CoveredRiskType GetCoveredRiskTypeObjectByTechnicalPlan(
			PRODEN.TechnicalPlan technicalPlan)
		{
            return (PARAMEN.CoveredRiskType)this.GetObjectByRelation(
				"TechnicalPlanCoveredRiskType", technicalPlan, false);
		}

		/// <summary>
		/// Obtiene todos los objetos TechnicalPlan relacionados con un objeto
		/// CoveredRiskType dado.
		/// </summary>
		/// <param name="coveredRiskType">
		/// 
		/// </param>
		/// <returns></returns>
		public BusinessCollection GetTechnicalPlanListByCoveredRiskType(
            PARAMEN.CoveredRiskType coveredRiskType)
		{
			return this.GetObjectsByRelation(
				"TechnicalPlanCoveredRiskType", 
				coveredRiskType, 
				true);
		}

        /// <summary>
        /// Obtiene todas las coberturas aliadas de una cobertura especifica        
        /// </summary>
        /// <param name="coveredRiskType">
        /// 
        /// </param>
        /// <returns></returns>
        public BusinessCollection GetCoverageAllyByCoverage(
            QUOEN.Coverage coverage)
        {
            return this.GetObjectsByRelation(
                "AllyCoverageCoverage",
                coverage,
                true);
        }
	/**************************************************************************
	 * RELACIÓN : TechnicalPlanTechnicalPlanCoverage
	 **************************************************************************/
		/// <summary>
		/// Obtiene la lista de objetos PRODEN.TechnicalPlanCoverage relacionado a un 
		/// objeto TechnicalPlan.
		/// </summary>
		/// <param name="technicalPlan">
		/// Objeto TechnicalPlan a partir del que se desea obtener los objetos
		/// PRODEN.TechnicalPlanCoverage asociado.</param>
		/// <returns>
		/// Lista de objetos CoveredRiskType.
		/// </returns>
		public BusinessCollection GetTechnicalPlanCoverageListByTechnicalPlan(
            PRODEN.TechnicalPlan technicalPlan)
		{
			BusinessCollection resultado = new BusinessCollection();
		
			foreach(PRODEN.TechnicalPlanCoverage technicalPlanCoverage in
				this.TechnicalPlanCoverageList)
			{
				if(technicalPlanCoverage.TechnicalPlanId == 
					technicalPlan.TechnicalPlanId)
				{
					resultado.Add(technicalPlanCoverage);
				}
			}

			return resultado;
		}
		
		/// <summary>
		/// Obtiene el TechnicalPlan relacionados con un objeto
		/// PRODEN.TechnicalPlanCoverage dado.
		/// </summary>
		/// <param name="technicalPlanCoverage">
		/// PRODEN.TechnicalPlanCoverage al que esta asociado el TechnicalPlan
		/// </param>
		/// <returns>TechnicalPlan relacionado con el PRODEN.TechnicalPlanCoverage</returns>
		public PRODEN.TechnicalPlan GetTechnicalPlanByTechnicalPlanCoverage(
            PRODEN.TechnicalPlanCoverage technicalPlanCoverage)
		{
			return (PRODEN.TechnicalPlan)this.GetObjectByRelation(
				"TechnicalPlanTechnicalPlanCoverage", 
				technicalPlanCoverage,
				true);
		}

	/**************************************************************************
	 * RELACIÓN : PRODEN.TechnicalPlanCoverage
	 **************************************************************************/
		/// <summary>
		/// Obtiene la lista de objetos TechnicalPlan relacionado a un 
		/// objeto Coverage.
		/// </summary>
		/// <param name="coverage">
		/// Objeto Coverage a partir del que se desea obtener los objetos
		/// TechnicalPlan asociado.</param>
		/// <returns>
		/// Lista de objetos TechnicalPlan.
		/// </returns>
		public BusinessCollection GetTechnicalPlanListByCoverage(
			QUOEN.Coverage coverage)
		{
			BusinessCollection resultado = new BusinessCollection();
		
			foreach(PRODEN.TechnicalPlanCoverage technicalPlanCoverage in
				this.GetObjectsByRelation("TechnicalPlanCoverageCoverage"
				,coverage,true))
			{
				resultado.Add(this.GetTechnicalPlanByTechnicalPlanCoverage(
					technicalPlanCoverage));
			}

			return resultado;
		}
		
		/// <summary>
		/// Obtiene el TechnicalPlan relacionados con un objeto
		/// Coverage dado.
		/// </summary>
		/// <param name="technicalPlan">
		/// TechnicalPlan al que esta asociada la lista de Coverage
		/// </param>
		/// <returns>Lista de Coverage asociadas al TechnicalPlan</returns>
		public BusinessCollection GetCoverageListByTechnicalPlan(
            PRODEN.TechnicalPlan technicalPlan)
		{
			BusinessCollection resultado = new BusinessCollection();

			foreach(PRODEN.TechnicalPlanCoverage technicalPlanCoverage in
				this.GetTechnicalPlanCoverageListByTechnicalPlan(
				technicalPlan))
			{
				resultado.Add(this.GetObjectByRelation(
					"TechnicalPlanCoverageCoverage",technicalPlanCoverage
					,false));
			}
			return resultado;
		}

		/// <summary>
		/// Devuelve el Coverage relacionado a un PRODEN.TechnicalPlanCoverage
		/// </summary>
		/// <param name="technicalPlanCoverage">Objeto PRODEN.TechnicalPlanCoverage
		/// al que esta relacionado el Coverage que se desea obtener</param>
		public QUOEN.Coverage GetCoverageByTechnicalPlanCoverage(
			PRODEN.TechnicalPlanCoverage technicalPlanCoverage)
		{
			return (QUOEN.Coverage)this.GetObjectByRelation(
				"TechnicalPlanCoverageCoverage", 
				technicalPlanCoverage, 
				false);
		}

		/**************************************************************************
		* RELACIÓN : CoverageSubLineBusiness
		**************************************************************************/
		/// <summary>
		/// Devuelve un SubLineBusiness a partir de un Coverage
		/// </summary>
		/// <param name="coverage">Objeto Coverage
		/// al que esta relacionado el SubLineBusiness que se desea obtener</param>
		public COMMEN.SubLineBusiness GetSubLineBusinessByCoverage(
			QUOEN.Coverage coverage)
		{
			return (COMMEN.SubLineBusiness)this.GetObjectByRelation(
				"CoverageSubLineBusiness", 
				coverage, 
				false);
		}

		/**************************************************************************
		* RELACIÓN : SubLinesBusinessLineBusiness
		**************************************************************************/
		/// <summary>
		/// Devuelve un LineBusiness a partir de un SubLineBusiness
		/// </summary>
		/// <param name="subLineBusiness">SubLineBusiness al que 
		/// esta asociado el LineBusiness</param>
		public COMMEN.LineBusiness GetLineBusinessBySubLineBusiness(
            COMMEN.SubLineBusiness subLineBusiness)
		{
			return (COMMEN.LineBusiness)this.GetObjectByRelation(
				"SubLinesBusinessLineBusiness", 
				subLineBusiness, 
				false);
		}

		/**************************************************************************
		* RELACIÓN : TechnicalPlanCoveredRiskType
		**************************************************************************/
		/// <summary>
		/// Devuelve el CoveredRiskType relacionado a un TechnicalPlan.
		/// </summary>
		/// <param name="technicalPlan">Objeto TechnicalPlan al que esta 
		/// asociado el CoveredRiskType.</param>
		public PARAMEN.CoveredRiskType GetCoveredRiskTypeByTechnicalPlan(
			PRODEN.TechnicalPlan technicalPlan)
		{
			return (PARAMEN.CoveredRiskType)this.GetObjectByRelation(
				"TechnicalPlanCoveredRiskType", 
				technicalPlan, 
				false);
		}

		/// <summary>
		/// Devuelve un LineBusiness asociado a una Coverage.
		/// </summary>
		/// <param name="coverage">Coverage del cual se desea traer el
		/// LineBusiness
		/// </param>
		public COMMEN.LineBusiness GetLineBusinessByCoverage(
			QUOEN.Coverage coverage)
		{
			COMMEN.SubLineBusiness subLineBusiness = 
				this.GetSubLineBusinessByCoverage(coverage);

			return this.GetLineBusinessBySubLineBusiness(subLineBusiness); 
		}
		
		/**********************************************************************
		* RELACIÓN : AllyCoverage - TechnicalPlan
		**********************************************************************/

		/// <summary>
		/// Obtiene una colección de objetos de la entidad AllyCoverage 
		/// relacionado a un objeto de la entidad TechnicalPlan.
		/// </summary>
		/// <param name="technicalPlan">
		/// Objeto de la entidad TechnicalPlan a partir del que se desea 
		/// obtener una colección de objetos de la entidad AllyCoverage
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad AllyCoverage.
		/// </returns>
		public BusinessCollection GetAllyCoverageListByTechnicalPlan(
			PRODEN.TechnicalPlan technicalPlan)
		{
			BusinessCollection	retorno = new BusinessCollection();

			// Obtener todas las PRODEN.TechnicalPlanCoverage.
			foreach(PRODEN.TechnicalPlanCoverage item in 
				this.GetTechnicalPlanCoverageListByTechnicalPlan(
					technicalPlan))
			{	
				QUOEN.AllyCoverage allyCoverage;

				allyCoverage = (QUOEN.AllyCoverage) this.GetObjectByRelation(
					"AllyCoverageTechnicalPlanCoverage", item, true);

				//Si es aliada, la cargo en el retorno.
				if(allyCoverage!=null)
				{
					retorno.Add(allyCoverage);
				}
			}

			return retorno;
		}

		/// <summary>
		/// Obtener un TechnicalPlan a partir del id.
		/// </summary>
		/// <param name="technicalPlanId">
		/// Id del TechnicalPlan a obtener.
		/// </param>
		/// <returns>
		/// TechnicalPlan correspondiente al Id.
		/// </returns>
		public PRODEN.TechnicalPlan GetTechnicalPlanByTechnicalPlanId(int technicalPlanId)
		{
			PRODEN.TechnicalPlan resultado = null;
			foreach (PRODEN.TechnicalPlan technicalPlan in this.TechnicalPlanList)
			{
				if (technicalPlan.TechnicalPlanId == technicalPlanId)
				{
					resultado = technicalPlan;
				}
			}

			return resultado;
		}

		/**********************************************************************
		* RELACIÓN : AlliedTechnicalPlanCoverage - PRODEN.TechnicalPlanCoverage
		**********************************************************************/
		/// <summary>
		/// Obtiene los objetos de la entidad TechnicalPlanCoveage, 
		/// que son PRODEN.TechnicalPlanCoverage aliadas del objeto 
		/// PRODEN.TechnicalPlanCoverage (Cobertura) dado.
		/// </summary>
		/// <param name="technicalPlanCoverage">
		/// Objeto de la entidad PRODEN.TechnicalPlanCoverage a partir del que se 
		/// desea obtener todos los objetos PRODEN.TechnicalPlanCoverage (aliadas)
		/// relacionados a través de la entidad PRODEN.TechnicalPlanCoverage
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad PRODEN.TechnicalPlanCoverage.
		/// </returns>
		public BusinessCollection 
			GetAlliedTechnicalPlanCoverageListByTechnicalPlanCoverage(
			PRODEN.TechnicalPlanCoverage technicalPlanCoverage)
		{
			BusinessCollection respuesta = new BusinessCollection();

			// Recorrer la lista de aliadas (PRODEN.TechnicalPlanCoverage)
			foreach (PRODEN.TechnicalPlanCoverage item in this
				.AlliedTechnicalPlanCoverageList)
			{
				// Agregar al resultado sólo las PRODEN.TechnicalPlanCoverage que 
				// tienen como padre a la PRODEN.TechnicalPlanCoverage pasada por 
				// parámetro.
				if(item.MainCoverageId == technicalPlanCoverage.CoverageId
					&& item.TechnicalPlanId == technicalPlanCoverage
					.TechnicalPlanId)
				{
					respuesta.Add(item);
				}
			}
			return respuesta;
		}

		/// <summary>
		/// Obtener un coverage a partir de un PRODEN.TechnicalPlanCoverage(aliada).
		/// </summary>
		/// <param name="coverageId">
		/// Identificador de cobertura.
		/// </param>
		/// <returns>
		/// Entidad Coverage.
		/// </returns>
		public QUOEN.Coverage GetCoverageByCoverageId(
			int coverageId)
		{
            QUOEN.Coverage resultado = null;
			foreach (QUOEN.Coverage coverage in this.CoverageList)
			{
				if (coverage.CoverageId == coverageId)
				{
					resultado = coverage;
				}
			}

			return resultado;
		}
		/// <summary>
		/// Devuelve el Coverage relacionado a un PRODEN.TechnicalPlanCoverage(aliada)
		/// </summary>
		/// <param name="alliedTechnicalPlanCoverage">Objeto PRODEN.TechnicalPlanCoverage
		/// al que esta relacionado el Coverage que se desea obtener</param>
		public QUOEN.Coverage GetCoverageByAlliedTechnicalPlanCoverage(
			PRODEN.TechnicalPlanCoverage alliedTechnicalPlanCoverage)
		{
			return GetCoverageByCoverageId(alliedTechnicalPlanCoverage.CoverageId);
		}
	}
}
