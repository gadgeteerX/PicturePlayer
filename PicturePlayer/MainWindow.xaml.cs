using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PicturePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string spath = @"d:\repos2\PicturePlayer\aaa.txt";
        string[] sPlaylists = new string[] { };                //playlists in current directory
        string[] strImages = new string[] { };                 //pictures in current playlist
        int index;

        public MainWindow()
        {
            InitializeComponent();

            GetPlaylists();
            cmbPlaylist.SelectedIndex = 0;
        }

        private void GetPlaylists()
        {
            //find all playlists (*.txt) in application directory
            string[] sLists = Directory.GetFiles(@"d:\repos2\PicturePlayer");
            string sfilename;
            foreach (string s in sLists)
            {
                if (s.IndexOf(".txt") > 0)          
                {
                    Array.Resize(ref sPlaylists, sPlaylists.Length + 1);
                    sPlaylists[sPlaylists.Length - 1] = s;          //add path to playlist

                    sfilename = System.IO.Path.GetFileName(s);      
                    cmbPlaylist.Items.Add(sfilename);               //add filename to combo
                }
            }
        }

        private void GetPictures()
        {
            //list out all pictures in txtList
            strImages = File.ReadAllLines(spath);
            index = -1;
            txtList.Text = "";
            foreach (string s in strImages)
            {
                txtList.Text += s + "\n";
            }
        }

        private void ShowNextImage()
        {
            index += 1;
            if (index == strImages.Length)
                index = 0;

            try
            {
                ImageSource imageSource = new BitmapImage(new Uri(strImages[index]));
                imgViewer.Source = imageSource;
                txtStatus.Text = "Picture " + (index+1).ToString() + ")    " + strImages[index];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "error opening image"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPrevImage()
        {
            index += -1;
            if (index < 0)
                index = strImages.Length - 1;

            try
            {
                ImageSource imageSource = new BitmapImage(new Uri(strImages[index]));
                imgViewer.Source = imageSource;
                txtStatus.Text = "Picture " + (index+1).ToString() + ")    " + strImages[index];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error opening picture"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void mnuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            ShowNextImage();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.DefaultDirectory = @"d:\repos2\PicturePlayer";
            o.Filter = "Text files | *.txt";
            o.FileName = "aaa.txt";
            if (o.ShowDialog() == true)
            {
                txtList.Text = File.ReadAllText(o.FileName);
            }
        }

        private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.DefaultDirectory = @"d:\repos2\PicturePlayer";
            s.Filter = "Text files | *.txt";
            s.FileName = "aaa.txt";
            s.OverwritePrompt = false;
            if (s.ShowDialog() == true)
            {
                File.WriteAllText(s.FileName, txtList.Text);
            }
        }

        private void cmbPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = Convert.ToInt32((sender as ComboBox)?.SelectedIndex);

            spath = sPlaylists[i];
            GetPictures();
            ShowNextImage();
        }

        private void btnPlayPrev_Click(object sender, RoutedEventArgs e)
        {
            ShowPrevImage();
        }

        private void btnClose2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
