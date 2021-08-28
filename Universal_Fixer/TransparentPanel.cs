using System.Windows.Forms;

namespace Universal_Fixer
{
	public class TransparentPanel : Panel
	{
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 32;
				return createParams;
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}
	}
}
