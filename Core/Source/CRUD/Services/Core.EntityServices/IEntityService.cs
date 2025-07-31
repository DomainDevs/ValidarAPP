// -----------------------------------------------------------------------
// <copyright file="IEntityService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.EntityServices
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Models;

    /// <summary>
    /// Interfaz IEntityService
    /// </summary>
    [ServiceContract]
    public interface IEntityService
    {
        /// <summary>
        /// Obtiene los ensamblados
        /// </summary>
        /// <returns>Listado de cadenas</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        List<string> GetAssemblies();

        /// <summary>
        /// Obtiene tipo de entidad
        /// </summary>       
        /// <param name="asemblyName">Listado modelo PostEntity</param>
        /// <returns>Listado de cadenas</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        List<string> GetEntityTypes(string asemblyName);

        /// <summary>
        /// Asigna el tipo de entidad
        /// </summary>
        /// <param name="entityType">Tipo de entidad</param>
        /// <returns>modelo PostEntity</returns>
        [OperationContract]
        [WebInvoke(Method = "GET")]
        PostEntity GetMetadata(string entityType);

        /// <summary>
        /// Crea la entidad       
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        /// <returns>modelo PostEntity</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        PostEntity Create(PostEntity postEntity);

        /// <summary>
        /// Actualiza la entidad
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>   
        /// <returns>Returns PostEntity model</returns> 
        [OperationContract]
        [WebInvoke(Method = "POST")]
        PostEntity Update(PostEntity postEntity);

        /// <summary>
        /// Elimina la entidad       
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        void Delete(PostEntity postEntity);

        /// <summary>
        /// Obtiene la entidad        
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        /// <returns>modelo PostEntity</returns> 
        [OperationContract]
        [WebInvoke(Method = "POST")]
        PostEntity GetEntity(PostEntity postEntity);

        /// <summary>
        /// Obtiene las entidad por filtro      
        /// </summary>      
        /// <param name="postEntity">Modelo PostEntity</param>
        /// <returns>Listado de modelo PostEntity</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        List<PostEntity> GetEntities(PostEntity postEntity);

        /// <summary>
        /// CRUD para listado de entidades
        /// </summary>
        /// <param name="postEntities">Listado de entidades</param>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        void CreateArrange(List<PostEntity> postEntities);
    }
}
