using System;
using EnumRules = Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    public class ConceptViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripción { get; set; }
        public int IdEntidad { get; set; }

        [Required]
        public int ConceptId { get; set; }

        [Required]
        public int EntityId { get; set; }

        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelModule")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Module { get; set; }

        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelLevels")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Level { get; set; }

        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelDescription")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        public string ConceptName { get; set; }

        public int KeyOrder { get; set; }

        public bool IsStatic { get; set; }

        public EnumRules.ConceptControlType ConceptControlCode { get; set; }

        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelVisible")]
        public bool IsVisible { get; set; }

        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelIsNull")]
        public bool IsNull { get; set; }

        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelIsPersistible")]
        public bool IsPersistible { get; set; }

        public int OrderNum { get; set; }


        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelTypeConcept")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public EnumRules.ConceptType ConceptTypeCode { get; set; }
        public string ConceptTypeDescription { get; set; }

        public string StaticDescription { get; set; }

        public int IdQuestion { get; set; }
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelTextQuestion")]
        public string QuestionDescription { get; set; }


        public static ConceptViewModel ConceptToViewModel(Concept concept)
        {
            ConceptViewModel ConceptViewModel = null;

            switch (concept.ConceptTypeCode)
            {
                case EnumRules.ConceptType.Basic:
                    ConceptViewModel = new ConceptBasicViewModel();
                    ((ConceptBasicViewModel)ConceptViewModel).BasicTypeCode = ((BasicConcept)concept).BasicTypeCode;
                    ((ConceptBasicViewModel)ConceptViewModel).MinValue = ((BasicConcept)concept).MinValue;
                    ((ConceptBasicViewModel)ConceptViewModel).MaxValue = ((BasicConcept)concept).MaxValue;
                    ((ConceptBasicViewModel)ConceptViewModel).Length = ((BasicConcept)concept).Length;

                    if (((BasicConcept)concept).MinDate != null)
                    {
                        ((ConceptBasicViewModel)ConceptViewModel).MinDate = ((DateTime)((BasicConcept)concept).MinDate).ToString("dd/MM/yyyy");

                    }
                    if (((BasicConcept)concept).MaxDate != null)
                    {
                        ((ConceptBasicViewModel)ConceptViewModel).MaxDate = ((DateTime)((BasicConcept)concept).MaxDate).ToString("dd/MM/yyyy");
                    }
                    break;
                case EnumRules.ConceptType.Range:
                    ConceptViewModel = new ConceptRangeViewModel();
                    ((ConceptRangeViewModel)ConceptViewModel).RangeEntityCode = ((RangeConcept)concept).RangeEntityCode;
                    ((ConceptRangeViewModel)ConceptViewModel).EntityValues = ((RangeConcept)concept).EntityValues;
                    break;
                case EnumRules.ConceptType.List:
                    ConceptViewModel = new ConceptListViewModel();
                    ((ConceptListViewModel)ConceptViewModel).ListEntityCode = ((ListConcept)concept).ListEntityCode;
                    ((ConceptListViewModel)ConceptViewModel).EntityValues = ((ListConcept)concept).EntityValues;
                    break;
                case EnumRules.ConceptType.Reference:
                    ConceptViewModel = new ConceptReferenceViewModel();
                    ((ConceptReferenceViewModel)ConceptViewModel).FEntityId = ((ReferenceConcept)concept).FEntityId;
                    break;
            }
            ConceptViewModel.ConceptId = concept.ConceptId;
            ConceptViewModel.EntityId = concept.EntityId;
            ConceptViewModel.Description = concept.Description;
            ConceptViewModel.ConceptName = concept.ConceptName;
            ConceptViewModel.KeyOrder = concept.KeyOrder;
            ConceptViewModel.IsStatic = concept.IsStatic;
            ConceptViewModel.ConceptControlCode = concept.ConceptControlCode;
            ConceptViewModel.IsVisible = concept.IsVisible;
            ConceptViewModel.IsNull = concept.IsNull;
            ConceptViewModel.IsPersistible = concept.IsPersistible;
            ConceptViewModel.OrderNum = concept.OrderNum;
            ConceptViewModel.ConceptTypeCode = concept.ConceptTypeCode;
            ConceptViewModel.ConceptTypeDescription = concept.ConceptTypeCode.ToString();
            ConceptViewModel.StaticDescription = concept.IsStatic ? App_GlobalResources.Language.Static : App_GlobalResources.Language.Dynamic;

            if (concept.Question != null)
            {
                ConceptViewModel.IdQuestion = concept.Question.QuestionId;
                ConceptViewModel.QuestionDescription = concept.Question.Description;
            }

            return ConceptViewModel;
        }

        public static Concept ConceptToModel(ConceptViewModel concept)
        {
            Concept ConceptModel = null;

            if (concept.GetType() != typeof(ConceptViewModel))
            {
                switch (concept.ConceptTypeCode)
                {
                    case EnumRules.ConceptType.Basic:
                        ConceptModel = new BasicConcept();
                        ((BasicConcept)ConceptModel).BasicTypeCode = ((ConceptBasicViewModel)concept).BasicTypeCode;
                        ((BasicConcept)ConceptModel).MinValue = ((ConceptBasicViewModel)concept).MinValue;
                        ((BasicConcept)ConceptModel).MaxValue = ((ConceptBasicViewModel)concept).MaxValue;
                        ((BasicConcept)ConceptModel).Length = ((ConceptBasicViewModel)concept).Length;

                        if (!string.IsNullOrEmpty(((ConceptBasicViewModel)concept).MinDate))
                        {
                            ((BasicConcept)ConceptModel).MinDate = DateTime.ParseExact(((ConceptBasicViewModel)concept).MinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        }
                        if (!string.IsNullOrEmpty(((ConceptBasicViewModel)concept).MaxDate))
                        {
                            ((BasicConcept)ConceptModel).MaxDate = DateTime.ParseExact(((ConceptBasicViewModel)concept).MaxDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        break;
                    case EnumRules.ConceptType.List:
                        ConceptModel = new ListConcept();
                        ((ListConcept)ConceptModel).ListEntityCode = ((ConceptListViewModel)concept).ListEntityCode;
                        break;

                    case EnumRules.ConceptType.Range:
                        ConceptModel = new RangeConcept();
                        ((RangeConcept)ConceptModel).RangeEntityCode = ((ConceptRangeViewModel)concept).RangeEntityCode;
                        break;
                    case EnumRules.ConceptType.Reference:
                        ConceptModel = new ReferenceConcept();
                        ((ReferenceConcept)ConceptModel).FEntityId = ((ConceptReferenceViewModel)concept).FEntityId;
                        break;
                }
            }
            else
            {
                ConceptModel = new Concept();
            }
            ConceptModel.ConceptId = concept.ConceptId;
            ConceptModel.EntityId = concept.EntityId;
            ConceptModel.Description = concept.Description;
            ConceptModel.ConceptName = concept.ConceptName;
            ConceptModel.KeyOrder = concept.KeyOrder;
            ConceptModel.IsStatic = concept.IsStatic;
            ConceptModel.ConceptControlCode = concept.ConceptControlCode;
            ConceptModel.IsVisible = concept.IsVisible;
            ConceptModel.IsNull = concept.IsNull;
            ConceptModel.IsPersistible = concept.IsPersistible;
            ConceptModel.OrderNum = concept.OrderNum;
            ConceptModel.ConceptTypeCode = concept.ConceptTypeCode;

            if (!string.IsNullOrEmpty(concept.QuestionDescription))
            {
                ConceptModel.Question = new Question()
                {
                    ConceptId = concept.ConceptId,
                    EntityId = concept.EntityId,
                    Description = concept.QuestionDescription,
                    QuestionId = concept.IdQuestion
                };
            }
            return ConceptModel;
        }

    }

    public class ConceptBasicViewModel : ConceptViewModel
    {
        public EnumRules.BasicType BasicTypeCode { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string MinDate { get; set; }
        public string MaxDate { get; set; }
        public int? Length { get; set; }
    }

    public class ConceptListViewModel : ConceptViewModel
    {
        public int ListEntityCode { get; set; }
        public List<EntityValue> EntityValues { get; set; }
    }

    public class ConceptRangeViewModel : ConceptViewModel
    {
        public int RangeEntityCode { get; set; }
        public List<RangeEntityValue> EntityValues { get; set; }
    }

    public class ConceptReferenceViewModel : ConceptViewModel
    {
        public int FEntityId { get; set; }
    }

    public class ConceptsToSave
    {
        List<ConceptBasicViewModel> ConceptBasicAdd { get; set; }
        List<ConceptBasicViewModel> ConceptBasicEdit { get; set; }
        List<ConceptBasicViewModel> ConceptBasicDelete { get; set; }

        List<ConceptListViewModel> ConceptListAdd { get; set; }
        List<ConceptListViewModel> ConceptListEdit { get; set; }
        List<ConceptListViewModel> ConceptListDelete { get; set; }

        List<ConceptRangeViewModel> ConceptRangeAdd { get; set; }
        List<ConceptRangeViewModel> ConceptRangeEdit { get; set; }
        List<ConceptRangeViewModel> ConceptRangeDelete { get; set; }

        List<ConceptReferenceViewModel> ConceptReferenceAdd { get; set; }
        List<ConceptReferenceViewModel> ConceptReferenceEdit { get; set; }
        List<ConceptReferenceViewModel> ConceptReferenceDelete { get; set; }
    }
}
