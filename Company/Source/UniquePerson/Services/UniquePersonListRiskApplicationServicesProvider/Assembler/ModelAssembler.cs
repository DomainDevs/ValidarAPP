using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServicesProvider.Assembler
{
    public static class ModelAssembler
    {
        public static CompanyListRiskPerson CreateCompanyListRiskPerson(ListRiskPersonDTO listRiskPersonDTO)
        {
            return new CompanyListRiskPerson()
            {
                IdCardNo = listRiskPersonDTO.IdCardNo,
                DocumentType = listRiskPersonDTO.DocumentType.Id.GetValueOrDefault(),
                AssignmentDate = DateTime.Now,
                CreateListUserId = listRiskPersonDTO.CreateListUserId,
                ExludedPerson = listRiskPersonDTO.ExcludedPerson,
                IsEnabled = listRiskPersonDTO.IsEnabled,
                LastChangeDate = DateTime.Now,
                ListRisk = listRiskPersonDTO.ListRisk.Id,
                ListRiskDescription = listRiskPersonDTO.ListRisk.Description,
                Name = listRiskPersonDTO.Name,
                LastName = listRiskPersonDTO.LastName,
                UpdateListUserId = listRiskPersonDTO.CreateListUserId
            };
        }

        public static ListRiskPersonDTO CreateListRiskPersonDTO(CompanyListRiskPerson companyListRisk)
        {
            return new ListRiskPersonDTO()
            {
                IdCardNo = companyListRisk.IdCardNo,
                DocumentType = new DocumentTypeDTO { Id = companyListRisk.DocumentType },
                AssignmentDate = companyListRisk.AssignmentDate,
                CreateListUserId = companyListRisk.CreateListUserId,
                ExcludedPerson = companyListRisk.ExludedPerson,
                IsEnabled = companyListRisk.IsEnabled,
                LastChangeDate = companyListRisk.LastChangeDate,
                ListRisk = new ListRiskDTO { Id = companyListRisk.ListRisk, Description = companyListRisk.ListRiskDescription },
                Name = companyListRisk.Name,
                LastName = companyListRisk.LastName,
                UpdateListUserId = companyListRisk.UpdateListUserId
            };
        }

        public static ListRiskPersonDTO CreateListRiskPersonDTO(CompanyListRisk companyListRisk)
        {
            return new ListRiskPersonDTO()
            {
                Id = companyListRisk.Id,
                DataId = companyListRisk.DocumentNumber,
                DocumentType = new DocumentTypeDTO { Id = companyListRisk.DocumentType, Description = companyListRisk.DocumentTypeDescription },
                IdCardNo = companyListRisk.DocumentNumber,
                Name = companyListRisk.Name ?? string.Empty,
                LastName = companyListRisk.LastName ?? string.Empty,
                AliasName = companyListRisk.Alias,
                BirthDate = companyListRisk.BirthDate,
                ListRisk = new ListRiskDTO()
                {
                    Id = companyListRisk.ListRiskId,
                    RiskListType = companyListRisk.ListRiskType,
                    Description = companyListRisk.ListRiskDescription,
                    RiskListTypeDescription = companyListRisk.ListRiskTypeDescription
                },
                AssignmentDate = companyListRisk.AssignmentDate,
                Event = companyListRisk.Event,
                ProcessId = companyListRisk.ProcessId,
                Excluded = companyListRisk.Event == 2 ? RiskListConstants.SI : RiskListConstants.NO,
                IssueDate = companyListRisk.IssueDate
            };
        }

        public static ListRiskPersonDTO CreateListRiskPersonDTO(CompanyListRiskOnu companyListRisk)
        {
            return new ListRiskPersonDTO()
            {
                Id = companyListRisk.Id,
                DataId = companyListRisk.DataId.ToString(),
                DocumentType = new DocumentTypeDTO { Description = companyListRisk.Document == null ? string.Empty : string.Join(" , ", companyListRisk.Document.Where(x => !string.IsNullOrEmpty(x.DocumentType)).Select(x => x.DocumentType)) },
                IdCardNo = companyListRisk.Document == null ? string.Empty : string.Join(" , ", companyListRisk.Document.Where(x => !string.IsNullOrEmpty(x.DocumentNumber)).Select(x => x.DocumentNumber)),
                Name = $"{companyListRisk.FirstName ?? string.Empty} {companyListRisk.SecondName ?? string.Empty}",
                LastName = $"{ companyListRisk.ThirdName ?? string.Empty } { companyListRisk.FourthName ?? string.Empty }",
                AliasName = companyListRisk.Alias == null ? string.Empty : string.Join(" , ", companyListRisk.Alias.Where(x => !string.IsNullOrEmpty(x.AliasName)).Select(x => x.AliasName)),
                BirthDate = null,
                ListRisk = new ListRiskDTO()
                {
                    Id = companyListRisk.ListRiskId,
                    RiskListType = companyListRisk.ListRiskType,
                    Description = companyListRisk.ListRiskDescription,
                    RiskListTypeDescription = companyListRisk.ListRiskTypeDescription
                },
                AssignmentDate = companyListRisk.AssignmentDate,
                Event = companyListRisk.Event,
                ProcessId = companyListRisk.ProcessId,
                Excluded = companyListRisk.Event == 2 ? RiskListConstants.SI : RiskListConstants.NO
            };
        }

        public static ListRiskPersonDTO CreateListRiskPersonDTO(CompanyListRiskOfac companyListRisk)
        {
            var remarksSplit = companyListRisk.Remarks.Split(';');
            var documentType = "";
            var idCardNo = "";

            remarksSplit.ToList().ForEach(x =>
            {
                if (x.Trim().EndsWith(")") || x.Trim().EndsWith(")."))
                {
                    if (!x.Contains("a.k.a"))
                    {
                        if (x.ToCharArray().Count(c => c == '(') == 1 && x.ToCharArray().Count(c => c == ')') == 1)
                        {
                            if (x.Split('(')[1].Length > 3)
                            {
                                var split = x.Split(' ');
                                var first = split.Select((item, index) => new { item, index }).First(c => c.item.Contains("("));

                                if (string.IsNullOrEmpty(idCardNo))
                                {
                                    idCardNo = string.Join(" ", split.Skip(first.index - 1));
                                    documentType = string.Join(" ", split.Take(first.index - 1));
                                }
                                else
                                {
                                    idCardNo += " , " + string.Join(" ", split.Skip(first.index - 1));
                                    documentType += " , " + string.Join(" ", split.Take(first.index - 1));
                                }
                            }
                        }
                    }
                }
            });

            return new ListRiskPersonDTO()
            {
                Id = companyListRisk.Id,
                DataId = companyListRisk.EntNum.ToString(),
                DocumentType = new DocumentTypeDTO { Description = documentType },
                IdCardNo = idCardNo,
                Name = companyListRisk.SDNName,
                LastName = string.Empty,
                AliasName = string.Join(" , ", remarksSplit.Where(x => x.Contains("a.k.a"))),
                BirthDate = null,
                ListRisk = new ListRiskDTO()
                {
                    Id = companyListRisk.ListRiskId,
                    RiskListType = companyListRisk.ListRiskType,
                    Description = companyListRisk.ListRiskDescription,
                    RiskListTypeDescription = companyListRisk.ListRiskTypeDescription
                },
                AssignmentDate = companyListRisk.AssignmentDate,
                Event = companyListRisk.Event,
                ProcessId = companyListRisk.ProcessId,
                Excluded = companyListRisk.Event == 2 ? RiskListConstants.SI : RiskListConstants.NO
            };
        }

        public static List<CompanyListRiskPerson> CreateCompanyListRiskPersonList(List<ListRiskPersonDTO> listRiskPersonDTO)
        {
            List<CompanyListRiskPerson> listRiskPeople = new List<CompanyListRiskPerson>();
            listRiskPersonDTO.ForEach(x => listRiskPeople.Add(CreateCompanyListRiskPerson(x)));
            return listRiskPeople;
        }

        public static List<ListRiskPersonDTO> CreateListRiskPersonDTOList(List<CompanyListRiskPerson> companyListRiskPerson)
        {
            List<ListRiskPersonDTO> listRiskPeople = new List<ListRiskPersonDTO>();
            companyListRiskPerson.ForEach(x => listRiskPeople.Add(CreateListRiskPersonDTO(x)));
            return listRiskPeople;
        }
        public static List<ListRiskPersonDTO> CreateListRiskPersonDTOList(CompanyListRiskModel companyListRiskModel)
        {
            List<ListRiskPersonDTO> listRiskPeople = new List<ListRiskPersonDTO>();

            companyListRiskModel.CompanyRiskListOwn.ForEach(x => listRiskPeople.Add(CreateListRiskPersonDTO(x)));
            companyListRiskModel.CompanyRiskListOfac.ForEach(x => listRiskPeople.Add(CreateListRiskPersonDTO(x)));
            companyListRiskModel.CompanyListRiskOnu.ForEach(x => listRiskPeople.Add(CreateListRiskPersonDTO(x)));

            return listRiskPeople;
        }
        public static List<ListRiskMatchDTO> CreateListRiskMatchDTOList(List<RiskListMatch> companyListRiskModel)
        {
            List<ListRiskMatchDTO> listRiskMatchDTOs = new List<ListRiskMatchDTO>();
            companyListRiskModel.ForEach(x => listRiskMatchDTOs.Add(CreateListRiskMatchDTO(x)));
            return listRiskMatchDTOs;
        }

        public static ListRiskMatchDTO CreateListRiskMatchDTO(RiskListMatch riskListMatch)
        {

            return new ListRiskMatchDTO
            {
                jModel = riskListMatch.jModel,
                listVersion = riskListMatch.listVersion,
                listType = riskListMatch.listType,
                registrationDate = riskListMatch.registrationDate,
                fileName = riskListMatch.fileName,
                Status = (Core.Application.UniquePersonListRiskApplicationServices.Enum.ProcessStatusEnum)riskListMatch.Status
            };
        }


        public static CompanyListRiskLoad CreateListRiskLoad(ListRiskLoadDTO listRiskLoadDTO)
        {
            if (listRiskLoadDTO != null)
            {
                return new CompanyListRiskLoad
                {
                    File = new File
                    {
                        Name = listRiskLoadDTO.FileName,
                    },
                    User = listRiskLoadDTO.User,
                    BeginDate = listRiskLoadDTO.BeginDate,
                    Description = listRiskLoadDTO.Description,
                    ProcessId = listRiskLoadDTO.ProcessId,
                    ListRiskId = listRiskLoadDTO.ListRisk.Id,
                    ListRiskDescription = listRiskLoadDTO.ListRisk.Description,
                    RiskListType = listRiskLoadDTO.ListRisk.RiskListType
                };
            }
            else
            {
                return null;
            }
        }

        public static CompanyListRisk CreateCompanyListRisk(ListRiskPersonDTO listRiskPersonDTO)
        {
            if (listRiskPersonDTO != null)
            {
                return new CompanyListRisk
                {
                    Id = listRiskPersonDTO.Id,
                    DocumentType = listRiskPersonDTO.DocumentType.Id,
                    DocumentTypeDescription = listRiskPersonDTO.DocumentType.Description,
                    DocumentNumber = listRiskPersonDTO.IdCardNo,
                    Name = listRiskPersonDTO.Name,
                    LastName = listRiskPersonDTO.LastName,
                    Alias = listRiskPersonDTO.AliasName,
                    BirthDate = listRiskPersonDTO.BirthDate,
                    AssignmentDate = DateTime.Now,
                    ListRiskType = listRiskPersonDTO.ListRisk.RiskListType,
                    CreatedUser = listRiskPersonDTO.CreateListUserName,
                    Event = listRiskPersonDTO.Event,
                    ProcessId = listRiskPersonDTO.ProcessId,
                    ListRiskDescription = listRiskPersonDTO.ListRisk.RiskListTypeDescription,
                    ListRiskId = listRiskPersonDTO.ListRisk.Id,
                    ListRiskTypeDescription = listRiskPersonDTO.ListRisk.Description
                };
            }
            else
            {
                return null;
            }
        }

        public static CompanyListRiskOfac CreateCompanyListRiskOfac(ListRiskPersonDTO listRiskPersonDTO)
        {
            if (listRiskPersonDTO != null)
            {
                return new CompanyListRiskOfac
                {
                    Id = listRiskPersonDTO.Id,
                    EntNum = listRiskPersonDTO.DataId,
                    SDNName = listRiskPersonDTO.Name,
                    AliasName = listRiskPersonDTO.AliasName,
                    BirthDate = listRiskPersonDTO.BirthDate,
                    AssignmentDate = DateTime.Now,
                    ListRiskType = listRiskPersonDTO.ListRisk.RiskListType,
                    CreatedUser = listRiskPersonDTO.CreateListUserName,
                    Event = listRiskPersonDTO.Event,
                    ProcessId = listRiskPersonDTO.ProcessId,
                    ListRiskDescription = listRiskPersonDTO.ListRisk.RiskListTypeDescription,
                    ListRiskId = listRiskPersonDTO.ListRisk.Id,
                    ListRiskTypeDescription = listRiskPersonDTO.ListRisk.Description
                };
            }
            else
            {
                return null;
            }
        }

        public static CompanyListRiskOnu CreateCompanyListRiskOnu(ListRiskPersonDTO listRiskPersonDTO)
        {
            if (listRiskPersonDTO != null)
            {
                return new CompanyListRiskOnu
                {
                    Id = listRiskPersonDTO.Id,
                    DataId = listRiskPersonDTO.DataId,
                    AssignmentDate = DateTime.Now,
                    ListRiskType = listRiskPersonDTO.ListRisk.RiskListType,
                    CreatedUser = listRiskPersonDTO.CreateListUserName,
                    Event = listRiskPersonDTO.Event,
                    ProcessId = listRiskPersonDTO.ProcessId,
                    ListRiskDescription = listRiskPersonDTO.ListRisk.RiskListTypeDescription,
                    ListRiskId = listRiskPersonDTO.ListRisk.Id,
                    ListRiskTypeDescription = listRiskPersonDTO.ListRisk.Description
                };
            }
            else
            {
                return null;
            }
        }
    }
}
