using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Scripts.Entities;
using System;
using System.CodeDom;
using System.Collections;
using PARAMEN =Sistran.Core.Application.Parameters.Entities;
using RuleEntities = Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    public static class RuleHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static System.CodeDom.CodeTypeReference GetConceptTypeReference(SCREN.Concept c)
        {
            bool isNullable = (c.IsNullable || !c.IsStatic);

            switch (c.ConceptTypeCode)
            {
                case (int)ConceptType.Types.List:
                    SCREN.ListConcept lc = ListConceptDAO.FindListConcept(c.ConceptId, c.EntityId);
                    if (lc == null)
                    {
                        throw new Exception(string.Format("No se encontró la lista para el concepto: {0}|{1}.", c.EntityId, c.ConceptId));
                    }

                    if (lc.ListEntityCode == 2) // Lista Booleana
                    {
                        if (isNullable)
                        {
                            return new System.CodeDom.CodeTypeReference("bool?");
                        }
                        return new System.CodeDom.CodeTypeReference(typeof(bool));
                    }

                    if (isNullable)
                    {
                        return new System.CodeDom.CodeTypeReference("int?");
                    }

                    return new System.CodeDom.CodeTypeReference(typeof(int));

                // no hay break a proposito
                case (int)ConceptType.Types.Range:
                case (int)ConceptType.Types.Reference:
                    if (isNullable)
                    {
                        return new System.CodeDom.CodeTypeReference("int?");
                    }

                    return new System.CodeDom.CodeTypeReference(typeof(int));

                case (int)ConceptType.Types.Basic:
                    SCREN.BasicConcept bc = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(c.ConceptId, c.EntityId);
                    switch ((BasicType.Types)bc.BasicTypeCode)
                    {
                        case BasicType.Types.Date:
                            if (isNullable)
                            {
                                return new System.CodeDom.CodeTypeReference("DateTime?");
                            }

                            return new System.CodeDom.CodeTypeReference(typeof(DateTime));

                        case BasicType.Types.Decimal:
                            if (isNullable)
                            {
                                return new System.CodeDom.CodeTypeReference("decimal?");
                            }

                            return new System.CodeDom.CodeTypeReference(typeof(decimal));

                        case BasicType.Types.Number:
                            if (isNullable)
                            {
                                return new System.CodeDom.CodeTypeReference("int?");
                            }

                            return new System.CodeDom.CodeTypeReference(typeof(int));

                        case BasicType.Types.Text:
                            return new System.CodeDom.CodeTypeReference(typeof(string));


                        default:
                            throw new Exception("Basic type not supported: " + bc.BasicTypeCode.ToString());
                    }

                default:
                    throw new Exception("Concept type not supported: " + c.ConceptTypeCode.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paremeters"></param>
        /// <returns></returns>
        public static CodeParameterDeclarationExpression GetParameterExpression(PARAMEN.Entity entity, IDictionary paremeters)
        {
            if (!paremeters.Contains(entity.EntityName))
            {
                RuleEntities.Package pkg = PackageDAO.FindPackage(entity.PackageId);

                string typeString = pkg.Namespace + ".Entities." + entity.EntityName +
                    ", " + pkg.Namespace + ".Entities";

                CodeParameterDeclarationExpression paramExp =
                    new CodeParameterDeclarationExpression(typeString, "prm" + entity.EntityName);

                paremeters.Add(entity.EntityName, paramExp);
                return paramExp;
            }

            object obj = paremeters[entity.EntityName];

            return (CodeParameterDeclarationExpression)obj;
        }

        /// <summary>
        /// Concepto en la  condición o accion
        /// </summary>
        /// <param name="conceptExpr"></param>
        /// <returns></returns>
        public static CodeExpression GetConceptExpression(Models.Concept conceptExpr, IDictionary parameters, bool conceptLeft)
        {
            SCREN.Concept conceptEntity = ConceptDAO.GetConceptByConceptIdEntityId(conceptExpr.ConceptId, conceptExpr.EntityId);
            PARAMEN.Entity entity = EntityDAO.FindEntity(conceptExpr.EntityId);

            CodeParameterDeclarationExpression paramExp = RuleHelper.GetParameterExpression(entity, parameters);

            CodeArgumentReferenceExpression arg = new CodeArgumentReferenceExpression(paramExp.Name);

            // si es un concepto estatico
            if (conceptEntity.IsStatic)
            {
                return new CodePropertyReferenceExpression(arg, conceptEntity.ConceptName);
            }

            // es un concepto dinámico
            CodeTypeReference t = RuleHelper.GetConceptTypeReference(conceptEntity);
            CodeMethodInvokeExpression mi = null;

            if (conceptLeft)
            {
                //jonathan
                return arg;
                //return new CodeCastExpression(t, arg);
                //mi = new CodeMethodInvokeExpression( new CodeCastExpression(typeof(IDynamicConceptContainer), arg), "SetDynamicConcept", 
                //new CodeExpression[] { new CodePrimitiveExpression(conceptExpr.ConceptId) });
                //return mi;
            }
            else
            {

                mi = new CodeMethodInvokeExpression(
                new CodeCastExpression(typeof(IDynamicConceptContainer),arg), "GetDynamicConcept",
                new CodeExpression[] { new CodePrimitiveExpression(conceptExpr.ConceptId) });

                return new CodeCastExpression(t, mi);

            }


            //if (t == null)
            //{
            //    return mi;
            //}

            

            //return mi;
        }
    }
}
