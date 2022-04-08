using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTester.DiagnosticResults;
using RoslynTester.Helpers.CSharp;
using RoslynTester.Helpers.Testing;
using Tests.SampleAnalyzer;

namespace Tests.Tests;

public class SampleAnalyzerTests : CSharpCodeFixVerifier
{
    protected override CodeFixProvider CodeFixProvider => new TestCodeFix();

    protected override DiagnosticAnalyzer DiagnosticAnalyzer => new TestAnalyzer();

    [Test]
    public void Analyzer_With_LocationDiagnostic()
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

        var expectedDiagnostic = new DiagnosticResult
        {
            Id = TestAnalyzer.DiagnosticId,
            Message = string.Format(TestAnalyzer.Message, "Method"),
            Severity = TestAnalyzer.Severity,
            Locations =
                new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 10, 24)
                }
        };

        VerifyDiagnostic(original, expectedDiagnostic);
        VerifyFix(original, result);
    }

    [Test]
    public void Analyzer_With_MessageDiagnostic()
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

        VerifyDiagnostic(original, string.Format(TestAnalyzer.Message, "Method"));
        VerifyFix(original, result);
    }

    [Test]
    public void Analyzer_ThrowsExceptionForBrokenCode_Diagnostic()
    {
        var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            async Task Method(
            {
                
            }
        }
    }";

        Assert.Throws<InvalidCodeException>(() => VerifyDiagnostic(original, string.Format(TestAnalyzer.Message, "Method")));
    }

    [Test]
    public void Analyzer_ThrowsExceptionForBrokenCode_CodeFix()
    {
        var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            async Task Method(
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
            async Task MethodAsync(
            {
                
            }
        }
    }";

        Assert.Throws<InvalidCodeException>(() => VerifyFix(original, result));
    }

    [Test]
    public void Analyzer_Without_Diagnostic()
    {
        var original = @"
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
        VerifyDiagnostic(original);
    }

    [Test]
    public void Analyzer_With_MessageDiagnostic_FixProducesNewDiagnostic()
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

        VerifyDiagnostic(original, string.Format(TestAnalyzer.Message, "Method"));
        VerifyFix(original, result);
    }
}