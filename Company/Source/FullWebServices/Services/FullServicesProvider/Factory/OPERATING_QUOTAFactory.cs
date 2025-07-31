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
    public class OPERATING_QUOTAFactory
    {

        #region data Members

        OPERATING_QUOTA_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public OPERATING_QUOTAFactory()
        {
            _dataObject = new OPERATING_QUOTA_ABM();
        }

        public OPERATING_QUOTAFactory(string Connection)
        {
            _dataObject = new OPERATING_QUOTA_ABM(Connection);
        }


        public OPERATING_QUOTAFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new OPERATING_QUOTA_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new OPERATING_QUOTA
        /// </summary>
        /// <param name="businessObject">OPERATING_QUOTA object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(OPERATING_QUOTA businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing OPERATING_QUOTA
        /// </summary>
        /// <param name="businessObject">OPERATING_QUOTA object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(OPERATING_QUOTA businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(OPERATING_QUOTA businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get OPERATING_QUOTA by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public OPERATING_QUOTA GetByPrimaryKey(OPERATING_QUOTAKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all OPERATING_QUOTAs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<OPERATING_QUOTA> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of OPERATING_QUOTA by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<OPERATING_QUOTA> GetAllBy(OPERATING_QUOTA.OPERATING_QUOTAFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete OPERATING_QUOTA by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(OPERATING_QUOTA.OPERATING_QUOTAFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<OPERATING_QUOTA> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (OPERATING_QUOTA mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "OPERATING_QUOTA";
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
