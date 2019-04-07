using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace WpfControlLibrary
{
    public partial class ToggleSwitch : UserControl
    {
        public ToggleSwitch()
        {
            InitializeComponent();
            borderTrack.Background = new SolidColorBrush(Colors.Gray); ;
        }

        private void ButtonToggle_Click(object sender, RoutedEventArgs e)
        {
            IsOn = !IsOn;
        }

        #region ******************************* TrackBackgroundOnColor
        [Category("Switch")]
        public Color TrackBackgroundOnColor
        {
            get { return (Color)this.GetValue(TrackBackgroundOnColorProperty); }
            set { this.SetValue(TrackBackgroundOnColorProperty, value); }
        }

        public static readonly DependencyProperty TrackBackgroundOnColorProperty =
            DependencyProperty.Register("TrackBackgroundOnColor", typeof(Color),
                typeof(ToggleSwitch),
                new FrameworkPropertyMetadata(Colors.DeepSkyBlue,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    TrackBackgroundOnColorChangeFunc,
                    TrackBackgroundOnColorCoerceFunc));

        static void TrackBackgroundOnColorChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (Color)e.OldValue;
            var nf = (Color)e.NewValue;
            var obj = (ToggleSwitch)target;
        }

        static object TrackBackgroundOnColorCoerceFunc(DependencyObject target, object baseValue)
        {
            var obj = (ToggleSwitch)target;
            var val = (Color)baseValue;

            return val;
        }
        #endregion

        #region ******************************* TrackBackgroundOffColor
        [Category("Switch")]
        public Color TrackBackgroundOffColor
        {
            get { return (Color)this.GetValue(TrackBackgroundOffColorProperty); }
            set { this.SetValue(TrackBackgroundOffColorProperty, value); }
        }

        public static readonly DependencyProperty TrackBackgroundOffColorProperty =
            DependencyProperty.Register("TrackBackgroundOffColor", typeof(Color),
                typeof(ToggleSwitch),
                new FrameworkPropertyMetadata(Colors.Gray,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    TrackBackgroundOffColorChangeFunc));

        static void TrackBackgroundOffColorChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (Color)e.OldValue;
            var nf = (Color)e.NewValue;
            var obj = (ToggleSwitch)target;
            obj.borderTrack.Background = new SolidColorBrush(nf);
        }
        #endregion

        #region ******************************* CircleBackgroundColor
        [Category("Switch")]
        public Color CircleBackgroundColor
        {
            get { return (Color)this.GetValue(CircleBackgroundColorProperty); }
            set { this.SetValue(CircleBackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty CircleBackgroundColorProperty =
            DependencyProperty.Register("CircleBackgroundColor", typeof(Color),
                typeof(ToggleSwitch),
                new FrameworkPropertyMetadata(Colors.SteelBlue,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    CircleBackgroundColorChangeFunc));

        static void CircleBackgroundColorChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (Color)e.OldValue;
            var nf = (Color)e.NewValue;
            var obj = (ToggleSwitch)target;
            obj.ellipseToggle.Fill = new SolidColorBrush(nf);
        }
        #endregion

        #region ******************************* CircleBorderColor
        [Category("Switch")]
        public Color CircleBorderColor
        {
            get { return (Color)this.GetValue(CircleBorderColorProperty); }
            set { this.SetValue(CircleBorderColorProperty, value); }
        }

        public static readonly DependencyProperty CircleBorderColorProperty =
            DependencyProperty.Register("CircleBorderColor", typeof(Color),
                typeof(ToggleSwitch),
                new FrameworkPropertyMetadata(Colors.White,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    CircleBorderColorChangeFunc));

        static void CircleBorderColorChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (Color)e.OldValue;
            var nf = (Color)e.NewValue;
            var obj = (ToggleSwitch)target;

            obj.ellipseToggle.Stroke = new SolidColorBrush(nf);
        }
        #endregion

        //BindsTwoWayByDefaultを設定する事
        #region ******************************* IsOn
        [Category("Switch")]
        [Description("Switch status")]
        public bool IsOn
        {
            get { return (bool)this.GetValue(IsOnProperty); }
            set { this.SetValue(IsOnProperty, value); }
        }
        public static readonly DependencyProperty IsOnProperty =
            DependencyProperty.Register("IsOn", typeof(bool),
                typeof(ToggleSwitch),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    IsOnChangeFunc));

        static void IsOnChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (bool)e.OldValue;
            var nf = (bool)e.NewValue;
            var obj = (ToggleSwitch)target;

            if (nf)
            {
                obj.borderTrack.Background = new SolidColorBrush(obj.TrackBackgroundOffColor);
                var ca = new ColorAnimation(obj.TrackBackgroundOnColor, TimeSpan.FromSeconds(obj.Duration));
                obj.borderTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                var da = new DoubleAnimation(10, TimeSpan.FromSeconds(obj.Duration));
                obj.translateTransform.BeginAnimation(TranslateTransform.XProperty, da);
            }
            else
            {
                obj.borderTrack.Background = new SolidColorBrush(obj.TrackBackgroundOnColor);
                var ca = new ColorAnimation(obj.TrackBackgroundOffColor, TimeSpan.FromSeconds(obj.Duration));
                obj.borderTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                var da = new DoubleAnimation(-10, TimeSpan.FromSeconds(obj.Duration));
                obj.translateTransform.BeginAnimation(TranslateTransform.XProperty, da);
            }
        }
        #endregion

        #region ******************************* Duration
        [Category("Switch")]
        [Description("Change Duration (sec)")]
        public double Duration
        {
            get { return (double)this.GetValue(DurationProperty); }
            set { this.SetValue(DurationProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("ChangeDuration", typeof(double),
                typeof(ToggleSwitch),
                new PropertyMetadata(0.05));
        #endregion

    }
}
