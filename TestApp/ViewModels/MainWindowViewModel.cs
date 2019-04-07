using Prism.Mvvm;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }

        private bool ts_Status;
        /// <summary>
        /// トグルスイッチの表示状態
        /// 状態変化に応じた処理を実行する
        /// </summary>
        public bool Ts_Status
        {
            get { return ts_Status; }
            set {
                SetProperty(ref ts_Status, value);
                if (processError == false)
                {
                    Task.Run(() => Ts_Process(value));
                }
                else
                {
                    processError = false;
                }
            }
        }


        /// <summary>
        /// トグルスイッチの変化で起動する処理
        /// </summary>
        /// <param name="val"></param>
        public async void Ts_Process(bool val)
        {
            if (val)
            {
                bool ret = await Task.Run<bool>(() => TsTaskOn());
                if (!ret)
                {
                    processError = true;
                    Ts_Status = false;
                }
            }
            else
            {
                bool ret = await Task.Run<bool>(() => TsTaskOff());
                if (!ret)
                {
                    processError = true;
                    Ts_Status = true;
                }
            }

        }

        /// <summary>
        /// トグルスイッチがONに切り替わった時に行う処理
        /// </summary>
        /// <returns></returns>
        public bool TsTaskOn()
        {
            Thread.Sleep(500);
            bool ret = (true && !DummyFlag);
            //処理が成功したのでProcess_Statusをtrueにする
            if (ret) Process_Status = true;
            return ret;
        }

        /// <summary>
        /// トグルスイッチがOFFに切り替わった時に行う処理
        /// </summary>
        /// <returns></returns>
        public bool TsTaskOff()
        {
            Thread.Sleep(500);
            bool ret = (true && !DummyFlag);
            //処理が成功したのでProcess_Statusをfalseにする
            if (ret) Process_Status = false;
            return ret;
        }


        private bool process_Status;
        /// <summary>
        /// トグルスイッチで起動した処理の結果
        /// </summary>
        public bool Process_Status
        {
            get { return process_Status; }
            set
            {
                SetProperty(ref process_Status, value);
            }
        }

        /// <summary>
        /// 処理が失敗した時に元に戻すためのフラグ
        /// </summary>
        private bool processError = false;


        private bool dummyFlag = false;
        /// <summary>
        /// 処理を失敗させるためのダミーフラグ
        /// </summary>
        public bool DummyFlag
        {
            get { return dummyFlag; }
            set { SetProperty(ref dummyFlag, value); }
        }


    }
}
