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
    public class PAYMENT_METHOD_CARDFactory
    {

        #region data Members

        PAYMENT_METHOD_CARD_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public PAYMENT_METHOD_CARDFactory()
        {
            _dataObject = new PAYMENT_METHOD_CARD_ABM();
        }

        public PAYMENT_METHOD_CARDFactory(string Connection)
        {
            _dataObject = new PAYMENT_METHOD_CARD_ABM(Connection);
        }


        public PAYMENT_METHOD_CARDFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new PAYMENT_METHOD_CARD_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new PAYMENT_METHOD_CARD
        /// </summary>
        /// <param name="businessObject">PAYMENT_METHOD_CARD object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(PAYMENT_METHOD_CARD businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing PAYMENT_METHOD_CARD
        /// </summary>
        /// <param name="businessObject">PAYMENT_METHOD_CARD object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(PAYMENT_METHOD_CARD businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(PAYMENT_METHOD_CARD businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get PAYMENT_METHOD_CARD by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public PAYMENT_METHOD_CARD GetByPrimaryKey(PAYMENT_METHOD_CARDKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all PAYMENT_METHOD_CARDs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<PAYMENT_METHOD_CARD> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of PAYMENT_METHOD_CARD by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<PAYMENT_METHOD_CARD> GetAllBy(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete PAYMENT_METHOD_CARD by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(PAYMENT_METHOD_CARD.PAYMENT_METHOD_CARDFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<PAYMENT_METHOD_CARD> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (PAYMENT_METHOD_CARD mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "PAYMENT_METHOD_CARD";
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
