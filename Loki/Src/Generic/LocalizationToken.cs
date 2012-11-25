using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Generic {

    /// <summary>
    /// Represents a localization token
    /// </summary>
    public class LocalizationToken {

        #region Instance Fields

        private string category = null;
        private string key = null;
        private string sample = null;

        #endregion Instance Fields

        #region Instance Properties

        /// <summary>
        /// Gets the token's category
        /// </summary>
        public string Category {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// Gets the token's key
        /// </summary>
        public string Key {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// Gets a token's sample 
        /// </summary>
        public string Sample {
            get { return sample; }
            set { sample = value; }
        }

        #endregion Instance Properties

        #region Ctor

        /// <summary>
        /// Creates a new Localization Token
        /// </summary>
        /// <param name="c">The category</param>
        /// <param name="k">The Key</param>
        /// <param name="s">A sample</param>
        public LocalizationToken(string c, string k, string s)
        {
            Category = c;
            Key = k;
            Sample = s;
        }

        #endregion Ctor

    };
}
