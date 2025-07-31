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
    public class BOARD_DIRECTORSFactory
    {

        #region data Members

        BOARD_DIRECTORS_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public BOARD_DIRECTORSFactory()
        {
            _dataObject = new BOARD_DIRECTORS_ABM();
        }

        public BOARD_DIRECTORSFactory(string Connection)
        {
            _dataObject = new BOARD_DIRECTORS_ABM(Connection);
        }


        public BOARD_DIRECTORSFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new BOARD_DIRECTORS_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new BOARD_DIRECTORS
        /// </summary>
        /// <param name="businessObject">BOARD_DIRECTORS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(BOARD_DIRECTORS businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing BOARD_DIRECTORS
        /// </summary>
        /// <param name="businessObject">BOARD_DIRECTORS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(BOARD_DIRECTORS businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(BOARD_DIRECTORS businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get BOARD_DIRECTORS by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public BOARD_DIRECTORS GetByPrimaryKey(BOARD_DIRECTORSKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all BOARD_DIRECTORSs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<BOARD_DIRECTORS> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of BOARD_DIRECTORS by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<BOARD_DIRECTORS> GetAllBy(BOARD_DIRECTORS.BOARD_DIRECTORSFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete BOARD_DIRECTORS by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(BOARD_DIRECTORS.BOARD_DIRECTORSFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<BOARD_DIRECTORS> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (BOARD_DIRECTORS mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "BOARD_DIRECTORS";
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
