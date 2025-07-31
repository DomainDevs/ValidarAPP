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
    public class Tipo_persona_asegFactory
    {

        #region data Members

        Tipo_persona_aseg_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tipo_persona_asegFactory()
        {
            _dataObject = new Tipo_persona_aseg_ABM();
        }

        public Tipo_persona_asegFactory(string Connection)
        {
            _dataObject = new Tipo_persona_aseg_ABM(Connection);
        }


        public Tipo_persona_asegFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tipo_persona_aseg_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tipo_persona_aseg
        /// </summary>
        /// <param name="businessObject">Tipo_persona_aseg object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tipo_persona_aseg businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tipo_persona_aseg
        /// </summary>
        /// <param name="businessObject">Tipo_persona_aseg object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tipo_persona_aseg businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tipo_persona_aseg by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tipo_persona_aseg GetByPrimaryKey(Tipo_persona_asegKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tipo_persona_asegs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tipo_persona_aseg> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tipo_persona_aseg by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tipo_persona_aseg> GetAllBy(Tipo_persona_aseg.Tipo_persona_asegFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tipo_persona_asegKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tipo_persona_aseg by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tipo_persona_aseg.Tipo_persona_asegFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tipo_persona_aseg> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tipo_persona_aseg mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tipo_persona_aseg";
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
