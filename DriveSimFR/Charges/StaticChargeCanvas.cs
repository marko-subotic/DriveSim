using SkiaSharp;
using System.IO;
using System.Windows.Input;
using DriveSimFR;
using System.Windows.Forms;
using System.Collections;
using DriveSim.Charges;
using DriveSim.Utils;

public class StaticChargeCanvas
{
    enum state
    {
        intro,
        drawing
    }
    enum method
    {
        slope,
        euler
    }
    private state canvasState = state.intro;
    private method drawingMethod;
    private SKCanvas canvas;
    private MyButton SlopeFieldButton;
    private MyButton EulerButton;
    private Point dimensions;
    private StaticElectricField field;

    public StaticChargeCanvas(ref SKCanvas canvas, Point dimensions)
    {
        this.canvas = canvas;
        this.dimensions = dimensions;
        SlopeFieldButton= new MyButton(DrawingUtils.alignCenter(dimensions.X/4, dimensions.Y/2, dimensions.X/3, dimensions.Y/10), "Slope Field Method");
        EulerButton = new MyButton(DrawingUtils.alignCenter(3*dimensions.X / 4, dimensions.Y / 2, dimensions.X / 3, dimensions.Y / 10), "Euler Method");
        field = new StaticElectricField(dimensions.X, dimensions.Y,dimensions.X/30, dimensions.Y/30, 30);
        drawCanvas();
    }

    public void click_event(object sender, EventArgs e, PictureBox picture_box)
    {
        Vector mousePos = new Vector(picture_box.PointToClient(Control.MousePosition));
        switch (canvasState)
        {
            case state.intro:
                if (EulerButton.rectangle.contains(mousePos)){
                    canvasState = state.drawing;
                    drawingMethod = method.euler;
                }else if (SlopeFieldButton.rectangle.contains(mousePos)){
                    canvasState = state.drawing;
                    drawingMethod = method.slope;
                }
                break;
            case state.drawing:
                MouseEventArgs me = (MouseEventArgs)e;
                bool left = me.Button == MouseButtons.Left;
                switch (drawingMethod){
                    case method.euler:
                        field.addChargeEulers(new PointCharge(mousePos, 100* (left ? 1 : -1)));
                        break;
                    case method.slope:
                        field.addChargeSlopeField(new PointCharge(mousePos, 100 * (left ? 1 : -1)));
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
            field.clearCharges();
            if (canvasState == state.drawing)
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
            DrawingUtils.drawButton(canvas, textPaint, textPaint, EulerButton);
            DrawingUtils.drawButton(canvas, textPaint, textPaint, SlopeFieldButton);
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
            case state.drawing:
                using (SKPaint paint = new SKPaint())
                {
                    switch (drawingMethod)
                    {
                        case method.euler:
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
                                            canvas.DrawLine(MathUtils.vecToPt(node.Value), MathUtils.vecToPt(node.Next.Value), paint);
                                        }
                                    }
                                }

                            }
                            break;
                        case method.slope:

                            paint.Color = SKColors.Blue;
                            paint.IsAntialias = true;
                            paint.StrokeWidth = 5;
                            paint.Style = SKPaintStyle.Stroke;
                            Vector[,,] electricFields = field.getElectricFields();
                            for (int r = 0; r < electricFields.GetLength(0); r++)
                            {
                                for (int c = 0; c < electricFields.GetLength(1); c++)
                                {
                                    Vector p1 = new Vector(electricFields[r, c, 0]);
                                    Vector p2 = p1 + electricFields[r, c, 1];
                                    canvas.DrawLine(MathUtils.vecToPt(p1), MathUtils.vecToPt(p2), paint);
                                }
                            }
                            break;
                    }
                    paint.Color = SKColors.Red;
                    foreach (PointCharge charge in field.getCharges())
                    {
                        canvas.DrawCircle(MathUtils.vecToPt(charge.location), 10, paint);

                    }
                    break;
                }
        }
    }
}