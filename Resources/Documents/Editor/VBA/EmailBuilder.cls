Option Compare Database
Option Explicit

Private m_Error As String
Private m_Sender As String
Private m_Reciever As String
Private m_Subject As String
Private m_Copy() As String
Private m_Body As String





'----------------------------------------------------------------------------------
'   Type        Contructor/Finalizer
'   Name        Class_Initialize()/Class_Terminate
'   Parameters  Void
'   Retval      AccessData
'   Purpose
'---------------------------------------------------------------------------------
Public Sub Class_Initialize()
End Sub

Public Sub Class_Terminate()
End Sub




'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  String
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Sender(Value As String)
    On Error GoTo ErrorHandler:
    If Not Value & "" = "" Then
        m_Sender = Value
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Sender"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  Void
'   RetVal:      String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Sender() As String
    On Error GoTo ErrorHandler:
    If Not m_Sender & "" = "" Then
        Sender = m_Sender
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Sender"
    Exit Property
End Property







'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  String
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Reciever(Value As String)
    On Error GoTo ErrorHandler:
    If Not Value & "" = "" Then
        m_Reciever = Value
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Receiver"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  Void
'   RetVal:      String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Reciever() As String
    On Error GoTo ErrorHandler:
    If Not m_Reciever & "" = "" Then
        Reciever = m_Reciever
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Reciever"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  String
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Subject(Value As String)
    On Error GoTo ErrorHandler:
    If Not Value & "" = "" Then
        m_Subject = Value
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Subject"
    Exit Property
End Property


'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  Void
'   RetVal:      String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Subject() As String
    On Error GoTo ErrorHandler:
    If Not m_Subject & "" = "" Then
        Subject = m_Subject
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Subject"
    Exit Property
End Property







'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  String
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Body(Value As String)
    On Error GoTo ErrorHandler:
    If Not Value & "" = "" Then
        m_Body = Value
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Body"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:        Property
'   Name:
'   Parameters:  Void
'   RetVal:      String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Body() As String
    On Error GoTo ErrorHandler:
    If Not m_Body & "" = "" Then
        Body = m_Body
    End If
ErrorHandler:
    ProcessError Name:="EmailBuilder", _
        Member:="Body"
    Exit Property
End Property











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



