using System;
using System.Configuration;
using Sistran.Core.Framework.Reflection;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Engine;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleFunctionDelegate : RuleFunctionProvider
    {
        public override void ExecuteFunction(string functionName, params object[] arguments)
        {
            WorkingMemory workingMemory = RuleExecutionContext.Current.WorkingMemory;
            Vocabulary vocabulary = RuleExecutionContext.Current.Vocabulary;
            Facade facade = RuleExecutionContext.Current.Facade;

            RuleExecutionContext.Current.RuleEngine.WorkingMemoryTransformation.TransformFromWorkingMemory(workingMemory, vocabulary, facade);

            RulesFunctionSection rulesFunctionSection = (RulesFunctionSection)ConfigurationManager.GetSection("sistran.core.application.underwriting/functionRuleToMethod");
            RuleFunctionElement ruleFunctionElement = rulesFunctionSection.functionRuleToMethods[functionName];

            if (ruleFunctionElement == null)
            {
                throw new Exception(string.Format("Funcion {0} Not Found in ApplicationServerService.exe.config", functionName));
            }
            else
            {
                Type type = Type.GetType(ruleFunctionElement.AssemblyName);
                object instanceFunction = Activator.CreateInstance(type);
                MethodModel methodModel = TypeModel.GetModel(type).Methods[ruleFunctionElement.MethodName];

                if (methodModel == null)
                {
                    throw new Exception(string.Format("Funcion {0} Not Found in {1}", functionName, ruleFunctionElement.AssemblyName));
                }
                else
                {
                    methodModel.Invoke(instanceFunction, facade);
                    RuleExecutionContext.Current.RuleEngine.WorkingMemoryTransformation.TransformToWorkingMemory(workingMemory, vocabulary, facade);
                }
            }
        }
    }
}