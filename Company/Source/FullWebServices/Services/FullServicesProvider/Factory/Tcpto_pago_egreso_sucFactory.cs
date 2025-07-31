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
    public class Tcpto_pago_egreso_sucFactory
    {

        #region data Members

        Tcpto_pago_egreso_suc_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tcpto_pago_egreso_sucFactory()
        {
            _dataObject = new Tcpto_pago_egreso_suc_ABM();
        }

        public Tcpto_pago_egreso_sucFactory(string Connection)
        {
            _dataObject = new Tcpto_pago_egreso_suc_ABM(Connection);
        }


        public Tcpto_pago_egreso_sucFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tcpto_pago_egreso_suc_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tcpto_pago_egreso_suc
        /// </summary>
        /// <param name="businessObject">Tcpto_pago_egreso_suc object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tcpto_pago_egreso_suc businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tcpto_pago_egreso_suc
        /// </summary>
        /// <param name="businessObject">Tcpto_pago_egreso_suc object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tcpto_pago_egreso_suc businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tcpto_pago_egreso_suc by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tcpto_pago_egreso_suc GetByPrimaryKey(Tcpto_pago_egreso_sucKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tcpto_pago_egreso_sucs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tcpto_pago_egreso_suc> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tcpto_pago_egreso_suc by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tcpto_pago_egreso_suc> GetAllBy(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tcpto_pago_egreso_sucKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tcpto_pago_egreso_suc by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tcpto_pago_egreso_suc.Tcpto_pago_egreso_sucFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tcpto_pago_egreso_suc> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tcpto_pago_egreso_suc mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tcpto_pago_egreso_suc";
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
