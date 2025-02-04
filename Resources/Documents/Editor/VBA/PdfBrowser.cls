Option Compare Database
Option Explicit

Private mDocument As String


Private Sub Form_Load()
    If Not Me.OpenArgs & "" = "" Then
        mDocument = CStr(Me.OpenArgs)
        Me.Document.ControlSource = Application.CurrentProject.path _
            & "\etc\docs\rmds" & mDocument & ".pdf"
    Else
        mDocument = "ObjecClassManual"
    End If
End Sub


Private Sub Form_Current()
    Dim sHTMLFile             As String    'Path to the HTML File to be rendered in the Webbrowser Control
    Dim sPDFFile              As String    'Path to each PDF
    Dim sHTML                 As String    'HTML for the HTML File to be rendered in the Webbrowser Control

    sHTMLFile = Application.CurrentProject.path & "\PDF.html"
    sPDFFile = Application.CurrentProject.path & "\etc\docs\rmds" & mDocument & ".pdf"
    sHTML = "<!DOCTYPE html>" & vbCrLf
    sHTML = sHTML & "<!-- saved from url=(0016)http://localhost -->" & vbCrLf
    sHTML = sHTML & "<html>" & vbCrLf
    sHTML = sHTML & "   <head>" & vbCrLf
    sHTML = sHTML & "       <meta charset=""UTF-8"">" & vbCrLf
    sHTML = sHTML & "       <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />" & vbCrLf
    sHTML = sHTML & "       <title>Inline Documents</title>" & vbCrLf
    sHTML = sHTML & "       <style>html, body, object { height: 100%; width: 100%; }</style>" & vbCrLf
    sHTML = sHTML & "   </head>" & vbCrLf
    sHTML = sHTML & "   <body>" & vbCrLf
    sHTML = sHTML & "       <object data=""" & sPDFFile & "#view=fitH,100&scrollbar=1&toolbar=0&statusbar=0&navpanes=0"" type=""application/pdf"">" & vbCrLf
    sHTML = sHTML & "           <p>It appears you don't have Adobe Reader or PDF support in this web browser. <a href=""" & sPDFFile & """>Click here to download the PDF</a>.</p>" & vbCrLf
    sHTML = sHTML & "           <embed src=""" & sPDFFile & "#view=fitH,100&scrollbar=1&toolbar=0&statusbar=0&navpanes=0"" type=""application/pdf""/>" & vbCrLf
    sHTML = sHTML & "       </object>" & vbCrLf
    sHTML = sHTML & "   </body>" & vbCrLf
    sHTML = sHTML & "</html>"
    
    
'    'Dynamic HTML, no I/O!  ***********DOES NOT WORK****************
'    With Me.WB_Document.Object.Document
'        .Open
'        .Write sHTML
'        .Close
'    End With

    Call OverwriteTxt(sHTMLFile, sHTML)

    Me.Document.ControlSource = "=""" & sHTMLFile & """"
    On Error Resume Next
    Me.Document.Object.Refresh
End Sub
