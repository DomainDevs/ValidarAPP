using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using CLMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ParticipantDAO
    {
        public Participant CreateParticipant(Participant participant)
        {
            CLMEN.Participant entityparticipant = EntityAssembler.CreateParticipant(participant);
            return ModelAssembler.CreateParticipant((CLMEN.Participant)DataFacadeManager.Insert(entityparticipant));
        }

        public Participant GetParticipantByParticipantId(int participantId)
        {
            PrimaryKey primaryKey = CLMEN.Participant.CreatePrimaryKey(participantId);
            CLMEN.Participant entityParticipant = (CLMEN.Participant)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateParticipant(entityParticipant);
        }

        public Participant UpdateParticipant(Participant participant)
        {
            bool isUpdated = DataFacadeManager.Update(EntityAssembler.CreateParticipant(participant));

            if (isUpdated)
            {
                return participant;
            }
            else
            {
                throw new Exception();
            }
        }

        public List<Participant> GetParticipantsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(CLMEN.Participant.Properties.ParticipantCode, "p");
                        filter.Equal();
                        filter.Constant(identificationNumber);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(CLMEN.Participant.Properties.DocumentNumber, "p");
                        filter.Like();
                        filter.Constant(identificationNumber.ToString() + '%');
                        break;
                }
            }
            else
            {
                filter.Property(CLMEN.Participant.Properties.Fullname, "p");
                filter.Like();
                filter.Constant(description + "%");
            }

            return ModelAssembler.CreateParticipants(DataFacadeManager.GetObjects(typeof(CLMEN.Participant), filter.GetPredicate(), null, 1, 50, false));
        }
    }
}
