using System;
using System.Configuration;
using System.Data;
using Sybase.Data.AseClient;
using System.Data.SqlTypes;

//CSUP cambiar el namespace por Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer
namespace Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer
{
    /// <summary>
    /// Data layer base class for database interaction.
    /// </summary>
    abstract class DataLayerBase : IDisposable
    {
        #region Data Members

        AseConnection _mainConnection;
        AseCommand _mainCommand;
        bool _isDisposed;
        string _usser;
        int _appId;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        public DataLayerBase()
        {
            // Initialize the class members of data layer.
            InitClass();
        }

        /// <summary>
        /// Constructor with Connection
        /// </summary>
        public DataLayerBase(string Connection)
        {
            // Initialize the class members of data layer.
            InitClass(Connection);
        }


        /// <summary>
        /// Constructor with Connection
        /// </summary>
        public DataLayerBase(string Connection, string userId, int AppId)
        {
            // Initialize the class members of data layer.
            InitClass(Connection, userId, AppId);
        }


        /// <summary>
        /// Constructor with Command
        /// </summary>
        public DataLayerBase(string Connection, string userId, int AppId, AseCommand Command)
        {
            Command.CommandTimeout = 2000;
            // Initialize the class members of data layer.
            InitClass(Connection, userId, AppId,Command);
        }


        #endregion

        #region Properties

        /// <summary>
        /// get the sql connection object
        /// </summary>
        protected AseConnection MainConnection
        {
            get { return _mainConnection; }

        }

        /// <summary>
        /// get the sql Command object
        /// </summary>
        protected AseCommand MainCommand
        {
            get { return _mainCommand; }

        }

        protected string User
        {
            get { return _usser; }
            set { _usser = value; }
        }

        protected int AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes class members.
        /// </summary>
        private void InitClass()
        {
            // create Connection Object
            _mainConnection = new AseConnection();

            // Get connection string from Config File and set to the connection
            _mainConnection.ConnectionString = ConfigurationManager.AppSettings["Main.ConnectionString"];
            _isDisposed = false;
        }

        /// <summary>
        /// Initializes class members.
        /// </summary>
        private void InitClass(string Connection)
        {
            // create Connection Object
            _mainConnection = new AseConnection();

            // Get connection string from code
            _mainConnection.ConnectionString = ConfigurationManager.ConnectionStrings[Connection].ToString();
            _isDisposed = false;
        }

        /// <summary>
        /// Initializes class members.
        /// </summary>
        private void InitClass(string Connection, string userId, int AppId)
        {
            // create Connection Object
            _mainConnection = new AseConnection();

            // Get connection string from code
            _mainConnection.ConnectionString = ConfigurationManager.ConnectionStrings[Connection].ToString();
            _usser = userId;
            _appId = AppId;
            _isDisposed = false;
        }

        /// <summary>
        /// Initializes class members.
        /// </summary>
        private void InitClass(string Connection, string userId, int AppId,AseCommand Command)
        {
            // create Connection Object
            _mainConnection = new AseConnection();
            // Get connection string from code
            _mainConnection.ConnectionString = ConfigurationManager.ConnectionStrings[Connection].ToString();
            _mainCommand = Command;
            _usser = userId;
            _appId = AppId;
            _isDisposed = false;
        }

        #endregion

        #region IDisposeable

        /// <summary>
        /// Implements the IDispose' method Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements the Dispose functionality.
        /// </summary>
        protected virtual void Dispose(bool bIsDisposing)
        {
            // Check to see if Dispose has already been called.
            if (!_isDisposed)
            {
                if (bIsDisposing)
                {
                    // Dispose managed resources.
                    _mainConnection.Dispose();
                    _mainConnection = null;
                }
            }
            _isDisposed = true;
        }

        #endregion

    }
}
