using System;
using System.Text;

namespace PostmanExport.FiddlerExtensions
{
	public class Element
	{
		private StringBuilder stringBuilder;

		public string addXmlHead(string xmlBody)
		{
			this.stringBuilder = new StringBuilder();
			this.stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
			this.stringBuilder.Append(xmlBody);
			return this.stringBuilder.ToString();
		}

		public string surroundByTestPlan(string content, string testPlanName = "测试计划")
		{
			this.stringBuilder = new StringBuilder();
			this.stringBuilder.Append("<PostmanTestPlan version=\"1.2\" properties=\"3.2\">");
			this.stringBuilder.Append("<hashTree>");
			this.stringBuilder.Append(string.Format("<TestPlan guiclass=\"TestPlanGui\" testclass=\"TestPlan\" testname=\"{0}\" enabled=\"true\">", testPlanName));
			this.stringBuilder.Append("<stringProp name=\"TestPlan.comments\"></stringProp>");
			this.stringBuilder.Append("<boolProp name=\"TestPlan.functional_mode\">false</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"TestPlan.serialize_threadgroups\">false</boolProp>");
			this.stringBuilder.Append("<elementProp name=\"TestPlan.user_defined_variables\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"用户定义的变量\" enabled=\"true\">");
			this.stringBuilder.Append("<collectionProp name=\"Arguments.arguments\"/>");
			this.stringBuilder.Append("</elementProp>");
			this.stringBuilder.Append("<stringProp name=\"TestPlan.user_define_classpath\"></stringProp>");
			this.stringBuilder.Append("</TestPlan>");
			this.stringBuilder.Append("<hashTree>");
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append("</hashTree>");
			this.stringBuilder.Append("<WorkBench guiclass=\"WorkBenchGui\" testclass=\"WorkBench\" testname=\"工作台\" enabled=\"true\">");
			this.stringBuilder.Append("<boolProp name=\"WorkBench.save\">true</boolProp>");
			this.stringBuilder.Append("</WorkBench>");
			this.stringBuilder.Append("<hashTree/>");
			this.stringBuilder.Append("</hashTree>");
			this.stringBuilder.Append("</PostmanTestPlan>");
			return this.stringBuilder.ToString();
		}

