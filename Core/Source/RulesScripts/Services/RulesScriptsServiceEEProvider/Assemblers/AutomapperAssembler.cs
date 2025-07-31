using AutoMapper;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using models = Sistran.Core.Application.RulesScriptsServices.Models;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Automapper
        #region Nodes
        /// <summary>
        /// Creates the map node.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapNode()
        {
            var config = MapperCache.GetMapper< Entities.Node, Node>(cfg =>
            {
                cfg.CreateMap<Entities.Node,Node>();
             
            });
            return config;
        }
        #endregion Nodes
        #region NodeQuestion
        /// <summary>
        /// Creates the map node question.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapNodeQuestion()
        {
            var config = MapperCache.GetMapper<Entities.NodeQuestion, NodeQuestion>(cfg =>
            {
                cfg.CreateMap<Entities.NodeQuestion, NodeQuestion>();

            });
            return config;
        }
        #endregion NodeQuestion

        #region Edge
        /// <summary>
        /// Creates the map node question.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapEdge()
        {
            var config = MapperCache.GetMapper<Entities.Edge, Edge>(cfg =>
            {
                cfg.CreateMap<Entities.Edge, Edge>();

            });
            return config;
        }
        #endregion Edge

        #region Script
        /// <summary>
        /// Creates the map Script.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapScript()
        {
            var config = MapperCache.GetMapper<Entities.Script, models.Script>(cfg =>
            {
                cfg.CreateMap<Entities.Script, models.Script>();

            });
            return config;
        }
        #endregion Script
        #region Question

        public static IMapper CreateMapQuestion()
        {
            var config = MapperCache.GetMapper<Entities.Question, models.Question>(cfg =>
            {
                cfg.CreateMap<Entities.Question, models.Question>();

            });
            return config;
        }
        #endregion Question
        #endregion Automapper

    }
}
