using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace WpfControlLibrary
{
    /// <summary>
    /// MeterCircle1.xaml の相互作用ロジック
    /// </summary>
    public partial class MeterCircle : UserControl
    {
        public MeterCircle()
        {
            InitializeComponent();
        }

        #region DependencyProperty : MeterMax
        [Category("MeterProperty")]
        [Description("Maximum Limit")]
        public double MeterMax
        {
            get { return (double)this.GetValue(MeterMaxProperty); }
            set { this.SetValue(MeterMaxProperty, value); }
        }
        public static readonly DependencyProperty MeterMaxProperty =
          DependencyProperty.Register("MeterMax", typeof(double),
                                       typeof(MeterCircle),
                                       new PropertyMetadata(135.0,
                                           MeterMaxChangedFunc,
                                           MeterMaxCoerceFunc));
        // 変更時のコールバック処理。
        static void MeterMaxChangedFunc(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var nf = (double)e.NewValue;
            MeterCircle obj = (MeterCircle)target;
        }
        // 値の強制。整形したり並べ替えたり。これが呼ばれた後で、PropertyChangedCallbackが呼ばれる
        static object MeterMaxCoerceFunc(DependencyObject target, object baseValue)
        {
            MeterCircle obj = (MeterCircle)target;
            var val = (double)baseValue;
            return val;
        }
        #endregion

        #region DependencyProperty : MeterMin
        [Category("MeterProperty")]
        [Description("Minimum Limit")]
        public double MeterMin
        {
            get { return (double)this.GetValue(MeterMinProperty); }
            set { this.SetValue(MeterMinProperty, value); }
        }
        public static readonly DependencyProperty MeterMinProperty =
          DependencyProperty.Register("MeterMin", typeof(double),
                                       typeof(MeterCircle),
                                       new PropertyMetadata(-135.0,
                                           MeterMinChangedFunc,
                                           MeterMinCoerceFunc));
        // 変更時のコールバック処理。
        static void MeterMinChangedFunc(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var nf = (double)e.NewValue;
            MeterCircle obj = (MeterCircle)target;

        }
        // 値の強制。整形したり並べ替えたり。これが呼ばれた後で、PropertyChangedCallbackが呼ばれる
        static object MeterMinCoerceFunc(DependencyObject target, object baseValue)
        {
            MeterCircle obj = (MeterCircle)target;
            var val = (double)baseValue;

            return val;
        }
        #endregion

        #region DependencyProperty : MeterValue
        [Category("MeterProperty")]
        [Description("Meter Value")]
        public double MeterValue
        {
            get { return (double)this.GetValue(MeterValueProperty); }
            set { this.SetValue(MeterValueProperty, value); }
        }
        public static readonly DependencyProperty MeterValueProperty =
          DependencyProperty.Register("MeterValue", typeof(double),
                                       typeof(MeterCircle),
                                       new PropertyMetadata(0.0,
                                           MeterValueChangedFunc,
                                           MeterValueCoerceFunc));
        // 変更時のコールバック処理。
        static void MeterValueChangedFunc(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var nf = (double)e.NewValue;
            MeterCircle obj = (MeterCircle)target;
            obj.DispValue.Value = (nf - obj.MeterMin) / (obj.MeterMax - obj.MeterMin) * 270.0 - 135.0;
        }

        // 値の強制。整形したり並べ替えたり。これが呼ばれた後で、PropertyChangedCallbackが呼ばれる
        static object MeterValueCoerceFunc(DependencyObject target, object baseValue)
        {
            MeterCircle obj = (MeterCircle)target;
            var val = (double)baseValue;
            if (val > obj.MeterMax) val = obj.MeterMax;
            if (val < obj.MeterMin) val = obj.MeterMin;
            return val;
        }
        #endregion

        #region DependencyProperty : UnitString
        [Category("MeterProperty")]
        [Description("Display string")]
        public string UnitString
        {
            get { return (string)this.GetValue(UnitStringProperty); }
            set { this.SetValue(UnitStringProperty, value); }
        }

        public static readonly DependencyProperty UnitStringProperty =
            DependencyProperty.Register("UnitString", typeof(string),
                typeof(MeterCircle),
                new PropertyMetadata("0.01 mm/目盛",
                    UnitStringChangeFunc,
                    UnitStringCoerceFunc));

        static void UnitStringChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (string)e.OldValue;
            var nf = (string)e.NewValue;
            var obj = (MeterCircle)target;
            obj.Unit.Text = nf;
        }

        static object UnitStringCoerceFunc(DependencyObject target, object baseValue)
        {
            var obj = (MeterCircle)target;
            var val = (string)baseValue;

            return val;
        }
        #endregion


    }
}
