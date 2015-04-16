
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BlueSheep.Interface
{

    static class Drawing
    {

        public static GraphicsPath RoundRect(Rectangle rect, int slope)
        {
            GraphicsPath gp = new GraphicsPath();
            int arcWidth = slope * 2;
            gp.AddArc(new Rectangle(rect.X, rect.Y, arcWidth, arcWidth), -180, 90);
            gp.AddArc(new Rectangle(rect.Width - arcWidth + rect.X, rect.Y, arcWidth, arcWidth), -90, 90);
            gp.AddArc(new Rectangle(rect.Width - arcWidth + rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 0, 90);
            gp.AddArc(new Rectangle(rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 90, 90);
            gp.CloseAllFigures();
            return gp;
        }

    }

    class SadikButton : Control
    {

        public enum Style
        {
            Blue,
            Dark,
            Light
        }

        private Style _style;
        public Style ButtonStyle
        {
            get { return _style; }
            set
            {
                _style = value;
                Invalidate();
            }
        }

        private Image _image;
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        private bool _rounded;
        public bool RoundedCorners
        {
            get { return _rounded; }
            set
            {
                _rounded = value;
                Invalidate();
            }
        }

        public enum State
        {
            None,
            Over,
            Down
        }


        private State MouseState;
        public SadikButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            MouseState = State.None;
            Size = new Size(65, 26);
            Font = new Font("Verdana", 8);
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics G = e.Graphics;

            G.Clear(Parent.BackColor);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            int slope = 3;

            Rectangle shadowRect = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath shadowPath = Drawing.RoundRect(shadowRect, slope);
            Rectangle mainRect = new Rectangle(0, 0, Width - 2, Height - 2);
            switch (ButtonStyle)
            {
                case Style.Blue:
                    if (_rounded)
                    {
                        G.FillPath(new SolidBrush(Color.FromArgb(20, 135, 195)), shadowPath);
                        G.FillPath(new SolidBrush(Color.FromArgb(20, 160, 230)), Drawing.RoundRect(mainRect, slope));
                    }
                    else
                    {
                        G.FillRectangle(new SolidBrush(Color.FromArgb(20, 135, 195)), shadowRect);
                        G.FillRectangle(new SolidBrush(Color.FromArgb(20, 160, 230)), mainRect);
                    }
                    break;
                case Style.Dark:
                    if (_rounded)
                    {
                        G.FillPath(new SolidBrush(Color.FromArgb(45, 45, 45)), shadowPath);
                        G.FillPath(new SolidBrush(Color.FromArgb(75, 75, 75)), Drawing.RoundRect(mainRect, slope));
                    }
                    else
                    {
                        G.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 45)), shadowRect);
                        G.FillRectangle(new SolidBrush(Color.FromArgb(75, 75, 75)), mainRect);
                    }
                    break;
                case Style.Light:
                    if (_rounded)
                    {
                        G.FillPath(new SolidBrush(Color.FromArgb(145, 145, 145)), shadowPath);
                        G.FillPath(new SolidBrush(Color.FromArgb(170, 170, 170)), Drawing.RoundRect(mainRect, slope));
                    }
                    else
                    {
                        G.FillRectangle(new SolidBrush(Color.FromArgb(145, 145, 145)), shadowRect);
                        G.FillRectangle(new SolidBrush(Color.FromArgb(170, 170, 170)), mainRect);
                    }
                    break;
            }

            if (_image == null)
            {
                float textX = ((this.Width - 1) / 2) - (G.MeasureString(Text, Font).Width / 2);
                float textY = ((this.Height - 1) / 2) - (G.MeasureString(Text, Font).Height / 2);
                G.DrawString(Text, Font, Brushes.White, textX, textY);
            }
            else
            {
                Size textSize = new Size((int)G.MeasureString(Text, Font).Width, (int)G.MeasureString(Text, Font).Height);
                int imageWidth = this.Height - 19;
                int imageHeight = this.Height - 19;
                int imageX = ((this.Width - 1) / 2) - ((imageWidth + 4 + textSize.Width) / 2);
                int imageY = ((this.Height - 1) / 2) - (imageHeight / 2);
                G.DrawImage(_image, imageX, imageY, imageWidth, imageHeight);
                G.DrawString(Text, Font, Brushes.White, new Point(imageX + imageWidth + 4, ((this.Height - 1) / 2) - textSize.Height / 2));
            }

            if (MouseState == State.Over)
            {
                G.FillPath(new SolidBrush(Color.FromArgb(25, Color.White)), shadowPath);
            }
            else if (MouseState == State.Down)
            {
                G.FillPath(new SolidBrush(Color.FromArgb(40, Color.White)), shadowPath);
            }

        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseState = State.Over;
            Invalidate();
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseState = State.None;
            Invalidate();
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseState = State.Over;
            Invalidate();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseState = State.Down;
            Invalidate();
        }
    }

    class SadikTabControl : TabControl
    {
        public SadikTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            ItemSize = new Size(0, 30);
            Font = new Font("Verdana", 8);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Top;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;

            Pen borderPen = new Pen(Color.FromArgb(225, 225, 225));

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            Rectangle fillRect = new Rectangle(2, ItemSize.Height + 2, Width - 6, Height - ItemSize.Height - 3);
            G.FillRectangle(Brushes.White, fillRect);
            G.DrawRectangle(borderPen, fillRect);

            Color FontColor = new Color();

            int i = 0;
            for (i = 0; i <= TabCount - 1; i++)
            {
                Rectangle mainRect = GetTabRect(i);


                if (i == SelectedIndex)
                {
                    G.FillRectangle(new SolidBrush(Color.White), mainRect);
                    G.DrawRectangle(borderPen, mainRect);
                    G.DrawLine(new Pen(Color.FromArgb(20, 160, 230)), new Point(mainRect.X + 1, mainRect.Y), new Point(mainRect.X + mainRect.Width - 1, mainRect.Y));
                    G.DrawLine(Pens.White, new Point(mainRect.X + 1, mainRect.Y + mainRect.Height), new Point(mainRect.X + mainRect.Width - 1, mainRect.Y + mainRect.Height));
                    FontColor = Color.FromArgb(20, 160, 230);


                }
                else
                {
                    G.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), mainRect);
                    G.DrawRectangle(borderPen, mainRect);
                    FontColor = Color.FromArgb(160, 160, 160);

                }

                float titleX = (mainRect.Location.X + mainRect.Width / 2) - (G.MeasureString(TabPages[i].Text, Font).Width / 2);
                float titleY = (mainRect.Location.Y + mainRect.Height / 2) - (G.MeasureString(TabPages[i].Text, Font).Height / 2);
                G.DrawString(TabPages[i].Text, Font, new SolidBrush(FontColor), new Point((int)titleX, (int)titleY));

                try
                {
                    TabPages[i].BackColor = Color.White;
                }
                catch
                {
                }

            }

        }

    }

    class SadikGroupBox : ContainerControl
    {

        public SadikGroupBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.FromArgb(250, 250, 250);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            Rectangle mainRect = new Rectangle(0, 0, Width - 1, Height - 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), mainRect);
            G.DrawRectangle(new Pen(Color.FromArgb(225, 225, 225)), mainRect);

        }

    }

    class SadikLabelHeader : Label
    {

        public SadikLabelHeader()
        {
            Font = new Font("Verdana", 10, FontStyle.Bold);
        }

    }

    public class SadikLabel : Label
    {

        public SadikLabel()
        {
            Font = new Font("Verdana", 8);
            ForeColor = Color.FromArgb(135, 135, 135);
        }

    }

    class SadikProgressBar : Control
    {

        private int _Maximum = 100;
        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                if (value < 1)
                    value = 1;
                if (value < _Value)
                    _Value = value;
                _Maximum = value;
                Invalidate();
            }
        }

        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                if (value > _Maximum)
                    value = Maximum;
                _Value = value;
                Invalidate();
            }
        }

        public SadikProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(100, 40);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            Rectangle mainRect = new Rectangle(0, 0, Width - 1, Height - 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), mainRect);
            G.DrawLine(new Pen(Color.FromArgb(230, 230, 230)), new Point(mainRect.X, mainRect.Height), new Point(mainRect.Width, mainRect.Height));

            Rectangle barRect = new Rectangle(0, 0, Convert.ToInt32(((Width / _Maximum) * _Value) - 1), Height - 1);
            //G.FillRectangle(new SolidBrush(Color.FromArgb(20, 160, 230)), barRect);
            G.FillRectangle(new SolidBrush(Color.DeepSkyBlue), barRect);
            if (_Value > 1)
                G.DrawLine(new Pen(Color.DeepSkyBlue), new Point(barRect.X, barRect.Height), new Point(barRect.Width, barRect.Height));

            int textX = 12;
            float textY = ((this.Height - 1) / 2) - (G.MeasureString(Text, Font).Height / 2);
            float percent = (_Value / _Maximum) * 100;
            string txt = Text + " " + Convert.ToString(percent) + "%";
            G.DrawString(txt, Font, new SolidBrush(Color.Black), new Point(textX + 1, (int)textY + 1));
            G.DrawString(txt, Font, Brushes.White, new Point(textX, (int)textY));

        }

    }

    class SadikBar : Control
    {
        int min = 0;	// Minimum value for progress range
        int max = 100;	// Maximum value for progress range
        int val = 0;		// Current progress
        Color BarColor = Color.DeepSkyBlue;		// Color of progress meter

        protected override void OnResize(EventArgs e)
        {
            // Invalidate the control to get a repaint.
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(BarColor);
            float percent = (float)(val - min) / (float)(max - min);
            double p = Math.Truncate(100 * percent);
            Rectangle rect = this.ClientRectangle;

            // Calculate area for drawing the progress.
            rect.Width = (int)((float)rect.Width * percent);

            // Draw the progress meter.
            g.FillRectangle(brush, rect);
            int textX = 12;
            float textY = ((this.Height - 1) / 2) - (g.MeasureString(Text, Font).Height / 2);
            string txt = Text + " " + p + "%";
            g.DrawString(txt, Font, new SolidBrush(Color.White), new Point(textX + 1, (int)textY + 1));
            g.DrawString(txt, Font, Brushes.Black, new Point(textX, (int)textY));

            // Draw a three-dimensional border around the control.
            Draw3DBorder(g);

            // Clean up.
            brush.Dispose();
            g.Dispose();
        }

        public int Minimum
        {
            get
            {
                return min;
            }

            set
            {
                // Prevent a negative value.
                if (value < 0)
                {
                    min = 0;
                }

                // Make sure that the minimum value is never set higher than the maximum value.
                if (value > max)
                {
                    min = value;
                    min = value;
                }

                // Ensure value is still in range
                if (val < min)
                {
                    val = min;
                }

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return max;
            }

            set
            {
                // Make sure that the maximum value is never set lower than the minimum value.
                if (value < min)
                {
                    min = value;
                }

                max = value;

                // Make sure that value is still in range.
                if (val > max)
                {
                    val = max;
                }

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        public int Value
        {
            get
            {
                return val;
            }

            set
            {
                int oldValue = val;

                // Make sure that the value does not stray outside the valid range.
                if (value < min)
                {
                    val = min;
                }
                else if (value > max)
                {
                    val = max;
                }
                else
                {
                    val = value;
                }

                // Invalidate only the changed area.
                float percent;

                Rectangle newValueRect = this.ClientRectangle;
                Rectangle oldValueRect = this.ClientRectangle;

                // Use a new value to calculate the rectangle for progress.
                percent = (float)(val - min) / (float)(max - min);
                newValueRect.Width = (int)((float)newValueRect.Width * percent);

                // Use an old value to calculate the rectangle for progress.
                percent = (float)(oldValue - min) / (float)(max - min);
                oldValueRect.Width = (int)((float)oldValueRect.Width * percent);

                Rectangle updateRect = new Rectangle();

                // Find only the part of the screen that must be updated.
                if (newValueRect.Width > oldValueRect.Width)
                {
                    updateRect.X = oldValueRect.Size.Width;
                    updateRect.Width = newValueRect.Width - oldValueRect.Width;
                }
                else
                {
                    updateRect.X = newValueRect.Size.Width;
                    updateRect.Width = oldValueRect.Width - newValueRect.Width;
                }

                updateRect.Height = this.Height;

                // Invalidate the intersection region only.
                this.Invalidate(updateRect);
            }
        }

        public Color ProgressBarColor
        {
            get
            {
                return BarColor;
            }

            set
            {
                BarColor = value;

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        private void Draw3DBorder(Graphics g)
        {
            int PenWidth = (int)Pens.White.Width;

            g.DrawLine(Pens.DarkGray,
                new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
                new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top));
            g.DrawLine(Pens.DarkGray,
                new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
                new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth));
            g.DrawLine(Pens.White,
                new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth),
                new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
            g.DrawLine(Pens.White,
                new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top),
                new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
        } 
    }

    class SadikAlertBox : Control
    {

        private Point exitLocation;

        private bool overExit;
        public enum Style
        {
            _Error,
            _Success,
            _Warning,
            _Notice
        }

        private Style _alertStyle;
        public Style AlertStyle
        {
            get { return _alertStyle; }
            set
            {
                _alertStyle = value;
                Invalidate();
            }
        }

        public SadikAlertBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Font = new Font("Verdana", 8);
            Size = new Size(200, 40);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            Color borderColor = default(Color);
            Color innerColor = default(Color);
            Color textColor = default(Color);
            switch (_alertStyle)
            {
                case Style._Error:
                    borderColor = Color.FromArgb(250, 195, 195);
                    innerColor = Color.FromArgb(255, 235, 235);
                    textColor = Color.FromArgb(220, 90, 90);
                    break;
                case Style._Notice:
                    borderColor = Color.FromArgb(180, 215, 230);
                    innerColor = Color.FromArgb(235, 245, 255);
                    textColor = Color.FromArgb(80, 145, 180);
                    break;
                case Style._Success:
                    borderColor = Color.FromArgb(180, 220, 130);
                    innerColor = Color.FromArgb(235, 245, 225);
                    textColor = Color.FromArgb(95, 145, 45);
                    break;
                case Style._Warning:
                    borderColor = Color.FromArgb(220, 215, 140);
                    innerColor = Color.FromArgb(250, 250, 220);
                    textColor = Color.FromArgb(145, 135, 110);
                    break;
            }

            Rectangle mainRect = new Rectangle(0, 0, Width - 1, Height - 1);
            G.FillRectangle(new SolidBrush(innerColor), mainRect);
            G.DrawRectangle(new Pen(borderColor), mainRect);

            string styleText = null;
            switch (_alertStyle)
            {
                case Style._Error:
                    styleText = "Error!";
                    break;
                case Style._Notice:
                    styleText = "Notice!";
                    break;
                case Style._Success:
                    styleText = "Success!";
                    break;
                case Style._Warning:
                    styleText = "Warning!";
                    break;
            }

            Font styleFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            float textY = ((this.Height - 1) / 2) - (G.MeasureString(Text, Font).Height / 2);
            G.DrawString(styleText, styleFont, new SolidBrush(textColor), new Point(10, (int)textY));
            G.DrawString(Text, Font, new SolidBrush(textColor), new Point(10 + (int)G.MeasureString(styleText, styleFont).Width + 4, (int)textY));

            Font exitFont = new Font("Marlett", 6);
            float exitY = ((this.Height - 1) / 2) - (G.MeasureString("r", exitFont).Height / 2) + 1;
            exitLocation = new Point(Width - 26, (int)exitY - 3);
            G.DrawString("r", exitFont, new SolidBrush(textColor), new Point(Width - 23, (int)exitY));

        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.X >= Width - 26 && e.X <= Width - 12 && e.Y > exitLocation.Y && e.Y < exitLocation.Y + 12)
            {
                if (Cursor != Cursors.Hand)
                    Cursor = Cursors.Hand;
                overExit = true;
            }
            else
            {
                if (Cursor != Cursors.Arrow)
                    Cursor = Cursors.Arrow;
                overExit = false;
            }

            Invalidate();

        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (overExit)
                this.Visible = false;

        }

    }

    class SadikCombo : ComboBox
    {

        public SadikCombo()
        {
            DrawItem += replaceItem;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Font = new Font("Verdana", 8);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DoubleBuffered = true;
            ItemHeight = 20;

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            Rectangle mainRect = new Rectangle(0, 0, Width - 1, Height - 1);
            G.FillRectangle(Brushes.White, mainRect);
            G.DrawRectangle(new Pen(Color.FromArgb(225, 225, 225)), mainRect);

            Point[] triangle = new Point[] {
                new Point(Width - 14, 16),
                new Point(Width - 18, 10),
                new Point(Width - 10, 10)
            };
            G.FillPolygon(Brushes.DarkGray, triangle);
            G.DrawLine(new Pen(Color.FromArgb(225, 225, 225)), new Point(Width - 27, 0), new Point(Width - 27, Height - 1));

            try
            {
                if (Items.Count > 0)
                {
                    if (!(SelectedIndex == -1))
                    {
                        int textX = 6;
                        int textY = ((this.Height - 1) / 2) - (int)(G.MeasureString((string)Items[SelectedIndex], Font).Height / 2) + 1;
                        G.DrawString((string)Items[SelectedIndex], Font, Brushes.Gray, new Point(textX, textY));
                    }
                    else
                    {
                        int textX = 6;
                        int textY = ((this.Height - 1) / 2) - (int)(G.MeasureString((string)Items[0], Font).Height / 2) + 1;
                        G.DrawString(Items[0].ToString(), Font, Brushes.Gray, new Point(textX, textY));
                    }
                }
            }
            catch
            {
            }

        }

        public void replaceItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();

            Graphics G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle rect = new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 1, e.Bounds.Height + 1);

            try
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    G.FillRectangle(new SolidBrush(Color.FromArgb(20, 160, 230)), rect);
                    G.DrawString(base.GetItemText(base.Items[e.Index]), Font, Brushes.White, new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height));
                    G.DrawRectangle(new Pen(Color.FromArgb(20, 160, 230)), rect);
                }
                else
                {
                    G.FillRectangle(Brushes.White, rect);
                    G.DrawString(base.GetItemText(base.Items[e.Index]), Font, Brushes.DarkGray, new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height));
                    G.DrawRectangle(new Pen(Color.FromArgb(20, 160, 230)), rect);
                }

            }
            catch
            {
            }

        }

        protected override void OnSelectedItemChanged(System.EventArgs e)
        {
            base.OnSelectedItemChanged(e);
            Invalidate();
        }

    }

    [DefaultEvent("CheckedChanged")]
    public class SadikCheckbox : Control
    {

        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                Invalidate();
            }
        }

        public SadikCheckbox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(140, 20);
            Font = new Font("Verdana", 8);
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Color.White);

            Rectangle box = new Rectangle(0, 0, Height, Height - 1);
            G.FillRectangle(Brushes.White, box);
            G.DrawRectangle(new Pen(Color.Gray), box);

            Pen markPen = new Pen(Color.FromArgb(150, 155, 160));
            Pen lightMarkPen = new Pen(Color.FromArgb(170, 175, 180));
            if (_checked)
                G.DrawString("a", new Font("Marlett", 14), Brushes.Gray, new Point(0, 0));

            float textY = (Height / 2) - (G.MeasureString(Text, Font).Height / 2);
            G.DrawString(Text, Font, Brushes.Gray, new Point(24, (int)textY));

        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_checked)
            {
                _checked = false;
            }
            else
            {
                _checked = true;
            }

            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();

        }

    }

    [DefaultEvent("CheckedChanged")]
    public class SadikRadioButton : Control
    {

        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                Invalidate();
            }
        }

        public SadikRadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(140, 20);
            Font = new Font("Verdana", 8);
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            Rectangle box = new Rectangle(0, 0, Height - 1, Height - 1);
            G.FillEllipse(Brushes.White, box);
            G.DrawEllipse(new Pen(Color.FromArgb(0, 0, 0)), box);

            if (_checked)
            {
                Rectangle innerMark = new Rectangle(6, 6, Height - 13, Height - 13);
                G.FillEllipse(new SolidBrush(Color.FromArgb(20, 160, 230)), innerMark);
            }

            float textY = (Height / 2) - (G.MeasureString(Text, Font).Height / 2);
            G.DrawString(Text, Font, Brushes.Gray, new Point(24, (int)textY));

        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            foreach (Control C in Parent.Controls)
            {
                if (!object.ReferenceEquals(C, this) && C is SadikRadioButton)
                {
                    ((SadikRadioButton)C).Checked = false;
                }
            }

            if (_checked)
            {
                _checked = false;
            }
            else
            {
                _checked = true;
            }

            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();

        }
    }

    class SadikVerticalTabControl : TabControl
    {

        public SadikVerticalTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(30, 100);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Left;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);


            G.Clear(Color.DeepSkyBlue);
            int i = 0;
            for (i = 0; i <= TabCount - 1; i++)
            {
                Rectangle TabRectangle = GetTabRect(i);
                if (i == SelectedIndex)
                {
                    //// tab is selected
                    G.FillRectangle(Brushes.DeepSkyBlue, TabRectangle);
                }
                else
                {
                    //// tab is not selected
                    G.FillRectangle(new SolidBrush(Color.FromArgb(255, 51, 51, 51)), TabRectangle);
                }
                G.DrawString(TabPages[i].Text, Font, Brushes.White, TabRectangle, new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            }

            e.Graphics.DrawImage(B, 0, 0);
            G.Dispose();
            B.Dispose();

           
        }
    }
}

