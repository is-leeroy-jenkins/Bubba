Option Compare Database
Option Explicit



Private m_DataPath As DataPath
Private m_Template As String
Private m_SavePath As String
Private m_Name As String
Private m_Extension As String
Private m_Location As String
Private m_FileSys As Scripting.FileSystemObject
Private m_Excel As Excel.Application
Private m_Workbook As Excel.Workbook
Private m_Worksheet As Excel.Worksheet
Private m_Chart As Excel.Chart
Private m_PivotTable As Excel.PivotTable
Private m_PivotFields As Excel.PivotFields
Private m_PivotItems As Excel.PivotItems
Private m_PivotCache As Excel.PivotCache
Private m_Slicer As Excel.Slicer
Private m_SlicerCache As Excel.SlicerCache
Private m_SlicerItem As Excel.SlicerItem
Private m_SlicerItems As Excel.SlicerItems
Private m_Error As String
Private m_Notification As String
Private m_Data As DAO.Database
Private m_DataSet As DAO.Recordset






'---------------------------------------------------------------------------------
'   Type:            Constructor/Finalizer
'   Name:            ExcelReport
'   Parameters:      Void
'   Retval:          BudgetFile
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
    Set m_Data = CurrentDb
    Set m_DataPath = New DataPath
End Sub

Private Sub Class_Terminate()
    KillExcel
    KillData
    KillFiles
End Sub





'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            CreateWorkbook
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub CreateWorkbook(Name As String)
    On Error GoTo ErrorHandler:
    If Not Name & "" = "" Then
            Set m_Excel = New Excel.Application
            Set m_Workbook = m_Excel.Workbooks.add(Name)
            Set m_Worksheet = m_Workbook.Worksheets.add
            m_Workbook.SaveAs FileName:=Name, _
                FileFormat:=xlOpenXMLWorkbook
            m_Workbook.Close SaveChanges:=True
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
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
Public Function GetTemplate() As Excel.Workbook
    On Error GoTo ErrorHandler:
    m_Template = m_DataPath.ReportTemplate
    Set m_Excel = New Excel.Application
    Set m_Workbook = m_Excel.Workbooks.Open(m_Template)
    Set GetTemplate = m_Workbook
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="GetTemplate"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetRange(Column As Integer, Row As Integer) As Excel.Range
    On Error GoTo ErrorHandler:
    If Column > 0 And Row > 0 Then
        Dim m_Template As String
        m_Template = m_DataPath.ReportTemplate
        Set m_Excel = New Excel.Application
        Set m_Workbook = m_Excel.Workbooks.Open(m_Template)
        Set m_Worksheet = m_Workbook.Worksheets.add
        Dim m_Range As Excel.Range
        Set m_Range = m_Workbook.Worksheets(1).Cells(Column, Row)
        Set GetRange = m_Range
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="GetRange"
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub PopulateData(Value As DAO.Recordset)
    On Error GoTo ErrorHandler:
    Dim m_Template As String
    m_Template = m_DataPath.ReportTemplate
    Dim m_Range As Excel.Range
    Set m_Workbook = m_Excel.Workbooks.add
    Set m_Workbook = m_Excel.Workbooks.Open(m_Template)
    Set m_Range = m_Workbook.Worksheets(1).Cells(1, 2)
    m_Template = m_DataPath.ReportTemplate
    m_Workbook.Worksheets(2).Visible = False
    m_Range.CopyFromRecordset Data:=Value, MaxRows:=59, MaxColumns:=10
    m_Excel.Visible = True
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="PopulateData"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenFileDialog()
    On Error GoTo ErrorHandler:
    Dim p_Dialog As FileDialog
    Dim p_Result As Integer
    Set p_Dialog = Application.FileDialog(msoFileDialogFilePicker)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select an Excel file"
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    p_Dialog.Filters.add "Excel files", "*.xlsx"
    p_Dialog.Filters.add "All files", "*.*"
    If p_Dialog.Show = -1 Then
       Debug.Print p_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="OpenFileDialog"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenFolderDialog()
    On Error GoTo ErrorHandler:
    Dim p_Dialog As FileDialog
    Dim p_Result As Integer
    Set p_Dialog = Application.FileDialog(msoFileDialogFolderPicker)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select an Folder"
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    If p_Dialog.Show = -1 Then
       Debug.Print p_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="OpenFolderDialog"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenSaveDialog()
    On Error GoTo ErrorHandler:
    Dim p_Dialog As FileDialog
    Dim p_Result As Integer
    Set p_Dialog = Application.FileDialog(msoFileDialogSaveAs)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select location to save file."
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    If p_Dialog.Show = -1 Then
       Debug.Print p_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="OpenSaveDialog"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenExcelFile(Value As String)
    On Error GoTo ErrorHandler:
    If Not m_Excel Is Nothing Then
        Set m_Excel = CreateObject("Excel.Application")
        Set m_Workbook = m_Excel.Workbooks.Open(Value)
        m_Excel.Visible = True
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
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
Public Sub CreateCsvFile(FolderPath As String, FileName As String)
    On Error GoTo ErrorHandler:
    If Not FileName & "" = "" And _
        Not Dir(FolderPath) & "" = "" Then
            Set m_Excel = New Excel.Application
            Set m_Workbook = m_Excel.Workbooks.Open(FileName)
            Set m_Worksheet = m_Workbook.Worksheets.add
            m_Worksheet.Copy
            m_Workbook.SaveAs FileName:=FolderPath & "\" & FileName, _
                FileFormat:=xlCSV, CreateBackup:=False
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="CreateCsvFile"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------------
' Type:         Function
' Name:         ExcelExport(String)
' Parameters:   Sting
' Retval:       Excel.Application
' Purpose:      Export recordset to Excel
'---------------------------------------------------------------------------------------
Function ExportQuery(ByVal pQuery As String)
    On Error GoTo ErrorHandler:
    Set m_DataSet = m_Data.OpenRecordset(pQuery)
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
    ProcessError Name:="ExcelReport", _
        Member:="ExportQuery"
    Exit Function
