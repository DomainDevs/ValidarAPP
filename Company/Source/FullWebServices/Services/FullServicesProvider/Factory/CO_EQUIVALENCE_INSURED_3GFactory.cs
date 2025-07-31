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
    public class CO_EQUIVALENCE_INSURED_3GFactory
    {

        #region data Members

        CO_EQUIVALENCE_INSURED_3G_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public CO_EQUIVALENCE_INSURED_3GFactory()
        {
            _dataObject = new CO_EQUIVALENCE_INSURED_3G_ABM();
        }

        public CO_EQUIVALENCE_INSURED_3GFactory(string Connection)
        {
            _dataObject = new CO_EQUIVALENCE_INSURED_3G_ABM(Connection);
        }


        public CO_EQUIVALENCE_INSURED_3GFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new CO_EQUIVALENCE_INSURED_3G_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new CO_EQUIVALENCE_INSURED_3G
        /// </summary>
        /// <param name="businessObject">CO_EQUIVALENCE_INSURED_3G object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(CO_EQUIVALENCE_INSURED_3G businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing CO_EQUIVALENCE_INSURED_3G
        /// </summary>
        /// <param name="businessObject">CO_EQUIVALENCE_INSURED_3G object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(CO_EQUIVALENCE_INSURED_3G businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(CO_EQUIVALENCE_INSURED_3G businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get CO_EQUIVALENCE_INSURED_3G by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public CO_EQUIVALENCE_INSURED_3G GetByPrimaryKey(CO_EQUIVALENCE_INSURED_3GKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all CO_EQUIVALENCE_INSURED_3Gs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<CO_EQUIVALENCE_INSURED_3G> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of CO_EQUIVALENCE_INSURED_3G by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<CO_EQUIVALENCE_INSURED_3G> GetAllBy(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete CO_EQUIVALENCE_INSURED_3G by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(CO_EQUIVALENCE_INSURED_3G.CO_EQUIVALENCE_INSURED_3GFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<CO_EQUIVALENCE_INSURED_3G> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (CO_EQUIVALENCE_INSURED_3G mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";                
                tMessage.NameTable = "CO_EQUIVALENCE_INSURED_3G";
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
