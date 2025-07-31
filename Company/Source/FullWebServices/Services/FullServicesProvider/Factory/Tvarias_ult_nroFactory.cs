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
    public class Tvarias_ult_nroFactory
    {

        #region data Members

        Tvarias_ult_nro_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tvarias_ult_nroFactory()
        {
            _dataObject = new Tvarias_ult_nro_ABM();
        }

        public Tvarias_ult_nroFactory(string Connection)
        {
            _dataObject = new Tvarias_ult_nro_ABM(Connection);
        }


        public Tvarias_ult_nroFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tvarias_ult_nro_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tvarias_ult_nro
        /// </summary>
        /// <param name="businessObject">Tvarias_ult_nro object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tvarias_ult_nro businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tvarias_ult_nro
        /// </summary>
        /// <param name="businessObject">Tvarias_ult_nro object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tvarias_ult_nro businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tvarias_ult_nro by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tvarias_ult_nro GetByPrimaryKey(Tvarias_ult_nroKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tvarias_ult_nros
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tvarias_ult_nro> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tvarias_ult_nro by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tvarias_ult_nro> GetAllBy(Tvarias_ult_nro.Tvarias_ult_nroFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tvarias_ult_nroKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tvarias_ult_nro by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tvarias_ult_nro.Tvarias_ult_nroFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tvarias_ult_nro> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tvarias_ult_nro mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tvarias_ult_nro";
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
