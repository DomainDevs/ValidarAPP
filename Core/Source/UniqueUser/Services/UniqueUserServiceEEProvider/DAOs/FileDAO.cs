using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UtilitiesServices.Enums;
using EnumUtilities = Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class FileDAO
    {
        /// <summary>
        /// Generar Archivo Modulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToModules(List<Module> modules, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.Modules;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Module module in modules)
                {
                    var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = module.Id.ToString();
                    fields[1].Value = module.Description;
                    fields[2].Value = module.EnabledDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// Generar Archivo SubModulo
        /// </summary>
        /// <param name="submodule">subModulos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToSubmodules(List<SubModule> modules, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.Submodules;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (SubModule module in modules)
                {
                    var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = module.Module.Id.ToString();
                    fields[1].Value = module.Module.Description;
                    fields[2].Value = module.Id.ToString();
                    fields[3].Value = module.Description;
                    fields[4].Value = module.EnabledDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// Generar Archivo Accesos
        /// </summary>
        /// <param name="access">accesos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToAccess(List<AccessObject> access, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.Accesses;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (AccessObject item in access)
                {
                    var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = item.SubModule.Module.Id.ToString();
                    fields[1].Value = item.SubModule.Module.Description;
                    fields[2].Value = item.SubModule.Id.ToString();
                    fields[3].Value = item.SubModule.Description;
                    fields[4].Value = item.Description;
                    fields[5].Value = Enum.GetName(typeof(AccessObjectType), item.ObjectTypeId);
                    fields[6].Value = item.Url;
                    fields[7].Value = item.EnabledDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generar Archivo Accesos
        /// </summary>
        /// <param name="access">accesos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToProfiles(List<Profile> profiles, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.Profiles;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Profile item in profiles)
                {
                    var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = item.Id.ToString();
                    fields[1].Value = item.Description;
                    fields[2].Value = item.EnabledDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

    }
}
