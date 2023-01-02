using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace majiang
{
    internal enum typeMJ
    {
        TONG = 1,
        TIAO = 2,
        WAN = 3
    };

    public partial class Form1 : Form
    {
        private int pCenter = 10;
        private int pWidth = 4;
        private int pEdge = 5;
        private double Angle45 = Math.PI / 4;

        public class PointD
        {
            private double x;
            private double y;
            public double X => x;
            public double Y => y;
            public PointD(double xx, double yy)
            {
                x = xx;
                y = yy;
            }
            public PointD(PointD p)
            {
                x = p.X;
                y = p.Y;
            }
            public PointF PointF => new PointF((float)x, (float)y);
            public PointD MirrorXY => new PointD(y, x);
            public PointD MirrorY => new PointD(-x, y);

        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            DrawTong(pictureBox1, 1);
            DrawTong(pictureBox2, 2);
            DrawTong(pictureBox3, 3);
            DrawTong(pictureBox4, 4);
            DrawTong(pictureBox5, 5);
            DrawTong(pictureBox6, 6);
            DrawTong(pictureBox7, 7);
            DrawTong(pictureBox8, 8);
            DrawTong(pictureBox9, 9);

            DrawTiao(pictureBox10, 1);
            DrawTiao(pictureBox11, 2);
            DrawTiao(pictureBox12, 3);
            DrawTiao(pictureBox13, 4);
            DrawTiao(pictureBox14, 5);
            DrawTiao(pictureBox15, 6);
            DrawTiao(pictureBox16, 7);
            DrawTiao(pictureBox17, 8);
            DrawTiao(pictureBox18, 9);

            DrawWan(pictureBox19, 1);
            DrawWan(pictureBox20, 2);
            DrawWan(pictureBox21, 3);
            DrawWan(pictureBox22, 4);
            DrawWan(pictureBox23, 5);
            DrawWan(pictureBox24, 6);
            DrawWan(pictureBox25, 7);
            DrawWan(pictureBox26, 8);
            DrawWan(pictureBox27, 9);
        }
        private PointD[] GetRectCenter(PointD c, double r, int n)
        {
            double alpha = 0, theta = 0;
            PointD[] pts = new PointD[n];
            pts[0] = c;
            theta = GetAngle(n);
            alpha = theta / 2;
            if (n == 2) alpha = 0;
            for (int i = 0; i < n; i++)
                pts[i] = new PointD(c.X + r * Math.Sin(alpha + i * theta), c.Y + r * Math.Cos(alpha + i * theta));
            return pts;
        }
        private PointD[] GetCenter(PointD c, double r, int n)
        {
            double alpha = 0, theta = 0;
            PointD[] pts = new PointD[n];
            pts[0] = c;
            theta = GetAngle(n);
            alpha = theta / 2;
            if (n % 2 == 0 || n == 3)
            {//2,3,4,6,8
                if (n == 2) alpha = 0;
                for (int i = 0; i < n; i++)
                    pts[i] = new PointD(c.X + r * Math.Sin(alpha + i * theta), c.Y + r * Math.Cos(alpha + i * theta));
            }
            else
            {//1,5,7,9
                if (n > 1)
                {
                    theta = GetAngle(n - 1);
                    alpha = theta / 2;
                    for (int i = 1; i < n; i++)
                        pts[i] = new PointD(c.X + r * Math.Sin(alpha + i * theta), c.Y + r * Math.Cos(alpha + i * theta));
                }
            }
            return pts;
        }
        private double[] GetR(typeMJ type, Rectangle rect, int n, ref PointD[] c)
        {
            double r = Math.Min(rect.Width, rect.Height) / 2 - 4 * pEdge;
            double rr = r;
            c = new PointD[n];
            PointD cc = new PointD(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            switch (n)
            {
                case 2:
                    r = (rect.Height - 4 * pEdge) / 4;
                    rr = r;
                    break;
                case 3:
                    r = (rect.Width - 4 * pEdge) / 4;
                    rr = r / Math.Cos(Math.PI / 6);
                    break;
                case 4:
                    r = (rect.Width - 4 * pEdge) / 4;
                    rr = r * Math.Sqrt(2);
                    break;
                case 5:
                    r = (rect.Width / 2 - 2 * pEdge) / (1 + Math.Cos(Math.PI / 20) / Math.Cos(Math.PI * 3 / 10));
                    rr = 2 * r;
                    break;
                case 6:
                    r = (rect.Width - 4 * pEdge) / 6;
                    rr = 2 * r;
                    break;
                case 7:
                    r = (rect.Width - 4 * pEdge) / 6;
                    rr = 2 * r;
                    break;
                case 8:
                    r = (rect.Width / 2 - 2 * pEdge) / (1 + Math.Tan(Math.PI * 3 / 8));
                    rr = r / Math.Sin(Math.PI / 8);
                    break;
                case 9:
                    r = (rect.Width / 2 - 2 * pEdge) / (1 + Math.Tan(Math.PI * 3 / 8));
                    rr = r / Math.Sin(Math.PI / 8);
                    break;
            }
            if (type == typeMJ.TONG)
                c = GetCenter(cc, rr, n);
            else
                c = GetRectCenter(cc, rr, n);
            return new double[2] { r, rr };
        }
        private PointD AxisToScreen(PointD p, PointD c)
        {
            return new PointD(c.X + p.X, c.Y - p.Y);
        }
        private PointD[] AxisToScreen(PointD[] p, PointD c)
        {
            for (int i = 0; i < p.Length; i++)
                p[i] = AxisToScreen(p[i], c);
            return p;
        }
        private PointD[,] AxisToScreen(PointD[,] p, PointD c)
        {
            for (int j = 0; j < p.Rank; j++)
                for (int i = 0; i < p.Length / p.Rank; i++)
                    if (p[i, j] != null)
                        p[i, j] = AxisToScreen(p[i, j], c);
            return p;
        }
        private PointD AxisFromScreen(PointD p, PointD c)
        {
            return new PointD(p.X - c.X, c.Y - p.Y);
        }
        private PointD RotateDot(PointD p, PointD c, double theta)
        {
            p = AxisFromScreen(p, c);
            PointD pt = new PointD(p.X * Math.Cos(theta) - p.Y * Math.Sin(theta), p.X * Math.Sin(theta) + p.Y * Math.Cos(theta));
            return AxisToScreen(pt, c);
        }

        private PointD[] RotatePolygen(PointD[] p, PointD c, double theta)
        {
            PointD[] pt = new PointD[p.Length];
            for (int i = 0; i < p.Length; i++)
                pt[i] = RotateDot(p[i], c, theta);
            return pt;
        }
        private PointD[,] RotatePolygen(PointD[,] p, PointD c, double theta)
        {
            PointD[,] pt = new PointD[p.Length / p.Rank, p.Rank];
            for (int j = 0; j < p.Rank; j++)
                for (int i = 0; i < p.Length / p.Rank; i++)
                    pt[i, j] = RotateDot(p[i, j], c, theta);
            return pt;
        }
        private SizeF GetRectSize(Rectangle rect, int n)
        {
            double theta = Math.PI / n;
            float h = rect.Width / 2 - pEdge;
            float w = rect.Width / 3;
            if (n > 4)
            {
                theta = Math.PI / n / 2;
                w = rect.Width / (n % 5 + 4);
                h = h - pEdge;
                if (n == 5) h = h - pEdge;
                if (n == 8) h = h - 4 * pEdge;
            }
            else if (n > 3)
            {
                theta = Math.PI / (n + n / 2);
                w = h * (float)Math.Tan(theta);
                h = h - 3 * pEdge;
            }
            else if (n > 2)
            {
                theta = Math.PI / n / 2;
                w = h * (float)Math.Tan(theta);
                h = h - 2 * pEdge;
            }
            return new SizeF(w, h);
        }
        private SizeF GetDiamondSize(Rectangle rect, int n)
        {
            double theta = GetAngle(2 * n);
            float h = rect.Height / 2 - 2 * pEdge;
            float w = rect.Width / 3;
            if (n > 2)
            {
                h = rect.Width / 2 - pEdge;
                if (n > 4)
                {
                    theta = 1.6 * Math.PI / n;
                    w = h * (float)Math.Tan(theta) / 2;
                }
                else if (n > 3)
                {
                    theta = Math.PI / (n + n / 2);
                    w = h * (float)Math.Tan(theta);
                }
                else
                {
                    theta = Math.PI / n / 2;
                    w = h * (float)Math.Tan(theta);
                }
            }
            return new SizeF(w, h);
        }
        private PointD[,] GetSingleRect(Rectangle rect, PointD c, SizeF s, int n, ref double r)
        {
            //r = n > 2 ? s.Width / Math.Tan(Math.PI / n) / 2 : pEdge;
            if (n > 2)
                r = s.Width / Math.Tan(Math.PI / n) / 2;
            else
                r = pEdge;

            PointD[,] p = new PointD[4, 2];
            if (n % 4 == 0) //4 and 8
            {
                double d1 = r * Math.Cos(Angle45);
                double d2 = (r + s.Height) * Math.Cos(Angle45);
                double d3 = s.Width * Math.Sin(Angle45) / 2;
                p[0, 0] = new PointD(d1 + d3, d1 - d3);
                p[1, 0] = p[0, 0].MirrorXY;
                p[2, 0] = new PointD(d2 - d3, d2 + d3);
                p[3, 0] = p[2, 0].MirrorXY;

                if (n == 4)
                {
                    d1 = (r + pEdge) * Math.Cos(Angle45);
                    d2 = (r + s.Height + pEdge) * Math.Cos(Angle45);
                }
                d3 = s.Width * Math.Sin(Angle45) / 4;
                p[0, 1] = new PointD(d1 + d3, d1 - d3);
                p[1, 1] = p[0, 1].MirrorXY;
                p[2, 1] = new PointD(d2 - d3, d2 + d3);
                p[3, 1] = p[2, 1].MirrorXY;
            }
            else
            {   // 1,2,3,5,6,7,9
                p[0, 0] = new PointD(s.Width / 2, r);
                p[1, 0] = p[0, 0].MirrorY;
                p[2, 0] = new PointD(-s.Width / 2, s.Height);
                p[3, 0] = p[2, 0].MirrorY;

                if (n == 3)
                {
                    p[0, 1] = new PointD(s.Width / 4, r + pEdge);
                    p[1, 1] = p[0, 1].MirrorY;
                    p[2, 1] = new PointD(-s.Width / 4, s.Height);
                    p[3, 1] = p[2, 1].MirrorY;
                }
                else if (n == 5)
                {
                    p[0, 1] = new PointD(s.Width / 4, r + 0.5 * pEdge);
                    p[1, 1] = p[0, 1].MirrorY;
                    p[2, 1] = new PointD(-s.Width / 4, s.Height);
                    p[3, 1] = p[2, 1].MirrorY;
                }
                else
                {
                    p[0, 1] = new PointD(s.Width / 4, r);
                    p[1, 1] = p[0, 1].MirrorY;
                    p[2, 1] = new PointD(-s.Width / 4, s.Height);
                    p[3, 1] = p[2, 1].MirrorY;
                }
            }
            return AxisToScreen(p, c);
        }
        private PointD[] GetSingleDiamond(PointD c, SizeF s, int n)
        {
            PointD[] p = new PointD[4];
            if (n % 4 == 0)
            {
                double d2 = s.Height * Math.Sin(Angle45) / 2;
                double d3 = s.Width * Math.Sin(Angle45) / 2;
                p[0] = new PointD(0, 0);
                p[1] = new PointD(d2 - d3, d2 + d3);
                p[2] = new PointD(d2 * 2, d2 * 2);
                p[3] = p[1].MirrorXY;
            }
            else
            {
                p[0] = new PointD(0, 0);
                p[1] = new PointD(-s.Width / 2, s.Height / 2);
                p[2] = new PointD(0, s.Height);
                p[3] = p[1].MirrorY;
            }
            return AxisToScreen(p, c);
        }
        private List<PointD[,]> GetRect(Rectangle rect, int n, ref double r)
        {
            List<PointD[,]> lstPts = new List<PointD[,]>();
            PointD[,] pt = new PointD[4, 2];
            double theta = GetAngle(n);
            PointD cc = new PointD(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            SizeF s = GetRectSize(rect, n);
            if (n > 1)
            {
                pt = GetSingleRect(rect, cc, s, n, ref r);
                lstPts.Add(pt);
                for (int i = 1; i < n; i++)
                    lstPts.Add(RotatePolygen(pt, cc, theta * i));
            }
            else
            {
                pt[0, 0] = new PointD(rect.Width / 6, rect.Height / 2 - pEdge - rect.Width / 6);
                pt[1, 0] = new PointD(-rect.Width / 6, rect.Height / 2 - pEdge - rect.Width / 6);
                pt[2, 0] = new PointD(-rect.Width / 6, -rect.Height / 2 + pEdge + rect.Width / 6);
                pt[3, 0] = new PointD(rect.Width / 6, -rect.Height / 2 + pEdge + rect.Width / 6);

                pt[0, 1] = new PointD(rect.Width / 9, rect.Height / 2 - pEdge - rect.Width / 6);
                pt[1, 1] = new PointD(-rect.Width / 9, rect.Height / 2 - pEdge - rect.Width / 6);
                pt[2, 1] = new PointD(-rect.Width / 9, -rect.Height / 2 + pEdge + rect.Width / 6);
                pt[3, 1] = new PointD(rect.Width / 9, -rect.Height / 2 + pEdge + rect.Width / 6);

                lstPts.Add(AxisToScreen(pt, cc));
            }
            return lstPts;
        }
        private List<PointD[]> GetDiamond(Rectangle rect, int n)
        {
            List<PointD[]> lstPts = new List<PointD[]>();
            PointD[] pt = new PointD[4];
            PointD cc = new PointD(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            SizeF s = GetDiamondSize(rect, n);
            if (n > 1)
            {
                double theta = GetAngle(n);
                pt = GetSingleDiamond(cc, s, n);
                lstPts.Add(pt);
                for (int i = 1; i < n; i++)
                    lstPts.Add(RotatePolygen(pt, cc, theta * i));
            }
            else
            {
                pt[0] = new PointD(rect.Width / 2, 2 * pEdge);
                pt[1] = new PointD(rect.Width / 4, rect.Height / 2);
                pt[2] = new PointD(rect.Width / 2, rect.Height - 2 * pEdge);
                pt[3] = new PointD(rect.Width * 3 / 4, rect.Height / 2);
                lstPts.Add(pt);
            }
            return lstPts;
        }
        private void DrawCircle(Graphics g, Pen pen, PointD p, double r, bool fill)
        {
            if (fill)
            {
                Brush b = new SolidBrush(pen.Color);
                g.FillEllipse(b, new RectangleF((float)(p.X - r), (float)(p.Y - r), (float)(2 * r), (float)(2 * r)));
            }
            else
                g.DrawEllipse(pen, new RectangleF((float)(p.X - r), (float)(p.Y - r), (float)(2 * r), (float)(2 * r)));
        }
        private void Draw3Circle(Graphics g, Pen pen, PointD c, double r)
        {
            DrawCircle(g, pen, c, r, false);
            pen.Width = pWidth / 2;
            DrawCircle(g, pen, c, r * 2 / 3, false);
            DrawCircle(g, pen, c, r / 4, true);
            pen.Width = pWidth;
        }
        private void Draw3Diamond(Graphics g, Pen pen, PointD[] rect)
        {
            PointF[] pp = new PointF[rect.Length];
            for (int i = 0; i < rect.Length; i++)
                pp[i] = rect[i].PointF;
            g.DrawPolygon(pen, pp);
            pen.Width = pWidth / 2;
            g.DrawLine(pen, new PointD((rect[0].X + rect[1].X) / 2, (rect[0].Y + rect[1].Y) / 2).PointF, new PointD((rect[2].X + rect[3].X) / 2, (rect[2].Y + rect[3].Y) / 2).PointF);
            g.DrawLine(pen, new PointD((rect[1].X + rect[2].X) / 2, (rect[1].Y + rect[2].Y) / 2).PointF, new PointD((rect[0].X + rect[3].X) / 2, (rect[0].Y + rect[3].Y) / 2).PointF);
            pen.Width = pWidth;
        }
        private void DrawTong(PictureBox p, int n)
        {
            using (Graphics g = p.CreateGraphics())
            {
                using (Pen pen = new Pen(Color.Red, pWidth))
                {
                    PointD[] pts = null;
                    double[] r = GetR(typeMJ.TONG, p.ClientRectangle, n, ref pts);
                    for (int i = 0; i < pts.Length; i++)
                    {
                        Draw3Circle(g, pen, pts[i], r[0]);
                    }
                }
            }
        }
        private RectangleF GetRect(PointD c, double r)
        {
            return new RectangleF((float)(c.X - r), (float)(c.Y - r), (float)(2 * r), (float)(2 * r));
        }
        private double Distance(PointD p1, PointD p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        private PointD GetMid(PointD p1, PointD p2)
        {
            return new PointD((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
        private double GetAngle(int n)
        {
            return Math.PI / n * 2;
        }
        private float GetStartAngle(PointD p1, PointD p2, PointD c)
        {
            PointD pp1 = AxisFromScreen(p1, c);
            PointD pp2 = AxisFromScreen(p2, c);

            float an = (float)(180 * Math.Atan(Math.Abs(pp1.Y - pp2.Y) / Math.Abs(pp1.X - pp2.X)) / Math.PI);
            if (Math.Abs(pp1.X - pp2.X) < 0.1)
            {
                if (pp1.Y > pp2.Y)
                    return 0f;
                else
                    return 180f;
            }
            else
            {
                if (p1.X < p2.X)
                {
                    if (p1.Y < p2.Y)
                        return an - 90;
                    else
                        return 270 - an;
                }
                else
                {
                    if (p1.Y < p2.Y)
                        return 90 - an;
                    else
                        return 90 + an;
                }
            }
        }
        private void DrawRect(Graphics g, Pen pen, PointD[,] p, PointD c, double r, int n)
        {
            //Font f = new Font("Arial", 12, FontStyle.Bold);
            //for (int i = 0; i < p.Length; i++)
            //    g.DrawString($"{i}", f, Brushes.Black, p[i].PointF);

            g.DrawLine(pen, p[1, 0].PointF, p[2, 0].PointF);
            g.DrawLine(pen, p[0, 0].PointF, p[3, 0].PointF);
            if (n > 7)
                pen.Width = pWidth / 4;
            else
                pen.Width = pWidth / 2;
            g.DrawLine(pen, p[1, 1].PointF, p[2, 1].PointF);
            g.DrawLine(pen, p[0, 1].PointF, p[3, 1].PointF);
            if(n <6)
                pen.Width = pWidth;
            g.DrawLine(pen, GetMid(p[0, 1], p[1, 1]).PointF, GetMid(p[2, 1], p[3, 1]).PointF);
            pen.Width = pWidth;
            double d = Distance(p[0, 0], p[1, 0]);

            if (n == 2)
            {
                g.DrawLine(pen, p[0, 0].PointF, p[1, 0].PointF);
                pen.Width = pWidth / 2;
                g.DrawLine(pen, p[0, 1].PointF, p[1, 1].PointF);
                pen.Width = pWidth;
            }
            else if (n == 1)
            {
                g.DrawArc(pen, GetRect(GetMid(p[0, 0], p[1, 0]), d / 2), 180, 180);
                pen.Width = pWidth / 2;
                g.DrawArc(pen, GetRect(GetMid(p[0, 0], p[1, 0]), d / 3), 180, 180);
                pen.Width = pWidth;
            }

            float angle = GetStartAngle(p[0, 0], p[3, 0], c);
            RectangleF rect = GetRect(GetMid(p[2, 0], p[3, 0]), d / 2);
            g.DrawArc(pen, rect, angle, 180);
            pen.Width = pen.Width / 2;
            if (n == 1)
            {
                rect = GetRect(GetMid(p[2, 0], p[3, 0]), d / 3);
                g.DrawArc(pen, rect, angle, 180);
            }
            else
            {
                if (n > 7)
                    pen.Width = pen.Width / 2;
                rect = GetRect(GetMid(p[2, 0], p[3, 0]), d / 4);
                g.DrawArc(pen, rect, angle, 180);
            }
            /*
            g.DrawLine(pen, p[1].PointF, p[2].PointF);
            g.DrawLine(pen, p[0].PointF, p[3].PointF);
            */
            pen.Width = pWidth;

        }
        private void Draw3Rect(Graphics g, Pen pen, PointD[,] pt, PointD c, double r, int n)
        {
            pWidth = 4;
            if (r > pEdge)
            {
                double d = Distance(pt[0, 0], pt[1, 0]) / 2;
                PointD cc = GetMid(pt[0, 0], pt[1, 0]);
                d = d / Math.Sin(Math.PI / n);
                //g.DrawEllipse(pen, GetRect(c, d));
                Draw3Circle(g, pen, c, d);
            }
            DrawRect(g, pen, pt, c, r, n);
        }
        private void DrawTiao(PictureBox p, int n)
        {
            using (Graphics g = p.CreateGraphics())
            {
                using (Pen pen = new Pen(Color.Lime, pWidth))
                {
                    double r = 0;
                    List<PointD[,]> lstpts = GetRect(p.ClientRectangle, n, ref r);
                    PointD c = new PointD(p.ClientRectangle.Width / 2, p.ClientRectangle.Height / 2);
                    foreach (PointD[,] pt in lstpts)
                        Draw3Rect(g, pen, pt, c, r, n);
                }
            }
        }
        private void DrawWan(PictureBox p, int n)
        {
            using (Graphics g = p.CreateGraphics())
            {
                using (Pen pen = new Pen(Color.Blue, pWidth))
                {
                    List<PointD[]> lstpts = GetDiamond(p.ClientRectangle, n);
                    foreach (PointD[] pt in lstpts)
                        Draw3Diamond(g, pen, pt);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }


}
