using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTester.Helpers.CSharp;
using RoslynTester.Helpers.Testing;
using Tests.SampleAnalyzerWithErrorSeverity;

namespace Tests.Tests;

public class SampleAnalyzerWithErrorSeverityTests : CSharpCodeFixVerifier
{
    protected override CodeFixProvider CodeFixProvider => new TestCodeFixWithErrorSeverity();

    protected override DiagnosticAnalyzer DiagnosticAnalyzer => new TestAnalyzerWithErrorSeverity();

    [Test]
    public void Analyzer_With_MessageDiagnostic_DoesNotThrowUncompilable()
    {
        var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            async Task Method()
            {
                
            }
        }
    }";

        var result = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            async Task MethodAsync()
            {
                
            }
        }
    }";

        VerifyDiagnostic(original, string.Format(TestAnalyzerWithErrorSeverity.Message, "Method"));
        VerifyFix(original, result);
    }

    [Test]
    public void Analyzer_WithUnCompilableCode_ThrowsException()
    {
        var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        classicqsddsdqdsdsqddqqddq MyClass
        {   
            async Task Method()
            {
                
            }
        }
    }";

        var result = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        classicqsddsdqdsdsqddqqddq MyClass
        {   
            async Task MethodAsync()
            {
                
            }
        }
    }";

        VerifyDiagnostic(original, string.Format(TestAnalyzerWithErrorSeverity.Message, "Method"));
        Assert.Throws<InvalidCodeException>(() => VerifyFix(original, result));
    }
}