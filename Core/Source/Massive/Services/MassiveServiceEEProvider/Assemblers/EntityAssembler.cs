using Sistran.Core.Application.MassiveServices.Models;
using System;
using System.Linq;
using MSVEN = Sistran.Core.Application.Massive.Entities;

namespace Sistran.Core.Application.MassiveServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        internal static MSVEN.MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad)
        {
            MSVEN.MassiveLoad entityMassiveLoad = new MSVEN.MassiveLoad
            {
                Description = massiveLoad.Description,
                FileName = massiveLoad.File.Name,
                UserId = massiveLoad.User.UserId,
                StatusId = (int)massiveLoad.Status,
                HasError = false,
                TotalRows = massiveLoad.TotalRows,
                LoadTypeId = massiveLoad.LoadType.Id
            };

            return entityMassiveLoad;
        }
    }
}