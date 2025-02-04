Option Compare Database
Option Explicit


Private m_BudgetExecution As DAO.Workspace
Private m_Database As DAO.Database
Private m_DataSet As DAO.Recordset
Private m_Error As String
Private m_Notification As String
Private m_SQL As String







'----------------------------------------------------------------------------------
'   Type        Contructor/Finalizer
'   Name        Class_Initialize()/Class_Terminate
'   Parameters  Void
'   Retval      AccessData
'   Purpose
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
    Set m_Database = CurrentDb
    Set m_BudgetExecution = DBEngine.Workspaces(0)
End Sub

Private Sub Class_Terminate()
    If Not IsNull(m_Database) Then
        Set m_Database = Nothing
    End If
    If Not IsNull(m_BudgetExecution) Then
        Set m_BudgetExecution = Nothing
    End If
End Sub





'----------------------------------------------------------------------------------
'   Type:        Property Get
'   Name:        IsConnected
'   Parameters:  Void
'   RetVal:      Boolean
'   Purpose:     Function returns boolean value indicating if Workspace is open
'---------------------------------------------------------------------------------
Public Property Get IsConnected() As Boolean
    On Error GoTo ErrorHandler
    If Not IsObject(m_Database) Then
        IsConnected = True
    End If
    If IsObject(m_Database) Then
        IsConnected = False
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="IsConnected()"
    Exit Property
End Property





'----------------------------------------------------------------------------------
'   Type        Property Get
'   Name        IsOpen
'   Parameters  Void
'   RetVal      Boolean
'   Purpose     Function returns boolean value indicating if Workspace is open
'---------------------------------------------------------------------------------
Public Property Get IsOpen() As Boolean
    On Error GoTo ErrorHandler
    If Not IsObject(m_BudgetExecution) Then
        IsOpen = True
    End If
    If IsObject(m_BudgetExecution) Then
        IsOpen = False
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="IsOpen()"
    Exit Property
End Property





'----------------------------------------------------------------------------------
'   Type        Function
'   Name        GetDataSet
'   Parameters  String - table;
'   RetVal      DAO.Recordset
'   Purpose     Function returns the records from the Data Table using the table's
'               name or a sql select query
'---------------------------------------------------------------------------------
Public Function GetDataSet(Value As String) As DAO.Recordset
    On Error GoTo ErrorHandler
    Set m_Database = CurrentDb
    If Not Value & "" = "" Then
        Set m_DataSet = m_Database.OpenRecordset(Value, dbOpenTable)
        Set GetDataSet = m_DataSet
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="GetDataSet()"
    Exit Function
End Function





'----------------------------------------------------------------------------------
'   Type        Function
'   Name        GetRecordByIndex(String, Integer)
'   Parameters  String - Value;
'   RetVal      DAO.Recordset
'   Purpose     Function returns record using Index parameter and Primary Key
'               defined in a sql select query
'---------------------------------------------------------------------------------
Public Function GetRecordByIndex(Value As String, Index As Integer) As DAO.Recordset
    On Error GoTo ErrorHandler
    If Not Value & "" = "" And _
        Not Index < 0 Then
            Set m_Database = CurrentDb
            m_SQL = "SELECT * FROM " & Value & ";"
            Set m_DataSet = m_Database.OpenRecordset(m_SQL, dbOpenDynaset)
            Dim p_Column As New DAO.Field
            Dim p_Filter As String
            Set p_Column = m_DataSet.Fields(0)
            p_Filter = p_Column.Name & " = '" & Index & "'" & ""
            Dim p_Record As DAO.Recordset
            m_DataSet.Filter = p_Filter
            Set GetRecordByIndex = m_DataSet
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="GetDataRecord()"
    Exit Function
End Function




'----------------------------------------------------------------------------------
'   Type        Function
'   Name        GetDatabase()
'   Parameters  Void
'   RetVal      Void
'   Purpose     Method used to close the database
'---------------------------------------------------------------------------------
Public Function CreateDatabase(Value As String) As DAO.Database
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        Set m_Database = DBEngine.CreateDatabase(Value, dbLangGeneral)
        Set CreateDatabase = m_Database
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="GetDatabase()"
    Exit Function
End Function




