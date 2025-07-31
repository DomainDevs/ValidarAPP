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
    public class Mpres_cptoFactory
    {

        #region data Members

        Mpres_cpto_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mpres_cptoFactory()
        {
            _dataObject = new Mpres_cpto_ABM();
        }

        public Mpres_cptoFactory(string Connection)
        {
            _dataObject = new Mpres_cpto_ABM(Connection);
        }


        public Mpres_cptoFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpres_cpto_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpres_cpto
        /// </summary>
        /// <param name="businessObject">Mpres_cpto object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpres_cpto businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpres_cpto
        /// </summary>
        /// <param name="businessObject">Mpres_cpto object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpres_cpto businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        public bool Delete(Mpres_cpto businessObject)
        {
            return _dataObject.Delete(businessObject);
        }


        #region Metodos Comentados

        ///// <summary>
        ///// get Mpres_cpto by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpres_cpto GetByPrimaryKey(Mpres_cptoKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpres_cptos
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpres_cpto> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpres_cpto by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpres_cpto> GetAllBy(Mpres_cpto.Mpres_cptoFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Mpres_cptoKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mpres_cpto by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpres_cpto.Mpres_cptoFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpres_cpto> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Mpres_cpto mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Mpres_cpto";
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
