using System;
using System.Collections.Generic;
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

        [JsonProperty("port")]
        public string Port { get; set; }

        [JsonProperty("path")]
        public IList<string> Path { get; set; }

        
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

}
