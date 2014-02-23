using Microsoft.Kinect;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// oyakusoku
namespace WpfApplication_KinectAudioSample
{
    /// <summary>
    /// 
    /// /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        

        #region Field

        /// <summary>
        /// Kinect censor connect
        /// </summary>
        KinectSensor kinect;

        #endregion Field

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            //Kinect初期化。
            this.kinect = KinectSensor.KinectSensors[0];
            this.kinect.Start();

            //音が発生したとき、その音が発生した方向(角度)を通知するイベント。2種類ある。
            // beamは大雑把な角度　Sound Sourceは詳細
            this.kinect.AudioSource.BeamAngleChanged += AudioSource_BeamAngleChanged;
            this.kinect.AudioSource.SoundSourceAngleChanged += AudioSource_SoundSourceAngleChanged;

            //音入力処理開始。
            this.kinect.AudioSource.Start();
            

        }

        #endregion Constructor

        #region EventHandler

        void AudioSource_BeamAngleChanged(object sender, BeamAngleChangedEventArgs e)
        {
            //音が発生した角度
            this.Label_BeamAngleChanged_Angle.Content = "Angle : " + e.Angle;
            //音が発生した角度をスライダに対応付ける
            this.Slider_BeamAngleChanged_Angle.Value = e.Angle;
        }

        void AudioSource_SoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            //音が発生した角度
            this.Label_SoundSourceAngleChanged_Angle.Content = "Angle : " + e.Angle;
            //推定された角度の信頼度
            this.Label_SoundSourceAngleChanged_Confidence.Content = "Confidence : " + e.ConfidenceLevel;
            //音が発生した角度をスライダに対応付ける
            this.Slider_SoundSourceAngleChanged_Angle.Value = e.Angle;

        }

        void hikaku(SoundSourceAngleChangedEventArgs f)
        {
            double hk;
            //ターゲットの決定
            Random rand = new Random();
            double tgnumber = rand.Next(-50, 50);
            this.Label_Targetangle.Content = "Target : " + tgnumber;
            
            if (tgnumber >= f.Angle) {  hk = tgnumber - f.Angle; }
            else { hk = f.Angle - tgnumber; }
            if (hk > 20) { this.Label_compare.Content = "distant"; }
            else
                if( hk > 10){this.Label_compare.Content = "near"  ; }
                else if (hk > 5) { this.Label_compare.Content = "good"; }
                else if (hk > 1) { this.Label_compare.Content = "Great"; }
                else { this.Label_compare.Content = "EXELLENT"; }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Label_SoundSourceAngle.Content = "SoundSourceAngle : " + this.kinect.AudioSource.SoundSourceAngle;
            this.Label_SoundSourceAngleConfidence.Content = "SoundSourceAngleConfidence : " + this.kinect.AudioSource.SoundSourceAngleConfidence;
            this.Label_BeamAngle.Content = "BeamAngle : " + this.kinect.AudioSource.BeamAngle;
            //event hikaku

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.kinect.AudioSource.Stop();
            this.kinect.Dispose();
        }


        #endregion EventHandler
    }
}
