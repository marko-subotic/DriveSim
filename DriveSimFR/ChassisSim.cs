using DriveSimFR;
using SharpDX.XInput;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using DriveSim.Utils;

namespace DriveSim
{
    public partial class ChassisSim : Form
    {
        SKSurface surface;
        SKCanvas canvas;
        SKPaint paint;
        enum state
        {
            intro,
            driving
        }
        enum method
        {
            tank,
            x
        }
        private state canvasState = state.intro;
        private method drivingMethod;
        private MyButton TankDriveButton;
        private MyButton XDriveButton;
        private MyButton DebugString;
        private Point dimensions = new Point(width, height);
        private Chassis chassis;
        private Controller controller;
        private bool connected;
        private int radius = 100;
        private int strokewidth = 5;
        Thread stepper;
        Thread drawer;
        public ChassisSim()
        {
            SKImageInfo imageInfo = new SKImageInfo(EFields.width, EFields.height);
            surface = SKSurface.Create(imageInfo);
            canvas = surface.Canvas;
            InitializeComponent();
            TankDriveButton = new MyButton(DrawingUtils.alignCenter(dimensions.X / 4, dimensions.Y / 2, dimensions.X / 3, dimensions.Y / 10), "Tank Drive");
            XDriveButton = new MyButton(DrawingUtils.alignCenter(3 * dimensions.X / 4, dimensions.Y / 2, dimensions.X / 3, dimensions.Y / 10), "X-Drive");
            paint = new SKPaint();
            drawCanvas();

            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;
            sendFrame();


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            
            Vector mousePos = new Vector(pictureBox1.PointToClient(Control.MousePosition));
            switch (canvasState)
            {
                case state.intro:
                    if (TankDriveButton.rectangle.contains(mousePos))
                    {
                        canvasState = state.driving;
                        drivingMethod = method.tank;
                        chassis = new TankChassis(radius, new Vector(width/2, height/2), strokewidth, 4.0, 300, .00005, .002, 5);
                    }
                    else if (XDriveButton.rectangle.contains(mousePos))
                    {
                        canvasState = state.driving;
                        drivingMethod = method.x;
                        chassis = new XChassis(radius, new Vector(width/2, height/2), strokewidth, 4.0,200, .0001, .005, 1);
                    }
                    drawCanvas();
                    break;
            }
            sendFrame();

        }

        private void KeyUpMethod(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (canvasState == state.driving)
                {
                    canvasState = state.intro;
                }
            }
            drawCanvas();
            sendFrame();

        }
        public void drawCanvas()
        {
            switch (canvasState)
            {
                case (state.driving):
                    stepper = new Thread(stepMethod);
                    drawer = new Thread(drawDrivingCanvas);
                    drawer.Start();
                    stepper.Start();
                    break;
                case (state.intro):
                    drawIntroScreen();
                    break;
            }
        }
        public void drawIntroScreen()
        {
            canvas.Clear(SKColors.Black);
            using (SKPaint textPaint = new SKPaint())
            {
                textPaint.StrokeWidth = 2;
                textPaint.Color = SKColors.White;
                textPaint.Style = SKPaintStyle.Stroke;
                textPaint.TextAlign = SKTextAlign.Center;
                textPaint.TextSize = 64.0f;
                textPaint.IsAntialias = true;
                DrawingUtils.drawButton(canvas, textPaint, textPaint, TankDriveButton);
                DrawingUtils.drawButton(canvas, textPaint, textPaint, XDriveButton);
            }
        }

        public void drawDrivingCanvas()
        {
            
            while (canvasState == state.driving)
            {
                canvas.Clear(SKColors.Black);
                paint.Color = SKColors.Blue;
                paint.IsAntialias = true;
                paint.StrokeWidth = strokewidth;
                paint.Style = SKPaintStyle.Stroke;
                Vector[,] wheelPos = chassis.getGlobalWheelPositions();
                Vector[] body = chassis.getBody();
                Vector[] headerLine = chassis.getHeaderLine();
                for (int i = 0; i < body.Length;i++)
                {
                    canvas.DrawLine(MathUtils.vecToPt(body[i]), MathUtils.vecToPt(body[(i+1)%(body.Length)]), paint);
                }
                paint.Color = SKColors.White;
                for (int i = 0; i < wheelPos.GetLength(0); i++)
                {
                    for(int j = 0; j < wheelPos.GetLength(1); j++)
                    {
                        canvas.DrawLine(MathUtils.vecToPt(wheelPos[i,j]), MathUtils.vecToPt(wheelPos[i,(j+1) % (wheelPos.GetLength(1))]), paint);
                    }
                }
                paint.Color = SKColors.Red;
                canvas.DrawLine(Utils.MathUtils.vecToPt(headerLine[0]), Utils.MathUtils.vecToPt(headerLine[1]), paint);
                sendFrame();
                Thread.Sleep(20);

            }

        }

        public void stepMethod()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            double prevTime = 0;
            bool beyblading = false;
            bool prevA = controller.GetState().Gamepad.Buttons == GamepadButtonFlags.A;
            while (canvasState == state.driving && connected)
            {
                if (drivingMethod == method.tank)
                {
                    chassis.inputWheelPowers(ControlUtils.wheelPowsFromJoyStickTank(controller.GetState().Gamepad.LeftThumbY, controller.GetState().Gamepad.RightThumbY));
                }
                else
                {
                    if (prevA == (controller.GetState().Gamepad.Buttons == GamepadButtonFlags.A))
                    {
                        beyblading = !beyblading;
                    }
                    prevA = controller.GetState().Gamepad.Buttons == GamepadButtonFlags.A;
                    if (beyblading)
                    {
                        chassis.inputWheelPowers(ControlUtils.wheelPowsFromJoyStickX(controller.GetState().Gamepad.LeftThumbY, controller.GetState().Gamepad.RightThumbY, controller.GetState().Gamepad.LeftThumbX));
                    }
                    else
                    {
                        chassis.inputWheelPowers(ControlUtils.wheelPowsFromJoyStickX(controller.GetState().Gamepad.LeftThumbY, controller.GetState().Gamepad.RightThumbY, controller.GetState().Gamepad.LeftThumbX));
                    }
                }
                chassis.step(timer.ElapsedMilliseconds / 1000.0 - prevTime);
                prevTime = timer.ElapsedMilliseconds/1000.0;
                Thread.Sleep(1);
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
