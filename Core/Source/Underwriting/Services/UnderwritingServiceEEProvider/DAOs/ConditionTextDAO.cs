using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PAREN = Sistran.Core.Application.Parameters.Entities;
using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class ConditionTextDAO
    {
        /// <summary>
        /// retorna el consecutivo correspondiente a el conditiontext máximo 
        /// </summary>
        /// <returns></returns>
        public int FindIdConditionText()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            System.Collections.IList conditionalTextList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ConditionText), null, null));
            List<ParamConditionText> listParamConditionText = new List<ParamConditionText>();
            foreach (ConditionText conditionText in conditionalTextList)
            {
                ParamConditionText paramConditionText = new ParamConditionText();
                paramConditionText.Id = conditionText.ConditionTextId;
                listParamConditionText.Add(paramConditionText);
            }

            return listParamConditionText.Max(x => x.Id) + 1;

        }

        /// <summary>
        /// retorna el consecutivo correspondiente a el conditiontext máximo 
        /// </summary>
        /// <returns></returns>
        private int FindIdConditionTextLevel()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            System.Collections.IList conditionalTextList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CondTextLevel), null, null));
            List<ParamConditionText> listParamConditionText = new List<ParamConditionText>();
            foreach (CondTextLevel conditionText in conditionalTextList)
            {
                ParamConditionText paramConditionText = new ParamConditionText();
                paramConditionText.Id = conditionText.CondTextLevelId;
                listParamConditionText.Add(paramConditionText);
            }

            return listParamConditionText.Max(x => x.Id) + 1;

        }
        public ParamConditionText CreateConditiontext(ParamConditionText conditionText)
        {
            int PkConditionText = FindIdConditionText(), PkConditionTextLevel = FindIdConditionTextLevel();
            ConditionText entityconditionText = new ConditionText();
            CondTextLevel entityCondTextLevel = new CondTextLevel();
            conditionText.Id = PkConditionText;

            entityconditionText = Assemblers.EntityAssembler.CreateParamConditionText(conditionText);
            entityCondTextLevel = Assemblers.EntityAssembler.CreateParamConditionTextLevel(conditionText);
            entityCondTextLevel.CondTextLevelId = PkConditionTextLevel;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.InsertObject(entityconditionText);
                daf.InsertObject(entityCondTextLevel);
            }

            if (entityconditionText.ConditionTextId != 0)
            {
                conditionText.Id = entityconditionText.ConditionTextId;
                return conditionText;
            }
            else
            {
                throw new BusinessException("Error en CreateCity consultando consecutivo city ");
            }
        }

        public ParamConditionText UpdateConditiontext(ParamConditionText conditionText)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CondTextLevel.Properties.ConditionTextId, typeof(CondTextLevel).Name);
                filter.Equal();
                filter.Constant(conditionText.Id);
                ConditionTextView view = new ConditionTextView();
                ViewBuilder builder = new ViewBuilder("ConditionTextView");
                builder.Filter = filter.GetPredicate();
                System.Collections.IList conditionalTextList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CondTextLevel), null, null));

                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }

                foreach (CondTextLevel CondTextLevel in view.CondTextLevel)
                {
                    PrimaryKey primaryKey1 = CondTextLevel.CreatePrimaryKey(CondTextLevel.CondTextLevelId);
                    var EntityCondTextLevel = (CondTextLevel)DataFacadeManager.GetObject(primaryKey1);
                    EntityCondTextLevel.ConditionLevelId = conditionText.ConditionTextLevelType.Id;
                    DataFacadeManager.Update(EntityCondTextLevel);
                }

                PrimaryKey primaryKey = ConditionText.CreatePrimaryKey(conditionText.Id);
                var Entityconditiontext = (ConditionText)DataFacadeManager.GetObject(primaryKey);
                Entityconditiontext.TextTitle = conditionText.Title;
                Entityconditiontext.TextBody = conditionText.Body;
                Entityconditiontext.ConditionLevelCode = conditionText.ConditionTextLevel.Id;
                DataFacadeManager.Update(Entityconditiontext);
                return conditionText;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public string DeleteConditiontext(ParamConditionText conditionText)
        {
            try
            {
                bool result = false;
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CondTextLevel.Properties.ConditionTextId, typeof(CondTextLevel).Name);
                filter.Equal();
                filter.Constant(conditionText.Id);
                ConditionTextView view = new ConditionTextView();
                ViewBuilder builder = new ViewBuilder("ConditionTextView");
                builder.Filter = filter.GetPredicate();
                System.Collections.IList conditionalTextList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CondTextLevel), null, null));

                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }

                foreach (CondTextLevel CondTextLevel in view.CondTextLevel)
                {
                    PrimaryKey primaryKey1 = CondTextLevel.CreatePrimaryKey(CondTextLevel.CondTextLevelId);
                    result = DataFacadeManager.Delete(primaryKey1);
                    if (result == false)
                    {
                        return "Error";
                    }
                }

                PrimaryKey primaryKey = ConditionText.CreatePrimaryKey(conditionText.Id);
                result = DataFacadeManager.Delete(primaryKey);
                if (result == false)
                {
                    return "Error";
                }
                return "Ok";
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamConditionText> GetConditiontext()

        {
            try
            {

                List<ParamConditionText> listCompanyParamCity = new List<ParamConditionText>();
                ConditionTextView view = new ConditionTextView();
                ViewBuilder builder = new ViewBuilder("ConditionTextView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (ConditionText conditionText in view.ConditionText)
                {
                    PAREN.ConditionLevel conditionLevel = view.ConditionLevels.Cast<PAREN.ConditionLevel>().First(x => x.ConditionLevelCode == conditionText.ConditionLevelCode);
                    CondTextLevel conditionLevelType = view.CondTextLevel.Cast<CondTextLevel>().First(x => x.ConditionTextId == conditionText.ConditionTextId);
                    ParamConditionText paramText = new ParamConditionText();
                    paramText.Id = conditionText.ConditionTextId;
                    paramText.Title = conditionText.TextTitle;
                    paramText.Body = conditionText.TextBody;
                    paramText.ConditionTextLevel = new BaseConditionTextLevel { Id = conditionLevel.ConditionLevelCode, Description = conditionLevel.SmallDescription };
                    paramText.ConditionTextLevelType = new Models.Base.BaseConditionTextLevelType { Id = Convert.ToInt32(conditionLevelType.ConditionLevelId), Description = "" };
                    listCompanyParamCity.Add(paramText);
                }
                return listCompanyParamCity;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);

            }
        }

        public List<ParamConditionText> GetConditiontextByDescription(int integer = 0, string description = "")
        {
            try
            {
                List<ParamConditionText> listCompanyParamCity = new List<ParamConditionText>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ConditionText.Properties.TextTitle, typeof(ConditionText).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                ConditionTextView view = new ConditionTextView();
                ViewBuilder builder = new ViewBuilder("ConditionTextView");
                builder.Filter = filter.GetPredicate();
                System.Collections.IList conditionalTextList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CondTextLevel), null, null));

                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }

                foreach (ConditionText conditionText in view.ConditionText)
                {
                    PAREN.ConditionLevel conditionLevel = view.ConditionLevels.Cast<PAREN.ConditionLevel>().First(x => x.ConditionLevelCode == conditionText.ConditionLevelCode);
                    CondTextLevel conditionLevelType = view.CondTextLevel.Cast<CondTextLevel>().First(x => x.ConditionTextId == conditionText.ConditionTextId);
                    ParamConditionText paramText = new ParamConditionText();
                    paramText.Id = conditionText.ConditionTextId;
                    paramText.Title = conditionText.TextTitle;
                    paramText.Body = conditionText.TextBody;
                    paramText.ConditionTextLevel = new BaseConditionTextLevel { Id = conditionLevel.ConditionLevelCode, Description = conditionLevel.SmallDescription };
                    paramText.ConditionTextLevelType = new Models.Base.BaseConditionTextLevelType { Id = Convert.ToInt32(conditionLevelType.ConditionLevelId), Description = "" };
                    listCompanyParamCity.Add(paramText);
                }


                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
                throw new BusinessException("excepcion en  Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs.CityDao.GetAllCity", ex);
            }
        }


        public string GenerateFileToConditiontext(List<ParamConditionText> ConditionTextList, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue
                {
                    Key1 = (int)UTILEN.FileProcessType.ParametrizationText
                };
                FileDAO fileDAO = new FileDAO();
                File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);


                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamConditionText conditionTextElement in ConditionTextList)
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

                        fields[0].Value = conditionTextElement.Id.ToString();
                        fields[1].Value = conditionTextElement.Title;
                        fields[2].Value = conditionTextElement.ConditionTextLevel.Description;
                        fields[3].Value = conditionTextElement.ConditionTextLevelType.Description;
                        fields[4].Value = conditionTextElement.Body;

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
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public string GenerateFileToConditiontext()
        {
            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            //return cityDao.CreateCity(companyParamCity);
            throw new NotImplementedException();
        }
    }
}

    
