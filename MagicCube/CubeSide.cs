using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MagicCube
{
	public class CubeSide : IEnumerable<CellColor>, IEnumerator<CellColor>
	{
		private readonly CellColor[] colors;
		private int index;

		public CubeSide(CellColor[] colors)
		{
			this.colors = colors;
		}

		public CubeSide(CellColor sideColor)
		{
			colors = Enumerable
				.Range(1, 8)
				.Select(i => sideColor)
				.ToArray();
		}

		public CubeSide(CubeSide sideColor)
		{
			colors = (CellColor[])sideColor.colors.Clone();
		}

		public CellColor this[int i]
		{
			get { return colors[i]; }
			set { colors[i] = value; }
		}

		#region IEnumerable<T

		public IEnumerator<CellColor> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			//ignore
		}

		public bool MoveNext()
		{
			if (index == colors.Length - 1)
			{
				Reset();
				return false;
			}

			++index;
			return true;
		}

		public void Reset()
		{
			index = -1;
		}

		public CellColor Current => colors[index];

		object IEnumerator.Current => Current;

		#endregion IEnumerable<T>
	}
}