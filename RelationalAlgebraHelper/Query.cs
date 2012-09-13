namespace RelationalAlgebraHelper
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;


    /// <summary>
    /// Esta clase contiene las funciones de algebra relacional
    /// </summary>
    public static class Do
    {
        /// <summary>
        /// Es utilizada para realizar la operación de selección.
        /// </summary>
        /// <typeparam name="T">El tipo de la entidad de la tabla/conjunto sobre la que se realiza la selección.</typeparam>
        /// <param name="criteria">El criterio lógico de selección.</param>
        /// <param name="rows">El conjunto de entidades a seleccionar.</param>
        /// <returns>Lass entidades seleccionadas.</returns>
        public static IQueryable<T> Seleccion<T>(
            Expression<Func<T, bool>> criteria,
            IQueryable<T> rows)
        {
            return rows.Where(criteria).Distinct();
        }

        /// <summary>
        /// Realiza la operación de unión sobre dos conjuntos
        /// </summary>
        /// <typeparam name="T">Tipo de dato contenido en el conjunto.</typeparam>
        /// <param name="outer">El conjunto del lado izquierdo de la unión.</param>
        /// <param name="inner">El conjunto del lado derecho de la unión.</param>
        /// <returns>La unión de ambos conjuntos</returns>
        public static IQueryable<T> Union<T>(
            this IQueryable<T> outer,
            IQueryable<T> inner)
        {
            return Queryable.Union(outer, inner);
        }

        /// <summary>
        /// Realiza la operación de proyección sobre un conjunto de entidades
        /// </summary>
        /// <typeparam name="TSource">El tipo de entidad del conjunto de entidades a proyectar.</typeparam>
        /// <typeparam name="TResult">El tipo resultante de la proyección.</typeparam>
        /// <param name="func">El criterio de proyección.</param>
        /// <param name="rows">El conjunto de entidades a proyectar.</param>
        /// <returns>Un conjunto de entidades proyectadas.</returns>
        public static IQueryable<TResult> Proyeccion<TSource, TResult>(
            Func<TSource, TResult> func,
            IQueryable<TSource> rows)
        {
            return rows.Select(func).Distinct().AsQueryable();
        }

        /// <summary>
        /// Realiza la operación de Join sobre el conjunto <paramref name="outer"/> y el conjunto <paramref name="inner"/>
        /// </summary>
        /// <typeparam name="TOuter">El tipo de entidades del conjunto externo.</typeparam>
        /// <typeparam name="TInner">El tipo de entidades del conjunto interno.</typeparam>
        /// <typeparam name="TKey">El tipo de la llave seleccionada para el Join.</typeparam>
        /// <param name="outer">El conjunto de entidades del lado izquierdo del join.</param>
        /// <param name="outerKey">La expresión de selección de llave del conjunto externo.</param>
        /// <param name="innerKey">La expresión de selección de llave del conjunto interno.</param>
        /// <param name="inner">El conjunto interno de entidades para el Join.</param>
        /// <returns>El resultado del Join entre los conjuntos.</returns>
        public static IQueryable<JoinResult<TOuter, TInner>> Join<TOuter, TInner, TKey>(
            this IQueryable<TOuter> outer,
            Expression<Func<TOuter, TKey>> outerKey,
            Expression<Func<TInner, TKey>> innerKey,
            IQueryable<TInner> inner)
        {
            return outer.Join(inner, outerKey, innerKey, (outt, inn) => new JoinResult<TOuter, TInner>(outt, inn)).AsQueryable();
        }

        /// <summary>
        /// Realiza la operación de SemiJoin sobre 2 conjuntos de entidades
        /// </summary>
        /// <typeparam name="TOuter">El tipo de entidades dentro del conjunto externo.</typeparam>
        /// <typeparam name="TInner">El tipo de entidades dentro del conjunto interno.</typeparam>
        /// <typeparam name="TKey">El tipo de dato a utilizar como llave de comparación en el Join.</typeparam>
        /// <param name="outer">El conjunto externo de entidades.</param>
        /// <param name="outerKey">La expresión de selección de llave de la entidad externa.</param>
        /// <param name="innerKey">La expresión de selección de llave de la entidad interna.</param>
        /// <param name="inner">El conjunto interno de entidades.</param>
        /// <returns>El subconjunto de entidades del conjunto externo que cumplieron con el criterio de semijoin</returns>
        public static IQueryable<TOuter> SemiJoin<TOuter, TInner, TKey>(
            this IQueryable<TOuter> outer,
            Func<TOuter, TKey> outerKey,
            Func<TInner, TKey> innerKey,
            IQueryable<TInner> inner)
        {
            return outer.Join(inner, outerKey, innerKey, (outt, inn) => outt).Distinct().AsQueryable();
        }

        /// <summary>
        /// Realiza la operación de LeftJoin sobre dos conjuntos de entidades.
        /// </summary>
        /// <typeparam name="TOuter">El tipo de entidades dentro del conjunto externo.</typeparam>
        /// <typeparam name="TInner">El tipo de entidades dentro del conjunto interno.</typeparam>
        /// <typeparam name="TKey">El tipo de dato a utilizar como llave de comparación en el LeftJoin.</typeparam>
        /// <param name="outer">El conjunto externo de entidades.</param>
        /// <param name="outerKey">La expresión de selección de llave de la entidad externa.</param>
        /// <param name="innerKey">La expresión de selección de llave de la entidad interna.</param>
        /// <param name="inner">El conjunto interno de entidades.</param>
        /// <returns>
        /// El subconjunto de entidades del conjunto externo e interno 
        /// que cumplieron con el criterio de leftjoin
        /// </returns>
        public static IQueryable<JoinResult<TOuter, TInner>> LeftJoin<TOuter, TInner, TKey>(
            this IQueryable<TOuter> outer,
            Expression<Func<TOuter, TKey>> outerKey,
            Expression<Func<TInner, TKey>> innerKey,
            IQueryable<TInner> inner)
        {
            Func<TOuter, TKey> outerKeyFunc = outerKey.Compile();
            Func<TInner, TKey> innerKeyFunc = innerKey.Compile();
            //inner
            var normalJoin = outer.Join(outerKey, innerKey, inner);
            var leftJoin = outer.Except(normalJoin.Select(item => item.Outer)).Select(outt => new JoinResult<TOuter, TInner>(outt, default(TInner)));

            return normalJoin.Union(leftJoin).Distinct();
        }

    }
}
