using System;

namespace $namespace {
	
    /// <summary>
    /// $name exception
    /// </summary>
	public class $name : $parent {

        #region Constructors

		/// <summary>
        /// $name constructor
        /// </summary>
        /// <param name="reason">Exception message</param>
        public $name( string reason ) 
			: base(reason)
        {
        }
		
		/// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reason">The exception message</param>
        /// <param name="ex">The original exception</param>
        public $name(string reason, Exception ex)
            : base(reason, ex)
        {
        }

		/// <summary>
        /// $name constructor
        /// </summary>
        /// <param name="reason">Exception message</param>
        /// <param name="args">Parameters to format the message</param>
        public $name( string reason, params object[] args) 
			: base(string.Format(reason, args))
        {
        }
		
		/// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reason">The exception message</param>
        /// <param name="ex">The original exception</param>
        /// <param name="args">Parameters to format the message</param>
        public $name(string reason, Exception ex, params object[] args )
            : base(string.Format(reason, args), ex)
        {
        }

        #endregion Constructors

    };

}