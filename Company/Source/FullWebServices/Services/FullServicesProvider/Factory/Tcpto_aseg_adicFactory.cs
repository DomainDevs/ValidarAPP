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
    public class Tcpto_aseg_adicFactory
    {

        #region data Members

        Tcpto_aseg_adic_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tcpto_aseg_adicFactory()
        {
            _dataObject = new Tcpto_aseg_adic_ABM();
        }

        public Tcpto_aseg_adicFactory(string Connection)
        {
            _dataObject = new Tcpto_aseg_adic_ABM(Connection);
        }


        public Tcpto_aseg_adicFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tcpto_aseg_adic_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tcpto_aseg_adic
        /// </summary>
        /// <param name="businessObject">Tcpto_aseg_adic object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tcpto_aseg_adic businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tcpto_aseg_adic
        /// </summary>
        /// <param name="businessObject">Tcpto_aseg_adic object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tcpto_aseg_adic businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tcpto_aseg_adic by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tcpto_aseg_adic GetByPrimaryKey(Tcpto_aseg_adicKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tcpto_aseg_adics
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tcpto_aseg_adic> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tcpto_aseg_adic by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tcpto_aseg_adic> GetAllBy(Tcpto_aseg_adic.Tcpto_aseg_adicFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tcpto_aseg_adicKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tcpto_aseg_adic by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tcpto_aseg_adic.Tcpto_aseg_adicFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tcpto_aseg_adic> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tcpto_aseg_adic mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tcpto_aseg_adic";
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
