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
    public class Maseg_deporteFactory
    {

        #region data Members

        Maseg_deporte_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Maseg_deporteFactory()
        {
            _dataObject = new Maseg_deporte_ABM();
        }

        public Maseg_deporteFactory(string Connection)
        {
            _dataObject = new Maseg_deporte_ABM(Connection);
        }


        public Maseg_deporteFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Maseg_deporte_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Maseg_deporte
        /// </summary>
        /// <param name="businessObject">Maseg_deporte object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Maseg_deporte businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Maseg_deporte
        /// </summary>
        /// <param name="businessObject">Maseg_deporte object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Maseg_deporte businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Maseg_deporte businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Maseg_deporte by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Maseg_deporte GetByPrimaryKey(Maseg_deporteKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Maseg_deportes
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Maseg_deporte> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Maseg_deporte by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Maseg_deporte> GetAllBy(Maseg_deporte.Maseg_deporteFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete Maseg_deporte by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Maseg_deporte.Maseg_deporteFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Maseg_deporte> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Maseg_deporte mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Maseg_deporte";
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
