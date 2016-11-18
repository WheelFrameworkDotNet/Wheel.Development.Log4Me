using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Wheel.Development.Log4Me.Config.Entities;
using Wheel.Development.Log4Me.Entities;

namespace Wheel.Development.Log4Me.Writers
{
    /// <summary>
    /// Concepto y contrato de los escritores de Log4Me.
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
    public abstract class ALogWriter
    {
        /// <summary>
        /// Representa el formato predeterminado de salida. Sólo lectura.
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
        public static string FormatoPredeterminado { get { return "[ThreadGUID]{0}[MethodGUID]{0}[Namespace]{0}[Class]{0}[Method]{0}[Type]{0}[DateTime]{0}[VarName]{0}[VarValue]{0}[StackTrace]{0}[Data]{0}[ExceptionType]{0}[Message]{0}[Correlative]{0}"; } }

        /// <summary>
        /// Representa el separador de formato predeterminado de salida. Sólo lectura.
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
        public static char SeparadorPredeterminado { get { return ';'; } }

        /// <summary>
        /// Contiene un valor que debe ser reemplazado al formatear, y el valor con que será reemplazado.
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
        private static IDictionary<char, char> CaracteresDeReemplazo = new Dictionary<char, char>();

        /// <summary>
        /// Contiene un valor que indica si los parámetros han sido persistidos o no.
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
        private static bool parametrosAgregados = false;

        /// <summary>
        /// Contiene un valor que indica si el escritor ha sido inicializado o no.
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
        private bool inicializado = false;

        /// <summary>
        /// Permite obtener o establecer el modo del escritor.
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
        [XmlIgnore]
        public Modo Modo
        {
            get
            {
                return Modo.ToEnumeracion(_Modo);
            }
            set
            {
                _Modo = value == null ? value.Codigo : null;
            }
        }

        /// <summary>
        /// Permite obtener o establecer el modo del escritor.
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
        [XmlAttribute(AttributeName = "Mode")]
        public string _Modo { get; set; }

        /// <summary>
        /// Permite obtener o establecer el separador del texto de salida.
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
        [XmlIgnore]
        public char SeparadorSalida { get; set; }

