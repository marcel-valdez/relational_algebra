using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using RelationalAlgebraHelper;

namespace RelationalAlgebraHelper.UnitTest
{
    [TestFixture]
    public class QueryTest : RelationalAlgebraTest
    {

        [TearDown]
        public void TearDown()
        {
            Clear();
        }

        [Test]
        public void TestIfItCanDoSeleccion()
        {
            // Arrange	  
                        
            var names = new string[] { "Perez", "Martinez", "Martinez", "Garcia" };
            Add<Person>(4, p => p.FirstName = names[p.Key - 1]);

            // Act
            var result = Do.Seleccion(p => p.FirstName == "Martinez", People);

            // Assert
            Assert.That(result, Has.All.With.Property("FirstName").EqualTo("Martinez"));

            // Reset
            
        }

        [Test]
        public void TestIfItCanDoProyeccion()
        {
            // Arrange	  
            var fnames = new string[] { "Perez", "Martinez", "Martinez", "Garcia" };
            var lnames = new string[] { "PerezL", "MartinezL", "MartinezL", "GarciaL" };
            var expected = new string[] { 
                fnames[0] + lnames[0], 
                fnames[1] + lnames[1], 
                fnames[2] + lnames[2], 
                fnames[3] + lnames[3]
            };

            Add<Person>(4, p =>
            {
                p.FirstName = fnames[p.Key - 1];
                p.LastName = lnames[p.Key - 1];
            });

            // Act
            IQueryable<string> result = Do.Proyeccion(p => p.FirstName + p.LastName, People);

            // Assert            
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result, Has.All.With.Matches<string>(fullname => expected.Contains(fullname)));

            // Reset
            
        }

        [Test]
        public void TestIfItCanDoUnion()
        {
            // Arrange	  
            var group1 = Add<Person>(4, p => p.FirstName = "Name" + p.Key);
            var group2 = Add<Person>(4, p => p.FirstName = "Name" + p.Key);

            // Act
            IQueryable<Person> actual = group1.Union(group2);

            // Assert
            CollectionAssert.AreEquivalent(People, actual);

            // Reset
            
        }

        [Test]
        public void TestIfItCanJoin()
        {
            // Arrange	  
            var people = Add<Person>(4, p => p.FirstName = "Name" + p.Key);
            var addresses = Add<Address>(1, With: addr => addr.PersonKey = 1);
            var addresses2 = Add<Address>(2, With: addr => addr.PersonKey = 2);
            var addresses3 = Add<Address>(3, With: addr => addr.PersonKey = 3);
            var addresses4 = Add<Address>(4, With: addr => addr.PersonKey = 4);

            // Act
            var actual = People.Join(p => p.Key, addr => addr.PersonKey, Addresses);

            // Assert
            Assert.That(actual.Count(), Is.EqualTo(10));
            Assert.That(actual.Count(p => p.Outer.Key == 1), Is.EqualTo(1));
            Assert.That(actual.Count(p => p.Outer.Key == 2), Is.EqualTo(2));
            Assert.That(actual.Count(p => p.Outer.Key == 3), Is.EqualTo(3));
            Assert.That(actual.Count(p => p.Outer.Key == 4), Is.EqualTo(4));
            Assert.That(
                actual,
                Has.All.With.Matches<JoinResult<Person, Address>>(
                    join => join.Inner.PersonKey.Equals(join.Outer.Key)));

            // Reset
            
        }


        [Test]
        public void TestIfItCanSemiJoin()
        {
            // Arrange	  
            var people = Add<Person>(4, p => p.FirstName = "Name" + p.Key);
            var addresses = Add<Address>(1, With: addr => addr.PersonKey = 1);
            var addresses2 = Add<Address>(2, With: addr => addr.PersonKey = 2);
            var addresses3 = Add<Address>(3, With: addr => addr.PersonKey = 3);
            var addresses4 = Add<Address>(4, With: addr => addr.PersonKey = 4);

            // Act
            IQueryable<Person> actual = People.SemiJoin(p => p.Key, addr => addr.PersonKey, Addresses);

            // Assert
            Assert.That(actual.Count(), Is.EqualTo(4));
            Assert.That(actual.Count(p => p.Key == 1), Is.EqualTo(1));
            Assert.That(actual.Count(p => p.Key == 2), Is.EqualTo(1));
            Assert.That(actual.Count(p => p.Key == 3), Is.EqualTo(1));
            Assert.That(actual.Count(p => p.Key == 4), Is.EqualTo(1));

            // Reset
            
        }

        [Test]
        public void TestIfItCanDoLeftJoin()
        {
            // Arrange	  
            var people = Add<Person>(4, p => p.FirstName = "Name" + p.Key);
            var addresses = Add<Address>(1, With: addr => addr.PersonKey = 1);
            var addresses2 = Add<Address>(2, With: addr => addr.PersonKey = 2);
            var addresses3 = Add<Address>(3, With: addr => addr.PersonKey = 3);

            // Act
            var actual = People.LeftJoin(p => p.Key, addr => addr.PersonKey, Addresses);

            // Assert
            Assert.That(actual.Count(), Is.EqualTo(7));
            Assert.That(
                actual,
                Has.All.With.Matches<JoinResult<Person, Address>>(
                    join => join.Inner == null && join.Outer != null ||
                            join.Outer.Key.Equals(join.Inner.PersonKey)));

            // Reset
            
        }
    }
}
