using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventComparisonTypeDAO
    {
        /// <summary>
        /// obtitne los tipos de comparadores segun la entidad y el tipo de query
        /// </summary>
        /// <param name="IdEntity">id de la entidad</param>
        /// <param name="IdQueryType">id del tipo del query</param>
        /// <returns></returns>
        public List<Models.EventComparisonType> GetOperatorConditionByIdEntityIdQueryType(int IdEntity, int IdQueryType)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.ComparatorCode, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.Description, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.SmallDesc, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.Symbol, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.TextInd, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.ComboInd, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.QueryInd, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventComparisonType.Properties.NumValues, "c")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                switch (IdQueryType)
                {
                    case 1:
                        where.Property(EVENTEN.CoEventComparisonType.Properties.TextInd, "c").Equal().Constant(1); //Texto
                        break;
                    case 2:
                        where.Property(EVENTEN.CoEventComparisonType.Properties.ComboInd, "c").Equal().Constant(1); //combo
                        break;
                    case 3:
                        where.Property(EVENTEN.CoEventComparisonType.Properties.QueryInd, "c").Equal().Constant(1); //query
                        break;
                    default:
                        break;
                }

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventComparisonType), "c");
                select.Where = where.GetPredicate();

                List<Models.EventComparisonType> result = new List<Models.EventComparisonType>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        result.Add(new Models.EventComparisonType
                        {
                            ComparatorCode = (int)reader["ComparatorCode"],
                            Description = (string)reader["Description"],
                            SmallDesc = (string)reader["SmallDesc"],
                            Symbol = (string)reader["Symbol"],
                            TextInd = (bool)reader["TextInd"],
                            ComboInd = (bool)reader["ComboInd"],
                            QueryInd = (bool)reader["QueryInd"],
                            NumValues = (decimal)reader["NumValues"]
                        });
                    }
                }
                return result.OrderBy(x => x.Description).ToList();

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetOperatorConditionByIdEntityIdQueryType", ex);
            }
        }

        /// <summary>
        /// Obtiene el comparador 
        /// </summary>
        /// <param name="IdComparator">id comparador</param>
        /// <returns></returns>
        public Models.EventComparisonType GetComparatorTypeByIdComparator(int IdComparator)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventComparisonType.CreatePrimaryKey(IdComparator);
                return ModelAssembler.CreateComparisonType(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventComparisonType);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetComparatorTypeByIdComparator", ex);
            }
        }
    }
}
