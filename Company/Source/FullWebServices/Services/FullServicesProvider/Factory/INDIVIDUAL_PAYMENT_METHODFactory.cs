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
    public class INDIVIDUAL_PAYMENT_METHODFactory
    {

        #region data Members

        INDIVIDUAL_PAYMENT_METHOD_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public INDIVIDUAL_PAYMENT_METHODFactory()
        {
            _dataObject = new INDIVIDUAL_PAYMENT_METHOD_ABM();
        }

        public INDIVIDUAL_PAYMENT_METHODFactory(string Connection)
        {
            _dataObject = new INDIVIDUAL_PAYMENT_METHOD_ABM(Connection);
        }


        public INDIVIDUAL_PAYMENT_METHODFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new INDIVIDUAL_PAYMENT_METHOD_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new INDIVIDUAL_PAYMENT_METHOD
        /// </summary>
        /// <param name="businessObject">INDIVIDUAL_PAYMENT_METHOD object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(INDIVIDUAL_PAYMENT_METHOD businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing INDIVIDUAL_PAYMENT_METHOD
        /// </summary>
        /// <param name="businessObject">INDIVIDUAL_PAYMENT_METHOD object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(INDIVIDUAL_PAYMENT_METHOD businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(INDIVIDUAL_PAYMENT_METHOD businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get INDIVIDUAL_PAYMENT_METHOD by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public INDIVIDUAL_PAYMENT_METHOD GetByPrimaryKey(INDIVIDUAL_PAYMENT_METHODKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all INDIVIDUAL_PAYMENT_METHODs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<INDIVIDUAL_PAYMENT_METHOD> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of INDIVIDUAL_PAYMENT_METHOD by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<INDIVIDUAL_PAYMENT_METHOD> GetAllBy(INDIVIDUAL_PAYMENT_METHOD.INDIVIDUAL_PAYMENT_METHODFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete INDIVIDUAL_PAYMENT_METHOD by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(INDIVIDUAL_PAYMENT_METHOD.INDIVIDUAL_PAYMENT_METHODFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<INDIVIDUAL_PAYMENT_METHOD> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (INDIVIDUAL_PAYMENT_METHOD mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "INDIVIDUAL_PAYMENT_METHOD";
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
