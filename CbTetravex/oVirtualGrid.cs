using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CbTetravex
{
	public class oVirtualGrid
	{

		public int CaseWidth = 100;
		public int CaseHeight = 100;
		public int CaseSpace = 10;

		public int Top = 0;
		public int Left = 0;
		

		public Rectangle GetCasePosition(int x, int y)
		{
			Rectangle rep = new Rectangle();
			rep.X = this.Left + this.CaseSpace;
			rep.Y = this.Top + this.CaseSpace;
			rep.X += (this.CaseSpace + this.CaseWidth) * x;
			rep.Y += (this.CaseSpace + this.CaseHeight) * y;
			rep.Width = this.CaseWidth;
			rep.Height = this.CaseHeight;
			return rep;
		}



		public struct CasePos
		{
			public bool Exist;
			//coordonner de la case
			public int x;
			public int y;
			
			//void new()
			public CasePos(bool StartExit, int startx, int starty)
			{
				this.Exist = StartExit;
				this.x = startx;
				this.y = starty;
			}
		}
		//obtien la position virtuel de la case situer sous la coordonner graphique
		public CasePos GetCaseUnderPoint(int x, int y)
		{
			CasePos rep = new CasePos(false, 0, 0);
			if (x < 0 || y < 0)
			{
				rep.Exist = false;
			}
			else
			{
				int actualx = x - this.Left;
				int nlx = 0; //nombre de loop x
				rep.Exist = false; //just to be sure
				while (actualx > 0)
				{
					if (actualx > this.CaseSpace && actualx < this.CaseSpace + this.CaseWidth)
					{
						rep.Exist = true;
						rep.x = nlx;
						break;
					}
					actualx -= this.CaseSpace + this.CaseWidth;
					nlx++;
				}

				if (rep.Exist)//s'il n'existe pas sur la coordonner x, la y n'y changera rien
				{
					int actualy = y - this.Top;
					int nly = 0; //nombre de loop y
					rep.Exist = false;
					while (actualy > 0)
					{
						if (actualy > this.CaseSpace && actualy < this.CaseSpace + this.CaseHeight)
						{
							rep.Exist = true;
							rep.y = nly;
							break;
						}
						actualy -= this.CaseSpace + this.CaseHeight;
						nly++;
					}

					if (!rep.Exist)
					{
						rep.x = 0;
						rep.y = 0;
					}
				}
			}
			return rep;
		}




	}
}
