using PostSharp.Aspects;
using System;
using System.Reflection;
using System.Threading;
using Wheel.Development.Log4Me.Entities;
using Wheel.Development.Log4Me.Writers;
using Wheel.Extensions.Threading;

namespace Wheel.Development.Log4Me
{
    /// <summary>
    /// Anotacion utilizada para interceptar la ejecución de un método, registrando los datos de entrada, salida y excepción del método.
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
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    [Serializable]
    public sealed partial class Log : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Contiene la instancia actual de Log correspondiente a la clase que lo invoca.
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
        private static Log Instancia
        {
            get
            {
                return Thread.CurrentThread.Obtener("Log4Me_CurrentInstance") as Log;
            }

            set
            {
                Thread.CurrentThread.Guardar("Log4Me_CurrentInstance", value);
            }
        }

        /// <summary>
        /// Obtiene el identificador único (GUID) del hilo actual.
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
        public static string ThreadGUID
        {
            get
            {
                string threadGUID = Thread.CurrentThread.Obtener("Log4Me_ThreadGUID") as string;

                if (threadGUID == null)
                {
                    threadGUID = Guid.NewGuid().ToString();
                    Thread.CurrentThread.Guardar("Log4Me_ThreadGUID", threadGUID);
                }

                return threadGUID;
            }
        }
        
        /// <summary>
        /// Permite obtener el identificador único global (GUID) del método actual.
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
        public static string MethodGUID
        {
            get
            {
                if (Instancia != null)
                {
                    return Instancia._MethodGUID;
                }
                return null;
            }
        }

        #region Tiempo de Diseño

            /// <summary>
            /// Comrpueba si el método interceptado cumple las condiciones.
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
            /// <param name="method">Metodo interceptado accedido mediante reflexión.</param>
            /// <returns><value>true</value> en caso de ser válido, o <value>false</value> de lo contrario.</returns>
            public override bool CompileTimeValidate(MethodBase method)
            {
                return true;
            }

        #endregion Tiempo de Diseño

        #region Incertepción

            /// <summary>
            /// Es ejecutado al llamar a un método interceptado.
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
            /// <param name="args">Argumento con información de la llamada.</param>
            public override void OnEntry(MethodExecutionArgs args)
            {
                Metodo = args.Method;
                Writers = Log4MeBO.ObtenerWriters(Metodo);
                Instancia = this;
                _Inicio(args.Arguments.ToArray());
            }

            /// <summary>
            /// Es ejecutado al salir del método interceptado.
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
            /// <param name="args">Argumento con información de la llamada.</param>
            public override void OnExit(MethodExecutionArgs args)
            {
                _Retorno(args.ReturnValue);
            }

            /// <summary>
            /// Es ejecutado al lanzar una excepción desde el método interceptado.
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
            /// <param name="args">Argumento con información de la llamada.</param>
            public override void OnException(MethodExecutionArgs args)
            {
                _Excepcion(args.Exception, false);
            }

        #endregion Incertepción

        /// <summary>
        /// Logea el identifiador de ejecución del hilo actual. Es recomendable utilizar el RUN del usuario u otro valor que identifique la sesión actual.
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
        /// <param name="identificador">Identificador del hilo.</param>
        public static void Identificador(string identificador)
        {
            RegistroInLineTO registro = new RegistroInLineTO()
            {
                ThreadGUID = ThreadGUID,
                MethodGUID = Instancia != null ? Instancia._MethodGUID : null,
                Namespace = Instancia != null ? Instancia.Metodo.DeclaringType.Namespace : null,
                Clase = Instancia != null ? Instancia.Metodo.DeclaringType.FullName : null,
                Metodo = Instancia != null ? Instancia.Metodo.ToString() : null,
                Correlativo = 0,
                Tipo = Tipo.Identificador,
                ValorVariable = identificador
            };

            if (Configuration != null)
            {
                foreach (ALogWriter writer in Configuration.Writers)
                {
                    writer.Guardar(registro);
                }
            }
        }

        /// <summary>
        /// Registra el nombre, tipo y valor de una variable en el método actual.
        /// Requiere que el inicio y el retorno del método esté logeado.
        /// </summary>
        /// <example>
        ///     public int Sumar(int valor1, int valor2, int valor3)
        ///     {
        ///         log.Inicio(valor1, valor2);
        ///         int resultado = valor1 + valor2;
        ///         
        ///         log.Variable("resultado", resultado);
        ///         retultado += valor3;
        ///         
        ///         log.Retorno(resultado);
        ///         return resultado;
        ///     }
        /// </example>
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
        /// <param name="nombre">Nombre de la variable.</param>
        /// <param name="valor">Valor de la variable.</param>
        public static void Variable(string nombre, object valor)
        {
            Instancia._Variable(nombre, valor);
        }

        /// <summary>
        /// Registra un mensaje con un nivel de importancia en el flujo del método actual.
        /// Requiere que el inicio y el retorno del método esté logeado.
        /// </summary>
        /// <example>
        ///     public class UsuarioBO
        ///     {
        ///         private UsuarioDAO dao = new UsuarioDAO();
        ///         
        ///         public void Agregar(UsuarioTO usuario)
        ///         {
        ///             log.Mensaje("Antes de llamar al DAO", Nivel.INFO);
        ///             dao.Agregar(usuario);
        ///             log.Mensaje("Luego de llamar al DAO", Nivel.INFO);
        ///         }
        ///     }
        /// </example>
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
        /// <param name="mensaje">Mensaje a registrar.</param>
        /// <param name="nivel">Nivel de importancia o tipo del mensaje.</param>
        public static void Mensaje(string mensaje, Nivel nivel)
        {
            Instancia._Mensaje(mensaje, nivel);
        }

