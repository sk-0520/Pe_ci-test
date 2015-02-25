﻿namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	using System;
	using System.Linq;
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.Library.Utility;
	using NUnit.Framework;

	[TestFixture]
	class T4TemplateProcessorTest
	{
		[TestCase("", "", "", true)]
		[TestCase(" ", "", "", true)]
		[TestCase("", " ", "", true)]
		[TestCase("a", "", "", true)]
		[TestCase("a", " ", "", true)]
		[TestCase("", "a", "", true)]
		[TestCase(" ", "a", "", true)]
		[TestCase("a", "a", "a", true)]
		[TestCase("a", "a", "<#@ template language=\"C#\" #>", false)]
		public void GeneratSource_ErrorTest(string name, string cls, string ts, bool isError)
		{
			var t4 = new T4TemplateProcessor() {
				NamespaceName = name,
				ClassName = cls,
				TemplateSource = ts,
			};
			bool hasError;
			try {
				t4.GeneratSource();
				hasError = t4.GeneratedErrorList.Count > 0;
			} catch(InvalidOperationException ex) {
				Debug.WriteLine(ex);
				hasError = true;
			}
			Assert.IsTrue(hasError == isError);
		}

		[TestCase(false, " ")]
		[TestCase(true, "<#@ template language=\"C#\" #>")]
		[TestCase(true, @"
<#@ template language=""C#"" #>
test
")]
		[TestCase(false, @"
<#@ template language=""C#"" #>
<# test #>
")]
		[TestCase(false, @"
<#@ template language=""C#"" #>
<#= test #>
")]
		[TestCase(true, @"
<#@ template language=""C#"" #>
<#= ""test"" #>
")]
		[TestCase(true, @"
<#@ template language=""C#"" #>
<# var foo = ""bar""; #>
foo is <#= foo #>
")]
		public void CompileSource_ErrorTest(bool test, string ts)
		{
			Debug.Assert(!string.IsNullOrEmpty(ts), "GeneratSource_ErrorTest!");

			var t4 = new T4TemplateProcessor() {
				NamespaceName = "a",
				ClassName = "b",
				TemplateSource = ts,
			};

			t4.GeneratSource();
			bool hasError;
			try {
				t4.CompileSource();
				hasError = t4.CompileErrorList.Count > 0;
			} catch(InvalidOperationException) {
				hasError = true;
			}
			if(hasError) {
				Debug.WriteLine("T: " +  string.Join(Environment.NewLine, t4.GeneratedErrorList.Select(e => e.ToString())));
				Debug.WriteLine("S: " + string.Join(Environment.NewLine, t4.CompileErrorList.Select(e => e.ToString())));
			} else {
				Debug.WriteLine(t4.GeneratedSource);
			}

			Assert.IsTrue(hasError != test);
		}

		[TestCase("1", "1")]
		[TestCase("<#= 1+1 #>", "2")]
		public void TransformText_CSharpSimpleTest(string src, string result)
		{
			var ts = "<#@ template language=\"C#\" #>" + Environment.NewLine + src;
			var t4 = new T4TemplateProcessor() {
				NamespaceName = "a",
				ClassName = "b",
				TemplateSource = ts,
			};
			t4.GeneratSource();
			t4.CompileSource();
			var output = t4.TransformText();
			Assert.IsTrue(output == result);
		}

		[TestCase("host", "host")]
		[TestCase("<#= host.Session[\"a\"] #>", "123")]
		[TestCase("<#= host.Session[\"A\"] #>", "123")]
		[TestCase("<#= (string)host.Session[\"a\"] + 4 #>", "1234")]
		[TestCase("<#= (int)host.Session[\"A\"] + 4 #>", "127")]
		public void TransformText_CSharpHostTest(string src, string result)
		{
			var ts
				= "<#@ template language=\"C#\" debug=\"true\" hostSpecific=\"true\" #>" + Environment.NewLine
				+ "<# var host = (Microsoft.VisualStudio.TextTemplating.ITextTemplatingSessionHost) Host; #>"+ Environment.NewLine
				//+ "<# var host = (ContentTypeTextNet.Pe.Library.Utility.TextTemplatingSession) Host; #>" + Environment.NewLine
				+ src;
			
			var t4 = new T4TemplateProcessor() {
				NamespaceName = "a",
				ClassName = "b",
				TemplateSource = ts,
			};
			var session = t4.Variable;
			session["a"] = "123";
			session["A"] = 123;
			t4.GeneratSource();
			t4.CompileSource();
			var output = t4.TransformText();
			Assert.IsTrue(output == result);
		}
	}


	[TestFixture]
	class T4TemplateTest
	{
		[Test]
		public void test()
		{
			var s = @"
<#@ template language=""C#"" debug=""true"" hostSpecific=""true"" #>
...
<#
// ""Host""変数を有効とするために、hostSpecific=""true"" とすること。
var sessionHost = (Microsoft.VisualStudio.TextTemplating.ITextTemplatingSessionHost) Host;
var mx = (int)sessionHost.Session[""maxCount""];
#>
へんすう<#= mx #>。
			";
			var r = T4TemplateUtility.Convert(s);
			Debug.WriteLine(r);
		}
	}
}
