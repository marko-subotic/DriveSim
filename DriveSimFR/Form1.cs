using DriveSimFR;
using SkiaSharp;

namespace DriveSimFR
{
    public partial class Form1 : Form
    {
        StaticElectricField field;
        SKSurface surface;
        SKCanvas canvas;

        public Form1()
        {
            field = new StaticElectricField(Form1.width, Form1.height,Form1.width/30,Form1.height/30);
            SKImageInfo imageInfo = new SKImageInfo(Form1.width, Form1.height);
            surface = SKSurface.Create(imageInfo);
            canvas = surface.Canvas;
            InitializeComponent();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SKImageInfo imageInfo = new SKImageInfo(Form1.width, Form1.height);
            field.addCharge(new PointCharge(new Vector(pictureBox1.PointToClient(System.Windows.Forms.Control.MousePosition)), 100));
            canvas.Clear(SKColors.Black);
            using (SKPaint paint = new SKPaint())
            {
                paint.Color = SKColors.Blue;
                paint.IsAntialias = true;
                paint.StrokeWidth = 2;
                paint.Style = SKPaintStyle.Stroke;
                Vector[,,] electricFields = field.getElectricFields();
                for(int r = 0;r< electricFields.GetLength(0);r++)
                {
                    for(int c = 0; c < electricFields.GetLength(1); c++)
                    {
                        Vector p1 = new Vector(electricFields[r, c, 0]);
                        Vector p2 = p1 + electricFields[r, c, 1];
                        canvas.DrawLine(Utils.vecToPt(p1), Utils.vecToPt(p2), paint);
                    }
                }
                paint.Color = SKColors.Red;
                foreach(PointCharge charge in field.getCharges())
                {
                    canvas.DrawCircle(Utils.vecToPt(charge.location), 10, paint);

                }
                using (SKImage image = surface.Snapshot())
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (MemoryStream mStream = new MemoryStream(data.ToArray()))
                {
                    Bitmap bm = new Bitmap(mStream, false);
                    pictureBox1.Image = bm;
                }
            }
        }
    }
}