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
    public class AGENT_PREFIXFactory
    {

        #region data Members

        AGENT_PREFIX_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public AGENT_PREFIXFactory()
        {
            _dataObject = new AGENT_PREFIX_ABM();
        }

        public AGENT_PREFIXFactory(string Connection)
        {
            _dataObject = new AGENT_PREFIX_ABM(Connection);
        }


        public AGENT_PREFIXFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new AGENT_PREFIX_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new AGENT_PREFIX
        /// </summary>
        /// <param name="businessObject">AGENT_PREFIX object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(AGENT_PREFIX businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing AGENT_PREFIX
        /// </summary>
        /// <param name="businessObject">AGENT_PREFIX object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(AGENT_PREFIX businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(AGENT_PREFIX businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get AGENT_PREFIX by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public AGENT_PREFIX GetByPrimaryKey(AGENT_PREFIXKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all AGENT_PREFIXs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<AGENT_PREFIX> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of AGENT_PREFIX by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<AGENT_PREFIX> GetAllBy(AGENT_PREFIX.AGENT_PREFIXFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete AGENT_PREFIX by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(AGENT_PREFIX.AGENT_PREFIXFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<AGENT_PREFIX> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (AGENT_PREFIX mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "AGENT_PREFIX";
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
