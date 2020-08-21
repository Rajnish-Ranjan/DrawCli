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
using Figures_Set;

namespace DrawCli
{
    /// <summary>
    /// This acts as an static class containing the information of the current tool and its properties
    /// </summary>
    public class DrawTools
    {
        public static DrawToolType currTool = DrawTools.DrawToolType.Pointer;
        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Polygon,
            NumberOfDrawTools
        };
        public static Brush colorStroke = Brushes.Black;
        public static int thicknessVal = 1;
    }


    //Main Window
    public partial class MainWindow : Window
    {
        #region Members

        //public Figure fig
        //{
        //    get; set;
        //}

        //public List<object> selected_Components
        //{
        //    get;
        //}

        private Point _start, _end;

        List<object> MyBoardObjects;

        Stack<List<object>> deletedObjects;

        private object _figure = null;

        bool _isSet;

        #endregion

        /// <summary>
        /// Apply color and thickness derived from setProperties to each of the selected components 
        /// </summary>
        /// <param name="_clr">Color for the next object to be drawn</param>
        /// <param name="_thickness">Thickness for the next object to be drawn </param>
        public void changeProperties(Color _clr, int _thickness)
        {
            //temp = "#"+temp.Substring(3, 6);
            DrawTools.colorStroke = (Brush)new BrushConverter().ConvertFromString(
                string.Format("#{0:X2}{1:X2}{2:X2}", _clr.R, _clr.G, _clr.B));
            if (_thickness > 0)
            {
                DrawTools.thicknessVal = _thickness;
            }
        }

        #region Mouse Triggers on Canvas MyBoard

        /// <summary>
        /// Even Trigger when left mouse button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isSet = true;
            _start = e.GetPosition(this.MyBoard);
            switch (DrawTools.currTool)
            {
                case DrawTools.DrawToolType.Pointer:
                    break;
                case DrawTools.DrawToolType.Line:
                    _figure= getLine(sender, e);
                    break;
                case DrawTools.DrawToolType.Rectangle:
                    _figure = getRectangle(sender, e);
                    break;
                case DrawTools.DrawToolType.Ellipse:
                    _figure = getEllipse(sender, e);
                    break;
                case DrawTools.DrawToolType.Polygon:
                    _figure = getSketch(sender,e);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event Trigger when mouse is pressed and it is moved on the canvas "MyBoard"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void board_MouseMove(object sender, MouseEventArgs e)
        {
            _end = e.GetPosition(this.MyBoard);
            if (_isSet == false)
            {
                return;
            }
            if(_figure is Line)
            {
                editLine(_figure as Line);
            }
            else if(_figure is Rectangle)
            {
                editRectangle(_figure as Rectangle);
            }
            else if(_figure is Ellipse)
            {
                editEllipse(_figure as Ellipse);
            }
            else if(_figure is PointCollection)
            {
                editSetOfLines(_figure as PointCollection);
            }
            else
            {

            }
            //changeLayout();
        }

        /// <summary>
        /// Event Trigger when left mouse button is lifted on MyBoard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void board_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isSet = false;
        }

        #endregion

        #region Methods to set Figures

        /// <summary>
        /// The method creates rectangle object and save it in the list MyBoardObjects and also 
        /// prints it on Canvas "MyBoard"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private object getRectangle(object sender, MouseEventArgs e)
        {
            Rectangle rect = new Rectangle()
            {
                Stroke = DrawTools.colorStroke,
                //IsEnabled = true,
                StrokeThickness = DrawTools.thicknessVal,
                //Fill = DrawTools.colorStroke,
                //Width = Math.Abs(_end.X - _start.X),
                //Height = Math.Abs(_end.Y - _start.Y)
                Width = 2,
                Height=2
            };

            rect.SetValue(LeftProperty, _start.X);
            rect.SetValue(TopProperty, _start.Y);
            MyBoard.Children.Add(rect);
            Canvas.SetLeft(rect, Math.Min( _start.X,_end.X));
            Canvas.SetTop(rect, Math.Min( _start.Y,_end.Y));

            MyRectangle newRect = new MyRectangle(rect);

            MyBoardObjects.Add(newRect);

            return rect;
        }

        private object getEllipse(object sender, MouseEventArgs e)
        {
            _end = e.GetPosition(MyBoard);
            Ellipse ellip = new Ellipse()
            {
                Stroke = DrawTools.colorStroke,
                //IsEnabled = true,
                StrokeThickness = DrawTools.thicknessVal,
                //Fill = DrawTools.colorStroke,
                //Width = Math.Abs(_end.X - _start.X),
                //Height = Math.Abs(_end.Y - _start.Y)
                Width=2,
                Height=2
            };

            ellip.SetValue(LeftProperty, _start.X);
            ellip.SetValue(TopProperty, _start.Y);
            MyBoard.Children.Add(ellip);
            Canvas.SetLeft(ellip, Math.Min(_start.X, _end.X));
            Canvas.SetTop(ellip, Math.Min(_start.Y, _end.Y));

            MyEllipse newEllip = new MyEllipse(ellip);

            MyBoardObjects.Add(newEllip);

            return ellip;
        }

        private object getLine(object sender, MouseEventArgs e)
        {
            _end = e.GetPosition(MyBoard);
            Line _line = new Line() 
            { 
                X1 = _start.X,
                X2 = _end.X, 
                Y1 = _start.Y, 
                Y2 = _end.Y, 
                Stroke = DrawTools.colorStroke, 
                StrokeThickness = DrawTools.thicknessVal 
            };

            MyBoard.Children.Add(_line);
            MyLine newLine = new MyLine(_line);
            MyBoardObjects.Add(newLine);

            return _line;
        }

        private object getSketch(object sender, MouseButtonEventArgs e)
        {
            PointCollection pointsColl = new PointCollection() {_start };
            MySketch mysketch = new MySketch(pointsColl);
            MyBoardObjects.Add(mysketch);
            return pointsColl;
        }
        /// <summary>
        /// The method prints a number of lines as per the collection of points
        /// Between two consecutive points, there is a line
        /// </summary>
        /// <param name="points">Collection of points from MySketch</param>
        private void addLines(PointCollection points)
        {
            for(int i = 1; i < points.Count; i++)
            {
                MyBoard.Children.Add(new Line()
                {
                    X1=points[i-1].X,
                    X2=points[i].X,
                    Y1=points[i-1].Y,
                    Y2=points[i].Y,
                    Stroke=DrawTools.colorStroke,
                    StrokeThickness=DrawTools.thicknessVal
                });
            }
        }

        #endregion

        #region Edit FIgures

        /// <summary>
        /// To edit the layout of the rectangle
        /// </summary>
        /// <param name="rect">Rectangle object which is to be edited</param>
        private void editRectangle(Rectangle rect )
        {
            rect.SetValue(LeftProperty, Math.Min(_start.X, _end.X));
            rect.SetValue(WidthProperty, Math.Abs(_end.X - _start.X));
            rect.SetValue(TopProperty, Math.Min(_start.Y, _end.Y));
            rect.SetValue(HeightProperty, Math.Abs(_end.Y - _start.Y));
        }
        private void editEllipse(Ellipse ellip)
        {
            ellip.SetValue(LeftProperty, Math.Min(_start.X, _end.X));
            ellip.SetValue(WidthProperty, Math.Abs(_end.X - _start.X));
            ellip.SetValue(TopProperty, Math.Min(_start.Y, _end.Y));
            ellip.SetValue(HeightProperty, Math.Abs(_end.Y - _start.Y));
        }

        private void editLine(Line _line)
        {
            _line.X2 = _end.X;
            _line.Y2 = _end.Y;
        }

        private void editSetOfLines(PointCollection _pointsCollection)
        {
            try
            {
                Point prevPoint = _pointsCollection[_pointsCollection.Count - 1];
                Line newLine = new Line()
                {
                    X1 = prevPoint.X,
                    X2 = _end.X,
                    Y1 = prevPoint.Y,
                    Y2 = _end.Y,
                    Stroke = DrawTools.colorStroke,
                    StrokeThickness = DrawTools.thicknessVal
                };
                MyBoard.Children.Add(newLine);
            }
            catch (Exception) { }
            _pointsCollection.Add(_end);
            var temp = MyBoardObjects[MyBoardObjects.Count - 1] as MySketch;
            this.Title = Convert.ToString(temp.pointsSet.Count);
        }
        #endregion
    }
}