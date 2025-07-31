// -----------------------------------------------------------------------
// <copyright file="GenericDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.EntityServices.EEProvider.DAOs
{
    using Models;
    using Sistran.Core.Application.EntityServices.Enums;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// DAO para clase generica
    /// </summary>
    public class GenericDAO
    {
        /// <summary>
        /// Object entidad
        /// </summary>        
        private BusinessObject entity = null;

        /// <summary>
        /// Propiedad Tipo
        /// </summary>
        private System.Type type = null;

        /// <summary>
        /// Llave primaria
        /// </summary>
        private List<string> primaryKeys = null;

        /// <summary>
        /// Nombre propiedades
        /// </summary>
        private string[] propertyNames = null;

        /// <summary>
        /// Diccionario dependencias
        /// </summary>
        private Dictionary<string, string> dependencies = null;

        /// <summary>
        /// Inicializa una nueva instancia de la clase GenericDAO
        /// </summary>
        public GenericDAO()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="GenericDAO"/>
        /// </summary>
        /// <param name="entityName"> nombre completo de la entidad</param>
        public GenericDAO(string entityName)
        {
            this.Initialize(entityName);
        }

        /// <summary>
        /// Asigna el tipo de entidad
        /// </summary>     
        /// <returns>modelo PostEntity</returns>
        public PostEntity GetMetadata()
        {
            PostEntity postEntity = new PostEntity
            {
                EntityType = this.type.FullName,
                Fields = new List<Field>()
            };

            PropertyInfo[] propertiesInfo = this.type.GetProperties();

            for (int i = 0; i < propertiesInfo.Length - 3; i++)
            {
                Field field = new Field
                {
                    Name = propertiesInfo[i].Name,
                    Type = new FieldType()
                };

                if (propertiesInfo[i].PropertyType.Name == "Nullable`1")
                {
                    field.Type.Name = propertiesInfo[i].PropertyType.GenericTypeArguments[0].FullName;
                }
                else
                {
                    field.Type.Name = propertiesInfo[i].PropertyType.FullName;
                }

                postEntity.Fields.Add(field);
            }

            this.SetDependencies(postEntity);

            return postEntity;
        }

        /// <summary>
        /// Crea la entidad       
        /// </summary>
        /// <param name="postEntity">Entidad postEntity</param>
        /// <returns>modelo PostEntity</returns>
        public PostEntity Create(PostEntity postEntity)
        {
            if (postEntity.KeyType == KeyType.NextValue)
            {
                var findKey = this.primaryKeys.FirstOrDefault();
                int consecutive = this.GetMaxEntities(postEntity);
                this.entity.SetProperty(findKey, consecutive);
            }

            if (postEntity.KeyType == KeyType.IdentityByKey)
            {
                AssignConsecutiveById(postEntity);
            }

            foreach (string propertyName in this.propertyNames)
            {
                if (postEntity.Fields.Exists(x => x.Name == propertyName && x.IsConsecutiveByKeyOtherColumn == false))
                {
                    this.entity.SetProperty(propertyName, this.ChangeValueType(postEntity.Fields.First(x => x.Name == propertyName)));
                }
            }

            DataFacadeManager.Instance.GetDataFacade().InsertObject(this.entity);

            this.SetPrimaryKeys(postEntity);

            return postEntity;
        }

        /// <summary>
        /// Actualiza la entidad
        /// </summary>
        /// <param name="postEntity">Entidad postEntity</param>   
        /// <returns>Returns PostEntity model</returns> 
        public PostEntity Update(PostEntity postEntity)
        {
            this.LoadEntity(postEntity);

            foreach (string propertyName in this.propertyNames)
            {
                if (postEntity.Fields.Exists(x => x.Name == propertyName) && !this.primaryKeys.Exists(x => x == propertyName))
                {
                    this.entity.SetProperty(propertyName, this.ChangeValueType(postEntity.Fields.First(x => x.Name == propertyName)));
                }
            }

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(this.entity);

            return postEntity;
        }

        /// <summary>
        /// Elimina la entidad       
        /// </summary>
        /// <param name="postEntity">Entidad postEntity</param>
        public void Delete(PostEntity postEntity)
        {
            this.LoadEntity(postEntity);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(this.entity);
        }

        /// <summary>
        /// Obtiene la entidad        
        /// </summary>
        /// <param name="postEntity">Entidad postEntity</param>
        /// <returns>modelo PostEntity</returns> 
        public PostEntity GetEntity(PostEntity postEntity)
        {
            this.LoadEntity(postEntity);

            if (this.entity != null)
            {
                postEntity = this.SetPostEntity(this.entity);
                this.GetDependencies(postEntity);

                return postEntity;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene las entidad por filtro      
        /// </summary>
        /// <param name="postEntity">entidad postEntity</param>
        /// <returns>Listado modelo PostEntity</returns>  
        public List<PostEntity> GetEntities(PostEntity postEntity)
        {
            List<PostEntity> postEntities = new List<PostEntity>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (postEntity.Fields != null)
            {
                for (int i = 0; i < postEntity.Fields.Count; i++)
                {
                    if (i > 0)
                    {
                        filter.And();
                    }

                    filter.PropertyEquals(postEntity.Fields[i].Name, postEntity.Fields[i].Value);
                }
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(this.type, filter.GetPredicate()));

            foreach (BusinessObject businessObject in businessCollection)
            {
                postEntities.Add(this.SetPostEntity(businessObject));
            }

            return postEntities;
        }

        /// <summary>
        /// Obtiene toda la entidad
        /// </summary>
        /// <returns>Listado de entidades</returns>
        public List<PostEntity> GetAllEntities()
        {
            try
            {
                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(this.type, null);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// CRUD para listado de entidades
        /// </summary>
        /// <param name="postEntities">Listado de entidades</param>
        public void CreateArrange(List<PostEntity> postEntities)
        {
            foreach (PostEntity postEntity in postEntities)
            {
                if (postEntity.Status == StatusTypeService.Delete)
                {
                    this.Delete(postEntity);
                }
                else if (postEntity.Status == StatusTypeService.Update)
                {
                    this.Update(postEntity);
                }
                else
                {
                    this.Create(postEntity);
                }
            }
        }

        /// <summary>
        /// Obtiene tipo de entidad
        /// </summary>       
        /// <param name="assemblyName">nombre del ensamblado</param>
        /// <returns>Listado modelo PostEntity</returns>
        public List<string> GetEntityTypes(string assemblyName)
        {
            List<string> entityTypes = new List<string>();

            Assembly assembly = Assembly.Load(assemblyName);

            foreach (System.Type entityType in assembly.ExportedTypes)
            {
                if (entityType.Name == "Properties")
                {
                    break;
                }

                entityTypes.Add(entityType.FullName);
            }

            return entityTypes;
        }

        /// <summary>
        /// Obtiene los ensamblados
        /// </summary>  
        /// <returns>Listado de cadenas</returns>
        public List<string> GetAssemblies()
        {
            PostEntity postEntity = new PostEntity
            {
                EntityType = "Sistran.Core.Application.Parameters.Entities.EntityAssembly"
            };

            this.Initialize(postEntity.EntityType);
            List<PostEntity> postEntities = this.GetEntities(postEntity);
            List<string> assemblies = new List<string>();
            foreach (PostEntity assembly in postEntities)
            {
                assemblies.Add(assembly.Fields.Last().Value);
            }

            return assemblies;
        }

        /// <summary>
        /// Obtiene el valor máximo de la entidad
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        /// <returns>valor máximo</returns>
        private int GetMaxEntities(PostEntity postEntity)
        {
            int consecutive = 0;
            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Max);
            var findKey = this.primaryKeys.FirstOrDefault();

            select.AddSelectValue(new SelectValue(function, findKey));
            select.Table = new ClassNameTable(this.type, this.type.Name);
            function.AddParameter(new Column(findKey, this.type.Name));
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        consecutive = Convert.ToInt32(reader[0].ToString()) + 1;
                    }
                }
            }

            return consecutive;
        }

        /// <summary>
        /// Carga la entidad
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        private void LoadEntity(PostEntity postEntity)
        {
            foreach (string item in this.primaryKeys)
            {
                this.entity.SetProperty(item, postEntity.Fields.First(x => x.Name == item).Value);
            }

            this.entity = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(this.entity.PrimaryKey);
        }

        /// <summary>
        /// Asigna los valores de la entidad
        /// </summary>
        /// <param name="businessObject">objecto de negocio</param>
        /// <returns>entidad PostEntity</returns>
        private PostEntity SetPostEntity(BusinessObject businessObject)
        {
            string[] propertyNames = businessObject.GetPropertyNames();
            PostEntity postEntity = new PostEntity
            {
                Fields = new List<Field>()
            };

            foreach (string propertyName in propertyNames)
            {
                object propertyValue = businessObject.GetProperty(propertyName);

                postEntity.Fields.Add(new Field
                {
                    Name = propertyName,
                    Value = propertyValue != null ? propertyValue.ToString() : string.Empty
                });
            }

            return postEntity;
        }

        /// <summary>
        /// Asigna dependencias
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        private void SetDependencies(PostEntity postEntity)
        {
            MethodModel methodModel = TypeModel.GetModel(this.type).Methods["GetDependencies"];

            if (methodModel != null)
            {
                this.dependencies = (Dictionary<string, string>)methodModel.Invoke(this.type);

                foreach (KeyValuePair<string, string> dependency in this.dependencies)
                {
                    string[] dependencyType = dependency.Value.Split(',');

                    postEntity.Fields.Add(new Field
                    {
                        Name = dependency.Key,
                        Type = new FieldType
                        {
                            Name = dependencyType[0],
                            Multiple = Convert.ToBoolean(dependencyType[1])
                        }
                    });
                }
            }
        }

        /// <summary>
        /// obtiene dependencias
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        private void GetDependencies(PostEntity postEntity)
        {
            this.SetDependencies(postEntity);

            if (this.dependencies != null)
            {
                foreach (KeyValuePair<string, string> dependency in this.dependencies)
                {
                    string[] dependencyType = dependency.Value.Split(',');
                    this.Initialize(dependencyType[0]);
                }
            }
        }

        /// <summary>
        /// Inicializa entidad
        /// </summary>
        /// <param name="entityName">Nombre entidad</param>
        private void Initialize(string entityName)
        {
            string[] assembly = entityName.Split('.');
            this.type = System.Type.GetType(entityName + "," + assembly[1] + "." + assembly[3] + "." + assembly[4]);
            this.entity = (BusinessObject)Activator.CreateInstance(this.type);
            this.primaryKeys = this.entity.PrimaryKey.GetKeys().Keys.Cast<string>().ToList();
            this.propertyNames = this.entity.GetPropertyNames();
        }

        /// <summary>
        /// Asigna llave primaria
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        private void SetPrimaryKeys(PostEntity postEntity)
        {
            foreach (string primaryKey in this.primaryKeys)
            {
                if (!postEntity.Fields.Exists(x => x.Name == primaryKey))
                {
                    postEntity.Fields.Add(new Field
                    {
                        Name = primaryKey,
                        Value = this.entity.GetProperty(primaryKey).ToString()
                    });
                }
                else
                {
                    postEntity.Fields.First(x => x.Name == primaryKey).Value = this.entity.GetProperty(primaryKey).ToString();
                }
            }
        }

        /// <summary>
        /// Crea dependencia
        /// </summary>
        /// <param name="postEntity">Modelo postEntity</param>
        private void CreateDependencies(PostEntity postEntity)
        {
            foreach (KeyValuePair<string, string> dependency in this.dependencies)
            {
                if (postEntity.Fields.Exists(x => x.Name == dependency.Key))
                {
                    string[] dependencyType = dependency.Value.Split(',');
                    this.Initialize(dependencyType[0]);
                }
            }
        }

        /// <summary>
        /// Asigna el tipo
        /// </summary>
        /// <param name="field">parametros del campo</param>
        /// <returns>objeto del campo</returns>
        private object ChangeValueType(Field field)
        {
            if (field.Value == null)
            {
                return null;
            }
            else
            {
                object fieldValue = null;
                string typeName = string.Empty;
                PropertyInfo propertyInfo = this.type.GetProperty(field.Name);

                if (propertyInfo.PropertyType.Name == "Nullable`1")
                {
                    typeName = propertyInfo.PropertyType.GenericTypeArguments[0].FullName;
                }
                else
                {
                    typeName = propertyInfo.PropertyType.FullName;
                }

                switch (typeName)
                {
                    case "System.Int16":
                        fieldValue = Convert.ToInt16(field.Value);
                        break;
                    case "System.Int32":
                        fieldValue = Convert.ToInt32(field.Value);
                        break;
                    case "System.Int64":
                        fieldValue = Convert.ToInt64(field.Value);
                        break;
                    case "System.Decimal":
                        fieldValue = Convert.ToDecimal(field.Value);
                        break;
                    case "System.Boolean":
                        fieldValue = Convert.ToBoolean(field.Value);
                        break;
                    case "System.DateTime":
                        fieldValue = Convert.ToDateTime(field.Value);
                        break;
                    default:
                        fieldValue = field.Value;
                        break;
                }

                return fieldValue;
            }
        }

        /// <summary>
        /// Asigna el valor consecutivo para el campo especifico
        /// </summary>
        /// <param name="postEntity">Dato propios de la entidad</param>
        private void AssignConsecutiveById(PostEntity postEntity)
        {
            int consecutive = 0;
            var key = postEntity.Fields.First(x => x.IsKeyForOtherColumn == true);
            var consecutiveByKey = postEntity.Fields.First(x => x.IsConsecutiveByKeyOtherColumn == true);

            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Max);
            function.AddParameter(new Column(consecutiveByKey.Name, this.type.Name));

            select.AddSelectValue(new SelectValue(function, consecutiveByKey.Name));
            select.Table = new ClassNameTable(this.type, this.type.Name);
            
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(key.Name);
            filter.Equal();           
            filter.Constant(ChangeValueType(postEntity.Fields.First(x => x.Name == key.Name)));
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        consecutive = Convert.ToInt32(reader[0].ToString()) + 1;
                        this.entity.SetProperty(consecutiveByKey.Name, consecutive);
                    }
                    else
                    {
                        consecutive = 1;
                        entity.SetProperty(consecutiveByKey.Name, consecutive);
                    }
                }
            }
        }
    }
}
