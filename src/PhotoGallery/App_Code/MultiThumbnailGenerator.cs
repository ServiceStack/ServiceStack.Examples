using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public class MultiThumbnailGenerator : IDisposable {
    /* common variables */
    private static int imageDimension = 200;
    private static float maxMultiplier = 0.7f;
    private static float maxDimension = (float)imageDimension * maxMultiplier;
    private static float thinLineWidth = 0.6f;
    private static float thickLineWidth = 2.0f;
    private static float maxAngle = 25f;

    /* instance variables */
    private Random random = new Random();
    private Bitmap surfaceBitmap = new Bitmap(imageDimension, imageDimension);
    private Graphics surfaceGraphics;
    private RectangleF? surfaceBounds;

    public MultiThumbnailGenerator() {
        surfaceGraphics = Graphics.FromImage(surfaceBitmap);
        surfaceGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        surfaceGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
    }

    public void AddImage(Image image) {
        // scale image
        float widthMultiplier = maxDimension / (float)image.Width;
        float heightMultiplier = maxDimension / (float)image.Height;
        float minMultiplier = Math.Min(Math.Min(widthMultiplier, heightMultiplier), 1f);
        float newWidth = minMultiplier * (float)image.Width;
        float newHeight = minMultiplier * (float)image.Height;

        // choose a random translation & rotation
        float angle = GetRandomAngle();
        PointF newDimensions = RotateAndFindNewDimensions(new PointF(newWidth, newHeight), angle);
        PointF offset = GetRandomOffset(newDimensions);
        surfaceGraphics.TranslateTransform(offset.X, offset.Y);
        surfaceGraphics.RotateTransform(angle);

        // draw borders + image
        DrawRectangle(Brushes.White, newWidth, newHeight, thickLineWidth);
        DrawRectangle(Brushes.Black, newWidth, newHeight, thinLineWidth);
        surfaceGraphics.DrawImage(image, -newWidth / 2f, -newHeight / 2f, newWidth, newHeight);

        // calculate new image boundaries
        RectangleF newBounds = new RectangleF(
            offset.X - newDimensions.X / 2f,
            offset.Y - newDimensions.Y / 2f,
            newDimensions.X,
            newDimensions.Y);

        if (surfaceBounds.HasValue) {
            surfaceBounds = Union(surfaceBounds.Value, newBounds);
        } else {
            surfaceBounds = newBounds;
        }

        surfaceGraphics.ResetTransform();
    }

    private void DrawRectangle(Brush brush, float width, float height, float lineWidth) {
        surfaceGraphics.FillRectangle(brush,
            -width / 2f - lineWidth,
            -height / 2f - lineWidth,
            width + 2f * lineWidth,
            height + 2f * lineWidth);
    }

    private static PointF RotateAndFindNewDimensions(PointF point, float angle) {
        using (Matrix matrix = new Matrix()) {
            PointF[] points = new PointF[] {
                    new PointF(0f, 0f),
                    new PointF(0f, point.Y),
                    new PointF(point.X, 0f),
                    new PointF(point.X, point.Y)
                };

            matrix.Rotate(angle);
            matrix.TransformPoints(points);
            float minX = points[0].X;
            float maxX = minX;
            float minY = points[0].Y;
            float maxY = minY;
            for (int i = 1; i < 4; i++) {
                minX = Math.Min(minX, points[i].X);
                maxX = Math.Max(maxX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
                maxY = Math.Max(maxY, points[i].Y);
            }

            return new PointF(maxX - minX, maxY - minY);
        }
    }

    private static RectangleF Union(RectangleF oldBounds, RectangleF newBounds) {
        float left = Math.Min(oldBounds.Left, newBounds.Left);
        float top = Math.Min(oldBounds.Top, newBounds.Top);
        float right = Math.Max(oldBounds.Right, newBounds.Right);
        float bottom = Math.Max(oldBounds.Bottom, newBounds.Bottom);
        return new RectangleF(left, top, right - left, bottom - top);
    }

    private float GetRandomAngle() {
        float r = (float)random.NextDouble();
        return (r * 2f - 1f) * maxAngle;
    }

    private PointF GetRandomOffset(PointF dimensions) {
        float deltaX = (float)imageDimension - dimensions.X - 2f * (thinLineWidth + 1f);
        float deltaY = (float)imageDimension - dimensions.Y - 2f * (thinLineWidth + 1f);

        float rX = (float)random.NextDouble();
        float rY = (float)random.NextDouble();

        float newX = (deltaX * rX) + (dimensions.X / 2f) + thinLineWidth + 1f;
        float newY = (deltaY * rY) + (dimensions.Y / 2f) + thinLineWidth + 1f;

        return new PointF(newX, newY);
    }

    public void Dispose() {
        surfaceGraphics.Dispose();
        surfaceBitmap.Dispose();
    }

    public void WritePngToStream(Stream outStream) {
        using (Bitmap outBitmap = new Bitmap(imageDimension, imageDimension)) {
            using (Graphics outGraphics = Graphics.FromImage(outBitmap)) {

                outGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                outGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // center the image
                RectangleF bounds = surfaceBounds ?? new RectangleF();
                float deltaX = (imageDimension - bounds.Width) / 2f - bounds.Left;
                float deltaY = (imageDimension - bounds.Height) / 2f - bounds.Top;
                outGraphics.DrawImage(surfaceBitmap, new PointF(deltaX, deltaY));

                outBitmap.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }

}
