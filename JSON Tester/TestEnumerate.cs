using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using JSON_Enumerate;
using System.Linq;
using JSON_Enumerate.Implementation;
using JSON_Enumerate.Operation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSON_Display.Operation;

namespace JSON_Tester
{
    [TestClass]
    public class TestEnumerate
    {
        [TestMethod]
        public void ProcessCSharpClass()
        {

            // type: String size 1 => public string {key} { get; set;}
            // Define {key} NVARCHAR({size/default};

            //            JsonDocument.AddJsonOptions(option =>
            //option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

            var options = new JsonDocumentOptions
            {
                
               // P = JsonNamingPolicy.CamelCase,
            };

            var doc = JsonDocument.Parse(@"{ ""Text"": ""1"", ""DetailChanges"":[{""Property"":""Comments"",""ChangedTo"":""2nd Comment"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""CCC"",""ChangedTo"":""XR71"",""UniqueId"":null,""SummaryInstance"":null}], ""Value"": 1, ""IsValid"": true, ""Account"" : { ""AccountId"" : 1234  }  }"
, options); 

            Assert.IsNotNull(doc);

            var settings = new OperationSettings() {Name = "TestName"};

            var instance = doc.RootElement.WalkStructure<CSharpClass, CSharpProperty>
                (new CSharpClass(settings, null), null);

            Assert.IsNotNull(instance);

            SettingsSingleton.Settings = new TempSettings() { Name = "ProcessCharpClass" };

            Console.WriteLine(instance.ToString());

        }
    }

    public class TempSettings : IJsonSettings
    {
        public string Name { get; set; }
        public bool AddDTOSuffix { get; set; }
        public bool AddJsonProperty { get; set; }
        public bool AddJsonPropertyName { get; set; }
        public bool AddTableTypeConstraint { get; set; }
        public bool IsSortProperties { get; set; }
    }
}
