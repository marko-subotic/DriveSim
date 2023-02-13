using SkiaSharp;
using System.IO;
using System.Windows.Input;
using DriveSimFR;
using System.Windows.Forms;
using System.Collections;
using SharpDX.XInput;

public class ChassisCanvas
{
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
    private SKCanvas canvas;
    private MyButton TankDriveButton;
    private MyButton XDriveButton;
    private Point dimensions;
    private Chassis chassis;
    Controller controller;

    public ChassisCanvas(ref SKCanvas canvas, Point dimensions)
    {
        this.canvas = canvas;
        this.dimensions = dimensions;
        TankDriveButton = new MyButton(DrawingUtils.alignCenter(dimensions.X / 4, dimensions.Y / 2, dimensions.X / 3, dimensions.Y / 10), "Tank Drive");
        XDriveButton = new MyButton(DrawingUtils.alignCenter(3 * dimensions.X / 4, dimensions.Y / 2, dimensions.X / 3, dimensions.Y / 10), "X-Drive");
        drawCanvas();
    }

    public void click_event(object sender, EventArgs e, PictureBox picture_box)
    {
        Vector mousePos = new Vector(picture_box.PointToClient(Control.MousePosition));
        switch (canvasState)
        {
            case state.intro:
                if (TankDriveButton.rectangle.contains(mousePos))
                {
                    canvasState = state.driving;
                    drivingMethod = method.tank;
                }
                else if (XDriveButton.rectangle.contains(mousePos))
                {
                    canvasState = state.driving;
                    drivingMethod = method.x;
                }
                break;
            case state.driving:
                MouseEventArgs me = (MouseEventArgs)e;
                bool left = me.Button == MouseButtons.Left;
                switch (drivingMethod)
                {
                    case method.tank:
                        break;
                    case method.x:
                        break;
                }
                break;

        }
        drawCanvas();
    }

    public void key_event(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            if (canvasState == state.driving)
            {
                canvasState = state.intro;
            }
        }
        drawCanvas();
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
    public void drawCanvas()
    {
        canvas.Clear(SKColors.Black);
        switch (canvasState)
        {
            case state.intro:
                drawIntroScreen();
                break;
            case state.driving:
                using (SKPaint paint = new SKPaint())
                {
                    switch (drivingMethod)
                    {
                        case method.tank:
                            paint.Color = SKColors.Blue;
                            paint.IsAntialias = true;
                            paint.StrokeWidth = 5;
                            paint.Style = SKPaintStyle.Stroke;
                           
                            break;
                        case method.x:

                            paint.Color = SKColors.Blue;
                            paint.IsAntialias = true;
                            paint.StrokeWidth = 5;
                            paint.Style = SKPaintStyle.Stroke;
                            
                            break;
                    }
                    paint.Color = SKColors.Red;
                    break;
                }
        }
    }
}