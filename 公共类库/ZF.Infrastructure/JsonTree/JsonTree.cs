using System.Collections.Generic;

namespace ZF.Infrastructure.JsonTree
{
    public class JsonTree
    {
        private string _id;
        private string _text;
        private string _state = "open";
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();
        private object _children;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        public Dictionary<string, string> attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
        public object children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
