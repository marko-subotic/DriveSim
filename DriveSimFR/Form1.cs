using DriveSimFR;
using SkiaSharp;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DriveSimFR
{
    public partial class Form1 : Form
    {
        StaticElectricField field;
        SKSurface surface;
        SKCanvas canvas;
        StaticChargeCanvas canvas_board;
        public Form1()
        {
            field = new StaticElectricField(Form1.width, Form1.height,Form1.width/30,Form1.height/30, 30);
            SKImageInfo imageInfo = new SKImageInfo(Form1.width, Form1.height);
            surface = SKSurface.Create(imageInfo);
            canvas = surface.Canvas;
            InitializeComponent();
            canvas_board = new StaticChargeCanvas(ref canvas, new Point(Form1.width, Form1.height));
            using (SKImage image = surface.Snapshot())
            using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (MemoryStream mStream = new MemoryStream(data.ToArray()))
            {
                Bitmap bm = new Bitmap(mStream, false);
                pictureBox1.Image = bm;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            canvas_board.click_event(sender, e, pictureBox1);
            sendFrame();

        }

        private void pictureBox1_KeyUp(object sender, KeyEventArgs e)
        {
            canvas_board.key_event(sender, e);
            sendFrame();

        }
        private void drawChargeSlope(SKCanvas canvas, bool positive)
        {
            field.addChargeSlopeField(new PointCharge(new Vector(pictureBox1.PointToClient(Control.MousePosition)), 100 * (positive ? 1 : -1)));
            using (SKPaint paint = new SKPaint())
            {
                paint.Color = SKColors.Blue;
                paint.IsAntialias = true;
                paint.StrokeWidth = 5;
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
                foreach (PointCharge charge in field.getCharges())
                {
                    canvas.DrawCircle(Utils.vecToPt(charge.location), 10, paint);

                }
            }
        }

        private void drawChargeEulers(SKCanvas canvas, bool positive)
        {
            SKImageInfo imageInfo = new SKImageInfo(Form1.width, Form1.height);

            field.addChargeEulers(new PointCharge(new Vector(pictureBox1.PointToClient(Control.MousePosition)), 100 * (positive ? 1: -1)));
            canvas.Clear(SKColors.Black);
            using (SKPaint paint = new SKPaint())
            {
                paint.Color = SKColors.Blue;
                paint.IsAntialias = true;
                paint.StrokeWidth = 5;
                paint.Style = SKPaintStyle.Stroke;
                ArrayList electricLines = field.getElectricLines();
                foreach (ArrayList chargeLines in electricLines)
                {
                    foreach (LinkedList<Vector> lines in chargeLines)
                    {
                        if (lines.Count > 0)
                        {
                            for (LinkedListNode<Vector> node = lines.First; node.Next != null; node = node.Next)
                            {
                                canvas.DrawLine(Utils.vecToPt(node.Value), Utils.vecToPt(node.Next.Value), paint);
                            }
                        }
                    }

                }
                paint.Color = SKColors.Red;
                foreach (PointCharge charge in field.getCharges())
                {
                    canvas.DrawCircle(Utils.vecToPt(charge.location), 10, paint);

                }
                
            }
        }
        private void sendFrame()
        {
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