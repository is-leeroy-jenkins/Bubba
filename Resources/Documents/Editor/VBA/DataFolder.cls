Option Compare Database
Option Explicit



Private m_InFile As String
Private m_Path As String
Private m_Name As String
Private m_Extension As String
Private m_ParentFolder As String
Private m_FileSys As Scripting.FileSystemObject
Private m_Folder As Scripting.Folder
Private m_File As Scripting.File
Private m_Files As Scripting.Files
Private m_SubFolders As Scripting.folders
Private m_Drive As String
Private m_Type As String
Private m_DateCreated As Date
Private m_LastAccessed As Date
Private m_LastModified As Date
Private m_Stream As Scripting.TextStream
Private m_Error As String
Private m_Notification As String






'----------------------------------------------------------------------------------
'   Type        Contructor
'   Name        Class_Initialize()
'   Parameters  Void
'   Retval      AccessData
'   Purpose
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
End Sub

'----------------------------------------------------------------------------------
'   Type        DeContructor/Finalizer
'   Name        Class_Terminate
'   Parameters  Void
'   Retval      Void
'   Purpose
'---------------------------------------------------------------------------------
Private Sub Class_Terminate()
End Sub










'---------------------------------------------------------------------------------
'   Type:            Property Let
'   Name:
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let InFile(Value As String)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        m_InFile = Value
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="InFile"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Property Let
'   Name:
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get InFile() As String
    On Error GoTo ErrorHandler
    If Not Dir(m_InFile) & "" = "" Then
        InFile = m_InFile
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="InFile"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get FileCount() As Integer
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        FileCount = m_FileSys.GetFolder(m_InFile).Files.Count
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="FileCount"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            Extension
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get FolderCount() As Integer
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        Set m_SubFolders = m_FileSys.GetFolder(m_InFile).SubFolders
        FolderCount = m_SubFolders.Count
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="FolderCount"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            Exists
'   Parameters:      String
'   Retval:          Boolean
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Exists() As Boolean
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        If Not Dir(m_InFile) & "" = "" Then
            Exists = True
        Else
            Exists = False
        End If
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Exists"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            Exists
'   Parameters:      String
'   Retval:          Boolean
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get IsRooted() As Boolean
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        If m_FileSys.GetFolder(m_InFile).IsRootFolder = True Then
            IsRooted = True
        Else
            IsRooted = False
        End If
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="IsRooted"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            CurrentDirectory
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get CurrentDirectory() As String
    On Error GoTo ErrorHandler
    Dim p_Current As String
    p_Current = CurDir()
    CurrentDirectory = p_Current
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="CurrentDirectory"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Path() As String
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" And _
        Not Dir(m_InFile) & "" = "" Then
            Set m_FileSys = New FileSystemObject
            m_Path = m_FileSys.GetFolder(m_InFile).ShortPath
            Path = m_Path
            Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Path"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Drive() As String
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_Drive = m_FileSys.GetFolder(m_InFile).Drive
        Drive = m_Drive
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Drive"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get ParentFolder() As String
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_ParentFolder = m_FileSys.GetParentFolderName(m_InFile)
        ParentFolder = m_ParentFolder
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="ParentFolder"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            DateCreated
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get DateCreated() As Date
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_DateCreated = m_FileSys.GetFolder(m_InFile).DateCreated
        DateCreated = m_DateCreated
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="DateCreated"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:            LastAccessed
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get LastAccessed() As Date
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_LastAccessed = m_FileSys.GetFolder(m_InFile).DateLastAccessed
        LastAccessed = m_LastAccessed
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="LastAccessed"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get LastModified() As Date
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_LastModified = m_FileSys.GetFolder(m_InFile).DateLastModified
        LastModified = m_LastModified
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="LastModified"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Name() As String
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_Name = m_FileSys.GetFolder(m_InFile).ShortName
        Name = m_Name
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Name"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Size() As Variant
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        Set Size = m_FileSys.GetFolder(m_InFile).Size
    End If
    Set m_FileSys = Nothing
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Size"
    Exit Property
End Property



 
'---------------------------------------------------------------------------------
'   Type:            Function Procedure
'   Name:            Extension
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get SubFolders() As Scripting.folders
    On Error GoTo ErrorHandler
    Dim folders As Scripting.folders
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        Set m_SubFolders = m_FileSys.GetFolder(m_InFile).SubFolders
        Set SubFolders = m_SubFolders
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="SubFolders"
    Exit Property
End Property


