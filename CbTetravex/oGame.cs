using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace CbTetravex
{
	public class oGame
	{
		public Point MousePos { get { return this.ImageBox.PointToClient(Cursor.Position); } }
		
		private PictureBox ImageBox;
		public Control Parent
		{
			get { return this.ImageBox.Parent; }
			set { this.ImageBox.Parent = value; }
		}
		public int Top
		{
			get { return this.ImageBox.Top; }
			set { this.ImageBox.Top = value; }
		}
		public int Left
		{
			get { return this.ImageBox.Left; }
			set { this.ImageBox.Left = value; }
		}
		public int Width
		{
			get { return this.ImageBox.Width; }
		}
		public int Height
		{
			get { return this.ImageBox.Height; }
		}

		public event EventHandler SizeChanged;
		private void Raise_SizeChanged() { if (this.SizeChanged != null) { this.SizeChanged(this, new EventArgs()); } }


		

		private int ImgWidth = 600;
		private int ImgHeight = 300;

		private enum GameStade
		{
			sNothingHappening,
			sTileMoving
		}
		private GameStade ActualGameStade = GameStade.sNothingHappening;

		private List<oTile> AllTile = new List<oTile>();//cette liste contien toute les tuile du jeu
		//ces grille represente les position graphique des tile
		private oVirtualGrid GridRep = new oVirtualGrid();
		private oVirtualGrid GridStart = new oVirtualGrid();

		private bool IsAnyTileAtPos(oTile.TilePos ThePos)
		{
			bool rep = false;

			foreach (oTile ActualTile in this.AllTile)
			{
				if (ActualTile.pos.x == ThePos.x && ActualTile.pos.y == ThePos.y && ActualTile.pos.parent == ThePos.parent)
				{
					rep = true;
					break;
				}
			}
			return rep;
		}
		private oTile GetTileAtPos(oTile.TilePos ThePos)
		{
			oTile rep = null;

			foreach (oTile ActualTile in this.AllTile)
			{
				if (ActualTile.pos.x == ThePos.x && ActualTile.pos.y == ThePos.y && ActualTile.pos.parent == ThePos.parent)
				{
					rep = ActualTile;
					break;
				}
			}
			return rep;
		}
		private oTile GetTheMovingTile()
		{
			oTile rep = null;
			foreach (oTile ActualTile in this.AllTile)
			{
				if (ActualTile.IsMoving)
				{
					rep = ActualTile;
					break;
				}
			}
			return rep;
		}



		private void ResetForNewGame()
		{
			while (this.AllTile.Count > 0) { this.AllTile.RemoveAt(0); }
			this.ActualGameStade = GameStade.sNothingHappening;

			List<oTile> NewTile = oGridGenerator.GetRandom3x3Grid();
			foreach (oTile t in NewTile)
			{
				this.AllTile.Add(t);
			}


		}
		//check si le joueur a terminer le jeu
		private bool IsGameCleared()
		{
			bool rep = true;
			foreach (oTile t in this.AllTile)
			{
				if (t.pos.parent == oTile.GridParent.gStart)
				{
					rep = false;
					break;
				}
			}

			if (rep)
			{
				//transfere toute les tile dans une grille pour l'analyse des arrete
				oTile[,] TileGrid = new oTile[3, 3];
				foreach (oTile t in this.AllTile)
				{
					TileGrid[t.pos.x, t.pos.y] = t;
				}
				
				//analyse si les arrete corresponde
				//horizontale
				for (int y = 0; y <= 2; y++)
				{
					for (int x = 0; x <= 1; x++)
					{
						if (TileGrid[x, y].TriRight.TheNumber != TileGrid[x + 1, y].TriLeft.TheNumber)
						{
							rep = false;
							break;
						}
					}
					if (!rep) { break; }
				}
				if (rep)
				{
					//verticale
					for (int y = 0; y <= 1; y++)
					{
						for (int x = 0; x <= 2; x++)
						{
							if (TileGrid[x, y].TriDown.TheNumber != TileGrid[x, y + 1].TriUp.TheNumber)
							{
								rep = false;
								break;
							}
						}
						if (!rep) { break; }
					}
				}
				
				
			}

			return rep;
		}

		//check si le joueur a terminer le jeu et gerer le reset de la grille si tel est le cas
		private void EasyResetGameIfOk()
		{
			bool IsPlayerCleared = this.IsGameCleared();
			if (IsPlayerCleared)
			{
				this.Refresh();

				oWinForm wf = new oWinForm();
				wf.ShowDialog(module.MainForm);

				if (wf.TheExitMethod == oWinForm.ExitMethod.CancelButton)
				{
					Application.Exit();
				}
				else
				{
					this.ResetForNewGame();
				}


			}

		}


		public void RefreshVariable()
		{
			int GridSpacement = 30; //espacement entre les 2 grille

			this.GridRep.Top = 3;
			this.GridRep.Left = 3;
			this.GridRep.CaseWidth = oTile.TileWidth;
			this.GridRep.CaseHeight = oTile.TileWidth;
			this.GridRep.CaseSpace = 3;

			this.GridStart.Top = this.GridRep.Top;
			this.GridStart.Left = this.GridRep.Left + this.GridRep.CaseSpace + ((this.GridRep.CaseSpace + this.GridRep.CaseWidth) * 3) + GridSpacement;
			this.GridStart.CaseWidth = oTile.TileWidth;
			this.GridStart.CaseHeight = oTile.TileWidth;
			this.GridStart.CaseSpace = this.GridRep.CaseSpace;
			


			this.ImgHeight = (this.GridRep.Top * 2) + this.GridRep.CaseSpace + ((this.GridRep.CaseSpace + this.GridRep.CaseHeight) * 3);
			this.ImgWidth = this.GridStart.Left + this.GridStart.CaseSpace + ((this.GridStart.CaseSpace + this.GridStart.CaseWidth) * 3) + this.GridRep.Left;
		}
		public void Refresh()
		{
			Bitmap img = new Bitmap(this.ImgWidth, this.ImgHeight);
			Graphics g = Graphics.FromImage(img);
			g.Clear(Color.Gray);

			//dessine les grille
			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					Rectangle ActualCaseRep = this.GridRep.GetCasePosition(x, y);
					Rectangle ActualCaseStart = this.GridStart.GetCasePosition(x, y);
					g.FillRectangle(Brushes.Gainsboro, ActualCaseRep);
					g.FillRectangle(Brushes.Gainsboro, ActualCaseStart);
				}
			}



			//lorsqu'une tile est en mouvement, il aparait dans la case sous la tile un petit rectangle foncer. il doit etre dessiner avant toute les tile pour qu'il soit dessous
			if (this.ActualGameStade == GameStade.sTileMoving)
			{
				Point mpos = this.MousePos;
				int w2 = this.ImgWidth / 2;
				if (mpos.X < w2)
				{
					oVirtualGrid.CasePos cp = this.GridRep.GetCaseUnderPoint(mpos.X, mpos.Y);
					if (cp.Exist && cp.x <= 2 && cp.y <= 2)
					{
						Rectangle TheZone = this.GridRep.GetCasePosition(cp.x, cp.y);
						TheZone.Width -= 1;
						TheZone.Height -= 1;
						TheZone.Inflate(-5, -5);
						g.DrawRectangle(Pens.DimGray, TheZone);
						TheZone.Inflate(-1, -1);
						g.DrawRectangle(Pens.DimGray, TheZone);

					}
				}
				if (mpos.X > w2)
				{
					oVirtualGrid.CasePos cp = this.GridStart.GetCaseUnderPoint(mpos.X, mpos.Y);
					if (cp.Exist && cp.x <= 2 && cp.y <= 2)
					{
						Rectangle TheZone = this.GridStart.GetCasePosition(cp.x, cp.y);
						TheZone.Width -= 1;
						TheZone.Height -= 1;
						TheZone.Inflate(-5, -5);
						g.DrawRectangle(Pens.DimGray, TheZone);
						TheZone.Inflate(-1, -1);
						g.DrawRectangle(Pens.DimGray, TheZone);

					}
				}

			}
			
			//dessine les tuile
			//cette gestion de la tile en movement est obligatoire pour qu'elle soit dessiner par dessus toute les autre
			oTile TheMovingTile = null;
			bool IsAMovingTile = false;
			foreach (oTile ActualTile in this.AllTile)
			{
				if (!ActualTile.IsMoving)
				{
					if (ActualTile.pos.parent == oTile.GridParent.gRep)
					{
						Rectangle ActualTilePos = this.GridRep.GetCasePosition(ActualTile.pos.x, ActualTile.pos.y);
						g.DrawImage(ActualTile.ActualImage, ActualTilePos.Location);
					}
					if (ActualTile.pos.parent == oTile.GridParent.gStart)
					{
						Rectangle ActualTilePos = this.GridStart.GetCasePosition(ActualTile.pos.x, ActualTile.pos.y);
						g.DrawImage(ActualTile.ActualImage, ActualTilePos.Location);
					}
				}
				else
				{
					TheMovingTile = ActualTile;
					IsAMovingTile = true;
					
				}
			}
			if (IsAMovingTile)
			{
				Point mpos = this.MousePos;
				int demiwidth = oTile.TileWidth / 2;
				Point tilepos = new Point(mpos.X - demiwidth, mpos.Y - demiwidth);
				g.DrawImage(TheMovingTile.ActualImage, tilepos);
			}
			



			g.Dispose();
			if (this.ImageBox.Image != null) { this.ImageBox.Image.Dispose(); }
			this.ImageBox.Image = img;
			this.ImageBox.Width = this.ImgWidth;
			this.ImageBox.Height = this.ImgHeight;
			GC.Collect();
		}


		//void new()
		public oGame()
		{
			this.ImageBox = new PictureBox();
			this.ImageBox.Width = 400;
			this.ImageBox.Height = 400;
			this.ImageBox.SizeChanged += new EventHandler(this.ImageBox_SizeChanged);
			this.ImageBox.MouseMove += new MouseEventHandler(this.ImageBox_MouseMove);
			this.ImageBox.MouseDown += new MouseEventHandler(this.ImageBox_MouseDown);




			////test de fonctionnement des tile lorsque je les ai terminer
			//oTile t1 = new oTile();
			//t1.TriUp.TheNumber = 1;
			//t1.pos.parent = oTile.GridParent.gRep;
			//t1.RefreshImage();
			//this.AllTile.Add(t1);
			//oTile t2 = new oTile();
			//t2.TriUp.TheNumber = 6;
			//t2.TriRight.TheNumber = 7;
			//t2.TriDown.TheNumber = 2;
			//t2.TriLeft.TheNumber = 9;
			//t2.pos = new oTile.TilePos(1, 0, oTile.GridParent.gStart);
			//t2.RefreshImage();
			//this.AllTile.Add(t2);


			this.ResetForNewGame();

			this.RefreshVariable();
		}

		private void ImageBox_SizeChanged(object sender, EventArgs e)
		{
			this.Raise_SizeChanged();
		}
		private void ImageBox_MouseMove(object sender, MouseEventArgs e)
		{
			this.Refresh();
		}
		private void ImageBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Point mpos = this.MousePos;
				int w2 = this.ImgWidth / 2; //pour savoir quelle grille il faut chercker

				//======== si aucune tile est en deplacement
				if (this.ActualGameStade == GameStade.sNothingHappening)
				{
					if (mpos.X < w2)
					{
						oVirtualGrid.CasePos CaseUnderM = this.GridRep.GetCaseUnderPoint(mpos.X, mpos.Y);
						if (CaseUnderM.Exist && CaseUnderM.x <= 2 && CaseUnderM.y <= 2)
						{
							oTile.TilePos tp = new oTile.TilePos(CaseUnderM.x, CaseUnderM.y, oTile.GridParent.gRep);
							//check s'il y a une tile a cette case
							if (this.IsAnyTileAtPos(tp))
							{
								oTile TheClickedTile = this.GetTileAtPos(tp);
								TheClickedTile.SavePos();
								TheClickedTile.IsMoving = true;
								//ce changement de coordonner sert a degager la tile des grille au cas ou le joueur la remet a la meme place
								TheClickedTile.pos.x = -1;
								TheClickedTile.pos.y = -1;
								this.ActualGameStade = GameStade.sTileMoving;
							}
						}
					}
					if (mpos.X > w2)
					{
						oVirtualGrid.CasePos CaseUnderM = this.GridStart.GetCaseUnderPoint(mpos.X, mpos.Y);
						if (CaseUnderM.Exist && CaseUnderM.x <= 2 && CaseUnderM.y <= 2)
						{
							oTile.TilePos tp = new oTile.TilePos(CaseUnderM.x, CaseUnderM.y, oTile.GridParent.gStart);
							//check s'il y a une tile a cette case
							if (this.IsAnyTileAtPos(tp))
							{
								oTile TheClickedTile = this.GetTileAtPos(tp);
								TheClickedTile.SavePos();
								TheClickedTile.IsMoving = true;
								//ce changement de coordonner sert a degager la tile des grille au cas ou le joueur la remet a la meme place
								TheClickedTile.pos.x = -1;
								TheClickedTile.pos.y = -1;
								this.ActualGameStade = GameStade.sTileMoving;
							}
						}
					}
				}
				// /!\ /!\ /!\ IL EST TRES IMPORTANT QUE CE SOIT UN ELSE car lorsqu'une case est clicker pour commencer un deplacement, this.ActualGameStade devien sTileMoving et il ne faut pas que le code de relachement de la tile s'execute immediatement apres
				else //========= si une tile est en deplacement
				{
					oTile TheMovingTile = this.GetTheMovingTile();

					if (mpos.X < w2)
					{
						oVirtualGrid.CasePos cp = this.GridRep.GetCaseUnderPoint(mpos.X, mpos.Y);
						if (cp.Exist && cp.x <= 2 && cp.y <= 2)
						{
							oTile.TilePos tp = new oTile.TilePos(cp.x, cp.y, oTile.GridParent.gRep);
							//check s'il y a deja une tile en dessous
							bool IsAnyTile = this.IsAnyTileAtPos(tp);
							oTile TheTileUnder = this.GetTileAtPos(tp);
							if (!IsAnyTile)
							{
								TheMovingTile.pos.x = tp.x;
								TheMovingTile.pos.y = tp.y;
								TheMovingTile.pos.parent = tp.parent;
								TheMovingTile.IsMoving = false;
								this.ActualGameStade = GameStade.sNothingHappening;
								this.EasyResetGameIfOk();
							}
							else
							{
								//oTile.TilePos temppos = TheMovingTile.pos;
								TheMovingTile.pos = TheTileUnder.pos;
								TheTileUnder.pos = TheMovingTile.savedpos;
								TheMovingTile.IsMoving = false;
								this.ActualGameStade = GameStade.sNothingHappening;
								this.EasyResetGameIfOk();
							}
						}
						
					}
					if (mpos.X > w2)
					{
						oVirtualGrid.CasePos cp = this.GridStart.GetCaseUnderPoint(mpos.X, mpos.Y);
						if (cp.Exist && cp.x <= 2 && cp.y <= 2)
						{
							oTile.TilePos tp = new oTile.TilePos(cp.x, cp.y, oTile.GridParent.gStart);
							//check s'il y a deja une tile en dessous
							bool IsAnyTile = this.IsAnyTileAtPos(tp);
							oTile TheTileUnder = this.GetTileAtPos(tp);
							if (!IsAnyTile)
							{
								TheMovingTile.pos.x = tp.x;
								TheMovingTile.pos.y = tp.y;
								TheMovingTile.pos.parent = tp.parent;
								TheMovingTile.IsMoving = false;
								this.ActualGameStade = GameStade.sNothingHappening;
								this.EasyResetGameIfOk();
							}
							else
							{
								TheMovingTile.pos = TheTileUnder.pos;
								TheTileUnder.pos = TheMovingTile.savedpos;
								TheMovingTile.IsMoving = false;
								this.ActualGameStade = GameStade.sNothingHappening;
								this.EasyResetGameIfOk();
							}
						}

					}


				}
			}
			this.Refresh();

			////aucune tile ne doit etre en deplacement car les coordonner negative des tile en movement vont generer des erreur
			//if (this.ActualGameStade != GameStade.sTileMoving)
			//{
			//	this.EasyResetGameIfOk();
			//}

		}




	}
}
