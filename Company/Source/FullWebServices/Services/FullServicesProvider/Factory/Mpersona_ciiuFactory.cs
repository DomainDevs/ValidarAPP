using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServices.Models.DataLayer;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using Sybase.Data.AseClient;


namespace Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer
{
    public class Mpersona_ciiuFactory
    {

        #region data Members

        Mpersona_ciiu_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mpersona_ciiuFactory()
        {
            _dataObject = new Mpersona_ciiu_ABM();
        }

        public Mpersona_ciiuFactory(string Connection)
        {
            _dataObject = new Mpersona_ciiu_ABM(Connection);
        }


        public Mpersona_ciiuFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpersona_ciiu_ABM(Connection, userId, AppId,Command);
        }   
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpersona_ciiu
        /// </summary>
        /// <param name="businessObject">Mpersona_ciiu object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpersona_ciiu businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpersona_ciiu
        /// </summary>
        /// <param name="businessObject">Mpersona_ciiu object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpersona_ciiu businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mpersona_ciiu by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpersona_ciiu GetByPrimaryKey(Mpersona_ciiuKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpersona_ciius
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpersona_ciiu> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpersona_ciiu by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpersona_ciiu> GetAllBy(Mpersona_ciiu.Mpersona_ciiuFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Mpersona_ciiuKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mpersona_ciiu by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpersona_ciiu.Mpersona_ciiuFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpersona_ciiu> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Mpersona_ciiu mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Mpersona_ciiu";
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
