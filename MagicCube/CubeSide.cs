namespace MagicCube
{
    public class CubeSide
    {
        private readonly CellColor[] colors;

        public CubeSide(CellColor[] colors)
        {
            this.colors = colors;
        }

        public CellColor this[int i] => colors[i];
    }
}