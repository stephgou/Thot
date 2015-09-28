﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Collections.Generic;
using System.Linq;
using Doc.Search.App.Models;
using Newtonsoft.Json.Linq;

namespace Doc.Search.App.Models
{
    public static partial class DocumentPropertiesCollection
    {
        /// <summary>
        /// Deserialize the object
        /// </summary>
        public static IList<DocumentProperties> DeserializeJson(JToken inputObject)
        {
            IList<DocumentProperties> deserializedObject = new List<DocumentProperties>();
            foreach (JToken iListValue in ((JArray)inputObject))
            {
                DocumentProperties documentProperties = new DocumentProperties();
                documentProperties.DeserializeJson(iListValue);
                deserializedObject.Add(documentProperties);
            }
            return deserializedObject;
        }
    }
}