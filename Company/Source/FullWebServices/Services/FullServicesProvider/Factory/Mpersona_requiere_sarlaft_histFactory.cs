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
    public class Mpersona_requiere_sarlaft_histFactory
    {

        #region data Members

        Mpersona_requiere_sarlaft_hist_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mpersona_requiere_sarlaft_histFactory()
        {
            _dataObject = new Mpersona_requiere_sarlaft_hist_ABM();
        }

        public Mpersona_requiere_sarlaft_histFactory(string Connection)
        {
            _dataObject = new Mpersona_requiere_sarlaft_hist_ABM(Connection);
        }


        public Mpersona_requiere_sarlaft_histFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpersona_requiere_sarlaft_hist_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpersona_requiere_sarlaft_hist
        /// </summary>
        /// <param name="businessObject">Mpersona_requiere_sarlaft_hist object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpersona_requiere_sarlaft_hist businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpersona_requiere_sarlaft_hist
        /// </summary>
        /// <param name="businessObject">Mpersona_requiere_sarlaft_hist object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpersona_requiere_sarlaft_hist businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Mpersona_requiere_sarlaft_hist businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mpersona_requiere_sarlaft_hist by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpersona_requiere_sarlaft_hist GetByPrimaryKey(Mpersona_requiere_sarlaft_histKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpersona_requiere_sarlaft_hists
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpersona_requiere_sarlaft_hist> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpersona_requiere_sarlaft_hist by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpersona_requiere_sarlaft_hist> GetAllBy(Mpersona_requiere_sarlaft_hist.Mpersona_requiere_sarlaft_histFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete Mpersona_requiere_sarlaft_hist by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpersona_requiere_sarlaft_hist.Mpersona_requiere_sarlaft_histFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpersona_requiere_sarlaft_hist> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Mpersona_requiere_sarlaft_hist mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Mpersona_requiere_sarlaft_hist";
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
