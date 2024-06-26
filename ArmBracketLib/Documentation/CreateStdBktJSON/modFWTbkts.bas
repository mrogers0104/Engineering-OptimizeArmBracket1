Attribute VB_Name = "modFWTbkts"
Option Explicit

Function BktSideWidth(opening As Single, thick As Single, flatWidth As Single) As Single

    Dim Pi As Single: Pi = 4 * Atn(1)
    Dim tm As Single: tm = 0.45 * thick
    Dim Rm As Single: Rm = 4 * tm
    Dim r As Single: r = 4 * thick
    Dim w As Single: w = (opening + 2 * tm) - 2 * Rm
    Dim c As Single: c = Pi * Rm
    
    Dim s As Single: s = flatWidth - (w + c)
    s = s / 2
    
    s = RoundUp(s, 0.25)
    
    BktSideWidth = s
End Function

Function RoundIt(value As Single, nearest As Single) As Single

    Dim val As Single: val = value / nearest
    Dim iVal As Integer: iVal = val
    
    Dim d As Single: d = val - iVal
    
    If d >= 0.5 Then iVal = iVal + 1
    
    RoundIt = iVal * nearest
End Function

Function RoundUp(value As Single, nearest As Single) As Single

    Dim val As Single: val = value / nearest
    Dim iVal As Integer: iVal = val
    
    Dim d As Single: d = val - iVal
    
    If d > 0 Then iVal = iVal + 1
    
    RoundUp = iVal * nearest
End Function

Sub MapToBackend()

    Dim FWTwkSht As Worksheet: Set FWTwkSht = Sheet6
    Dim backWkSht As Worksheet: Set backWkSht = Worksheets("MapToBackend")
    
    Dim rEnd As Integer: rEnd = Sheet6.UsedRange.Rows.Count - 1
    Dim r As Integer
    Dim FWTr As Integer: FWTr = 2
    Dim spec As String
    Dim shape As String
    Dim armDia As Single
    Dim armThk As Single
    Dim armType As String
    Dim bktThk As Single
'    Dim part As Integer: part = 40
'    Dim partNum As String
    
    For r = 5 To rEnd Step 1
        armType = FWTwkSht.Cells(r, "A")

        armDia = FWTwkSht.Cells(r, "B")
        Select Case armDia
            Case Is < 10
                armThk = 0.25
            Case Is < 16.5
                armThk = 0.3125
            Case Else
                armThk = 0.375
        End Select
        
        backWkSht.Cells(FWTr, "A") = FWTwkSht.Cells(r, "B") - 0.25
        backWkSht.Cells(FWTr, "B") = FWTwkSht.Cells(r, "B") + 0.25 ' arm od max
                       
        backWkSht.Cells(FWTr, "C") = IIf(shape = "Arm11", "CustomHex", "Hex")
        backWkSht.Cells(FWTr, "D") = armThk
        backWkSht.Cells(FWTr, "E") = SplitYokeID(FWTwkSht, r)  ' bracket ID
        
        bktThk = FWTwkSht.Cells(r, "D")    ' bkt thick
        backWkSht.Cells(FWTr, "F") = bktThk    ' bkt thick
        backWkSht.Cells(FWTr, "G") = FWTwkSht.Cells(r, "Z")    ' bkt side width
        
        backWkSht.Cells(FWTr, "H") = FWTwkSht.Cells(r, "H") ' Thru Plate ID (single)
        backWkSht.Cells(FWTr, "I") = FWTwkSht.Cells(r, "I") ' Thru Plate ID (double)
        
        backWkSht.Cells(FWTr, "J") = FWTwkSht.Cells(r, "J")    ' thru plate thick
        backWkSht.Cells(FWTr, "K") = FWTwkSht.Cells(r, "M")    ' thru plate width
        backWkSht.Cells(FWTr, "X") = FWTwkSht.Cells(r, "M") + 0.125     ' bkt opening
        
        backWkSht.Cells(FWTr, "L") = FWTwkSht.Cells(r, "N")    ' thru plate ht
        backWkSht.Cells(FWTr, "W") = FWTwkSht.Cells(r, "P")    ' edge distance

        backWkSht.Cells(FWTr, "Q") = FWTwkSht.Cells(r, "Q")    ' bolt count
        backWkSht.Cells(FWTr, "R") = "A325"
        backWkSht.Cells(FWTr, "S") = FWTwkSht.Cells(r, "U")    ' bolt dia
        backWkSht.Cells(FWTr, "T") = FWTwkSht.Cells(r, "W")    ' bolt length
        
        backWkSht.Cells(FWTr, "U") = 0    ' bkt black wt
        backWkSht.Cells(FWTr, "V") = 0   ' bkt black wt
        
        Dim stifQty As Integer: stifQty = 0
        Dim stifThk As Single: stifThk = 0
        Dim stifWid As Single: stifWid = 0
        Dim stifSpc As Single: stifSpc = 0
        
        If armDia >= 13 Then
            stifQty = 2
            stifThk = bktThk
            stifWid = GetStiffnerWidth(armDia + 0.25)
            stifSpc = GetStiffnerSpacing(armDia + 0.25)
        End If
        ' ** Stiffner
        backWkSht.Cells(FWTr, "M") = stifQty
        backWkSht.Cells(FWTr, "N") = stifThk
        backWkSht.Cells(FWTr, "O") = stifWid
        backWkSht.Cells(FWTr, "P") = stifSpc
        
        FWTr = FWTr + 1
        
nextRow:
    Next r
    
    
    
End Sub

Function SplitYokeID(wkSht As Worksheet, row As Integer) As String

    Dim val As String: val = wkSht.Cells(row, "C")
    Dim w() As String: w = Split(val, " ")
    
    SplitYokeID = ""
    
    If UBound(w) > 0 Then
        SplitYokeID = w(0)
    End If

End Function

Function GetStiffnerWidth(armDia As Single) As Single
    GetStiffnerWidth = 0
    
    Select Case armDia
        Case Is < 29.5
            GetStiffnerWidth = 4.5
        Case Is < 27.25
            GetStiffnerWidth = 4.25
        Case Is < 25.25
            GetStiffnerWidth = 4#
        Case Is < 21.25
            GetStiffnerWidth = 3.75
        Case Is < 20.25
            GetStiffnerWidth = 3.25
        Case Is < 15.5
            GetStiffnerWidth = 3#
        Case Is < 11.5
            GetStiffnerWidth = 2.5
        Case Else
            GetStiffnerWidth = 0
    End Select
End Function

Function GetStiffnerSpacing(maxArmDia As Single) As Single

    Dim wid As Single: wid = maxArmDia + 1
    
    wid = RoundIt(wid, 0.25)
    
    GetStiffnerSpacing = wid
End Function