'---------------------------------------------------------------------------------
'   Type:            Function Procedure
'   Name:            FolderExists
'   Parameters:      String
'   Retval:          Boolean
'   Purpose:
'---------------------------------------------------------------------------------
Public Static Function Verify(Value As String) As Boolean
    On Error GoTo ErrorHandler
    If Not Value & "" = "" And _
        Not m_InFile & "" = "" Then
            Set m_FileSys = New FileSystemObject
            If m_FileSys.FolderExists(Value) = True Then
                Verify = True
            Else
                Verify = False
            End If
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Verify()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetSize
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetName(Value As String) As String
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" And _
        Not Dir(Value) & "" = "" Then
            Set m_FileSys = New FileSystemObject
            GetName = m_FileSys.CreateFolder(Value).Name
            Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="GetName()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetSize
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetSubFiles(Value As String) As Scripting.Files
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" And _
        Not Dir(Value) & "" = "" Then
            Set m_FileSys = New FileSystemObject
            Set GetSubFiles = m_FileSys.CreateFolder(Value).Files
            Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="GetSubFiles()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetSubFolders
'   Parameters:      String
'   Retval:          Scripting.Folders
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetSubFolders(Value As String) As Scripting.folders
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" And _
        Not Dir(Value) & "" = "" Then
            Set m_FileSys = New FileSystemObject
            Set GetSubFolders = m_FileSys.CreateFolder(Value).SubFolders
            Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="GetSubFolders()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetSize
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetSize(Value As String) As Variant
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" And _
        Not Dir(Value) & "" = "" Then
            Set m_FileSys = New FileSystemObject
            Set GetSize = m_FileSys.GetFolder(Value).Size
    Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="GetSize()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:
'   Parameters:      Void
'   Retval:          Scripting.Folder
'   Purpose:
'---------------------------------------------------------------------------------
Public Function Create() As Scripting.Folder
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Set m_FileSys = New FileSystemObject
        Set m_Folder = m_FileSys.CreateFolder(m_InFile)
        Set Create = m_Folder
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Create()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Static Function CreateDirectory(Value As String) As Scripting.Folder
    On Error GoTo ErrorHandler
    Dim p_Folder As Scripting.Folder
    If Not Value & "" = "" Then
        Set m_FileSys = New FileSystemObject
        Set p_Folder = m_FileSys.CreateFolder(Value)
        Set CreateDirectory = p_Folder
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="CreateDirectory()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            GetCurrentDirectory()
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Static Sub ChangeCurrentDirectory(Value As String)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        ChDir (m_InFile)
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="ChangeDirectory()"
    Exit Sub
End Sub



 
'---------------------------------------------------------------------------------
'   Type:            Function Procedure
'   Name:            Extension
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub AddSubFolder(ValueName As String)
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Dim p_Folder As New Scripting.FileSystemObject
        Set m_FileSys = New FileSystemObject
        Set m_SubFolders = m_FileSys.GetFolder(m_InFile).SubFolders
        Set p_Folder = m_SubFolders.add(ValueName)
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="AddSubFolder()"
    Exit Sub
End Sub


'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get SubFiles() As Collection
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Dim p_File As Scripting.File
        Dim p_FileCollection As Collection
        Set p_FileCollection = New Collection
        Set m_FileSys = New Scripting.FileSystemObject
        For Each p_File In m_FileSys.GetFolder(m_InFile).Files
            p_FileCollection.add p_File, p_File.Name
        Next p_File
        Set SubFiles = p_FileCollection
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="SubFiles()"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            ContainsFile
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function ContainsFile(pFile As String) As Boolean
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Dim p_Files As Scripting.Files
        Dim p_File As Scripting.File
        Set m_FileSys = New FileSystemObject
        Set p_Files = m_FileSys.GetFolder(m_InFile).Files
        If p_Files.Count > 0 Then
            For Each p_File In p_Files
                If p_File.Name = pFile Then
                    ContainsFile = True
                    Exit Function
                End If
            Next p_File
        End If
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="ContainsFile"
    Exit Function
End Function






'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            ContainsSubFolder
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function ContainsSubFolder(Value As String) As Boolean
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
        Dim p_Folders As Scripting.Folders
        Dim p_Folder As Scripting.Folder
        Set m_FileSys = New Scripting.FileSystemObject
        Set p_Folders = m_FileSys.GetFolder(Value)
        If p_Folders.Count > 0 Then
            For Each p_Folder In p_Folders
                If p_Folder.Name = Value Then
                    ContainsSubFolder = True
                    Exit Function
                End If
            Next p_Folder
        End If
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="ContainsSubFolder"
    Exit Function
End Function






