using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelationalAlgebraHelper.UnitTest
{
    public class RelationalAlgebraTest
    {
        private static readonly List<Country> countries = new List<Country>();

        private static readonly List<State> states = new List<State>();

        private static readonly List<City> cities = new List<City>();

        private static readonly List<Person> people = new List<Person>();

        private static readonly List<Address> addresses = new List<Address>();

        public static IQueryable<Country> Countries
        {
            get
            {
                return countries.AsQueryable();
            }
        }

        public static IQueryable<State> States
        {
            get
            {
                return states.AsQueryable();
            }
        }

        public static IQueryable<City> Cities
        {
            get
            {
                return cities.AsQueryable();
            }
        }

        public static IQueryable<Person> People
        {
            get
            {
                return people.AsQueryable();
            }
        }

        public static IQueryable<Address> Addresses
        {
            get
            {
                return addresses.AsQueryable();
            }
        }


        public T Add<T>(Action<T> With)
            where T : Entity, new()
        {
            return Add<T>(1, With).Single();
        }

        public IQueryable<T> Add<T>(int count, Action<T> With = null)
            where T : Entity, new()
        {
            With = With ?? (o =>
            {
            });


            List<T> added = new List<T>();

            for(int i = 0; i < count; i++)
            {
                T obj = new T()
                {
                    Key = i + 1
                };

                With(obj);

                GetTable<T>().Add(obj);
                added.Add(obj);
            }

            return added.AsQueryable();
        }

        public void Clear()
        {
            people.Clear();
            addresses.Clear();
            cities.Clear();
            states.Clear();
            countries.Clear();
        }


        private List<T> GetTable<T>()
            where T : class
        {
            if(typeof(T).Equals(typeof(Person)))
            {
                return people as List<T>;
            }

            if (typeof(T).Equals(typeof(Address)))
            {
                return addresses as List<T>;
            }

            if(typeof(T).Equals(typeof(City)))
            {
                return cities as List<T>;
            }

            if(typeof(T).Equals(typeof(State)))
            {
                return states as List<T>;
            }

            if(typeof(T).Equals(typeof(Country)))
            {
                return countries as List<T>;
            }

            throw new Exception(string.Format("The type {0} has no table.", typeof(T).Name));
        }
    }
}
