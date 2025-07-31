using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using MSVEN = Sistran.Core.Application.Massive.Entities;

namespace Sistran.Core.Application.MassiveServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        internal static LoadType CreateLoadType(MSVEN.LoadType entityLoadType)
        {
            return new LoadType
            {
                Id = entityLoadType.LoadTypeCode,
                Description = entityLoadType.Description,
                SmallDescription = entityLoadType.SmallDescription,
                ProcessType = (MassiveProcessType)entityLoadType.ProcessTypeCode
            };
        }

        internal static List<LoadType> CreateLoadTypes(BusinessCollection businessCollection)
        {
            List<LoadType> loadTypes = new List<LoadType>();

            foreach (MSVEN.LoadType entityLoadType in businessCollection)
            {
                loadTypes.Add(ModelAssembler.CreateLoadType(entityLoadType));
            }

            return loadTypes;
        }

        internal static MassiveLoad CreateMassiveLoad(MSVEN.MassiveLoad entityMassiveLoad)
        {
            MassiveLoad massiveLoad = new MassiveLoad
            {
                Id = entityMassiveLoad.Id,
                LoadType = new LoadType()
                {
                    Id = entityMassiveLoad.LoadTypeId.Value
                },
                File = new File()
                {
                    Name = entityMassiveLoad.FileName
                },
                User = new UniqueUserServices.Models.User
                {
                    UserId = entityMassiveLoad.UserId
                },
                Description = entityMassiveLoad.Description,
                Status = (MassiveLoadStatus)entityMassiveLoad.StatusId,
                HasError = entityMassiveLoad.HasError,
                ErrorDescription = entityMassiveLoad.ErrorDescription,
                TotalRows = entityMassiveLoad.TotalRows.GetValueOrDefault()
            };

            if (massiveLoad.ErrorDescription != null)
            {
                string[] errorMessages = massiveLoad.ErrorDescription.Split('(');
                massiveLoad.ErrorDescription = errorMessages[0];
            }

            return massiveLoad;
        }

        internal static List<MassiveLoad> CreateMassiveLoads(BusinessCollection businessCollection)
        {
            List<MassiveLoad> massiveLoads = new List<MassiveLoad>();

            foreach (MSVEN.MassiveLoad entityMassiveLoad in businessCollection)
            {
                massiveLoads.Add(ModelAssembler.CreateMassiveLoad(entityMassiveLoad));
            }

            return massiveLoads;
        }
        
        internal static List<Printing> CreatePrintings(BusinessCollection businessCollection)
        {
            List<Printing> printings = new List<Printing>();

            foreach (MSVEN.Printing entityPrinting in businessCollection)
            {
                printings.Add(CreatePrinting(entityPrinting));
            }

            return printings;
        }

        internal static Printing CreatePrinting(MSVEN.Printing entityPrinting)
        {
            Printing printing = new Printing()
            {
                Id = entityPrinting.Id,
                PrintingTypeId = entityPrinting.PrintingTypeId,
                KeyId = entityPrinting.KeyId,
                Total = entityPrinting.Total,
                BeginDate = entityPrinting.BeginDate,
                UserId = entityPrinting.UserId,
            };

            if (!string.IsNullOrEmpty(entityPrinting.UrlFile))
            {
                printing.UrlFile = entityPrinting.UrlFile;
            }

            //if (entityPrinting. HasError.HasValue)
            //{
            //    printing.HasError = entityPrinting.HasError.Value;
            //    printing.ErrorMessage = entityPrinting.ErrorMessage;
            //}

            if (entityPrinting.FinishDate.HasValue)
            {
                printing.FinishDate = entityPrinting.FinishDate.Value;
            }

            return printing;
        }
    }
}