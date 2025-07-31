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
    public class PERSON_INTEREST_GROUPFactory
    {

        #region data Members

        PERSON_INTEREST_GROUP_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public PERSON_INTEREST_GROUPFactory()
        {
            _dataObject = new PERSON_INTEREST_GROUP_ABM();
        }

        public PERSON_INTEREST_GROUPFactory(string Connection)
        {
            _dataObject = new PERSON_INTEREST_GROUP_ABM(Connection);
        }


        public PERSON_INTEREST_GROUPFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new PERSON_INTEREST_GROUP_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new PERSON_INTEREST_GROUP
        /// </summary>
        /// <param name="businessObject">PERSON_INTEREST_GROUP object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(PERSON_INTEREST_GROUP businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing PERSON_INTEREST_GROUP
        /// </summary>
        /// <param name="businessObject">PERSON_INTEREST_GROUP object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(PERSON_INTEREST_GROUP businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(PERSON_INTEREST_GROUP businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get PERSON_INTEREST_GROUP by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public PERSON_INTEREST_GROUP GetByPrimaryKey(PERSON_INTEREST_GROUPKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all PERSON_INTEREST_GROUPs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<PERSON_INTEREST_GROUP> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of PERSON_INTEREST_GROUP by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<PERSON_INTEREST_GROUP> GetAllBy(PERSON_INTEREST_GROUP.PERSON_INTEREST_GROUPFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete PERSON_INTEREST_GROUP by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(PERSON_INTEREST_GROUP.PERSON_INTEREST_GROUPFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<PERSON_INTEREST_GROUP> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (PERSON_INTEREST_GROUP mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "PERSON_INTEREST_GROUP";
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
