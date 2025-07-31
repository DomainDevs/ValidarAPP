using System;
using System.Collections;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Views
{
    /// <summary>
    /// Devuelve los valores de respuesta posible para las preguntas
    /// </summary>
    [Serializable()]
	public class RangeValuesView : BusinessView 
	{
		#region Internal Values
		private int _conceptId;
		private int _entityId;
		#endregion

		public RangeValuesView(){}


		#region Collections
		/// <summary>
		/// Colección de elementos <see 
		/// cref="Sistran.Core.Application.RulesService.RangeConcept">RangeConcept
		/// </see>
		/// </summary>
		/// <value>
		/// Coleccion de RangeConcept
		/// </value>
		public BusinessCollection RangeConcepts
		{
			get
			{
				return base["RangeConcepts"];
			}
		}

		/// <summary>
		/// Colección de elementos <see 
		/// cref="Sistran.Core.Application.RulesService.RangeEntity">RangeEntity
		/// </see>
		/// </summary>
		/// <value>
		/// Coleccion de RangeEntity
		/// </value>
		public BusinessCollection RangeEntities
		{
			get
			{
				return base["RangeEntities"];
			}
		}
		/// <summary>
		/// Colección de elementos <see 
		/// cref="Sistran.Core.Application.RulesService.RangeEntityValue">RangeEntityValue
		/// </see>
		/// </summary>
		/// <value>
		/// Coleccion de RangeEntityValue
		/// </value>
		public BusinessCollection RangeEntityValues
		{
			get
			{
				return base["RangeEntityValues"];
			}
		}

		#endregion

		/// <summary>
		/// Devuelve un DictionaryEntry con un IList de 
		/// posibles respuestas de tipo IDescriptionable 		
		/// relacionados con el concepto suministrado
		/// </summary>
		/// <param name="concept">Concepto del cual se quieren conocer sus valores de rango asociados</param>
		/// <returns>Si el concepto no posee valores de rango asociados se generara un error</returns>
		public DictionaryEntry GetRangeEntityValuesByConcept(SCREN.Concept concept)
		{
			this._conceptId=concept.ConceptId; 
			this._entityId=concept.EntityId;
            SCREN.RangeConcept rangeConcept =(SCREN.RangeConcept)this.RangeConcepts.Find(new BusinessObjectFilter(FindEntityByConcept));
			RangeEntity rangeEntity=(RangeEntity)base.GetObjectByRelation("RangeConceptRangeEntity",rangeConcept,false);
			return new DictionaryEntry("r" + rangeConcept.RangeEntityCode.ToString(),base.GetObjectsByRelation("RangeEntityRangeEntityValue",rangeEntity,false));
			/*ArrayList returnCol=new ArrayList();
			foreach(RangeEntityValue rv in base.GetObjectsByRelation("RangeEntityRangeEntityValue",rangeEntity,false))
			{
				returnCol.Add( new PossibleAnswer(rv)); 
			}			
			
			return new DictionaryEntry("r" + rangeConcept.RangeEntityCode.ToString(),returnCol);
			*/
		}
		
		/// <summary>
		/// Busca un entidad de tipo rango o lista segun corresponda. Esta funcion no debe usars fuera de la clase
		/// </summary>
		/// <param name="obj">BussinesObject que representa el objeto rango o lista que se esta buscando</param>
		/// <returns>Devuelve verdadero si es el que se esta buscando sino false</returns>
		private bool FindEntityByConcept(BusinessObject obj)
		{
			if(((int)obj.GetProperties()["ConceptId"] == this._conceptId)
				&& ((int)obj.GetProperties()["EntityId"]== this._entityId))
			{
				return true;
			}
			return false;
		}

	}
}