'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            Move
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Move(Value As String)
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Move()"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            FileCopy
'   Parameters:      String, String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Copy(Value As String)
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" Then
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Copy()"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            Delete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Delete()
    On Error GoTo ErrorHandler
    If Not Dir(m_InFile) & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_FileSys.DeleteFolder (m_InFile)
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Delete()"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            CreateTextFile
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function CreateTextFile(Value As String) As Scripting.TextStream
    On Error GoTo ErrorHandler
    If Not Dir(Value) & "" = "" And _
        Not m_InFile & "" = "" Then
            Dim p_TextFile As Scripting.TextStream
            Set m_FileSys = New FileSystemObject
            Set p_TextFile = m_FileSys.CreateTextFile(FileName:=Value, Overwrite:=True)
            CreateTextFile = p_TextFile
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="CreateTextFile()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Browse()
    On Error GoTo ErrorHandler
    Dim p_Dialog As FileDialog
    Dim p_Result As Integer
    Set m_FileSys = New FileSystemObject
    Set p_Dialog = Application.FileDialog(msoFileDialogFilePicker)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select a file"
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    p_Dialog.Filters.add "Text files", "*.txt"
    p_Dialog.Filters.add "All files", "*.*"
    If p_Dialog.Show = -1 Then
       m_FileSys.OpenTextFile (p_Dialog.Item)
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Browse()"
    Exit Sub
End Sub

 


'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            FileRename
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Rename(Value As String)
    On Error GoTo ErrorHandler
    If Not m_InFile & "" = "" And _
        Not Value & "" = "" Then
         If Dir(m_InFile) <> "" Then
            Name m_InFile As Value
        End If
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Rename()"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            OpenFolderDialog()
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub OpenFolderDialog()
    On Error GoTo ErrorHandler
    Dim p_Dialog As FileDialog
    Dim p_Result As Integer
    Set p_Dialog = Application.FileDialog(msoFileDialogFolderPicker)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select a Folder"
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    If p_Dialog.Show = -1 Then
       Debug.Print p_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="OpenFolderDialog()"
    Exit Sub
End Sub





'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            Zip
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Zip(FolderPath As Variant, ZipPath As Variant)
    On Error GoTo ErrorHandler
    Dim p_Shell As Object
    Open ZipPath For Output As #1
    Print #1, Chr$(80) & Chr$(75) & Chr$(5) & Chr$(6) & String(18, 0)
    Close #1
    Set p_Shell = CreateObject("Shell.Application")
    p_Shell.Namespace(ZipPath).CopyHere p_Shell.Namespace(FolderPath).Items
    Do Until p_Shell.Namespace(ZipPath).Items.Count = p_Shell.Namespace(FolderPath).Items.Count
        p_Shell.Wait (Now + TimeValue("0:00:01"))
    Loop
ErrorHandler:
    ProcessError Name:="DataFolder", _
        Member:="Zip(FolderPath, ZipPath)"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:           AutoRefresh
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub AutoRefresh()
    Dim p_FileSys As Object
    Dim p_Folder As Object
    Dim p_File As Object
    m_Path = "C:\Users\teppler\OneDrive - Environmental Protection Agency (EPA)\EPPLER_one_drive\CBOR\" _ 
        "AppropriationDocuments\clean\"

    Set p_FileSys = CreateObject("Scripting.FileSystemObject")
    Set p_Folder = p_FileSys.GetFolder(m_Path)
    With Application
           .DisplayAlerts = False
           .ScreenUpdating = False
           .EnableEvents = False
           .AskToUpdateLinks = False
    End With
    For Each p_File In p_Folder.Files
       If Right(p_File.Name, 4) = "xlsx" Or Right(p_File.Name, 3) = "xls" Then
               Workbooks.Open m_Path & p_File.Name
               ActiveWorkbook.UpdateLink Name:=ActiveWorkbook.LinkSources
               ActiveWorkbook.Close True
       End If
    Next
    With Application
           .DisplayAlerts = True
           .ScreenUpdating = True
           .EnableEvents = True
           .AskToUpdateLinks = True
    End With
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
                & vbCrLf & "Issue:      " & Err.Valuecription _
                & vbCrLf & "Class:      " & Name _
                & vbCrLf & "Member:     " & Member
    End If
    If Err.Number <> 0 And _
        IsMissing(Name) And _
        IsMissing(Member) Then
            m_Error = "Source:      " & Err.Source _
                & vbCrLf & "Number:     " & Err.Number _
                & vbCrLf & "Issue:      " & Err.Valuecription
    End If
    MessageFactory.ShowError (m_Error)
    Err.Clear
End Sub





