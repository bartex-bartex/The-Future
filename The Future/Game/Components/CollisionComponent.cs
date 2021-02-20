using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    struct AABB
    {
        float x, y, width, height;
        public bool IsCollision(AABB a, AABB b)
        {
            if (a.x < b.x + b.width &&
                a.x + a.width > b.x &&
                a.y < b.y + b.height &&
                a.y + a.height > b.y)
                return true;

            return false;
        }

        public void ResolveCollision(AABB a, AABB b)
        {

        }
    }

    class CollisionComponent : Component
    {
        AABB box;

        public CollisionComponent()
        {
            this.componentType = ComponentType.CollisionComponent;
        }

        public void Update(AABB player)
        {
            if(box.IsCollision(player, box))
            {
                box.ResolveCollision(player, box);
            }
        }
    }
}
