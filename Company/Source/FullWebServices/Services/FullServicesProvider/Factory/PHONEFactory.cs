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
    public class PHONEFactory
    {

        #region data Members

        PHONE_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public PHONEFactory()
        {
            _dataObject = new PHONE_ABM();
        }

        public PHONEFactory(string Connection)
        {
            _dataObject = new PHONE_ABM(Connection);
        }


        public PHONEFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new PHONE_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new PHONE
        /// </summary>
        /// <param name="businessObject">PHONE object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(PHONE businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing PHONE
        /// </summary>
        /// <param name="businessObject">PHONE object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(PHONE businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(PHONE businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        public bool GetPhoneIndividual(int individualId)
        {
            return _dataObject.GetPhoneIndividual(individualId);
        }
        #region Metodos Comentados

        ///// <summary>
        ///// get PHONE by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public PHONE GetByPrimaryKey(PHONEKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all PHONEs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<PHONE> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of PHONE by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<PHONE> GetAllBy(PHONE.PHONEFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete PHONE by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(PHONE.PHONEFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<PHONE> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (PHONE mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "PHONE";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                    {
                        var phonesUser = GetPhoneIndividual(mp.INDIVIDUAL_ID);
                        if (phonesUser)
                        { tMessage.Message = Update(mp).ToString(); }
                        else
                        { tMessage.Message = Insert(mp).ToString(); }
                    }
                        
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
