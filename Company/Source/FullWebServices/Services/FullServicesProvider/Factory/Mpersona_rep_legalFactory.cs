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
    public class Mpersona_rep_legalFactory
    {

        #region data Members

        Mpersona_rep_legal_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mpersona_rep_legalFactory()
        {
            _dataObject = new Mpersona_rep_legal_ABM();
        }

        public Mpersona_rep_legalFactory(string Connection)
        {
            _dataObject = new Mpersona_rep_legal_ABM(Connection);
        }


        public Mpersona_rep_legalFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpersona_rep_legal_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpersona_rep_legal
        /// </summary>
        /// <param name="businessObject">Mpersona_rep_legal object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpersona_rep_legal businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpersona_rep_legal
        /// </summary>
        /// <param name="businessObject">Mpersona_rep_legal object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpersona_rep_legal businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mpersona_rep_legal by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpersona_rep_legal GetByPrimaryKey(Mpersona_rep_legalKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpersona_rep_legals
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpersona_rep_legal> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpersona_rep_legal by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpersona_rep_legal> GetAllBy(Mpersona_rep_legal.Mpersona_rep_legalFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Mpersona_rep_legalKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mpersona_rep_legal by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpersona_rep_legal.Mpersona_rep_legalFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpersona_rep_legal> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Mpersona_rep_legal mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Mpersona_rep_legal";
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
