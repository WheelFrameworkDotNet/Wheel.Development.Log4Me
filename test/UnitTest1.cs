using ConsoleTest.DAO;
using ConsoleTest.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Wheel.Development.Log4Me;
using Wheel.Development.Log4Me.Entities;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        //[Log]
        [TestMethod]
        public void TestMethod1()
        {
            /*
            try
            {
                Log.Identificador("1-9");

                UsuarioTO to = new UsuarioTO()
                {
                    Nombre = "Juan Perez",
                    Edad = 22,
                    FechaNacimiento = DateTime.Now
                };

                Log.Variable("to", to);
                Log.Mensaje("Mensaje1", Nivel.Debug);
                Log.Mensaje("Mensaje2", Nivel.Alert);
                Log.Mensaje("Mensaje3", Nivel.Error);

                EjemploDAO dao = new EjemploDAO();
                dao.Agregar("Juan");
                dao.Modificar();
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            */
        }
    }
}
