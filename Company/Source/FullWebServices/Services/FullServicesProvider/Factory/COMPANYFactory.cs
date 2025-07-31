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
    public class COMPANYFactory
    {

        #region data Members

        COMPANY_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public COMPANYFactory()
        {
            _dataObject = new COMPANY_ABM();
        }

        public COMPANYFactory(string Connection)
        {
            _dataObject = new COMPANY_ABM(Connection);
        }


        public COMPANYFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new COMPANY_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new COMPANY
        /// </summary>
        /// <param name="businessObject">COMPANY object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(COMPANY businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing COMPANY
        /// </summary>
        /// <param name="businessObject">COMPANY object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(COMPANY businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
        /// </summary>
        /// <param name="businessObject">COMPANY object</param>
        /// <returns>true for successfully saved</returns>
        public bool UpdateU(COMPANY businessObject)
        {
            return _dataObject.UpdateU(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(COMPANY businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get COMPANY by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public COMPANY GetByPrimaryKey(COMPANYKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all COMPANYs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<COMPANY> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of COMPANY by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<COMPANY> GetAllBy(COMPANY.COMPANYFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete COMPANY by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(COMPANY.COMPANYFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<COMPANY> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (COMPANY mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "COMPANY";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                        tMessage.Message = Update(mp).ToString();
                    //Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
                    else if (mp.State.Equals('B'))
                        //tMessage.Message = UpdateU(mp).ToString(); //OT PENDIENTE POR PASAR
                        tMessage.Message = Update(mp).ToString();
                    //Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
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
