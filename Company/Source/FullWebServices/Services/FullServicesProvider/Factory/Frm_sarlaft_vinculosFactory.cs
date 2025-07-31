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
    public class Frm_sarlaft_vinculosFactory
    {

        #region data Members

        Frm_sarlaft_vinculos_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Frm_sarlaft_vinculosFactory()
        {
            _dataObject = new Frm_sarlaft_vinculos_ABM();
        }

        public Frm_sarlaft_vinculosFactory(string Connection)
        {
            _dataObject = new Frm_sarlaft_vinculos_ABM(Connection);
        }


        public Frm_sarlaft_vinculosFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Frm_sarlaft_vinculos_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Frm_sarlaft_vinculos
        /// </summary>
        /// <param name="businessObject">Frm_sarlaft_vinculos object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Frm_sarlaft_vinculos businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Frm_sarlaft_vinculos
        /// </summary>
        /// <param name="businessObject">Frm_sarlaft_vinculos object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Frm_sarlaft_vinculos businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Frm_sarlaft_vinculos businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Frm_sarlaft_vinculos by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Frm_sarlaft_vinculos GetByPrimaryKey(Frm_sarlaft_vinculosKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Frm_sarlaft_vinculoss
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Frm_sarlaft_vinculos> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Frm_sarlaft_vinculos by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Frm_sarlaft_vinculos> GetAllBy(Frm_sarlaft_vinculos.Frm_sarlaft_vinculosFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete Frm_sarlaft_vinculos by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Frm_sarlaft_vinculos.Frm_sarlaft_vinculosFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Frm_sarlaft_vinculos> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Frm_sarlaft_vinculos mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Frm_sarlaft_vinculos";
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