        /// <summary>
        /// Registra el inicio de un método, guarda la hora actual y cada parametro especificando
        /// nombre, tipo y valor. Se recomienda que esta sea la primera instrucción de cada método.
        /// Si se logea el inicio, es obligatorio logear el retorno aunque el método sea Void.
        /// </summary>
        /// <example>
        ///     public void Metodo1(int a, string b, bool c)
        ///     {
        ///         log.Inicio(a, b, c);
        ///         log.Retorno();
        ///     }
        ///     
        ///     public void Metodo2()
        ///     {
        ///         log.Inicio();
        ///         log.Retorno();
        ///     }
        /// </example>
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
        /// <param name="parametros">Lista de parametros del método actual.</param>
        public static void Inicio(params object[] parametros)
        {
            Instancia._Inicio(parametros);
        }

        /// <summary>
        /// Registra el fin de la ejecución de un método, guardando la fecha de termino, el valor de retorno y su tipo.
        /// Se debe logear antes de retornar. Requiere que se haya logeado el inicio del método.
        /// </summary>
        /// <example>
        ///     public int Sumar(int valor1, int valor2)
        ///     {
        ///         log.Inicio(valor1, valor2);
        ///         int resultado = valor1 + valor2;
        ///         log.Retorno(resultado);
        ///         return resultado;
        ///     }
        /// </example>
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
        /// <param name="valor">Valor de retorno del método.</param>
        public static void Retorno(object valor = null)
        {
            Instancia._Retorno(valor);
        }

        /// <summary>
        /// Registra la información relevante de una excepción lanzada por un método. Se debe logear antes de ser lanzada.
        /// Requiere que el inicio y el retorno del método esté logeado.
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
        /// <param name="excepcion">Excepción lanzada.</param>
        /// <param name="permiteContinuar">Valor que indica si la excepción interrumpe el flujo (no permite continuar).
        /// En caso de ser falso, se logea automáticamente el retorno del método como Excepción.</param>
        public static void Excepcion(Exception excepcion, bool permiteContinuar)
        {
            Instancia._Excepcion(excepcion, permiteContinuar);
        }

        /// <summary>
        /// Carga el puntero del log (MethodGUID) en una excepción.
        /// Es útil para guardar el puntero exactamente donde ocurrió la excepción.
        /// </summary>
        /// <example>
        ///     public class UsuarioDAO
        ///     {
        ///         public void Agregar(UsuarioTO usuario)
        ///         {
        ///             try
        ///             {
        ///                 //Llamada a base de datos.
        ///             }
        ///             catch(Exception e)
        ///             {
        ///                 throw log.CargarPuntero(e);
        ///             }
        ///         }
        ///     }
        ///     
        ///     public class UsuarioBO
        ///     {
        ///         private UsuarioDAO dao = new UsuarioDAO();
        ///         
        ///         public void Agregar(UsuarioTO usuario)
        ///         {
        ///             try
        ///             {
        ///                 dao.Agregar(usuario);
        ///             }catch(Exception e)
        ///             {
        ///                 string MethodGUID = log.ObtenerPuntero(e);
        ///             }
        ///         }
        ///     }
        /// </example>
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
        /// <typeparam name="E">Tipo de la excepción que se lanzó.</typeparam>
        /// <param name="excepcion">Excepción que se lanzó.</param>
        /// <returns>Excepción cargada.</returns>
        public static E CargarPuntero<E>(E excepcion) where E : Exception
        {
            return Instancia._CargarPuntero(excepcion);
        }

        /// <summary>
        /// Obtiene el puntero (MethodGUID) de una excepción con el puntero cargado mediante el método <see cref="Wheel.Development.Log4Me.Log.CargarPuntero"/>.
        /// </summary>
        /// <example>
        ///     public class UsuarioBO
        ///     {
        ///         private UsuarioDAO dao = new UsuarioDAO();
        ///         
        ///         public void Agregar(UsuarioTO usuario)
        ///         {
        ///             try
        ///             {
        ///                 dao.Agregar(usuario);
        ///             }catch(Exception e)
        ///             {
        ///                 string MethodGUID = log.ObtenerPuntero(e);
        ///             }
        ///         }
        ///     }
        ///     
        ///     public class UsuarioDAO
        ///     {
        ///         public void Agregar(UsuarioTO usuario)
        ///         {
        ///             try
        ///             {
        ///                 //Llamada a base de datos.
        ///             }
        ///             catch(Exception e)
        ///             {
        ///                 log.CargarPuntero(e);
        ///                 throw e;
        ///             }
        ///         }
        ///     }
        /// </example>
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
        /// <param name="excepcion">Excepción que contiene el puntero.</param>
        /// <returns>MethodGUID.</returns>
        public static string ObtenerPuntero(Exception excepcion)
        {
            return Instancia._ObtenerPuntero(excepcion);
        }
    }
}
