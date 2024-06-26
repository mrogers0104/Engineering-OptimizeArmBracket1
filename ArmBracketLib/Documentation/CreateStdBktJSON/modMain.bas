Attribute VB_Name = "modMain"
Option Explicit

Public eltMapTable As ListObject
Public eltMapArray As Variant

Public armBktWkshtName As String

' *********************************************
' ******   M  A  I  N       E X P O R T *******
' *********************************************
' ** Export the Sabre Std Brackets table to JSON.
' ** This JSON will not contain the FWT Std Bkts.
' ** They will need to be added manually for now (7/27/17)
' **
Sub ExportSabreFWTStdBracketsWithHex()

    ' ** "New Std" arm bracket to convert to JSON
    armBktWkshtName = "ArmBracket110717"
    
    ' 1. create a collection with keys
    '   see url: https://excelmacromastery.com/vba-dictionary/#Checking_if_a_Key_Exists
    '   first row in worksheet will contain the keys
    ' 2. convert to JSON
    ' 3. write JSON string to file.
    
    Dim fso As FileSystemObject: Set fso = New FileSystemObject
    Dim path As String: path = "I:\Documentation\Backend\SabreFWTstdBrackets"
    Dim fileJSON As String: fileJSON = "NewJSON.txt"
    Dim fullPath As String: fullPath = fso.BuildPath(path, fileJSON)
    
    Dim cntSabre As Integer: cntSabre = 0
    Dim cntFWT As Integer: cntFWT = 0
    
    Dim jLine As String
    
    Open fullPath For Output As #1
    
    jLine = "["
    
    Call ExportSabreStdBkt(jLine, cntSabre)
    
    'Call ExportFWTStdBkt(jLine, cntFWT)
    
    ' ** Replace the last comma, with a square bracket ]
    Dim i As Integer: i = 0
    i = InStrRev(jLine, ",")
    jLine = Left(jLine, i - 1) & "]"
    'jLine = jLine & "]"
    
    Print #1, jLine
    
    Close #1
    
    Application.StatusBar = "JSON file complete: " & fileJSON & " ::: Sabre = " & cntSabre & " rows &&&  FWT = " & cntFWT & " rows"
    
End Sub

Sub ExportSabreStdBkt(ByRef jLine As String, cnt As Integer)
    Dim s As Integer
    Dim h As Integer
    
    Dim r As Integer
    Dim c As Integer
    Dim wkSht As Worksheet: Set wkSht = Worksheets(armBktWkshtName)
    Dim strtRow As Integer: strtRow = 7
'    Dim hdrRow As Integer: hdrRow = strtRow - 1
    Dim endRow As Integer: endRow = 44 ' wkSht.UsedRange.Rows.Count
    Dim endCol As Integer: endCol = 43  ' column "AQ"
    
    Dim elt As String
    Dim elt1 As String
    Dim elt2 As String
    
    Dim val As String
    Dim q As String: q = Chr(34)    ' double quote
      
    For r = strtRow To endRow
        jLine = jLine & "{" & vbCrLf
        
        For c = 2 To endCol
            elt = FindElement(c)
'            ' ** element name could be on hdrRow or hdrRow+1
'            ' ** must check both
'            If Not (wkSht.Cells(hdrRow, c).Comment Is Nothing) Then elt1 = wkSht.Cells(hdrRow, c).Comment.Text
'            If Not (wkSht.Cells(hdrRow + 1, c).Comment Is Nothing) Then elt2 = wkSht.Cells(hdrRow + 1, c).Comment.Text
'
'            If elt1 <> "" Then
'                elt = elt1
'            ElseIf elt2 <> "" Then
'                elt = elt2
'            Else
'                GoTo nextCol
'            End If

            If elt = "" Then GoTo nextCol
            
            jLine = jLine & q & elt & q & ":"
            
            val = wkSht.Cells(r, c)
            
            ' ** For some reason, Stiffener fields are different.
            ' ** Maybe because they are "Custom"
            h = InStr(elt, "HEX")
            s = InStr(elt, "Stiff")
            If s > 0 Or h > 0 Then
                val = wkSht.Cells(r, c)
                If val = "" Then
                    val = 0
                Else
                    val = CSng(val)
                End If
            End If
            
            If InStr(elt, "BoltSpec") > 0 And InStr(val, "-") > 0 Then
                val = Replace(val, "-", "") ' remove dash from BoltSpec
            End If
                        
            If IsNumeric(val) Then
                jLine = jLine & CStr(val)
            Else
                jLine = jLine & q & val & q
            End If
            
            jLine = jLine & "," & vbCrLf
            
            If InStr(elt, "MaxArmBase") > 0 Then
                jLine = jLine & AddArmType("Common")
            End If
            
            elt1 = ""
            elt2 = ""
            
nextCol:
        Next c
        
        jLine = jLine & "}," & vbCrLf
        
        cnt = cnt + 1
        
