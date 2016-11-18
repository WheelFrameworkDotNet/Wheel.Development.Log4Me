using System;
using System.Collections.Generic;

namespace Wheel.Development.Log4Me.Entities.Mapper
{
    /// <summary>
    /// Mapeador de entidades del Log.
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
    public class LogEntityMapper
    {
        /// <summary>
        /// Convierte una colección de registros (fieles al txt) mediante un filtro, a una representación de cada parte del flujo.
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
        /// <param name="registros">Colección de registros en línea(fieles al txt).</param>
        /// <param name="filtro">Filtro para excluir elementos. <value>true</value> para incluir, <value>false</value> para excluir.</param>
        /// <returns>Representación completa del flujo</returns>
        public MetodoTO Convertir(IList<RegistroInLineTO> registros, Func<RegistroInLineTO, bool> filtro = null)
        {
            MetodoTO retorno = new MetodoTO();
            int indiceInicio = 0;
            retorno = CargarArbol(retorno, ref indiceInicio, registros, filtro);
            return retorno;
        }

        /// <summary>
        /// Carga una colección de registros (fieles al txt) mediante un filtro, a una representación de cada parte del flujo.
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
        /// <param name="retorno">Representación completa del flujo, para cargar los datos leídos.</param>
        /// <param name="indiceInicio">Fila inicial a leer. Luego se continúa hasta el final.</param>
        /// <param name="registros">Registros fieles el archivo plano (Origen de la información).</param>
        /// <param name="filtro">Filtro para excluir elementos. <value>true</value> para incluir, <value>false</value> para excluir.</param>
        /// <returns>Representación completa del flujo</returns>
        private MetodoTO CargarArbol(MetodoTO retorno, ref int indiceInicio, IList<RegistroInLineTO> registros, Func<RegistroInLineTO, bool> filtro = null)
        {
            for (int indice = indiceInicio; indice < registros.Count; indice++)
            {
                RegistroInLineTO registro = registros[indice];

                if (filtro == null || (filtro != null && filtro(registro)))
                {
                    if (registro.Tipo == Tipo.Inicio)
                    {
                        if (retorno.Inicio != null)
                        {
                            retorno.Llamadas.Add(CargarArbol(new MetodoTO(), ref indiceInicio, registros));
                        }
                        else
                        {
                            retorno.Inicio = new InicioTO()
                            {
                                Namespace = registro.Namespace,
                                Clase = registro.Clase,
                                Llamada = registro.Correlativo,
                                Fecha = registro.Fecha,
                                MethodGUID = registro.MethodGUID,
                                Metodo = registro.Metodo,
                                ThreadGUID = registro.ThreadGUID
                            };

                            retorno.Namespace = retorno.Inicio.Namespace;
                            retorno.Clase = retorno.Inicio.Clase;
                            retorno.Llamada = retorno.Inicio.Llamada;
                            retorno.Fecha = retorno.Inicio.Fecha;
                            retorno.MethodGUID = retorno.Inicio.MethodGUID;
                            retorno.ThreadGUID = retorno.Inicio.ThreadGUID;
                            retorno.Metodo = retorno.Inicio.Metodo;
                        }
                    }
                    else if (registro.Tipo == Tipo.Variable)
                    {
                        retorno.Variables.Add(new VariableTO()
                        {
                            Namespace = retorno.Inicio.Namespace,
                            Clase = retorno.Inicio.Clase,
                            Llamada = retorno.Inicio.Llamada,
                            Fecha = registro.Fecha,
                            MethodGUID = retorno.Inicio.MethodGUID,
                            ThreadGUID = retorno.Inicio.ThreadGUID,
                            Nombre = registro.NombreVariable,
                            Valor = registro.ValorVariable
                        });
                    }
                    else if (registro.Tipo == Tipo.Retorno)
                    {
                        retorno.Retorno = new RetornoTO()
                        {
                            Namespace = retorno.Inicio.Namespace,
                            Clase = retorno.Inicio.Clase,
                            Llamada = retorno.Inicio.Llamada,
                            Fecha = registro.Fecha,
                            MethodGUID = retorno.Inicio.MethodGUID,
                            ThreadGUID = retorno.Inicio.ThreadGUID,
                            Valor = registro.ValorVariable
                        };

                        return retorno;
                    }
                    else if (registro.Tipo == Tipo.Excepcion)
                    {
                        retorno.Excepciones.Add(new ExcepcionTO()
                        {
                            Namespace = retorno.Inicio.Namespace,
                            Clase = retorno.Inicio.Clase,
                            Llamada = retorno.Inicio.Llamada,
                            Fecha = registro.Fecha,
                            MethodGUID = retorno.Inicio.MethodGUID,
                            ThreadGUID = retorno.Inicio.ThreadGUID,
                            Data = registro.Data,
                            Mensaje = registro.Mensaje,
                            StackTrace = registro.StackTrace,
                            Tipo = registro.NombreVariable,
                        });
                    }
                    else if (registro.Tipo == Tipo.Mensaje)
                    {
                        retorno.Mensajes.Add(new MensajeTO()
                        {
                            Namespace = retorno.Inicio.Namespace,
                            Clase = retorno.Inicio.Clase,
                            Llamada = retorno.Inicio.Llamada,
                            Fecha = registro.Fecha,
                            Nivel = registro.Nivel,
                            MethodGUID = retorno.Inicio.MethodGUID,
                            ThreadGUID = retorno.Inicio.ThreadGUID,
                            Mensaje = registro.Mensaje
                        });
                    }
                    else if (registro.Tipo == Tipo.Parametro)
                    {
                        retorno.Parametros.Add(new ParametroTO()
                        {
                            Namespace = retorno.Inicio.Namespace,
                            Clase = retorno.Inicio.Clase,
                            Llamada = retorno.Inicio.Llamada,
                            Fecha = registro.Fecha,
                            MethodGUID = retorno.Inicio.MethodGUID,
                            ThreadGUID = retorno.Inicio.ThreadGUID,
                            Nombre = registro.NombreVariable,
                            Valor = registro.ValorVariable
                        });
                    }
                }
            }

            return null;
        }
    }
}
