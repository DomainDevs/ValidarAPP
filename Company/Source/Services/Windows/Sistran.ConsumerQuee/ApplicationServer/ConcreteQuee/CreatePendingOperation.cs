using Utiles.Extentions;
using System;
using System.Linq;
using Principal = ApplicationServer.UnderwritingBrokerService;
using Replica = ApplicationServer.UnderwritingBrokerServiceReplica;
using ApplicationServer.AbstractQuee;
using ApplicationServer.Enums;
using ApplicationServer.Helpers;
using ApplicationServer;

namespace ApplicationServer.ConcreteQuee
{
    public class CreatePendingOperationQuee : TemplateQuee
    {
        public const char Separator = (char)007;
        protected override void ActionQueeToExcecute(Object body)
        {
            string businessCollection = (string)body;

            if (businessCollection != null)
            {
                WrapperObjectQuee json = GetObjectToProcess((string)body);
                string[] objectsToSave = json.JsonToProcess.Split((char)007);
                if (objectsToSave.Length > 1)
                {
                    try
                    {
                        int pendingOperationId = 0;
                        string rowToUpdate = string.Empty;
                        switch (objectsToSave.Last().Trim())
                        {
                            case MassiveLoadTypes.MassiveEmissionRow:
                            case MassiveLoadTypes.MassiveRenewalRow:
                                
                                using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerReplicaService"))
                                {
                                    pendingOperationId = ub.CreatePendingOperationWithParent($"{objectsToSave[0]}{(char)007}{objectsToSave[1]}");
                                }
                                rowToUpdate = $"{objectsToSave[2]}{(char)007}{pendingOperationId}{(char)007}{objectsToSave.Last().Trim()}";
                                break;
                            case MassiveLoadTypes.CollectiveEmissionRow:
                                using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerReplicaService"))
                                {
                                    pendingOperationId = ub.CreatePendingOperation(objectsToSave[0]);
                                }
                                rowToUpdate = $"{objectsToSave[1]}{(char)007}{pendingOperationId}{(char)007}{objectsToSave.Last().Trim()}";
                                break;
                            case MassiveLoadTypes.MassiveCancellationRow:
                                using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerReplicaService"))
                                {
                                    pendingOperationId = ub.CreatePendingOperation(objectsToSave[0]);
                                }
                                rowToUpdate = $"{objectsToSave[1]}{(char)007}{pendingOperationId}{(char)007}{objectsToSave.Last().Trim()}";
                                break;
                        }
                        using (Principal.UnderwritingBrokerServiceClient ub = new Principal.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService"))
                        {
                            ub.UpdateMassiveEmissionRows(rowToUpdate, string.Empty);
                        }
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            var error = "Error en el servicio de manejo de colas " + e;
                            string rowToUpdate = string.Empty;
                            switch (objectsToSave.Last().Trim())
                            {
                                case MassiveLoadTypes.MassiveEmissionRow:
                                case MassiveLoadTypes.MassiveRenewalRow:
                                    rowToUpdate = $"{objectsToSave[2]}{(char)007}{objectsToSave.Last().Trim()}";
                                    break;
                               
                                case MassiveLoadTypes.CollectiveEmissionRow:
                                case MassiveLoadTypes.MassiveCancellationRow:
                                    rowToUpdate = $"{objectsToSave[1]}{(char)007}{objectsToSave.Last().Trim()}";
                                    break;
                            }
                            using (Principal.UnderwritingBrokerServiceClient ub = new Principal.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService"))
                            {
                                ub.UpdateMassiveEmissionRows(rowToUpdate, error);
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionQueActions(body, e);
                        }
                    }
                }
                else
                {
                    using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerReplicaService"))
                    {
                        ub.CreatePendingOperation(objectsToSave[0]);
                    }
                }
                
            }

        }
       
        protected override void ExceptionQueActions(Object body, Exception e)
        {

            WrapperObjectQuee jsonRetry = GetObjectToProcess((string)body);
            RemoveMessage = WouldRemoveMessage(jsonRetry);
            if (RemoveMessage)
            {
                QueueHelper.PutOnQueueJsonByQueue(jsonRetry.GetJson(), PrincipalQueeName);
            }
            else
            {
                CreateEventLog(body, e);
            }
        }
    }
}