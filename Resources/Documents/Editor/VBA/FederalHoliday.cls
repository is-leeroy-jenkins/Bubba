Option Compare Database
Option Explicit



Private m_Base As String
Private m_ID As Integer
Private m_Name As String
Private m_Date As Date
Private m_Names() As String
Private m_Dates() As Date
Private m_WeekDay As Boolean
Private m_DayOfWeek As String
Private m_Error As String
Private m_Notification As String
















'----------------------------------------------------------------------------------
'   Type:        Constructor/Finalizer
'   Name:
'   Parameters:  Void
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
End Sub

Private Sub Class_Terminate( )
End Sub

'---------------------------------------------------------------------------------
'   Type            Property-Let
'   Name            Base
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let Base(Value  As String)
    On Error GoTo ErrorHandler:
    If Not Value  & "" = "" Then
        m_Base = Value 
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="Base"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type            Property-Get
'   Name            Base
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get Base() As String
    On Error GoTo ErrorHandler:
    If Not m_Base & "" = "" Then
        Base = m_Base
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="Base"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type            Property-Let
'   Name            ID
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let ID(Value  As Integer)
    On Error GoTo ErrorHandler:
    If Value  > 0 Then
        m_ID = Value 
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="ID"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type            Property-Get
'   Name            ID
'   Parameters      Void
'   Return          String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get ID() As Integer
    On Error GoTo ErrorHandler:
    If m_ID > 0 Then
        ID = m_ID
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="ID"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type            Property-Let
'   Name            Name
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let Name(Value  As String)
    On Error GoTo ErrorHandler:
    If Not Value  & "" = "" Then
        m_Name = Value 
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="Name"
    Exit Property
End Property


'---------------------------------------------------------------------------------
'   Type            Property-Let
'   Name            Name
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get Name() As String
    On Error GoTo ErrorHandler:
    If Not m_Name & "" = "" Then
        Name = m_Name
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="Name"
    Exit Property
End Property







'---------------------------------------------------------------------------------
'   Type            Property-Let
'   Name            Day()
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let Day(Value  As Date)
    On Error GoTo ErrorHandler:
    If IsDate(Value ) Then
        m_Date = Value 
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="Day"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type            Property-Let
'   Name            Day
'   Parameters      Void
'   Return          Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get Day() As Date
    On Error GoTo ErrorHandler:
    If IsDate(m_Date) Then
        Day = m_Date
    End If
ErrorHandler:
    ProcessError Name:="FederalHoliday", _
        Member:="Day"
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


