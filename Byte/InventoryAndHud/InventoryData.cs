using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.InventoryAndHud;


public class InventoryData
{
    public int Rupees { get; set; } = 0;
    public int Keys { get; set; } = 0;
    public int Bombs { get; set; } = 0;

    public int MaxHalfHearts { get; set; } = 6;
    public int CurrentHalfHearts { get; set; } = 6;

    public bool HasMap { get; set; } = true;
    public bool HasCompass { get; set; } = true;

    public bool HasBoomerang { get; set; } = false;
    public bool HasBomb { get; set; } = false;
    public bool HasBow { get; set; } = false;

    public bool HasSword { get; set; } = true;

    public List<Point> VisitedRooms { get; private set; } = new List<Point>();
    public Point CurrentRoomCoords { get; set; } = new Point(0, 0);

}
