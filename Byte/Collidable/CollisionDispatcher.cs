namespace Byte.Collidable
{
    public class CollisionDispatcher
    {
        private readonly Dictionary<(string A, string B), CollisionHandler> handlers = new();

        public void Register(String typeA, String typeB, CollisionHandler handler)
        {
            handlers[(typeA, typeB)] = handler;
            handlers[(typeB, typeA)] = (a, b, side) => handler(b, a, Reverse(side));
        }
        public void Dispatch(ICollidable a, ICollidable b, CollisionSide side)
        {
            if (handlers.TryGetValue((a.Properties.Type, b.Properties.Type), out var handler))
            {
                handler(a, b, side);
            }
            else if (handlers.TryGetValue((b.Properties.Type, a.Properties.Type), out handler))
            {
                handler(b, a, Reverse(side));
            }
        }

        private static CollisionSide Reverse(CollisionSide side)
        {
            return side switch
            {
                CollisionSide.Top => CollisionSide.Bottom,
                CollisionSide.Bottom => CollisionSide.Top,
                CollisionSide.Left => CollisionSide.Right,
                CollisionSide.Right => CollisionSide.Left,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(side))
            };
        }
    }
}
