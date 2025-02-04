Option Compare Database
Option Explicit


Private strFormName As String



Private Sub Class_Initialize()
End Sub



'----------------------------------------------------------------------------------
'   Type        SubProcedure
'   Name        DisplayErrorMessage
'   Parameters  String - ErrorNumber; String - ErrorDescription
'   RetVal      NA
'   Purpose     Subprocedure to open the ErrorDialog form and display message
'---------------------------------------------------------------------------------
Public Sub DisplayErrorMessage(ErrorNumber As String, ErrorDescription As String)
    Error.TextBox = "Error: " & ErrorNumber & " " & ErrorDescription
    DoCmd.OpenForm ("ErrorDialog")
End Sub



'----------------------------------------------------------------------------------
'   Type        Function
'   Name        GetProperty
'   Parameters  String - strFormName, strCtrlName, strPropName
'   RetVal      Variant
'   Purpose     Function that returns the value of a control's property given
'               a form's name, the control, and the property name
'---------------------------------------------------------------------------------
Public Function GetProperty(ByVal strFormName As String, ByVal strCtrlName As String, ByVal strPropName As String) As Variant

    Dim FrmName As Form
    Dim strMsg As String
    Dim strFunction As String
    Dim strObjName As String

    On Error GoTo HandleError
    GetProperty = "ERROR"
    strFunction = "SetProperty"

    If Len(strCtrlName) > 0 Then
        strObjName = strCtrlName
        Set FrmName = Forms(strFormName)
        GetProperty = FrmName(strCtrlName).Properties(strPropName)
        ElseIf Len(strFormName) = 0 Then
            Exit Function

        strObjName = strFormName
        Set FrmName = Forms(strFormName)
        GetProperty = FrmName.Properties(strPropName)
    End If

ExitHere:
        Exit Function
HandleError:
        Select Case Err
            Case 2450
                strMsg = "'" & strFormName & " ' is not an open from"

            Case 2465
                strMsg = "'" & strCtrlName & "' is not a control on '" & strFormName & "'"

            Case 2455
                strMsg = "'" & strPropName & "' is not a property of '" & strObjName & "'"

            Case Else
                strMsg = "Error - " & Err & ": " & Err.Description
        End Select

    MsgBox strMsg, vbExclamation + vbOKOnly, "Error in Procedure " & strFunction
    GetProperty = strMsg

    Resume ExitHere
End Function

