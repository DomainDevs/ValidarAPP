using Sistran.Co.Application.Data;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Co.Application.Data;


namespace Sistran.Company.Application.UniquePersonServices.V1.DAOs
{
    public class ConsortiumDAO
    {
        /// <summary>
        /// Crear Consorcio
        /// </summary>
        /// <param name="consortium">Modelo Consorcio</param>
        /// <returns></returns>
        public Models.CompanyConsortium CreateConsortium(Models.CompanyConsortium consortium)
        {
            CoConsortium coConsortiumEntity = EntityAssembler.CreateConsortium(consortium);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(coConsortiumEntity);
            return ModelAssembler.CreateConsortium(coConsortiumEntity);
        }

        /// <summary>
        /// Actualizar Consorcio
        /// </summary>
        /// <param name="consortium">The consortium.</param>
        /// <returns></returns>
        public Models.CompanyConsortium UpdateCoConsortium(Models.CompanyConsortium coConsortium)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(coConsortium.IndividualId);
            filter.And();
            filter.Property(CoConsortium.Properties.InsuredCode, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(coConsortium.InsuredCode);

            CoConsortium coConsortiumEntity = (CoConsortium)DataFacadeManager.Instance.GetDataFacade().List(typeof(CoConsortium), filter.GetPredicate()).FirstOrDefault();
            if (coConsortiumEntity != null)
            {
                coConsortiumEntity.IsMain = coConsortium.IsMain;
                coConsortiumEntity.ParticipationRate = coConsortium.ParticipationRate;
                coConsortiumEntity.StartDate = coConsortium.StartDate;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(coConsortiumEntity);
                return ModelAssembler.CreateConsortium(coConsortiumEntity);
            }

            return CreateConsortium(coConsortium);

        }

        /// <summary>
        /// Obtener Consorcio por Asegurado
        /// </summary>
        /// <param name="insuredCode">The insured code.</param>
        /// <returns></returns>
        public List<Models.CompanyConsortium> GetCoConsortiumsByInsuredCode(int insuredCode)
        {
            Entities.views.CoConsorcioViewV1 view = new Entities.views.CoConsorcioViewV1();
            ViewBuilder builder = new ViewBuilder("CoConsorcioViewV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entities.Insured.Properties.InsuredCode, typeof(entities.Insured).Name);
            filter.Equal();
            filter.Constant(insuredCode);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.CompanyConsortium> consortiums = ModelAssembler.CreateCoConsortiums(view);
            return consortiums;
        }

        /// <summary>
        /// Obtiene un Consorcio
        /// </summary>
        /// <param name="insuredId">The insured identifier.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyConsortium GetConsortiumsByInsuredIdByIndividualId(int insuredId, int IndividualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoConsortium.Properties.InsuredCode, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(insuredId);
            filter.And();
            filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            CoConsortium consortium = (CoConsortium)DataFacadeManager.Instance.GetDataFacade().List(typeof(CoConsortium), filter.GetPredicate()).FirstOrDefault();
            Models.CompanyConsortium consortiumModel = ModelAssembler.CreateConsortium(consortium);
            return consortiumModel;
        }

        /// <summary>
        /// Deletes the agent prefix by indivual identifier.
        /// </summary>
        /// <param name="IndivualId">The indivual identifier.</param>
        public void DeleteConsortiumByInsuredCode(int InsuredCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CoConsortium.Properties.InsuredCode, InsuredCode);
            DataFacadeManager.Instance.GetDataFacade().Delete<CoConsortium>(filter.GetPredicate());
        }

        #region Cumplimiento

