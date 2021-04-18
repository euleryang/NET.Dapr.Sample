using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace NHibernate.DAL
{
    //单例 实体的操作
    public class EntityControl
    {
        private string _AssemblyName;

        //静态成员
        static readonly object padlock = new object();
        private static EntityControl entity;

        public virtual ISession Session
        {
            get
            {
                return SessionFactory.OpenSession(_AssemblyName);
            }
        }

        /// <summary>
        /// 实体的操作
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <returns></returns>
        public static EntityControl CreateEntityControl(string AssemblyName)
        {
            if (entity == null)
            {
                lock (padlock)
                {
                    if (entity == null)
                    {
                        entity = new EntityControl();
                        entity._AssemblyName = AssemblyName;
                    }
                }
            }
            else//控制当AssemblyName发生变化，重新生成Entity
            {
                lock (padlock)
                {
                    if (entity != null)
                    {
                        if (entity._AssemblyName != AssemblyName)
                        {
                            entity = new EntityControl();
                            entity._AssemblyName = AssemblyName;
                        }
                    }
                }
            }

            return entity;
        }

        public void AddEntity(Object entity)
        {
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();
            try
            {
                v_session.Save(entity);
                v_session.Flush();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                v_session.Close();
            }
        }

        public void SaveOrUpdateEntity(Object entity)
        {
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();
            try
            {
                v_session.SaveOrUpdate(entity);
                v_session.Flush();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                v_session.Close();
            }
        }

        public void UpdateEntity(Object entity)
        {
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();
            try
            {
                v_session.Update(entity);
                v_session.Flush();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                v_session.Close();
            }
        }

        public void UpdateEntity(Object entity, Object key)
        {
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();
            try
            {
                v_session.Update(entity, key);
                v_session.Flush();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                v_session.Close();
            }
        }

        public void DeleteEntity(Object entity)
        {
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();

            try
            {
                v_session.Delete(entity);
                v_session.Flush();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                v_session.Close();
            }
        }

        public void AddEntity(Object entity, ISession a_session)
        {
            a_session.Save(entity);
        }

        public void SaveOrUpdateEntity(Object entity, ISession a_session)
        {
            a_session.SaveOrUpdate(entity);
        }

        public void DeleteEntity(Object entity, ISession a_session)
        {
            a_session.Delete(entity);
        }

        public void UpdateEntity(Object entity, ISession a_session)
        {
            a_session.Update(entity);
        }

        public void UpdateEntity(Object entity, Object key, ISession a_session)
        {
            a_session.Update(entity, key);
        }

        public void UpdateEntities(IQuery iquery)
        {
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();
            try
            {
                iquery.ExecuteUpdate();
                v_session.Flush();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                v_session.Close();
            }
        }

        /// <summary>
        /// Returns the number of objects deleted.
        /// </summary>
        /// <param name="strHQL"></param>
        /// <returns></returns>
        public int DelEntities(string strHQL)
        {
            int re;
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();

            //执行HQL是通过IQuery来实现的，如下是详细的写法：
            re = v_session.Delete(strHQL);

            transaction.Commit();
            v_session.Flush();
            v_session.Close();

            return re;
        }

        /// <summary>
        /// Returns the number of objects deleted.
        /// </summary>
        /// <param name="strHQL"></param>
        /// <returns></returns>
        public int DelEntities(string strHQL, ISession a_session)
        {
            int re;
            //执行HQL是通过IQuery来实现的，如下是详细的写法：
            re = a_session.Delete(strHQL);

            return re;
        }

        public IList GetEntities(string strHQL)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();
            
            //执行HQL是通过IQuery来实现的，如下是详细的写法：
            IQuery query = v_session.CreateQuery(strHQL);
            IList lst = query.List();

            transaction.Commit();
            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList<T> GetEntities<T>(string strHQL)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);
            ITransaction transaction = v_session.BeginTransaction();

            //执行HQL是通过IQuery来实现的，如下是详细的写法：
            IQuery query = v_session.CreateQuery(strHQL);
            IList<T> lst = query.List<T>();

            transaction.Commit();
            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList GetCriteriaEntities(System.Type t)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);

            ICriteria criteria = v_session.CreateCriteria(t);
            IList lst = criteria.List();

            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList<T> GetCriteriaEntities<T>()
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);

            ICriteria criteria = v_session.CreateCriteria(typeof(T));
            IList<T> lst = criteria.List<T>();

            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList GetCriteriaEntities(System.Type t, ICriterion[] e)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);

            ICriteria criteria = v_session.CreateCriteria(t);
            for (int i = 0; i <= e.Length - 1; i++ )
            {
                criteria.Add(e[i]);
            }

            IList lst = criteria.List();

            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList<T> GetCriteriaEntities<T>(ICriterion[] e)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);

            ICriteria criteria = v_session.CreateCriteria(typeof(T));
            for (int i = 0; i <= e.Length - 1; i++)
            {
                criteria.Add(e[i]);
            }

            IList<T> lst = criteria.List<T>();

            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList GetCriteriaEntities(System.Type t, ICriterion e)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);

            ICriteria criteria = v_session.CreateCriteria(t);
            criteria.Add(e);

            IList lst = criteria.List();

            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList<T> GetCriteriaEntities<T>(ICriterion e)
        {
            //创建新的Session
            ISession v_session = SessionFactory.OpenSession(_AssemblyName);

            ICriteria criteria = v_session.CreateCriteria(typeof(T));
            criteria.Add(e);

            IList<T> lst = criteria.List<T>();

            v_session.Flush();
            v_session.Close();

            return lst;
        }

        public IList GetEntities(string strHQL, ISession a_session)
        {
            //执行HQL是通过IQuery来实现的，如下是详细的写法：
            IQuery query = a_session.CreateQuery(strHQL);
            IList lst = query.List();

            return lst;
        }

        public IList<T> GetEntities<T>(string strHQL, ISession a_session)
        {
            //执行HQL是通过IQuery来实现的，如下是详细的写法：
            IQuery query = a_session.CreateQuery(strHQL);
            IList<T> lst = query.List<T>();

            return lst;
        }

        public IList GetCriteriaEntities(System.Type t, ISession a_session)
        {
            ICriteria criteria = a_session.CreateCriteria(t);
            IList lst = criteria.List();

            return lst;
        }

        public IList<T> GetCriteriaEntities<T>(ISession a_session)
        {
            ICriteria criteria = a_session.CreateCriteria(typeof(T));
            IList<T> lst = criteria.List<T>();

            return lst;
        }

        public IList GetCriteriaEntities(System.Type t, ICriterion[] e, ISession a_session)
        {
            ICriteria criteria = a_session.CreateCriteria(t);
            for (int i = 0; i <= e.Length - 1; i++)
            {
                criteria.Add(e[i]);
            }

            IList lst = criteria.List();

            return lst;
        }

        public IList<T> GetCriteriaEntities<T>(ICriterion[] e, ISession a_session)
        {
            ICriteria criteria = a_session.CreateCriteria(typeof(T));
            for (int i = 0; i <= e.Length - 1; i++)
            {
                criteria.Add(e[i]);
            }

            IList<T> lst = criteria.List<T>();

            return lst;
        }

        public IList GetCriteriaEntities(System.Type t, ICriterion e, ISession a_session)
        {
            ICriteria criteria = a_session.CreateCriteria(t);
            criteria.Add(e);

            IList lst = criteria.List();

            return lst;
        }

        public IList<T> GetCriteriaEntities<T>(ICriterion e, ISession a_session)
        {
            ICriteria criteria = a_session.CreateCriteria(typeof(T));
            criteria.Add(e);

            IList<T> lst = criteria.List<T>();

            return lst;
        }
    }
}
