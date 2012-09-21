Este proyecto contiene una libreria en C# para realizar queries usando lo mas parecido posible al algebra relacional.

````csharp
/*
* Ejemplos de uso:
*/
//Seleccion
IQueryable<Person> result = Do.Seleccion(p => p.FirstName == "Martinez", People);

//Proyeccion
IQueryable<string> result = Do.Proyeccion(p => p.FirstName + p.LastName, People);

// Union
IQueryable<Person> actual = group1.Union(group2);

// Join
IQueryable<JoinResult<Person, Address>> actual = People.Join(p => p.Key, addr => addr.PersonKey, Addresses);

// SemiJoin
IQueryable<Person> actual = People.SemiJoin(p => p.Key, addr => addr.PersonKey, Addresses);
		
// LeftJoin
IQueryable<JoinResult<Person, Address>> actual = People.LeftJoin(p => p.Key, addr => addr.PersonKey, Addresses);

````