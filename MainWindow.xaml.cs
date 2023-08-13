using Microsoft.Win32;
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

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private List<string> playlist = new List<string>();
        private int currentTrackIndex = -1;
        private bool isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMediaPlayer();
        }

        private void InitializeMediaPlayer()
        {
            mediaPlayer.MediaEnded += (sender, e) => PlayNextTrack();
            volumeSlider.Value = mediaPlayer.Volume * 100;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count > 0 && currentTrackIndex >= 0)
            {
                if (!isPlaying)
                {
                    mediaPlayer.Play();
                    isPlaying = true;
                }
                else
                {
                    mediaPlayer.Pause();
                    isPlaying = false;
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            isPlaying = false;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count > 0)
            {
                currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;
                PlaySelectedTrack();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count > 0)
            {
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
                PlaySelectedTrack();
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                playlist.Clear();
                playlist.AddRange(openFileDialog.FileNames);
                currentTrackIndex = 0;
                PlaySelectedTrack();
            }
        }

        private void PlaySelectedTrack()
        {
            if (currentTrackIndex >= 0 && currentTrackIndex < playlist.Count)
            {
                mediaPlayer.Open(new Uri(playlist[currentTrackIndex]));
                mediaPlayer.Play();
                isPlaying = true;
            }
        }

        private void PlayNextTrack()
        {
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
            PlaySelectedTrack();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = volumeSlider.Value / 100;
            }
        }
    }
}
