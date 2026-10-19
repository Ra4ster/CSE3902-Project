using System.Runtime.CompilerServices;
using Byte.Block;
using Byte.Collidable;
using Byte.Controller;
using Byte.Factory;
using Byte.Item;
using Byte.Rooms;
using Byte.Player;
using Byte.Sprite;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

int checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
}

// Only the keyboard snapshot is read by Link.Update; no native input or GPU is needed.
Link CreateLink(Vector2 position) => new Link(position, new FakeSprite(), null!, null!, null!,
    (KeyboardController)RuntimeHelpers.GetUninitializedObject(typeof(KeyboardController)));

void Collide(ICollidable a, ICollidable b)
{
    var dispatcher = new CollisionDispatcher();
    LinkCollisionHandler.Register(dispatcher);
    foreach (var collision in new CollisionDetector().DetectCollisions(new[] { a, b }))
        dispatcher.Dispatch(collision.A, collision.B, collision.Side);
}

GameTime Step(double seconds) => new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(seconds));

// Enter each face of a solid tile, in either order in the collision list.
var tile = new SquareBlock(new FakeSprite(), new Vector2(200, 200));
foreach (Vector2 position in new[] {
    new Vector2(160, 300), new Vector2(336, 300),
    new Vector2(248, 190), new Vector2(248, 366) })
{
    foreach (bool reversed in new[] { false, true })
    {
        Link link = CreateLink(position);
        Check(link.BoundingBox.Intersects(tile.BoundingBox), "Fixture must overlap tile");
        if (reversed) Collide(tile, link); else Collide(link, tile);
        Check(!link.BoundingBox.Intersects(tile.BoundingBox), "Solid tile must block each face in either order");
    }
}

foreach (AbstractBlock floor in new AbstractBlock[] {
    new SpeckledFloorBlock(new FakeSprite(), new Vector2(200, 200)),
    new SandBlock(new FakeSprite(), new Vector2(200, 200)),
    new LadderBlock(new FakeSprite(), new Vector2(200, 200)),
    new StairsBlock(new FakeSprite(), new Vector2(200, 200)) })
{
    Link link = CreateLink(new Vector2(248, 278));
    Vector2 before = link.Position;
    Collide(link, floor);
    Check(link.Position == before, "Non-barrier tile must remain walkable");
}
foreach (AbstractBlock barrier in new AbstractBlock[] {
    new BrickBlock(new FakeSprite(), Vector2.Zero), new WaterBlock(new FakeSprite(), Vector2.Zero),
    new VoidBlock(new FakeSprite(), Vector2.Zero), new KeystoneBlock(new FakeSprite(), Vector2.Zero),
    new StatueLeftBlock(new FakeSprite(), Vector2.Zero), new StatueRightBlock(new FakeSprite(), Vector2.Zero) })
    Check(barrier.Properties.BlocksPlayer, "Barrier classification");

Link player = CreateLink(new Vector2(220, 220));
var enemy = new TestEnemy(new Vector2(180, 180), 2f);
Check(enemy.BoundingBox.Size == new Point(32, 32), "Enemy bounds must use actual scale");
Collide(player, enemy);
Check(player.Health == 2 && player.isDamaged, "Enemy contact must hurt Link");
for (int frame = 0; frame < 30; frame++) Collide(enemy, player);
Check(player.Health == 2, "Contact must not drain health every frame");
player.Update(Step(0.25));
Check(player.currentSprite!.Color == Color.Red, "Damage tint must survive recreated idle sprites");
player.Update(Step(0.26));
Check(!player.isDamaged && player.currentSprite!.Color == Color.White, "Invulnerability must expire");
Collide(player, enemy);
Check(player.Health == 1, "Contact after cooldown must hurt again");
player.Update(Step(0.51)); player.TakeDamage();
player.Update(Step(0.51)); player.TakeDamage();
Check(player.Health == 0, "Health must not become negative");

// The same enemy must keep triggering visible feedback, including at zero HP.
Link repeatContact = CreateLink(new Vector2(220, 220));
for (int hit = 1; hit <= 6; hit++)
{
    Collide(repeatContact, enemy);
    Check(repeatContact.HitsTaken == hit && repeatContact.isDamaged,
        "Same enemy must keep producing damage feedback after health reaches zero");
    Collide(repeatContact, enemy);
    Check(repeatContact.HitsTaken == hit, "Cooldown must still prevent repeated damage events");
    repeatContact.Update(Step(0.15));
    Check(repeatContact.currentSprite!.Color == Color.White, "Damage feedback must visibly blink");
    repeatContact.Update(Step(0.1));
    Check(repeatContact.currentSprite!.Color == Color.Red, "Damage feedback must blink back to red");
    repeatContact.Update(Step(0.26));
}
Check(repeatContact.Health == 0, "Repeated test hits must never make health negative");
Check(CreateLink(Vector2.Zero).HitsTaken == 0, "Rebuilding Link must reset the hit counter");

