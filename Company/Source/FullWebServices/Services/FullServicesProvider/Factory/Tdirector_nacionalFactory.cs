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
    public class Tdirector_nacionalFactory
    {

        #region data Members

        Tdirector_nacional_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Tdirector_nacionalFactory()
        {
            _dataObject = new Tdirector_nacional_ABM();
        }

        public Tdirector_nacionalFactory(string Connection)
        {
            _dataObject = new Tdirector_nacional_ABM(Connection);
        }


        public Tdirector_nacionalFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Tdirector_nacional_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Tdirector_nacional
        /// </summary>
        /// <param name="businessObject">Tdirector_nacional object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Tdirector_nacional businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Tdirector_nacional
        /// </summary>
        /// <param name="businessObject">Tdirector_nacional object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Tdirector_nacional businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Tdirector_nacional by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Tdirector_nacional GetByPrimaryKey(Tdirector_nacionalKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Tdirector_nacionals
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Tdirector_nacional> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Tdirector_nacional by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Tdirector_nacional> GetAllBy(Tdirector_nacional.Tdirector_nacionalFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Tdirector_nacionalKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Tdirector_nacional by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Tdirector_nacional.Tdirector_nacionalFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Tdirector_nacional> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Tdirector_nacional mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Tdirector_nacional";
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
