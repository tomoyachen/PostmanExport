using Fiddler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private string filePath;

        private Session[] oSessions;

        public Collection()
        {
        }

        public Collection(Session[] oSessions)
        {
        }

        public Collection(Session[] oSessions, string filePath)
        {
            this.filePath = filePath;
            this.oSessions = oSessions;
        }

        private string getRequestPath(Session session)
        {
            FiddlerApplication.Log.LogString("getRequestPath: " + WebUtility.HtmlEncode(session.PathAndQuery));
            return WebUtility.HtmlEncode(session.PathAndQuery);

        }

        private string getRequestMethod(Session session)
        {
            FiddlerApplication.Log.LogString("getRequestMethod: " + session.oRequest.headers.HTTPMethod.ToUpper());
            return session.oRequest.headers.HTTPMethod.ToUpper();
        }


        private string getRequestBody(Session session)
        {

            FiddlerApplication.Log.LogString("getRequestBody: " + WebUtility.HtmlEncode(session.GetRequestBodyAsString()));
            return WebUtility.HtmlEncode(session.GetRequestBodyAsString());
        }

        private string getProtocol(Session session)
        {
            FiddlerApplication.Log.LogString("getProtocol: " + session.oRequest.headers.UriScheme.ToLower());
            return session.oRequest.headers.UriScheme.ToLower();
        }

        private bool isExistContentType(Session session)
        {

            return session.oRequest.headers.Exists("Content-Type");
        }

        private string getContentType(Session session)
        {
            
            FiddlerApplication.Log.LogString("getContentType: " + session.oRequest["Content-Type"].ToString());
            return session.oRequest["Content-Type"].ToString();
        }


        private string getIpAddress(Session session)
        {
            FiddlerApplication.Log.LogString("getIpAddress: " + session.hostname);
            return session.hostname;
        }

        private void log(string log)
        {
            FiddlerApplication.Log.LogString(log);
        }

        //施工中
        public string generate()
        {
            if (this.oSessions.Length < 1) //无会话
            {
                FiddlerApplication.Log.LogString("没有任何会话可以导出！");
                throw new Exception();
            }
            StringBuilder stringBuilder = new StringBuilder();
            Session[] array = this.oSessions;
            for (int i = 0; i < array.Length; i++)
            {
                Session session = array[i]; //获取第一个会话


                //headers
                List<Header> headerList = new List<Header>();
                foreach (HTTPHeaderItem element in session.oRequest.headers) //循环取出全部的头信息
                {
                    Header header = new Header();
                    header.Key = element.Name;
                    header.Value = element.Value;
                    header.Type = "text";
                    headerList.Add(header);
                }

                string headerJson = JsonConvert.SerializeObject(headerList); //Header Json
                FiddlerApplication.Log.LogString("headerJson >>> " + headerJson);


                //序列化formdata
                String requestbody = getRequestBody(session).Replace("&amp", "&").Replace("&quot;", "\"");
                Dictionary<string, string> dict = new Dictionary<string, string>();


                bool isContainFile = this.getContentType(session).Contains("boundary"); //存在文件
                if (!isContainFile)
                {
                    string[] bodyList = requestbody.Split('&');

                    foreach (string item in bodyList)
                    {
                        string[] tmp = item.Split('=');
                        if (tmp.Length >= 2)
                        {
                            dict.Add(tmp[0], tmp[1]);
                        }
                        else if (tmp.Length == 1)
                        {
                            dict.Add(tmp[0], "");
                        }

                    }

                }
                else
                {


                    log("requestbody >>> " + requestbody);

                    Regex reg = new Regex("name=\"(.+?)\"");
                    Match math = reg.Match(requestbody);
                    string value = math.Groups[0].Value;

                    while (math.Success)
                    {
                        log("key >>> " + math.Groups[1].Value);
                        math = math.NextMatch();
                    }


                    Regex reg2 = new Regex("\n(.+?)\n--");
                    Match math2 = reg2.Match(requestbody);
                    string value2 = math.Groups[0].Value;

                    while (math2.Success)
                    {
                        log("value >>> " + math2.Groups[1].Value);
                        math2 = math2.NextMatch();
                    }

                }


                //formdata
                List<Formdata> formdataList = new List<Formdata>();
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    Formdata formdata = new Formdata();
                    formdata.Key = kvp.Key;
                    formdata.Value = kvp.Value;
                    formdata.Type = "text";
                    formdataList.Add(formdata);
                }
                
                //body
                Body body = new Body();
                body.Mode = "formdata";
                body.Formdata = formdataList;


                





                /*
                string text5 = null;
                string requestPath = this.getRequestPath(session2);
                bool flag2 = text.Equals(this.getIpAddress(session2));
                if (flag2)
                {
                    text = "";
                }
                else
                {
                    text = this.getIpAddress(session2);
                }
                bool flag3 = text2.Equals(session2.port.ToString());
                if (flag3)
                {
                    text2 = "";
                }
                else
                {
                    text2 = session2.port.ToString();
                }
                bool flag4 = text3.Equals(this.getProtocol(session2));
                if (flag4)
                {
                    text3 = "";
                }
                else
                {
                    text3 = this.getProtocol(session2);
                }
                bool flag5 = this.isExistContentType(session2);
                string value;
                if (flag5)
                {
                    string contentType = this.getContentType(session2);
                    bool flag6 = !contentType.Contains("boundary") && !contentType.Contains("octet - stream") && !contentType.Contains("image") && !contentType.Contains("video") && !contentType.Contains("audio") && !contentType.Contains("tar") && !contentType.Contains("zip") && !contentType.Contains("rtf") && (!contentType.Contains("pdf") && !contentType.Contains("powerpoint")) && !contentType.Contains("x-compress") && !contentType.Contains("msword");
                    if (flag6)
                    {
                        value = this.getRequestBody(session2);
                    }
                    else
                    {
                        value = "";
                    }
                }
                else
                {
                    value = this.getRequestBody(session2);
                }
                bool flag7 = requestPath.Contains("/unode/stor/uploadPart");
                if (flag7)
                {
                    value = "";
                }
                text5 = element.surroundByHTTPSamplerProxy(text5, text, text2, text3, this.getRequestMethod(session2), requestPath, value, requestPath);
                stringBuilder.Append(text5);
                */
            }

            return "";
        }



        public string generateContent()
        {
            bool flag = this.oSessions.Length == 0;
            if (flag)
            {
                FiddlerApplication.Log.LogString("Error, There has no sessions, please check.");
                throw new Exception();
            }
            Session session = this.oSessions[0];
            string text = this.getIpAddress(session);
            string text2 = session.port.ToString();
            string encoding = "utf-8";
            string text3 = this.getProtocol(session);
            Element element = new Element();
            string text4 = element.addConfigTestElement(null, text, text2, encoding, text3, "HTTP请求默认值");
            text4 = element.addCookieManager(text4, "HTTP Cookie 管理器");
            text4 = element.addArguments(text4, "用户定义的变量");
            StringBuilder stringBuilder = new StringBuilder();
            Session[] array = this.oSessions;
            for (int i = 0; i < array.Length; i++)
            {
                Session session2 = array[i];
                string text5 = null;
                text5 = element.addJSONPathAssertion(text5, "$.stat", "OK", "验证响应结果");
                string requestPath = this.getRequestPath(session2);
                bool flag2 = text.Equals(this.getIpAddress(session2));
                if (flag2)
                {
                    text = "";
                }
                else
                {
                    text = this.getIpAddress(session2);
                }
                bool flag3 = text2.Equals(session2.port.ToString());
                if (flag3)
                {
                    text2 = "";
                }
                else
                {
                    text2 = session2.port.ToString();
                }
                bool flag4 = text3.Equals(this.getProtocol(session2));
                if (flag4)
                {
                    text3 = "";
                }
                else
                {
                    text3 = this.getProtocol(session2);
                }
                bool flag5 = this.isExistContentType(session2);
                string value;
                if (flag5)
                {
                    string contentType = this.getContentType(session2);
                    bool flag6 = !contentType.Contains("boundary") && !contentType.Contains("octet - stream") && !contentType.Contains("image") && !contentType.Contains("video") && !contentType.Contains("audio") && !contentType.Contains("tar") && !contentType.Contains("zip") && !contentType.Contains("rtf") && (!contentType.Contains("pdf") && !contentType.Contains("powerpoint")) && !contentType.Contains("x-compress") && !contentType.Contains("msword");
                    if (flag6)
                    {
                        value = this.getRequestBody(session2);
                    }
                    else
                    {
                        value = "";
                    }
                }
                else
                {
                    value = this.getRequestBody(session2);
                }
                bool flag7 = requestPath.Contains("/unode/stor/uploadPart");
                if (flag7)
                {
                    value = "";
                }
                text5 = element.surroundByHTTPSamplerProxy(text5, text, text2, text3, this.getRequestMethod(session2), requestPath, value, requestPath);
                stringBuilder.Append(text5);
            }
            string value2 = element.surroundByThreadGroup(stringBuilder.ToString(), "线程组");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(text4);
            stringBuilder2.Append(value2);
            text4 = element.addViewResultTree(stringBuilder2.ToString(), "察看结果树");
            text4 = element.addAssertionResult(text4, "断言结果");
            text4 = element.surroundByTestPlan(text4, "测试计划");
            FiddlerApplication.Log.LogString(text4);
            return element.addXmlHead(XDocument.Parse(text4).ToString());
        }

        public void saveAsPostmanScript()
        {
            Encoding encoding = new UTF8Encoding(true);
            string value = this.generateContent();
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(this.filePath, false, encoding);
                streamWriter.Write(value);
                MessageBox.Show("Postman脚本导出成功 !", "提示", MessageBoxButtons.OK);
            }
            finally
            {
                bool flag = streamWriter != null;
                if (flag)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
            }
        }
    }
}
