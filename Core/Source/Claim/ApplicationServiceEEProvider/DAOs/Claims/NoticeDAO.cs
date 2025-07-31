using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.VehicleServices.DTOs;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class NoticeDAO
    {
        public List<Notice> SearchNotices(SearchClaimNotice searchClaimNotice)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            ClaimDAO claimDAO = new ClaimDAO();

            bool emptyFilter = true;
            bool specificFilter = false;

            List<Notice> notices = new List<Notice>();
            List<Dictionary<string, dynamic>> dictionaryNotices = new List<Dictionary<string, dynamic>>();


            #region Filters

            if (searchClaimNotice.PrefixId != null && searchClaimNotice.PrefixId > 0)
            {
                specificFilter = true;
                int coveredRiskType = claimDAO.GetClaimPrefixCoveredRiskTypeByPrefixCode(Convert.ToInt32(searchClaimNotice.PrefixId));

                switch ((CoveredRiskType)coveredRiskType)
                {
                    case CoveredRiskType.Aeronavigation:
                        filter.Property(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name);
                        filter.Equal();
                        filter.Constant(coveredRiskType);
                        emptyFilter = false;
                        specificFilter = false;
                        break;
                    case CoveredRiskType.Location:
                        if (searchClaimNotice.RiskId > 0)
                        {
                            filter.Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name);
                            filter.Equal();
                            filter.Constant(searchClaimNotice.RiskId);
                        }
                        else
                        {
                            filter.Property(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name);
                            filter.Equal();
                            filter.Constant(coveredRiskType);
                            emptyFilter = false;
                            specificFilter = false;

                            if (searchClaimNotice.Address != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name);
                                filter.Like();
                                filter.Constant(searchClaimNotice.Address + "%");
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.CityId != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskLocation.Properties.CityCode, typeof(ISSEN.RiskLocation).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.CityId);
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.StateId != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskLocation.Properties.StateCode, typeof(ISSEN.RiskLocation).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.StateId);
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.CountryId != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskLocation.Properties.CountryCode, typeof(ISSEN.RiskLocation).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.CountryId);
                                emptyFilter = false;
                            }
                        }
                        break;
                    case CoveredRiskType.Surety:
                        if (searchClaimNotice.RiskId > 0)
                        {
                            filter.Property(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name);
                            filter.Equal();
                            filter.Constant(searchClaimNotice.RiskId);
                        }
                        else
                        {
                            filter.Property(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name);
                            filter.Equal();
                            filter.Constant(coveredRiskType);
                            emptyFilter = false;
                            specificFilter = false;

                            if (searchClaimNotice.BidNumber != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskSurety.Properties.BidNumber, typeof(ISSEN.RiskSurety).Name);
                                filter.Like();
                                filter.Constant(searchClaimNotice.BidNumber + "%");
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.CourtNumber != null)
                            {
                                //if (!emptyFilter)
                                //    filter.And();
                                //filter.Property(ISSEN.RiskSurety.Properties., typeof(ISSEN.RiskSurety).Name);
                                //filter.Like();
                                //filter.Constant(searchClaimNotice.BidNumber + "%");
                                //emptyFilter = false;
                            }
                        }
                        break;
                    case CoveredRiskType.Transport:
                        filter.Property(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name);
                        filter.Equal();
                        filter.Constant(coveredRiskType);
                        emptyFilter = false;
                        specificFilter = false;
                        break;
                    case CoveredRiskType.Vehicle:
                        if (searchClaimNotice.RiskId > 0)
                        {
                            filter.Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name);
                            filter.Equal();
                            filter.Constant(searchClaimNotice.RiskId);
                        }
                        else
                        {
                            filter.Property(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name);
                            filter.Equal();
                            filter.Constant(coveredRiskType);
                            emptyFilter = false;
                            specificFilter = false;

                            if (searchClaimNotice.LicensePlate != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
                                filter.Like();
                                filter.Constant(searchClaimNotice.LicensePlate + "%");
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.VehicleMakeId != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskVehicle.Properties.VehicleMakeCode, typeof(ISSEN.RiskVehicle).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.VehicleMakeId);
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.VehicleModelId != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskVehicle.Properties.VehicleModelCode, typeof(ISSEN.RiskVehicle).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.VehicleModelId);
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.VehicleVersionId != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskVehicle.Properties.VehicleVersionCode, typeof(ISSEN.RiskVehicle).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.VehicleVersionId);
                                emptyFilter = false;
                            }

                            if (searchClaimNotice.VehicleYear != null)
                            {
                                if (!emptyFilter)
                                    filter.And();
                                filter.Property(ISSEN.RiskVehicle.Properties.VehicleYear, typeof(ISSEN.RiskVehicle).Name);
                                filter.Equal();
                                filter.Constant(searchClaimNotice.VehicleYear);
                                emptyFilter = false;
                            }
                        }
                        break;
                    default:
                        filter.Property(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name);
                        filter.Equal();
                        filter.Constant(coveredRiskType);
                        emptyFilter = false;
                        specificFilter = false;
                        break;
                }

                if (!emptyFilter)
                    filter.And();
                filter.OpenParenthesis();
                filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(searchClaimNotice.PrefixId);
                filter.Or();
                filter.Property(CLMEN.ClaimNotice.Properties.PolicyId, typeof(CLMEN.ClaimNotice).Name);
                filter.IsNull();
                filter.CloseParenthesis();
            }

            if (!specificFilter)
            {
                if (searchClaimNotice.NoticeNumber != null && searchClaimNotice.NoticeNumber > 0)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.PropertyEquals(CLMEN.ClaimNotice.Properties.Number, typeof(CLMEN.ClaimNotice).Name, searchClaimNotice.NoticeNumber);
                    emptyFilter = false;
                }

                if (searchClaimNotice.BranchId != null)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
                    filter.Equal();
                    filter.Constant(searchClaimNotice.BranchId);
                    emptyFilter = false;
                }

                if (searchClaimNotice.DocumentNumber != null && searchClaimNotice.DocumentNumber != "")
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                    filter.Equal();
                    filter.Constant(searchClaimNotice.DocumentNumber);
                    emptyFilter = false;
                }

                if (searchClaimNotice.HolderId != null)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(CLMEN.ClaimNotice.Properties.IndividualId, typeof(CLMEN.ClaimNotice).Name);
                    filter.Equal();
                    filter.Constant(searchClaimNotice.HolderId);
                    emptyFilter = false;
                }                

                if (searchClaimNotice.DateNoticeFrom != null && searchClaimNotice.DateNoticeTo != null)
                {                                        
                    if (searchClaimNotice.DateNoticeFrom > DateTime.MinValue)
                    {                        
                        if (!emptyFilter)
                            filter.And();

                        filter.Property(CLMEN.ClaimNotice.Properties.ClaimNoticeDate, typeof(CLMEN.ClaimNotice).Name);
                        filter.GreaterEqual();
                        filter.Constant(searchClaimNotice.DateNoticeFrom);
                        emptyFilter = false;
                    }

                    if (searchClaimNotice.DateNoticeTo > DateTime.MinValue)
                    {
                        searchClaimNotice.DateNoticeTo = new DateTime(Convert.ToDateTime(searchClaimNotice.DateNoticeTo).Year, Convert.ToDateTime(searchClaimNotice.DateNoticeTo).Month, Convert.ToDateTime(searchClaimNotice.DateNoticeTo).Day, 23, 59, 59);

                        if (!emptyFilter)
                            filter.And();

                        filter.Property(CLMEN.ClaimNotice.Properties.ClaimNoticeDate, typeof(CLMEN.ClaimNotice).Name);
                        filter.LessEqual();
                        filter.Constant(searchClaimNotice.DateNoticeTo);
                        emptyFilter = false;
                    }
                }

                if (searchClaimNotice.DateOcurrenceFrom != null && searchClaimNotice.DateOcurrenceTo != null)
                {
                    if (searchClaimNotice.DateOcurrenceFrom > DateTime.MinValue)
                    {                       
                        
                        if (!emptyFilter)
                            filter.And();

                        filter.Property(CLMEN.ClaimNotice.Properties.ClaimDate, typeof(CLMEN.ClaimNotice).Name);
                        filter.GreaterEqual();
                        filter.Constant(searchClaimNotice.DateOcurrenceFrom);
                        emptyFilter = false;
                    }

                    if (searchClaimNotice.DateOcurrenceTo > DateTime.MinValue)
                    {
                        searchClaimNotice.DateOcurrenceTo = Convert.ToDateTime(searchClaimNotice.DateOcurrenceTo).Add(new TimeSpan(23, 59, 59));

                        if (!emptyFilter)
                            filter.And();

                        filter.Property(CLMEN.ClaimNotice.Properties.ClaimDate, typeof(CLMEN.ClaimNotice).Name);
                        filter.LessEqual();
                        filter.Constant(searchClaimNotice.DateOcurrenceTo);
                        emptyFilter = false;
                    }
                }

                if (searchClaimNotice.UserId != null && searchClaimNotice.UserId > 0)
                {
                    if (!emptyFilter)
                        filter.And();

                    filter.PropertyEquals(CLMEN.ClaimNotice.Properties.UserId, typeof(CLMEN.ClaimNotice).Name, searchClaimNotice.UserId);
                    emptyFilter = false;
                }
            }

            #endregion

            #region Selects

            SelectQuery selectQuery = new SelectQuery();

            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimNoticeCode, typeof(CLMEN.ClaimNotice).Name), "ClaimNoticeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimDate, typeof(CLMEN.ClaimNotice).Name), "ClaimDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimNoticeDate, typeof(CLMEN.ClaimNotice).Name), "ClaimNoticeDate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.Location, typeof(CLMEN.ClaimNotice).Name), "Location"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.CityCode, typeof(CLMEN.ClaimNotice).Name), "CityCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.StateCode, typeof(CLMEN.ClaimNotice).Name), "StateCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.CountryCode, typeof(CLMEN.ClaimNotice).Name), "CountryCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.Description, typeof(CLMEN.ClaimNotice).Name), "Description"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ObjectedDescription, typeof(CLMEN.ClaimNotice).Name), "ObjectedDescription"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.UserId, typeof(CLMEN.ClaimNotice).Name), "UserId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.CoveredRiskTypeCode, typeof(CLMEN.ClaimNotice).Name), "CoveredRiskTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimNoticeTypeId, typeof(CLMEN.ClaimNotice).Name), "ClaimNoticeTypeId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name), "RiskId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimDamageResponsibilityCode, typeof(CLMEN.ClaimNotice).Name), "ClaimDamageResponsibilityCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimDamageTypeCode, typeof(CLMEN.ClaimNotice).Name), "ClaimDamageTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.EndorsementId, typeof(CLMEN.ClaimNotice).Name), "EndorsementId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.PolicyId, typeof(CLMEN.ClaimNotice).Name), "PolicyId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.Longitude, typeof(CLMEN.ClaimNotice).Name), "Longitude"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.Latitude, typeof(CLMEN.ClaimNotice).Name), "Latitude"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimNoticeReasonCode, typeof(CLMEN.ClaimNotice).Name), "ClaimNoticeReasonCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.Number, typeof(CLMEN.ClaimNotice).Name), "Number"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.NumberObjected, typeof(CLMEN.ClaimNotice).Name), "NumberObjected"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.IndividualId, typeof(CLMEN.ClaimNotice).Name), "IndividualId"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.OthersAffected, typeof(CLMEN.ClaimNotice).Name), "OthersAffected"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimedAmount, typeof(CLMEN.ClaimNotice).Name), "ClaimedAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimReasonOthers, typeof(CLMEN.ClaimNotice).Name), "ClaimReasonOthers"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.ClaimNoticeStateCode, typeof(CLMEN.ClaimNotice).Name), "ClaimNoticeStateCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNotice.Properties.InternalConsecutive, typeof(CLMEN.ClaimNotice).Name), "InternalConsecutive"));

            //Notice Contact Information
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNoticeContactInformation.Properties.Name, typeof(CLMEN.ClaimNoticeContactInformation).Name), "Name"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNoticeContactInformation.Properties.Phone, typeof(CLMEN.ClaimNoticeContactInformation).Name), "Phone"));
            selectQuery.AddSelectValue(new SelectValue(new Column(CLMEN.ClaimNoticeContactInformation.Properties.Mail, typeof(CLMEN.ClaimNoticeContactInformation).Name), "Mail"));

            //Policy
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name), "PrefixCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name), "BranchCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name), "DocumentNumber"));

            //Notice State
            selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.ClaimNoticeState.Properties.Description, typeof(PARAMEN.ClaimNoticeState).Name), "ClaimNoticeStateDescription"));

            #endregion

            #region Joins

            Join join = new Join(new ClassNameTable(typeof(CLMEN.ClaimNotice), typeof(CLMEN.ClaimNotice).Name), new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.ClaimNotice.Properties.PolicyId, typeof(CLMEN.ClaimNotice).Name)
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(CLMEN.ClaimNoticeContactInformation), typeof(CLMEN.ClaimNoticeContactInformation).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.ClaimNotice.Properties.ClaimNoticeCode, typeof(CLMEN.ClaimNotice).Name)
                .Equal()
                .Property(CLMEN.ClaimNoticeContactInformation.Properties.ClaimNoticeCode, typeof(CLMEN.ClaimNoticeContactInformation).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.ClaimNoticeType), typeof(PARAMEN.ClaimNoticeType).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.ClaimNotice.Properties.ClaimNoticeTypeId, typeof(CLMEN.ClaimNotice).Name)
                .Equal()
                .Property(PARAMEN.ClaimNoticeType.Properties.ClaimNoticeTypeId, typeof(PARAMEN.ClaimNoticeType).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.ClaimNoticeState), typeof(PARAMEN.ClaimNoticeState).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CLMEN.ClaimNotice.Properties.ClaimNoticeStateCode, typeof(CLMEN.ClaimNotice).Name)
                .Equal()
                .Property(PARAMEN.ClaimNoticeState.Properties.ClaimNoticeStateCode, typeof(PARAMEN.ClaimNoticeState).Name))
                .GetPredicate();

            if (searchClaimNotice.PrefixId != null)
            {
                switch ((SubCoveredRiskType)GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(Convert.ToInt32(searchClaimNotice.PrefixId), claimDAO.GetClaimPrefixCoveredRiskTypeByPrefixCode(Convert.ToInt32(searchClaimNotice.PrefixId))).SubCoveredRiskTypeCode)
                {
                    case SubCoveredRiskType.Vehicle:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))                            
                            .GetPredicate();                        
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        //selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name), "LicensePlate"));

                        //join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskVehicle), typeof(ISSEN.RiskVehicle).Name), JoinType.Inner);
                        //join.Criteria = (new ObjectCriteriaBuilder()
                        //    .PropertyCLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                        //    .Equal()
                        //    .Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name))
                        //    .GetPredicate();
                        break;
                    case SubCoveredRiskType.Property:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name), "Street"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskLocation), typeof(ISSEN.RiskLocation).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Liability:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name), "Street"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskLocation), typeof(ISSEN.RiskLocation).Name), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Surety:
                    case SubCoveredRiskType.Lease:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskSurety), typeof(ISSEN.RiskSurety).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskSurety.Properties.RiskId, typeof(ISSEN.RiskSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskJudicialSurety.Properties.InsuredId, typeof(ISSEN.RiskJudicialSurety).Name), "InsuranceId"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskJudicialSurety), typeof(ISSEN.RiskJudicialSurety).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskJudicialSurety.Properties.RiskId, typeof(ISSEN.RiskJudicialSurety).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Transport:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskTransport.Properties.TransportCargoTypeCode, typeof(ISSEN.RiskTransport).Name), "TransportCargoTypeCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskTransport), typeof(ISSEN.RiskTransport).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.RiskTransport).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Aircraft:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.RegisterNo, typeof(ISSEN.RiskAircraft).Name), "RegisterNo"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Marine:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftDescription, typeof(ISSEN.RiskAircraft).Name), "AircraftDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskAircraft.Properties.AircraftYear, typeof(ISSEN.RiskAircraft).Name), "AircraftYear"));


                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskAircraft), typeof(ISSEN.RiskAircraft).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskAircraft.Properties.RiskId, typeof(ISSEN.RiskAircraft).Name))
                            .GetPredicate();
                        break;
                    case SubCoveredRiskType.Fidelity:
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.Description, typeof(ISSEN.RiskFidelity).Name), "FidelityDescription"));
                        selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskFidelity.Properties.RiskCommercialClassCode, typeof(ISSEN.RiskFidelity).Name), "RiskCommercialClassCode"));

                        join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskFidelity), typeof(ISSEN.RiskFidelity).Name), JoinType.Left);
                        join.Criteria = (new ObjectCriteriaBuilder()
                            .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                            .Equal()
                            .Property(ISSEN.RiskFidelity.Properties.RiskId, typeof(ISSEN.RiskFidelity).Name))
                            .GetPredicate();                        
                        break;
                }

                join = new Join(join, new ClassNameTable(typeof(ISSEN.Risk), typeof(ISSEN.Risk).Name), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(CLMEN.ClaimNotice.Properties.RiskId, typeof(CLMEN.ClaimNotice).Name)
                    .Equal()
                    .Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name))
                    .GetPredicate();

                if (searchClaimNotice.IndividualId != null)
                {
                    if (!emptyFilter)
                        filter.And();
                    filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
                    filter.Equal();
                    filter.Constant(searchClaimNotice.IndividualId);
                    emptyFilter = false;
                }
            }

            #endregion

            selectQuery.Table = join;            
            selectQuery.Where = filter.GetPredicate();


            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {

                    Dictionary<string, dynamic> notice = new Dictionary<string, dynamic>();

                    notice.Add("ClaimNoticeCode", Convert.ToInt32(reader["ClaimNoticeCode"]));
                    notice.Add("ClaimDate", Convert.ToDateTime(reader["ClaimDate"]));
                    notice.Add("ClaimNoticeDate", Convert.ToDateTime(reader["ClaimNoticeDate"]));
                    notice.Add("Location", Convert.ToString(reader["Location"]));
                    notice.Add("CityCode", Convert.ToInt32(reader["CityCode"]));
                    notice.Add("StateCode", Convert.ToInt32(reader["StateCode"]));
                    notice.Add("CountryCode", Convert.ToInt32(reader["CountryCode"]));
                    notice.Add("Description", Convert.ToString(reader["Description"]));
                    notice.Add("ObjectedDescription", Convert.ToString(reader["ObjectedDescription"]));
                    notice.Add("UserId", Convert.ToInt32(reader["UserId"]));
                    notice.Add("CoveredRiskTypeCode", Convert.ToInt32(reader["CoveredRiskTypeCode"]));
                    notice.Add("ClaimNoticeTypeId", Convert.ToInt32(reader["ClaimNoticeTypeId"]));
                    notice.Add("RiskId", Convert.ToInt32(reader["RiskId"]));
                    notice.Add("ClaimDamageResponsibilityCode", Convert.ToInt32(reader["ClaimDamageResponsibilityCode"]));
                    notice.Add("ClaimDamageTypeCode", Convert.ToInt32(reader["ClaimDamageTypeCode"]));
                    notice.Add("EndorsementId", Convert.ToInt32(reader["EndorsementId"]));
                    notice.Add("PolicyId", Convert.ToInt32(reader["PolicyId"]));
                    notice.Add("DocumentNumber", Convert.ToString(reader["DocumentNumber"]));
                    notice.Add("Longitude", Convert.ToDecimal(reader["Longitude"]));
                    notice.Add("Latitude", Convert.ToDecimal(reader["Latitude"]));
                    notice.Add("Name", Convert.ToString(reader["Name"]));
                    notice.Add("Phone", Convert.ToString(reader["Phone"]));
                    notice.Add("Mail", Convert.ToString(reader["Mail"]));
                    notice.Add("ClaimNoticeReasonCode", Convert.ToInt32(reader["ClaimNoticeReasonCode"]));
                    notice.Add("ClaimNoticeStateCode", Convert.ToInt32(reader["ClaimNoticeStateCode"]));
                    notice.Add("ClaimNoticeStateDescription", Convert.ToString(reader["ClaimNoticeStateDescription"]));
                    notice.Add("Number", Convert.ToInt32(reader["Number"]));
                    notice.Add("NumberObjected", Convert.ToInt32(reader["NumberObjected"]));
                    notice.Add("IndividualId", Convert.ToInt32(reader["IndividualId"]));
                    notice.Add("OthersAffected", Convert.ToString(reader["OthersAffected"]));
                    notice.Add("ClaimedAmount", Convert.ToDecimal(reader["ClaimedAmount"]));
                    notice.Add("ClaimReasonOthers", Convert.ToString(reader["ClaimReasonOthers"]));
                    notice.Add("InternalConsecutive", Convert.ToString(reader["InternalConsecutive"]));

                    if (Convert.ToInt32(reader["PolicyId"]) > 0)
                    {
                        notice.Add("PrefixCode", Convert.ToInt32(reader["PrefixCode"]));
                        notice.Add("BranchCode", Convert.ToInt32(reader["BranchCode"]));
                    }

                    switch ((CoveredRiskType)Convert.ToInt32(reader["CoveredRiskTypeCode"]))
                    {
                        case CoveredRiskType.Vehicle:
                            if (Convert.ToInt32(reader["RiskId"]) > 0 && searchClaimNotice.PrefixId != null)
                            {
                                notice.Add("LicensePlate", Convert.ToString(reader["LicensePlate"]));
                            }
                            break;
                        case CoveredRiskType.Location:
                            if (Convert.ToInt32(reader["RiskId"]) > 0 && searchClaimNotice.PrefixId != null)
                            {
                                notice.Add("Street", Convert.ToString(reader["Street"]));

                            }
                            break;
                        case CoveredRiskType.Surety:
                            if (Convert.ToInt32(reader["RiskId"]) > 0 && searchClaimNotice.PrefixId != null)
                            {
                                if (Convert.ToInt32(reader["CityCode"]) == 0)
                                {
                                    notice.Add("InsuranceId", Convert.ToString(reader["InsuranceId"]));
                                }
                                else
                                {
                                    notice.Add("RiskCommercialClassCode", Convert.ToInt32(reader["RiskCommercialClassCode"]));
                                    notice.Add("FidelityDescription", Convert.ToString(reader["FidelityDescription"]));

                                }
                            }
                            break;
                        case CoveredRiskType.Transport:
                            if (Convert.ToInt32(reader["RiskId"]) > 0 && searchClaimNotice.PrefixId != null)
                            {
                                notice.Add("TransportCargoTypeCode", Convert.ToString(reader["TransportCargoTypeCode"]));
                            }
                            break;
                        case CoveredRiskType.Aircraft:
                            if (Convert.ToInt32(reader["RiskId"]) > 0 && searchClaimNotice.PrefixId != null)
                            {
                                notice.Add("AircraftYear", Convert.ToString(reader["AircraftYear"]));
                            }
                            break;
                    }

                    dictionaryNotices.Add(notice);
                }
            }

            foreach (Dictionary<string, dynamic> dictionaryNotice in dictionaryNotices)
            {
                Notice notice = new Notice
                {
                    Id = dictionaryNotice["ClaimNoticeCode"],
                    ClaimDate = dictionaryNotice["ClaimDate"],
                    CreationDate = dictionaryNotice["ClaimNoticeDate"],
                    Address = dictionaryNotice["Location"],
                    City = new City
                    {
                        Id = dictionaryNotice["CityCode"],
                        State = new State
                        {
                            Id = dictionaryNotice["StateCode"],
                            Country = new Country
                            {
                                Id = dictionaryNotice["CountryCode"]
                            }
                        }
                    },
                    Description = dictionaryNotice["Description"],
                    ObjectedReason = dictionaryNotice["ObjectedDescription"],
                    UserId = dictionaryNotice["UserId"],
                    CoveredRiskTypeId = dictionaryNotice["CoveredRiskTypeCode"],
                    Type = new NoticeType
                    {
                        Id = dictionaryNotice["ClaimNoticeTypeId"]
                    },
                    Risk = new Risk
                    {
                        Id = dictionaryNotice["RiskId"],
                        RiskId = dictionaryNotice["RiskId"]
                    },
                    DamageResponsability = new DamageResponsibility
                    {
                        Id = dictionaryNotice["ClaimDamageResponsibilityCode"]
                    },
                    DamageType = new DamageType
                    {
                        Id = dictionaryNotice["ClaimDamageTypeCode"]
                    },
                    Endorsement = new ClaimEndorsement
                    {
                        Id = dictionaryNotice["EndorsementId"]
                    },
                    Policy = new Policy
                    {
                        Id = dictionaryNotice["PolicyId"],
                        DocumentNumber = dictionaryNotice["DocumentNumber"]
                    },
                    Latitude = dictionaryNotice["Longitude"],
                    Longitude = dictionaryNotice["Latitude"],
                    ContactInformation = new ContactInformation
                    {
                        ClaimNoticeId = dictionaryNotice["ClaimNoticeCode"],
                        Name = dictionaryNotice["Name"],
                        Phone = dictionaryNotice["Phone"],
                        Mail = dictionaryNotice["Mail"]
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = dictionaryNotice["ClaimNoticeReasonCode"]
                    },
                    NoticeState = new NoticeState
                    {
                        Id = dictionaryNotice["ClaimNoticeStateCode"],
                        Description = dictionaryNotice["ClaimNoticeStateDescription"]
                    },
                    Number = dictionaryNotice["Number"],
                    NumberObjected = dictionaryNotice["NumberObjected"],
                    IndividualId = dictionaryNotice["IndividualId"],
                    OthersAffected = dictionaryNotice["OthersAffected"],
                    ClaimedAmount = dictionaryNotice["ClaimedAmount"],
                    ClaimReasonOthers = dictionaryNotice["ClaimReasonOthers"],
                    InternalConsecutive = dictionaryNotice["InternalConsecutive"],
                };

                if (notice.Policy.Id > 0)
                {
                    notice.Policy.PrefixId = dictionaryNotice["PrefixCode"];
                    notice.Policy.BranchId = dictionaryNotice["BranchCode"];
                    notice.Policy.DocumentNumber = dictionaryNotice["DocumentNumber"];
                }

                switch ((CoveredRiskType)notice.CoveredRiskTypeId)
                {
                    case CoveredRiskType.Vehicle:
                        if (notice.Risk.RiskId > 0)
                        {
                            if (searchClaimNotice.PrefixId != null)
                            {
                                notice.Risk.Description = dictionaryNotice["LicensePlate"];
                            }
                            else
                            {
                                PrimaryKey key = ISSEN.RiskVehicle.CreatePrimaryKey(notice.Risk.RiskId);
                                ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(key);

                                if (entityRiskVehicle != null)
                                {
                                    notice.Risk.Description = entityRiskVehicle.LicensePlate;
                                }
                                else
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                        }
                        else
                        {
                            notice.Risk.Description = GetRiskVehicleByClaimNoticeId(notice.Id).Plate;
                        }
                        break;
                    case CoveredRiskType.Location:
                        if (notice.Risk.RiskId > 0)
                        {
                            if (searchClaimNotice.PrefixId != null)
                            {
                                notice.Risk.Description = dictionaryNotice["Street"];
                            }
                            else
                            {
                                PrimaryKey key = ISSEN.RiskLocation.CreatePrimaryKey(notice.Risk.RiskId);
                                ISSEN.RiskLocation entityRiskLocation = (ISSEN.RiskLocation)DataFacadeManager.GetObject(key);

                                if (entityRiskLocation != null)
                                {
                                    notice.Risk.Description = entityRiskLocation.Street;
                                }
                                else
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                        }
                        else
                        {
                            notice.Risk.Description = GetRiskLocationByClaimNoticeId(notice.Id).Address;
                        }
                        break;
                    case CoveredRiskType.Surety:
                        if (notice.Risk.RiskId > 0)
                        {
                            if (notice.City.Id == 0) //Consideración hecha porque los avisos de fianza tienen el mismo tipo de riesgo cubierto que los avisos de manejo, pero, los avisos de fianza no guardan información de la ubicación
                            {
                                if (searchClaimNotice.PrefixId != null && dictionaryNotice["InsuranceId"] != "")
                                {
                                    string insuranceId = dictionaryNotice["InsuranceId"];
                                    InsuredDTO suretyIssuanceInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(insuranceId, InsuredSearchType.IndividualId, CustomerType.Individual).First();

                                    notice.Risk.Description = suretyIssuanceInsured.FullName;
                                }
                                else
                                {
                                    PrimaryKey key = ISSEN.RiskSurety.CreatePrimaryKey(notice.Risk.RiskId);
                                    ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.GetObject(key);

                                    if (entityRiskSurety == null)
                                    {
                                        key = ISSEN.RiskJudicialSurety.CreatePrimaryKey(notice.Risk.RiskId);
                                        ISSEN.RiskJudicialSurety entityRiskJudicialSurety = (ISSEN.RiskJudicialSurety)DataFacadeManager.GetObject(key);

                                        if (entityRiskJudicialSurety != null)
                                        {
                                            InsuredDTO judicialSuretyIssuanceInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskJudicialSurety.InsuredId), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                                            notice.Risk.Description = judicialSuretyIssuanceInsured.FullName;
                                        }
                                        else
                                        {
                                            notice.Risk.Description = Resources.Resources.RiskNotFound;
                                        }
                                    }
                                    else
                                    {
                                        InsuredDTO suretyIssuanceInsured = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(entityRiskSurety.IndividualId), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                                        notice.Risk.Description = suretyIssuanceInsured.FullName;
                                    }
                                }

                            }
                            else
                            {
                                PrimaryKey primaryKey = PARAMEN.RiskCommercialClass.CreatePrimaryKey(dictionaryNotice["RiskCommercialClassCode"]);

                                PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(primaryKey);

                                if (entityRiskCommercialClass != null)
                                {
                                    notice.Risk.Description = entityRiskCommercialClass.Description + " - " + dictionaryNotice["FidelityDescription"];
                                }
                                else
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                        }
                        else
                        {
                            NoticeSurety riskSurety = GetRiskSuretyByClaimNoticeId(notice.Id);
                            if (riskSurety != null)
                            {
                                notice.Risk.Description = riskSurety.Name;
                            }
                            else
                            {
                                try
                                {
                                    NoticeFidelity riskFidelity = GetRiskFidelityByClaimNoticeId(notice.Id);

                                    PrimaryKey primaryKey = PARAMEN.RiskCommercialClass.CreatePrimaryKey(riskFidelity.RiskCommercialClassId);

                                    PARAMEN.RiskCommercialClass entityRiskCommercialClass = (PARAMEN.RiskCommercialClass)DataFacadeManager.GetObject(primaryKey);
                                    if (entityRiskCommercialClass != null)
                                    {
                                        notice.Risk.Description = entityRiskCommercialClass.Description + " - " + riskFidelity.Description;
                                    }
                                    else
                                    {
                                        notice.Risk.Description = Resources.Resources.RiskNotFound;
                                    }
                                }
                                catch (Exception)
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                        }
                        break;
                    case CoveredRiskType.Transport:
                        if (notice.Risk.RiskId > 0)
                        {
                            if (searchClaimNotice.PrefixId != null)
                            {
                                PrimaryKey primaryKey = COMMEN.TransportCargoType.CreatePrimaryKey(dictionaryNotice["TransportCargoTypeCode"]);

                                COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(primaryKey);
                                if (entityTransportCargoType != null)
                                {
                                    notice.Risk.Description = entityTransportCargoType.Description;
                                }
                                else
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                            else
                            {
                                PrimaryKey key = ISSEN.RiskTransport.CreatePrimaryKey(notice.Risk.RiskId);
                                ISSEN.RiskTransport entityRiskTransport = (ISSEN.RiskTransport)DataFacadeManager.GetObject(key);

                                if (entityRiskTransport != null)
                                {
                                    key = COMMEN.TransportCargoType.CreatePrimaryKey(entityRiskTransport.TransportCargoTypeCode);
                                    COMMEN.TransportCargoType entityTransportCargoType = (COMMEN.TransportCargoType)DataFacadeManager.GetObject(key);

                                    notice.Risk.Description = entityTransportCargoType.Description;
                                }
                                else
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                        }
                        else
                        {
                            notice.Risk.Description = GetRiskTransportByClaimNoticeId(notice.Id).CargoType;
                        }
                        break;
                    case CoveredRiskType.Aircraft:
                        if (notice.Risk.RiskId > 0)
                        {
                            if (searchClaimNotice.PrefixId != null)
                            {
                                notice.Risk.Description = dictionaryNotice["RegisterNo"] + " - " + dictionaryNotice["AircraftYear"];
                            }
                            else
                            {
                                PrimaryKey key = ISSEN.RiskAircraft.CreatePrimaryKey(notice.Risk.RiskId);
                                ISSEN.RiskAircraft entityRiskAircraft = (ISSEN.RiskAircraft)DataFacadeManager.GetObject(key);

                                if (entityRiskAircraft != null)
                                {
                                    notice.Risk.Description = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear;
                                }
                                else
                                {
                                    notice.Risk.Description = Resources.Resources.RiskNotFound;
                                }
                            }
                        }
                        else
                        {
                            notice.Risk.Description = GetRiskAirCraftByClaimNoticeId(notice.Id).RegisterNumer;
                        }
                        break;
                }

                notices.Add(notice);
            }

            return notices;
        }

        public ContactInformation GetContactInformationByClaimNoticeCode(int claimNoticeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(CLMEN.ClaimNoticeContactInformation.Properties.ClaimNoticeCode, typeof(CLMEN.ClaimNoticeContactInformation).Name, claimNoticeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CLMEN.ClaimNoticeContactInformation), filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreateContactInformation(businessCollection.Cast<CLMEN.ClaimNoticeContactInformation>().ToList().First());
            }
            else
            {
                return null;
            }
        }

        public Notice CreateNotice(Notice notice)
        {
            if (notice.Id == 0)
            {
                notice.NoticeState.Id = (int)Enums.NoticeState.OPEN;
            }

            CLMEN.ClaimNotice entityClaimNotice = EntityAssembler.CreateClaimNotice(notice);

            if (entityClaimNotice.RiskId == 0)
            {
                entityClaimNotice.RiskId = null;
                entityClaimNotice.PolicyId = null;
                entityClaimNotice.EndorsementId = null;
            }

            if (entityClaimNotice.ClaimDamageResponsibilityCode == 0)
                entityClaimNotice.ClaimDamageResponsibilityCode = null;

            if (entityClaimNotice.ClaimDamageTypeCode == 0)
                entityClaimNotice.ClaimDamageTypeCode = null;

            DataFacadeManager.Insert(entityClaimNotice);
            int claimNoticeCode = entityClaimNotice.ClaimNoticeCode;

            CLMEN.ClaimNoticeDocumentation entityClaimNoticeDocumentation = new CLMEN.ClaimNoticeDocumentation(claimNoticeCode, 1); //notice.Documentation.DocumentId
            DataFacadeManager.Insert(entityClaimNoticeDocumentation);

            CLMEN.ClaimNoticeContactInformation entityClaimNoticeContactInformation = EntityAssembler.CreateClaimContactInformation(notice.ContactInformation, claimNoticeCode);
            DataFacadeManager.Insert(entityClaimNoticeContactInformation);

            if (notice.NoticeCoverages != null)
            {
                foreach (NoticeCoverage coverage in notice.NoticeCoverages.ToList())
                {
                    CLMEN.ClaimNoticeCoverage entityClaimNoticeCoverage = EntityAssembler.CreateClaimNoticeCoverage(coverage, claimNoticeCode);                   

                    DataFacadeManager.Insert(entityClaimNoticeCoverage);
                }
            }

            return ModelAssembler.CreateNotice(entityClaimNotice);
        }

        public Notice UpdateNotice(Notice notice)
        {
            CLMEN.ClaimNotice entityClaimNotice = EntityAssembler.CreateClaimNotice(notice);

            if (entityClaimNotice.RiskId == 0)
            {
                entityClaimNotice.RiskId = null;
                entityClaimNotice.PolicyId = null;
            }

            DataFacadeManager.Update(entityClaimNotice);
            int claimNoticeCode = entityClaimNotice.ClaimNoticeCode;

            CLMEN.ClaimNoticeContactInformation entityClaimNoticeContactInformation = EntityAssembler.CreateClaimContactInformation(notice.ContactInformation, claimNoticeCode);
            DataFacadeManager.Update(entityClaimNoticeContactInformation);
            ParticipantDAO participantDAO = new ParticipantDAO();

            if (notice.NoticeCoverages != null)
            {
                foreach (NoticeCoverage coverage in notice.NoticeCoverages)
                {
                    CLMEN.ClaimNoticeCoverage entityClaimNoticeCoverage = EntityAssembler.CreateClaimNoticeCoverage(coverage, claimNoticeCode);                                     
                    
                    PrimaryKey key = CLMEN.ClaimNoticeCoverage.CreatePrimaryKey(entityClaimNoticeCoverage.ClaimNoticeCode, entityClaimNoticeCoverage.CoverageId, entityClaimNoticeCoverage.IndividualId, entityClaimNoticeCoverage.EstimateTypeCode);
                    CLMEN.ClaimNoticeCoverage entityCoverageNotice = (CLMEN.ClaimNoticeCoverage)DataFacadeManager.GetObject(key);

                    if (entityCoverageNotice == null)
                    {
                        DataFacadeManager.Insert(entityClaimNoticeCoverage);
                    }
                    else
                    {
                        DataFacadeManager.Update(entityClaimNoticeCoverage);
                    }
                }
            }

            return ModelAssembler.CreateNotice(entityClaimNotice);
        }

        public void DeleteNoticeCoverage(int noticeId, int coverageId, int individualId, int estimateTypeId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimNoticeCoverage.CreatePrimaryKey(noticeId, coverageId, individualId, estimateTypeId);
            if (primaryKey != null)
            {
                DataFacadeManager.Delete(primaryKey);
            }
        }

        public Notice ObjectNotice(Notice notice)
        {
            PrimaryKey primaryKey = CLMEN.ClaimNotice.CreatePrimaryKey(notice.Id);
            CLMEN.ClaimNotice entityClaimNotice = (CLMEN.ClaimNotice)DataFacadeManager.GetObject(primaryKey);

            entityClaimNotice.ObjectedDescription = notice.ObjectedReason;
            entityClaimNotice.ClaimNoticeStateCode = (int)Enums.NoticeState.CLOSE;

            DataFacadeManager.Update(entityClaimNotice);

            return ModelAssembler.CreateNotice(entityClaimNotice);
        }

        public Notice GetNoticeByNoticeId(int noticeId)
        {
            Notice notice = new Notice();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.ClaimNotice.Properties.ClaimNoticeCode, typeof(CLMEN.ClaimNotice).Name);
            filter.Equal();
            filter.Constant(noticeId);

            ClaimNoticeView claimNoticeView = new ClaimNoticeView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimNoticeView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimNoticeView);

            if (claimNoticeView.ClaimNotices.Count > 0)
            {
                CLMEN.ClaimNotice entityClaimNotice = claimNoticeView.ClaimNotices.Cast<CLMEN.ClaimNotice>().FirstOrDefault();
                List<CLMEN.ClaimNoticeCoverage> entityClaimNoticeCoverages = claimNoticeView.ClaimNoticeCoverages.Cast<CLMEN.ClaimNoticeCoverage>().ToList();
                CLMEN.ClaimNoticeDocumentation entityClaimNoticeDocumentation = claimNoticeView.ClaimNoticeDocumentations.Cast<CLMEN.ClaimNoticeDocumentation>().FirstOrDefault();
                CLMEN.ClaimNoticeContactInformation entityClaimNoticeContactInformation = claimNoticeView.ClaimNoticeContactInformations.Cast<CLMEN.ClaimNoticeContactInformation>().FirstOrDefault();

                notice = ModelAssembler.CreateNotice(entityClaimNotice);
                notice.ContactInformation = ModelAssembler.CreateContactInformation(entityClaimNoticeContactInformation);
                notice.NoticeCoverages = ModelAssembler.NoticeCoverages(entityClaimNoticeCoverages);
                notice.Documentation = null;

                if (Convert.ToInt32(notice.Risk.RiskId) > 0)
                {
                    filter.Clear();

                    filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name).Equal();
                    filter.Constant(notice.Policy.Id).And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name).Equal();
                    filter.Constant(notice.Endorsement.Id).And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name).Equal();
                    filter.Constant(notice.Risk.RiskId);

                    string[] sortColumn = new string[1];
                    sortColumn[0] = "-" + ISSEN.EndorsementRisk.Properties.RiskId;

                    ISSEN.EndorsementRisk entityEndorsementRisk = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.EndorsementRisk), filter.GetPredicate(), sortColumn, 1)).Cast<ISSEN.EndorsementRisk>().FirstOrDefault();
                    notice.Risk.Number = entityEndorsementRisk.RiskNum;
                }

            }

            return notice;
        }

        #region Surety
        public NoticeSurety CreateNoticeSurety(NoticeSurety noticeSurety)
        {
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(2215);

            noticeSurety.Notice.Number = Convert.ToInt32(parameter.NumberParameter);

            parameter.NumberParameter += 1;

            DelegateService.commonServiceCore.UpdateParameter(parameter);


            Notice notice = CreateNotice(noticeSurety.Notice);
            noticeSurety.Notice = notice;

            CLMEN.ClaimNoticeRiskSurety entityNoticeSurety = EntityAssembler.CreateClaimNoticeSurety(noticeSurety, noticeSurety.Notice.Id);

            if (noticeSurety.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Insert(entityNoticeSurety);
            }

            return ModelAssembler.CreateNoticeSurety(entityNoticeSurety, notice);
        }

        public NoticeSurety UpdateNoticeSurety(NoticeSurety noticeSurety)
        {
            Notice notice = UpdateNotice(noticeSurety.Notice);
            noticeSurety.Notice = notice;

            CLMEN.ClaimNoticeRiskSurety entityClaimNoticeSurety = EntityAssembler.CreateClaimNoticeSurety(noticeSurety, noticeSurety.Notice.Id);

            if (noticeSurety.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Update(entityClaimNoticeSurety);
            }

            return ModelAssembler.CreateNoticeSurety(entityClaimNoticeSurety, notice);
        }

        public NoticeSurety GetNoticeSuretyByNoticeId(int noticeId)
        {
            PrimaryKey key = CLMEN.ClaimNoticeRiskSurety.CreatePrimaryKey(noticeId);

            CLMEN.ClaimNoticeRiskSurety entityClaimSurety = (CLMEN.ClaimNoticeRiskSurety)DataFacadeManager.GetObject(key);

            return ModelAssembler.CreateClaimNoticeSurety(entityClaimSurety);
        }

        public NoticeSurety GetRiskSuretyByClaimNoticeId(int claimNoticeId)
        {
            PrimaryKey key = CLMEN.ClaimNoticeRiskSurety.CreatePrimaryKey(claimNoticeId);
            CLMEN.ClaimNoticeRiskSurety entityClaimNoticeSurety = (CLMEN.ClaimNoticeRiskSurety)DataFacadeManager.GetObject(key);
            return ModelAssembler.CreateClaimNoticeSurety(entityClaimNoticeSurety);
        }
        #endregion

        #region Vehicle
        public NoticeVehicle CreateNoticeVehicle(NoticeVehicle noticeVehicle)
        {

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(2215);

            noticeVehicle.Notice.Number = Convert.ToInt32(parameter.NumberParameter);

            parameter.NumberParameter += 1;

            DelegateService.commonServiceCore.UpdateParameter(parameter);


            Notice notice = CreateNotice(noticeVehicle.Notice);
            noticeVehicle.Notice = notice;

            CLMEN.ClaimNoticeVehicle entityClaimNoticeVehicle = EntityAssembler.CreateNoticeVehicle(noticeVehicle, noticeVehicle.Notice.Id);

            if (noticeVehicle.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Insert(entityClaimNoticeVehicle);
            }

            return ModelAssembler.CreateNoticeRiskVehicle(entityClaimNoticeVehicle, notice);
        }

        public NoticeVehicle UpdateNoticeVehicle(NoticeVehicle noticeVehicle)
        {
            Notice notice = UpdateNotice(noticeVehicle.Notice);
            noticeVehicle.Notice = notice;

            CLMEN.ClaimNoticeVehicle entityClaimNoticeVehicle = EntityAssembler.CreateNoticeVehicle(noticeVehicle, noticeVehicle.Notice.Id);

            if (noticeVehicle.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Update(entityClaimNoticeVehicle);
            }


            return ModelAssembler.CreateNoticeRiskVehicle(entityClaimNoticeVehicle, notice);
        }

        public NoticeVehicle GetNoticeVehicleByNoticeId(int noticeId)
        {
            PrimaryKey key = CLMEN.ClaimNoticeVehicle.CreatePrimaryKey(noticeId);
            CLMEN.ClaimNoticeVehicle claimVehicle = (CLMEN.ClaimNoticeVehicle)DataFacadeManager.GetObject(key);
            return ModelAssembler.CreateNoticeVehicle(claimVehicle);
        }

        public VehicleDTO GetRiskVehicleByClaimNoticeId(int claimNoticeId)
        {
            PrimaryKey key = CLMEN.ClaimNoticeVehicle.CreatePrimaryKey(claimNoticeId);
            CLMEN.ClaimNoticeVehicle entityClaimNoticeVehicle = (CLMEN.ClaimNoticeVehicle)DataFacadeManager.GetObject(key);
            return ModelAssembler.CreateClaimNoticeRiskVehicle(entityClaimNoticeVehicle);
        }

        #endregion

        #region Location
        public NoticeLocation CreateNoticeLocation(NoticeLocation noticeLocation)
        {
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(2215);

            noticeLocation.Notice.Number = Convert.ToInt32(parameter.NumberParameter);

            parameter.NumberParameter += 1;

            DelegateService.commonServiceCore.UpdateParameter(parameter);


            Notice notice = CreateNotice(noticeLocation.Notice);
            noticeLocation.Notice = notice;

            CLMEN.ClaimNoticeRiskLocation entityClaimNoticeLocation = EntityAssembler.CreateNoticeLocation(noticeLocation, noticeLocation.Notice.Id);

            if (noticeLocation.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Insert(entityClaimNoticeLocation);
            }

            return ModelAssembler.CreateNoticeRiskLocation(entityClaimNoticeLocation, notice);
        }

        public NoticeLocation UpdateNoticeLocation(NoticeLocation noticeLocation)
        {
            Notice notice = UpdateNotice(noticeLocation.Notice);
            noticeLocation.Notice = notice;

            CLMEN.ClaimNoticeRiskLocation entityClaimNoticeLocation = EntityAssembler.CreateNoticeLocation(noticeLocation, noticeLocation.Notice.Id);

            if (noticeLocation.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Update(entityClaimNoticeLocation);
            }

            return ModelAssembler.CreateNoticeRiskLocation(entityClaimNoticeLocation, notice);
        }

        public ClaimLocation GetRiskLocationByClaimNoticeId(int claimNoticeId)
        {
            PrimaryKey key = CLMEN.ClaimNoticeRiskLocation.CreatePrimaryKey(claimNoticeId);
            CLMEN.ClaimNoticeRiskLocation entityClaimRiskLocation = (CLMEN.ClaimNoticeRiskLocation)DataFacadeManager.GetObject(key);
            return ModelAssembler.CreateNoticeLocation(entityClaimRiskLocation);
        }
        #endregion

        #region Transport
        public NoticeTransport CreateNoticeTransport(NoticeTransport noticeTransport)
        {

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(2215);

            noticeTransport.Notice.Number = Convert.ToInt32(parameter.NumberParameter);

            parameter.NumberParameter += 1;

            DelegateService.commonServiceCore.UpdateParameter(parameter);


            Notice notice = CreateNotice(noticeTransport.Notice);
            noticeTransport.Notice = notice;

            CLMEN.ClaimNoticeRiskTransport entityClaimNoticeRiskTransport = EntityAssembler.CreateNoticeTransport(noticeTransport, noticeTransport.Notice.Id);

            if (noticeTransport.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Insert(entityClaimNoticeRiskTransport);
            }

            return ModelAssembler.CreateNoticeRiskTransport(entityClaimNoticeRiskTransport, notice);
        }

        public NoticeTransport UpdateNoticeTransport(NoticeTransport noticeTransport)
        {
            Notice notice = UpdateNotice(noticeTransport.Notice);
            noticeTransport.Notice = notice;

            CLMEN.ClaimNoticeRiskTransport entityClaimNoticeRiskTransport = EntityAssembler.CreateNoticeTransport(noticeTransport, noticeTransport.Notice.Id);

            if (noticeTransport.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Update(entityClaimNoticeRiskTransport);
            }

            return ModelAssembler.CreateNoticeRiskTransport(entityClaimNoticeRiskTransport, notice);
        }

        public NoticeTransport GetRiskTransportByClaimNoticeId(int claimNoticeId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimNoticeRiskTransport.CreatePrimaryKey(claimNoticeId);

            CLMEN.ClaimNoticeRiskTransport entityClaimNoticeRiskTransport = (CLMEN.ClaimNoticeRiskTransport)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateNoticeTransport(entityClaimNoticeRiskTransport);
        }
        #endregion

        #region AirCraft
        public NoticeAirCraft CreateNoticeAirCraft(NoticeAirCraft noticeAirCraft)
        {
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(2215);

            noticeAirCraft.Notice.Number = Convert.ToInt32(parameter.NumberParameter);

            parameter.NumberParameter += 1;

            DelegateService.commonServiceCore.UpdateParameter(parameter);


            Notice notice = CreateNotice(noticeAirCraft.Notice);
            noticeAirCraft.Notice = notice;

            CLMEN.ClaimNoticeRiskAircraft entityClaimNoticeRiskAircraft = EntityAssembler.CreateNoticeAirCraft(noticeAirCraft, noticeAirCraft.Notice.Id);

            if (noticeAirCraft.Notice.Risk.RiskId == 0)
            {
                entityClaimNoticeRiskAircraft.AircraftTypeCode = 1; //AVIACIÓN
                DataFacadeManager.Insert(entityClaimNoticeRiskAircraft);
            }

            return ModelAssembler.CreateNoticeRiskAirCraft(entityClaimNoticeRiskAircraft, notice);
        }

        public NoticeAirCraft UpdateNoticeAirCraft(NoticeAirCraft noticeAirCraft)
        {
            Notice notice = UpdateNotice(noticeAirCraft.Notice);
            noticeAirCraft.Notice = notice;

            CLMEN.ClaimNoticeRiskAircraft entityClaimNoticeRiskAircraft = EntityAssembler.CreateNoticeAirCraft(noticeAirCraft, noticeAirCraft.Notice.Id);

            if (noticeAirCraft.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Update(entityClaimNoticeRiskAircraft);
            }

            return ModelAssembler.CreateNoticeRiskAirCraft(entityClaimNoticeRiskAircraft, notice);
        }

        public NoticeAirCraft GetRiskAirCraftByClaimNoticeId(int claimNoticeId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimNoticeRiskAircraft.CreatePrimaryKey(claimNoticeId);

            CLMEN.ClaimNoticeRiskAircraft entityClaimNoticeRiskAircraft = (CLMEN.ClaimNoticeRiskAircraft)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateNoticeAirCraft(entityClaimNoticeRiskAircraft);
        }
        #endregion

        #region Fidelity

        public NoticeFidelity CreateNoticeFidelity(NoticeFidelity noticeFidelity)
        {
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(2215);

            noticeFidelity.Notice.Number = Convert.ToInt32(parameter.NumberParameter);

            parameter.NumberParameter += 1;

            DelegateService.commonServiceCore.UpdateParameter(parameter);


            Notice notice = CreateNotice(noticeFidelity.Notice);
            noticeFidelity.Notice = notice;

            CLMEN.ClaimNoticeRiskFidelity entityClaimNoticeRiskFidelity = EntityAssembler.CreateNoticeFidelity(noticeFidelity, noticeFidelity.Notice.Id);

            if (noticeFidelity.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Insert(entityClaimNoticeRiskFidelity);
            }

            return ModelAssembler.CreateNoticeRiskFidelity(entityClaimNoticeRiskFidelity, notice);
        }

        public NoticeFidelity UpdateNoticeFidelity(NoticeFidelity noticeFidelity)
        {
            Notice notice = UpdateNotice(noticeFidelity.Notice);
            noticeFidelity.Notice = notice;

            CLMEN.ClaimNoticeRiskFidelity entityClaimNoticeRiskFidelity = EntityAssembler.CreateNoticeFidelity(noticeFidelity, noticeFidelity.Notice.Id);

            if (noticeFidelity.Notice.Risk.RiskId == 0)
            {
                DataFacadeManager.Update(entityClaimNoticeRiskFidelity);
            }

            return ModelAssembler.CreateNoticeRiskFidelity(entityClaimNoticeRiskFidelity, notice);
        }

        public NoticeFidelity GetRiskFidelityByClaimNoticeId(int claimNoticeId)
        {
            PrimaryKey primaryKey = CLMEN.ClaimNoticeRiskFidelity.CreatePrimaryKey(claimNoticeId);

            CLMEN.ClaimNoticeRiskFidelity entityClaimNoticeRiskFidelity = (CLMEN.ClaimNoticeRiskFidelity)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateNoticeFidelity(entityClaimNoticeRiskFidelity);
        }

        #endregion

        public List<Notice> GetNoticesByPolicyId(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.ClaimNotice.Properties.PolicyId, typeof(CLMEN.ClaimNotice).Name);
            filter.Equal();
            filter.Constant(policyId);

            return ModelAssembler.CreateNotices(DataFacadeManager.GetObjects(typeof(CLMEN.ClaimNotice), filter.GetPredicate()));
        }

        public List<Notice> GetNoticesByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.ClaimNotice.Properties.IndividualId, typeof(CLMEN.ClaimNotice).Name);
            filter.Equal();
            filter.Constant(individualId);

            return ModelAssembler.CreateNotices(DataFacadeManager.GetObjects(typeof(CLMEN.ClaimNotice), filter.GetPredicate()));
        }

        private PARAMEN.HardRiskType GetSubCoverageRiskTypeByPrefixIdByRiskTypeId(int prefixId, int riskTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(riskTypeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.HardRiskType), filter.GetPredicate());
            PARAMEN.HardRiskType entityHardRiskType = businessCollection.Cast<PARAMEN.HardRiskType>().FirstOrDefault();

            return entityHardRiskType;
        }
    }
}
