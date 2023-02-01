using SkiaSharp;
using System.IO;
using System.Windows.Input;

public abstract class Canvas
{
    protected SKImageInfo surfaceDims;
    protected SKSurface surface;
    protected SKCanvas canvas;
    protected SKImage image;
    protected SKData data;
    protected MemoryStream mStream;
    protected SKBitmap bm;

}