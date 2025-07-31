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
    public class Tdirector_comercial_histFactory
    {

        #region data Members

        Tdirector_comercial_hist_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tdirector_comercial_histFactory()
        {
            _dataObject = new Tdirector_comercial_hist_ABM();
        }

        public Tdirector_comercial_histFactory(string Connection)
        {
            _dataObject = new Tdirector_comercial_hist_ABM(Connection);
        }


        public Tdirector_comercial_histFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tdirector_comercial_hist_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tdirector_comercial_hist
        /// </summary>
        /// <param name="businessObject">Tdirector_comercial_hist object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tdirector_comercial_hist businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tdirector_comercial_hist
        /// </summary>
        /// <param name="businessObject">Tdirector_comercial_hist object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tdirector_comercial_hist businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Tdirector_comercial_hist businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tdirector_comercial_hist by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tdirector_comercial_hist GetByPrimaryKey(Tdirector_comercial_histKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tdirector_comercial_hists
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tdirector_comercial_hist> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tdirector_comercial_hist by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tdirector_comercial_hist> GetAllBy(Tdirector_comercial_hist.Tdirector_comercial_histFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete Tdirector_comercial_hist by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tdirector_comercial_hist.Tdirector_comercial_histFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tdirector_comercial_hist> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tdirector_comercial_hist mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Tdirector_comercial_hist";
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
