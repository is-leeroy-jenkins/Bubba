Option Compare Database
Option Explicit


Private m_Path As String
Private m_Name As String
Private m_Extension As String
Private m_ParentFolder As String
Private m_FileSys As Scripting.FileSystemObject
Private m_File As Scripting.File
Private m_CurrentDirectory As String
Private m_Drive As String
Private m_Type As String
Private m_DateCreated As Date
Private m_LastAccessed As Date
Private m_LastModified As Date
Private m_Stream As Scripting.TextStream
Private m_Error As String
Private m_Notification As String





'----------------------------------------------------------------------------------
'   Type        Contructor/Finalizer
'   Name        Class_Initialize()/Class_Terminate
'   Parameters  Void
'   Retval      AccessData
'   Purpose
'---------------------------------------------------------------------------------
Private Sub Class_Initialize()
End Sub


Private Sub Class_Terminate()
    If Not m_FileSys Is Nothing Then
        Set m_FileSys = Nothing
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
        m_Path = Value
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
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
    If m_Path & "" = "" Then
        Path = m_Path
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Path"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type:            Property Let
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Name(Value As String)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        m_Name = Value
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Name"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Property Get
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Name() As String
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_Name = m_FileSys.GetBaseName(m_Path)
        Name = m_Name
        m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Name"
    Exit Property
End Property






'---------------------------------------------------------------------------------
'   Type:            Property Let
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Let Extension(Value As String)
    On Error GoTo ErrorHandler
    If Not Value & "" = "" Then
        m_Extension = Value
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Extension"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Property Get
'   Name:            Path
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get Extension() As String
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New FileSystemObject
        m_Extension = m_FileSys.GetExtensionName(m_Path)
        Extension = m_Extension
        m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Extension"
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
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_ParentFolder = m_FileSys.GetFile(m_Path).ParentFolder
        ParentFolder = m_ParentFolder
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="ParentFolder"
    Exit Property
End Property




'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get DateCreated() As Date
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_DateCreated = m_FileSys.GetFile(m_Path).DateCreated
        DateCreated = m_DateCreated
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="DateCreated"
    Exit Property
End Property


'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get LastAccessed() As Date
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_LastAccessed = m_FileSys.GetFile(m_Path).DateLastAccessed
        LastAccessed = m_LastAccessed
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
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
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_LastModified = m_FileSys.GetFile(m_Path).DateLastModified
        LastModified = m_LastModified
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="LastModified()"
    Exit Property
End Property





'---------------------------------------------------------------------------------
'   Type:            Read-Only Property
'   Name:
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Property Get FileType() As String
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_Type = m_FileSys.GetFile(m_Path).Type
        FileType = m_Type
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="FileType"
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
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Size = m_FileSys.GetFile(m_Path).Size
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Size"
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
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        m_Drive = m_FileSys.GetFile(m_Path).Drive
        Drive = m_Drive
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Drive"
    Exit Property
End Property



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetPath
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetPath(Value As String) As String
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
            Set p_Other = New Scripting.FileSystemObject
                GetPath = m_FileSys.GetAbsolutePathName(Value)
                Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetPath()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetName
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetName(Value As String) As String
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
            Set p_Other = New Scripting.FileSystemObject
            GetName = p_Other.GetFileName(Value)
            Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetName()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetSize
