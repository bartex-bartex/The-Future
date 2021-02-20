using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public class Object
    {
        public float xpos { get; }
        public float ypos { get; }
        public float width { get; }
        public float height { get; }

        Dictionary<ComponentType, Component> objects = new Dictionary<ComponentType, Component>();

        public Object(float xpos, float ypos, float width, float height, Component[] components)
        {
            this.xpos = xpos;
            this.ypos = ypos;
            this.width = width;
            this.height = height;
            foreach (Component c in components)
            {
                objects.Add(c.componentType, c);
            }
        }
    }
}
