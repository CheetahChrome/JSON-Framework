using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate
{


    public static class JsonEnumerator
    {
        public static readonly string InitiationError = "Invalid JSON snippet received, must start with object or array of objects.";
        /// <summary>
        /// This method walks the JSON tree and processes the nodes/elements and allows for reportage by `IJsonOperation` and `IProperty`
        /// instances to be provided by/consumed by the originator
        /// </summary>
        /// <typeparam name="T">Top level, usually a class type structure for the end user. The initiation of this usually passes in
        /// a valid instance instead of null, as to name the top level class/node.</typeparam>
        /// <typeparam name="P">Holds the properties for the T class and json data.</typeparam>
        /// <param name="je">Each node starts with a `JsonElement`</param>
        /// <param name="instance"></param>
        /// <param name="originParent"></param>
        /// <remarks>This is called recursively.</remarks>
        /// <returns>The new T instance created or the existing one passed in.</returns>
        public static T WalkStructure<T,P>(this JsonElement je, T instance, T originParent) where T : IJsonOperation, new()
                                                                                            where P : IProperty, new()
        {
            if (instance == null)
                instance = new T();

            // Origin parent contains all the sub objects
            if (originParent == null)
                originParent = instance;

            switch (je.ValueKind)
            {
                case JsonValueKind.Object:

                    if (instance.Properties == null)
                        instance.Properties = new List<IProperty>();

                    instance.Properties.AddRange(
                        je.EnumerateObject().Select(jp =>  WalkProperties<P,T>(jp, originParent))
                                                     .OfType<IProperty>()

                        );

                    break;

                case JsonValueKind.Array:
                    var arrItems = je.EnumerateArray();

                    if (arrItems.Any())
                    {
                        WalkStructure<T,P>(arrItems.First(), instance, originParent);



                        //arrItems.Select((jElement, index) => new TreeViewItemEx(jElement, index, showQuote))
                        //    .ToList()
                        //    .ForEach(tvi => root.Items.Add(tvi));
                    }
                    break;

                // Technically node of these should be hit, valid json will be an object or an array of objects.
                case JsonValueKind.Number:
                case JsonValueKind.String:
                case JsonValueKind.Null:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Undefined:
                    throw new ApplicationException(InitiationError);

            }

            return instance;
        }

        public static P WalkProperties<P,T>(JsonProperty jp, T originParent ) where P : IProperty, new() where T : IJsonOperation, new()
        {
            var instance = new P() { Name = jp.Name };
            var jElement = jp.Value;

 //           base.ToolTip = jElement.ValueKind.ToString();
            var key = jp.Name;

            switch (jElement.ValueKind)
            {

                case JsonValueKind.String:
                    instance.JsonType = JsonPropType.StrType;
                    instance.Size = jElement.GetRawText().Length;
                    break;

                case JsonValueKind.Number:
                    instance.JsonType = JsonPropType.NumberType;
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    instance.JsonType = JsonPropType.BoolType;
                    break;
                //case JsonValueKind.Null:
                //    base.Header = $"{key} : null";
                //    break;

                case JsonValueKind.Object:
                    // Set a property to a user type
                    instance.JsonType = JsonPropType.UserType;

                    // Generate a sub object UserType which is named as the key

                    var sub = new T() { Name = key };

                    // Add it to the origin parent
                    originParent.SubClasses.Add(sub);

                    // Recurse back into processing

                    WalkStructure<T, P>(jElement, sub, originParent);

                    break;

                case JsonValueKind.Array:
                    instance.JsonType = JsonPropType.UserType;
                    var subArray = new T() { Name = key };

                    // Set a property to a user type
                    instance.JsonType = JsonPropType.UserType;

                    // Add it to the origin parent
                    originParent.SubClasses.Add(subArray);

                    WalkStructure<T, P>(jElement, subArray, originParent);

                    //base.Header = key;
                    //base.ToolTip += $" ({jElement.GetArrayLength()})";

                    //jElement.ProcessJson(this);
                    break;


                    //case JsonValueKind.Undefined:
                    //    base.Header += $"${key} : -undefined-";
                    //    break;


                    //default:
                    //    base.Header = key;
            }


            return instance;
        }


    }
}
