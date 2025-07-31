using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServices.Models.DataLayer;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;


namespace Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer
{
    public class Tsarlaft_motivo_exoneraFactory
    {

        #region data Members

        Tsarlaft_motivo_exonera_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tsarlaft_motivo_exoneraFactory()
        {
            _dataObject = new Tsarlaft_motivo_exonera_ABM();
        }

        public Tsarlaft_motivo_exoneraFactory(string Connection)
        {
            _dataObject = new Tsarlaft_motivo_exonera_ABM(Connection);
        }


        public Tsarlaft_motivo_exoneraFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tsarlaft_motivo_exonera_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tsarlaft_motivo_exonera
        /// </summary>
        /// <param name="businessObject">Tsarlaft_motivo_exonera object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tsarlaft_motivo_exonera businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tsarlaft_motivo_exonera
        /// </summary>
        /// <param name="businessObject">Tsarlaft_motivo_exonera object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tsarlaft_motivo_exonera businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tsarlaft_motivo_exonera by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tsarlaft_motivo_exonera GetByPrimaryKey(Tsarlaft_motivo_exoneraKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tsarlaft_motivo_exoneras
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tsarlaft_motivo_exonera> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tsarlaft_motivo_exonera by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tsarlaft_motivo_exonera> GetAllBy(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tsarlaft_motivo_exoneraKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tsarlaft_motivo_exonera by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tsarlaft_motivo_exonera.Tsarlaft_motivo_exoneraFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tsarlaft_motivo_exonera> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tsarlaft_motivo_exonera mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tsarlaft_motivo_exonera";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                        tMessage.Message = Update(mp).ToString();

                }
                catch (SupException ex)
                {
                    tMessage.Message = ex.Source;
                    continue;
                }
                finally
                {
                    ListTableMessage.Add(tMessage);
                }
                
            }
            return ListTableMessage;
        }
        #endregion

    }
}
