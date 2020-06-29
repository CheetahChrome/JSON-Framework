using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using JSON_Enumerate;
using System.Linq;
using JSON_Enumerate.Implementation;
using JSON_Enumerate.Operation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var doc = JsonDocument.Parse(@"{ ""Text"": ""1"", ""DetailChanges"":[{""Property"":""Comments"",""ChangedTo"":""2nd Comment"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""CCC"",""ChangedTo"":""XR71"",""UniqueId"":null,""SummaryInstance"":null}], ""Value"": 1, ""IsValid"": true, ""Account"" : { ""AccountId"" : 1234  }  }"); 

            Assert.IsNotNull(doc);

            var instance = doc.RootElement.WalkStructure<CSharpClass, CSharpProperty>
                (new CSharpClass("Top"), null);

            Assert.IsNotNull(instance);

            Console.WriteLine(instance.ToString());

        }
    }



}
