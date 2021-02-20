using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{

    public enum ComponentType
    {
        RenderComponent,
        CollisionComponent
    }

    public class Component
    {
        public ComponentType componentType { get; set; }
    }
}
