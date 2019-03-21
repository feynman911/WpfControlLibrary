using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media.Animation;

using System.Linq;


namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for MeterSlide
    /// </summary>
    public partial class MeterSlide : UserControl
    {
        public MeterSlide()
        {
            InitializeComponent();
            memoribanInit();
            mojibanInit();
            sliderAnimationInit();
        }

        SolidColorBrush red = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        SolidColorBrush pink = new SolidColorBrush(Color.FromArgb(255, 255, 100, 100));
        SolidColorBrush black = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        SolidColorBrush blue = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
        SolidColorBrush gray = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
        SolidColorBrush gray2 = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));

        DoubleAnimation myDoubleAnimation = new DoubleAnimation();
        Storyboard myStoryboard = new Storyboard();

        double realValue = 0;
        double writeAreaPercent = 1.6; //表示エリアはみだし量
        int posNum1 = 0;
        int posNum2 = 0;

        int off1 = 0;
        int off2 = 0;
        double scale = 1.0;

        private void DisplayArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            memoribanInit();
            mojibanInit();
        }

        #region ******************* memoribanInit
        void memoribanInit()
        {
            Memoriban.Children.Clear();
            Memoriban.Width = DisplayArea.ActualWidth * writeAreaPercent;
            Memoriban.Height = DisplayArea.ActualHeight;
            Memoriban.HorizontalAlignment = HorizontalAlignment.Center;
            Memoriban.VerticalAlignment = VerticalAlignment.Center;
            double center = Memoriban.Width / 2;

            posNum1 = (int)(center / ScaleResolution);
            posNum2 = (int)(center / 100);

            off1 = 0;
            off2 = 0;

            //目盛り線
            for (int dispPos = -posNum1; dispPos <= posNum1 + 1; dispPos++)
            {
                Rectangle rt = new Rectangle();
                rt.StrokeThickness = 1.6;
                rt.Stroke = gray;
                rt.Fill = gray;
                rt.Height = Memoriban.Height / 5;
                rt.HorizontalAlignment = HorizontalAlignment.Left;
                rt.VerticalAlignment = VerticalAlignment.Center;
                double pos = center + (dispPos * ScaleResolution - off1) * scale;
                rt.Margin = new Thickness(pos - rt.StrokeThickness / 2, 0, 0, 0);
                Memoriban.Children.Add(rt);

            }

            //100毎に線を太くする
            for (int dispPos = -posNum2; dispPos <= posNum2 + 1; dispPos++)
            {
                Rectangle rt = new Rectangle();
                rt.StrokeThickness = 2;
                rt.Stroke = black;
                rt.Fill = black;
                rt.Height = Memoriban.Height / 2.5;
                rt.HorizontalAlignment = HorizontalAlignment.Left;
                rt.VerticalAlignment = VerticalAlignment.Center;
                double pos = center + (dispPos * 100 - off2) * scale;
                rt.Margin = new Thickness(pos - rt.StrokeThickness / 2, 0, 0, 0);
                Memoriban.Children.Add(rt);
            }

        }
        #endregion

        //100単位でMemoribanをスライドさせて表示（目盛り作り直し回避）
        #region ******************* MoveMemori
        void MoveMemori()
        {
            if (Memoriban.Width == 0) return;
            off2 = (int)(realValue) % 100;
            double center = Memoriban.Width / 2;
            double widearea = (Memoriban.Width - DisplayArea.ActualWidth) / 2;
            Memoriban.Margin = new Thickness(-off2 - widearea, 0, off2 - widearea, 0);
        }
        #endregion


        #region ******************* MojibanInit
        void mojibanInit()
        {
            Mojiban.Children.Clear();
            Mojiban.Width = DisplayArea.ActualWidth * writeAreaPercent;
            Mojiban.Height = DisplayArea.ActualHeight;
            Mojiban.HorizontalAlignment = HorizontalAlignment.Center;
            Mojiban.VerticalAlignment = VerticalAlignment.Center;

            double center = Mojiban.Width / 2;
            posNum2 = (int)(center / 100);

            //数字の表示
            for(int dispPos = -posNum2; dispPos <= posNum2 + 1; dispPos++)
            {
                int dv = (int)(realValue / 100) * 100;
                int v = dv + dispPos * 100;
                TextBlock tb = new TextBlock();
                tb.Text = v.ToString();
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                double pos = center + (dispPos * 100 - off2) * scale;
                tb.Margin = new Thickness(pos - 1 , 0, 0, 0);
                Mojiban.Children.Add(tb);

            }
        }
        #endregion

        //100太線単位でMojibanをスライドさせてTextBlockを再利用
        #region ******************* MoveMoji
        void MoveMoji()
        {
            if (Mojiban.ActualHeight == 0) return;
            if (Mojiban.Children.Count == 0) return;

            double center = Mojiban.Width / 2;
            posNum2 = (int)(center / 100);
            off2 = (int)(realValue) % 100;

            int i = 0;
            for(int dispPos = -posNum2; dispPos <= posNum2 + 1; dispPos++)
            {
                int dv = (int)(realValue / 100) * 100;
                int v = dv + dispPos * 100;
                Mojiban.Children.OfType<TextBlock>().ElementAt(i).Text = 
                    v.ToString();
                i++;
            }

            double widearea = (Mojiban.Width - DisplayArea.ActualWidth) / 2;
            Mojiban.Margin = new Thickness(-off2 - widearea, 0, off2 - widearea, 0);
        }
        #endregion

        #region ******************* sliderAnimationInit
        private void sliderAnimationInit()
        {
            if (myStoryboard.Children.Count != 0) myStoryboard.Children.Clear();
            myStoryboard.Children.Add(myDoubleAnimation);
            myDoubleAnimation.EasingFunction = new SineEase();
            ((EasingFunctionBase)myDoubleAnimation.EasingFunction).EasingMode = EasingMode.EaseInOut;
            myDoubleAnimation.Duration = new Duration(System.TimeSpan.FromSeconds(Duration));
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("DispValue"));
            Storyboard.SetTarget(myDoubleAnimation, this);
            myStoryboard.FillBehavior = FillBehavior.HoldEnd;

        }
        #endregion

        #region ******************* SliderAnimation
        private void SliderAnimation(double st, double end)
        {
            if (myDoubleAnimation == null) return;
            //myDoubleAnimation.From = st;
            myDoubleAnimation.To = end;
            BeginStoryboard(myStoryboard, HandoffBehavior.SnapshotAndReplace);
        }
        #endregion



        #region DependencyProperty : DispValue
        [Category("MeterProperty")]
        [Description("DispValue no Animation")]
        public double DispValue
        {
            get { return (double)this.GetValue(DispValueProperty); }
            set { this.SetValue(DispValueProperty, value); }
        }
        public static readonly DependencyProperty DispValueProperty =
            DependencyProperty.Register("DispValue", typeof(double),
                typeof(MeterSlide),
                new PropertyMetadata(0.0,
                    DispValueChangeFunc,
                    DispValueCoerceFunc));

        static void DispValueChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (double)e.OldValue;
            var nf = (double)e.NewValue;
            var obj = (MeterSlide)target;
            obj.realValue = nf;
            obj.MoveMemori();
            obj.MoveMoji();
            
        }

        static object DispValueCoerceFunc(DependencyObject target,
            object baseValue)
        {
            var obj = (MeterSlide)target;
            var val = (double)baseValue;

            return val;
        }
        #endregion

        #region DependencyProperty : DispValueAnime
        [Category("MeterProperty")]
        [Description("DispValue with Aimation")]
        public double DispValueAnime
        {
            get { return (double)this.GetValue(DispValueAnimeProperty); }
            set { this.SetValue(DispValueAnimeProperty, value); }
        }

        public static readonly DependencyProperty DispValueAnimeProperty =
            DependencyProperty.Register("DispValueAnime", typeof(double),
                typeof(MeterSlide),
                new PropertyMetadata(0.0,
                    DispValueAnimeChangeFunc,
                    DispValueAnimeCoerceFunc));

        static void DispValueAnimeChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (double)e.OldValue;
            var nf = (double)e.NewValue;
            var obj = (MeterSlide)target;
            obj.SliderAnimation(of, nf);
        }

        static object DispValueAnimeCoerceFunc(DependencyObject target, object baseValue)
        {
            var obj = (MeterSlide)target;
            var val = (double)baseValue;

            return val;
        }
        #endregion

        #region DependencyProperty : ScaleResolution
        [Category("MeterProperty")]
        [Description("Scale Resolution")]
        public double ScaleResolution
        {
            get { return (double)this.GetValue(ScaleResolutionProperty); }
            set { this.SetValue(ScaleResolutionProperty, value); }
        }

        public static readonly DependencyProperty ScaleResolutionProperty =
            DependencyProperty.Register("ScaleResolution", typeof(double),
                typeof(MeterSlide),
                new PropertyMetadata(10.0,
                    ScaleResolutionChangeFunc,
                    ScaleResolutionCoerceFunc));

        static void ScaleResolutionChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (double)e.OldValue;
            var nf = (double)e.NewValue;
            var obj = (MeterSlide)target;
            obj.memoribanInit();
        }

        static object ScaleResolutionCoerceFunc(DependencyObject target, object baseValue)
        {
            var obj = (MeterSlide)target;
            var val = (double)baseValue;

            return val;
        }
        #endregion

        #region DependencyProperty : Duration
        [Category("MeterProperty")]
        [Description("Duration for Animation sec")]
        public double Duration
        {
            get { return (double)this.GetValue(DurationProperty); }
            set { this.SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double),
                typeof(MeterSlide),
                new PropertyMetadata(0.5,
                    DurationChangeFunc,
                    DurationCoerceFunc));

        static void DurationChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (double)e.OldValue;
            var nf = (double)e.NewValue;
            var obj = (MeterSlide)target;
            obj.sliderAnimationInit();
        }

        static object DurationCoerceFunc(DependencyObject target, object baseValue)
        {
            var obj = (MeterSlide)target;
            var val = (double)baseValue;

            return val;
        }
        #endregion

    }


}