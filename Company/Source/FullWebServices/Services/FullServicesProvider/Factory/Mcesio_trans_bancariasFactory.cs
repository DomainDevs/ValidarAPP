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
    public class Mcesio_trans_bancariasFactory
    {

        #region data Members

        Mcesio_trans_bancarias_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mcesio_trans_bancariasFactory()
        {
            _dataObject = new Mcesio_trans_bancarias_ABM();
        }

        public Mcesio_trans_bancariasFactory(string Connection)
        {
            _dataObject = new Mcesio_trans_bancarias_ABM(Connection);
        }


        public Mcesio_trans_bancariasFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mcesio_trans_bancarias_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mcesio_trans_bancarias
        /// </summary>
        /// <param name="businessObject">Mcesio_trans_bancarias object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mcesio_trans_bancarias businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mcesio_trans_bancarias
        /// </summary>
        /// <param name="businessObject">Mcesio_trans_bancarias object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mcesio_trans_bancarias businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mcesio_trans_bancarias by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mcesio_trans_bancarias GetByPrimaryKey(Mcesio_trans_bancariasKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mcesio_trans_bancariass
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mcesio_trans_bancarias> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mcesio_trans_bancarias by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mcesio_trans_bancarias> GetAllBy(Mcesio_trans_bancarias.Mcesio_trans_bancariasFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Mcesio_trans_bancariasKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mcesio_trans_bancarias by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mcesio_trans_bancarias.Mcesio_trans_bancariasFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mcesio_trans_bancarias> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Mcesio_trans_bancarias mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Mcesio_trans_bancarias";
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
