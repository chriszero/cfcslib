using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cfcslib.Controller;
using Cfcslib.Filter;

using ZedGraph;
using Cfcslib.NumMath;
using Cfcslib;

namespace Visu {
    public partial class Form1 : Form {
        int tickStart;
        private HighBand _lb1 = new HighBand();
        private PidController _pid = new PidController(1.0, 0.2, 1.0);
        private PidControl _pid2 = new PidControl(1.0, 1.0, 0.2, 1.0);

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "Test of Dynamic Data Update with ZedGraph\n" +
                  "(After 25 seconds the graph scrolls)";
            myPane.XAxis.Title.Text = "Time, Seconds";
            myPane.YAxis.Title.Text = "Sample Potential, Volts";

            // Save 1200 points.  At 50 ms sample rate, this is one minute
            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            RollingPointPairList list = new RollingPointPairList(1200);
            RollingPointPairList list2 = new RollingPointPairList(1200);

            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            LineItem curve = myPane.AddCurve("Pid1 (Meiner)", list, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;

            LineItem curve2 = myPane.AddCurve("Pid2", list2, Color.Red, SymbolType.None);
            curve2.Line.IsAntiAlias = true;

            // Sample at 50ms intervals
            timer1.Interval = 50;
            timer1.Enabled = true;
            timer1.Start();

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 30;
            myPane.XAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 5;

            // Scale the axes
            zedGraphControl1.AxisChange();

            // Save the beginning time for reference
            tickStart = Environment.TickCount;
        }
        double last = 0;
        private void timer1_Tick(object sender, EventArgs e) {
            // Make sure that the curvelist has at least one curve
            if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                return;

            // Get the first CurveItem in the graph
            LineItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            LineItem curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
            if (curve == null)
                return;

            // Get the PointPairList
            IPointListEdit list = curve.Points as IPointListEdit;
            IPointListEdit list2 = curve2.Points as IPointListEdit;
            // If this is null, it means the reference at curve.Points does not
            // support IPointListEdit, so we won't be able to modify it
            if (list == null)
                return;

            // Time is measured in seconds
            double time = (Environment.TickCount - tickStart) / 1000.0;

            double outp = 0;
            outp = _pid.Calculate(0, trackBar1.Value, 0, 0);
            _pid2.Update(trackBar1.Value, 65);
            //last = outp;
            // 3 seconds per cycle
            // Math.Sin(2.0 * Math.PI * time / 3.0)
            list.Add(time, outp);
            list2.Add(time, _pid2.Cv);

            // Keep the X scale at a rolling 30 second interval, with one
            // major step between the max X value and the end of the axis
            Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            if (time > xScale.Max - xScale.MajorStep) {
                xScale.Max = time + xScale.MajorStep;
                xScale.Min = xScale.Max - 30.0;
            }

            // Make sure the Y axis is rescaled to accommodate actual data
            zedGraphControl1.AxisChange();
            // Force a redraw
            zedGraphControl1.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            label1.Text = string.Format("{0}", trackBar1.Value);
        }
    }
}
