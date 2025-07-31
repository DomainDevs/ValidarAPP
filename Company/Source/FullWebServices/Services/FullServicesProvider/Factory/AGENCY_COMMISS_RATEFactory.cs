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
    public class AGENCY_COMMISS_RATEFactory
    {

        #region data Members

        AGENCY_COMMISS_RATE_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public AGENCY_COMMISS_RATEFactory()
        {
            _dataObject = new AGENCY_COMMISS_RATE_ABM();
        }

        public AGENCY_COMMISS_RATEFactory(string Connection)
        {
            _dataObject = new AGENCY_COMMISS_RATE_ABM(Connection);
        }


        public AGENCY_COMMISS_RATEFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new AGENCY_COMMISS_RATE_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new AGENCY_COMMISS_RATE
        /// </summary>
        /// <param name="businessObject">AGENCY_COMMISS_RATE object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(AGENCY_COMMISS_RATE businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing AGENCY_COMMISS_RATE
        /// </summary>
        /// <param name="businessObject">AGENCY_COMMISS_RATE object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(AGENCY_COMMISS_RATE businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(AGENCY_COMMISS_RATE businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get AGENCY_COMMISS_RATE by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public AGENCY_COMMISS_RATE GetByPrimaryKey(AGENCY_COMMISS_RATEKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all AGENCY_COMMISS_RATEs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<AGENCY_COMMISS_RATE> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of AGENCY_COMMISS_RATE by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<AGENCY_COMMISS_RATE> GetAllBy(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete AGENCY_COMMISS_RATE by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<AGENCY_COMMISS_RATE> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (AGENCY_COMMISS_RATE mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "AGENCY_COMMISS_RATE";
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
