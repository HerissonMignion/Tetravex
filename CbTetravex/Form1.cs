using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CbTetravex
{
	public partial class Form1 : Form
	{





		oGame TheGame;







		public Form1()
		{
			InitializeComponent();


			this.TheGame = new oGame();
			this.TheGame.Parent = this;
			this.TheGame.SizeChanged += new EventHandler(this.TheGame_SizeChanged);



		}

		private void Form1_Load(object sender, EventArgs e)
		{
			



			this.TheGame.Refresh();
		}

		private void TheGame_SizeChanged(object sender, EventArgs e)
		{
			this.RefreshSize();
		}





		public void RefreshSize()
		{
			this.TheGame.Top = 1;
			this.TheGame.Left = 1;
			this.Width = this.TheGame.Left + this.TheGame.Width + 17;
			this.Height = this.TheGame.Top + this.TheGame.Height + 40;


		}




	}
}
