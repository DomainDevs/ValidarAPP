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
    public class Test_civilFactory
    {

        #region data Members

        Test_civil_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public Test_civilFactory()
        {
            _dataObject = new Test_civil_ABM();
        }

        public Test_civilFactory(string Connection)
        {
            _dataObject = new Test_civil_ABM(Connection);
        }


        public Test_civilFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new Test_civil_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new Test_civil
        /// </summary>
        /// <param name="businessObject">Test_civil object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(Test_civil businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing Test_civil
        /// </summary>
        /// <param name="businessObject">Test_civil object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(Test_civil businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get Test_civil by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public Test_civil GetByPrimaryKey(Test_civilKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all Test_civils
        ///// </summary>
        ///// <returns>list</returns>
        //public List<Test_civil> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of Test_civil by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<Test_civil> GetAllBy(Test_civil.Test_civilFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        ///// <summary>
        ///// delete by primary key
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>true for succesfully deleted</returns>
        //public bool Delete(Test_civilKeys keys)
        //{
        //    return _dataObject.Delete(keys); 
        //}

        ///// <summary>
        ///// delete Test_civil by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(Test_civil.Test_civilFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<Test_civil> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (Test_civil mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "Test_civil";
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
