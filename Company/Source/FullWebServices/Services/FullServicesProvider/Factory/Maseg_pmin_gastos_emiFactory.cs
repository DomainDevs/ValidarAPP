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
    public class Maseg_pmin_gastos_emiFactory
    {

        #region data Members

        Maseg_pmin_gastos_emi_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Maseg_pmin_gastos_emiFactory()
        {
            _dataObject = new Maseg_pmin_gastos_emi_ABM();
        }

        public Maseg_pmin_gastos_emiFactory(string Connection)
        {
            _dataObject = new Maseg_pmin_gastos_emi_ABM(Connection);
        }


        public Maseg_pmin_gastos_emiFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Maseg_pmin_gastos_emi_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Maseg_pmin_gastos_emi
        /// </summary>
        /// <param name="businessObject">Maseg_pmin_gastos_emi object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Maseg_pmin_gastos_emi businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Maseg_pmin_gastos_emi
        /// </summary>
        /// <param name="businessObject">Maseg_pmin_gastos_emi object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Maseg_pmin_gastos_emi businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Maseg_pmin_gastos_emi by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Maseg_pmin_gastos_emi GetByPrimaryKey(Maseg_pmin_gastos_emiKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Maseg_pmin_gastos_emis
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Maseg_pmin_gastos_emi> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Maseg_pmin_gastos_emi by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Maseg_pmin_gastos_emi> GetAllBy(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Maseg_pmin_gastos_emiKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Maseg_pmin_gastos_emi by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Maseg_pmin_gastos_emi.Maseg_pmin_gastos_emiFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Maseg_pmin_gastos_emi> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Maseg_pmin_gastos_emi mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Maseg_pmin_gastos_emi";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                        tMessage.Message = Update(mp).ToString();

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
