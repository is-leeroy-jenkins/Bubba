Attribute VB_Name = "Strings"
Option Compare Database
Option Explicit


Private m_Error As String





'---------------------------------------------------------------------------------
'   Type            Function
'   Name            ToProperCase
'   Parameters      Void
'   Return          Void
'   Purpose
'                   Capitalize the first character and add a space before
'                   each capitalized letter (except the first character).
'---------------------------------------------------------------------------------
Public Static Function ToProperCase(ByVal pString As String) As String
    On Error GoTo ErrorHandler:
    Dim p_Result As String
    Dim i As Integer
    Dim ch As String
    If Len(pString) < 2 Then
        ToProperCase = UCase$(pString)
        Exit Function
    End If
    p_Result = UCase$(mID$(pString, 1, 1))
    For i = 2 To Len(pString)
        ch = mID$(pString, i, 1)
        If (UCase$(ch) = ch) Then p_Result = p_Result & " "
        p_Result = p_Result & ch
    Next i
    ToProperCase = p_Result
ErrorHandler:
    ProcessError
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type            Function
'   Name            ToCamelCase
'   Parameters      Void
'   Return          Void
'   Purpose
'                   Capitalize the first character and add a space before
'                   each capitalized letter (except the first character).
'---------------------------------------------------------------------------------
Public Function ToCamelCase(ByVal pString As String) As String
    On Error GoTo ErrorHandler:
    Dim p_Result As String
    p_Result = ToPascalCase(pString)
    If Len(p_Result) > 0 Then
        Mid$(p_Result, 1, 1) = LCase$(mID$(p_Result, 1, 1))
    End If
    ToCamelCase = p_Result
ErrorHandler:
    ProcessError
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type            Function
'   Name            ToPascalCase
'   Parameters      Void
'   Return          Void
'   Purpose
'                   Capitalize the first character and add a space before
'                   each capitalized letter (except the first character).
'---------------------------------------------------------------------------------
Public Function ToPascalCase(ByVal pString As String) As String
    On Error GoTo ErrorHandler:
    Dim p_Words() As String
    Dim i As Integer
    If Len(pString) < 2 Then
        ToPascalCase = UCase$(pString)
        Exit Function
    End If
    p_Words = Split(pString)
    For i = LBound(p_Words) To UBound(p_Words)
        If (Len(p_Words(i)) > 0) Then
            Mid$(p_Words(i), 1, 1) = UCase$(mID$(p_Words(i), 1, _
                1))
        End If
    Next i
    ToPascalCase = Join(p_Words, "")
ErrorHandler:
    ProcessError
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type            Function
'   Name            SearchArray
'   Parameters      Void
'   Return          Void
'   Purpose
'                   Capitalize the first character and add a space before
'                   each capitalized letter (except the first character).
'---------------------------------------------------------------------------------
Public Function SearchArray(pArray As Variant, pString As String) As Integer
    Dim p_Search As Integer
    p_Search = -1
    Dim i As Integer
    For i = LBound(pArray) To UBound(pArray)
        If pString = pArray(i) Then
            p_Search = i
            Exit For
        End If
    Next i
ErrorHandler:
    ProcessError
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type            Function
'   Name            SplitToArray
'   Parameters      Void
'   Return          Void
'   Purpose
'                   Capitalize the first character and add a space before
'                   each capitalized letter (except the first character).
'---------------------------------------------------------------------------------
Public Function SplitIntoArray(pString As String, m_Separator As String) As Variant
    Dim p_StringArray As Variant
    If Len(pString) > 0 Then
        p_StringArray = Split(pString, m_Separator)
        Dim i As Integer
        For i = LBound(p_StringArray) To UBound(p_StringArray)
            p_StringArray(i) = Trim(p_StringArray(i))
        Next i
    Else
        p_StringArray = Array()
    End If
    SplitIntoArray = p_StringArray
ErrorHandler:
    ProcessError
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type:        Sub-Procedure
'   Name:        ProcessError
'   Parameters:  Void
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub ProcessError()
    If Err.Number <> 0 Then
        m_Error = "Source:      " & Err.Source _
            & vbCrLf & "Number:     " & Err.Number _
            & vbCrLf & "Issue:      " & Err.Description
        Err.Clear
    End If
    MessageFactory.ShowError (m_Error)
End Sub



