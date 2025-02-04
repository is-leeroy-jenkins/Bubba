Option Compare Database
Option Explicit


Private m_DataPath As DataPath
Private m_Name As String
Private m_Extension As String
Private m_FileSys As Scripting.FileSystemObject
Private m_Excel As Excel.Application
Private m_Workbook As Excel.Workbook
Private m_Worksheet As Excel.Worksheet
Private m_Chart As Excel.Chart
Private m_PivotTable As Excel.PivotTable
Private m_PivotFields As Excel.PivotFields
Private m_PivotItems As Excel.PivotItems
Private m_Error As String
Private m_Notification As String
Private m_Data As DAO.Database
Private m_DataSet As DAO.Recordset
Private m_Query As DAO.QueryDef
Private m_Connection As ADODB.Connection
Private pAnd As String





'---------------------------------------------------------------------------------
'   Type:            Constructor
'   Name:
'   Parameters:      Void
'   Retval:          BudgetFile
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
    Set m_DataPath = New DataPath
    Set m_Excel = New Excel.Application
    Set m_Workbook = New Excel.Workbook
    pAnd = " AND "
End Sub

Private Sub Class_Terminate()
    If Not m_Excel Is Nothing Then
        Set m_Excel = Nothing
    End If
    If Not m_DataPath Is Nothing Then
        Set m_DataPath = Nothing
    End If
    If Not m_Workbook Is Nothing Then
        Set m_Workbook = Nothing
    End If
End Sub




