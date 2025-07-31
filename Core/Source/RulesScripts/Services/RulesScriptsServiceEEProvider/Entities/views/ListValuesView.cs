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
    [Serializable]
	public class ListValuesView : BusinessView 
	{
		#region Internal Values
		private int _conceptId;
		private int _entityId;
		#endregion


		#region Collections

		public BusinessCollection ListConcepts
		{
			get
			{
				return base["ListConcepts"];
			}
		}

		public BusinessCollection ListEntities
		{
			get
			{
				return base["ListEntities"];
			}
		}

		public BusinessCollection ListEntityValues
		{
			get
			{
				return base["ListEntityValues"];
			}
		}

		#endregion


		public DictionaryEntry GetListEntityValuesByConcept(SCREN.Concept concept)
		{					
			_conceptId = concept.ConceptId; 
			_entityId = concept.EntityId;

			ListConcept listConcept = (ListConcept)
                ListConcepts.Find(FindEntityByConcept);
			ListEntity listEntity=(ListEntity)GetObjectByRelation("ListConceptListEntity",listConcept,false);
			return new DictionaryEntry("l" + listConcept.ListEntityCode, GetObjectsByRelation("ListEntityListEntityValue",listEntity,false));

		}

		/// <summary>
		/// Busca un entidad de tipo rango o lista segun corresponda. Esta funcion no debe usars fuera de la clase
		/// </summary>
		/// <param name="obj">BussinesObject que representa el objeto rango o lista que se esta buscando</param>
		/// <returns>Devuelve verdadero si es el que se esta buscando sino false</returns>
		private bool FindEntityByConcept(BusinessObject obj)
		{
			if(((int)obj.GetProperties()["ConceptId"] == _conceptId)
				&& ((int)obj.GetProperties()["EntityId"] == _entityId))
			{
				return true;
			}
			return false;
		}

	}
}