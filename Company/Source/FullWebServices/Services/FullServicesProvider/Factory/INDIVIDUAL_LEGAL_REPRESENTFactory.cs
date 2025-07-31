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
    public class INDIVIDUAL_LEGAL_REPRESENTFactory
    {

        #region data Members

        INDIVIDUAL_LEGAL_REPRESENT_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public INDIVIDUAL_LEGAL_REPRESENTFactory()
        {
            _dataObject = new INDIVIDUAL_LEGAL_REPRESENT_ABM();
        }

        public INDIVIDUAL_LEGAL_REPRESENTFactory(string Connection)
        {
            _dataObject = new INDIVIDUAL_LEGAL_REPRESENT_ABM(Connection);
        }


        public INDIVIDUAL_LEGAL_REPRESENTFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new INDIVIDUAL_LEGAL_REPRESENT_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new INDIVIDUAL_LEGAL_REPRESENT
        /// </summary>
        /// <param name="businessObject">INDIVIDUAL_LEGAL_REPRESENT object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(INDIVIDUAL_LEGAL_REPRESENT businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing INDIVIDUAL_LEGAL_REPRESENT
        /// </summary>
        /// <param name="businessObject">INDIVIDUAL_LEGAL_REPRESENT object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(INDIVIDUAL_LEGAL_REPRESENT businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(INDIVIDUAL_LEGAL_REPRESENT businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get INDIVIDUAL_LEGAL_REPRESENT by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public INDIVIDUAL_LEGAL_REPRESENT GetByPrimaryKey(INDIVIDUAL_LEGAL_REPRESENTKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all INDIVIDUAL_LEGAL_REPRESENTs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<INDIVIDUAL_LEGAL_REPRESENT> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of INDIVIDUAL_LEGAL_REPRESENT by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<INDIVIDUAL_LEGAL_REPRESENT> GetAllBy(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete INDIVIDUAL_LEGAL_REPRESENT by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<INDIVIDUAL_LEGAL_REPRESENT> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (INDIVIDUAL_LEGAL_REPRESENT mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "INDIVIDUAL_LEGAL_REPRESENT";
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
