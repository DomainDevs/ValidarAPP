// -----------------------------------------------------------------------
// <copyright file="EntityServiceEEProvider.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.EntityServices.EEProvider
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using DAOs;
    using Models;
    using Resources;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.DAF.Engine.Factories;

    /// <summary>
    /// Clase provider
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class EntityServiceEEProvider : IEntityService
    {

        /// <summary>
        /// Obtiene los ensamblados
        /// </summary>
        /// <returns>Listado de cadenas</returns>
        public List<string> GetAssemblies()
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO();
                return genericDAO.GetAssemblies();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetAssemblies, ex);
            }
        }

        /// <summary>
        /// Asigna el tipo de entidad
        /// </summary>
        /// <param name="entityType">Tipo de entidad</param>
        /// <returns>modelo PostEntity</returns>
        public PostEntity GetMetadata(string entityType)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(entityType);
                return genericDAO.GetMetadata();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetMetadata, ex);
            }
        }

        /// <summary>
        /// Obtiene tipo de entidad
        /// </summary>       
        /// <param name="asemblyName">nombre del ensamblado</param>
        /// <returns>Listado de cadenas</returns>
        public List<string> GetEntityTypes(string asemblyName)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO();
                return genericDAO.GetEntityTypes(asemblyName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetEntityTypes, ex);
            }
        }

        /// <summary>
        /// Crea la entidad       
        /// </summary>
        /// <param name="postEntity">entidad postEntity</param>
        /// <returns>modelo PostEntity</returns>
        public PostEntity Create(PostEntity postEntity)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(postEntity.EntityType);
                return genericDAO.Create(postEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreate, ex);
            }
        }

        /// <summary>
        /// Actualiza la entidad
        /// </summary>
        /// <param name="postEntity">entidad postEntity</param>   
        /// <returns>Returns PostEntity model</returns> 
        public PostEntity Update(PostEntity postEntity)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(postEntity.EntityType);
                return genericDAO.Update(postEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorUpdate, ex);
            }
        }

        /// <summary>
        /// Elimina la entidad       
        /// </summary>
        /// <param name="postEntity">entidad postEntity</param>
        public void Delete(PostEntity postEntity)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(postEntity.EntityType);
                genericDAO.Delete(postEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorDelete, ex);
            }
        }

        /// <summary>
        /// Obtiene la entidad        
        /// </summary>
        /// <param name="postEntity">entidad postEntity</param>
        /// <returns>modelo PostEntity</returns> 
        public PostEntity GetEntity(PostEntity postEntity)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(postEntity.EntityType);
                return genericDAO.GetEntity(postEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetEntity, ex);
            }
        }

        /// <summary>
        /// Obtiene las entidad por filtro      
        /// </summary>
        /// <param name="postEntity">entidad postEntity</param>
        /// <returns>Listado modelo PostEntity</returns>  
        public List<PostEntity> GetEntities(PostEntity postEntity)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(postEntity.EntityType);
                return genericDAO.GetEntities(postEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetEntities, ex);
            }
        }

        /// <summary>
        /// CRUD para listado de entidades
        /// </summary>
        /// <param name="postEntities">Listado de entidades</param>
        public void CreateArrange(List<PostEntity> postEntities)
        {
            try
            {
                GenericDAO genericDAO = new GenericDAO(postEntities[0].EntityType);
                genericDAO.CreateArrange(postEntities);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
