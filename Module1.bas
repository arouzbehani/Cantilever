Option Compare Database

Sub CalculateDeflection()
   
    Dim beamObj As New Cantilever.BeamElement

    beamObj.force = CDbl(Form_Form1.txtForce.Value)
    beamObj.length = CDbl(Form_Form1.txtLength.Value)
    beamObj.sectionId = Int(Form_Form1.cmbSections.Value)
    beamObj.matId = Int(Form_Form1.cmbMaterials.Value)
    
    Dim meshNum As Integer
    meshNum = Int(Form_Form1.txtMeshNum.Value)
    res = beamObj.Displacements(meshNum)
    Dim x_coord() As Variant
    Dim z_coord() As Variant
    
    ReDim x_coord(0 To meshNum)
    ReDim z_coord(0 To meshNum)
    
    For i = 0 To meshNum
        z_coord(i) = Round(res(i), 4)
        x_coord(i) = i / meshNum * beamObj.length
    Next i
    AddArraysToDeflections x_coord(), z_coord()
    Form_Form1.chartDeflections.Requery


End Sub
Sub CalculateDeflection_FromWebService()
   Dim meshNum As Integer
   Dim length As Double
   length = CDbl(Form_Form1.txtLength.Value)
   meshNum = Int(Form_Form1.txtMeshNum.Value)
    res = GetDisplacements(Int(Form_Form1.cmbMaterials.Value), Int(Form_Form1.cmbSections.Value), CDbl(Form_Form1.txtForce.Value), length, Int(Form_Form1.txtMeshNum.Value))

    Dim x_coord() As Variant
    Dim z_coord() As Variant
    
    ReDim x_coord(0 To meshNum)
    ReDim z_coord(0 To meshNum)
    
    For i = 0 To meshNum
        z_coord(i) = Round(res(i), 4)
        x_coord(i) = i / meshNum * length
    Next i
    AddArraysToDeflections x_coord(), z_coord()
    Form_Form1.chartDeflections.Requery


End Sub
Sub AddArraysToDeflections(x_coord() As Variant, z_coord() As Variant)
    Dim db As DAO.Database
    Dim rs As DAO.Recordset
    Dim strSQL As String
    Dim i As Long
    

    
    ' Open the database
    Set db = CurrentDb
    
    ' Clear existing records from Deflections table
    strSQL = "DELETE FROM Deflections;"
    db.Execute strSQL, dbFailOnError
    
    ' Insert new records from arrays
    For i = LBound(x_coord) To UBound(x_coord)
        strSQL = "INSERT INTO Deflections (X, Z) VALUES (" & x_coord(i) & ", " & z_coord(i) & ");"
        db.Execute strSQL, dbFailOnError
    Next i
    
    ' Clean up
    Set rs = Nothing
    Set db = Nothing
End Sub
Function GetDisplacements(matId As Integer, secId As Integer, force As Double, length As Double, meshNum As Integer) As Variant
    Dim objHTTP As Object
    Dim baseUrl As String
    Dim fullUrl As String
    Dim strResponse As String

    ' Set the URL of the web service
    baseUrl = "http://localhost:8686/BeamApi"
    fullUrl = baseUrl & "?matId=" & matId & "&secId=" & secId & "&force=" & force & "&length=" & length & "&meshNum=" & meshNum

    ' Create an HTTP request object
    Set objHTTP = CreateObject("MSXML2.ServerXMLHTTP")

    ' Open a GET request to the URL
    objHTTP.Open "GET", fullUrl, False

    ' Send the request
    objHTTP.send ""

    ' Handle The Response
    Dim responseText As String
    Dim listOfDoubles() As String

    responseText = objHTTP.responseText
    listOfDoubles = Split(responseText, ",")
    
    Dim i As Integer
    Dim numstr As String
    For i = LBound(listOfDoubles) To UBound(listOfDoubles)
        numstr = Replace(listOfDoubles(i), "[", "")
        numstr = Replace(numstr, "]", "")
        listOfDoubles(i) = CDbl(numstr)
    Next i
    GetDisplacements = listOfDoubles
    ' Clean up the HTTP object
    Set objHTTP = Nothing
    
    
End Function

