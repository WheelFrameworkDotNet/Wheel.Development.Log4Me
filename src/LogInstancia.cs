using System;
using System.Collections.Generic;
using System.Reflection;
using Wheel.Data.Json;
using Wheel.Development.Log4Me.Entities;
using Wheel.Development.Log4Me.Writers;

namespace Wheel.Development.Log4Me
{
    /// <summary>
    /// Contrato ha cumplir por un tipo de Logger de Log4Me.
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
    public sealed partial class Log
    {
        /// <summary>
        /// Contiene el identificador único global (GUID) del método actual.
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
        private string methodGUID;

        /// <summary>
        /// Permite obtener o establecer el número de llamadas que ha realizado el método actual.
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
        private int Llamada { get; set; }

        /// <summary>
        /// Permite obtener los escritores válidos para el método actual.
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
        private IList<ALogWriter> Writers { get; set; }

        /// <summary>
        /// Permite obtener el método actual mediante reflexión.
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
        private MethodBase Metodo { get; set; }

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
        private string _MethodGUID
        {
            get
            {
                if (methodGUID == null)
                {
                    methodGUID = Guid.NewGuid().ToString();
                }
                return methodGUID;
            }
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
        private void _Inicio(params object[] parametros)
        {
            Llamada++;

            RegistroInLineTO registro = new RegistroInLineTO()
            {
                ThreadGUID = ThreadGUID,
                MethodGUID = _MethodGUID,
                Namespace = Metodo.DeclaringType.Namespace,
                Clase = Metodo.DeclaringType.Name,
                Metodo = Metodo.ToString(),
                Correlativo = Llamada,
                Tipo = Tipo.Inicio
            };

            foreach (ALogWriter writer in Writers)
            {
                if (writer.PermiteTipo(Tipo.Inicio))
                {
                    writer.Guardar(registro);
                }
            }

            int numParametro = 0;

            if (parametros != null && parametros.Length > 0)
            {
                foreach (ParameterInfo param in Metodo.GetParameters())
                {
                    Llamada++;
                    string valor = string.Empty;

                    try
                    {
                        valor = parametros[numParametro].ToJson(true);
                    }
                    catch (Exception e)
                    {
                        valor = "Error al transformar a Json - " + e.Message.Replace("\n", "\t");
                    }

                    RegistroInLineTO parametro = new RegistroInLineTO()
                    {
                        ThreadGUID = ThreadGUID,
                        MethodGUID = _MethodGUID,
                        Namespace = Metodo.DeclaringType.Namespace,
                        Clase = Metodo.DeclaringType.Name,
                        Metodo = Metodo.ToString(),
                        NombreVariable = param.Name,
                        ValorVariable = valor,
                        Correlativo = Llamada,
                        Tipo = Tipo.Parametro
                    };

                    foreach (ALogWriter writer in Writers)
                    {
                        if (writer.PermiteTipo(Tipo.Parametro))
                        {
                            writer.Guardar(parametro);
                        }
                    }
                    numParametro++;
                }
            }
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
        private void _Retorno(object valor = null)
        {
            Llamada++;

            if (valor == null) valor = "<Void>";

            string valorJson = string.Empty;

            try
            {
                valorJson = valor.ToJson(true);
            }
            catch (Exception e)
            {
                valorJson = "Error al transformar a Json - " + e.Message;
            }

            RegistroInLineTO registro = new RegistroInLineTO()
            {
                ThreadGUID = ThreadGUID,
                MethodGUID = _MethodGUID,
                Namespace = Metodo.DeclaringType.Namespace,
                Clase = Metodo.DeclaringType.Name,
                Metodo = Metodo.ToString(),
                ValorVariable = valorJson,
                Correlativo = Llamada,
                Tipo = Tipo.Retorno
            };

            foreach (ALogWriter writer in Writers)
            {
                if (writer.PermiteTipo(Tipo.Retorno))
                {
                    writer.Guardar(registro);
                }
            }
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
        private void _Excepcion(Exception excepcion, bool permiteContinuar)
        {
            Llamada++;

            string valorJson = string.Empty;

            try
            {
                valorJson = excepcion.Data.ToJson(true);
            }
            catch (Exception ex)
            {
                valorJson = "Error al transformar a Json - " + ex.Message;
            }

            RegistroInLineTO registro = new RegistroInLineTO()
            {
                ThreadGUID = ThreadGUID,
                MethodGUID = _MethodGUID,
                Namespace = Metodo.DeclaringType.Namespace,
                Clase = Metodo.DeclaringType.Name,
                Metodo = Metodo.ToString(),
                StackTrace = excepcion.StackTrace,
                Data = valorJson,
                TipoExcepcion = excepcion.GetType().FullName,
                Mensaje = excepcion.Message,
                Correlativo = Llamada,
                Tipo = Tipo.Excepcion
            };

            foreach (ALogWriter writer in Writers)
            {
                if (writer.PermiteTipo(Tipo.Excepcion))
                {
                    writer.Guardar(registro);
                }
            }

            if (!permiteContinuar)
            {
                Retorno("<" + registro.Tipo + ">");
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
        private void _Variable(string nombre, object valor)
        {
            Llamada++;

            string valorJson = string.Empty;

            try
            {
                valorJson = valor.ToJson(true);
            }
            catch (Exception e)
            {
                valorJson = "Error al transformar a Json - " + e.Message;
            }

            RegistroInLineTO registro = new RegistroInLineTO()
            {
                ThreadGUID = ThreadGUID,
                MethodGUID = _MethodGUID,
                Namespace = Metodo.DeclaringType.Namespace,
                Clase = Metodo.DeclaringType.Name,
                Metodo = Metodo.ToString(),
                NombreVariable = nombre,
                ValorVariable = valorJson,
                Correlativo = Llamada,
                Tipo = Tipo.Variable
            };

            foreach (ALogWriter writer in Writers)
            {
                if (writer.PermiteTipo(Tipo.Variable))
                {
                    writer.Guardar(registro);
                }
            }
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
        private void _Mensaje(string mensaje, Nivel nivel)
        {
            Llamada++;

            RegistroInLineTO registro = new RegistroInLineTO()
            {
                ThreadGUID = ThreadGUID,
                MethodGUID = _MethodGUID,
                Namespace = Metodo.DeclaringType.Namespace,
                Clase = Metodo.DeclaringType.Name,
                Metodo = Metodo.ToString(),
                Mensaje = mensaje,
                Nivel = nivel,
                Correlativo = Llamada,
                Tipo = Tipo.Mensaje
            };

            foreach (ALogWriter writer in Writers)
            {
                if (writer.PermiteNivel(nivel))
                {
                    writer.Guardar(registro);
                }
            }
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
        private E _CargarPuntero<E>(E excepcion) where E : Exception
        {
            excepcion.Data.Add("Log4Me_MethodGUID", _MethodGUID);
            return excepcion;
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
        private string _ObtenerPuntero(Exception excepcion)
        {
            if (excepcion != null && excepcion.Data.Contains("Log4Me_MethodGUID"))
            {
                return excepcion.Data["Log4Me_MethodGUID"] != null ? excepcion.Data["Log4Me_MethodGUID"].ToString() : null;
            }

            return null;
        }
    }
}