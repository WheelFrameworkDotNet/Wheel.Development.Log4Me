using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: ComVisible(true)]
[assembly: AssemblyTitle("Wheel.Development.Log4Me")]
[assembly: AssemblyDescription("Logea una aplicación de forma automática o manual. Parte de WheelFramework.")]
[assembly: AssemblyCompany("Marcos Abraham Hernández Bravo")]
[assembly: AssemblyProduct("Wheel.Development.Log4Me")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: NeutralResourcesLanguageAttribute("es-CL")]
[assembly: Guid("CFB5C538-AFEA-4A86-B206-2210F5262335")]

// La información de versión esta regida por el Versionamiento Semántico 2.0.0-rc.2 (http://semver.org/lang/es/)
[assembly: AssemblyVersion("1.0.0")]
[assembly: AssemblyFileVersion("1.0.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyConfiguration("Release")]
#endif