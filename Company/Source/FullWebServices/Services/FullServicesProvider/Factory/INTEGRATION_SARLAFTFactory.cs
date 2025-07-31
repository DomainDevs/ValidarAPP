using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServices.Models.DataLayer;
using Sistran.Co.Previsora.Application.FullServices.Models;


namespace Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer
{
    public class INTEGRATION_SARLAFTFactory
    {

        #region data Members

        INTEGRATION_SARLAFT_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public INTEGRATION_SARLAFTFactory()
        {
            _dataObject = new INTEGRATION_SARLAFT_ABM();
        }

        public INTEGRATION_SARLAFTFactory(string Connection)
        {
            _dataObject = new INTEGRATION_SARLAFT_ABM(Connection);
        }


        public INTEGRATION_SARLAFTFactory(string Connection, string userId, int AppId)
        {
            _dataObject = new INTEGRATION_SARLAFT_ABM(Connection, userId, AppId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new INTEGRATION_SARLAFT
        /// </summary>
        /// <param name="businessObject">INTEGRATION_SARLAFT object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(INTEGRATION_SARLAFT businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing INTEGRATION_SARLAFT
        /// </summary>
        /// <param name="businessObject">INTEGRATION_SARLAFT object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(INTEGRATION_SARLAFT businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(INTEGRATION_SARLAFT businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get INTEGRATION_SARLAFT by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public INTEGRATION_SARLAFT GetByPrimaryKey(INTEGRATION_SARLAFTKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all INTEGRATION_SARLAFTs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<INTEGRATION_SARLAFT> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of INTEGRATION_SARLAFT by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<INTEGRATION_SARLAFT> GetAllBy(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete INTEGRATION_SARLAFT by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<INTEGRATION_SARLAFT> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (INTEGRATION_SARLAFT mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "INTEGRATION_SARLAFT";
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
