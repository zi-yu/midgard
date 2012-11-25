using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Generic
{
    public class RelationDependency
    {
        #region Fields

        private string name;
        private bool isReusable = false;
        private List<RelationDependency> dependencies;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Return the list of entities that this depends
        /// </summary>
        public List<RelationDependency> Dependencies
        {
            get { return dependencies; }
        }

        /// <summary>
        /// Entity name
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// If the Name of the entity already exists in the tree return true
        /// </summary>
        public bool IsReusable
        {
            get { return isReusable; }
            set { isReusable = value; }
        }

        #endregion Properties

        public RelationDependency(string _name)
        {
            name = _name;
            dependencies = new List<RelationDependency>();
        }
    }
}
