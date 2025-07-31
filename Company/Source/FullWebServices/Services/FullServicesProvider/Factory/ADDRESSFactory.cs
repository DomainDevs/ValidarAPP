using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServices.Models.DataLayer;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using Sistran.Co.Previsora.Application.FullServicesProvider.Provider;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer
{
    public class ADDRESSFactory
    {

        #region data Members

        ADDRESS_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public ADDRESSFactory()
        {
            _dataObject = new ADDRESS_ABM();
        }

        public ADDRESSFactory(string Connection)
        {
            _dataObject = new ADDRESS_ABM(Connection);
        }


        public ADDRESSFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new ADDRESS_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new ADDRESS
        /// </summary>
        /// <param name="businessObject">ADDRESS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(ADDRESS businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing ADDRESS
        /// </summary>
        /// <param name="businessObject">ADDRESS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(ADDRESS businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(ADDRESS businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        public bool GetAddressIndividual(int individualId)
        {
            return _dataObject.GetAddressIndividual(individualId);
        }

       
        #region Metodos Comentados

        ///// <summary>
        ///// get ADDRESS by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public ADDRESS GetByPrimaryKey(ADDRESSKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all ADDRESSs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<ADDRESS> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of ADDRESS by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<ADDRESS> GetAllBy(ADDRESS.ADDRESSFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete ADDRESS by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(ADDRESS.ADDRESSFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<ADDRESS> businessObject, List<TableMessage> ListTableMessage)
        {
            GetDto dtoUser = new GetDto();
            foreach (ADDRESS mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "ADDRESS";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                    {
                        var addressUser = GetAddressIndividual(mp.INDIVIDUAL_ID);
                        if (addressUser)
                        { tMessage.Message = Update(mp).ToString(); }
                        else
                        { tMessage.Message = Insert(mp).ToString(); }
                    }
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
