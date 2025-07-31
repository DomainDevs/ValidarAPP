using ApplicationServer.AbstractQuee;
using Principal = ApplicationServer.UnderwritingBrokerService;
using Replica = ApplicationServer.UnderwritingBrokerServiceReplica;
using System;
using System.Linq;
using Utiles.Extentions;
using Utiles.Log;
using ApplicationServer.Enums;
namespace ApplicationServer.ConcreteQuee
{
    public class UpdatePendingOperationQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)         
        {
            WrapperObjectQuee json = GetObjectToProcess((string)body);
            string businessCollection = json.JsonToProcess;

            if (businessCollection != null)
            {
                string[] objectsToSave = businessCollection.Split((char)007);
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
                                    pendingOperationId = ub.UpdatePendingOperation(businessCollection);
                                }
                                rowToUpdate = $"{objectsToSave[2]}{(char)007}{pendingOperationId}{(char)007}{objectsToSave.Last().Trim()}";
                                break;
                            case MassiveLoadTypes.CollectiveEmissionRow:
                                using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerReplicaService"))
                                {
                                    pendingOperationId = ub.UpdatePendingOperation(businessCollection);
                                }
                                rowToUpdate = $"{objectsToSave[1]}{(char)007}{pendingOperationId}{(char)007}{objectsToSave.Last().Trim()}";
                                break;
                            case MassiveLoadTypes.MassiveCancellationRow:
                                using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerReplicaService"))
                                {
                                    pendingOperationId = ub.UpdatePendingOperation(businessCollection);
                                }
                                rowToUpdate = $"{objectsToSave[2]}{(char)007}{pendingOperationId}{(char)007}{objectsToSave.Last().Trim()}";
                                break;
                        }
                        using (Principal.UnderwritingBrokerServiceClient ub = new Principal.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService"))
                        {
                            if (objectsToSave.Last().Trim() != MassiveLoadTypes.CollectiveEmissionRow)
                            {
                                ub.UpdateMassiveEmissionRows(rowToUpdate, string.Empty);
                            }
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
                        ub.UpdatePendingOperation(businessCollection);
                    }
                }

            }
        }
    }
}