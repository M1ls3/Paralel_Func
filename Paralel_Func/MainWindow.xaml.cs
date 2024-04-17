using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Paralel_Func
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static double a1, b1, a2, b2;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            double[] x, y;
            string[] x_text = textBox.Text.Split(';');
            string[] y_text = textBox_Copy.Text.Split(';');
            if (x_text.Length != y_text.Length)
            {
                MessageBox.Show("The number of x values must be equal to the number of y values.");
            return;
            }
            x = new double[x_text.Length];
            y = new double[y_text.Length];
            for (int i = 0; i < x_text.Length; i++)
            {
                if (!double.TryParse(x_text[i], out x[i]))
                {
                    MessageBox.Show("Invalid input for x value at index " + i);
                    return;
                }
            }
            for (int i = 0; i < y_text.Length; i++)
            {
                if (!double.TryParse(y_text[i], out y[i]))
                {
                    MessageBox.Show("Invalid input for y value at index " + i);
                    return;
                }
            }
            if (x.Length == y.Length)
            {
                n = x.Length;
            };
            // Нормалізація даних
            for (int i = 0; i < n; i++)
            {
                x[i] = Math.Log(x[i]);
            }
            Thread thread1 = new Thread(() => ThreadFunction1(x, y));
            thread1.Start();
            Thread thread2 = new Thread(() => ThreadFunction2(x, y));
            thread2.Start();
            thread1.Join();
            thread2.Join();

            if (d1 < d2)
            {
                MessageBox.Show("Result Point Vector: \ny = " + a1 + "* lnx +" + b1 +
                "\n" + "d1: " + d1 + "\n" + "d2: " + d2);
            }
            else
            {
                MessageBox.Show("Result Point Vector: \ny = " + Math.Pow(Math.E, a2) +
                " * x^" + b2 + "\n" + "d1: " + d1 + "\n" + "d2: " + d2);
            }
        }

        static double d1, d2;
        static int n = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        static void ThreadFunction1(double[] x, double[] y)
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;
            for (int i = 0; i < x.Length; i++)
            {
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }

            a1 = (Yi * Xi2 * x.Length - XiYi * x.Length * Xi) / (Xi2 * x.Length * 
                x.Length - x.Length * Xi * Xi);
            b1 = (XiYi * x.Length - Yi * Xi) / (Xi2 * x.Length - Xi * Xi);
            d1 = Math.Sqrt(((Yi - a1 * Xi - b1) * (Yi - a1 * Xi - b1)) / (Yi * Yi));
            Console.WriteLine("d1 = " + d1);
        }

        static void ThreadFunction2(double[] x, double[] y)
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;

            for (int i = 0; i < x.Length; i++)
            {
                y[i] = Math.Log(y[i]);
            }
            for (int i = 0; i < x.Length; i++)
            {
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }
            a2 = (Yi * Xi2 * x.Length - XiYi * x.Length * Xi) / (Xi2 * x.Length * 
                x.Length - x.Length * Xi * Xi);
            b2 = (XiYi * x.Length - Yi * Xi) / (Xi2 * x.Length - Xi * Xi);
            d2 = Math.Sqrt(((Yi - a2 * Xi - b2) * (Yi - a2 * Xi - b2)) / (Yi * Yi));
            Console.WriteLine("d2 = " + d2);
        }
    }
}
