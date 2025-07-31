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
    public class TECHNICAL_CARD_DESCRIPTIONFactory
    {

        #region data Members

        TECHNICAL_CARD_DESCRIPTION_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public TECHNICAL_CARD_DESCRIPTIONFactory()
        {
            _dataObject = new TECHNICAL_CARD_DESCRIPTION_ABM();
        }

        public TECHNICAL_CARD_DESCRIPTIONFactory(string Connection)
        {
            _dataObject = new TECHNICAL_CARD_DESCRIPTION_ABM(Connection);
        }


        public TECHNICAL_CARD_DESCRIPTIONFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new TECHNICAL_CARD_DESCRIPTION_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new TECHNICAL_CARD_DESCRIPTION
        /// </summary>
        /// <param name="businessObject">TECHNICAL_CARD_DESCRIPTION object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(TECHNICAL_CARD_DESCRIPTION businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing TECHNICAL_CARD_DESCRIPTION
        /// </summary>
        /// <param name="businessObject">TECHNICAL_CARD_DESCRIPTION object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(TECHNICAL_CARD_DESCRIPTION businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(TECHNICAL_CARD_DESCRIPTION businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get TECHNICAL_CARD_DESCRIPTION by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public TECHNICAL_CARD_DESCRIPTION GetByPrimaryKey(TECHNICAL_CARD_DESCRIPTIONKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all TECHNICAL_CARD_DESCRIPTIONs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<TECHNICAL_CARD_DESCRIPTION> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of TECHNICAL_CARD_DESCRIPTION by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<TECHNICAL_CARD_DESCRIPTION> GetAllBy(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete TECHNICAL_CARD_DESCRIPTION by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<TECHNICAL_CARD_DESCRIPTION> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (TECHNICAL_CARD_DESCRIPTION mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "TECHNICAL_CARD_DESCRIPTION";
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
