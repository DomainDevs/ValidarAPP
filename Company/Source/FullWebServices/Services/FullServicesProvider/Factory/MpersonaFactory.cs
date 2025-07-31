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
    public class MpersonaFactory
    {

        #region data Members

        Mpersona_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public MpersonaFactory()
        {
            _dataObject = new Mpersona_ABM();
        }

        public MpersonaFactory(string Connection)
        {
            _dataObject = new Mpersona_ABM(Connection);
        }

        public MpersonaFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Mpersona_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Mpersona
        /// </summary>
        /// <param name="businessObject">Mpersona object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Mpersona businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Mpersona
        /// </summary>
        /// <param name="businessObject">Mpersona object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Mpersona businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
        /// </summary>
        /// <param name="businessObject">Mpersona object</param>
        /// <returns>true for successfully saved</returns>
        public bool UpdateU(Mpersona businessObject)
        {
            return _dataObject.UpdateU(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Mpersona by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Mpersona GetByPrimaryKey(MpersonaKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Mpersonas
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Mpersona> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Mpersona by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Mpersona> GetAllBy(Mpersona.MpersonaFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(MpersonaKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Mpersona by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Mpersona.MpersonaFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Mpersona> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (Mpersona mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "Mpersona";
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
