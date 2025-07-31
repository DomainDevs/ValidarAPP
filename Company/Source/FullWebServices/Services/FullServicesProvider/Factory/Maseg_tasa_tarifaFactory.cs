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
    public class Maseg_tasa_tarifaFactory
    {

        #region data Members

        Maseg_tasa_tarifa_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Maseg_tasa_tarifaFactory()
        {
            _dataObject = new Maseg_tasa_tarifa_ABM();
        }

        public Maseg_tasa_tarifaFactory(string Connection)
        {
            _dataObject = new Maseg_tasa_tarifa_ABM(Connection);
        }


        public Maseg_tasa_tarifaFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Maseg_tasa_tarifa_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Maseg_tasa_tarifa
        /// </summary>
        /// <param name="businessObject">Maseg_tasa_tarifa object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Maseg_tasa_tarifa businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Maseg_tasa_tarifa
        /// </summary>
        /// <param name="businessObject">Maseg_tasa_tarifa object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Maseg_tasa_tarifa businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Maseg_tasa_tarifa businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Maseg_tasa_tarifa by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Maseg_tasa_tarifa GetByPrimaryKey(Maseg_tasa_tarifaKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Maseg_tasa_tarifas
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Maseg_tasa_tarifa> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Maseg_tasa_tarifa by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Maseg_tasa_tarifa> GetAllBy(Maseg_tasa_tarifa.Maseg_tasa_tarifaFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete Maseg_tasa_tarifa by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Maseg_tasa_tarifa.Maseg_tasa_tarifaFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Maseg_tasa_tarifa> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Maseg_tasa_tarifa mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Maseg_tasa_tarifa";
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
