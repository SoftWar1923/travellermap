﻿using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Maps.Rendering
{
    public interface MGraphics : IDisposable
    {
        XSmoothingMode SmoothingMode { get; set; }
        Graphics Graphics { get; }

        void ScaleTransform(double scaleXY);
        void ScaleTransform(double scaleX, double scaleY);
        void TranslateTransform(double dx, double dy);
        void RotateTransform(double angle);
        void MultiplyTransform(XMatrix m);

        void IntersectClip(XGraphicsPath path);
        void IntersectClip(RectangleF rect);

        void DrawLine(XPen pen, double x1, double y1, double x2, double y2);
        void DrawLine(XPen pen, PointF pt1, PointF pt2);
        void DrawLines(XPen pen, XPoint[] points);
        void DrawPath(XPen pen, XGraphicsPath path);
        void DrawPath(XSolidBrush brush, XGraphicsPath path);
        void DrawCurve(XPen pen, PointF[] points, double tension = 0.5);
        void DrawClosedCurve(XPen pen, PointF[] points, double tension = 0.5);
        void DrawClosedCurve(XSolidBrush brush, PointF[] points, double tension = 0.5);
        void DrawRectangle(XPen pen, double x, double y, double width, double height);
        void DrawRectangle(XSolidBrush brush, double x, double y, double width, double height);
        void DrawRectangle(XSolidBrush brush, RectangleF rect);
        void DrawEllipse(XPen pen, double x, double y, double width, double height);
        void DrawEllipse(XSolidBrush brush, double x, double y, double width, double height);
        void DrawEllipse(XPen pen, XSolidBrush brush, double x, double y, double width, double height);
        void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle);
        void DrawImage(XImage image, double x, double y, double width, double height);
        void DrawImage(XImage image, RectangleF destRect, RectangleF srcRect, XGraphicsUnit srcUnit);

        XSize MeasureString(string text, XFont font);
        void DrawString(string s, XFont font, XSolidBrush brush, double x, double y, XStringFormat format);
        void DrawStringWithAlignment(string s, XFont font, XSolidBrush brush, RectangleF textBounds, XParagraphAlignment alignment);

        MGraphicsState Save();
        void Restore(MGraphicsState state);
    }
    public interface MGraphicsState { }
    public class MTextFormatter
    {
        private MGraphics g;
        public MTextFormatter(MGraphics g) { this.g = g; }
        public XParagraphAlignment Alignment { get; set; }
        public void DrawString(string s, XFont font, XSolidBrush brush, RectangleF textBounds)
        {
            g.DrawStringWithAlignment(s, font, brush, textBounds, Alignment);
        }
    }

    internal class MXGraphics : MGraphics
    {
        private XGraphics g;
        public MXGraphics(XGraphics g) { this.g = g; }

        public XSmoothingMode SmoothingMode { get { return g.SmoothingMode; } set { g.SmoothingMode = value; } }
        public Graphics Graphics { get { return g.Graphics; } }

        public void ScaleTransform(double scaleXY) { g.ScaleTransform(scaleXY); }
        public void ScaleTransform(double scaleX, double scaleY) { g.ScaleTransform(scaleX, scaleY); }
        public void TranslateTransform(double dx, double dy) { g.TranslateTransform(dx, dy); }
        public void RotateTransform(double angle) { g.RotateTransform(angle); }
        public void MultiplyTransform(XMatrix m) { g.MultiplyTransform(m); }

        public void IntersectClip(XGraphicsPath path) { g.IntersectClip(path); }
        public void IntersectClip(RectangleF rect) { g.IntersectClip(rect); }

        public void DrawLine(XPen pen, double x1, double y1, double x2, double y2) { g.DrawLine(pen, x1, y1, x2, y2); }
        public void DrawLine(XPen pen, PointF pt1, PointF pt2) { g.DrawLine(pen, pt1, pt2); }
        public void DrawLines(XPen pen, XPoint[] points) { g.DrawLines(pen, points); }
        public void DrawPath(XPen pen, XGraphicsPath path) { g.DrawPath(pen, path); }
        public void DrawPath(XSolidBrush brush, XGraphicsPath path) { g.DrawPath(brush, path); }
        public void DrawCurve(XPen pen, PointF[] points, double tension) { g.DrawCurve(pen, points, tension); }
        public void DrawClosedCurve(XPen pen, PointF[] points, double tension) { g.DrawClosedCurve(pen, points, tension); }
        public void DrawClosedCurve(XSolidBrush brush, PointF[] points, double tension) { g.DrawClosedCurve(brush, points, XFillMode.Alternate, tension); }
        public void DrawRectangle(XPen pen, double x, double y, double width, double height) { g.DrawRectangle(pen, x, y, width, height); }
        public void DrawRectangle(XSolidBrush brush, double x, double y, double width, double height) { g.DrawRectangle(brush, x, y, width, height); }
        public void DrawRectangle(XSolidBrush brush, RectangleF rect) { g.DrawRectangle(brush, rect); }
        public void DrawEllipse(XPen pen, double x, double y, double width, double height) { g.DrawEllipse(pen, x, y, width, height); }
        public void DrawEllipse(XSolidBrush brush, double x, double y, double width, double height) { g.DrawEllipse(brush, x, y, width, height); }
        public void DrawEllipse(XPen pen, XSolidBrush brush, double x, double y, double width, double height) { g.DrawEllipse(pen, brush, x, y, width, height); }
        public void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle) { g.DrawArc(pen, x, y, width, height, startAngle, sweepAngle); }
        public void DrawImage(XImage image, double x, double y, double width, double height) { g.DrawImage(image, x, y, width, height); }
        public void DrawImage(XImage image, RectangleF destRect, RectangleF srcRect, XGraphicsUnit srcUnit) { g.DrawImage(image, destRect, srcRect, srcUnit); }

        public XSize MeasureString(string text, XFont font) { return g.MeasureString(text, font); }
        public void DrawString(string s, XFont font, XSolidBrush brush, double x, double y, XStringFormat format) { g.DrawString(s, font, brush, x, y, format); }
        public void DrawStringWithAlignment(string s, XFont font, XSolidBrush brush, RectangleF textBounds, XParagraphAlignment alignment)
        {
            XTextFormatter format = new XTextFormatter(g);
            format.Alignment = alignment;
            format.DrawString(s, font, brush, textBounds);
        }

        public MGraphicsState Save() { return new MXGraphicsState(g.Save()); }
        public void Restore(MGraphicsState state) { g.Restore(((MXGraphicsState)state).state); }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    g.Dispose();
                }
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MXGraphics() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        private class MXGraphicsState : MGraphicsState
        {
            public XGraphicsState state;
            public MXGraphicsState(XGraphicsState state) { this.state = state; }
        }

    }

    internal class SVGGraphics : MGraphics
    {
        public const string MediaTypeName = "image/svg+xml";

        private class Element
        {
            public string name;
            public Dictionary<string, string> attributes = new Dictionary<string, string>();
            public List<Element> children = new List<Element>();

            public Element(string name) { this.name = name; }
            public Element(string name, string transform) { this.name = name; attributes["transform"] = transform; }

            public Element Append(Element child) { children.Add(child); return child; }

            public void Serialize(TextWriter b)
            {
                b.Write("<");
                b.Write(name);
                foreach (KeyValuePair<string, string> entry in attributes)
                {
                    b.Write(" ");
                    b.Write(entry.Key);
                    b.Write("=\"");
                    b.Write(System.Security.SecurityElement.Escape(entry.Value));
                    b.Write("\"");
                }
                if (children.Count == 0)
                {
                    b.Write("/>");
                    return;
                }
                b.Write(">");
                foreach (var child in children)
                {
                    child.Serialize(b);
                }
                b.Write("</");
                b.Write(name);
                b.Write(">");
            }

            public void Set(string name, string value) { attributes[name] = value; }
            public void Set(string name, double value) { attributes[name] = value.ToString(CultureInfo.InvariantCulture); }
            public void Set(string name, XColor color) {
                if (color.IsEmpty || color.A == 0)
                    attributes[name] = "None";
                else if (color.A != 1.0)
                    attributes[name] = String.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, color.A);
                else
                    attributes[name] = String.Format("rgb({0},{1},{2})", color.R, color.G, color.B);
            }

            public void Apply(XPen pen)
            {
                if (pen == null)
                {
                    Set("stroke", XColor.Empty);
                }
                else
                {
                    Set("stroke", pen.Color);
                    Set("stroke-width", pen.Width);
                }
            }
            public void Apply(XSolidBrush brush)
            {
                Set("fill", brush == null ? XColor.Empty : brush.Color);
            }
            public void Apply(XPen pen, XSolidBrush brush)
            {
                Apply(pen);
                Apply(brush);
            }
        }

        public void Serialize(TextWriter writer)
        {
            writer.WriteLine("<?xml version = \"1.0\" encoding=\"utf-8\"?>");
            writer.Write(String.Format("<svg version=\"1.1\" baseProfile=\"full\" xmlns=\"http://www.w3.org/2000/svg\" " +
                                "width=\"{0}\" height=\"{1}\">",
                width, height));
            root.Serialize(writer);
            writer.Write("</svg>");
            writer.Flush();
        }

        private double width;
        private double height;
        private Element root = new Element("g");
        private Stack<Element> stack = new Stack<Element>();

        private Element Current {  get { return stack.Peek(); } }

        private Element Open(Element element)
        {
            stack.Push(Current.Append(element));
            return element;
        }
        private Element Append(Element element)
        {
            return Current.Append(element);
        }

        public SVGGraphics(double width, double height)
        {
            this.width = width;
            this.height = height;
            stack.Push(root);
        }

        Graphics MGraphics.Graphics { get { return null; } }
        XSmoothingMode MGraphics.SmoothingMode { get; set; }

        #region Drawing

        public void DrawLine(XPen pen, double x1, double y1, double x2, double y2)
        {
            var e = Append(new Element("line"));
            e.Set("x1", x1);
            e.Set("y1", y1);
            e.Set("x2", x2);
            e.Set("y2", y2);
            e.Apply(pen);
        }

        public void DrawLines(XPen pen, XPoint[] points)
        {
            var e = Append(new Element("polyline"));
            e.Set("points", string.Join(" ", points.Select(pt => String.Format("{0},{1}", pt.X, pt.Y))));
            e.Apply(pen);
        }

        public void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            // TODO
        }

        public void DrawPath(XPen pen, XSolidBrush brush, XGraphicsPath path)
        {
            // TODO
        }

        public void DrawCurve(XPen pen, PointF[] points, double tension)
        {
            // TODO
        }

        public void DrawClosedCurve(XPen pen, XSolidBrush brush, PointF[] points, double tension)
        {
            // TODO
        }

        public void DrawRectangle(XPen pen, XSolidBrush brush, double x, double y, double width, double height)
        {
            var e = Append(new Element("rect"));
            e.Set("x", x);
            e.Set("y", y);
            e.Set("width", width);
            e.Set("height", height);
            e.Apply(pen, brush);
        }

        public void DrawEllipse(XPen pen, XSolidBrush brush, double x, double y, double width, double height)
        {
            var e = Append(new Element("ellipse"));
            e.Set("cx", x + width / 2);
            e.Set("cy", y + height / 2);
            e.Set("rx", width / 2);
            e.Set("ry", height / 2);
            e.Apply(pen, brush);
        }
        #endregion

        #region Images - TODO
        public void DrawImage(XImage image, RectangleF destRect, RectangleF srcRect, XGraphicsUnit srcUnit)
        {
        }

        public void DrawImage(XImage image, double x, double y, double width, double height)
        {
        }
        #endregion

        #region Clipping - TODO
        public void IntersectClip(RectangleF rect)
        {
        }

        public void IntersectClip(XGraphicsPath path)
        {
        }
        #endregion

        #region Text - TODO
        public XSize MeasureString(string text, XFont font)
        {
            return new XSize(0, 0);
        }
        public void DrawString(string s, XFont font, XSolidBrush brush, double x, double y, XStringFormat format)
        {
        }

        public void DrawStringWithAlignment(string s, XFont font, XSolidBrush brush, RectangleF textBounds, XParagraphAlignment alignment)
        {
        }
        #endregion

        #region Transforms - DONE but could optimize
        // TODO: If last element added (not on stack) is a <g> then concatenate transforms
        public void ScaleTransform(double scaleX, double scaleY)
        {
            Open(new Element("g", String.Format("scale({0} {1})", scaleX, scaleY)));
        }
        public void TranslateTransform(double dx, double dy)
        {
            Open(new Element("g", String.Format("translate({0} {1})", dx, dy)));
        }
        public void RotateTransform(double angle)
        {
            Open(new Element("g", String.Format("rotate({0})", angle)));
        }
        public void MultiplyTransform(XMatrix m)
        {
            // TODO: Verify matrix order
            Open(new Element("g", String.Format("matrix({0} {1} {2} {3} {4} {5})", 
                m.M11, m.M12, m.M21, m.M22, m.OffsetX, m.OffsetY)));
        }
        #endregion

        #region State - DONE
        public MGraphicsState Save()
        {
            var state = new State(new Element("g"));
            Open(state.element);
            return state;
        }
        public void Restore(MGraphicsState state)
        {
            while (stack.Peek() != ((State)state).element)
                stack.Pop();
            stack.Pop();
        }

        private class State : MGraphicsState
        {
            public Element element;
            public State(Element e) { element = e; }
        }
        #endregion

        #region Relay Methods
        public void DrawLine(XPen pen, PointF pt1, PointF pt2)
        {
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }
        public void DrawPath(XSolidBrush brush, XGraphicsPath path)
        {
            DrawPath(null, brush, path);
        }
        public void DrawPath(XPen pen, XGraphicsPath path)
        {
            DrawPath(pen, null, path);
        }
        public void DrawRectangle(XSolidBrush brush, RectangleF rect)
        {
            DrawRectangle(null, brush, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void DrawRectangle(XSolidBrush brush, double x, double y, double width, double height)
        {
            DrawRectangle(null, brush, x, y, width, height);
        }
        public void DrawRectangle(XPen pen, double x, double y, double width, double height)
        {
            DrawRectangle(pen, null, x, y, width, height);
        }
        public void DrawEllipse(XSolidBrush brush, double x, double y, double width, double height)
        {
            DrawEllipse(null, brush, x, y, width, height);
        }
        public void DrawEllipse(XPen pen, double x, double y, double width, double height)
        {
            DrawEllipse(pen, null, x, y, width, height);
        }
        public void DrawClosedCurve(XSolidBrush brush, PointF[] points, double tension)
        {
            DrawClosedCurve(null, brush, points, tension);
        }
        public void DrawClosedCurve(XPen pen, PointF[] points, double tension)
        {
            DrawClosedCurve(pen, null, points, tension);
        }

        public void ScaleTransform(double scaleXY)
        {
            ScaleTransform(scaleXY, scaleXY);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SVGGraphics() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}