VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Employee"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Compare Database
Option Explicit




Private m_HireDate As String
Private m_RpioCode As String
Private m_HROrgCode As String
Private m_RcCode As String
Private m_EmployeeNumber As String
Private m_LastName As String
Private m_FirstName As String
Private m_PayGrade As String
Private m_Step As String
Private m_JobTitle As String
Private m_Error As String





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





'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        RpioCode
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let RpioCode(rp As String)
    On Error GoTo ErrorHandler:
    If Not rp & "" = "" Then
        m_RpioCode = rp
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property


'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        RpioCode
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get RpioCode() As String
    On Error GoTo ErrorHandler:
    If Not m_RpioCode & "" = "" Then
        RpioCode = m_RpioCode
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property




'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        HrOrgCode
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let HrOrgCode(oc As String)
    On Error GoTo ErrorHandler:
    If Not oc & "" = "" Then
       m_HROrgCode = oc
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        HrOrgCode
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get HrOrgCode() As String
    On Error GoTo ErrorHandler:
    If Not m_HROrgCode & "" = "" Then
        HrOrgCode = m_HROrgCode
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property




'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        EmployeeNumber
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let EmployeeNumber(eid As String)
    On Error GoTo ErrorHandler:
    If eid & "" = "" Then
        m_EmployeeNumber = eid
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property





'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        EmployeeNumber
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get EmployeeNumber() As String
    On Error GoTo ErrorHandler:
    If Not m_EmployeeNumber & "" = "" Then
        EmployeeNumber = m_EmployeeNumber
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        LastName
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let LastName(ln As String)
    On Error GoTo ErrorHandler:
    If ln & "" = "" Then
        m_LastName = ln
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        LastName
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get LastName() As String
    On Error GoTo ErrorHandler:
    If Not m_LastName & "" = "" Then
        LastName = m_LastName
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        FirstName
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let FirstName(fn As String)
    On Error GoTo ErrorHandler:
    If fn & "" = "" Then
    m_FirstName = fn
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        FirstName
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get FirstName() As String
    On Error GoTo ErrorHandler:
    If Not m_FirstName & "" = "" Then
        FirstName = m_FirstName
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        RcCode
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let RcCode(RC As String)
    On Error GoTo ErrorHandler:
    If Not RC & "" = "" Then
        m_RcCode = RC
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        RcCode
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get RcCode() As String
    On Error GoTo ErrorHandler:
    If Not m_RcCode & "" = "" Then
        RcCode = m_RcCode
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        PayGrade
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let PayGrade(pg As String)
    On Error GoTo ErrorHandler:
    If Not pg & "" = "" Then
        m_PayGrade = pg
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property




'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        PayGrade
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get PayGrade() As String
    On Error GoTo ErrorHandler:
    If Not m_PayGrade & "" = "" Then
        PayGrade = m_PayGrade
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property


'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        Step
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let Step(sp As String)
    On Error GoTo ErrorHandler:
    If Not sp & "" = "" Then
        m_Step = sp
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        Step
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get Step() As String
    On Error GoTo ErrorHandler:
    If Not m_Step & "" = "" Then
        Step = m_Step
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property


'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        JobTitle
'   Parameters:  String
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let JobTitle(jt As String)
    On Error GoTo ErrorHandler:
    If Not jt & "" = "" Then
        m_JobTitle = jt
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        JobTitle
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get JobTitle() As String
    On Error GoTo ErrorHandler:
    If Not m_JobTitle & "" = "" Then
        JobTitle = m_JobTitle
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property


'----------------------------------------------------------------------------------
'   Type:        Property-Let
'   Name:        HireDate
'   Parameters:  Date
'   RetVal:      Void
'   Purpose
'---------------------------------------------------------------------------------
Public Property Let HireDate(hd As Date)
    On Error GoTo ErrorHandler:
    If Not hd & "" = "" Then
        m_HireDate = hd
    End If
ErrorHandler:
    ProcessError
    Exit Property
End Property



'----------------------------------------------------------------------------------
'   Type:        Property-Get
'   Name:        HireDate
'   Parameters:  Void
'   RetVal:      String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get HireDate() As Date
    On Error GoTo ErrorHandler:
    If Not m_HireDate & "" = "" Then
        HireDate = m_HireDate
    End If
ErrorHandler:
    If Err.Number > 0 Then
        m_Error = "Source:       Employee" _
            & vbCrLf & "Member:      HireDate" _
            & vbCrLf & "Descript:   " & Err.Description
        Err.Clear
    End If
    MessageFactory.ShowError (m_Error)
    Exit Property
End Property






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



