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
    public class Tcondicion_impuestoFactory
    {

        #region data Members

        Tcondicion_impuesto_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tcondicion_impuestoFactory()
        {
            _dataObject = new Tcondicion_impuesto_ABM();
        }

        public Tcondicion_impuestoFactory(string Connection)
        {
            _dataObject = new Tcondicion_impuesto_ABM(Connection);
        }


        public Tcondicion_impuestoFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tcondicion_impuesto_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tcondicion_impuesto
        /// </summary>
        /// <param name="businessObject">Tcondicion_impuesto object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tcondicion_impuesto businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tcondicion_impuesto
        /// </summary>
        /// <param name="businessObject">Tcondicion_impuesto object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tcondicion_impuesto businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Tcondicion_impuesto businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tcondicion_impuesto by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tcondicion_impuesto GetByPrimaryKey(Tcondicion_impuestoKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tcondicion_impuestos
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tcondicion_impuesto> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tcondicion_impuesto by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tcondicion_impuesto> GetAllBy(Tcondicion_impuesto.Tcondicion_impuestoFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete Tcondicion_impuesto by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tcondicion_impuesto.Tcondicion_impuestoFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tcondicion_impuesto> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tcondicion_impuesto mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tcondicion_impuesto";
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