        /// <summary>
        /// determinar si el individuales un cosrciado buscarlo en los consorcios R2
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public bool IsConsortiumindividualId(int individualId)
        {
            //Consorcios
            ConsorcioViewCoV1 view = new ConsorcioViewCoV1();
            ViewBuilder builder = new ViewBuilder("ConsorcioViewCoV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name).Equal().Constant(individualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (view.CoConsortiumList != null && view.CoConsortiumList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// vrific si es consorcio
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public bool IsConsortiumindividualIdR1(int individualId)
        {
            bool consorcioTypeValue = false;


            ConsorcioViewCoV1 viewco = new ConsorcioViewCoV1();
            ViewBuilder builder = new ViewBuilder("ConsorcioViewCoV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Insured.Properties.IndividualId);
            filter.Equal();
            filter.Constant(individualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewco);
            System.Collections.IList consorcio = viewco.CoAssociationTypeList;
            foreach (CoAssociationType consorcioType in consorcio)
            {
                consorcioTypeValue = consorcioType.IsConsortium;
            }
            return consorcioTypeValue;
        }

        public decimal GetAvailableCumulus(int individualId, int currencyCode, int prefixCode, System.DateTime issueDate)
        {
            decimal available = 0;
            decimal availableTotal = 0;
            System.Collections.IList consorcio = ConsortiumDAO.ListIndividualConsorcio(individualId);
            decimal cumulus = 0;
            decimal quota = 0;
          
            foreach (object coConsortium in consorcio)
            {
             
                if (!(coConsortium is Insured))
                {
                    CoConsortium coConsortiumObj = (CoConsortium)coConsortium;
                    quota = ConsortiumDAO.GetQuota(coConsortiumObj.IndividualId, currencyCode, prefixCode, issueDate);


                    System.Collections.ArrayList infoCumulus = ConsortiumDAO.GetCumulusDataSP(coConsortiumObj.IndividualId);
                    cumulus = (decimal)((object[])infoCumulus[0])[0] + (decimal)((object[])infoCumulus[0])[1] - (decimal)((object[])infoCumulus[0])[2];

                    decimal participationRate = System.Convert.ToDecimal(coConsortiumObj.ParticipationRate.ToString());
                    available = ((quota - cumulus) * participationRate) / 100;
                    availableTotal = availableTotal + available;
                }
                else
                {
                    Insured insuredObj = (Insured)coConsortium;
                    quota = ConsortiumDAO.GetQuota(insuredObj.IndividualId, currencyCode, prefixCode, issueDate);

                    System.Collections.ArrayList infoCumulus = ConsortiumDAO.GetCumulusDataSP(System.Convert.ToInt32(individualId));
                    cumulus = cumulus +  (decimal)((object[])infoCumulus[0])[0] + (decimal)((object[])infoCumulus[0])[1] - (decimal)((object[])infoCumulus[0])[2];


                    if (IsConsortiumindividualIdR1(System.Convert.ToInt32(individualId)))
                    {
                        if (consorcio != null && consorcio.Count > 0)
                        {
                            foreach (CoConsortium cco in consorcio)
                            {
                                Insured lstInsured = (from p in consorcio.Cast<Insured>().ToList()
                                                      where p.InsuredCode == cco.InsuredCode
                                                      select p).FirstOrDefault();

                                if (lstInsured != null)
                                {
                                    Dictionary<string, object> ParamListCo = new Dictionary<string, object>();
                                    ParamListCo.Add("INDIVIDUAL_ID", lstInsured.IndividualId);
                                    System.Collections.IList  AggreateCo = ListIndividualConsorcio(lstInsured.IndividualId);
                                    if (AggreateCo != null)
                                    {
                                        cumulus = cumulus + (((decimal)((object[])AggreateCo[0])[0]) * cco.ParticipationRate) / 100 + (((decimal)((object[])AggreateCo[0])[1]) * cco.ParticipationRate) / 100 - (((decimal)((object[])AggreateCo[0])[2]) * cco.ParticipationRate) / 100;
                                    }
                                    else
                                    {
                                        cumulus = 0;
                                    }
                                }
                            }

                        }

                        

                        
                    }
                    available = quota - cumulus;
                    availableTotal = availableTotal + available;
                    
                }
            }
            return availableTotal;
        }

        /// <summary>
        /// obtiene los participantes del consorcio
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public static System.Collections.IList ListIndividualConsorcio(int IndividualId)
        {

            bool consorcioTypeValue = false;
            System.Collections.IList consorcios;

            ConsorcioViewCoV1 request = new ConsorcioViewCoV1();
            ViewBuilder builder = new ViewBuilder("ConsorcioViewCoV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Insured.Properties.IndividualId);
            filter.Equal();
            filter.Constant(IndividualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, request);
            System.Collections.IList consorcio = request.CoAssociationTypeList;
            foreach (CoAssociationType consorcioType in consorcio)
            {
                consorcioTypeValue = consorcioType.IsConsortium;
            }

            
            InsuredViewV1 requests = new InsuredViewV1();
            ViewBuilder builders = new ViewBuilder("InsuredViewV1");
            ObjectCriteriaBuilder filters = new ObjectCriteriaBuilder();
            filters.Property(Insured.Properties.IndividualId, typeof(Insured).Name).Equal().Constant(IndividualId);

            builders.Filter = filters.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builders, requests);
            if (consorcioTypeValue == true)
            {
                
                int insuredId = (((entities.Insured)requests.Insureds[0]).InsuredCode) != null ?(((entities.Insured)requests.Insureds[0]).InsuredCode) : 0;

                ConsorcioViewCoV1 req = new ConsorcioViewCoV1();
                ViewBuilder builde = new ViewBuilder("ConsorcioViewV1");
                ObjectCriteriaBuilder obj = new ObjectCriteriaBuilder();
                obj.Property(CoConsortium.Properties.InsuredCode, typeof(CoConsortium).Name).Equal().Constant(insuredId);
                
                builde.Filter = obj.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builde, req);
                
                consorcios = req.CoConsortiumList;
              
            }
            else
            {
                consorcios = requests.Insureds;
            }
            return consorcios;
        }

        /// <summary>
        /// Obtiene  la cuota
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="currencyCode"></param>
        /// <param name="lineBusinessCode"></param>
        /// <param name="issueDate"></param>
        /// <returns></returns>
        public static decimal GetQuota(int insuredId, int currencyCode, int lineBusinessCode, System.DateTime issueDate)
        {
            System.Collections.IList consorcio = ConsortiumDAO.ListIndividualConsorcio(insuredId);
            decimal quota = 0;
            decimal operatingQuota = 0;
            decimal available = 0;
            decimal cumulus = 0;
            foreach (object coConsortium in consorcio)
            {

                if (!(coConsortium is Insured))
                {
                    
                    ObjectCriteriaBuilder filterQuota = new ObjectCriteriaBuilder();
                    filterQuota.Property(OperatingQuota.Properties.IndividualId);
                    filterQuota.Equal();
                    filterQuota.Constant(((CoConsortium)coConsortium).IndividualId);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrencyCode);
                    filterQuota.Equal();
                    filterQuota.Constant(currencyCode); 
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.LineBusinessCode);
                    filterQuota.Equal();
                    filterQuota.Constant(lineBusinessCode);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrentTo);
                    filterQuota.GreaterEqual();
                    filterQuota.Constant(issueDate);
                    OperatingQuota operatingQuotaInd = (OperatingQuota)DataFacadeManager.Instance.GetDataFacade().List(typeof(OperatingQuota), filterQuota.GetPredicate()).FirstOrDefault();

                    CoConsortium coConsortiumObj = (CoConsortium)coConsortium;
                    
                    if (operatingQuotaInd != null)
                    {
                        quota = (((OperatingQuota)operatingQuotaInd).OperatingQuotaAmount != null ? ((OperatingQuota)operatingQuotaInd).OperatingQuotaAmount : 0);// * coConsortiumObj.ParticipationRate))/100 : 0);


                        System.Collections.ArrayList infoCumulus = ConsortiumDAO.GetCumulusDataSP(System.Convert.ToInt32(coConsortiumObj.IndividualId));
                        cumulus = (decimal)((object[])infoCumulus[0])[0] + (decimal)((object[])infoCumulus[0])[1] - (decimal)((object[])infoCumulus[0])[2];
                        available = quota - cumulus;
                        operatingQuota = operatingQuota + available;
                    }
                    else
                    {
                        operatingQuota = operatingQuota + 0;
                    }

                }
                else
                {
                    Insured insuredObj = (Insured)coConsortium;
                    OperatingQuota request;
                    ObjectCriteriaBuilder filterQuota = new ObjectCriteriaBuilder();
                    filterQuota.Property(OperatingQuota.Properties.IndividualId);
                    filterQuota.Equal();
                    filterQuota.Constant(((Insured)coConsortium).IndividualId);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrencyCode);
                    filterQuota.Equal();
                    filterQuota.Constant(currencyCode); 
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.LineBusinessCode);
                    filterQuota.Equal();
                    filterQuota.Constant(lineBusinessCode);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrentTo);
                    filterQuota.GreaterEqual();
                    filterQuota.Constant(issueDate);
                    request = (OperatingQuota)DataFacadeManager.Instance.GetDataFacade().List(typeof(OperatingQuota), filterQuota.GetPredicate()).FirstOrDefault();
                 
                    if (request != null)
                    {
                        quota = (((OperatingQuota)request).OperatingQuotaAmount != null ? ((OperatingQuota)request).OperatingQuotaAmount : 0);
                        operatingQuota = operatingQuota + quota;
                    }
                    else
                    {
                        operatingQuota = operatingQuota + 0;

                    }
                }
            }

            return operatingQuota;
        }

        /// <summary>
        /// Calculo del cumulo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public static System.Collections.ArrayList GetCumulusDataSP(int individualId)
        {
            System.Collections.ArrayList response = new System.Collections.ArrayList();
        
            System.Data.DataTable result;
            Dictionary<string, object> ParamList = new Dictionary<string, object>();
            ParamList.Add("INDIVIDUAL_ID", individualId);


            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.GET_CALCULATE_CUMULUS", ConsortiumDAO.GetParameters(ParamList));
            }

            if (result != null && result.Rows.Count > 0)
            {
                 
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    response.Add((System.Object[])result.Rows[i].ItemArray);
                }
            }
            
            
            return response ;


            


        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="oldParams">The old parameters.</param>
        /// <returns></returns>
        private static  NameValue[] GetParameters(Dictionary<string, object> oldParams)
        {
            NameValue[] parameters = new NameValue[oldParams.Count];

            int index = 0;
            foreach (KeyValuePair<string, object> nm in oldParams)
            {
                NameValue p = new NameValue();
                p.Name = nm.Key;
                p.Value = nm.Value;
                parameters[index] = p;
                index++;
            }
            return parameters;
        }

        #endregion 
    }
}
