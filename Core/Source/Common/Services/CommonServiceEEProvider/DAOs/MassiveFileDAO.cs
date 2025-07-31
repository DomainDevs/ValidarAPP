using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CommonService.DAOs
{
    public class MassiveFileDAO
    {
        public List<File> ValidateMassiveFile(File file)
        {
            try
            {
                Template principalTemplate = CopyFile(file).Templates.First(x => x.IsPrincipal);
                List<Row> rows = principalTemplate.Rows;
                List<int> packageProcesses = DataFacadeManager.GetPackageProcesses(rows.Count, "MaxThreadMassive");

                List<Row> newRows = new List<Row>();
                List<Template> newTemplates = new List<Template>();
                List<File> newFiles = new List<File>();
                TemplateDAO templateDAO = new TemplateDAO();
                FileDAO fileDao = new FileDAO();

                for (int i = 0; i < packageProcesses.Count; i++)
                {
                    List<Row> packageRows = new List<Row>();
                    packageRows = rows.Take(packageProcesses[i]).ToList();
                    rows.RemoveRange(0, packageProcesses[i]);

                    Parallel.ForEach(packageRows, (row) =>
                    {
                        templateDAO.ValidateDataRow(row);

                        File newFile = CopyFile(file);
                        newFile.Templates.ForEach(t => t.Rows.Clear());
                        newFile.Templates.First().Rows.Add(row);
                        newFiles.Add(newFile);
                    });
                }
                
                List<Template> extraTemplates = CopyFile(file).Templates.Where(x => !x.IsPrincipal).ToList();

                foreach (Template templ in extraTemplates)
                {
                    Template newTemplate = templ;

                    rows = templ.Rows;
                    packageProcesses = DataFacadeManager.GetPackageProcesses(rows.Count, "MaxThreadMassive");

                    for (int i = 0; i < packageProcesses.Count; i++)
                    {
                        List<Row> packageRows = new List<Row>();
                        packageRows = rows.Take(packageProcesses[i]).ToList();
                        rows.RemoveRange(0, packageProcesses[i]);
                        Parallel.ForEach(packageRows, (row) =>
                        {
                            templateDAO.ValidateDataRow(row);
                            var identifier = row.Fields.First(x => x.PropertyName == FieldPropertyName.Identificator).Value;
                            var fileOfPolicy = newFiles.FindAll(f => f.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.First(x => x.PropertyName == FieldPropertyName.Identificator).Value == identifier);
                            if (fileOfPolicy.Count > 0)
                            {
                                fileOfPolicy.First().Templates.First(t => t.Id == templ.Id).Rows.Add(row);
                            }
                        });
                    }
                }

                return newFiles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private File CopyFile(File file)
        {


            List<Template> newTemps = new List<Template>();
            foreach (Template t in file.Templates)
            {
                List<Row> newRows = new List<Row>();
                foreach (Row r in t.Rows)
                {
                    List<Field> newFields = new List<Field>();
                    foreach(Field f in r.Fields)
                    {
                        newFields.Add(new Field()
                        {
                            ColumnSpan = f.ColumnSpan,
                             Description = f.Description,
                             FieldType = f.FieldType,
                             Id = f.Id,
                             IsEnabled = f.IsEnabled,
                             IsMandatory = f.IsMandatory,
                             Order = f.Order,
                             PropertyLength = f.PropertyLength,
                             PropertyName = f.PropertyName,
                             RowPosition = f.RowPosition,
                             SmallDescription = f.SmallDescription,
                             Value = f.Value
                        });
                    }
                    newRows.Add(new Row()
                    {
                        ErrorDescription = r.ErrorDescription,
                        Fields = newFields,
                        HasError = r.HasError,
                        Id = r.Id,
                        Number = r.Number
                    });
                }
                newTemps.Add(new Template()
                {
                    Description = t.Description,
                    Id = t.Id,
                    IsEnabled = t.IsEnabled,
                    IsMandatory = t.IsMandatory,
                    IsPrincipal = t.IsPrincipal,
                    Order = t.Order,
                    PropertyName = t.PropertyName,
                    Rows = newRows,
                    TemplateId = t.TemplateId
                });
            }
            File newFile = new File()
            {
                Description = file.Description,
                FileType = file.FileType,
                Id = file.Id,
                IsEnabled = file.IsEnabled,
                Name = file.Name,
                Observations = file.Observations,
                SmallDescription = file.SmallDescription,
                Templates = newTemps


            };

            return newFile;

        }
    }
}
