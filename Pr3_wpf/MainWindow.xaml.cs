using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace Pr3_wpf
{  
    public partial class MainWindow : Window
    {
       DispatcherTimer timer = new DispatcherTimer();
       List<string> Miusuk = new List<string>();
       List<string> copyMiusuk = new List<string>();
       string FillName;
       bool start;
        bool reaplei = false; 
        bool ponovai = false;
       int flag= 0;
       int namber = 0;
       
        public MainWindow()
       {
           InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(0.0);
            timer.Tick += timer_tick;

            Thread potok = new Thread(_ =>
            {
                while (true)
                {
                    while (start != false)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {                          
                            audioSlider.Value = (double)(media.Position.TotalSeconds);
                            if (media.NaturalDuration.HasTimeSpan == true)
                            {
                                TimeSpan remaine = media.NaturalDuration.TimeSpan - media.Position;
                                remained.Text = remaine.ToString(@"mm\:ss");
                                if (remained.Text == "00:00")
                                {
                                    if (reaplei == false) 
                                    {
                                        end();
                                    }
                                    
                                }                               
                            }
                            if (ponovai== true) 
                            {
                                TimeSpan remain = media.NaturalDuration.TimeSpan;
                                if (media.Position == remain)
                                {
                                    media.Source = new Uri(FillName);
                                }
                            }
                           
                        }));                      
                    }
                }
            });
            potok.Start();
         
        }
        void end()
        {
            ListMiusk.SelectedIndex = Math.Min(ListMiusk.SelectedIndex + 1, ListMiusk.Items.Count);
        }  
        private void PapkaChoose_Click(object sender, RoutedEventArgs e)
        {          
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok )
            {
                FillName = dialog.FileName;
                media.Source = new Uri(FillName);
                
                string[] miusuk = Directory.GetFiles(dialog.FileName);
                foreach (string m in miusuk)
                {
                    Miusuk.Add(m);
                }
            }
            ListMiusk.ItemsSource = Miusuk;
            string url = Miusuk[0];
            FillName = url;
            media.Source = new Uri(FillName);
            if (media.NaturalDuration.HasTimeSpan == true)
            {
                audioSlider.Maximum = (double)(media.NaturalDuration.TimeSpan.TotalSeconds);
                start = true;

            }

        }
        private void timer_tick(object sender, EventArgs e)
        {
            time.Text = media.Position.ToString(@"mm\:ss");
         
        }
        private void audioSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {           
            media.Position = TimeSpan.FromSeconds(audioSlider.Value);
            audioSlider.Value = (double)(media.Position.TotalSeconds);
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            audioSlider.Value = (double)(media.Position.TotalSeconds);
            media.Pause();
            timer.Stop();
            start = false;
                
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {        
            if (media.NaturalDuration.HasTimeSpan == true)
            {
                audioSlider.Maximum = (double)(media.NaturalDuration.TimeSpan.TotalSeconds);
                audioSlider.Value = (double)(media.Position.TotalSeconds);
                start = true;
                media.Play();
                timer.Start();
            }
        }
        private void SoundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {          
            if (media != null)
            {
                media.Volume = SoundSlider.Value;
            }
        }
        private void right_Click(object sender, RoutedEventArgs e)
        {
           
            ListMiusk.SelectedIndex = Math.Min(ListMiusk.SelectedIndex + 1, ListMiusk.Items.Count - 1);
            audioSlider.Value = (double)(media.Position.TotalSeconds);

        }
        private void ListMiusk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            audioSlider.Value = (double)(media.Position.TotalSeconds);
            string url = ListMiusk.SelectedItem.ToString();
            FillName = url;
            media.Source = new Uri(FillName);
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
           
            ListMiusk.SelectedIndex = Math.Min(ListMiusk.SelectedIndex - 1, ListMiusk.Items.Count + 1);
            audioSlider.Value = (double)(media.Position.TotalSeconds);

        }
        private void random_Click(object sender, RoutedEventArgs e)
        {
            audioSlider.Value = (double)(media.Position.TotalSeconds);
            namber += 1;
            if (namber %2 == 0)
            {              
                Miusuk.Clear();
                foreach (string j in copyMiusuk)
                {
                    Miusuk.Add(j);
                }
                
            }
            else if (namber %2 != 0) 
            {
               copyMiusuk.Clear();
                foreach (string i in Miusuk)
                {
                    copyMiusuk.Add(i);
                }
                Random random = new Random();
                for (int i = 0; i < Miusuk.Count; i++)
                {
                    string tmp = Miusuk[0];
                    Miusuk.RemoveAt(0);
                    Miusuk.Insert(random.Next(Miusuk.Count), tmp);
                    FillName = tmp;
                    media.Source = new Uri(FillName);
                }
            }
            
        }       
        private void Repleyi_Click(object sender, RoutedEventArgs e)
        {
            flag += 1;
            if (flag %2 == 0)
            {
                reaplei = false;
                ponovai= false;
            }
            else if (flag %2 != 0)
            {
                reaplei= true;
                ponovai= true;                               
            }
        }
    }
}
