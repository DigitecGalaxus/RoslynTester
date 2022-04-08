using System;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTester.Helpers.CSharp;

namespace Tests.Temp;

public class UnitTest1 : CSharpDiagnosticVerifier
{
    [Test]
    public void RedundantPrivateSetter_PartialClass_IrrelevantIdentifier()
    {
        var firstTree = @"
namespace ConsoleApplication1
{
    partial class MyClass
    {
        public int MyProperty { get; private set; }

        public MyClass()
        {
            MyProperty = 42;
        }
    }
}";

        var secondTree = @"
namespace ConsoleApplication1
{
    partial class MyClass
    {
        public void MyMethod()
        {
            var MyProperty = 42;
        }
    }
}";

        VerifyDiagnostic(new[] { firstTree, secondTree }, string.Format(RedundantPrivateSetterAnalyzer.Rule.MessageFormat.ToString(), "MyProperty"));
    }

    protected override DiagnosticAnalyzer DiagnosticAnalyzer => new RedundantPrivateSetterAnalyzer();
}