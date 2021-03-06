﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PostmanExport
{

    internal class Info
    {

        [JsonProperty("_postman_id")]
        public string PostmanId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }
    }

    internal class Header
    {

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    internal class Formdata
    {

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string Src { get; set; }
    }

    internal class Body
    {

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("formdata", NullValueHandling = NullValueHandling.Ignore)]
        public IList<Formdata> Formdata { get; set; }

        [JsonProperty("raw", NullValueHandling = NullValueHandling.Ignore)]
        public string Raw { get; set; }
    }

    internal class Url
    {

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("host")]
        public IList<string> Host { get; set; }

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public string Port { get; set; }

        [JsonProperty("path")]
        public IList<string> Path { get; set; }

        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Query { get; set; }


    }

    internal class Request
    {

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("header")]
        public IList<Header> Header { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("url")]
        public Url Url { get; set; }
    }

    internal class Item
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }

        [JsonProperty("response")]
        public IList<object> Response { get; set; }
    }

    internal class PostmanJson
    {

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("item")]
        public IList<Item> Item { get; set; }
    }

    class FormJson
    {

        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }
}
