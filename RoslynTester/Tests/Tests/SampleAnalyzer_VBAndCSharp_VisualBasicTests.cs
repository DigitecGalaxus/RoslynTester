﻿using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynTester.Helpers.VisualBasic;
using Tests.SampleAnalyzer_VBAndCSharp;

namespace Tests.Tests;

public class SampleAnalyzer_VBAndCSharp_VisualBasicTests : VisualBasicCodeFixVerifier
{
    protected override DiagnosticAnalyzer DiagnosticAnalyzer => new SampleAnalyzer_VBAndCSharpAnalyzer();

    protected override CodeFixProvider CodeFixProvider => new SampleAnalyzer_VBAndCSharpCodeFix();

    [Test]
    public void SampleAnalyzer_AddsFlagsAttribute()
    {
        var original = @"
Module Module1
    Enum Foo
        Bar
        Baz
    End Enum
End Module";

        var result = @"
Imports System
Module Module1
    <Flags>
    Enum Foo
        Bar
        Baz
    End Enum
End Module";

        VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
        VerifyFix(original, result);
    }

    [Test]
    public void SampleAnalyzer_AddsFlagsAttribute_DoesNotAddDuplicateUsingSystem()
    {
        var original =
            @"Imports System
Module Module1
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        var result =
            @"Imports System
Module Module1
    <Flags>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
        VerifyFix(original, result);
    }

    [Test]
    public void SampleAnalyzer_AddsFlagsAttribute_OnlyAddsFlagsAttribute()
    {
        var original = @"
Imports System
Module Module1
    <Obsolete(""I'm obsolete"")>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        var result = @"
Imports System
Module Module1
    <Obsolete(""I'm obsolete"")>
    <Flags>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
        VerifyFix(original, result);
    }

    [Test]
    public void SampleAnalyzer_EnumHasXmlDocComment_OnlyAddsFlagsAttribute()
    {
        var original = @"
Module Module1
    ''' <summary>
    ''' Doc comment for Foo...
    ''' </summary>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        var result = @"
Imports System
Module Module1
    ''' <summary>
    ''' Doc comment for Foo...
    ''' </summary>
    <Flags>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
        VerifyFix(original, result);
    }

    [Test]
    public void SampleAnalyzer_AddsFlagsAttribute_AddsUsingSystemWhenUsingSystemDotAnything()
    {
        var original =
            @"Imports System.Text
Module Module1
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        var result =
            @"Imports System.Text
Imports System
Module Module1
    <Flags>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
        VerifyFix(original, result);
    }

    [Test]
    public void SampleAnalyzer_InspectionDoesNotReturnWhenFlagsAlreadyApplied()
    {
        var original = @"
Imports System
Module Module1
    <Flags>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original);
    }

    [Test]
    public void SampleAnalyzer_InspectionDoesNotReturnWhenFlagsAttributeAlreadyApplied()
    {
        var original = @"
Imports System
Module Module1
    <FlagsAttribute>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original);
    }

    [Test]
    public void SampleAnalyzer_InspectionDoesNotReturnWhenFlagsAlreadyAppliedAsChain()
    {
        var original = @"
Imports System
Module Module1
    <Obsolete(""I'm obsolete""), Flags>
    Enum Foo
        Bar
        Baz
    End Enum

End Module";

        VerifyDiagnostic(original);
    }
}