Link fighter = CreateLink(new Vector2(400, 400));
foreach (var direction in new[] { CardinalDirections.North, CardinalDirections.South, CardinalDirections.East, CardinalDirections.West })
{
    fighter.Direction = direction;
    fighter.IsAttacking = false;
    fighter.Attack();
    fighter.state.Update(fighter, Step(0));
    fighter.Sword.Update(fighter);
    Check(fighter.Sword.BoundingBox.IsEmpty, "Wind-up must not damage enemies");
    var animation = (MovingAnimatedSprite)fighter.currentSprite!;
    animation.currentFrame = 1;
    fighter.Sword.Update(fighter);
    Rectangle sword = fighter.Sword.BoundingBox;
    Rectangle body = fighter.BoundingBox;
    Check(!sword.IsEmpty && !sword.Intersects(body), "Sword must extend outside body");
    Check(direction == CardinalDirections.North ? sword.Bottom == body.Top :
          direction == CardinalDirections.South ? sword.Top == body.Bottom :
          direction == CardinalDirections.East ? sword.Left == body.Right : sword.Right == body.Left,
          "Sword must face the attack direction");
    var target = new TestEnemy(new Vector2(sword.X, sword.Y), 1f);
    Collide(target, fighter.Sword);
    Collide(fighter.Sword, target);
    Check(target.Health == 2 && target.IsDamaged, "One attack must hurt each enemy once, regardless of order");
    target.UpdateDamage(Step(0.31));
    Check(!target.IsDamaged, "Enemy damage flash must end");
    fighter.IsAttacking = false;
    fighter.GetNewState(new PlayerIdleState());
    fighter.Attack();
    fighter.state.Update(fighter, Step(0));
    ((MovingAnimatedSprite)fighter.currentSprite!).currentFrame = 1;
    fighter.Sword.Update(fighter);
    fighter.Sword.Hit(target);
    Check(target.Health == 1, "A new attack must be able to hit the same enemy");
    fighter.state = new PlayerUseItemState();
    fighter.Sword.Update(fighter);
    Check(fighter.Sword.BoundingBox.IsEmpty, "Item use must not count as a sword attack");
}

Link edge = CreateLink(new Vector2(-100, -100));
LinkCollisionHandler.ClampToRoom(edge, new Rectangle(0, 0, 1600, 1600));
Check(edge.BoundingBox.Left == 0 && edge.BoundingBox.Top == 0, "Clamp top and left");
edge.Position = new Vector2(2000, 2000);
LinkCollisionHandler.ClampToRoom(edge, new Rectangle(0, 0, 1600, 1600));
Check(edge.BoundingBox.Right == 1600 && edge.BoundingBox.Bottom == 1600, "Clamp right and bottom");

var door = new TestDoor(edge.BoundingBox);
door.Initialize();
Collide(door, edge);
Check(door.Visitor == edge, "Door callback must receive Link even in reverse order");
var defeated = new TestEnemy(Vector2.Zero, 1f);
for (int hit = 0; hit < 4; hit++) defeated.TakeDamage();
Check(defeated.IsDefeated && defeated.Health == 0, "Enemy health must stop at zero");

// Pick-ups vanish on contact.
var rupee = new TestItem(new Vector2(200, 200), new Point(40, 80));
Link collector = CreateLink(new Vector2(200, 200));
Check(rupee.BoundingBox == new Rectangle(200, 200, 40, 80), "Item hitbox must match its scaled frame size");
Collide(rupee, collector);
Check(rupee.IsCollected, "Touching an item must collect it even in reverse order");
Collide(collector, rupee);
Check(rupee.IsCollected, "Collecting an item twice must be harmless");

// A door covers one grid tile and fires its callback when Link touches it.
var westDoor = new Door(new DoorData { Position = new GridPosition(0, 2), Direction = "West", TargetRoom = "Hall" });
Check(westDoor.BoundingBox == new Rectangle(0, 192, 96, 96), "Door hitbox must cover its grid tile");
ICollidable? visitor = null;
westDoor.Properties.OnCollide = who => visitor = who;
Link walker = CreateLink(new Vector2(48 + 90, 78 + 192)); // body left edge 6px into the door tile
Collide(walker, westDoor);
Check(visitor == walker, "Walking into a door must report Link to the door callback");
Link bystander = CreateLink(new Vector2(48 + 96, 78 + 192)); // body exactly on the next tile over
Check(!bystander.BoundingBox.Intersects(westDoor.BoundingBox), "Arriving on the tile next to a door must not re-trigger it");

// Shallow corner clips slide Link around the block, deep ones push him straight back.
var corner = new SquareBlock(new FakeSprite(), new Vector2(200, 200));
Link slider = CreateLink(new Vector2(48 + 107, 78 + 110)); // 3px into the block, 6px of vertical overlap
Collide(slider, corner);
Check(slider.BoundingBox.Left == 107 && slider.BoundingBox.Bottom == 200, "Shallow corner clips must nudge Link along the block");
Link blocked = CreateLink(new Vector2(48 + 107, 78 + 160)); // 3px into the block, 56px of vertical overlap
Collide(blocked, corner);
Check(blocked.BoundingBox.Right == 200 && blocked.BoundingBox.Top == 160, "Deep side hits must still push Link straight back");

