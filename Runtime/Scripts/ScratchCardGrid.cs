using UnityEngine;

namespace ScratchCard
{
    public readonly struct ScratchCardGrid
    {
        public static ScratchCardGrid Generate(Rect rect, Vector2 brushScale, int gridScale, ScratchCardGrid previous)
        {
            var aspect = rect.width / rect.height;
            var width = (int)Mathf.Clamp(gridScale * (1.0f / brushScale.x * aspect), 1, 256);
            var height = (int)Mathf.Clamp(gridScale * (1.0f / brushScale.y), 1, 64);

            ulong[] cells;
            
            if (previous.Width == width && previous.Height == height)
            {
                cells = previous.cells;
            }
            else
            {
                cells = new ulong[width];
            }
            
            return new ScratchCardGrid(width, height, cells);
        }

        public bool Valid => Width > 0 && Height > 0;
        
        public readonly int Width;
        public readonly int Height;

        private readonly ulong[] cells;

        public ScratchCardGrid(int width, int height, ulong[] cells)
        {
            Width = width;
            Height = height;
            
            this.cells = cells;
        }

        public float GetVisitedPercent()
        {
            var total = 0;
            var visited = 0;
            
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    total++;

                    if (GetVisited(x, y))
                    {
                        visited++;
                    }
                }
            }

            return (float)visited / total;
        }

        public void ClearVisited()
        {
            for (int i = 0; i < cells.Length; ++i)
            {
                cells[i] = 0;
            }
        }

        public bool GetVisited(int x, int y)
        {            
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }
            
            return (cells[x] & 1UL << y) > 0;
        }

        public bool SetVisited(int x, int y, bool value)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }

            var g = cells[x];
            var current = (g & 1UL << y) != 0;
            
            if (value)
            {
                g |= 1UL << y;
            }
            else
            {
                g &= ~(1UL << y);
            }

            cells[x] = g;
            
            return current != value;
        }
        
        public Rect CalculateCellRect(Rect containerRect, int x, int y)
        {
            var w = containerRect.width / Width;
            var h = containerRect.height / Height;

            var cx = containerRect.x + x * w + w / 2.0f;
            var cy = containerRect.y + y * h + h / 2.0f;

            return new Rect(cx, cy, w, h);
        }
        
        public Vector2Int LocalPointToCell(Rect rect, Vector2 point)
        {
            var w = rect.width / Width;
            var h = rect.height / Height;

            var x = (point.x - rect.x) / w;
            var y = (point.y - rect.y) / h;
            
            return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        }
    }
}