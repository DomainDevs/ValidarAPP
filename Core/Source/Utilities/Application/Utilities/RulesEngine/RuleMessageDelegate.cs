using System;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Rules.Engine;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleMessageDelegate : RuleMessageProvider
    {
        /// <summary>
        /// Mostrar Mensaje
        /// </summary>
        /// <param name="message">Mensaje</param>
        public override void ShowMessage(string message)
        {
            throw new ValidationException(message);
        }
    }
}