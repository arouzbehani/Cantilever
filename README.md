In this project I tried to Make a Deflection Analysis of a Cantilever Beam with Finite Element Analysis method using C# .NET.
For the purpose of data management and presentation I used VBA in MS ACCESS.
The code for the Beam Analysis has these Classes/Interfaces:
BeamElement
Material
Isection
SectionType
RectSection
DataReader

The code for Deflection Analysis is written with the help of PythonFEM project "https://github.com/vishnurvp/PythonFEM"
MathNet.Numerics Nuget package is also used for Matrix Calculations. "https://github.com/mathnet/mathnet-numerics"
The accuracy of the displacements are validated by analysis of a the same cantilever beam modeled in CSI SAP2022 sofware.

DataReader.cs is responsible for only reading the Materials and Sections from the MsAccess Database
The Beam is initialized simply by its length,section Id,material Id, and force value which are provided from a VBA form inside the access file and then it returns an array of displacements with respect to mesh number parameter passed to its Displacements method.
The result is displayed in a form of line chart which is bounded to the Deflections table.

![Alt Text](https://github.com/arouzbehani/Cantilever/blob/master/Cantilever/front.png)

Tips for adding .NET dll to vba project
1- I figured out that for adding C# dlls to VBA project as a reference they should be first registered with regsam.exe command:
      C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe cantilever.dll /codebase /tlb 
      then the tlb file can be added as a reference.
2- It was advised to decorate BeamElement class with [ClassInterface(ClassInterfaceType.AutoDual)]
3- Registering C# project for COM interop is necessary
4- Com visibility should be set to true in assemblyInfo.cs file: [assembly: ComVisible(true)]


