using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelationalAlgebraHelper
{
    /// <summary>
    /// Se utiliza para contener un registro resultante de un Join
    /// </summary>
    /// <typeparam name="TOuter">The type of the outer.</typeparam>
    /// <typeparam name="TInner">The type of the inner.</typeparam>
    public class JoinResult<TOuter, TInner>
    {
        public JoinResult(TOuter outer, TInner inner)
        {
            this.Outer = outer;
            this.Inner = inner;
        }

        /// <summary>
        /// Gets or sets the inner entity.
        /// </summary>
        /// <value>
        /// The inner.
        /// </value>
        public TInner Inner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the outer entity.
        /// </summary>
        /// <value>
        /// The outer.
        /// </value>
        public TOuter Outer
        {
            get;
            set;
        }
    }
}