        /// <summary>
        /// Permite obtener o establecer el separador del texto de salida.
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
        [XmlAttribute(AttributeName = "OutPutSeparator")]
        public string _SeparadorSalida
        {
            get
            {
                return SeparadorSalida.ToString();
            }

            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    SeparadorSalida = value[0];
                }
            }
        }

        /// <summary>
        /// Permite obtener o establecer el formato de salida del escritor.
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
        [XmlAttribute(AttributeName = "OutPutFormat")]
        public string FormatoSalida { get; set; }

        /// <summary>
        /// Permite obtener o establecer un valor que indica si se debe forzar la escritura de todos los campos en la salida.
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
        [XmlAttribute(AttributeName = "OutPutForzeAllFields")]
        public bool ForzarTodosCampos { get; set; }

        /// <summary>
        /// Permite obtener o establecer un valor que indica si se debe saltar la escritura del formato.
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
        [XmlAttribute(AttributeName = "SkipSaveFormat")]
        public bool SaltarGuardadoFormato { get; set; }

        /// <summary>
        /// Permite obtener o establecer la lista de registros incluidos.
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
        [XmlArray(ElementName = "Includes")]
        public List<IncludeTag> Incluidos { get; set; }

        /// <summary>
        /// Permite obtener o establecer la lista de registros excluidos.
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
        [XmlArray(ElementName = "Excludes")]
        public List<ExcludeTag> Excluidos { get; set; }

        /// <summary>
        /// Permite obtener o establecer la lista de niveles.
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
        [XmlArray(ElementName = "Types")]
        public List<TypeTag> Tipos { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia.
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
        public ALogWriter()
        {
            Incluidos = new List<IncludeTag>();
            Excluidos = new List<ExcludeTag>();
            Tipos = new List<TypeTag>();
        }

        /// <summary>
        /// Obtiene el valor de reemplazo de un caracter.
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
        /// <param name="caracterAReemplazar">Caracter a ser reemplazado.</param>
        /// <returns>Valor con el que se debe reemplazar.</returns>
        private char ObtenerCaracterReemplazo(char caracterAReemplazar)
        {
            if (CaracteresDeReemplazo.ContainsKey(caracterAReemplazar))
            {
                return CaracteresDeReemplazo[caracterAReemplazar];
            }

            char[] ValoresPosibles = new char[31];

            for (int i = 0; i < 256; i++)
            {
                if (i > 0 && i < 32) { ValoresPosibles[i - 1] = (char)i; }
            }

            foreach (char caracter in ValoresPosibles)
            {
                bool agregar = true;

                foreach (char valor in CaracteresDeReemplazo.Values)
                {
                    if (valor.Equals(caracter))
                    {
                        agregar = false;
                    }
                }

                if (agregar)
                {
                    CaracteresDeReemplazo.Add(caracterAReemplazar, caracter);
                    break;
                }
            }

            return CaracteresDeReemplazo[caracterAReemplazar];
        }

        /// <summary>
        /// Formatea un registro en una cadena de texto.
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
        /// <param name="registro">Registro unificado.</param>
        /// <returns>Cadena de texto formateada.</returns>
        public virtual string Formatear(RegistroInLineTO registro)
        {
            if (FormatoSalida == null) FormatoSalida = FormatoPredeterminado;
            if (SeparadorSalida == 0) SeparadorSalida = SeparadorPredeterminado;
            if (!FormatoSalida.EndsWith(SeparadorSalida.ToString()) && !FormatoSalida.Equals(string.Empty)) FormatoSalida += SeparadorSalida;

            if(FormatoSalida == FormatoPredeterminado + SeparadorSalida) FormatoSalida = string.Format(FormatoPredeterminado, SeparadorSalida);

            FormatoSalidaTO expresionFormato = new FormatoSalidaTO(FormatoSalida, SeparadorSalida);
            
            if (ForzarTodosCampos)
            {
                if (!expresionFormato.TagFormato.ContainsKey("[ThreadGUID]")) FormatoSalida += "[ThreadGUID]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[MethodGUID]")) FormatoSalida += "[MethodGUID]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Namespace]")) FormatoSalida += "[Namespace]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Class]")) FormatoSalida += "[Class]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Method]")) FormatoSalida += "[Method]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Type]")) FormatoSalida += "[Type]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Level]")) FormatoSalida += "[Level]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[DateTime]")) FormatoSalida += "[DateTime]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[VarName]")) FormatoSalida += "[VarName]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[VarValue]")) FormatoSalida += "[VarValue]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[StackTrace]")) FormatoSalida += "[StackTrace]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Data]")) FormatoSalida += "[Data]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[ExceptionType]")) FormatoSalida += "[ExceptionType]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Message]")) FormatoSalida += "[Message]" + SeparadorSalida;
                if (!expresionFormato.TagFormato.ContainsKey("[Correlative]")) FormatoSalida += "[Correlative]" + SeparadorSalida;

                expresionFormato.Recargar(FormatoSalida, SeparadorSalida);
            }

            string fechaActual = DateTime.Now.ToString();
            string formatoFecha = "";

            if (expresionFormato.TagFormato.ContainsKey("[DateTime]"))
            {
                formatoFecha = expresionFormato.TagFormato["[DateTime]"];
                fechaActual = DateTime.Now.ToString(formatoFecha);
            }
            
            string metodo = registro.Metodo.Substring(registro.Metodo.IndexOf(" ") + 1);
            metodo = metodo.Substring(0, metodo.IndexOf("("));

            string formatoMetodo = "Full";

            if (expresionFormato.TagFormato.ContainsKey("[Method]"))
            {
                formatoMetodo = expresionFormato.TagFormato["[Method]"];
            }

            if (!string.IsNullOrEmpty(formatoMetodo))
            {
                if (formatoMetodo.Equals("Full", StringComparison.InvariantCultureIgnoreCase))
                {
                    metodo = registro.Metodo;
                }
            }

            if (!string.IsNullOrEmpty(registro.StackTrace))
            {
                registro.StackTrace = "\n\t\t<StackTrace>\n\t\t\t" + CambiarCaracteres(registro.StackTrace) + "\n\t\t</StackTrace>\n";
            }
            
            string salida = FormatoSalida
                .Replace("[ThreadGUID]", registro.ThreadGUID)
                .Replace("[MethodGUID]", registro.MethodGUID)
                .Replace("[Namespace]", registro.Namespace)
                .Replace("[Class]", CambiarCaracteres(registro.Clase))
                .Replace("[Method]", CambiarCaracteres(metodo))
                .Replace(string.Format("[Method:{0}]", formatoMetodo), CambiarCaracteres(metodo))
                .Replace("[Type]", registro.Tipo.Codigo)
                .Replace("[Level]", registro.Nivel != null ? registro.Nivel.Codigo : SeparadorSalida.ToString())
                .Replace("[DateTime:" + formatoFecha + "]", fechaActual)
                .Replace("[DateTime]", fechaActual)
                .Replace("[VarName]", registro.NombreVariable)
                .Replace("[VarValue]", CambiarCaracteres(registro.ValorVariable))
                .Replace("[StackTrace]", registro.StackTrace)
                .Replace("[Data]", CambiarCaracteres(registro.Data))
                .Replace("[ExceptionType]", registro.TipoExcepcion)
                .Replace("[Message]", CambiarCaracteres(registro.Mensaje))
                .Replace("[Correlative]", registro.Correlativo.ToString());

            return salida;
        }

        /// <summary>
        /// Cambia los caracteres prohibidos para la cómoda lectura posterior del log.
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
        /// <param name="registro">Regitro unificado.</param>
        /// <returns>Cadena de texto normalizada.</returns>
        protected virtual string CambiarCaracteres(string registro)
        {
            if (string.IsNullOrEmpty(registro)) return string.Empty;

            char reemplazoPuntoYComa = ObtenerCaracterReemplazo(SeparadorSalida);

            return registro
                .Replace(SeparadorSalida, reemplazoPuntoYComa)
                .Replace("\n", "\n\t\t\t");
        }

        /// <summary>
        /// Obtiene un valor que indica si se permite escribir un determinado tipo.
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
        /// <param name="tipo">Tipo a logear.</param>
        /// <returns><value>true</value> en caso de permitirse, o <value>false</value> de lo contrario.</returns>
        public virtual bool PermiteTipo(Tipo tipo)
        {
            TypeTag tipoTag = ObtenerTipo(tipo);

            if (tipoTag != null)
            {
                return tipoTag.Modo == Modo.On;
            }

            return false;
        }

        /// <summary>
        /// Obtiene el tipo de configuración para el writer.
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
        /// <param name="tipo">Tipo a logear.</param>
        /// <param name="nivel">Nivel del mensaje. Parámetro opcional.</param>
        /// <returns><value>true</value> en caso de permitirse, o <value>false</value> de lo contrario.</returns>
        public virtual TypeTag ObtenerTipo(Tipo tipo, Nivel nivel = null)
        {
            if(nivel != null)
            {
                if (Tipos.Count(c => c.Nombre == tipo && c.Nivel != null && c.Nivel.Codigo.Equals(nivel.Codigo, StringComparison.InvariantCultureIgnoreCase)) > 0)
                {
                    return Tipos.First(c => c.Nombre == tipo && c.Nivel.Codigo.Equals(nivel.Codigo, StringComparison.InvariantCultureIgnoreCase));
                }

                if (Tipos.Count(c => c.Nombre == tipo && c._Nivel == "*") > 0)
                {
                    return Tipos.First(c => c.Nombre == tipo && c._Nivel == "*");
                }
            }

            if (Tipos.Count(c => c.Nombre == tipo) > 0)
            {
                return Tipos.First(c => c.Nombre == tipo);
            }

            if (Tipos.Count(c => c._Nombre == "*") > 0)
            {
                return Tipos.First(c => c._Nombre == "*");
            }

            return null;
        }

        /// <summary>
        /// Obtiene un valor que indica si se permite escribir un determinado nivel de mensaje.
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
        /// <param name="nivel">Nivel de mensaje.</param>
        /// <returns><value>true</value> en caso de permitirse, o <value>false</value> de lo contrario.</returns>
        public virtual bool PermiteNivel(Nivel nivel)
        {
            TypeTag tipoTag = ObtenerTipo(Tipo.Mensaje, nivel);

            if (tipoTag != null)
            {
                if (tipoTag.Nivel != null)
                {
                    return tipoTag.Nivel.Codigo.Equals(nivel.Codigo, StringComparison.InvariantCultureIgnoreCase);
                }
                else if(tipoTag._Nivel == "*")
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Persiste el formato y el separador.
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
        public abstract void AgregarParametros();

        /// <summary>
        /// Inicializa la instancia del escritor.
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
        public abstract void Inicializar();

        /// <summary>
        /// Persiste el registro.
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
        /// <param name="registro">Registro unificado a ser escrito.</param>
        public void Guardar(RegistroInLineTO registro)
        {
            if (!inicializado)
            {
                Inicializar();
                inicializado = true;
            }

            if (!SaltarGuardadoFormato && !parametrosAgregados)
            {
                AgregarParametros();
                parametrosAgregados = true;
            }

            Agregar(registro);
        }

        /// <summary>
        /// Persiste el registro recibido.
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
        /// <param name="registro">Registro unificado a ser escrito.</param>
        protected abstract void Agregar(RegistroInLineTO registro);
    }
}
