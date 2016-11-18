using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Wheel.Development.Log4Me.Config.Entities;
using Wheel.Development.Log4Me.Writers;

namespace Wheel.Development.Log4Me
{
    /// <summary>
    /// Negocio de Log4Me.
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
    internal static class Log4MeBO
    {
        /// <summary>
        /// Contiene el valor comodín que indica "Cualquier valor" (*).
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
        private static string CUALQUIER_VALOR = "*";

        /// <summary>
        /// Obtiene los escritores en los que el método proporcionado cumple con las condiciones señaladas.
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
        /// <param name="metodo">Método a validar.</param>
        /// <returns>Lista de escritores.</returns>
        public static IList<ALogWriter> ObtenerWriters(MethodBase metodo)
        {
            Log.Inicializar();
            
            IList<ALogWriter> retorno = new List<ALogWriter>();

            if (Log.Configuration != null)
            {
                foreach (ALogWriter writer in Log.Configuration.Writers)
                {
                    if (writer.Modo.Codigo.Equals(Modo.On.Codigo, StringComparison.InvariantCultureIgnoreCase))
                    {
                        /* 
                            Se establece un número que establece la razón de la exclusión:
                                -   0   =   No se excluye.
                                -   1   =   Ensamblado.
                                -   2   =   NameSpace.
                                -   3   =   Clase.
                                -   4   =   Método.
                        */
                        int prioridadExcluir = 0;

                        // Para excluir un registro, este debe cumplir con todas las condiciones del tag (operación &&).
                        foreach (ExcludeTag excluido in writer.Excluidos)
                        {
                            if (excluido.Metodo != null)
                            {
                                if (excluido.Metodo.isInternal && metodo.ReflectedType.IsNestedAssembly) prioridadExcluir = 4;
                                if (excluido.Metodo.isPrivate && metodo.ReflectedType.IsNestedPrivate) prioridadExcluir = 4;
                                if (excluido.Metodo.isPublic && metodo.ReflectedType.IsNestedPublic) prioridadExcluir = 4;
                                if (excluido.Metodo.isStatic && metodo.IsStatic) prioridadExcluir = 4;

                                if (CumpleCondicionSimple(excluido.Metodo, metodo.Name, metodo.ToString())) prioridadExcluir = 4;

                                if (prioridadExcluir == 0) continue;
                            }

                            if (excluido.Clase != null)
                            {
                                if (excluido.Clase.isInternal && metodo.DeclaringType.IsNestedAssembly) prioridadExcluir = 3;
                                if (excluido.Clase.isPrivate && metodo.DeclaringType.IsNestedPrivate) prioridadExcluir = 3;
                                if (excluido.Clase.isPublic && metodo.DeclaringType.IsNestedPublic) prioridadExcluir = 3;

                                if (CumpleCondicionSimple(excluido.Clase, metodo.DeclaringType.Name, metodo.DeclaringType.FullName)) prioridadExcluir = 3;

                                if (prioridadExcluir == 0) continue;
                            }

                            if (excluido.NameSpace != null)
                            {
                                if (CumpleCondicionSimple(excluido.NameSpace, metodo.DeclaringType.Namespace, metodo.DeclaringType.Namespace)) prioridadExcluir = 2;

                                if (prioridadExcluir == 0) continue;
                            }

                            if (excluido.Assembly != null)
                            {
                                Assembly ensamblado = metodo.DeclaringType.Assembly;
                                object[] atributosEnsamblado = ensamblado.GetCustomAttributes(true);

                                if (excluido.Assembly.Title != null)
                                {
                                    var atributo = atributosEnsamblado.OfType<AssemblyTitleAttribute>().FirstOrDefault();
                                    if (atributo != null && CumpleCondicionSimple(excluido.Assembly.Title, atributo.Title, ensamblado.GetName().FullName)) prioridadExcluir = 1;

                                    if (prioridadExcluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(excluido.Assembly.Configuration))
                                {
                                    var atributo = atributosEnsamblado.OfType<AssemblyConfigurationAttribute>().FirstOrDefault();
                                    if (atributo != null && excluido.Assembly.Configuration.Equals(atributo.Configuration, StringComparison.InvariantCultureIgnoreCase)) prioridadExcluir = 1;

                                    if (prioridadExcluir == 0) continue;
                                }

                                if (excluido.Assembly.Company != null)
                                {
                                    var atributo = atributosEnsamblado.OfType<AssemblyCompanyAttribute>().FirstOrDefault();
                                    if (atributo != null && CumpleCondicionSimple(excluido.Assembly.Company, atributo.Company, atributo.Company)) prioridadExcluir = 1;

                                    if (prioridadExcluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(excluido.Assembly.GUID))
                                {
                                    var atributo = atributosEnsamblado.OfType<GuidAttribute>().FirstOrDefault();
                                    if (atributo != null && excluido.Assembly.GUID.Equals(atributo.Value, StringComparison.InvariantCultureIgnoreCase)) prioridadExcluir = 1;

                                    if (prioridadExcluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(excluido.Assembly.MinimalVersion) || !string.IsNullOrEmpty(excluido.Assembly.MaxiumVersion))
                                {
                                    string minimalVersion = !string.IsNullOrEmpty(excluido.Assembly.MinimalVersion) ? excluido.Assembly.MinimalVersion : "*";
                                    string maxiumVersion = !string.IsNullOrEmpty(excluido.Assembly.MaxiumVersion) ? excluido.Assembly.MaxiumVersion : "*";

                                    var atributo = atributosEnsamblado.OfType<AssemblyVersionAttribute>().FirstOrDefault();

                                    if (AssemblyTag.CompararVersiones(atributo.Version, minimalVersion) == -1 &&
                                        AssemblyTag.CompararVersiones(atributo.Version, maxiumVersion) > -1)
                                        prioridadExcluir = 1;

                                    if (prioridadExcluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(excluido.Assembly.MinimalFileVersion) || !string.IsNullOrEmpty(excluido.Assembly.MaxiumFileVersion))
                                {
                                    string minimalFileVersion = !string.IsNullOrEmpty(excluido.Assembly.MinimalFileVersion) ? excluido.Assembly.MinimalFileVersion : "*";
                                    string maxiumFileVersion = !string.IsNullOrEmpty(excluido.Assembly.MaxiumFileVersion) ? excluido.Assembly.MaxiumFileVersion : "*";

                                    var atributo = atributosEnsamblado.OfType<AssemblyFileVersionAttribute>().FirstOrDefault();

                                    if (AssemblyTag.CompararVersiones(atributo.Version, minimalFileVersion) == -1 &&
                                        AssemblyTag.CompararVersiones(atributo.Version, maxiumFileVersion) > -1)
                                        prioridadExcluir = 1;
                                }
                            }

                            if (prioridadExcluir > 0) break;
                        }

                        /* 
                            Se establece un número que establece la razón de la inclusión:
                                -   0   =   No se incluye.
                                -   1   =   Ensamblado.
                                -   2   =   NameSpace.
                                -   3   =   Clase.
                                -   4   =   Método.
                        */
                        int prioridadIncluir = 0;

                        foreach (IncludeTag incluido in writer.Incluidos)
                        {
                            if (incluido.Metodo != null)
                            {
                                if (incluido.Metodo.isInternal && metodo.ReflectedType.IsNestedAssembly) prioridadIncluir = 4;
                                if (incluido.Metodo.isPrivate && metodo.ReflectedType.IsNestedPrivate) prioridadIncluir = 4;
                                if (incluido.Metodo.isPublic && metodo.ReflectedType.IsNestedPublic) prioridadIncluir = 4;
                                if (incluido.Metodo.isStatic && metodo.IsStatic) prioridadIncluir = 4;

                                if (CumpleCondicionSimple(incluido.Metodo, metodo.Name, metodo.ToString())) prioridadIncluir = 4;

                                if (prioridadIncluir == 0) continue;
                            }

                            if (incluido.Clase != null)
                            {
                                if (incluido.Clase.isInternal && metodo.DeclaringType.IsNestedAssembly) prioridadIncluir = 3;
                                if (incluido.Clase.isPrivate && metodo.DeclaringType.IsNestedPrivate) prioridadIncluir = 3;
                                if (incluido.Clase.isPublic && metodo.DeclaringType.IsNestedPublic) prioridadIncluir = 3;

                                if (CumpleCondicionSimple(incluido.Clase, metodo.DeclaringType.Name, metodo.DeclaringType.FullName)) prioridadIncluir = 3;

                                if (prioridadIncluir == 0) continue;
                            }

                            if (incluido.NameSpace != null)
                            {
                                if (CumpleCondicionSimple(incluido.NameSpace, metodo.DeclaringType.Namespace, metodo.DeclaringType.Namespace)) prioridadIncluir = 2;

                                if (prioridadIncluir == 0) continue;
                            }

                            if (incluido.Assembly != null)
                            {
                                Assembly ensamblado = metodo.DeclaringType.Assembly;
                                object[] atributosEnsamblado = ensamblado.GetCustomAttributes(true);

                                if (incluido.Assembly.Title != null)
                                {
                                    var atributo = atributosEnsamblado.OfType<AssemblyTitleAttribute>().FirstOrDefault();
                                    if (atributo != null && CumpleCondicionSimple(incluido.Assembly.Title, atributo.Title, ensamblado.GetName().FullName)) prioridadIncluir = 1;

                                    if (prioridadIncluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(incluido.Assembly.Configuration))
                                {
                                    var atributo = atributosEnsamblado.OfType<AssemblyConfigurationAttribute>().FirstOrDefault();
                                    if (atributo != null && incluido.Assembly.Configuration.Equals(atributo.Configuration, StringComparison.InvariantCultureIgnoreCase)) prioridadIncluir = 1;

                                    if (prioridadIncluir == 0) continue;
                                }

                                if (incluido.Assembly.Company != null)
                                {
                                    var atributo = atributosEnsamblado.OfType<AssemblyCompanyAttribute>().FirstOrDefault();
                                    if (atributo != null && CumpleCondicionSimple(incluido.Assembly.Company, atributo.Company, atributo.Company)) prioridadIncluir = 1;

                                    if (prioridadIncluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(incluido.Assembly.GUID))
                                {
                                    var atributo = atributosEnsamblado.OfType<GuidAttribute>().FirstOrDefault();
                                    if (atributo != null && incluido.Assembly.GUID.Equals(atributo.Value, StringComparison.InvariantCultureIgnoreCase)) prioridadIncluir = 1;

                                    if (prioridadIncluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(incluido.Assembly.MinimalVersion) || !string.IsNullOrEmpty(incluido.Assembly.MaxiumVersion))
                                {
                                    string minimalVersion = !string.IsNullOrEmpty(incluido.Assembly.MinimalVersion) ? incluido.Assembly.MinimalVersion : "*";
                                    string maxiumVersion = !string.IsNullOrEmpty(incluido.Assembly.MaxiumVersion) ? incluido.Assembly.MaxiumVersion : "*";

                                    var atributo = atributosEnsamblado.OfType<AssemblyVersionAttribute>().FirstOrDefault();

                                    if (AssemblyTag.CompararVersiones(atributo.Version, minimalVersion) == -1 &&
                                        AssemblyTag.CompararVersiones(atributo.Version, maxiumVersion) > -1)
                                        prioridadIncluir = 1;

                                    if (prioridadIncluir == 0) continue;
                                }

                                if (!string.IsNullOrEmpty(incluido.Assembly.MinimalFileVersion) || !string.IsNullOrEmpty(incluido.Assembly.MaxiumFileVersion))
                                {
                                    string minimalFileVersion = !string.IsNullOrEmpty(incluido.Assembly.MinimalFileVersion) ? incluido.Assembly.MinimalFileVersion : "*";
                                    string maxiumFileVersion = !string.IsNullOrEmpty(incluido.Assembly.MaxiumFileVersion) ? incluido.Assembly.MaxiumFileVersion : "*";

                                    var atributo = atributosEnsamblado.OfType<AssemblyFileVersionAttribute>().FirstOrDefault();

                                    if (AssemblyTag.CompararVersiones(atributo.Version, minimalFileVersion) == -1 &&
                                        AssemblyTag.CompararVersiones(atributo.Version, maxiumFileVersion) > -1)
                                        prioridadIncluir = 1;
                                }
                            }
                        }

                        if ((prioridadIncluir > 0 && prioridadExcluir == 0) || (prioridadIncluir < prioridadExcluir))
                        {
                            retorno.Add(writer);
                        }
                    }
                }
            }
            
            return retorno;
        }

        /// <summary>
        /// Verifica si un valor y su versión completa, cumple con una condición simple.
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
        /// <param name="condicion">Condición simple.</param>
        /// <param name="valor">Valor a comprobar</param>
        /// <param name="valorFull">Valor completo a comprobar.</param>
        /// <returns></returns>
        private static bool CumpleCondicionSimple(CondicionSimple condicion, string valor, string valorFull)
        {
            if (condicion.Equals != null && condicion.Equals.Trim() != CUALQUIER_VALOR)
            {
                if (condicion.IgnoreCase)
                {
                    if (!valor.ToLower().Equals(condicion.Equals.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!valor.Equals(condicion.Equals))
                    {
                        return false;
                    }
                }
            }

            if (condicion.NotEquals != null)
            {
                if (condicion.IgnoreCase)
                {
                    if (valor.ToLower().Equals(condicion.NotEquals.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (valor.Equals(condicion.NotEquals))
                    {
                        return false;
                    }
                }
            }

            if (condicion.Contains != null && condicion.Contains.Trim() != CUALQUIER_VALOR)
            {
                if (condicion.IgnoreCase)
                {
                    if (!valor.ToLower().Contains(condicion.Contains.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!valor.Contains(condicion.Contains))
                    {
                        return false;
                    }
                }
            }

            if (condicion.NotContains != null)
            {
                if (condicion.IgnoreCase)
                {
                    if (valor.ToLower().Contains(condicion.NotContains.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (valor.Contains(condicion.NotContains))
                    {
                        return false;
                    }
                }
            }

            if (condicion.EndWith != null && condicion.EndWith.Trim() != CUALQUIER_VALOR)
            {
                if (condicion.IgnoreCase)
                {
                    if (!valor.ToLower().EndsWith(condicion.EndWith.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!valor.EndsWith(condicion.EndWith))
                    {
                        return false;
                    }
                }
            }

            if (condicion.NotEndWith != null)
            {
                if (condicion.IgnoreCase)
                {
                    if (valor.ToLower().EndsWith(condicion.NotEndWith.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (valor.EndsWith(condicion.NotEndWith))
                    {
                        return false;
                    }
                }
            }

            if (condicion.FullEquals != null && condicion.FullEquals.Trim() != CUALQUIER_VALOR)
            {
                if (condicion.IgnoreCase)
                {
                    if (!valorFull.ToLower().Equals(condicion.FullEquals.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!valorFull.Equals(condicion.FullEquals))
                    {
                        return false;
                    }
                }
            }

            if (condicion.NotFullEquals != null)
            {
                if (condicion.IgnoreCase)
                {
                    if (valor.ToLower().Equals(condicion.NotFullEquals.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (valor.Equals(condicion.NotFullEquals))
                    {
                        return false;
                    }
                }
            }

            if (condicion.StartWith != null && condicion.StartWith.Trim() != CUALQUIER_VALOR)
            {
                if (condicion.IgnoreCase)
                {
                    if (!valor.ToLower().StartsWith(condicion.StartWith.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!valor.StartsWith(condicion.StartWith))
                    {
                        return false;
                    }
                }
            }

            if (condicion.NotStartWith != null)
            {
                if (condicion.IgnoreCase)
                {
                    if (valor.ToLower().StartsWith(condicion.NotStartWith.ToLower()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (valor.StartsWith(condicion.NotStartWith))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
