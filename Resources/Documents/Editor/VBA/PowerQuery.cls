Option Explicit



'-------------------------------------------------------------------------------------
'Purpose:   Refresh all data connections in the active workbook
'           after the sub below turns off all background refreshing
'Source:  
'Editor:    Jenn Ratten
'Revisions
'01/30/19   Enhanced the speed
'-------------------------------------------------------------------------------------
Sub Refresh_All_Data_Connections()    
    For Each objConnection In ActiveWorkbook.Connections
        'Get current background-refresh value
        bBackground = objConnection.OLEDBConnection.BackgroundQuery

        'Temporarily disable background-refresh
        objConnection.OLEDBConnection.BackgroundQuery = False

        'Refresh this connection
        objConnection.Refresh

        'Set background-refresh value back to original value
        objConnection.OLEDBConnection.BackgroundQuery = bBackground
    Next
End Sub



'-------------------------------------------------------------------------------------
' Purpose:   Turn off background refreshing for all workbook connections.
'            This prevents hangs and crashes
' Source:    
' Editor:    Jenn Ratten=
' Revisions
' 01/30/19   Enhanced the speed
'-------------------------------------------------------------------------------------
Sub ChangeConnectionRefreshModeAndRefreshAll( )
    Dim Connection As WorkbookConnection
    Dim bugfix As Integer
        For bugfix = 1 To 2
            On Error Resume Next
            For Each Connection In ActiveWorkbook.Connections    
                With Connection    
                    If (.Type = xlConnectionTypeODBC) Then
                        .ODBCConnection.BackgroundQuery = False               
                    Else            
                        If (.Type = xlConnectionTypeOLEDB) Then
                            .OLEDBConnection.BackgroundQuery = False                
                        End If                  
                    End If               
                End With
            Connection.Refresh   
            Next Connection
        Next bugfix
End Sub