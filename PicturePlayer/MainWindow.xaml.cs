using Microsoft.Win32;
using System.Diagnostics;
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
        string spath = Directory.GetCurrentDirectory();
        string[] sPlaylists = new string[] { };                //playlists in current directory
        int iPlaylist_index;
        string[] strImages = new string[] { };                 //pictures in current playlist
        int index;

        public MainWindow()
        {
            InitializeComponent();

            MakePlaylist();             //find all the playlists that exist
            GetPlaylists();             //load the picture filenames in the first playlist

            LoadHelpText();
        }


        private void MakePlaylist()
        {
            //make a default playlist for a newly-installed program
            //writing the current directory of installation into the playlist
            string[] sfiles = Directory.GetFiles(spath + @"\Resources");
            string[] spictures = new string[] { };
            foreach(string s in sfiles) 
            {
                if (s.IndexOf(".jpg") > 0)      //look for *.jpg pictures only
                {
                    Array.Resize(ref spictures, spictures.Length + 1);
                    spictures[spictures.Length - 1] = s;
                }
            } 
            if (spictures.Length > 0 &&
                    File.Exists(spath + @"\Food.txt") == false)
            {
                File.WriteAllLines(spath + @"\Food.txt", spictures);
            }

        }

        private void GetPlaylists()
        {
            //find all playlists (*.txt) in application directory
            string[] sLists = System.IO.Directory.GetFiles(spath);
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

            cmbPlaylist.SelectedIndex = 0;
        }

        private void LoadHelpText()
        {
            try
            {
                string strHelpPath = spath + @"\Resources\Help.txt";
                string[] strHelp = File.ReadAllLines(strHelpPath);
                foreach (string s in strHelp)
                {
                    txtHelp.Text += s + "\n";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error reading help.txt"
                    ,MessageBoxButton.OK, MessageBoxImage.Error);
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
                txtStatus.Text = (index+1).ToString() + ")    " 
                                    + System.IO.Path.GetFileName(strImages[index]) ;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error opening picture"
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
                txtStatus.Text = (index+1).ToString() + ")    "
                                    + System.IO.Path.GetFileName(strImages[index]);
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
            o.DefaultDirectory = spath;
            o.Filter = "Text files | *.txt";
            o.FileName = sPlaylists[iPlaylist_index];
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
            s.DefaultDirectory = spath;
            s.Filter = "Text files | *.txt";
            s.FileName = sPlaylists[iPlaylist_index];
            s.OverwritePrompt = false;
            if (s.ShowDialog() == true)
            {
                File.WriteAllText(s.FileName, txtList.Text);
            }
        }

        private void cmbPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = Convert.ToInt32((sender as ComboBox)?.SelectedIndex);

            iPlaylist_index = i;
            strImages = File.ReadAllLines(sPlaylists[iPlaylist_index]);
            index = -1;
            txtList.Text = "";
            foreach (string s in strImages)
            {
                txtList.Text += s + "\n";
            }

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