End Function



'---------------------------------------------------------------------------------------
' Type:         Function
' Name:         ExcelExport(String)
' Parameters:   Sting
' Retval:       Excel.Application
' Purpose:      Export recordset to Excel
'---------------------------------------------------------------------------------------
Public Function ExportData(Value As DAO.Recordset)
    On Error GoTo ErrorHandler:
    If Not Value Is Nothing Then
        Set m_DataSet = Value
        Dim p_IsOpen As Boolean
        Dim p_Columns As Integer
        Const xlCenter = -4108
        Set m_Excel = CreateObject("Excel.Application")
        Set m_Workbook = m_Excel.Workbooks.add()
        Set m_Worksheet = m_Workbook.Sheets(1)
        p_IsOpen = True
        m_Excel.ScreenUpdating = False
        m_Excel.Visible = False   '
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
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="ExportData"
    Exit Function
End Function






'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:
' Parameters:   Sting
' Retval:       Void
' Purpose:
'---------------------------------------------------------------------------------------
Public Sub SetLeftHeader(Value As String)
    On Error GoTo ErrorHandler:
    Dim p_LeftHeader As String
    p_LeftHeader = Value
    ActiveSheet.PageSetup.LeftHeader = p_LeftHeader
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="SetLeftHeader"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:
' Parameters:   Sting
' Retval:       Void
' Purpose:
'---------------------------------------------------------------------------------------
Public Sub SetCenterHeader(Value As String)
    On Error GoTo ErrorHandler:
    Dim p_CenterHeader As String
    p_CenterHeader = Value
    ActiveSheet.PageSetup.CenterHeader = p_CenterHeader
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="SetCenterHeader"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:
' Parameters:   Sting
' Retval:       Void
' Purpose:
'---------------------------------------------------------------------------------------
Public Sub SetRightHeader(Value As String)
    On Error GoTo ErrorHandler:
    Dim p_RightHeader As String
    p_RightHeader = Value
    ActiveSheet.PageSetup.RightFooter = p_RightHeader
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="SetRightHeader"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:
' Parameters:   Sting
' Retval:       Void
' Purpose:
'---------------------------------------------------------------------------------------
Public Sub SetLeftFooter(Value As String)
    On Error GoTo ErrorHandler:
    If Not Value & "" = "" Then
        Dim p_LeftFooter As String
        p_LeftFooter = Value
        ActiveSheet.PageSetup.LeftFooter = p_LeftFooter
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="SetLeftFooter"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------------
' Type:         Sub Procedure
' Name:
' Parameters:   Sting
' Retval:       Void
' Purpose:
'---------------------------------------------------------------------------------------
Public Sub SetCenterFooter(Value As String)
    On Error GoTo ErrorHandler:
    Dim p_CenterFooter As String
    p_CenterFooter = Value
    ActiveSheet.PageSetup.CenterFooter = p_CenterFooter
ErrorHandler:
    ProcessError Name:="ExcelReport", _
        Member:="SetCenterFooter"
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
    On Error GoTo ErrorHandler:
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
    ProcessError Name:="ExcelReport", _
        Member:="KillExcel"
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
    On Error GoTo ErrorHandler:
    If Not m_Data Is Nothing Then
        Set m_Data = Nothing
    End If
    If Not m_DataSet Is Nothing Then
        Set m_DataSet = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
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
    On Error GoTo ErrorHandler:
    If Not m_DataPath Is Nothing Then
        Set m_DataPath = Nothing
    End If
ErrorHandler:
    ProcessError Name:="ExcelReport", _
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





