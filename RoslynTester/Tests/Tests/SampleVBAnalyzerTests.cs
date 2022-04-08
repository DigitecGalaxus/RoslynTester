using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTester.Helpers.VisualBasic;
using Tests.SampleVBAnalyzer;

namespace Tests.Tests;

public class AttributeWithEmptyArgumentListTests : VisualBasicCodeFixVerifier
{
    protected override DiagnosticAnalyzer DiagnosticAnalyzer => new VBTestAnalyzer();

    protected override CodeFixProvider CodeFixProvider => new VBTestCodeFix();

    [Test]
    public void Analyzer_WithVisualBasicCode_WithCodeFix()
    {
        var original = @"
Imports System

Module Module1
    <Obsolete()>
    Sub Foo()
    End Sub

End Module";

        var result = @"
Imports System

Module Module1
    <Obsolete>
    Sub Foo()
    End Sub

End Module";

        VerifyDiagnostic(original, VBTestAnalyzer.Rule.MessageFormat.ToString());
        VerifyFix(original, result);
    }

    [Test]
    public void Analyzer_WithVisualBasicCode_WithDiagnostic()
    {
        var original = @"
Imports System

Module Module1
    <Obsolete>
    Sub Foo()
    End Sub

End Module";

        VerifyDiagnostic(original);
    }
}