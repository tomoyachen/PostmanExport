using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PostmanExport.FiddlerExtensions
{
    public class Collection
    {

        private Session[] oSessions;

        public Collection()
        {
        }

        public Collection(Session[] oSessions)
        {
            this.oSessions = oSessions;
        }



        private string getRequestPath(Session session)
        {
            //FiddlerApplication.Log.LogString("getRequestPath: " + WebUtility.HtmlEncode(session.PathAndQuery));
            //return WebUtility.HtmlEncode(session.PathAndQuery);
            return session.PathAndQuery;

        }

        private string getRequestMethod(Session session)
        {
            //FiddlerApplication.Log.LogString("getRequestMethod: " + session.oRequest.headers.HTTPMethod.ToUpper());
            return session.oRequest.headers.HTTPMethod.ToUpper();
        }

        
        private string getRequestBody(Session session)
        {

            //FiddlerApplication.Log.LogString("getRequestBody: " + session.GetRequestBodyAsString());
            //return WebUtility.HtmlEncode(session.GetRequestBodyAsString());
            return session.GetRequestBodyAsString();
        }

        private string getProtocol(Session session)
        {
            //FiddlerApplication.Log.LogString("getProtocol: " + session.oRequest.headers.UriScheme.ToLower());
            return session.oRequest.headers.UriScheme.ToLower();
        }

        private bool isExistContentType(Session session)
        {

            return session.oRequest.headers.Exists("Content-Type");
        }

        private string getContentType(Session session)
        {
            
            //FiddlerApplication.Log.LogString("getContentType: " + session.oRequest["Content-Type"].ToString());
            return session.oRequest["Content-Type"].ToString();
        }


        private string getIpAddress(Session session)
        {
            //FiddlerApplication.Log.LogString("getIpAddress: " + session.hostname);
            return session.hostname;
        }

        private void log(string log)
        {
            FiddlerApplication.Log.LogString(log);
        }


        //--封装------------

        private static bool isJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //获取头
        private List<Header> getHeader(Session session)
        {
            List<Header> headerList = new List<Header>();
            foreach (HTTPHeaderItem element in session.oRequest.headers) //循环取出全部的头信息
            {
                Header header = new Header();
                header.Key = element.Name;
                header.Value = System.Web.HttpUtility.UrlDecode(element.Value);
                header.Type = "text";
                headerList.Add(header);
            }

            return headerList;
        }

        //获取formdata
        private List<Formdata> getFormdataList(Session session)
        {
            String requestbody = getRequestBody(session);
            //log(requestbody);
            Dictionary<string, string> dict = new Dictionary<string, string>();

            bool isContainFile = this.getContentType(session).Contains("boundary"); //存在文件

            if (!isContainFile)
            {
                string[] bodyList = requestbody.Split('&');

                foreach (string item in bodyList)
                {
                    string[] tmp = item.Split('=');

                    try
                    {
                        if (tmp.Length >= 2 && tmp[0] != "")
                        {
                            dict.Add(tmp[0], tmp[1]);
                        }
                        else if (tmp.Length == 1 && tmp[0] != "")
                        {
                            dict.Add(tmp[0], "");
                        }
                    }
                    catch (Exception e)
                    {
                        //log(getRequestBody(session));
                        log("getFormdataList()异常: \r\n" + e);
                    }

                    

                }

            }
            else
            {

                List<string> keyList = new List<string>();
                Regex reg = new Regex(" name=\"(.*?)\"");
                Match math = reg.Match(requestbody);
                while (math.Success)
                {
                    keyList.Add(math.Groups[1].Value);
                    //log("key >>> " + math.Groups[1].Value);
                    math = math.NextMatch();
                }

                List<string> valueList = new List<string>();
                Regex reg2 = new Regex("\r\n(.*?)\r\n--");
                Match math2 = reg2.Match(requestbody);
                while (math2.Success)
                {
                    valueList.Add(math2.Groups[1].Value);
                    //log("value >>> " + math2.Groups[1].Value);
                    math2 = math2.NextMatch();
                }

                List<string> fileList = new List<string>();
                Regex reg3 = new Regex(" name=\"(.+?)\"; filename=\"(.*?)\"");
                Match math3 = reg3.Match(requestbody);
                while (math3.Success)
                {
                    fileList.Add(math3.Groups[1].Value);
                    //log("file >>> " + math3.Groups[1].Value);
                    math3 = math3.NextMatch();
                }

                for (int l = 0; l < keyList.Count; l++)
                {
                    try
                    {
                        dict.Add(keyList[l], valueList[l]);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            dict.Add(keyList[l], "");
                        }
                        catch (Exception e2) {
                            //log(getRequestBody(session));
                            log("getFormdataList()异常: \r\n" + e2);
                        }
                    }


                    if (fileList.Contains(keyList[l]))
                    {
                        dict[keyList[l]] = "e0e8ac77652c602a这是文件4e0306d57c6e3114"; //曲线救国，更新文件的value
                    }

                }

            }


            //formdata
            List<Formdata> formdataList = new List<Formdata>();
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                Formdata formdata = new Formdata();
                formdata.Key = kvp.Key;
                if (kvp.Value.Contains("e0e8ac77652c602a这是文件4e0306d57c6e3114"))
                {
                    formdata.Type = "file";
                    formdata.Src = "";
                }
                else
                {
                    formdata.Value = System.Web.HttpUtility.UrlDecode(kvp.Value); //如果有引号 则转义
                    formdata.Type = "text";
                }


                if (isJson(JsonConvert.SerializeObject(formdata)))
                {
                    formdataList.Add(formdata);//formdataList
                }
                else
                {
                    log("formdata属于非json格式" + getRequestPath(session));
                    
                }
            }

            return formdataList;
        }


        //本体
        public string generateContent()
        {
            if (this.oSessions.Length < 1) //无会话
            {
                //FiddlerApplication.Log.LogString("没有任何会话可以导出！");
                throw new Exception();
                
            }

            Session[] array = this.oSessions;
            List<Item> itemList = new List<Item>();
            for (int i = 0; i < array.Length; i++)
            {
                Session session = array[i]; //获取第一个会话


                //headers
                List<Header> headerList = getHeader(session);
                //FiddlerApplication.Log.LogString("headerJson >>> " + JsonConvert.SerializeObject(headerList));

                //序列化formdata
                List<Formdata> formdataList = getFormdataList(session);


                //body
                Body body = new Body();
                if (isJson(getRequestBody(session)) || getContentType(session).Contains("application/x-json-stream") || formdataList.Count < 1)
                {
                    body.Mode = "raw";
                    body.Raw = System.Web.HttpUtility.UrlDecode(getRequestBody(session));

                }
                else
                {
                    body.Mode = "formdata";
                    body.Formdata = formdataList;
                }

                Url url = new Url();
                url.Raw = getIpAddress(session) + System.Web.HttpUtility.UrlDecode(getRequestPath(session));
                url.Protocol = getProtocol(session);
                url.Host = getIpAddress(session).Split('.');
                string[] portTmp = getIpAddress(session).Split(':');
                if (portTmp.Length >= 2)
                {
                    url.Port = portTmp[1];
                }
                url.Path = System.Web.HttpUtility.UrlDecode(getRequestPath(session)).Split('/');

                //log("url >>> " + JsonConvert.SerializeObject(url));


                //FiddlerApplication.Log.LogString("bodyJson >>> " + JsonConvert.SerializeObject(body));


                Request request = new Request();
                request.Method = getRequestMethod(session);
                request.Header = headerList;
                request.Body = body;
                request.Url = url;

                //log("request >>> " + JsonConvert.SerializeObject(request));

                Item item = new Item();
                item.Name = getRequestPath(this.oSessions[0]).Split('?').GetValue(0).ToString();
                item.Request = request;
                item.Response = new List<Object>();

                //log("item >>> " + JsonConvert.SerializeObject(item));

                
                if (isJson(JsonConvert.SerializeObject(item)))
                {
                    itemList.Add(item);
                }
                else
                {
                    log("item属于非json格式" + getRequestPath(session));
                }


            }

            Info info = new Info();
            info.PostmanId = "";
            info.Name = getIpAddress(this.oSessions[0]);
            info.Schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json";

            PostmanJson postmanJson = new PostmanJson();
            postmanJson.Info = info;
            postmanJson.Item = itemList;

            //log("postmanJson >>> " + JsonConvert.SerializeObject(postmanJson));

            string postman = JsonConvert.SerializeObject(postmanJson); //获取JSON字符串
            postman = FormJson.ConvertJsonString(postman); //格式化JSON格式
            postman = System.Web.HttpUtility.UrlDecode(postman);

            return postman; 

        }



    }
}
