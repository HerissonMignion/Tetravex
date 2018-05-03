using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using cbFormObject;

namespace CbTetravex
{
	public class oWinForm
	{

		public enum ExitMethod
		{
			none,
			NewGameButton,
			CancelButton
		}
		private ExitMethod zzzTheExitMethod = ExitMethod.none;
		public ExitMethod TheExitMethod { get { return this.zzzTheExitMethod; } }



		private Form forme;
		private CbButton2 NewGameButton;
		private CbButton2 ExitButton;




		public void ShowDialog(Point PointToCenter)
		{
			this.RefreshSize();
			this.forme.Left = PointToCenter.X - (this.forme.Width / 2);
			this.forme.Top = PointToCenter.Y - (this.forme.Height / 2);
			
			this.forme.ShowDialog();
		}
		public void ShowDialog(Form FormToCenter)
		{
			Point CenterPoint = new Point(FormToCenter.Left + (FormToCenter.Width / 2), FormToCenter.Top + (FormToCenter.Height / 2));
			this.ShowDialog(CenterPoint);
		}



		public void RefreshSize()
		{
			//this.forme.Width = 300;
			//this.forme.Height = 200;

			int ButtonWidth = 275;
			int ButtonHeight = 85;
			int ButtonSpace = 5;

			this.NewGameButton.Top = ButtonSpace;
			this.NewGameButton.Left = ButtonSpace;
			this.NewGameButton.Width = ButtonWidth;
			this.NewGameButton.Height = ButtonHeight;

			this.ExitButton.Top = this.NewGameButton.Top + this.NewGameButton.Height + ButtonSpace;
			this.ExitButton.Left = ButtonSpace;
			this.ExitButton.Width = ButtonWidth;
			this.ExitButton.Height = ButtonHeight;

			this.forme.Width = this.NewGameButton.Left + this.NewGameButton.Width + ButtonSpace + 17;
			this.forme.Height = this.ExitButton.Top + this.ExitButton.Height + ButtonSpace + 39;



		}


		//void new()
		public oWinForm()
		{
			this.forme = new Form();
			this.forme.StartPosition = FormStartPosition.Manual;
			this.forme.Text = "CB Tetravex";
			this.forme.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.forme.MaximizeBox = false;
			this.forme.MinimizeBox = false;

			this.NewGameButton = new CbButton2();
			this.NewGameButton.Parent = this.forme;
			this.NewGameButton.Text = "New Game";
			this.NewGameButton.Font = new Font("consolas", 25);
			this.NewGameButton.MouseClick += new MouseEventHandler(this.NewGameButton_MouseClick);

			this.ExitButton = new CbButton2();
			this.ExitButton.Parent = this.forme;
			this.ExitButton.Text = "Exit";
			this.ExitButton.Font = new Font("consolas", 25);
			this.ExitButton.MouseClick += new MouseEventHandler(this.ExitButton_MouseClick);
			

		}

		private void NewGameButton_MouseClick(object sender, MouseEventArgs e)
		{
			this.zzzTheExitMethod = ExitMethod.NewGameButton;
			this.forme.Close();
		}
		private void ExitButton_MouseClick(object sender, MouseEventArgs e)
		{
			this.zzzTheExitMethod = ExitMethod.CancelButton;
			this.forme.Close();
		}




	}
}
