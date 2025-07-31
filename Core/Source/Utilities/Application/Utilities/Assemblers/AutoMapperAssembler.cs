using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using SCREN = Sistran.Core.Application.Script.Entities;
using Rules = Sistran.Core.Framework.Rules;
namespace Sistran.Core.Application.Utilities.Assemblers
{
    class AutoMapperAssembler
    {
        #region Conceptos
        public static IMapper CreateMapConcept()
        {
            IMapper config = MapperCache.GetMapper<SCREN.Concept, Sistran.Core.Framework.Rules.Concept>(cfg =>
           {
               cfg.CreateMap<SCREN.Concept, Sistran.Core.Framework.Rules.Concept>()
               .ConstructUsing(source => new Rules.Concept(source.ConceptId, source.ConceptName, GetType(source),  source.IsStatic, source.EntityId))
                .ForMember(d => d.Id, o => o.MapFrom(c => c.ConceptId))
                .ForMember(d => d.Name, o => o.MapFrom(c => c.ConceptName))
                .ForMember(d => d.Type, o => o.MapFrom(c => GetType(c)));
           });
            return config;
        }
        public static Type GetType(SCREN.Concept entityConcept)
        {
            RuleBridgeDelegate ruleBridgeDelegate = new RuleBridgeDelegate();
            Type type = ruleBridgeDelegate.GetConceptType(entityConcept);

            if (type.IsValueType && (entityConcept.IsNullable || !entityConcept.IsStatic))
            {
                type = typeof(Nullable<>).MakeGenericType(type);
            }
            return type;
        }
        #endregion Conceptos
    }
}