nextRow:
    Next r
    
End Sub

Sub ExportFWTStdBkt(ByRef jLine As String, cnt As Integer)
    
    Dim r As Integer
    Dim c As Integer
    Dim wkSht As Worksheet: Set wkSht = Worksheets("FWTstdBracketsWithHex")
    Dim strtRow As Integer: strtRow = 2
    Dim hdrRow As Integer: hdrRow = strtRow - 1
    Dim endRow As Integer: endRow = wkSht.UsedRange.Rows.Count
    Dim endCol As Integer: endCol = wkSht.UsedRange.Columns.Count ' 24  ' column "X"
    
    Dim elt As String
    Dim val As String
    Dim q As String: q = Chr(34)    ' double quote
      
    For r = strtRow To endRow
        jLine = jLine & "{" & vbCrLf
        
        For c = 1 To endCol
            elt = wkSht.Cells(hdrRow, c)
            
            jLine = jLine & q & elt & q & ":"
            
            val = wkSht.Cells(r, c)
            If IsNumeric(val) Then
                jLine = jLine & CStr(val)
            Else
                jLine = jLine & q & val & q
            End If
            
            jLine = jLine & "," & vbCrLf
            
nextCol:
        Next c
        
        jLine = jLine & "}," & vbCrLf
        
        cnt = cnt + 1
nextRow:
    Next r
        
End Sub


Sub GetElementMap()
    Dim x As Long
    Dim mapSht As Worksheet: Set mapSht = Worksheets("ElementMap")
       
    'Set path for Table variable
      Set eltMapTable = mapSht.ListObjects("ElementMap")
    
    'Create Array List from Table
      eltMapArray = eltMapTable.DataBodyRange
    
    'Loop through each item in Third Column of Table (displayed in Immediate Window [ctrl + g])
'      For x = LBound(eltMapArray) To UBound(eltMapArray)
'        Debug.Print eltMapArray(x, 3) & " @ Column: " & eltMapArray(x, 4)
'      Next x
End Sub

Function FindElement(colNum As Integer) As String
    Dim x As Long
    Dim col As Integer
    Dim elt As String: elt = ""
    
    'Dim L As Integer: L = LBound(eltMapArray)
    If IsEmpty(eltMapArray) Then GetElementMap
    
    'Loop through each item in Third Column of Table (displayed in Immediate Window [ctrl + g])
      For x = LBound(eltMapArray) To UBound(eltMapArray)
        If eltMapArray(x, 4) = colNum Then
            elt = eltMapArray(x, 3)
            Exit For
        End If
       ' Debug.Print eltMapArray(x, 3) & " @ Column: " & eltMapArray(x, 4)
      Next x

    FindElement = elt
End Function

Sub WriteHeader(wkSht As Worksheet, JSON As Object)

    Dim key As Variant
    Dim Items As Object
    Dim item As Object
    Dim c As Integer: c = 1
    Dim r As Integer: r = 1
    
    Dim m As Integer: m = JSON.Count
    
    For Each Items In JSON
        
        For Each key In Items.Keys
            wkSht.Cells(r, c) = key
            
            c = c + 1
        Next
        
        Exit For
    Next

End Sub

Function AddArmType(aType As String) As Variant
    Dim q As String: q = Chr(34)    ' double quote
    Dim jLine As String: jLine = ""
    Dim elt As String: elt = "ArmType"
    Dim val As Variant

    jLine = jLine & q & elt & q & ":"
    
    val = aType
    If IsNumeric(val) Then
        jLine = jLine & CStr(val)
    Else
        jLine = jLine & q & val & q
    End If
    
    jLine = jLine & "," & vbCrLf
    
    AddArmType = jLine

End Function


Function BktSideWidth(opening As Single, thick As Single, flatWidth As Single) As Single

    Dim Pi As Single: Pi = 4 * Atn(1)
    Dim tm As Single: tm = 0.45 * thick
    Dim Rm As Single: Rm = 4 * tm
    Dim r As Single: r = 4 * thick
    Dim w As Single: w = (opening + 2 * tm) - 2 * Rm
    Dim c As Single: c = Pi * Rm
    
    Dim s As Single: s = flatWidth - (w + c)
    s = s / 2
    
    BktSideWidth = s
End Function

Function BktFlatWidth(opening As Single, thick As Single, sideWidth As Single) As Single

    Dim Pi As Single: Pi = 4 * Atn(1)
    Dim tm As Single: tm = 0.45 * thick
    Dim Rm As Single: Rm = 4 * tm
    Dim r As Single: r = 4 * thick
    Dim w As Single: w = (opening + 2 * tm) - 2 * Rm
    Dim c As Single: c = Pi * Rm
    
    Dim s As Single: s = sideWidth
    s = s * 2
    
    BktFlatWidth = w + c + s
End Function

