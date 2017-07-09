using Net.Chdk.Model.Software;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Writers.Software.Json
{
    sealed class JsonSoftwareWriter : ISoftwareWriter
    {
        #region Static Constructor

        static JsonSoftwareWriter()
        {
            _serializer = new Lazy<JsonSerializer>(GetSerializer);
            _settings = new Lazy<JsonSerializerSettings>(GetSettings);
        }

        #endregion

        #region ISoftwareWriter Members

        public void WriteSoftware(string path, IDictionary<string, SoftwareInfo> hash2sw)
        {
            Write(path, hash2sw);
        }

        #endregion

        #region Write

        private static void Write<T>(string path, T obj)
        {
            using (var writer = new StreamWriter(path))
            {
                Serializer.Serialize(writer, obj);
            }
        }

        #endregion

        #region Serializer

        private static readonly Lazy<JsonSerializer> _serializer;

        private static JsonSerializer Serializer => _serializer.Value;

        private static JsonSerializer GetSerializer()
        {
            return JsonSerializer.CreateDefault(Settings);
        }

        #endregion

        #region Settings

        private static readonly Lazy<JsonSerializerSettings> _settings;

        public static JsonSerializerSettings Settings => _settings.Value;

        private static JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = GetConverters(),
            };
        }

        #endregion

        #region Converters

        private static JsonConverter[] GetConverters()
        {
            return new[]
            {
                new VersionConverter()
            };
        }

        #endregion
    }
}
