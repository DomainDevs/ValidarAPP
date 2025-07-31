using Sistran.Core.Application.Scripts.Entities;
using System;
using System.Collections;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for ConceptValueDTODictionary.
    /// </summary>
    [Serializable()]
    public class ConceptValueDTODictionary : DictionaryBase, IDynamicConceptContainer
    {
        public ConceptValueDTODictionary()
        {
        }

        public object this[int conceptId]
        {
            get
            {
                if (this.InnerHashtable.ContainsKey(conceptId))
                {
                    return ((ConceptValueDTO)this.InnerHashtable[conceptId]).Value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ConceptValueDTO dto = (ConceptValueDTO)this.InnerHashtable[conceptId];
                if (dto == null)
                {
                    dto = new ConceptValueDTO(-1, conceptId);
                    this.InnerHashtable[conceptId] = dto;
                }

                dto.Value = value;
            }
        }

        public void Add(ConceptValueDTO dto)
        {
            this.InnerHashtable.Add(dto.ConceptCode, dto);
        }

        public void Add(IList conceptValueDTOlist)
        {
            foreach (ConceptValueDTO dto in conceptValueDTOlist)
            {
                this.InnerHashtable.Add(dto.ConceptCode, dto);
            }
        }

        public void SetEntityId(int entityId)
        {
            foreach (ConceptValueDTO dto in this)
            {
                dto.EntityCode = entityId;
            }
        }

        public void SetPrimaryKey(Sistran.Core.Framework.DAF.PrimaryKey key)
        {
            foreach (ConceptValueDTO dto in this)
            {
                dto.PrimaryKey = key;
            }
        }

        public void SetKeys(Sistran.Core.Framework.DAF.PrimaryKey key, int entityId)
        {
            foreach (ConceptValueDTO dto in this)
            {
                dto.PrimaryKey = key;
                dto.EntityCode = entityId;
            }
        }

        #region IDynamicConceptContainer Members

        public object GetDynamicConcept(int conceptId)
        {
            return this[conceptId];
        }

        public void SetDynamicConcept(int conceptId, object value)
        {
            this[conceptId] = value;
        }

        #endregion

        #region IEnumerable Members

        public new IEnumerator GetEnumerator()
        {
            return this.InnerHashtable.Values.GetEnumerator();
        }

        #endregion
    }
}
