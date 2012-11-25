#region Licence Statment
// Copyright (c) Zi-Yu.com - All Rights Reserved
// http://midgard.zi-yu.com/
//
// The use and distribution terms for this software are covered by the
// LGPL (http://opensource.org/licenses/lgpl-license.php).
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion

using Loki.Generic;
using System.Collections.Generic;

namespace Loki.Interfaces {

    public interface IPlugin {

        /// <summary>
        /// Gets the Plugin company.
        /// </summary>
        /// <value>The company.</value>
        string Company { get;}

        /// <summary>
        /// Gets the Plugin name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get;}

        /// <summary>
        /// Plugin Description.
        /// </summary>
        /// <value>The Description.</value>
        string Description { get;}

        /// <summary>
        /// Gets the default parameters.
        /// </summary>
        /// <value>The default parameters.</value>
        Dictionary<string,string> DefaultParameters { get; }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        void SetParameters( Dictionary<string, string> parameters );

        /// <summary>
        /// Gets the plugin dependencies
        /// </summary>
        /// <returns>The dependency list</returns>
        List<IPlugin> GetDependencies();

    }

}

