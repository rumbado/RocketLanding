using System.Linq;

namespace LandingControlSystem
{
    public class LandingController
    {
        private readonly object accessLock = new object();
        private CellStatus[,] _landingArea;

        public LandingController() : this(100, 10, (5, 5)) { }

        public LandingController(int areaSize, int platformSize, (int x, int y) platformTopLeftCorner)
        {
            _landingArea = new CellStatus[areaSize, areaSize];

            FillLandingPlatform(platformSize, platformTopLeftCorner);
        }

        private void FillLandingPlatform(int platformSize, (int x, int y) platformTopLeftCorner){
            for (int i = platformTopLeftCorner.x; i < platformTopLeftCorner.x + platformSize; i++)
            {
                for (int j = platformTopLeftCorner.y; j < platformTopLeftCorner.y + platformSize; j++)
                {
                    _landingArea[i, j] = CellStatus.OkForLanding;
                }
            }
        }

        public string CheckPosition(int x, int y)
        {
            CellStatus result;

            lock (accessLock)
            {
                result = _landingArea[x, y];

                if (result == CellStatus.OkForLanding) SetClashArea(x, y);
            }
            
            return result.GetDescription();
        }

        private void SetClashArea(int x, int y)
        {
            _landingArea[x, y] = CellStatus.Clash;
            if (CellExist(x - 1, y)) _landingArea[x - 1, y] = CellStatus.Clash;
            if (CellExist(x - 1, y - 1)) _landingArea[x - 1, y - 1] = CellStatus.Clash;
            if (CellExist(x - 1, y + 1)) _landingArea[x - 1, y + 1] = CellStatus.Clash;
            if (CellExist(x + 1, y)) _landingArea[x + 1, y] = CellStatus.Clash;
            if (CellExist(x + 1, y - 1)) _landingArea[x + 1, y - 1] = CellStatus.Clash;
            if (CellExist(x + 1, y + 1)) _landingArea[x + 1, y + 1] = CellStatus.Clash;
            if (CellExist(x, y - 1)) _landingArea[x, y - 1] = CellStatus.Clash;
            if (CellExist(x, y + 1)) _landingArea[x, y + 1] = CellStatus.Clash;
        }

        private bool CellExist(int x, int y)
        {
            if (x < 0) return false;
            if (y < 0) return false;
            if (x > _landingArea.GetLength(0)) return false;
            if (y > _landingArea.GetLength(0)) return false;

            return true;
        }
    }
}
