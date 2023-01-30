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
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace Videos_Folder_Length
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEnumerable<string> AllVideos;
        int VideosCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog D = new FolderBrowserDialog();
            if (D.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = D.SelectedPath;
            }
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if(txtPath.Text == "Click Browse Button to select")
            {
                System.Windows.MessageBox.Show("Select The Folder First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AllVideos = Directory.GetFiles(txtPath.Text + "\\", "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".mp4") || s.EndsWith(".mkv"));

            if(AllVideos.Count() > 0)
            {
                VideosCount = AllVideos.Count();
            }
            else
            {
                System.Windows.MessageBox.Show("There is no videos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            lblResault.Visibility = Visibility.Visible;
            lblCount.Text = VideosCount.ToString();

            Calculate();
        }

        private void Calculate()
        {
            int Hours = 0;
            int Mins = 0;
            int Secs = 0;

            foreach(var file in AllVideos)
            {
                using (ShellObject shell = ShellObject.FromParsingName(file))
                {
                    // alternatively: shell.Properties.GetProperty("System.Media.Duration");
                    IShellProperty prop = shell.Properties.System.Media.Duration;
                    // Duration will be formatted as 00:44:08
                    string duration = prop.FormatForDisplay(PropertyDescriptionFormatOptions.None);

                    // 0 : Hours, 1 : Mins, 2 : Secs
                    string[] Times = duration.Split(':');

                    

                    try
                    {
                        Hours += Convert.ToInt32(Times[0]);
                        Mins += Convert.ToInt32(Times[1]);
                        Secs += Convert.ToInt32(Times[2]);
                    }
                    catch
                    {
                       
                        System.Windows.MessageBox.Show("There is a problem at the following file: \n\n"
                            + file + "\n\nWill be ignored", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            // Calc Total Lenght
            lblTotalLength.Text = Calc_Total(Hours, Mins, Secs);
            lblAvg.Text = Calc_Avg(Hours, Mins, Secs);


            grdResault.Visibility = Visibility.Visible;
        }

        private string Calc_Total(int h, int m, int s)
        {
            m += s / 60;
            s %= 60;

            h += m / 60;
            m %= 60;

            // Control Formate to be like that:  00:01:01
            string H = h.ToString();
            string M = m.ToString();
            string S = s.ToString();
            while (H.Length < 2 || M.Length < 2 || S.Length < 2)
            {
                if (H.Length < 2)
                    H = "0" + H;
                if (M.Length < 2)
                    M = "0" + M;
                if (S.Length < 2)
                    S = "0" + S;
            }

            return H + ":" + M + ":" + S;
        }

        private string Calc_Avg(int h, int m, int s)
        {
            // Get Total Time in Secounds
            s += h * 3600;
            s += m * 60;

            // Avg in Secounds
            int AvgS = s / VideosCount;

            string H = (AvgS / 3600).ToString();
            AvgS %= 3600;
            string M = (AvgS / 60).ToString();
            AvgS %= 60;
            string S = AvgS.ToString();

            // Control Formate to be like that:  00:01:01
            while (H.Length < 2 || M.Length < 2 || S.Length < 2)
            {
                if (H.Length < 2)
                    H = "0" + H;
                if (M.Length < 2)
                    M = "0" + M;
                if (S.Length < 2)
                    S = "0" + S;
            }

            return H + ":" + M + ":" + S;
        }
    }
}
