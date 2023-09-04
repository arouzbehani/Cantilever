# Cantilever Beam FEM Deflection Analysis
In this project I tried to make a *Deflection Analysis* of a Cantilever Beam with the Finite Element Analysis method using C# .NET.

For the purpose of data management and presentation I used VBA in MS ACCESS.

The code for the Beam Analysis has these Classes/Interfaces located in [**Cantilever Folder**](https://github.com/arouzbehani/Cantilever/tree/master/Cantilever):
- BeamElement.cs
- Material.cs
- Isection.cs
- SectionType.cs
- RectSection.cs
- DataReader.cs

[BeamElement.cs](https://github.com/arouzbehani/Cantilever/blob/master/Cantilever/BeamElement.cs) is the main class for defining a cantilever beam and other classes are created to define physical and section properties of the beam.

[DataReader.cs](https://github.com/arouzbehani/Cantilever/blob/master/Cantilever/BeamElement.cs) is also responsible for reading the Materials and Sections from the MsAccess Database.

The Beam is initialized simply by its length,section Id,material Id, and force value in [**vba module**](https://github.com/arouzbehani/Cantilever/blob/master/Module1.bas) which are provided from a VBA form inside the access file and then it returns an array of displacements with respect to mesh number parameter passed to its Displacements method.
```vb
    Dim beamObj As New Cantilever.BeamElement

    beamObj.Force = CDbl(Form_Form1.txtForce.Value)
    beamObj.Length = CDbl(Form_Form1.txtLength.Value)
    beamObj.SectionId = Int(Form_Form1.cmbSections.Value)
    beamObj.MatId = Int(Form_Form1.cmbMaterials.Value)
    
    Dim meshNum As Integer
    meshNum = Int(Form_Form1.txtMeshNum.Value)
    res = beamObj.Displacements(meshNum)
```
The result is displayed in the form of line chart which is bounded to the Deflections table.

[**https://youtu.be/fbS7ZiWBtJo**](https://youtu.be/fbS7ZiWBtJo)

[![Cantilever Beam Cover](https://github.com/arouzbehani/Cantilever/blob/master/res/cover.png)](https://youtu.be/fbS7ZiWBtJo)

## Dll Tester
[**Dll Tester**](https://github.com/arouzbehani/Cantilever/tree/master/DLL_Tester) is a Simple C# Console Application which is created to test and debug the Cantilever project by adding it as a reference.
```C#
﻿using System;
using Cantilever;
namespace DLL_Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BeamElement beam = new BeamElement(3000, 1,1,1000);
            Console.WriteLine(beam.Displacements());
        }
    }
}
```

## Adding .NET dll to vba project Considerations
1- I figured out that for adding C# dlls to VBA project as a reference they should be first registered with regasm.exe command:
<!---->
```
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe cantilever.dll /codebase /tlb 
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe cantilever.dll /codebase
```      
then the ***created tlb file*** should be added as a reference.

2- It was advised to decorate BeamElement class with 
```C#
[ClassInterface(ClassInterfaceType.AutoDual)]
```

3- Registering C# project for COM interop is necessary

4- Com visibility should be set to **true** in [assemblyInfo.cs](https://github.com/arouzbehani/Cantilever/blob/master/Cantilever/Properties/AssemblyInfo.cs) file: 
```c#
[assembly: ComVisible(true)]
```

## Web API Approach
I added an API endpoint in which the VBA project as well as any other front soloution can use the FEM Calculations in C#. The [**BeamApi**](https://github.com/arouzbehani/Cantilever/blob/master/API/Controllers/BeamApiController.cs) Controller adds [**Cantilever**](https://github.com/arouzbehani/Cantilever/tree/master/Cantilever) project as reference and receives length,section id , ... and othere parameters as input.
```C#
        using Microsoft.AspNetCore.Mvc;
        using Cantilever;

        [HttpGet(Name = "GetDisplacements")]
        public IList<double> Get(int matId=1,int secId=1,double force=1000,double length=3000, int meshNum=10)
        {
            try
            {
            var beam = new BeamElement(length, matId,secId, force);
            return beam.Displacements(meshNum);

            }
            catch (Exception exc)
            {

                return new List<double> { 0};
            }

        }

```
## Consuming Web Service in VBA
The Advantage of using Web Service in VBA Project is that it will be independent on importing dll files and no references needes to be imported after every changes in C# code.

For this purpose the [**Module1**](https://github.com/arouzbehani/Cantilever/blob/master/Module1.bas) is updated and a Function for getting displacements is added as follow:

```VBA
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
```
## Data Accessibility Considerations
Accessing an MS ACCESS Database is only applicable with Windows machines. Although the web API project is written in .NET Core and is able to run on either a Linux or Windows machine, reading data using the OleDb library can only be done on a Windows machine.

Therefore, for development purposes and a more distributed solution, it is wise to decouple the database from the frontend and backend domains.

## References
+ The code for Deflection Analysis is written with the help of Python project named  [**PythonFEM**](https://github.com/vishnurvp/PythonFEM) and for the purpose of matrix calculations the nuget package [**mathnet-numerics**](https://github.com/mathnet/mathnet-numerics) is installed.

+ The accuracy of the displacements are validated by analysis of the same cantilever beam modeled in [**CSI SAP 2022**](https://www.csiamerica.com/products/sap2000) sofware.

