using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UniqueUserBusinessService.Models;
using Sistran.Company.Application.UniqueUserBusinessServiceProvider.Assemblers;
using CompanyEntities = Sistran.Company.Application.UniqueUser.Entities;
using CoreEntities = Sistran.Core.Application.UniqueUser.Entities;

// -----------------------------------------------------------------------
// <copyright file="GroupDao.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserBusinessServiceProvider.DAOs
{
    /// <summary>
    /// DAO methods of user group.
    /// </summary>
    public class GroupDao
    {
        /// <summary>
        /// Get list of groups.
        /// </summary>
        /// <returns>List<CompanyGroup>. Object company Group</returns>
        internal List<CompanyGroup> GetAllGroup()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(CompanyEntities.Group.Properties.Enabled);
                filter.Equal();
                filter.Constant(true);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyEntities.Group), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return ModelAssembler.MappParamGroups(businessCollection);
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                throw new BusinessException("excepcion en  Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs.CityDao.GetAllCity", ex);
            }

        }

        /// <summary>
        /// Get list of user groups.
        /// </summary>
        /// <returns>List<CompanyUserGroup>. Object company User Group</returns>
        internal List<CompanyUserGroup> GetAllUserGroup(int userId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                
                filter.Property(CoreEntities.UserGroup.Properties.UserId);
                filter.Equal();
                filter.Constant(userId);                

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoreEntities.UserGroup), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return ModelAssembler.MappParamUserGroups(businessCollection);
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                throw new BusinessException("excepcion en  Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs.CityDao.GetAllCity", ex);
            }

        }

        /// <summary>
        /// Delete list of groups of User.
        /// </summary>
        /// <returns></returns>
        public void DeleteAllUserGroup(int userId)
        {
            if (userId > 0)
            {
                var filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CoreEntities.UserGroup.Properties.UserId, userId);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(CoreEntities.UserGroup), filter.GetPredicate());
            }
        }

        /// <summary>
        /// Save list of groups.
        /// </summary>
        /// <returns>CompanyUserGroup. Object company User Group</returns>
        public void SaveAllUserGroup(List<CompanyUserGroup> companyUserGroups)
        {
            if (companyUserGroups.Count > 0)
            {
                if (companyUserGroups[0].IdGroup != 0)
                {
                    List<CoreEntities.UserGroup> entityUserGroup = ModelAssembler.MappEntityUserGroups(companyUserGroups);
                    BusinessCollection businessCollection = new BusinessCollection();
                    businessCollection.AddRange(entityUserGroup);
                    DataFacadeManager.Instance.GetDataFacade().InsertObjects(businessCollection);
                }                
            }
        }
    }
}
