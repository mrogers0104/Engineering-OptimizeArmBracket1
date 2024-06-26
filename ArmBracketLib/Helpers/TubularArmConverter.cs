using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// 

namespace ArmBracketDesignLibrary.Helpers
{
    /// <summary>
    /// Convert an abstract class for Json serialize and deserialize.
    /// </summary>
    /// <remarks>
    /// See URL: https://blog.codeinside.eu/2015/03/30/json-dotnet-deserialize-to-abstract-class-or-interface/
    ///
    /// and
    ///
    /// See URL: http://kristianbrimble.com/deserializing-abstract-types-using-newtonsoft-json/
    ///
    /// and
    ///
    /// See URL: https://code.i-harness.com/en/q/183a32a
    /// </remarks>
    public class TubularArmConverter : JsonConverter
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Type _convertSubtypesOfType;

        private readonly IEnumerable<Type> _subTypes;

        #region Constructors

        public TubularArmConverter(Type convertSubtypesOfType)
        {
            _convertSubtypesOfType = convertSubtypesOfType;

            _subTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes(), (domainAssembly, assemblyType) => assemblyType)
                .Where(t => _convertSubtypesOfType.IsAssignableFrom(t) && t != _convertSubtypesOfType && !t.IsAbstract);
        }

        #endregion  // Constructors

        #region Implement JsonConverter

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(_convertSubtypesOfType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                return existingValue;
            }

            var obj = JObject.Load(reader);
            objectType = GetSubtypeToConvertTo(obj);
 
            existingValue = Activator.CreateInstance(objectType);
            serializer.Populate(obj.CreateReader(), existingValue);
 
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }


        public override bool CanRead => true;
        public override bool CanWrite => false;
 
        private Type GetSubtypeToConvertTo(JObject jObj)
        {
            foreach (var subtype in _subTypes)
            {
                if (JsonHasPropertiesFromType(subtype, jObj))
                    return subtype;
            }
            return _convertSubtypesOfType;
        }
 
        private static bool JsonHasPropertiesFromType(Type t, JObject json)
        {
            var jsonProperties = json.Properties().Select(jProp => jProp.Name.ToLowerInvariant());
            return t.GetProperties().Select(prop => prop.Name.ToLowerInvariant()).All(propName => jsonProperties.Contains(propName));
        }


        #endregion  // Implement JsonConverter


        #region Properties


        #endregion  // Properties

        #region Methods


        #endregion  // Methods
    }
}
