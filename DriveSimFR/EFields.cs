using DriveSimFR;
using SkiaSharp;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DriveSimFR
{
    public partial class EFields : Form
    {
        StaticElectricField field;
        SKSurface surface;
        SKCanvas canvas;
        StaticChargeCanvas canvas_board;
        public EFields()
        {
            field = new StaticElectricField(EFields.width, EFields.height,EFields.width/30,EFields.height/30, 30);
            SKImageInfo imageInfo = new SKImageInfo(EFields.width, EFields.height);
            surface = SKSurface.Create(imageInfo);
            canvas = surface.Canvas;
            InitializeComponent();
            canvas_board = new StaticChargeCanvas(ref canvas, new Point(EFields.width, EFields.height));
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