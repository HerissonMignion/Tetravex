using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CbTetravex
{
	static class Program
	{
		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);


			module.MainForm = new Form1();
			Application.Run(module.MainForm);
		}
	}
	static class module
	{
		public static Random rnd = new Random();

		public static Form MainForm;




		//optien la taille d'une chaine de text
		public static Size GetTextSize(string TheText, Font TheFont)
		{
			Bitmap img = new Bitmap(10, 10); //cette image sert juste a la creation d'un object graphics
			Graphics g = Graphics.FromImage(img);

			float TextHeight = g.MeasureString("NEBGREFCDpytqlkjhgfdb", TheFont).Height;
			float TextWidth = g.MeasureString(TheText, TheFont).Width;
			Size TextSize = new Size((int)TextWidth, (int)TextHeight);

			g.Dispose();
			img.Dispose();
			return TextSize;
		}





		public static Color MultiplyLightLevel(Color TheColor, float MulValue)
		{
			Color rep = Color.Black;

			float oRed = (float)TheColor.R;
			float oGreen = (float)TheColor.G;
			float oBlue = (float)TheColor.B;

			int rRed = (int)(oRed * MulValue);
			int rGreen = (int)(oGreen * MulValue);
			int rBlue = (int)(oBlue * MulValue);

			if (rRed > 255) { rRed = 255; }
			if (rGreen > 255) { rGreen = 255; }
			if (rBlue > 255) { rBlue = 255; }

			rep = Color.FromArgb(rRed, rGreen, rBlue);
			return rep;
		}
		public static Color AdjustBrightness(Color TheColor, float MulValue)
		{
			Color rep = Color.Black;

			float dRed = 255f - (float)TheColor.R;
			float dGreen = 255f - (float)TheColor.G;
			float dBlue = 255f - (float)TheColor.B;
			if (dRed < 0f) { dRed = 0f; }
			if (dGreen < 0f) { dGreen = 0f; }
			if (dBlue < 0f) { dBlue = 0f; }

			float ndRed = dRed * MulValue;
			float ndGreen = dGreen * MulValue;
			float ndBlue = dBlue * MulValue;

			int rRed = (int)(255f - ndRed);
			int rGreen = (int)(255f - ndGreen);
			int rBlue = (int)(255f - ndBlue);

			rep = Color.FromArgb(rRed, rGreen, rBlue);
			return rep;
		}

		public static void wdebug(string text) { System.Diagnostics.Debug.WriteLine(text); }
		
	}
}
