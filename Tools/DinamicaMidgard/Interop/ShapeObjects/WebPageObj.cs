using System;
using System.Collections.Generic;
using System.Text;

namespace Midgard.Interop.ShapeObjects
{
    public class WebPageObj
    {
        private string title, entity, operation;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string[] fields;
        private List<TransitionObj> transitions;

        public List<TransitionObj> Transitions
        {
            get { return transitions; }
            set { transitions = value; }
        }

        public string[] Fields
        {
            get { return fields; }
            set { fields = value; }
        }

        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public string Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        public WebPageObj()
        {
            transitions = new List<TransitionObj>();
        }

        public WebPageObj(string title): this()
        {
            this.title = title;
        }

    }
}
