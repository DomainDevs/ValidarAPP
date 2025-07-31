using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Diagnostics;
using System.ServiceModel;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.Queries;
using System.Linq;
using System.Data;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Clase que implementa la interfaz IUnderwritingParamServiceWeb.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class EconomicGroupDAO
    {
        /// <summary>
        /// Obtener Tipos de Direccion
        /// </summary>
        /// <returns></returns>
        public List<EconomicGroup> GetGroupEconomicId(int Id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EconomicGroup)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetGroupEconomicId");
            return ModelAssembler.CreateEconomicGroups(businessCollection);
        }

        /// <summary>
        /// Obtener Tipos de Direccion
        /// </summary>
        /// <returns></returns>
        public List<EconomicGroup> GetGroupEconomicById(int Id)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.EconomicGroup.Properties.EconomicGroupId, typeof(EconomicGroup).Name, Id);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UPEN.EconomicGroup>(filter.GetPredicate());
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetGroupEconomic");
            return ModelAssembler.CreateEconomicGroups(businessCollection);
        }

        public List<EconomicGroup> GetEconomicGroup(string groupName, string documentNo, bool? enabled)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if(groupName != null && documentNo != null && enabled != null)
            {
                filter.Property(UPEN.EconomicGroup.Properties.EconomicGroupName, typeof(EconomicGroup).Name);
                filter.Like();
                filter.Constant("%" + groupName + "%");
                filter.And();
                filter.Property(UPEN.EconomicGroup.Properties.TrubutaryIdNo, typeof(EconomicGroup).Name);
                filter.Like();
                filter.Constant("%" + documentNo + "%");
                filter.And();
                filter.Property(UPEN.EconomicGroup.Properties.EnabledInd, typeof(EconomicGroup).Name);
                filter.Equal();
                filter.Constant(enabled);
            }
            else if(groupName != null && enabled != null)
            {
                filter.Property(UPEN.EconomicGroup.Properties.EconomicGroupName, typeof(EconomicGroup).Name);
                filter.Like();
                filter.Constant("%" + groupName + "%");
                filter.And();
                filter.Property(UPEN.EconomicGroup.Properties.EnabledInd, typeof(EconomicGroup).Name);
                filter.Equal();
                filter.Constant(enabled);
            }
            else if (documentNo != null && enabled != null)
            {
                filter.Property(UPEN.EconomicGroup.Properties.TrubutaryIdNo, typeof(EconomicGroup).Name);
                filter.Like();
                filter.Constant("%" + documentNo + "%");
                filter.And();
                filter.Property(UPEN.EconomicGroup.Properties.EnabledInd, typeof(EconomicGroup).Name);
                filter.Equal();
                filter.Constant(enabled);
            }
            else if (enabled != null)
            {
                filter.Property(UPEN.EconomicGroup.Properties.EnabledInd, typeof(EconomicGroup).Name);
                filter.Equal();
                filter.Constant(enabled);
            }
            else if(enabled == null)
            {
                filter.Property(UPEN.EconomicGroup.Properties.EconomicGroupName, typeof(EconomicGroup).Name);
                filter.Like();
                filter.Constant(groupName == null || groupName == "" ? "null" : "%" + groupName + "%");
                filter.Or();
                filter.Property(UPEN.EconomicGroup.Properties.TrubutaryIdNo, typeof(EconomicGroup).Name);
                filter.Like();
                filter.Constant(documentNo == null || documentNo == "" ? "null" : "%" + documentNo + "%");
            }

            BusinessCollection collectionEconomicGroup = DataFacadeManager.Instance.GetDataFacade().List<UPEN.EconomicGroup>(filter.GetPredicate());
            if (collectionEconomicGroup == null)
            {
                return new List<EconomicGroup>();
            }
            else
            {
                List<EconomicGroup> listEconomicGroup = new List<EconomicGroup>();
                foreach (UPEN.EconomicGroup item in collectionEconomicGroup)
                {
                    EconomicGroup economicGroup = ModelAssembler.CreateEconomicGroup(item);
                    economicGroup.EconomicGroupDetails = GetEconomicGroupDetail(item.EconomicGroupId);
                    listEconomicGroup.Add(economicGroup);
                }
                return listEconomicGroup;
            }
        }

        public List<EconomicGroupDetail> GetEconomicGroupDetail(int EconomicGropId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.EconomicGroupDetail.Properties.EconomicGroupId, typeof(EconomicGroupDetail).Name);
            filter.Equal();
            filter.Constant(EconomicGropId);
            BusinessCollection entityEconomicGroup = DataFacadeManager.Instance.GetDataFacade().List<UPEN.EconomicGroupDetail>(filter.GetPredicate());
            if (entityEconomicGroup != null)
            {
                List<EconomicGroupDetail> economicGroup = ModelAssembler.CreateEconomicGroupDetails(entityEconomicGroup);
                return economicGroup;
            }
            else
            {
                return new List<EconomicGroupDetail>();
            }
        }

        public List<Insured> GetEconomicGroupInsureds(int EconomicGropId)
        {
            List<Insured> insureds = new List<Insured>();
            SelectQuery select = new SelectQuery();
            #region Select

            select.AddSelectValue(new SelectValue(new Column(UPEN.EconomicGroupDetail.Properties.EnabledInd, "egd"), "Enabled"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TradeName, "c"), "TradeName"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdTypeCode, "c"), "CTributaryIdTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdNo, "c"), "CTributaryIdNo"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.CompanyTypeCode, "c"), "CompanyTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.IndividualId, "c"), "CIndividualId"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, "p"), "Name"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, "p"), "Surname"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName, "p"), "MotherLastName"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IdCardTypeCode, "p"), "IdCardTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IdCardNo, "p"), "IdCardNo"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.PersonTypeCode, "p"), "PersonTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IndividualId, "p"), "PIndividualId"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.OperatingQuota.Properties.CurrentTo, "oq"), "EnteredDate"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.OperatingQuota.Properties.LineBusinessCode, "oq"), "BusinessLine"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.OperatingQuota.Properties.OperatingQuotaAmount, "oq"), "OperatingQuotaAmount"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Insured.Properties.DeclinedDate, "i"), "DeclinedDate"));

            #endregion

            Join join = new Join(new ClassNameTable(typeof(UPEN.EconomicGroupDetail), "egd"), new ClassNameTable(typeof(UPEN.Person), "p"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.EconomicGroupDetail.Properties.IndividualId, "egd")
                .Equal()
                .Property(UPEN.Person.Properties.IndividualId, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.Company), "c"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.EconomicGroupDetail.Properties.IndividualId, "egd")
                .Equal()
                .Property(UPEN.Company.Properties.IndividualId, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.OperatingQuota), "oq"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.EconomicGroupDetail.Properties.IndividualId, "egd")
                .Equal()
                .Property(UPEN.OperatingQuota.Properties.IndividualId, "oq")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.Insured), "i"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.EconomicGroupDetail.Properties.IndividualId, "egd")
                .Equal()
                .Property(UPEN.OperatingQuota.Properties.IndividualId, "i")
                .GetPredicate());

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.EconomicGroupDetail.Properties.EconomicGroupId, "egd");
            filter.Equal();
            filter.Constant(EconomicGropId);

            select.Table = join;
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Insured insured = new Insured();

                while (reader.Read())
                {
                    int typeId = reader["CTributaryIdTypeCode"] == null ? (int)reader["IdCardTypeCode"] : (int)reader["CTributaryIdTypeCode"];
                    insured = new Insured
                    {
                        FullName = reader["Name"] == null ? (string)reader["TradeName"] : 
                            (string)reader["Name"] + " " +(string)reader["Surname"] + " " + (string)reader["MotherLastName"],
                        IndividualId = reader["CIndividualId"] == null ? (int)reader["PIndividualId"] : (int)reader["CIndividualId"],
                        IdentificationDocument = reader["CTributaryIdNo"] == null ? (string)reader["IdCardNo"] : (string)reader["CTributaryIdNo"],
                        EnteredDate = reader["EnteredDate"] == null ? new DateTime() : (DateTime)reader["EnteredDate"],
                        IndividualType = reader["Name"] == null ? IndividualType.Company : IndividualType.Person,
                        CustomerType = CustomerType.Individual,
                        DeclinedDate = reader["DeclinedDate"] == null ? new DateTime() : (DateTime)reader["DeclinedDate"]
                    };
                    insured.SetExtendedProperty("OperatingQuotaAmount", (decimal)reader["OperatingQuotaAmount"]);
                    insured.SetExtendedProperty("BusinessLine", (int)reader["BusinessLine"]);
                    insured.SetExtendedProperty("Enabled", (bool)reader["Enabled"]);
                    insured.IdentificationDocument += "_" + typeId; 

                    insureds.Add(insured);
                }
            }

            return insureds;
        }

        public EconomicGroup CreateEconomicGroup(EconomicGroup economicGroup, List<EconomicGroupDetail> listEconomicGroupDetail)
        {

            UPEN.EconomicGroup entityEconomicGroup = null;
            if (economicGroup.EconomicGroupId > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.EconomicGroup.Properties.EconomicGroupId, typeof(EconomicGroup).Name);
                filter.Equal();
                filter.Constant(economicGroup.EconomicGroupId);

                entityEconomicGroup = (UPEN.EconomicGroup)DataFacadeManager.Instance.GetDataFacade().List(typeof(UPEN.EconomicGroup), filter.GetPredicate()).FirstOrDefault();
                if (entityEconomicGroup != null)
                {
                    entityEconomicGroup.EnteredDate = economicGroup.EnteredDate == DateTime.MinValue ? new DateTime(1900,01,01) : economicGroup.EnteredDate;
                    entityEconomicGroup.OperatingQuotaAmount = economicGroup.OperationQuoteAmount;
                    entityEconomicGroup.DeclinedDate = economicGroup.DeclinedDate;
                    entityEconomicGroup.EconomicGroupName = economicGroup.EconomicGroupName;
                    entityEconomicGroup.EnabledInd = economicGroup.Enabled;
                    entityEconomicGroup.TributaryIdTypeCode = economicGroup.TributaryIdType;
                    entityEconomicGroup.VerifyDigit = economicGroup.VerifyDigit;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityEconomicGroup);
                }
            }

            if (entityEconomicGroup == null)
            {
                economicGroup.EconomicGroupId = GetIdEconomicGroup();
                economicGroup.TributaryIdNo = GetTributatyId().ToString();
                PrimaryKey key = UPEN.EconomicGroup.CreatePrimaryKey(economicGroup.EconomicGroupId);
                entityEconomicGroup = EntityAssembler.CreateEconomicGroup(economicGroup);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityEconomicGroup);
            }

            if (entityEconomicGroup != null && entityEconomicGroup.EconomicGroupId > 0 && listEconomicGroupDetail?.Count() > 0)
            {
                List<EconomicGroupDetail> listEconomicGroup = new List<EconomicGroupDetail>();
                listEconomicGroupDetail.ForEach(x =>
                {
                    x.EconomicGroupId = entityEconomicGroup.EconomicGroupId;
                    CreateEconomicGroupDetail(x);
                    listEconomicGroup.Add(x);
                });
                economicGroup.EconomicGroupDetails = listEconomicGroup;
            }
            return economicGroup;
        }

        /// <summary>
        /// Obtiene el ID maximo para realizar el ingreso del nuevos Perfil de Asegurado.
        /// </summary>
        /// <returns>ID maximo</returns>
        public int GetIdEconomicGroup()
        {
            int maxIdEconomicGroup = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.EconomicGroup))).Cast<UPEN.EconomicGroup>().Max(x => x.EconomicGroupId);
            maxIdEconomicGroup++;
            return maxIdEconomicGroup;
        }

        /// <summary>
        /// Obtiene el ID maximo para realizar el ingreso del nuevos Perfil de Asegurado.
        /// </summary>
        /// <returns>ID maximo</returns>
        public int GetTributatyId()
        {
            IEnumerable<UPEN.EconomicGroup> maxIdEconomicGroup = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.EconomicGroup))).Cast<UPEN.EconomicGroup>();
            UPEN.EconomicGroup resutl = maxIdEconomicGroup.OrderByDescending(x => x.EconomicGroupId).FirstOrDefault();
            if (Int32.TryParse(resutl.TrubutaryIdNo, out int numValue))
            {
                return int.Parse(resutl.TrubutaryIdNo) + 1;
            }
            else
            {
                return resutl.EconomicGroupId + 1;
            }
        }

        /// <summary>
        /// Obtener Tipos de Direccion
        /// </summary>
        /// <returns></returns>
        public List<TributaryIdentityType> GetTributaryType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.TributaryIdentityType)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetTributaryType");
            return ModelAssembler.CreateTributaryTypes(businessCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EconomicGroupDetail CreateEconomicGroupDetail(EconomicGroupDetail economicGroupDetail)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.EconomicGroupDetail.Properties.IndividualId, typeof(UPEN.EconomicGroupDetail).Name);
            filter.Equal();
            filter.Constant(economicGroupDetail.IndividualId);
            filter.And();
            filter.Property(UPEN.EconomicGroupDetail.Properties.EconomicGroupId, typeof(UPEN.EconomicGroupDetail).Name);
            filter.Equal();
            filter.Constant(economicGroupDetail.EconomicGroupId);
            UPEN.EconomicGroupDetail entityGroupDetail = null;
            entityGroupDetail = (UPEN.EconomicGroupDetail)DataFacadeManager.Instance.GetDataFacade().List(typeof(UPEN.EconomicGroupDetail), filter.GetPredicate()).FirstOrDefault();

            if (entityGroupDetail != null)
            {
                entityGroupDetail.EnabledInd = economicGroupDetail.Enabled;
                if (economicGroupDetail.Enabled)
                {
                    entityGroupDetail.DeclinedDate = null;
                }
                else
                {
                    entityGroupDetail.DeclinedDate = economicGroupDetail.DeclinedDate;
                }
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityGroupDetail);
                return economicGroupDetail;
            }
            else
            {
                entityGroupDetail = EntityAssembler.CreateEconomicGroupDetail(economicGroupDetail);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateEconomicGroupDetail(economicGroupDetail));
                return economicGroupDetail;
            }
        }

        public EconomicGroupDetail GetExistIndividdualByIndividualId(int IndividualId)
        {
            EconomicGroupDetail economicGroupDetail = new EconomicGroupDetail();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.EconomicGroupDetail.Properties.IndividualId, typeof(UPEN.EconomicGroupDetail).Name, IndividualId);
            filter.And();
            filter.PropertyEquals(UPEN.EconomicGroupDetail.Properties.EnabledInd, typeof(UPEN.EconomicGroupDetail).Name, 1);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.EconomicGroupDetail), filter.GetPredicate());
            List<EconomicGroupDetail> economicGroupDetails = ModelAssembler.CreateEconomicGroupDetails(businessObjects);
            if(economicGroupDetails.Count > 0)
            {
                economicGroupDetail = economicGroupDetails.Last();
            }
            return economicGroupDetail;
        }


        public List<EconomicGroupDetail> GetEconomicGroupDetailByIndividual(int IndividualId)
        {
            EconomicGroupDetail economicGroupDetail = new EconomicGroupDetail();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.EconomicGroupDetail.Properties.IndividualId, typeof(UPEN.EconomicGroupDetail).Name, IndividualId);
            filter.And();
            filter.PropertyEquals(UPEN.EconomicGroupDetail.Properties.EnabledInd, typeof(UPEN.EconomicGroupDetail).Name, 1);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.EconomicGroupDetail), filter.GetPredicate());
            List<EconomicGroupDetail> economicGroupDetails = ModelAssembler.CreateEconomicGroupDetails(businessObjects);
            
            return economicGroupDetails;
        }

    }
}
