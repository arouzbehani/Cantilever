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
1- I figured out that for adding C# dlls to VBA project as a reference they should be first registered with regsam.exe command:
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

## References
+ The code for Deflection Analysis is written with the help of Python project named  [**PythonFEM**](https://github.com/vishnurvp/PythonFEM) and for the purpose of matrix calculations the nuget package [**mathnet-numerics**](https://github.com/mathnet/mathnet-numerics) is installed.

+ The accuracy of the displacements are validated by analysis of a the same cantilever beam modeled in [**CSI SAP 2022**](https://www.csiamerica.com/products/sap2000) sofware.

