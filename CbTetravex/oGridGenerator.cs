using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbTetravex
{
	public static class oGridGenerator
	{

		public static int GetRandomTriNumber()
		{
			Random rnd = module.rnd;
			return rnd.Next(0, 9);
		}


		public static List<oTile> GetRandom3x3Grid()
		{

			oTile[,] TileGrid = new oTile[3, 3];
			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					TileGrid[x, y] = new oTile();

					//la position n'est pas defini car elle doit etre defini aleatoirement

					////test
					//TileGrid[x, y].pos = new oTile.TilePos(x, y, oTile.GridParent.gStart);
					
				}
			}

			//generation aleatoire des arrete interieur
			//verticale
			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 1; x++)
				{
					int newn = oGridGenerator.GetRandomTriNumber();
					TileGrid[x, y].TriRight.TheNumber = newn;
					TileGrid[x + 1, y].TriLeft.TheNumber = newn;
				}
			}
			//horizontale
			for (int y = 0; y <= 1; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					int newn = oGridGenerator.GetRandomTriNumber();
					TileGrid[x, y].TriDown.TheNumber = newn;
					TileGrid[x, y + 1].TriUp.TheNumber = newn;
				}
			}
			
			//generation aleatoire des bordule
			//verticale
			for (int y = 0; y <= 2; y++)
			{
				TileGrid[0, y].TriLeft.TheNumber = oGridGenerator.GetRandomTriNumber();
				TileGrid[2, y].TriRight.TheNumber = oGridGenerator.GetRandomTriNumber();
			}
			//horizontale
			for (int x = 0; x <= 2; x++)
			{
				TileGrid[x, 0].TriUp.TheNumber = oGridGenerator.GetRandomTriNumber();
				TileGrid[x, 2].TriDown.TheNumber = oGridGenerator.GetRandomTriNumber();
			}


			//definition aleatoire de la position des case
			List<oTile.TilePos> AllTilePos = new List<oTile.TilePos>();
			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					oTile.TilePos newtilepos = new oTile.TilePos(x, y, oTile.GridParent.gStart);
					AllTilePos.Add(newtilepos);
				}
			}

			Random rnd = module.rnd;
			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					//optien un index aleatoire
					int rndindex = 0;
					rndindex = rnd.Next(0, AllTilePos.Count - 1);

					oTile.TilePos tp = AllTilePos[rndindex];
					AllTilePos.RemoveAt(rndindex);

					TileGrid[x, y].pos = tp;

				}
			}




			//end
			List<oTile> rep = new List<oTile>();
			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					TileGrid[x, y].RefreshImage();
					rep.Add(TileGrid[x, y]);
				}
			}
			return rep;
		}




	}
}
