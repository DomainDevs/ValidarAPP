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
    public class Tusuario_centro_costoFactory
    {

        #region data Members

        Tusuario_centro_costo_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tusuario_centro_costoFactory()
        {
            _dataObject = new Tusuario_centro_costo_ABM();
        }

        public Tusuario_centro_costoFactory(string Connection)
        {
            _dataObject = new Tusuario_centro_costo_ABM(Connection);
        }


        public Tusuario_centro_costoFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tusuario_centro_costo_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tusuario_centro_costo
        /// </summary>
        /// <param name="businessObject">Tusuario_centro_costo object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tusuario_centro_costo businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tusuario_centro_costo
        /// </summary>
        /// <param name="businessObject">Tusuario_centro_costo object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tusuario_centro_costo businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Tusuario_centro_costo businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tusuario_centro_costo by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tusuario_centro_costo GetByPrimaryKey(Tusuario_centro_costoKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tusuario_centro_costos
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tusuario_centro_costo> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tusuario_centro_costo by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tusuario_centro_costo> GetAllBy(Tusuario_centro_costo.Tusuario_centro_costoFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete Tusuario_centro_costo by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tusuario_centro_costo.Tusuario_centro_costoFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tusuario_centro_costo> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tusuario_centro_costo mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tusuario_centro_costo";
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