'   Parameters:      String
'   Retval:          Integer
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetSize(Value As String) As Integer
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
        Set p_Other = New Scripting.FileSystemObject
        GetSize = CInt(p_Other.GetFile(Value).Size)
        Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetSize()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetExtension
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetExtension(Value As String) As String
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
            Set p_Other = New Scripting.FileSystemObject
            GetExtension = p_Other.GetExtensionName(Value)
            Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetExtension()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetCreationDate
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetCreationDate(Value As String) As Date
    On Error GoTo ErrorHandler
    If Not Dir(m_Path) & "" = "" And _
        Not Dir(Value) & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Set m_FileSys = CreateObject("Scripting.FileSystem")
        GetCreationDate = m_FileSys.GetFile(Value).DateCreated
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetCreationDate()"
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetCreationDate
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetParentFolder(Value As String) As Scripting.Folder
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Dir(Value) & "" = "" Then
        Set p_Other = New Scripting.FileSystemObject
        Set GetParentFolder = p_Other.GetFile(Value).ParentFolder
        Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetParentFolder()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetCreationDate
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetAccessDate(Value As String) As Date
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
        Set p_Other = New Scripting.FileSystemObject
        GetAccessDate = p_Other.GetFile(Value).DateLastAccessed
        Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetAccessDate()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetModifiedDate
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetModifiedDate(Value As String) As Date
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Dir(Value) & "" = "" Then
            Set m_FileSys = New Scripting.FileSystemObject
            GetModifiedDate = m_FileSys.GetFile(Value).DateLastModified
            Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetModifiedDate()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetDrive
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetDrive(Value As String) As Scripting.Drive
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
        Set p_Other = New Scripting.FileSystemObject
        Set GetDrive = p_Other.GetFile(Value).Drive
        Set p_Other = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetDrive()"
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetDrive
'   Parameters:      String
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetType(Value As String) As String
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
        Set p_Other = New Scripting.FileSystemObject
        GetType = p_Other.GetFile(Value).Type
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetType()"
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type:            Function Procedure
'   Name:            FileExists
'   Parameters:      String
'   Retval:          Boolean
'   Purpose:
'---------------------------------------------------------------------------------
Public Static Function Verify(Value As String) As Boolean
    On Error GoTo ErrorHandler
    Dim p_Other As Scripting.FileSystemObject
    If Not Value & "" = "" Then
        Set p_Other = New Scripting.FileSystemObject
        If p_Other.FileExists(Value) = True Then
            Verify = True
        Else
            Verify = False
        End If
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Verify"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetStreamReader
'   Parameters:      String
'   Retval:          TextStream
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetStreamReader() As Scripting.TextStream
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Set m_File = m_FileSys.GetFile(m_Path)
        Set GetStreamReader = m_File.OpenAsTextStream(IOMode:=ForReading, _
            Format:=TristateUseDefault)
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetStreamReader()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetStreamReader
'   Parameters:      String
'   Retval:          TextStream
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetStreamWriter() As Scripting.TextStream
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
            Set m_File = m_FileSys.GetFile(m_Path)
            Set GetStreamReader = m_File.OpenAsTextStream(IOMode:=ForWriting, _
                Format:=TristateUseDefault)
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetStreamWriter()"
    Exit Function
End Function


'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            Create()
'   Parameters:      String
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function CreateText(FileName As String) As Scripting.TextStream
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" And _
        Not FileName & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Set CreateText = m_FileSys.CreateTextFile(FileName:=FileName, _
            Overwrite:=True)
        Set m_FileSys = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="CreateText()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            ReadAll
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function ReadAll() As String
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Set m_File = m_FileSys.GetFile(m_Path)
        Dim m_Out As String
        Set m_Stream = m_File.OpenAsTextStream
        m_Out = CStr(m_Stream.ReadAll)
        Set m_FileSys = Nothing
        Set m_File = Nothing
        Set m_Stream = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="ReadAll()"
    Exit Function
End Function





'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function Readline() As String
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Set m_File = m_FileSys.GetFile(m_Path)
        Set m_Stream = m_File.OpenAsTextStream
        Dim p_Out As String
        p_Out = m_Stream.Readline
        Set m_FileSys = Nothing
        Set m_File = Nothing
        Set m_Stream = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Readline()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            Move
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Move(des As String)
    If Not m_Path & "" = "" And _
        Not des & "" = "" Then
        Set m_FileSys = New Scripting.FileSystemObject
        Set m_File = m_FileSys.GetFile(m_Path)
        m_File.Move (des)
        Set m_FileSys = Nothing
        Set m_File = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
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
Public Sub Copy(Source As String, Destination As String)
    On Error GoTo ErrorHandler
    If Not m_Path & "" = "" And _
        Not Dir(Source) & "" = "" And _
        Not Dir(Destination) & "" = "" Then
            Set m_FileSys = New FileSystemObject
            Set m_File = m_FileSys.GetFile(Source)
            m_File.Copy Destination:=Destination, OverWriteFiles:=True
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
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
Public Sub Delete(Value As String)
    On Error GoTo ErrorHandler
    Set m_FileSys = New FileSystemObject
    If Not Dir(Value) & "" = "" Then
        m_FileSys.DeleteFile Value
    End If
    Set m_FileSys = Nothing
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Delete()"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            FileRename
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Rename(Name As String)
    On Error GoTo ErrorHandler
    Dim p_Other As String
    If Not m_Path & "" = "" And _
        Not Name & "" = "" Then
            Name p_Other As Name
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Rename()"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            RecursiveDelete
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub RecursiveDelete(FilePath As String, FileName As String)
    On Error GoTo ErrorHandler
    Dim p_CurrentDirectory As String
    Dim p_CurrentPath As String
    Dim p_File As Variant
    Dim p_Folders As Collection
    Set p_Folders = New Collection
    If Len(Dir(FilePath & FileName)) > 0 Then
       SetAttr FilePath & FileName, vbNormal
       Kill FilePath & FileName
    End If
    p_CurrentDirectory = Dir(FilePath, vbDirectory)
    Do Until p_CurrentDirectory = vbNullString
        If Left(p_CurrentDirectory, 1) <> "." _
            And (GetAttr(FilePath & p_CurrentDirectory) _
            And vbDirectory) = vbDirectory Then
                p_Folders.add FilePath & p_CurrentDirectory & "\"
        End If
        p_CurrentDirectory = Dir()
    Loop
    For Each p_File In p_Folders
        RecursiveDelete CStr(p_File), FileName
    Next p_File
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="RecursiveDelete()"
    Exit Sub
