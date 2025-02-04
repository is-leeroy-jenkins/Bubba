Option Compare Database


Public pid As Variant




'----------------------------------------------------------------------------------
'   Type        Function
'   Name        IsProcessRunning
'   Parameters  String - AppName
'   Purpose     Function to check for running application by its process name
'---------------------------------------------------------------------------------
Function IsProcessRunning(AppName As String)
    On Error GoTo ErrorHandler
        Dim objList As Object
        Set objList = GetObject("winmgmts:").ExecQuery("Select ProcessID from Win32_Process where Name='" & AppName & "'")
        If objList.Count > 0 Then
            IsProcessRunning = True
            For Each objProcess In objList
                If pid <> objProcess.ProcessID Then
                    pid = objProcess.ProcessID
                    Exit Function
                End If
            Next
        Else
            IsProcessRunning = False
            Exit Function
        End If
ErrorHandler:
    IsProcessRunning = False
    ProcessError Name:="Calculator", _
        Member:="IsProcessRunning"
        Exit Function
End Function






'----------------------------------------------------------------------------------
'   Type        SubProcedure
'   Name        Calculate
'   Parameters  Void
'   Purpose     Launches the Windows 10 calculator 'calc.exe'
'----------------------------------------------------------------------------------

Sub Calculate()
    On Error GoTo ErrorHandler:
    If IsProcessRunning("calc.exe") = True Then
        On Error GoTo Reload                ' Open new instance of calculator in event of error
        AppActivate (vPid)                  ' Reactivate calculator process using Public declared variant
        SendKeys "%{Enter}"                 ' Bring it back into focus if user minimises it
    Else
Reload:
        vPid = Shell("calc.exe", 1)         ' Run Calculator
    End If
ErrorHandler:
    ProcessError Name:="Calculator", _
        Member:="Calculate"
        Exit Sub
End Sub













'----------------------------------------------------------------------------------
'   Type:        Event Sub-Procedure
'   Name:        Process
'   Parameters:  Void
'   Retval:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub ProcessError(Optional Name As String, Optional Member As String)
    If Err.Number <> 0 And _
        Not IsMissing(Name) And _
        Not IsMissing(Member) Then
            m_Error = "Source:      " & Err.Source _
                & vbCrLf & "Number:     " & Err.Number _
                & vbCrLf & "Issue:      " & Err.Description _
                & vbCrLf & "Class:      " & Name _
                & vbCrLf & "Member:     " & Member
    End If
    If Err.Number <> 0 And _
        IsMissing(Name) And _
        IsMissing(Member) Then
            m_Error = "Source:      " & Err.Source _
                & vbCrLf & "Number:     " & Err.Number _
                & vbCrLf & "Issue:      " & Err.Description
    End If
    MessageFactory.ShowError (m_Error)
    Err.Clear
End Sub