// Rooms: doors go where the file says, debug keys wrap around, dead things get dropped.
RoomData MakeRoom(string name, bool start, params DoorData[] doors) => new RoomData
{
    Name = name, Background = "room1", PlayerSpawn = new GridPosition(1, 1), IsStartingRoom = start,
    Width = 5, Height = 5, Doors = doors.ToList(),
    Items = [new ItemData { Type = "Rupee", Position = new GridPosition(2, 2) }],
    Enemies = [new EnemyData { Type = "Stalfos", Position = new GridPosition(3, 3) }]
};
var toB = new DoorData { Position = new GridPosition(4, 2), Direction = "East", TargetRoom = "B", ArrivalPosition = new GridPosition(1, 2) };
var backToA = new DoorData { Position = new GridPosition(0, 2), Direction = "West", TargetRoom = "A" };
var rooms = new RoomManager(
    [MakeRoom("A", true, toB), MakeRoom("B", false, backToA), MakeRoom("C", false)],
    new FakeEnemies(), new BlockFactory(null!), new FakeItems());
Check(rooms.CurrentRoom.Name == "A", "Must start in the room flagged isStartingRoom");
Check(rooms.CurrentRoom.Doors.Count == 1 && rooms.CurrentRoom.Doors[0].Data == toB, "Room must build a Door per door entry");
Check(rooms.EnterDoor(rooms.CurrentRoom.Doors[0]) == new Vector2(96, 192) && rooms.CurrentRoom.Name == "B",
    "Entering a door must switch rooms and return the arrival tile");
Check(rooms.EnterDoor(rooms.CurrentRoom.Doors[0]) == new Vector2(96, 96) && rooms.CurrentRoom.Name == "A",
    "A door with no arrival position must use the target room's spawn");
bool threw = false;
try { rooms.EnterDoor(rooms.Rooms[1].Doors[0]); } catch (InvalidOperationException) { threw = true; }
Check(threw && rooms.CurrentRoom.Name == "A", "Using a door from another room must throw and stay put");
rooms.NextRoom(); rooms.NextRoom();
Check(rooms.CurrentRoom.Name == "C", "Next room must walk the load order");
rooms.NextRoom();
Check(rooms.CurrentRoom.Name == "A", "Next room must wrap to the first room");
rooms.PreviousRoom();
Check(rooms.CurrentRoom.Name == "C", "Previous room must wrap to the last room");
IRoom roomC = rooms.CurrentRoom;
Check(roomC.Items.Count == 1 && roomC.Enemies.Count == 1 && roomC.Collidables.Count() == 2, "Room collidables must include items and enemies");
roomC.Items[0].Collect();
for (int hit = 0; hit < 3; hit++) roomC.Enemies[0].TakeDamage();
roomC.Update(Step(0));
Check(roomC.Items.Count == 0 && roomC.Enemies.Count == 0, "Collected items and dead enemies must leave the room on update");
threw = false;
try { new RoomManager([MakeRoom("A", false)], new FakeEnemies(), new BlockFactory(null!), new FakeItems()); }
catch (InvalidDataException) { threw = true; }
Check(threw, "Loading with no starting room must fail");
Console.WriteLine($"Passed {checks} collision checks.");

sealed class TestDoor(Rectangle bounds) : ICollidable
{
    public ICollidable? Visitor;
    public Rectangle BoundingBox => bounds;
    public CollisionProperties Properties { get; } = new() { Type = CollidableTypes.Door };
    public void Initialize() => Properties.OnCollide = visitor => Visitor = visitor;
}

sealed class TestItem(Vector2 position, Point size) : AbstractItem(new FakeSprite(), position, size);

sealed class FakeItems : IFactory<ItemFactoryData, IItem>
{
    public IItem Create(ItemFactoryData data) => new TestItem(data.Position, new Point(40, 80));
}

sealed class FakeEnemies : IFactory<EnemyFactoryData, AbstractEnemy>
{
    public AbstractEnemy Create(EnemyFactoryData data) => new TestEnemy(data.Position, 1f);
}

sealed class TestEnemy : AbstractEnemy
{
    public TestEnemy(Vector2 position, float scale) : this(position, Vector2.Zero, new Rectangle(0, 0, 16, 16), scale) { }
    private TestEnemy(Vector2 position, Vector2 velocity, Rectangle frame, float scale)
        : base(ref position, ref velocity, frame, scale) { enemySprite = new FakeSprite(); }
    protected override void Move(GameTime gameTime) { }
}

sealed class FakeSprite : ISprite
{
    public bool Loop { get; set; }
    public bool IsFinished { get; set; }
    public SpriteEffects Effects { get; set; }
    public Color Color { get; set; } = Color.White;
    public void Update(GameTime gameTime) { }
    public void Draw(SpriteBatch spriteBatch) { }
    public void MoveOrigin(Vector2 origin) { }
    public void SetFrameOrigin(Vector2[] origins) { }
}
