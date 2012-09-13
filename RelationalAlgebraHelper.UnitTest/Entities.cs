using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelationalAlgebraHelper.UnitTest
{

    public class Entity {
        public int Key { get; set; }
    }

    public class Person : Entity {

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Address : Entity {
        public int PersonKey { get; set; }

        public int CityKey { get; set; }

        public string Street { get; set; }
    }

    public class City : Entity {
        public int StateKey { get; set; }

        public string Name { get; set; }
    }

    public class State : Entity {
        public int CountryKey { get; set; }

        public string Name { get; set; }
    }

    public class Country : Entity {
        public string Name { get; set; }
    }

}
