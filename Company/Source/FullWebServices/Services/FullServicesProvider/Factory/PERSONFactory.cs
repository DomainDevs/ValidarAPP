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
    public class PERSONFactory
    {

        #region data Members

        PERSON_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public PERSONFactory()
        {
            _dataObject = new PERSON_ABM();
        }

        public PERSONFactory(string Connection)
        {
            _dataObject = new PERSON_ABM(Connection);
        }


        public PERSONFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new PERSON_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new PERSON
        /// </summary>
        /// <param name="businessObject">PERSON object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(PERSON businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing PERSON
        /// </summary>
        /// <param name="businessObject">PERSON object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(PERSON businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
        /// </summary>
        /// <param name="businessObject">PERSON object</param>
        /// <returns>true for successfully saved</returns>
        public bool UpdateU(PERSON businessObject)
        {
            return _dataObject.UpdateU(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(PERSON businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get PERSON by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public PERSON GetByPrimaryKey(PERSONKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all PERSONs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<PERSON> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of PERSON by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<PERSON> GetAllBy(PERSON.PERSONFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete PERSON by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(PERSON.PERSONFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<PERSON> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (PERSON mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "PERSON";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                        tMessage.Message = Update(mp).ToString();
                    //Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
                    else if (mp.State.Equals('B'))
                        //tMessage.Message = UpdateU(mp).ToString(); //OT PENDIENTE POR PASAR
                        tMessage.Message = Update(mp).ToString();
                    //Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
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
