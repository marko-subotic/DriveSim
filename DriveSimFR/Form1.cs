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