'---------------------------------------------------------------------------------
'   Type:            Property Let
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Path(Value As String)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        m_DataPath = Value
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="Path"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Property Get
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Path() As String
    On Error GoTo ErrorHandler
    If m_DataPath & "" = "" Then
        Path = m_DataPath
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="Path"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            WorkSheetExists
'   Parameters:      Void
'   Retval:          BudgetFile
'   Purpose:
'---------------------------------------------------------------------------------
Public Function WorksheetExists(SheetName As String, Workbook As Excel.Workbook) As Boolean
    On Error GoTo ErrorHandler
    Dim p_CheckSheet As Worksheet
    Set p_CheckSheet = Workbook.Worksheets(SheetName)
    WorksheetExists = True
    Exit Function
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="WorksheetExists"
    WorksheetExists = False
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Destructor
'   Name:
'   Parameters:      Void
'   Retval:          BudgetFile
'   Purpose:
'---------------------------------------------------------------------------------
Public Function WorkbookExists(ExcelPath As String) As Boolean
    On Error GoTo ErrorHandler
    Set m_FileSys = New Scripting.FileSystemObject
    Dim p_Extension As String
    If Not Dir(ExcelPath) & "" = "" Then
        p_Extension = m_FileSys.GetExtensionName(ExcelPath)
        If InStr(p_Extension, "XLSX") Then
            WorkbookExists = True
        Else
            WorkbookExists = False
        End If
    Else
        WorkbookExists = False
    End If
    If Not m_FileSys Is Nothing Then
        Set m_FileSys = Nothing
    End If
    Exit Function
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="WorkbookExists"
    WorkbookExists = False
    If Not m_FileSys Is Nothing Then
        Set m_FileSys = Nothing
    End If
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            CreateWorkbook
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub CreateWorkbook(FilePath As String)
    On Error GoTo ErrorHandler
    Set m_Excel = CreateObject("Excel.Application")
    Set m_Workbook = m_Excel.Workbooks.add
    Set m_Worksheet = m_Workbook.Worksheets(1)
    m_Workbook.SaveAs FileName:=FilePath, _
        FileFormat:=xlOpenXMLWorkbook
    m_Workbook.Close SaveChanges:=False
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="CreateWorkbook"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetReportTemplate() As Excel.Workbook
    On Error GoTo ErrorHandler
    Dim m_DataPath As DataPath
    Dim m_Template As String
    Dim m_Report As String
    Set m_DataPath = New DataPath
    m_Template = m_DataPath.ReportTemplate
    Set m_Workbook = m_Excel.Workbooks.Open(m_Template)
    Set GetReportTemplate = m_Workbook
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="GetReportTemplate"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Private Function GetBudgetTemplate() As Excel.Workbook
    On Error GoTo ErrorHandler
    Dim m_DataPath As DataPath
    Dim m_Template As String
    Dim m_Report As String
    Set m_DataPath = New DataPath
    m_Template = m_DataPath.ReportTemplate
    Set m_Workbook = m_Excel.Workbooks.Open(m_Template)
    m_Excel.Workbooks.add (m_Workbook)
    Set GetReportTemplate = m_Workbook
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="GetBudgetTemplate"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenFileDialog()
    On Error GoTo ErrorHandler
    Dim m_Dialog As FileDialog
    Dim p_Result As Integer
    Set m_Dialog = Application.FileDialog(msoFileDialogFilePicker)
    m_Dialog.AllowMultiSelect = False
    m_Dialog.Title = "Select an Excel file"
    m_Dialog.InitialFileName = "C:\"
    m_Dialog.Filters.Clear
    m_Dialog.Filters.add "Excel files", "*.xlsx"
    m_Dialog.Filters.add "All files", "*.*"
    If m_Dialog.Show = -1 Then
       Debug.Print m_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="OpenFileDialog"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenExcelFile(FileName As String, FilePath As String)
    On Error GoTo ErrorHandler
    Dim view As ADODB.Connection
    Dim m_DataSet As ADODB.Recordset
    Set view = New ADODB.Connection
    view.Open "Provider=Microsoft.ACE.OLEDB.12.0;" _
        & "Data Source=" & FileName & _
        "Extended Properties=""Excel 12.0; HDR=YES"";"
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="OpenExcelFile"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            CreateCsvFile
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub CreateCsvFile(FileName As String, FilePath As String)
    On Error GoTo ErrorHandler
    If Not FileName & "" = "" And _
        Not FilePath & "" = "" Then
            Dim p_FilePath As String
            Dim p_CsvName As String
            Dim p_Sheet As Worksheet
            Set p_Sheet = ActiveWorkbook.Sheets("Sheet1")
            p_CsvName = FileName & ".csv"
            p_FilePath = Application.CurrentProject
            p_Sheet.Copy
            ActiveWorkbook.SaveAs FileName:=p_FilePath & "\" & p_CsvName, _
                FileFormat:=xlCSV, CreateBackup:=False
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="CreateCsvFile"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------------
' Type:         Function
' Name:         ExportQuery(String)
' Parameters:   Sting
' Retval:       Excel.Application
' Purpose:      Export recordset to Excel
'---------------------------------------------------------------------------------------
Function ExportQuery(ByVal Query As String)
    On Error GoTo ErrorHandler
    Set m_Data = Application.CurrentDb
    Set m_DataSet = m_Data.OpenRecordset(Query)
    Dim p_IsOpen As Boolean
    Dim p_Columns As Integer
    Const xlCenter = -4108
    Set m_Excel = CreateObject("Excel.Application")
    p_IsOpen = True
    m_Excel.ScreenUpdating = False
    m_Excel.Visible = False   '
    Set m_Workbook = m_Excel.Workbooks.add()
    Set m_Worksheet = m_Workbook.Sheets(1)
    With m_DataSet
        If .RecordCount <> 0 Then
            For p_Columns = 0 To m_DataSet.Fields.Count - 1
                m_Worksheet.Cells(1, p_Columns + 1).Value = m_DataSet.Fields(p_Columns).Name
            Next
            With m_Worksheet.Range(m_Worksheet.Cells(1, 1), _
                m_Worksheet.Cells(1, m_DataSet.Fields.Count))
                .Font.Bold = True
                .Font.ColorIndex = 2
                .Interior.ColorIndex = 1
                .HorizontalAlignment = xlCenter
            End With
            m_Worksheet.Range("A2").CopyFromRecordset m_DataSet
            m_Worksheet.Range(m_Worksheet.Cells(1, 1), _
                m_Worksheet.Cells(1, m_DataSet.Fields.Count)).Columns.AutoFit
            m_Worksheet.Range("A1").Select
        End If
    End With
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="ExportQuery"
    Exit Function
End Function




