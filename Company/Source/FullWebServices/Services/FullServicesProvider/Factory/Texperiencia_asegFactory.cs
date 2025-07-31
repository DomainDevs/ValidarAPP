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
    public class Texperiencia_asegFactory
    {

        #region data Members

        Texperiencia_aseg_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Texperiencia_asegFactory()
        {
            _dataObject = new Texperiencia_aseg_ABM();
        }

        public Texperiencia_asegFactory(string Connection)
        {
            _dataObject = new Texperiencia_aseg_ABM(Connection);
        }


        public Texperiencia_asegFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Texperiencia_aseg_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Texperiencia_aseg
        /// </summary>
        /// <param name="businessObject">Texperiencia_aseg object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Texperiencia_aseg businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Texperiencia_aseg
        /// </summary>
        /// <param name="businessObject">Texperiencia_aseg object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Texperiencia_aseg businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Texperiencia_aseg by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Texperiencia_aseg GetByPrimaryKey(Texperiencia_asegKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Texperiencia_asegs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Texperiencia_aseg> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Texperiencia_aseg by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Texperiencia_aseg> GetAllBy(Texperiencia_aseg.Texperiencia_asegFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Texperiencia_asegKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Texperiencia_aseg by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Texperiencia_aseg.Texperiencia_asegFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Texperiencia_aseg> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Texperiencia_aseg mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Texperiencia_aseg";
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
