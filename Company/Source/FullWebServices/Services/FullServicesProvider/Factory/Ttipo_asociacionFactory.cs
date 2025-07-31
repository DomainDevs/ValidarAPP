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
    public class Ttipo_asociacionFactory
    {

        #region data Members

        Ttipo_asociacion_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Ttipo_asociacionFactory()
        {
            _dataObject = new Ttipo_asociacion_ABM();
        }

        public Ttipo_asociacionFactory(string Connection)
        {
            _dataObject = new Ttipo_asociacion_ABM(Connection);
        }


        public Ttipo_asociacionFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Ttipo_asociacion_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Ttipo_asociacion
        /// </summary>
        /// <param name="businessObject">Ttipo_asociacion object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Ttipo_asociacion businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Ttipo_asociacion
        /// </summary>
        /// <param name="businessObject">Ttipo_asociacion object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Ttipo_asociacion businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Ttipo_asociacion by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Ttipo_asociacion GetByPrimaryKey(Ttipo_asociacionKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Ttipo_asociacions
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Ttipo_asociacion> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Ttipo_asociacion by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Ttipo_asociacion> GetAllBy(Ttipo_asociacion.Ttipo_asociacionFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Ttipo_asociacionKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Ttipo_asociacion by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Ttipo_asociacion.Ttipo_asociacionFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Ttipo_asociacion> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Ttipo_asociacion mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Ttipo_asociacion";
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
