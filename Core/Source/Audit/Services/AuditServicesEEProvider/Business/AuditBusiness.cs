using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using AUDSM = Sistran.Core.Application.ModelServices.Models.Audit;
namespace Sistran.Core.Application.AuditServices.EEProvider.Business
{
    public class AuditBusiness
    {
        public ErrorServiceModel ValidateAudit(AUDSM.AuditServiceModel auditServiceModel)
        {
            var ErrorServiceModel = new ErrorServiceModel();
            if (auditServiceModel.Package?.Id < 0)
            {
                ErrorServiceModel.ErrorTypeService = ErrorTypeService.BusinessFault;
                ErrorServiceModel.ErrorDescription.Add("El paquete es Obligatorio");
            }
            else
            {
                ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            }

            return ErrorServiceModel;
        }
    }
}
