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
    public class Mpersona_requiere_sarlaftFactory
    {

        #region data Members

        Mpersona_requiere_sarlaft_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mpersona_requiere_sarlaftFactory()
        {
            _dataObject = new Mpersona_requiere_sarlaft_ABM();
        }

        public Mpersona_requiere_sarlaftFactory(string Connection)
        {
            _dataObject = new Mpersona_requiere_sarlaft_ABM(Connection);
        }


        public Mpersona_requiere_sarlaftFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpersona_requiere_sarlaft_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpersona_requiere_sarlaft
        /// </summary>
        /// <param name="businessObject">Mpersona_requiere_sarlaft object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpersona_requiere_sarlaft businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpersona_requiere_sarlaft
        /// </summary>
        /// <param name="businessObject">Mpersona_requiere_sarlaft object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpersona_requiere_sarlaft businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Mpersona_requiere_sarlaft businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mpersona_requiere_sarlaft by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpersona_requiere_sarlaft GetByPrimaryKey(Mpersona_requiere_sarlaftKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpersona_requiere_sarlafts
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpersona_requiere_sarlaft> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpersona_requiere_sarlaft by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpersona_requiere_sarlaft> GetAllBy(Mpersona_requiere_sarlaft.Mpersona_requiere_sarlaftFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete Mpersona_requiere_sarlaft by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpersona_requiere_sarlaft.Mpersona_requiere_sarlaftFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpersona_requiere_sarlaft> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Mpersona_requiere_sarlaft mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Mpersona_requiere_sarlaft";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                        tMessage.Message = Update(mp).ToString();
                    else if (mp.State.Equals('D'))
                        tMessage.Message = Delete(mp).ToString();
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