'----------------------------------------------------------------------------------
'   Type        Function
'   Name        CreateTable()
'   Parameters  Void
'   RetVal      Void
'   Purpose     Method used to close the database
'---------------------------------------------------------------------------------
Public Function CreateTable(Value As String, Columns() As String) As DAO.TableDef
    On Error GoTo ErrorHandler
    If Not m_Database Is Nothing And _
        IsArray(Columns) Then
            Dim p_Table As DAO.TableDef
            Set p_Table = New DAO.TableDef
            Set p_Table = m_Database.CreateTableDef(Value)
            Dim i As Integer
            For i = 0 To i = UBound(Columns) - 1
                Dim p_Field As New DAO.Field
                With p_Field
                    .Name = Columns(i)
                    .OrdinalPosition = i
                End With
                p_Table.Fields.Append (p_Field)
            Next i
            m_Database.TableDefs.Append (p_Table)
            Set CreateTable = m_Database.TableDefs(p_Table)
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="CreateTable()"
    Exit Function
End Function







'----------------------------------------------------------------------------------
'   Type        Function
'   Name        GetDatabase()
'   Parameters  Void
'   RetVal      Void
'   Purpose     Method used to close the database
'---------------------------------------------------------------------------------
Public Function GetRecords(Value As String) As DAO.Recordset
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        Set m_DataSet = CurrentDb.OpenRecordset(Value)
        Set GetRecords = m_DataSet
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="GetDataSet()"
    Exit Function
End Function




'----------------------------------------------------------------------------------
'   Type        Subprocedure
'   Name        UpdateDate
'   Parameters  Table - String, pField - String, value - Variant
'   RetVal      Void
'   Purpose     Method used to update data in current recordset
'---------------------------------------------------------------------------------
Public Sub UpdateRecordset(Table As String, Index As Integer, Column As String, Value As Variant)
    On Error GoTo ErrorHandler
    If Not Table & "" = "" And _
        Not Column & "" = "" And _
        Not Value Is Nothing Then
            Set m_DataSet = CurrentDb.OpenRecordset(Name)
            m_DataSet.Edit
            m_DataSet.Fields(Column).Value = Value
            m_DataSet.Update
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="UpdateRecordset()"
    Exit Sub
End Sub




'----------------------------------------------------------------------------------
'   Type        Subprocedure
'   Name        AddRecord
'   Parameters  Value - String, pField- String, value - Variant
'   RetVal      Void
'   Purpose     Method used to add data to recordset
'---------------------------------------------------------------------------------
Public Sub AddRecord(Table As String, Column As String, Value As Variant)
    On Error GoTo ErrorHandler
    If Not Table & "" = "" Then
        Set m_DataSet = CurrentDb.OpenRecordset(Table)
        m_DataSet.AddNew
        m_DataSet.Fields(Column).Value = Value
        m_DataSet.Update
        Set m_DataSet = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="AddRecord()"
    Exit Sub
End Sub




'----------------------------------------------------------------------------------
'   Type        Subprocedure
'   Name        UpdateRecord
'   Parameters  Value - String, pField - String, value - Variant
'   RetVal      Void
'   Purpose     Method used to add data to recordset
'---------------------------------------------------------------------------------
Public Sub UpdateRecord(Value As String, pField As String, pNewValue As Variant)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" And _
        Not pField & "" = "" And _
        Not pNewValue Is Nothing Then
            Set m_DataSet = CurrentDb.OpenRecordset(Value)
            With m_DataSet
                .Edit
                .Fields(pField) = pNewValue
                .Update
            End With
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="UpdateRecord()"
    Exit Sub
End Sub





'----------------------------------------------------------------------------------
'   Type        Subprocedure
'   Name        DeleteRecord
'   Parameters  Value - String, pKey - String, id - integer
'   RetVal      Void
'   Purpose     Method used to delete records from the data set
'---------------------------------------------------------------------------------
Public Sub DeleteRecord(pTableName As String, pKey As String, pID As Integer)
    On Error GoTo ErrorHandler
    If Not pTableName & "" = "" And _
        Not pKey & "" = "" And _
        Not pID < 0 Then
            Set m_Database = CurrentDb
            m_SQL = "DELETE FROM " _
                & pTableName & " WHERE " & pKey & "' = '" & pID & "'" & ""
            Set m_DataSet = CurrentDb.OpenRecordset(pTableName)
    End If
ErrorHandler:
    ProcessError Name:="DataAccess", _
        Member:="DeleteRecord()"
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





