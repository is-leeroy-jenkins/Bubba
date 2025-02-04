Option Compare Database
Option Explicit


Private m_ReferenceModels As String
Private m_ViewModels As String
Private m_DataModels As String
Private m_ExcelReport As String
Private m_DataFolder As String
Private m_DivisionModels As String
Private m_Path As String
Private m_Error As String




'----------------------------------------------------------------------------------
'   Type:        Constructor/Finalizer
'   Name:
'   Parameters:  Void
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
    m_ViewModels = "Provider=Microsoft.Access.OLEDB.10.0;" _
        & "Persist Security Info=False;" _
        & "Data Source=" _
        & CurrentProject.Path _
        & "\Views.accdb;" _
        & "User ID=Admin;" _
        & "Data Provider=Microsoft.ACE.OLEDB.12.0"
    m_DataModels = "Provider=Microsoft.Access.OLEDB.10.0;" _
        & "Persist Security Info=False;" _
        & "Data Source=" _
        & CurrentProject.Path _
        & "\Data.accdb;" _
        & "User ID=Admin;" _
        & "Data Provider=Microsoft.ACE.OLEDB.12.0"
    m_ReferenceModels = "Provider=Microsoft.Access.OLEDB.10.0;" _
        & "Persist Security Info=False;" _
        & "Data Source=" _
        & CurrentProject.Path _
        & "\References.accdb;" _
        & "User ID=Admin;" _
        & "Data Provider=Microsoft.ACE.OLEDB.12.0"
    m_ExcelReport = CurrentProject.Path & "\etc\templates\reports\Excel.xlsx"
End Sub




'---------------------------------------------------------------------------------
'   Type:            Property-Get
'   Name:            References
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get References() As String
    On Error GoTo ErrorHandler
    If Not m_ReferenceModels & "" = "" Then
        References = m_ReferenceModels
    End If
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="References"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type:            Property-Get
'   Name:            Data
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Data() As String
    On Error GoTo ErrorHandler
    If Not m_DataModels & "" = "" Then
        Data = m_DataModels
    End If
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="Data"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type:            Property-Get
'   Name:            Views
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Views() As String
    On Error GoTo ErrorHandler
    If Not m_ViewModels & "" = "" Then
        Views = m_ViewModels
    End If
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="Views"
    Exit Property
End Property






'---------------------------------------------------------------------------------
'   Type:            Property-Get
'   Name:            ReportTemplate
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get ReportTemplate() As String
    On Error GoTo ErrorHandler
    If Not m_ExcelReport & "" = "" Then
        ReportTemplate = m_ExcelReport
    End If
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="ReportTemplate"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type:            Property-Let
'   Name:            External
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let ExternalFile(Value As String)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" And Not Dir(Value) = "" Then
        m_Path = Value
    End If
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="ExternalFile"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type            Property-Get
'   Name            ExternalFile
'   Parameters      Void
'   Retval          String
'   Purpose
'---------------------------------------------------------------------------------
Public Property Get ExternalFile() As String
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        ExternalFile = m_Path
    End If
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="ExternalFile"
    Exit Property
End Property






'---------------------------------------------------------------------------------
'   Type            Property-Get
'   Name            ExternalFile
'   Parameters      Void
'   Retval          String
'   Purpose
'---------------------------------------------------------------------------------
Private Property Get DevelopmentPath() As String
    On Error GoTo ErrorHandler
    DevelopmentPath = "C:\Users\terry\source\repos\BudgetX\db\accdb\models"
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="DevelopmentPath"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type            Property-Get
'   Name            ExternalFile
'   Parameters      Void
'   Retval          String
'   Purpose
'---------------------------------------------------------------------------------
Private Property Get ProductionPath() As String
    On Error GoTo ErrorHandler
    ProductionPath = "C:\Users\teppler\source\repos\BudgetX\db\accdb\models"
ErrorHandler:
    ProcessError Name:="DataPath", _
        Member:="ProductionPath"
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






