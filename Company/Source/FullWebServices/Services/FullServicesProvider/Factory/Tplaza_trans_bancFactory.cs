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
    public class Tplaza_trans_bancFactory
    {

        #region data Members

        Tplaza_trans_banc_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tplaza_trans_bancFactory()
        {
            _dataObject = new Tplaza_trans_banc_ABM();
        }

        public Tplaza_trans_bancFactory(string Connection)
        {
            _dataObject = new Tplaza_trans_banc_ABM(Connection);
        }


        public Tplaza_trans_bancFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tplaza_trans_banc_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tplaza_trans_banc
        /// </summary>
        /// <param name="businessObject">Tplaza_trans_banc object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tplaza_trans_banc businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tplaza_trans_banc
        /// </summary>
        /// <param name="businessObject">Tplaza_trans_banc object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tplaza_trans_banc businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tplaza_trans_banc by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tplaza_trans_banc GetByPrimaryKey(Tplaza_trans_bancKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tplaza_trans_bancs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tplaza_trans_banc> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tplaza_trans_banc by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tplaza_trans_banc> GetAllBy(Tplaza_trans_banc.Tplaza_trans_bancFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tplaza_trans_bancKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tplaza_trans_banc by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tplaza_trans_banc.Tplaza_trans_bancFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tplaza_trans_banc> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tplaza_trans_banc mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tplaza_trans_banc";
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
