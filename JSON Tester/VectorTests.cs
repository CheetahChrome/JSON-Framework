using JSON_Vector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using JSON_Vector.Attributes;
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
    [TestMethod]
    public void FullCycleTableType()
    {
        // Step 1 JSON
        // [{"EngagementId":666,"FacilityNumber":"FacNumber1","FacilityName":"FacName1","CostCenterNumber":"CostCenterNumber1","CostCenterName":"CostCenterName1","UOSDescription":"UOSDESC #1","BudgetedVolumeForUOS":1.0000,"BudgetedTotalWorkedFTEs":2.0000,"BudgetedTotalPaidFTEs":3.0000,"WHPUOS":5.1000,"Year":"2020-01-01"},{"EngagementId":666,"FacilityNumber":"FacNumber3","FacilityName":"FacName3","CostCenterNumber":"CostCenterNumber3","CostCenterName":"CostCenterName3","UOSDescription":"UOSDESC #2","BudgetedVolumeForUOS":5.0000,"BudgetedTotalWorkedFTEs":6.0000,"BudgetedTotalPaidFTEs":7.0000,"WHPUOS":8.9000,"Year":"2020-01-01"}]
    }


    private DataTable GenerateDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Tag");
        dt.Columns.Add("TagURL");
        dt.Columns.Add("Description");

        int counter = 0;
        while (counter < 1000)
        {
            DataRow row = dt.NewRow();
            row["Tag"] = ("Tag Name " + counter);
            row["TagURL"] = ("Tag URL " + counter);
            row["Description"] = ("Tag Description " + counter);

            dt.Rows.Add(row);
            counter++;
        }

        return dt;
    }

        [TestMethod]
    public void SendToDBLong()
    {
        var connection = @"Data Source=NOVA\jabberwocky;Initial Catalog=Scratch;Integrated Security=True";

        var data =
            "[{\"PublishedBudgetDataId\":1,\"EngagementId\":666,\"FacilityNumber\":\"FacNumber1\",\"FacilityName\":\"FacName1\",\"CostCenterNumber\":\"CostCenterNumber1\",\"CostCenterName\":\"CostCenterName1\",\"UOSDescription\":\"UOSDESC #1\",\"BudgetedVolumeForUOS\":1.0000,\"BudgetedTotalWorkedFTEs\":2.0000,\"BudgetedTotalPaidFTEs\":3.0000,\"WHPUOS\":100.1000,\"Year\":\"2020-01-01\"},{\"PublishedBudgetDataId\":2,\"EngagementId\":666,\"FacilityNumber\":\"FacNumber2\",\"FacilityName\":\"FacName2\",\"CostCenterNumber\":\"CostCenterNumber2\",\"CostCenterName\":\"CostCenterName2\",\"UOSDescription\":\"UOSDESC #2\",\"BudgetedVolumeForUOS\":5.0000,\"BudgetedTotalWorkedFTEs\":6.0000,\"BudgetedTotalPaidFTEs\":7.0000,\"WHPUOS\":8.9000,\"Year\":\"2020-01-01\"},{\"PublishedBudgetDataId\":3,\"EngagementId\":666,\"FacilityNumber\":\"FacNumber3\",\"FacilityName\":\"FacName3\",\"CostCenterNumber\":\"CostCenterNumber2\",\"CostCenterName\":\"CostCenterName2\",\"UOSDescription\":\"UOSDESC #2\",\"BudgetedVolumeForUOS\":20.0000,\"BudgetedTotalWorkedFTEs\":6.0000,\"BudgetedTotalPaidFTEs\":7.0000,\"WHPUOS\":8.9000,\"Year\":\"2020-01-01\"}]";

        var budgets = System.Text.Json.JsonSerializer.Deserialize<List<PublishedBudgetDataTT>>(data);


        SqlParameter parameters = budgets[0].ProcessThenYieldMultiples(budgets);

            //    var mparams = marina.ExtractParameters();

            //Assert.IsNotNull(mparams);
            //Assert.IsTrue(mparams.Any());

            //var tableMarina = mparams.FirstOrDefault();

            //Assert.IsNotNull(tableMarina);
            //Assert.AreEqual($"@{marina.TableTypeName}", tableMarina.ParameterName);

            var sproc = budgets[0].StoredProcedureName;



            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sproc, conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 600 })
                {
                    if (parameters != null)
                        cmd.Parameters.Add(parameters);

                    var reader =  cmd.ExecuteNonQuery();

//                    result = await reader.ReadAllAsync();
                }

                conn.Close();
            }


            //var connection = new DatabaseConnection(null);

            //var str = connection.CallSProcReturnJSON(marina.StoredProcedureName, mparams);

            //    var jtoken = JContainer.Parse(str);

            //Assert.IsNotNull(jtoken);

            //System.Console.WriteLine(jtoken.ToString());

        }

        [TestMethod]
        public void SendToDBShort()
        {
            var connection = @"Data Source=NOVA\jabberwocky;Initial Catalog=Scratch;Integrated Security=True";

            var data =
                "[{\"PublishedBudgetDataId\":1,\"EngagementId\":666,\"FacilityNumber\":\"FacNumber1\",\"FacilityName\":\"FacName1\",\"CostCenterNumber\":\"CostCenterNumber1\",\"CostCenterName\":\"CostCenterName1\",\"UOSDescription\":\"UOSDESC #1\",\"BudgetedVolumeForUOS\":1.0000,\"BudgetedTotalWorkedFTEs\":2.0000,\"BudgetedTotalPaidFTEs\":3.0000,\"WHPUOS\":100.1000,\"Year\":\"2020-01-01\"},{\"PublishedBudgetDataId\":2,\"EngagementId\":666,\"FacilityNumber\":\"FacNumber2\",\"FacilityName\":\"FacName2\",\"CostCenterNumber\":\"CostCenterNumber2\",\"CostCenterName\":\"CostCenterName2\",\"UOSDescription\":\"UOSDESC #2\",\"BudgetedVolumeForUOS\":5.0000,\"BudgetedTotalWorkedFTEs\":6.0000,\"BudgetedTotalPaidFTEs\":7.0000,\"WHPUOS\":8.9000,\"Year\":\"2020-01-01\"},{\"PublishedBudgetDataId\":3,\"EngagementId\":666,\"FacilityNumber\":\"FacNumber3\",\"FacilityName\":\"FacName3\",\"CostCenterNumber\":\"CostCenterNumber2\",\"CostCenterName\":\"CostCenterName2\",\"UOSDescription\":\"UOSDESC #2\",\"BudgetedVolumeForUOS\":20.0000,\"BudgetedTotalWorkedFTEs\":6.0000,\"BudgetedTotalPaidFTEs\":7.0000,\"WHPUOS\":8.9000,\"Year\":\"2020-01-01\"}]";

            var budgets = System.Text.Json.JsonSerializer.Deserialize<List<PublishedBudgetDataTT>>(data);

            using var db = new VectorDatabase(connection);

            db.SendToDatabase(budgets);

        }
    }


    public class PublishedBudgetDataTT : SprocOperationsBase
    {
        public override string StoredProcedureName => "[dbo].[PushBudget]";
        public override string TableTypeName => "budgetTT";


        [TableType(1)]
        [JsonProperty("EngagementId")]
        public int EngagementId { get; set; }

        [TableType(2)]
        [JsonProperty("FacilityNumber")]
        public string FacilityNumber { get; set; }

        [TableType(3)]
        [JsonProperty("FacilityName")]
        public string FacilityName { get; set; }

        [TableType(4)]
        [JsonProperty("CostCenterNumber")]
        public string CostCenterNumber { get; set; }

        [TableType(5)]
        [JsonProperty("CostCenterName")]
        public string CostCenterName { get; set; }

        [TableType(6)]
        [JsonProperty("UOSDescription")]
        public string UOSDescription { get; set; }

        [TableType(7)]
        [JsonProperty("BudgetedVolumeForUOS")]
        public decimal BudgetedVolumeForUOS { get; set; }

        [TableType(8)]
        [JsonProperty("BudgetedTotalWorkedFTEs")]
        public decimal BudgetedTotalWorkedFTEs { get; set; }

        [TableType(9)]
        [JsonProperty("BudgetedTotalPaidFTEs")]
        public decimal BudgetedTotalPaidFTEs { get; set; }

        [TableType(10)]
        [JsonProperty("WHPUOS")]
        public decimal WHPUOS { get; set; }

        [TableType(11)]
        [JsonProperty("Year")]
        public DateTime Year { get; set; }

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