End Sub



'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            Browse()
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Sub Browse()
    On Error GoTo ErrorHandler
    Dim p_Dialog As FileDialog
    Dim result As Integer
    Set p_Dialog = Application.FileDialog(msoFileDialogFilePicker)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select a file"
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    p_Dialog.Filters.add "All files", "*.*"
    If p_Dialog.Show = -1 Then
       Debug.Print p_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="Browse()"
    Exit Sub
End Sub
 
 
 


'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            UnZip()
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function UnZip(Source As String, Destination As String) As String
    On Error GoTo ErrorHandler
    Dim p_FailedReason As String
    Dim p_ZipToolPath As String
    Dim p_ShellCmd As String
    Dim p_Success As Boolean
    p_ZipToolPath = CurrentProject.Path & "\etc\7za"
    p_ShellCmd = p_ZipToolPath & " x " & Source & " -o" & Destination & " -ry"
    p_Success = Shell(p_ShellCmd, vbHide)
    If p_Success = True Then
        Kill Source
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="UnZip()"
    Exit Function
End Function



'---------------------------------------------------------------------------------
'   Type:            Function
'   Name:            GetCurrentDirectory()
'   Parameters:      Void
'   Retval:          String
'   Purpose:
'---------------------------------------------------------------------------------
Public Static Function GetCurrentDirectory() As String
    On Error GoTo ErrorHandler
    m_CurrentDirectory = CurDir()
    GetCurrentDirectory = m_CurrentDirectory
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetCurrentDirectory()"
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
    Dim p_Dialog As FileDialog
    Dim result As Integer
    Set p_Dialog = Application.FileDialog(msoFileDialogSaveAs)
    p_Dialog.AllowMultiSelect = False
    p_Dialog.Title = "Select location to save file."
    p_Dialog.InitialFileName = "C:\"
    p_Dialog.Filters.Clear
    If p_Dialog.Show = -1 Then
       Debug.Print p_Dialog.SelectedItems(1)
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="OpenSaveDialog()"
    Exit Sub
End Sub




'---------------------------------------------------------------------------------
'   Type:            Sub-Procedure
'   Name:            GetAttribute
'   Parameters:      Void
'   Retval:          Void
'   Purpose:
'---------------------------------------------------------------------------------
Public Function GetAttribute() As VbFileAttribute
    On Error GoTo ErrorHandler
    If Not Dir(m_Path) & "" = "" Then
        Dim p_Attribute As VbFileAttribute
        p_Attribute = GetAttr(m_Path)
        GetAttribute = p_Attribute
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="GetAttribute()"
    Exit Function
End Function




'---------------------------------------------------------------------------------
'   Type:        Sub-Procedure
'   Name:        KillFiles
'   Parameters:  Void
'   RetVal:      Void
'   Purpose:
'---------------------------------------------------------------------------------
Private Sub KillFiles()
    On Error GoTo ErrorHandler
    If Not m_FileSys Is Nothing Then
        Set m_FileSys = Nothing
    End If
    If Not m_File Is Nothing Then
        Set m_File = Nothing
    End If
ErrorHandler:
    ProcessError Name:="DataFile", _
        Member:="KillFiles()"
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



