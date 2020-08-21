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
using Figures_Set;

namespace DrawCli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region Variables Initialization

            MyBoardObjects = new List<object>();
            deletedObjects = new Stack<List<object>>();
            MyBoard.Background = Brushes.White;
            //DrawTools.currTool = DrawTools.DrawToolType.Pointer;
            //DrawTools.colorStroke = Brushes.Black;
            //DrawTools.thicknessVal = 1;
            // Flag to ensure thet, the figure will be edited while the mouse left button will be down only.
            _isSet = false;

            #endregion

            #region Glue Code Between UI and C#

            //Windows Events
            DrawCli.Closing += MainWindow_Closing;
            DrawCli.Closed += MainWindow_Closed;

            //Menu Events
            menu_New.Click += menu_New_Click;
            menu_Open.Click += menu_Open_Click;
            menu_Save.Click += menu_Save_Click;
            menu_Recent.Click += menu_Recent_Click;
            menu_Exit.Click += menu_Exit_Click;
            
            menu_SelectAll.Click += menu_SelectAll_Click;
            menu_UnSelectAll.Click += menu_UnSelectAll_Click;
            menu_Back.Click += menu_Back_Click;
            menu_Forward.Click += menu_Forward_Click;
            menu_Properties.Click += menu_Properties_Click;
            menu_Line.Click += menu_Line_Click;
            menu_Rect.Click += menu_Rect_Click;
            menu_Ellipse.Click += menu_Ellipse_Click;
            menu_Sketch.Click += menu_Sketch_Click;
            menu_About.Click += menu_About_Click;

            //Button Events
            Btn_New.Click += Btn_New_Click;
            Btn_Open.Click += Btn_Open_CLick;
            Btn_Save.Click += Btn_Save_CLick;
            Btn_Pointer.Click += Btn_Pointer_Click;
            Btn_Line.Click += Btn_Line_Click;
            Btn_Rect.Click += Btn_Rect_CLick;
            Btn_Ellipse.Click += Btn_Ellipse_Click;
            Btn_Sketch.Click += Btn_Sketch_CLick;
            Btn_Properties.Click += Btn_Properties_Click;
            Btn_Help.Click += Btn_Help_Click;
            btn_Back.Click += btn_Back_Click;
            Btn_Forward.Click += Btn_Forward_Click;
            btn_Clear.Click += btn_Clear_Click;


            // Canvas Events
            MyBoard.MouseMove += board_MouseMove;
            MyBoard.MouseDown += board_MouseDown;
            MyBoard.MouseUp += board_MouseUp;
            #endregion

        }

        #region Members

        public Figure fig
        {
            get;set;
        }

        public List<object> selected_Components
        {
            get;
        }

        /// <summary>
        /// To give direction to the control points
        /// </summary>
        public enum MyDirection{
            left,right,top,bottom,
            left_top,left_bottom,right_top,right_bottom,
            all_direction
        }


        //private Point _start, _end;

        //List<object> MyBoardObjects;

        #endregion

        #region Window Load Events
        /*
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }*/

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach(var obj in MyBoardObjects)
            {
                if(obj is Line)
                {
                    Line line = obj as Line;
                    line.IsEnabled = false;
                }
                else if(obj is Rectangle)
                {
                    Rectangle temprect = obj as Rectangle;
                    temprect.IsEnabled = false;
                }
                else if(obj is Ellipse)
                {
                    Ellipse tempell = obj as Ellipse;
                    tempell.IsEnabled = false;
                }
                else if(obj is PointCollection)
                {
                    PointCollection pntc = obj as PointCollection;
                    pntc.Clear();
                }
            }
            MyBoardObjects.Clear();
            MyBoard.Children.Clear();
            
        }


        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Menu Click Event

        private void menu_New_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void menu_Open_Click(object sender, RoutedEventArgs e)
        {
            _open();
        }

        private void menu_Save_Click(object sender, RoutedEventArgs e)
        {
            _save();
        }

        private void menu_Recent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menu_SelectAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menu_UnSelectAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menu_Back_Click(object sender, RoutedEventArgs e)
        {
            _backward();
        }

        private void menu_Forward_Click(object sender, RoutedEventArgs e)
        {
            _forward();
        }

        private void menu_Properties_Click(object sender, RoutedEventArgs e)
        {
            setProperties sp = new setProperties(this);
            sp.Show();
        }

        private void menu_Line_Click(object sender, RoutedEventArgs e)
        {
            _line(sender, e);
        }

        private void menu_Rect_Click(object sender, RoutedEventArgs e)
        {
            _rect(sender, e);
        }

        private void menu_Ellipse_Click(object sender, RoutedEventArgs e)
        {
            _ellipse(sender, e);
        }

        private void menu_Sketch_Click(object sender, RoutedEventArgs e)
        {
            _sketch(sender, e);
        }

        private void menu_About_Click(object sender, RoutedEventArgs e)
        {
            _about();
        }
        #endregion

        #region Button Events

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            _new();
        }

        private void Btn_Save_CLick(object sender, RoutedEventArgs e)
        {
            _save();
        }

        private void Btn_Open_CLick(object sender, RoutedEventArgs e)
        {
            _open();
        }

        private void Btn_Pointer_Click(object sender, RoutedEventArgs e)
        {
            DrawTools.currTool = DrawTools.DrawToolType.Pointer;
        }

        private void Btn_Line_Click(object sender, RoutedEventArgs e)
        {
            _line(sender, e);
        }

        private void Btn_Rect_CLick(object sender, RoutedEventArgs e)
        {
            _rect(sender, e);
        }

        private void Btn_Ellipse_Click(object sender, RoutedEventArgs e)
        {
            _ellipse(sender, e);
        }

        private void Btn_Sketch_CLick(object sender, RoutedEventArgs e)
        {
            _sketch(sender, e);
        }

        private void Btn_Properties_Click(object sender, RoutedEventArgs e)
        {
            setProperties sp = new setProperties(this);
            sp.Show();
        }

        private void Btn_Help_Click(object sender, RoutedEventArgs e)
        {
            _about();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            _backward();
        }

        private void Btn_Forward_Click(object sender, RoutedEventArgs e)
        {
            _forward();
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            MyBoard.Children.Clear();
            if(MyBoardObjects.Count>0)
                deletedObjects.Push(MyBoardObjects);
            MyBoardObjects.Clear();
        }

        #endregion

        #region Common Methods from menu and button

        private void _new()
        {

        }
        private void _open()
        {

        }

        private void _save()
        {

        }

        private void _about()
        {
            MessageBox.Show("You May visit to\n\t rajnish-ranjan.github.io");
        }

        private void _line(object sender, RoutedEventArgs e)
        {
            DrawTools.currTool = DrawTools.DrawToolType.Line;
        }

        private void _rect(object sender, RoutedEventArgs e)
        {
            DrawTools.currTool = DrawTools.DrawToolType.Rectangle;
        }

        private void _ellipse(object sender, RoutedEventArgs e)
        {
            DrawTools.currTool = DrawTools.DrawToolType.Ellipse;
        }

        private void _sketch(object sender, RoutedEventArgs e)
        {
            DrawTools.currTool = DrawTools.DrawToolType.Polygon;
        }

        private void _backward()
        {
            List<object> deletedList = new List<object>();
            try
            {
                int lastIndex = MyBoardObjects.Count - 1;
                object obj = MyBoardObjects[lastIndex];
                if (obj is MySketch)
                {
                    MySketch tempSketch = obj as MySketch;
                    int no_lines = tempSketch.pointsSet.Count - 1;
                    ///<summary>To removet the no_lines number of lines</summary>
                    MyBoard.Children.RemoveRange(MyBoard.Children.Count - no_lines, no_lines - 1);
                    deletedList.Add(MyBoardObjects[lastIndex]);
                    MyBoardObjects.RemoveAt(lastIndex);
                }
                else
                {
                    deletedList.Add(MyBoardObjects[lastIndex]);
                    MyBoardObjects.RemoveAt(lastIndex);
                    MyBoard.Children.RemoveAt(MyBoard.Children.Count - 1);
                }
                deletedObjects.Push(deletedList);
            }
            catch (Exception) { }
        }

        private void _forward()
        {
            List<object> lastObj;
            if (deletedObjects.Count == 0)
                return;
            lastObj = deletedObjects.Pop();
            foreach (object obj in lastObj)
            {
                if (obj is MySketch)
                {
                    MySketch mySketch = obj as MySketch;
                    PointCollection points = mySketch.pointsSet;
                    try
                    {
                        addLines(points);
                        MyBoardObjects.Add(mySketch);
                    }
                    catch (Exception) { }
                }
                else if (obj is MyRectangle)
                {
                    MyRectangle myrect = obj as MyRectangle;
                    try
                    {
                        MyBoard.Children.Add(myrect.rect);
                        MyBoardObjects.Add(myrect);
                    }
                    catch (Exception) { }
                }
                else if (obj is MyLine)
                {
                    MyLine myline = obj as MyLine;
                    try
                    {
                        MyBoard.Children.Add(myline._line);
                        MyBoardObjects.Add(myline);
                    }
                    catch (Exception) { }
                }
                else if (obj is MyEllipse)
                {
                    MyEllipse myelli = obj as MyEllipse;
                    try
                    {
                        MyBoard.Children.Add(myelli.ellipse);
                        MyBoardObjects.Add(myelli);
                    }
                    catch (Exception) { }
                }
                else
                {

                }
            }
        }

        #endregion

    }
}
