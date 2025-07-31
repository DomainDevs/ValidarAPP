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
    public class Maseg_autoriza_consulFactory
    {

        #region data Members

        Maseg_autoriza_consul_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Maseg_autoriza_consulFactory()
        {
            _dataObject = new Maseg_autoriza_consul_ABM();
        }

        public Maseg_autoriza_consulFactory(string Connection)
        {
            _dataObject = new Maseg_autoriza_consul_ABM(Connection);
        }


        public Maseg_autoriza_consulFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Maseg_autoriza_consul_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Maseg_autoriza_consul
        /// </summary>
        /// <param name="businessObject">Maseg_autoriza_consul object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Maseg_autoriza_consul businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Maseg_autoriza_consul
        /// </summary>
        /// <param name="businessObject">Maseg_autoriza_consul object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Maseg_autoriza_consul businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Maseg_autoriza_consul by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Maseg_autoriza_consul GetByPrimaryKey(Maseg_autoriza_consulKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Maseg_autoriza_consuls
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Maseg_autoriza_consul> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Maseg_autoriza_consul by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Maseg_autoriza_consul> GetAllBy(Maseg_autoriza_consul.Maseg_autoriza_consulFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Maseg_autoriza_consulKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Maseg_autoriza_consul by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Maseg_autoriza_consul.Maseg_autoriza_consulFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Maseg_autoriza_consul> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Maseg_autoriza_consul mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Maseg_autoriza_consul";
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
