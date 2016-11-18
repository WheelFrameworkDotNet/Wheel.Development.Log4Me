using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Wheel.Configuration;
using Wheel.Configuration.XML;
using Wheel.Development.Log4Me.Config.Entities;
using Wheel.Development.Log4Me.Writers;
using Wheel.Util.FileSystem;

namespace Wheel.Development.Log4Me.Config
{
    /// <summary>
    /// Administrador de la configuración de Log4Me.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <h2 class="groupheader">Registro de versiones</h2>
    ///         <ul>
    ///             <li>1.0.0</li>
    ///             <table>
    ///                 <tr style="font-weight: bold;">
    ///                     <td>Autor</td>
    ///                     <td>Fecha</td>
    ///                     <td>Descripción</td>
    ///                 </tr>
    ///                 <tr>
    ///                     <td>Marcos Abraham Hernández Bravo.</td>
    ///                     <td>11/11/2016</td>
    ///                     <td>Versión Inicial.</td>
    ///                 </tr>
    ///             </table>
    ///         </ul>
    ///     </para>
    /// </remarks>
    public sealed class Log4MeConfigManager : ConfiguracionXmlManager<Log4MeConfig>
    {
        /// <summary>
        /// Permite obtener el nombre del archivo de configuración en Web.Config o App.Config.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        public override string NombreArchivoConfiguracion
        {
            get { return "Log4MeConfig"; }
        }

        /// <summary>
        /// Permite obtener el nombre del archivo de validación (XSD) del XML establecido en Web.Config o App.Config.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        public override string NombreArchivoValidacionConfiguracion
        {
            get { return "Log4MeConfigValidator"; }
        }

        /// <summary>
        /// Permite obtener un valor que indica si se debe validar el XML mediante un XSD.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        protected override bool ValidarXmlSchema
        {
            get { return false; }
        }

        /// <summary>
        /// Permite obtener el nombre del archivo de configuración por defecto.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        public override string NombreArchivoPorDefecto
        {
            get { return "Log4Me.Config.xml"; }
        }

        /// <summary>
        /// Permite obtener el nombre del archivo de validación (XSD) del XML por defecto.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        public override string NombreArchivoValidacionPorDefecto
        {
            get { return "Log4Me.Config.xsd"; }
        }

        /// <summary>
        /// Obtiene el serializador XML para el archivo de configuración. Carga los ensamblados de los escritores de Log4Me.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        /// <returns></returns>
        protected override XmlSerializer ObtenerXmlSerializer()
        {
            XmlAttributeOverrides xOver = new XmlAttributeOverrides();
            XmlAttributes xAttrs = new XmlAttributes();

            List<Assembly> ensamblados = new List<Assembly>();
            Assembly assembly = null;
            
            string ruta = WheelConfigurationManager.ObtenerValorPropiedad<string>("Log4Me", "LibrariesFolderWriters");
            ruta = FileSystemUtil.ObtenerRutaAbsoluta(ruta);

            string[] archivos = Directory.GetFiles(ruta, "*.dll");

            foreach (string archivo in archivos)
            {
                if (Path.GetFileName(archivo).StartsWith("Wheel.Development.Log4Me.", StringComparison.InvariantCultureIgnoreCase))
                {
                    assembly = Assembly.LoadFile(archivo);
                    ensamblados.Add(assembly);
                }
            }

            var tipos = (from lAssembly in ensamblados
                         from tipo in lAssembly.GetTypes()
                         where typeof(ALogWriter).IsAssignableFrom(tipo)
                         where !tipo.IsAbstract
                         select tipo).ToArray();

            foreach (var tipo in tipos)
            {
                xAttrs.XmlArrayItems.Add(new XmlArrayItemAttribute(tipo));
            }

            xOver.Add(typeof(Log4MeConfig), "Writers", xAttrs);

            return new XmlSerializer(typeof(Log4MeConfig), xOver);
        }

        /// <summary>
        /// Valida el archivo de configuración.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <h2 class="groupheader">Registro de versiones</h2>
        ///         <ul>
        ///             <li>1.0.0</li>
        ///             <table>
        ///                 <tr style="font-weight: bold;">
        ///                     <td>Autor</td>
        ///                     <td>Fecha</td>
        ///                     <td>Descripción</td>
        ///                 </tr>
        ///                 <tr>
        ///                     <td>Marcos Abraham Hernández Bravo.</td>
        ///                     <td>11/11/2016</td>
        ///                     <td>Versión Inicial.</td>
        ///                 </tr>
        ///             </table>
        ///         </ul>
        ///     </para>
        /// </remarks>
        /// <param name="documento">Archivo de configuración como documento XML.</param>
        protected override void ValidarXml(XmlDocument documento) { }
    }
}