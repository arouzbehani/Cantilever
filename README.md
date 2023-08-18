In this project I tried to Make a Deflection Analysis of a Cantilever Beam with Finite Element Analysis method using C# .NET.
For the purpose of data management and presentation I have used VBA in MS ACCESS.
The code for the Beam Analysis has these Classes/Interfaces:
BeamElement
Material
Isection
SectionType
RectSection
DataReader


The code for Deflection Analysis is written with the help of PythonFEM project "https://github.com/vishnurvp/PythonFEM"
MathNet.Numerics Nuget package is also used for Matrix Calculations. "https://github.com/mathnet/mathnet-numerics"
DataReader.cs is responsible for only reading the Materials and Sections from the MsAccess Database
The Beam is initialized simply by its length,section Id,material Id, and force value which are provided from a VBA form inside the access file and then it returns an array of displacements with respect to mesh number parameter passed to its Displacements method.
The result is displayed in a form of line chart which is bounded to the Deflections table.

The accuracy of the displacements are validated by analysis of a the same cantilever beam modeled in CSI SAP2022 sofware.


![Alt Text](https://github.com/arouzbehani/Cantilever/blob/master/Cantilever/front.png)
