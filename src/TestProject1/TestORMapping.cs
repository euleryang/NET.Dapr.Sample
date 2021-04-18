using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.DAL;
using NHibernate.Linq;
using ORMapping;

namespace Test.ORMapping
{
    [TestClass]
    public class TestORMapping
    {
        private const string AssemblyName = "ORMapping";
        public static EntityControl control = EntityControl.CreateEntityControl(AssemblyName);

        [TestMethod]
        public void TestORMappingAll()
        {
            using (var vSession = control.Session)
            {
                var v1 = (from p in vSession.Query<TCountry>() select p);
                var a = v1.ToList();

                Console.WriteLine(a.Count());
            }            
        }
    }
}
