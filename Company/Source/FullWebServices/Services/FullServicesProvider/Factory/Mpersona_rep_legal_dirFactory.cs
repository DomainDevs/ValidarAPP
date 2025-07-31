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
    public class Mpersona_rep_legal_dirFactory
    {

        #region data Members

        Mpersona_rep_legal_dir_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Mpersona_rep_legal_dirFactory()
        {
            _dataObject = new Mpersona_rep_legal_dir_ABM();
        }

        public Mpersona_rep_legal_dirFactory(string Connection)
        {
            _dataObject = new Mpersona_rep_legal_dir_ABM(Connection);
        }


        public Mpersona_rep_legal_dirFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpersona_rep_legal_dir_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpersona_rep_legal_dir
        /// </summary>
        /// <param name="businessObject">Mpersona_rep_legal_dir object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpersona_rep_legal_dir businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpersona_rep_legal_dir
        /// </summary>
        /// <param name="businessObject">Mpersona_rep_legal_dir object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpersona_rep_legal_dir businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mpersona_rep_legal_dir by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpersona_rep_legal_dir GetByPrimaryKey(Mpersona_rep_legal_dirKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpersona_rep_legal_dirs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpersona_rep_legal_dir> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpersona_rep_legal_dir by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpersona_rep_legal_dir> GetAllBy(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Mpersona_rep_legal_dirKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mpersona_rep_legal_dir by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpersona_rep_legal_dir.Mpersona_rep_legal_dirFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpersona_rep_legal_dir> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Mpersona_rep_legal_dir mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Mpersona_rep_legal_dir";
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
