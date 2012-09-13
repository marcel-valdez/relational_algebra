Este proyecto contiene una librería en C# para realizar queries usando lo más parecido posible al álgebra relacional.

````csharp
/*
* Ejemplos de uso:
*/
//Seleccion
IQueryable<string> result = Do.Seleccion(p => p.FirstName == "Martinez", People);

//Proyeccion
IQueryable<string> result = Do.Proyeccion(p => p.FirstName + p.LastName, People);

// Union
IQueryable<Person> actual = group1.Union(group2);

// Join
JoinResult<Person, Address> actual = People.Join(p => p.Key, addr => addr.PersonKey, Addresses);


// SemiJoin
IQueryable<Person> actual = People.SemiJoin(p => p.Key, addr => addr.PersonKey, Addresses);
		
// LeftJoin
JoinResult<Person, Address> actual = People.LeftJoin(p => p.Key, addr => addr.PersonKey, Addresses);

````