		public string addConfigTestElement(string content, string ip, string port, string encoding = "utf-8", string protocol = "http", string elementName = "HTTP请求默认值")
		{
			this.stringBuilder = new StringBuilder();
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append(string.Format("<ConfigTestElement guiclass=\"HttpDefaultsGui\" testclass=\"ConfigTestElement\" testname = \"{0}\" enabled = \"true\">", elementName));
			this.stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\" guiclass=\"HTTPArgumentsPanel\" testclass=\"Arguments\" testname=\"用户定义的变量\" enabled=\"true\">");
			this.stringBuilder.Append("<collectionProp name=\"Arguments.arguments\"/>");
			this.stringBuilder.Append("</elementProp>");
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", ip));
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", port));
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", protocol));
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.contentEncoding\">{0}</stringProp>", encoding));
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.path\"></stringProp>");
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.concurrentPool\">6</stringProp>");
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
			this.stringBuilder.Append("</ConfigTestElement>");
			this.stringBuilder.Append("<hashTree/>");
			return this.stringBuilder.ToString();
		}

		public string addCookieManager(string content, string elementName = "HTTP Cookie 管理器")
		{
			this.stringBuilder = new StringBuilder();
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append(string.Format("<CookieManager guiclass=\"CookiePanel\" testclass=\"CookieManager\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append("<collectionProp name=\"CookieManager.cookies\"/>");
			this.stringBuilder.Append("<boolProp name=\"CookieManager.clearEachIteration\">false</boolProp>");
			this.stringBuilder.Append("</CookieManager>");
			this.stringBuilder.Append("<hashTree/>");
			return this.stringBuilder.ToString();
		}

		public string addArguments(string content, string elementName = "用户定义的变量")
		{
			this.stringBuilder = new StringBuilder();
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append(string.Format("<Arguments guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append("<collectionProp name=\"Arguments.arguments\"/>");
			this.stringBuilder.Append("</Arguments>");
			this.stringBuilder.Append("<hashTree/>");
			return this.stringBuilder.ToString();
		}

		public string surroundByThreadGroup(string content, string elementName = "线程组")
		{
			this.stringBuilder = new StringBuilder();
			this.stringBuilder.Append(string.Format("<ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append("<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp>");
			this.stringBuilder.Append("<elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"循环控制器\" enabled=\"true\">");
			this.stringBuilder.Append("<boolProp name=\"LoopController.continue_forever\">false</boolProp>");
			this.stringBuilder.Append("<stringProp name=\"LoopController.loops\">1</stringProp>");
			this.stringBuilder.Append("</elementProp>");
			this.stringBuilder.Append("<stringProp name=\"ThreadGroup.num_threads\">1</stringProp>");
			this.stringBuilder.Append("<stringProp name=\"ThreadGroup.ramp_time\">1</stringProp>");
			long num = Convert.ToInt64(DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds);
			this.stringBuilder.Append(string.Format("<longProp name=\"ThreadGroup.start_time\">{0}</longProp>", num));
			this.stringBuilder.Append(string.Format("<longProp name=\"ThreadGroup.end_time\">{0}</longProp>", num));
			this.stringBuilder.Append("<boolProp name=\"ThreadGroup.scheduler\">false</boolProp>");
			this.stringBuilder.Append("<stringProp name=\"ThreadGroup.duration\"></stringProp>");
			this.stringBuilder.Append("<stringProp name=\"ThreadGroup.delay\"></stringProp>");
			this.stringBuilder.Append("</ThreadGroup>");
			this.stringBuilder.Append("<hashTree>");
			this.stringBuilder.Append(content);
			this.stringBuilder.Append("</hashTree>");
			return this.stringBuilder.ToString();
		}

		public string addViewResultTree(string content, string elementName = "察看结果树")
		{
			this.stringBuilder = new StringBuilder();
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append(string.Format("<ResultCollector guiclass = \"ViewResultsFullVisualizer\" testclass=\"ResultCollector\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append("<boolProp name=\"ResultCollector.error_logging\">false</boolProp>");
			this.stringBuilder.Append("<objProp>");
			this.stringBuilder.Append("<name>saveConfig</name>");
			this.stringBuilder.Append("<value class=\"SampleSaveConfiguration\">");
			this.stringBuilder.Append("<time>true</time>");
			this.stringBuilder.Append("<latency>true</latency>");
			this.stringBuilder.Append("<timestamp>true</timestamp>");
			this.stringBuilder.Append("<success>true</success>");
			this.stringBuilder.Append("<success>true</success>");
			this.stringBuilder.Append("<label>true</label>");
			this.stringBuilder.Append("<code>true</code>");
			this.stringBuilder.Append("<message>true</message>");
			this.stringBuilder.Append("<threadName>true</threadName>");
			this.stringBuilder.Append("<dataType>true</dataType>");
			this.stringBuilder.Append("<encoding>false</encoding>");
			this.stringBuilder.Append("<assertions>true</assertions>");
			this.stringBuilder.Append("<subresults>true</subresults>");
			this.stringBuilder.Append("<responseData>false</responseData>");
			this.stringBuilder.Append("<samplerData>false</samplerData>");
			this.stringBuilder.Append("<xml>false</xml>");
			this.stringBuilder.Append("<fieldNames>true</fieldNames>");
			this.stringBuilder.Append("<responseHeaders>false</responseHeaders>");
			this.stringBuilder.Append("<requestHeaders>false</requestHeaders>");
			this.stringBuilder.Append("<responseDataOnError>false</responseDataOnError>");
			this.stringBuilder.Append("<saveAssertionResultsFailureMessage>true</saveAssertionResultsFailureMessage>");
			this.stringBuilder.Append("<assertionsResultsToSave>0</assertionsResultsToSave>");
			this.stringBuilder.Append("<bytes>true</bytes>");
			this.stringBuilder.Append("<sentBytes>true</sentBytes>");
			this.stringBuilder.Append("<threadCounts>true</threadCounts>");
			this.stringBuilder.Append("<idleTime>true</idleTime>");
			this.stringBuilder.Append("<connectTime>true</connectTime>");
			this.stringBuilder.Append("</value>");
			this.stringBuilder.Append("</objProp>");
			this.stringBuilder.Append("<stringProp name=\"filename\"></stringProp>");
			this.stringBuilder.Append("</ResultCollector>");
			this.stringBuilder.Append("<hashTree/>");
			return this.stringBuilder.ToString();
		}

		public string addAssertionResult(string content, string elementName = "断言结果")
		{
			this.stringBuilder = new StringBuilder();
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append(string.Format("<ResultCollector guiclass=\"AssertionVisualizer\" testclass=\"ResultCollector\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append("<boolProp name=\"ResultCollector.error_loggin\">false</boolProp>");
			this.stringBuilder.Append("<objProp>");
			this.stringBuilder.Append("<name>saveConfig</name>");
			this.stringBuilder.Append("<value class=\"SampleSaveConfiguration\">");
			this.stringBuilder.Append("<time>true</time>");
			this.stringBuilder.Append("<latency>true</latency>");
			this.stringBuilder.Append("<timestamp>true</timestamp>");
			this.stringBuilder.Append("<success>true</success>");
			this.stringBuilder.Append("<label>true</label>");
			this.stringBuilder.Append("<code>true</code>");
			this.stringBuilder.Append("<message>true</message>");
			this.stringBuilder.Append("<threadName>true</threadName>");
			this.stringBuilder.Append("<dataType>true</dataType>");
			this.stringBuilder.Append("<encoding>false</encoding>");
			this.stringBuilder.Append("<assertions>true</assertions>");
			this.stringBuilder.Append("<subresults>true</subresults>");
			this.stringBuilder.Append("<responseData>false</responseData>");
			this.stringBuilder.Append("<samplerData>false</samplerData>");
			this.stringBuilder.Append("<xml>false</xml>");
			this.stringBuilder.Append("<fieldNames>true</fieldNames>");
			this.stringBuilder.Append("<responseHeaders>false</responseHeaders>");
			this.stringBuilder.Append("<requestHeaders>false</requestHeaders>");
			this.stringBuilder.Append("<responseDataOnError>false</responseDataOnError>");
			this.stringBuilder.Append("<saveAssertionResultsFailureMessage>true</saveAssertionResultsFailureMessage>");
			this.stringBuilder.Append("<assertionsResultsToSave>0</assertionsResultsToSave>");
			this.stringBuilder.Append("<bytes>true</bytes>");
			this.stringBuilder.Append("<sentBytes>true</sentBytes>");
			this.stringBuilder.Append("<threadCounts>true</threadCounts>");
			this.stringBuilder.Append("<idleTime>true</idleTime>");
			this.stringBuilder.Append("<connectTime>true</connectTime>");
			this.stringBuilder.Append("</value>");
			this.stringBuilder.Append("</objProp>");
			this.stringBuilder.Append("<stringProp name=\"filename\"></stringProp>");
			this.stringBuilder.Append("</ResultCollector>");
			this.stringBuilder.Append("<hashTree/>");
			return this.stringBuilder.ToString();
		}

		public string surroundByHTTPSamplerProxy(string content, string ip, string port, string protocol, string method, string path, string value, string elementName)
		{
			this.stringBuilder = new StringBuilder();
			this.stringBuilder.Append(string.Format("<HTTPSamplerProxy guiclass=\"HttpTestSampleGui\" testclass=\"HTTPSamplerProxy\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append("<boolProp name=\"HTTPSampler.postBodyRaw\">true</boolProp>");
			this.stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\">");
			this.stringBuilder.Append("<collectionProp name=\"Arguments.arguments\">");
			this.stringBuilder.Append("<elementProp name=\"\" elementType=\"HTTPArgument\">");
			this.stringBuilder.Append("<boolProp name=\"HTTPArgument.always_encode\">false</boolProp>");
			this.stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", value));
			this.stringBuilder.Append("<stringProp name=\"Argument.metadata\">=</stringProp>");
			this.stringBuilder.Append("</elementProp>");
			this.stringBuilder.Append("</collectionProp>");
			this.stringBuilder.Append("</elementProp>");
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", ip));
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", port));
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", protocol));
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.contentEncoding\"></stringProp>");
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.path\">{0}</stringProp>", path));
			this.stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.method\">{0}</stringProp>", method));
			this.stringBuilder.Append("<boolProp name=\"HTTPSampler.follow_redirects\">true</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"HTTPSampler.auto_redirects\">false</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"HTTPSampler.use_keepalive\">true</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"HTTPSampler.DO_MULTIPART_POST\">false</boolProp>");
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.embedded_url_re\"></stringProp>");
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
			this.stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
			this.stringBuilder.Append("</HTTPSamplerProxy>");
			this.stringBuilder.Append("<hashTree>");
			this.stringBuilder.Append(content);
			this.stringBuilder.Append("</hashTree>");
			return this.stringBuilder.ToString();
		}

		public string addJSONPathAssertion(string content, string jsonPath = "$.stat", string expectedValue = "OK", string elementName = "验证响应结果")
		{
			this.stringBuilder = new StringBuilder();
			bool flag = content != null;
			if (flag)
			{
				this.stringBuilder.Append(content);
			}
			this.stringBuilder.Append(string.Format("<com.atlantbh.Postman.plugins.jsonutils.jsonpathassertion.JSONPathAssertion guiclass = \"com.atlantbh.Postman.plugins.jsonutils.jsonpathassertion.gui.JSONPathAssertionGui\" testclass=\"com.atlantbh.Postman.plugins.jsonutils.jsonpathassertion.JSONPathAssertion\" testname=\"{0}\" enabled=\"true\">", elementName));
			this.stringBuilder.Append(string.Format("<stringProp name=\"JSON_PATH\">{0}</stringProp>", jsonPath));
			this.stringBuilder.Append(string.Format("<stringProp name=\"EXPECTED_VALUE\">{0}</stringProp>", expectedValue));
			this.stringBuilder.Append("<boolProp name=\"JSONVALIDATION\">true</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"EXPECT_NULL\">false</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"INVERT\">false</boolProp>");
			this.stringBuilder.Append("<boolProp name=\"ISREGEX\">true</boolProp>");
			this.stringBuilder.Append("</com.atlantbh.Postman.plugins.jsonutils.jsonpathassertion.JSONPathAssertion>");
			this.stringBuilder.Append("<hashTree/>");
			return this.stringBuilder.ToString();
		}
	}
}
