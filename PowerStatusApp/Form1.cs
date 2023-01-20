using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PowerStatusApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームのロード処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            GetBatteryStatus();
            LblTime.Text = "";
            IsStart = false;
            // １秒間隔でバッテリーの状況を確認する
            TimCheck.Interval = 1000;
            TimCheck.Enabled = true;
        }

        /// <summary>
        /// バッテリーの充電状態を取得する
        /// </summary>
        /// <returns></returns>
        private string GetBatteryChargeStatus()
        {
            string sMessage = "";
            try
            {
                // バッテリーの充電状態を取得する
                BatteryChargeStatus bcs = SystemInformation.PowerStatus.BatteryChargeStatus;
                if (bcs == BatteryChargeStatus.Unknown)
                {
                    sMessage = "不明です";
                }
                else
                {
                    if ((bcs & BatteryChargeStatus.High) == BatteryChargeStatus.High)
                    {
                        sMessage = "充電レベルは、高い(66%より上)です";
                    }
                    if ((bcs & BatteryChargeStatus.Low) == BatteryChargeStatus.Low)
                    {
                        sMessage = "充電レベルは、低い(33%未満)です";
                    }
                    if ((bcs & BatteryChargeStatus.Critical) == BatteryChargeStatus.Critical)
                    {
                        sMessage = "充電レベルは、最低(5%未満)です";
                    }
                    if ((bcs & BatteryChargeStatus.Charging) == BatteryChargeStatus.Charging)
                    {
                        sMessage = "充電中です";
                    }
                    if ((bcs & BatteryChargeStatus.NoSystemBattery) == BatteryChargeStatus.NoSystemBattery)
                    {
                        sMessage = "バッテリーが存在しません";
                    }
                }
                label0.Text = sMessage;
                return sMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "エラー【GetBatteryChargeStatus】", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryChargeStatus()でエラーが発生しました";
            }
        }

        private bool IsStart;
        private Stopwatch sw = new();

        /// <summary>
        /// AC電源の状態を取得する
        /// </summary>
        /// <returns></returns>
        private string GetPowerLineStatus()
        {
            string sMessage = "";
            try
            {
                // AC電源の状態を取得する
                PowerLineStatus pls = SystemInformation.PowerStatus.PowerLineStatus;
                switch (pls)
                {
                    case PowerLineStatus.Offline:
                        sMessage = "AC電源がオフラインです";
                        if (!IsStart)
                        {
                            IsStart = true;
                            sw = new Stopwatch();
                            sw.Start();                            
                        }
                        break;
                    case PowerLineStatus.Online:
                        sMessage = "AC電源がオンラインです";
                        IsStart = false;
                        break;
                    case PowerLineStatus.Unknown:
                        sMessage = "AC電源の状態は不明です";
                        break;
                }
                return sMessage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "エラー【GetPowerLineStatus】", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetPowerLineStatus()でエラーが発生しました";
            }
        }

        /// <summary>
        /// バッテリー残量（割合）を取得する
        /// </summary>
        /// <returns></returns>
        private static string GetBatteryLifePercent()
        {
            string sMessage;
            try
            {

                // バッテリー残量（割合）
                float blp = SystemInformation.PowerStatus.BatteryLifePercent;
                sMessage = "バッテリー残量は、" + blp * 100 + "%です";                
                return sMessage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "エラー【GetBatteryLifePercent】", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryLifePercent()でエラーが発生しました";
            }
        }

        /// <summary>
        /// バッテリー残量（時間）を取得する
        /// </summary>
        /// <returns></returns>
        private static string GetBatteryLifeRemaining()
        {
            string sMessage;
            try
            {
                // バッテリー残量（時間）
                int blr = SystemInformation.PowerStatus.BatteryLifeRemaining;
                if (-1 < blr)
                {
                    float sSS = blr % 60;
                    float fwork = (float)Math.Floor((double)(blr / 60));
                    float sMM = fwork % 60;
                    float sHH = fwork / 60;
                    sMessage = "バッテリー残り時間は、" + sHH + "時間" + sMM + "分" + sSS + "秒です";
                }
                else
                {
                    //AC電源がオンラインの時など
                    sMessage = "バッテリー残り時間は、不明です";
                }
                
                //MessageBox.Show(blr + "秒");

                return sMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "エラー【GetBatteryLifeRemaining】", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryLifeRemaining()でエラーが発生しました";
            }
        }

        /// <summary>
        /// バッテリーがフル充電された時の持ち時間（バッテリー駆動時間）を取得する
        /// </summary>
        /// <returns></returns>
        private static string GetBatteryFullLifetime()
        {
            string sMessage;
            try
            {
                // バッテリーがフル充電された時の持ち時間（バッテリー駆動時間）
                int bfl = SystemInformation.PowerStatus.BatteryFullLifetime;
                if (-1 < bfl)
                {
                    sMessage = "バッテリー駆動時間は、" + bfl + "秒です";
                }
                else
                {
                    sMessage = "バッテリー駆動時間は、不明です";
                }
                return sMessage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "エラー【GetBatteryFullLifetime】", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryFullLifetime()でエラーが発生しました";
            }
        }

        /// <summary>
        /// 全てのバッテリーの状態を取得する
        /// </summary>
        private void GetBatteryStatus()
        {
            // バッテリーの充電状態を取得する
            label0.Text = GetBatteryChargeStatus();
            // AC電源の状態を取得する
            label1.Text = GetPowerLineStatus();
            // バッテリー残量（割合）を取得する
            label2.Text = GetBatteryLifePercent();
            // バッテリー残量（時間）を取得する
            label3.Text = GetBatteryLifeRemaining();
            // バッテリーがフル充電された時の持ち時間（バッテリー駆動時間）を取得する
            label4.Text = GetBatteryFullLifetime();
        }

        /// <summary>
        /// 一定間隔でバッテリーの状況を確認する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimCheck_Tick(object sender, EventArgs e)
        {
            GetBatteryStatus();
            if (IsStart)
            {
                TimeSpan ts = sw.Elapsed;
                string sTime = "";
                
                if (ts.Hours > 0)
                {
                    sTime += ts.Hours.ToString("00") + "時間 ";
                }

                if (ts.Minutes > 0)
                {
                    sTime += ts.Minutes.ToString("00") + "分 ";
                }

                if (ts.Seconds > 0)
                {
                    sTime += ts.Seconds.ToString("00") + "秒 ";
                }

                if (ts.Milliseconds > 0)
                {
                    sTime += ts.Milliseconds.ToString("000") + "ミリ秒";
                }

                //LblTime.Text = $"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒";
                LblTime.Text = "経過時間：" + sTime;
            }
            else
            {
                LblTime.Text = "";
            }
        }

        /// <summary>
        /// 「更新」ボタンのクリック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            GetBatteryStatus();
        }
    }
}