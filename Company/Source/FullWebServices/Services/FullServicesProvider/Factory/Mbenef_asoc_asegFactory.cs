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
    public class Mbenef_asoc_asegFactory
    {

        #region data Members

        Mbenef_asoc_aseg_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mbenef_asoc_asegFactory()
        {
            _dataObject = new Mbenef_asoc_aseg_ABM();
        }

        public Mbenef_asoc_asegFactory(string Connection)
        {
            _dataObject = new Mbenef_asoc_aseg_ABM(Connection);
        }


        public Mbenef_asoc_asegFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mbenef_asoc_aseg_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mbenef_asoc_aseg
        /// </summary>
        /// <param name="businessObject">Mbenef_asoc_aseg object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mbenef_asoc_aseg businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mbenef_asoc_aseg
        /// </summary>
        /// <param name="businessObject">Mbenef_asoc_aseg object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mbenef_asoc_aseg businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mbenef_asoc_aseg by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mbenef_asoc_aseg GetByPrimaryKey(Mbenef_asoc_asegKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mbenef_asoc_asegs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mbenef_asoc_aseg> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mbenef_asoc_aseg by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mbenef_asoc_aseg> GetAllBy(Mbenef_asoc_aseg.Mbenef_asoc_asegFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Mbenef_asoc_asegKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mbenef_asoc_aseg by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mbenef_asoc_aseg.Mbenef_asoc_asegFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mbenef_asoc_aseg> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Mbenef_asoc_aseg mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Mbenef_asoc_aseg";
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
