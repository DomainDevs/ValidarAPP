namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonServices.V1.DAOs;
    using Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.UniquePersonService.V1.DAOs;
    using Sistran.Core.Application.UniquePersonService.V1.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class CompanyFileDAO
    {
        public string GenerateFileToScoreTypeDoc(string fileName)
        {

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ScoreTypeDoc;
            ScoreTypeDocDAO scoreTypeDocDAO = new ScoreTypeDocDAO();
            List<Models.ScoreTypeDoc> listScoreTypeDoc = scoreTypeDocDAO.GetAllScoreTypeDoc();
            DocumentTypeDAO documentTypeDAO = new DocumentTypeDAO();
            List<DocumentType> documents = documentTypeDAO.GetDocumentTypes(1);
            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);
            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Models.ScoreTypeDoc scoreTypeDoc in listScoreTypeDoc)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
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

                    fields[0].Value = scoreTypeDoc.IdCardTypeScore.ToString();
                    fields[1].Value = scoreTypeDoc.Description;
                    if (scoreTypeDoc.IdCardTypeCode == 0)
                    {
                        fields[2].Value = "Ninguno";
                    }
                    else
                    {
                        fields[2].Value = documents.Find(x => x.Id == scoreTypeDoc.IdCardTypeCode).Description + " - " + scoreTypeDoc.IdCardTypeCode;
                    }

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
    }
}
