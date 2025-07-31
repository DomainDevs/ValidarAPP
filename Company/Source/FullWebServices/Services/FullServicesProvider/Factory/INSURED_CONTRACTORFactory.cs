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
    public class INSURED_CONTRACTORFactory
    {

        #region data Members

        INSURED_CONTRACTOR_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public INSURED_CONTRACTORFactory()
        {
            _dataObject = new INSURED_CONTRACTOR_ABM();
        }

        public INSURED_CONTRACTORFactory(string Connection)
        {
            _dataObject = new INSURED_CONTRACTOR_ABM(Connection);
        }


        public INSURED_CONTRACTORFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new INSURED_CONTRACTOR_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new INSURED_CONTRACTOR
        /// </summary>
        /// <param name="businessObject">INSURED_CONTRACTOR object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(INSURED_CONTRACTOR businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing INSURED_CONTRACTOR
        /// </summary>
        /// <param name="businessObject">INSURED_CONTRACTOR object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(INSURED_CONTRACTOR businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(INSURED_CONTRACTOR businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get INSURED_CONTRACTOR by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public INSURED_CONTRACTOR GetByPrimaryKey(INSURED_CONTRACTORKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all INSURED_CONTRACTORs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<INSURED_CONTRACTOR> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of INSURED_CONTRACTOR by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<INSURED_CONTRACTOR> GetAllBy(INSURED_CONTRACTOR.INSURED_CONTRACTORFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete INSURED_CONTRACTOR by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(INSURED_CONTRACTOR.INSURED_CONTRACTORFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<INSURED_CONTRACTOR> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (INSURED_CONTRACTOR mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "INSURED_CONTRACTOR";
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
