﻿using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ServiceStack.Examples.ServiceModel")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ServiceStack.Examples.ServiceModel")]
[assembly: AssemblyCopyright("Copyright ©  2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8870d9fc-01e9-46e9-a89f-e3194f965096")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.1.1205.1040")]
[assembly: AssemblyFileVersion("1.1.1205.1040")]

//MONO 2.8 now supports this so we can remove [DataContract(Namespace = ExampleConfig.DefaultNamespace)] when its popular
[assembly: ContractNamespace("http://schemas.servicestack.net/types",
	ClrNamespace = "ServiceStack.Examples.ServiceModel.Operations")]
[assembly: ContractNamespace("http://schemas.servicestack.net/types",
	ClrNamespace = "ServiceStack.Examples.ServiceModel.Types")]
