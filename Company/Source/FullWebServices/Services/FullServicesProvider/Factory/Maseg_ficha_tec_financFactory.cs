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
    public class Maseg_ficha_tec_financFactory
    {

        #region data Members

        Maseg_ficha_tec_financ_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Maseg_ficha_tec_financFactory()
        {
            _dataObject = new Maseg_ficha_tec_financ_ABM();
        }

        public Maseg_ficha_tec_financFactory(string Connection)
        {
            _dataObject = new Maseg_ficha_tec_financ_ABM(Connection);
        }


        public Maseg_ficha_tec_financFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Maseg_ficha_tec_financ_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Maseg_ficha_tec_financ
        /// </summary>
        /// <param name="businessObject">Maseg_ficha_tec_financ object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Maseg_ficha_tec_financ businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Maseg_ficha_tec_financ
        /// </summary>
        /// <param name="businessObject">Maseg_ficha_tec_financ object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Maseg_ficha_tec_financ businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(Maseg_ficha_tec_financ businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Maseg_ficha_tec_financ by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Maseg_ficha_tec_financ GetByPrimaryKey(Maseg_ficha_tec_financKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Maseg_ficha_tec_financs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Maseg_ficha_tec_financ> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Maseg_ficha_tec_financ by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Maseg_ficha_tec_financ> GetAllBy(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete Maseg_ficha_tec_financ by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Maseg_ficha_tec_financ.Maseg_ficha_tec_financFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Maseg_ficha_tec_financ> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Maseg_ficha_tec_financ mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Maseg_ficha_tec_financ";
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
