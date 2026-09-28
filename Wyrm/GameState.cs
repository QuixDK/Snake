using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyrm
{
    
    internal class GameState
    {
        public List<Point> WyrmBodyXy {  get; set; } = new List<Point>();
        public int WyrmSize { get; set; }
        public int AppleX {  get; set; } = 5;
        public int AppleY { get; set; } = 5;
        public bool gameEnded { get; set; } = false;

        public Direction direction { get; set; } = Direction.Right;

        public GameState() { }

        public enum Direction { Left, Right, Up, Down }

        public class Point
        {

            public int x {  get; set; }
            public int y { get; set; }

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}