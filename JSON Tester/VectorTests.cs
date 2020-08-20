using JSON_Vector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using JSON_Vector.Connection;
using SQLJSON.Extensions;

namespace JSON_Tester
{
    [TestClass]
    public class VectorTests
    {

        [TestMethod]
        public void GenerateVector()
        {
            var connection = @"Data Source=NOVA\jabberwocky;Initial Catalog=WideWorldImporters;Integrated Security=True";

            //           var jb = new JsonConnectionBase();

            var ct = new Cities();
            string result;

            using (var conn = new SqlConnection(connection))
            {
                conn.Open();

                using (var cmd = new SqlCommand(ct.TableTypeName, conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 600 })
                {
                    //if (parameters != null)
                    //    cmd.Parameters.AddRange(parameters);

                    var reader = cmd.ExecuteJsonReader();

                    result = reader.ReadAll();
                }
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));

            Console.WriteLine(result);

        }
    }




    public class Cities : SprocOperationsBase
    {
        public override string TableTypeName => "get.Cities";

        [JsonProperty("CityID")]
        public int CityID { get; set; }

        [JsonProperty("CityName")]
        public string CityName { get; set; }

        [JsonProperty("StateProvinceID")]
        public int StateProvinceID { get; set; }

    }

}
