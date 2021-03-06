﻿

using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog.Models;

namespace SimpleBlog
{
    public class Database
    {
        private const string SessionKey = "SimpleBlog.Database.SessionKey";

        private static ISessionFactory _sessionFactory;

        public static ISession Session => (ISession) HttpContext.Current.Items[SessionKey];
        

        public static void Configure()
        {
            var config = new Configuration();

            //configure the connection string

            config.Configure();

            // add our mapping
            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>();

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());


            // create session factory
            _sessionFactory = config.BuildSessionFactory();

        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory;
        }

        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
                session.Close();

            HttpContext.Current.Items.Remove(SessionKey);
        }
    }
}