'---------------------------------------------------------------------------------------
' Type:         Function
' Name:         ExportData(DAO.Recordset)
' Parameters:   Sting
' Retval:       Excel.Application
' Purpose:      Export recordset to Excel
'---------------------------------------------------------------------------------------
Public Function ImportRecordset(Data As DAO.Recordset)
    On Error GoTo ErrorHandler
    If Not Data Is Nothing Then
        Set m_DataSet = Data
        Dim p_IsOpen As Boolean
        Dim p_Columns As Integer
        Const xlCenter = -4108
        Set m_Excel = CreateObject("Excel.Application")
        p_IsOpen = True
        m_Excel.ScreenUpdating = False
        m_Excel.Visible = False   '
        Set m_Workbook = m_Excel.Workbooks(1)
        Set m_Worksheet = m_Workbook.Sheets(1)
        With Data
            If .RecordCount <> 0 Then
                For p_Columns = 0 To Data.Fields.Count - 1
                    m_Worksheet.Cells(1, p_Columns + 1).Value = Data.Fields(p_Columns).Name
                Next
                With m_Worksheet.Range(m_Worksheet.Cells(1, 1), _
                    m_Worksheet.Cells(1, Data.Fields.Count))
                    .Font.Bold = True
                    .Font.ColorIndex = 2
                    .Interior.ColorIndex = 1
                    .HorizontalAlignment = xlCenter
                End With
                m_Worksheet.Range("A2").CopyFromRecordset Data
                m_Worksheet.Range(m_Worksheet.Cells(1, 1), _
                    m_Worksheet.Cells(1, Data.Fields.Count)).Columns.AutoFit
                m_Worksheet.Range("A1").Select
            End If
        End With
        KillFiles
        KillData
        KillExcel
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="ImportRecordset"
    KillFiles
    KillData
    KillExcel
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenSaveDialog()
    On Error GoTo ErrorHandler
    Dim m_Dialog As FileDialog
    Dim p_Result As Integer
    Set m_Dialog = Application.FileDialog(msoFileDialogSaveAs)
    m_Dialog.AllowMultiSelect = False
    m_Dialog.Title = "Select location to save file."
    m_Dialog.InitialFileName = "C:\"
    m_Dialog.Filters.Clear
    If m_Dialog.Show = -1 Then
       Debug.Print m_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="OpenSaveDialog"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:         KillExcel()
' Parameters:   Void
' Retval:       Void
' Purpose:      Releases resources used by the Excel Application
'---------------------------------------------------------------------------------------
Private Sub KillExcel()
    On Error GoTo ErrorHandler
    If Not m_Excel Is Nothing Then
        Set m_Excel = Nothing
    End If
    If Not m_Workbook Is Nothing Then
        Set m_Workbook = Nothing
    End If
    If Not m_Workbook Is Nothing Then
        Set m_Worksheet = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="KillExcel"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:         KillExcel()
' Parameters:   Void
' Retval:       Void
' Purpose:      Releases resources used by the Excel Application
'---------------------------------------------------------------------------------------
Private Sub KillChart()
    On Error GoTo ErrorHandler
    If Not m_Chart Is Nothing Then
        Set m_Chart = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="KillChart"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:         KillExcel()
' Parameters:   Void
' Retval:       Void
' Purpose:      Releases resources used by the Pivot Table
'---------------------------------------------------------------------------------------
Private Sub KillPivot()
    On Error GoTo ErrorHandler
    If Not m_PivotTable Is Nothing Then
        Set m_PivotTable = Nothing
    End If
    If Not m_PivotFields Is Nothing Then
        Set m_PivotFields = Nothing
    End If
    If Not m_PivotItems Is Nothing Then
        Set m_PivotItems = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="KillPivot"
    Exit Sub
End Sub






'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:         KillData()
' Parameters:   Void
' Retval:       Void
' Purpose:      Releases resources used by the Database
'---------------------------------------------------------------------------------------
Private Sub KillData()
    On Error GoTo ErrorHandler
    If Not m_Data Is Nothing Then
        Set m_Data = Nothing
    End If
    If Not m_DataSet Is Nothing Then
        Set m_DataSet = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="KillData"
    Exit Sub
End Sub







'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:         KillFiles()
' Parameters:   Void
' Retval:       Void
' Purpose:      Releases resources used by the DataPath class
'---------------------------------------------------------------------------------------
Private Sub KillFiles()
    On Error GoTo ErrorHandler
    If Not m_DataPath Is Nothing Then
        Set m_DataPath = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelWorkbook", _
        Member:="KillFiles"
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








