using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using JSON_Vector.Interfaces;

namespace JSON_Vector
{
    /// <summary>
    /// Step by step to send and receive data to a database.
    /// </summary>
    public class VectorDatabase : IDisposable
    {
        private bool disposedValue;

        public string ConnectionString { get; set; }

        public static VectorDatabase Create()
            => new VectorDatabase();

        public static VectorDatabase Create(string connectionString)
            => new VectorDatabase(connectionString);

        private SqlConnection Connection { get; set; }

        public VectorDatabase(){}

        /// <summary>
        /// Create a instance with the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public VectorDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        //public void SendToDatabase<T>(T itemTOSend) where T : ISprocOperationDTO
        //{
        //    var item = itemTOSend ?? throw new ApplicationException("Need a valid item to send");

        //}


        public void SendToDatabase<T>(IList<T> listToSend) where T : class, ISprocOperationDTO, new()
        {
            var list = listToSend ?? throw new ApplicationException("Need a valid list");
            if (!list.Any())
                throw new ApplicationException("The data list must contain at least one item.");

            var typeInstance = listToSend.First();

            var sproc = typeInstance.StoredProcedureName;

            if (string.IsNullOrWhiteSpace(sproc))
                throw new ApplicationException("List instance type does not contain a valid stored procedure name. Set in `StoredProcedureName` property.");

            SqlParameter parameters = typeInstance.ProcessThenYieldMultiples(listToSend);

            Connection = new SqlConnection(ConnectionString);
            {
                Connection.Open();

                using (var cmd = new SqlCommand(sproc, Connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 600 })
                {
                    if (Connection != null)
                        cmd.Parameters.Add(parameters);

                    var reader = cmd.ExecuteNonQuery();

                    //                    result = await reader.ReadAllAsync();
                }

                Connection.Close();
            }

            Connection = null;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Connection?.Close();
                    Connection = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~VectorDatabase()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
