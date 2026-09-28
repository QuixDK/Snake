using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wyrm
{
    internal class Program
    {
        private const long MsForFrame = 1000000;
        private const int StartXLocation = 0;
        private const int StartYLocation = 0;
        private const int FieldSize = 10;
        private const int StartWyrmSize = 3;

        private static void Main(string[] args)
        {
            var gameState = new GameState();
            gameState.WyrmSize = StartWyrmSize;
            for (var i = 0; i < gameState.WyrmSize; i++)
            {
                gameState.WyrmBodyXy.Add(
                    new GameState.Point(StartXLocation + i, StartYLocation)
                );
            }

            var startedTime = DateTime.Now.Ticks;

            while (true)
            {
                var currentTime = DateTime.Now.Ticks;
                var passedTime = currentTime - startedTime;

                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    gameState.direction = Input(gameState, keyInfo);
                }

                if (passedTime > MsForFrame)
                {
                    startedTime = currentTime;
                    Update(gameState);
                    if (gameState.gameEnded)
                    {
                        Render(gameState);
                        break;
                    }

                    Render(gameState);
                }
            }
        }


        private static GameState.Direction Input(GameState gameState, ConsoleKeyInfo key) => key.Key switch
        {
            ConsoleKey.W => GameState.Direction.Up,
            ConsoleKey.A => GameState.Direction.Left,
            ConsoleKey.S => GameState.Direction.Down,
            ConsoleKey.D => GameState.Direction.Right,
            _ => gameState.direction
        };

        private static void Update(GameState gameState)
        {
            for (var i = 0; i < gameState.WyrmBodyXy.Count - 1; i++)
            {
                gameState.WyrmBodyXy[i].x = gameState.WyrmBodyXy[i + 1].x;
                gameState.WyrmBodyXy[i].y = gameState.WyrmBodyXy[i + 1].y;
            }

            var tailPointX = gameState.WyrmBodyXy[0].x;
            var tailPointY = gameState.WyrmBodyXy[0].y;
            var lastPoint = gameState.WyrmBodyXy.Last();
            switch (gameState.direction)
            {
                case GameState.Direction.Right:
                    lastPoint.x += 1;
                    if (lastPoint.x == FieldSize) lastPoint.x = 0;
                    break;
                case GameState.Direction.Left:
                    lastPoint.x -= 1;
                    if (lastPoint.x < 0) lastPoint.x = FieldSize;
                    break;
                case GameState.Direction.Up:
                    lastPoint.y -= 1;
                    if (lastPoint.y < 0) lastPoint.y = FieldSize;
                    break;
                case GameState.Direction.Down:
                    lastPoint.y += 1;
                    if (lastPoint.y == FieldSize) lastPoint.y = 0;
                    break;
                default:
                    break;
            }

            var appleEated = false;
            if (lastPoint.x == gameState.AppleX && lastPoint.y == gameState.AppleY)
            {
                gameState.WyrmSize += 1;
                appleEated = true;
            }
            foreach (var point in gameState.WyrmBodyXy)
            {
                
                if (point == lastPoint) continue;
                
                if (point.x == lastPoint.x &&
                    point.y == lastPoint.y)
                {
                    gameState.gameEnded = true;
                    break;
                }
                
            }

            if (!appleEated) return;

            gameState.WyrmBodyXy.Insert(0,
                new GameState.Point(tailPointX, tailPointY));
            
            PlaceApple(gameState);
            
        }

        private static void PlaceApple(GameState gameState)
        {
            while (true)
            {
                gameState.AppleX = new Random().Next(0, 10);
                gameState.AppleY = new Random().Next(0, 10);
                
                var isChecked = false;
                foreach (var point in gameState.WyrmBodyXy)
                {
                    if (gameState.AppleX == point.x && gameState.AppleY == point.y) isChecked = true;
                }
                
                if (!isChecked) break;
            }
        }

        private static void Render(GameState gameState)
        {
            
            Console.Clear();
            

            var isChecked = false;
            for (var y = 0; y < FieldSize; y++)
            {
                for (var x = 0; x < FieldSize; x++)
                {
                    foreach (var point in gameState.WyrmBodyXy)
                    {
                        if (point.x != x || point.y != y) continue;
                        Console.Write("0");
                        isChecked = true;
                        break;
                    }

                    if (isChecked)
                    {
                        isChecked = false;
                        continue;
                    }
                    else if (gameState.AppleX == x && gameState.AppleY == y)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }

                Console.WriteLine();
            }
            
            if (gameState.gameEnded)
            {
                Console.WriteLine("Game Over!");
            }
        }
    }
}