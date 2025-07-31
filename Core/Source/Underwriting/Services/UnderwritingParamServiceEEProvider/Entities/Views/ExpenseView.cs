// -----------------------------------------------------------------------
// <copyright file="ExpenseView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class ExpenseView : BusinessView
    {
        public BusinessCollection Components
        {
            get
            {
                return this["Component"];
            }
        }

        public BusinessCollection ExpenseComponents
        {
            get
            {
                return this["ExpenseComponent"];
            }
        }

        public BusinessCollection RateTypes
        {
            get
            {
                return this["RateType"];
            }
        }

        public BusinessCollection RulesSet
        {
            get
            {
                return this["RuleSet"];
            }
        }

    }
}
