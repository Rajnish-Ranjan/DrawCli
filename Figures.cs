using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using DrawCli;

namespace Figures_Set
{
    /// <summary>
    /// Control points set to a figure
    /// Mouse trigger changes layout of the figure, 
    /// if it's action happen on the control points in the selection mode of the figure
    /// </summary>
    public class ControlPoint:MainWindow
    {
        public ControlPoint(Point point,FlowDirection direct )
        {

        }
        
    }


    public class MyFigure :MainWindow
    {
        protected List<Point> controlPoints;
        protected Point _start, _end;
    }

    public class MyRectangle :MyFigure
    {
        public Rectangle rect;

        public MyRectangle(Rectangle rect)
        {
            this.rect = rect;
        }
        public SolidColorBrush borderColor
        {
            get;
        }
        public int _thickness
        {
            get;
        }
        private void getControlPoints()
        {
            double[] x = new double[2];
            double[] y = new double[2];
            x[0] = Canvas.GetLeft(rect);
            x[1] = Canvas.GetRight(rect);
            y[0] = Canvas.GetTop(rect);
            y[1] = Canvas.GetBottom(rect);
            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    controlPoints.Add(new Point() { X = x[i], Y = y[j] });
                    if (i == 0)
                        controlPoints.Add(new Point() { X = (x[0] + x[1]) / 2, Y = y[j] });
                    if (j == 0)
                        controlPoints.Add(new Point() { X = x[i], Y = (y[0] + y[1]) / 2 });
                }
            }
        }

        public void setControlPoints()
        {
            getControlPoints();
            //if the shape is highlighted the control points are activated
        }
    }



    public class MyEllipse : MyFigure
    {
        public Ellipse ellipse;
        
        public MyEllipse(Ellipse ellipse)
        {
            this.ellipse = ellipse;
        }

        private void getControlPoints()
        {
            double[] x = new double[2];
            double[] y = new double[2];
            x[0] = Canvas.GetLeft(ellipse);
            x[1] = Canvas.GetRight(ellipse);
            y[0] = Canvas.GetTop(ellipse);
            y[1] = Canvas.GetBottom(ellipse);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    controlPoints.Add(new Point() { X = x[i], Y = y[j] });
                    if (i == 0)
                        controlPoints.Add(new Point() { X = (x[0] + x[1]) / 2, Y = y[j] });
                    if (j == 0)
                        controlPoints.Add(new Point() { X = x[i], Y = (y[0] + y[1]) / 2 });
                }
            }
        }

    }

    public class MyLine:MyFigure
    {
        public Line _line;
        public MyLine(Line _line)
        {
            this._line = _line;
        }
    }
    /// <summary>
    /// In case of tool as Sketch, all the points during mouse movement are recorded to PointsCollection
    /// in left button pressed mode
    /// </summary>
    public class MySketch:MyFigure
    {
        public PointCollection pointsSet
        {
            get;
        }
        public MySketch(PointCollection pointsSet)
        {
            this.pointsSet = pointsSet;
            //controlPoints = new List<Point>();
        }
    }
}