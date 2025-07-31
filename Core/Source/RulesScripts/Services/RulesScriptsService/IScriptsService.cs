using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.RulesScriptsServices
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IScriptsService
    {
        /// <summary>
        /// obtiene todos los guiones 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Script> GetScripts();

        /// <summary>
        /// obtiene todas las preguntas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Question> GetQuestions();

        /// <summary>
        /// obtiene todas las preguntas por nivel
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        [OperationContract]
        List<Question> GetQuestionsByLevel(Enums.Level levels);

        /// <summary>
        /// obtiene las pregutnas y respuestas asociadas al guion
        /// </summary>
        /// <param name="ScriptId">id del guion </param>
        /// <returns></returns>
        [OperationContract]
        ScriptComposite GetScriptComposite(int ScriptId);

        /// <summary>
        /// Crea un guion completo
        /// </summary>
        /// <param name="ScriptComposite">compone todo un guion com preguntas y respuestas</param>
        /// <returns></returns>
        [OperationContract]
        ScriptComposite CreateScriptComposite(ScriptComposite ScriptComposite);

        /// <summary>
        /// Obtiene los datos de la pregunta
        /// </summary>
        /// <param name="IdQuestion">id de la pregunta</param>
        /// <returns></returns>
        [OperationContract]
        Question GetQuestion(int IdQuestion);

        /// <summary>
        /// obtiene los guiones por nivel 
        /// </summary>
        /// <param name="levelId">id del nivel</param>
        /// <returns></returns>
        [OperationContract]
        List<Script> GetScriptByLevelId(int? module, int? level, string Name, string Question);

        /// <summary>
        /// obtiene los giones por diferentes niveles
        /// </summary>
        /// <param name="level">listado de niveles</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Script> GetScriptsByLevels(List<Models.Level> level);

        /// <summary>
        /// Elimina un guion 
        /// </summary>
        /// <param name="ScriptId"></param>
        [OperationContract]
        void DeleteScript(int ScriptId);

        /// <summary>
        /// Gets the scripts by ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        [OperationContract]
        List<Script> GetScriptsByIds(List<int> ids);

        /// <summary>
        /// Genera el reporte de guiones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns></returns>
        [OperationContract]
        string GenerateScriptsReport(string fileName);

        [OperationContract]
        List<Models.Question> GetQuestionsByLevelAutomaticQuota(Enums.LevelAutomaticQuota levels);
    }
}
