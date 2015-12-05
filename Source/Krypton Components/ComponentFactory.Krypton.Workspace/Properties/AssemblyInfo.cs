﻿// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2012. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//  Version 4.4.1.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Security;
using System.Resources;
using System.Reflection;
using System.Security.Permissions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;

[assembly: AssemblyVersion("4.4.1.0")]
[assembly: AssemblyFileVersion("4.4.1.0")]
[assembly: AssemblyCopyright("© Component Factory Pty Ltd 2012. All rights reserved.")]
[assembly: AssemblyInformationalVersion("4.4.1.0")]
[assembly: AssemblyProduct("Krypton Workspace")]
[assembly: AssemblyDefaultAlias("ComponentFactory.Krypton.Workspace.dll")]
[assembly: AssemblyTitle("ComponentFactory.Krypton.Workspace")]
[assembly: AssemblyCompany("Component Factory Pty Ltd")]
[assembly: AssemblyDescription("ComponentFactory.Krypton.Workspace")]
[assembly: AssemblyConfiguration("Production")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: StringFreezing]
[assembly: ComVisible(true)]
[assembly: CLSCompliant(true)]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
//[assembly: AllowPartiallyTrustedCallers()]
[assembly: Dependency("System", LoadHint.Always)]
[assembly: Dependency("System.Drawing", LoadHint.Always)]
[assembly: Dependency("System.Windows.Forms", LoadHint.Always)]
[assembly: Dependency("ComponentFactory.Krypton.Toolkit", LoadHint.Always)]
[assembly: Dependency("ComponentFactory.Krypton.Navigator", LoadHint.Always)]
