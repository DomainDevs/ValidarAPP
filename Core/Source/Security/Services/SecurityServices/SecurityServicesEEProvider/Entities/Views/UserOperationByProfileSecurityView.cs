using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.SecurityServices.EEProvider.Entities.Views
{

    /// <summary>
    /// Definición de la vista que vincula User con todos los accesos y 
    /// entidades relacionadas.
    /// </summary>
    [Serializable()]
    public class UserOperationByProfileSecurityView : BusinessView
    {
        /// <summary>
        /// Constructor de instancias de vista.
        /// </summary>
        public UserOperationByProfileSecurityView()
        { }

        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.UniqueAccess.Entities.Access">Access
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de Access.
        /// </value>
        public BusinessCollection Access
        {
            get
            {
                return this["Access"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.UniqueAccess.Entities.AccessObject">AccessObject
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de AccessObject.
        /// </value>
        public BusinessCollection AccessObject
        {
            get
            {
                return this["AccessObject"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.UniqueSubmodule.Entities.Submodule">Submodule
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de Submodule.
        /// </value>
        public BusinessCollection Submodule
        {
            get
            {
                return this["Submodule"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.UniqueModule.Entities.Module">Module
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de Module.
        /// </value>
        public BusinessCollection Module
        {
            get
            {
                return this["Module"];
            }
        }
        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.Application.UniqueUser.Entities.AccessProfile">AccessProfile
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de AccessProfile.
        /// </value>
        public BusinessCollection AccessProfile
        {
            get
            {
                return this["AccessProfile"];
            }
        }

        //#######################################################
        // Relación "AccessSubmodule"
        //#######################################################

        /// <summary>
        /// Para la relación "AccessSubmodule", establecida entre <see 
        /// cref="Sistran.Core.Application.UniqueUser.Entities.Access">Access
        /// </see> y <see cref="Sistran.Core.Application.UniqueUser.Entities.Submodule">
        /// Submodule</see> obtiene un Access a partir de un Submodule.
        /// </summary>
        /// <param name="Submodule">
        /// Submodule a partir de la que se desea obtener el Access asociado.
        /// </param>
        /// <returns>
        /// Access asociado a la asociación Submodule dada.
        /// </returns>
        public UniqueUser.Entities.Submodules GetSubmoduleByOperation(UniqueUser.Entities.Accesses operation)
        {
            return (UniqueUser.Entities.Submodules)
                this.GetObjectByRelation(
                "OperationSubmodule", operation, false);
        }

        /// <summary>
        /// Para la relación "AccessSubmodule", establecida entre <see 
        /// cref="Sistran.Core.Application.UniqueUser.Entities.Access">Access
        /// </see> y <see cref="Sistran.Core.Application.UniqueUser.Entities.Submodule">
        /// Submodule</see> obtiene todos los Submodule relacionados con un 
        /// Access.
        /// </summary>
        /// <param name="access">
        /// Access del que se desea obtener todos los Submodule asociados.
        /// </param>
        /// <returns>
        /// Colección de objetos Submodule relacionados con el Access.
        /// </returns>
        public BusinessCollection GetOperationBySubmodule(
            UniqueUser.Entities.Submodules submodule)
        {
            return this.GetObjectsByRelation(
                "OperationSubmodule", submodule, true);
        }

        //#######################################################
        // Relación "SubmoduleModule"
        //#######################################################

        /// <summary>
        /// Para la relación "SubmoduleModule", establecida entre <see 
        /// cref="Sistran.Core.Application.UniqueUser.Entities.Submodule">Submodule
        /// </see> y <see cref="Sistran.Core.Application.UniqueUser.Entities.Module">
        /// Module</see> obtiene un Submodule a partir de un Module.
        /// </summary>
        /// <param name="Module">
        /// Module a partir de la que se desea obtener el Submodule asociado.
        /// </param>
        /// <returns>
        /// Submodule asociado a la asociación Module dada.
        /// </returns>
        public UniqueUser.Entities.Modules GetModuleBySubmodule(UniqueUser.Entities.Submodules submodule)
        {
            return (UniqueUser.Entities.Modules)
                this.GetObjectByRelation(
                "SubmoduleModule", submodule, false);
        }

        /// <summary>
        /// Para la relación "SubmoduleModule", establecida entre <see 
        /// cref="Sistran.Core.Application.UniqueUser.Entities.Submodule">Submodule
        /// </see> y <see cref="Sistran.Core.Application.UniqueUser.Entities.Module">
        /// Module</see> obtiene todos los Module relacionados con un 
        /// Submodule.
        /// </summary>
        /// <param name="submodule">
        /// Submodule del que se desea obtener todos los Module asociados.
        /// </param>
        /// <returns>
        /// Colección de objetos Module relacionados con el Submodule.
        /// </returns>
        public BusinessCollection GetSubModulesByModule(
            UniqueUser.Entities.Modules module)
        {
            return this.GetObjectsByRelation(
                "SubmoduleModule", module, true);
        }
    }
}
