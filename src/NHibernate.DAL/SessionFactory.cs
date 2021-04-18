using System;
using System.Reflection;
using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.Engine;
using System.Collections;
using System.Collections.Generic;

namespace NHibernate.DAL
{
    //单例 进行Session的创建
    //在这两个类中用单件模式，来限制Session的创建。为了做到与具体的应用程序无关，
    //在这里把程序集的名称作为参数，传递给OpenSession()方法。可以把这两个类单独放在一个名为Common的工程下，
    //这里先把它们放在DAL层中。这两个类只是个人的一种写法，大家可以自行去编写。
    public class SessionFactory
    {
        public SessionFactory() { }

        //静态成员
        private static string _AssemblyName;
        private static Configuration cfg;
        private static ISessionFactory sessions;
        static readonly object padlock = new object();

        public static ISession OpenSession(string AssemblyName)
        {
            if (sessions == null)
            {
                lock (padlock)
                {
                    if (sessions == null)
                    {
                        _AssemblyName = AssemblyName;
                        BuildSessionFactory(AssemblyName);
                    }
                }
            }
            else//控制当AssemblyName发生变化，重新生成Entity
            {
                lock (padlock)
                {
                    if (sessions != null)
                    {
                        if (_AssemblyName != AssemblyName)
                        {
                            _AssemblyName = AssemblyName;
                            BuildSessionFactory(AssemblyName);
                        }
                    }
                }
            }

            return sessions.OpenSession();
        }

        private static void BuildSessionFactory(string AssemblyName)
        {
            cfg = CreateConfiguration();
            cfg.AddAssembly(AssemblyName);
            sessions = cfg.BuildSessionFactory();
        }

        private static Configuration CreateConfiguration()
        {
            Configuration Createcfg = new Configuration();
            String strHibernateCfgFileName = System.Configuration.ConfigurationManager.AppSettings["hibernate.cfg.xml"];
            //NHibernate配置 value为空时，从当前配置文件读取配置
            if (String.IsNullOrEmpty(strHibernateCfgFileName))
            {
                return Createcfg.Configure();
            }
            return Createcfg.Configure(strHibernateCfgFileName);
        }
    }

}
