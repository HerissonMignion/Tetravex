using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CbTetravex
{
	
	public class oTile
	{

		#region static things

		//position des triangle dans les tile
		public enum TileDiv
		{
			up,
			down,
			right,
			left
		}
		public enum GridParent
		{
			gStart,
			gRep
		}
		public struct TilePos
		{
			public int x;
			public int y;
			public oTile.GridParent parent;

			//void new()
			public TilePos(int startx, int starty, oTile.GridParent StartGridParent)
			{
				this.x = startx;
				this.y = starty;
				this.parent = StartGridParent;
			}
		}

		public struct sTileTriangle
		{
			private int zzzTheNumber;
			public int TheNumber
			{
				get { return this.zzzTheNumber; }
				set
				{
					this.zzzTheNumber = value;
					this.RefreshVariable();
				}
			}
			
			private oTile.TileDiv zzzTheTileDiv;
			public oTile.TileDiv TheTileDiv { get { return this.zzzTheTileDiv; } set { this.zzzTheTileDiv = value; } }

			private oTile.sTileTriangleColor zzzTheTriangleColor;
			public oTile.sTileTriangleColor TheTriangleColor { get { return this.zzzTheTriangleColor; } }

			public Color BackColor { get { return this.TheTriangleColor.BackColor; } }
			public Color ForeColor { get { return this.TheTriangleColor.ForeColor; } }

			public void RefreshVariable()
			{
				this.zzzTheTriangleColor = oTile.GetTriangleColor(this.TheNumber);

			}

			//void new()
			public sTileTriangle(oTile.TileDiv StartDiv, int StartNumber)
			{
				this.zzzTheTileDiv = StartDiv;
				this.zzzTheNumber = StartNumber;

				this.zzzTheTriangleColor = new sTileTriangleColor(Color.Black, Color.White);
				this.RefreshVariable();
			}
		}
		public struct sTileTriangleColor
		{
			public Color BackColor;
			public Color ForeColor;

			//void new()
			public sTileTriangleColor(Color StartBackColor, Color StartForeColor)
			{
				this.BackColor = StartBackColor;
				this.ForeColor = StartForeColor;
			}
		}
		
		public static int TileWidth = 64; // 64 laugeur et hauteur d'une tile

		//en fonction des parametre d'une tuile, il cree un bitmap 64x64 qui contien les nombre de la tuile avec les bonne couleur
		public static Bitmap GetBitmapForTile(oTile TheTile)
		{
			int twidth = oTile.TileWidth;
			Bitmap img = new Bitmap(twidth, twidth);
			Graphics g = Graphics.FromImage(img);
			g.Clear(Color.White);

			Bitmap imgUp = oTile.GetBitmapForTriangle(TheTile.TriUp);
			Bitmap imgDown = oTile.GetBitmapForTriangle(TheTile.TriDown);
			Bitmap imgRight = oTile.GetBitmapForTriangle(TheTile.TriRight);
			Bitmap imgLeft = oTile.GetBitmapForTriangle(TheTile.TriLeft);

			g.DrawImage(imgUp, 0, 0);
			g.DrawImage(imgDown, 0, 0);
			g.DrawImage(imgRight, 0, 0);
			g.DrawImage(imgLeft, 0, 0);


			Pen DiagoPen = new Pen(Color.Black, 2f);
			g.DrawLine(DiagoPen, 0, 0, twidth, twidth);
			g.DrawLine(DiagoPen, 0, twidth, twidth, 0);

			Pen BorderPen = new Pen(Color.Black, 1f);
			g.DrawRectangle(BorderPen, 0, 0, twidth - 1, twidth - 1);


			g.Dispose();
			return img;
		}

		//optien un seul triangle d'une tile en fonction du nombre et la position du triangle
		public static Bitmap GetBitmapForTriangle(oTile.TileDiv TheDiv, int TheNumber)
		{
			int twidth = oTile.TileWidth;
			int twidth2 = twidth / 2;
			Bitmap img = new Bitmap(twidth, twidth);
			img.MakeTransparent();
			Graphics g = Graphics.FromImage(img);

			//fait quelque truc que tout le monde aura ou pourait avoir besoin
			string Text = TheNumber.ToString();
			Font TextFont = new Font("consolas", 15);
			Size TextSize = module.GetTextSize(Text, TextFont);

			sTileTriangleColor AllColor = oTile.GetTriangleColor(TheNumber);
			Brush BackBrush = new SolidBrush(AllColor.BackColor);
			Brush ForeBrush = new SolidBrush(AllColor.ForeColor);

			//les coordonner des coin et du centre
			Point cUL = new Point(0, 0);
			Point cUR = new Point(twidth, 0);
			Point cDL = new Point(0, twidth);
			Point cDR = new Point(twidth, twidth);
			Point cCenter = new Point(twidth2, twidth2);
			
			if (TheDiv == TileDiv.up)
			{
				Point[] t = new Point[] { cUL, cUR, cCenter };
				g.FillPolygon(BackBrush, t);

				Point TextPos = new Point(twidth2 - (TextSize.Width / 2), (twidth2 / 2) - (TextSize.Height / 2));
				g.DrawString(Text, TextFont, ForeBrush, (float)(TextPos.X), (float)(TextPos.Y));

			}
			if (TheDiv == TileDiv.down)
			{
				Point[] t = new Point[] { cDL, cDR, cCenter };
				g.FillPolygon(BackBrush, t);

				Point TextPos = new Point(twidth2 - (TextSize.Width / 2), (twidth2 * 3 / 2) - (TextSize.Height / 2));
				g.DrawString(Text, TextFont, ForeBrush, (float)(TextPos.X), (float)(TextPos.Y));
				
			}
			if (TheDiv == TileDiv.right)
			{
				Point[] t = new Point[] { cUR, cDR, cCenter };
				g.FillPolygon(BackBrush, t);

				Point TextPos = new Point((twidth2 * 3 / 2) - (TextSize.Width / 2), twidth2 - (TextSize.Height / 2));
				g.DrawString(Text, TextFont, ForeBrush, (float)(TextPos.X), (float)(TextPos.Y));

			}
			if (TheDiv == TileDiv.left)
			{
				Point[] t = new Point[] { cUL, cDL, cCenter };
				g.FillPolygon(BackBrush, t);

				Point TextPos = new Point((twidth2 / 2) - (TextSize.Width / 2), twidth2 - (TextSize.Height / 2));
				g.DrawString(Text, TextFont, ForeBrush, (float)(TextPos.X), (float)(TextPos.Y));

			}
			

			g.Dispose();
			return img;
		}
		public static Bitmap GetBitmapForTriangle(sTileTriangle TheTriangle)
		{
			return oTile.GetBitmapForTriangle(TheTriangle.TheTileDiv, TheTriangle.TheNumber);
		}

		//optien quel couleur est associer a un nombre
		public static sTileTriangleColor GetTriangleColor(int TheNumber)
		{
			sTileTriangleColor rep = new sTileTriangleColor(Color.Black, Color.White);
			switch (TheNumber)
			{
				case 0:
					rep.BackColor = Color.FromArgb(32, 32, 32);
					rep.ForeColor = Color.FromArgb(254, 254, 254);
					break;
				case 1:
					rep.BackColor = module.MultiplyLightLevel(Color.Chocolate, 1.1f);
					rep.ForeColor = Color.Black;
					break;
				case 2:
					rep.BackColor = module.MultiplyLightLevel(Color.Red, 0.9f);
					rep.ForeColor = Color.FromArgb(254, 254, 254);
					break;
				case 3:
					rep.BackColor = Color.Orange;
					rep.ForeColor = Color.Black;
					break;
				case 4:
					rep.BackColor = Color.Yellow;
					rep.ForeColor = Color.Black;
					break;
				case 5:
					rep.BackColor = Color.LimeGreen;
					rep.ForeColor = Color.Black;
					break;
				case 6:
					rep.BackColor = module.MultiplyLightLevel(Color.SteelBlue, 0.75f);
					rep.ForeColor = Color.FromArgb(254, 254, 254);
					break;
				case 7:
					rep.BackColor = Color.Purple;
					rep.ForeColor = Color.FromArgb(254, 254, 254);
					break;
				case 8:
					rep.BackColor = Color.Gray;
					rep.ForeColor = Color.Black;
					break;
				case 9:
					rep.BackColor = Color.Gainsboro;
					rep.ForeColor = Color.Black;
					break;
				default:
					rep.BackColor = Color.DarkBlue;
					rep.ForeColor = Color.White;
					break;
			}
			return rep;
		}


		#endregion



		//les triangle de l'object this
		public sTileTriangle TriUp;
		public sTileTriangle TriDown;
		public sTileTriangle TriRight;
		public sTileTriangle TriLeft;
		private void CreateAllTriangle()
		{
			this.TriUp = new sTileTriangle(TileDiv.up, 0);
			this.TriDown = new sTileTriangle(TileDiv.down, 0);
			this.TriRight = new sTileTriangle(TileDiv.right, 0);
			this.TriLeft = new sTileTriangle(TileDiv.left, 0);
			
		}


		public Bitmap ActualImage;
		public void RefreshImage()
		{
			this.ActualImage = oTile.GetBitmapForTile(this);
		}


		public oTile.TilePos pos;
		public oTile.TilePos savedpos;
		public void SavePos() { this.savedpos = this.pos; }
		public bool IsMoving = false; //indique si l'utilisateur est en train de deplacer la tile this


		//void new()
		public oTile()
		{
			this.CreateAllTriangle();
			this.pos = new TilePos(0, 0, GridParent.gStart);
			this.savedpos = new TilePos(0, 0, GridParent.gStart);

			this.RefreshImage();
		}


	}
}
