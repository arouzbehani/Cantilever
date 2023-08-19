Attribute VB_Name = "Module1"
Option Compare Database

Sub CalculateDeflection()
   
    Dim beamObj As New Cantilever.BeamElement

    beamObj.Force = CDbl(Form_Form1.txtForce.Value)
    beamObj.Length = CDbl(Form_Form1.txtLength.Value)
    beamObj.SectionId = Int(Form_Form1.cmbSections.Value)
    beamObj.MatId = Int(Form_Form1.cmbMaterials.Value)
    
    Dim meshNum As Integer
    meshNum = Int(Form_Form1.txtMeshNum.Value)
    res = beamObj.Displacements(meshNum)
    Dim x_coord() As Variant
    Dim z_coord() As Variant
    
    ReDim x_coord(0 To meshNum)
    ReDim z_coord(0 To meshNum)
    
    For i = 0 To meshNum
        z_coord(i) = Round(res(i), 4)
        x_coord(i) = i / meshNum * beamObj.Length
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
