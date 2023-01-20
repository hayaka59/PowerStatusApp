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
        /// �t�H�[���̃��[�h����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            GetBatteryStatus();
            LblTime.Text = "";
            IsStart = false;
            // �P�b�Ԋu�Ńo�b�e���[�̏󋵂��m�F����
            TimCheck.Interval = 1000;
            TimCheck.Enabled = true;
        }

        /// <summary>
        /// �o�b�e���[�̏[�d��Ԃ��擾����
        /// </summary>
        /// <returns></returns>
        private string GetBatteryChargeStatus()
        {
            string sMessage = "";
            try
            {
                // �o�b�e���[�̏[�d��Ԃ��擾����
                BatteryChargeStatus bcs = SystemInformation.PowerStatus.BatteryChargeStatus;
                if (bcs == BatteryChargeStatus.Unknown)
                {
                    sMessage = "�s���ł�";
                }
                else
                {
                    if ((bcs & BatteryChargeStatus.High) == BatteryChargeStatus.High)
                    {
                        sMessage = "�[�d���x���́A����(66%����)�ł�";
                    }
                    if ((bcs & BatteryChargeStatus.Low) == BatteryChargeStatus.Low)
                    {
                        sMessage = "�[�d���x���́A�Ⴂ(33%����)�ł�";
                    }
                    if ((bcs & BatteryChargeStatus.Critical) == BatteryChargeStatus.Critical)
                    {
                        sMessage = "�[�d���x���́A�Œ�(5%����)�ł�";
                    }
                    if ((bcs & BatteryChargeStatus.Charging) == BatteryChargeStatus.Charging)
                    {
                        sMessage = "�[�d���ł�";
                    }
                    if ((bcs & BatteryChargeStatus.NoSystemBattery) == BatteryChargeStatus.NoSystemBattery)
                    {
                        sMessage = "�o�b�e���[�����݂��܂���";
                    }
                }
                label0.Text = sMessage;
                return sMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "�G���[�yGetBatteryChargeStatus�z", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryChargeStatus()�ŃG���[���������܂���";
            }
        }

        private bool IsStart;
        private Stopwatch sw = new();

        /// <summary>
        /// AC�d���̏�Ԃ��擾����
        /// </summary>
        /// <returns></returns>
        private string GetPowerLineStatus()
        {
            string sMessage = "";
            try
            {
                // AC�d���̏�Ԃ��擾����
                PowerLineStatus pls = SystemInformation.PowerStatus.PowerLineStatus;
                switch (pls)
                {
                    case PowerLineStatus.Offline:
                        sMessage = "AC�d�����I�t���C���ł�";
                        if (!IsStart)
                        {
                            IsStart = true;
                            sw = new Stopwatch();
                            sw.Start();                            
                        }
                        break;
                    case PowerLineStatus.Online:
                        sMessage = "AC�d�����I�����C���ł�";
                        IsStart = false;
                        break;
                    case PowerLineStatus.Unknown:
                        sMessage = "AC�d���̏�Ԃ͕s���ł�";
                        break;
                }
                return sMessage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "�G���[�yGetPowerLineStatus�z", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetPowerLineStatus()�ŃG���[���������܂���";
            }
        }

        /// <summary>
        /// �o�b�e���[�c�ʁi�����j���擾����
        /// </summary>
        /// <returns></returns>
        private static string GetBatteryLifePercent()
        {
            string sMessage;
            try
            {

                // �o�b�e���[�c�ʁi�����j
                float blp = SystemInformation.PowerStatus.BatteryLifePercent;
                sMessage = "�o�b�e���[�c�ʂ́A" + blp * 100 + "%�ł�";                
                return sMessage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "�G���[�yGetBatteryLifePercent�z", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryLifePercent()�ŃG���[���������܂���";
            }
        }

        /// <summary>
        /// �o�b�e���[�c�ʁi���ԁj���擾����
        /// </summary>
        /// <returns></returns>
        private static string GetBatteryLifeRemaining()
        {
            string sMessage;
            try
            {
                // �o�b�e���[�c�ʁi���ԁj
                int blr = SystemInformation.PowerStatus.BatteryLifeRemaining;
                if (-1 < blr)
                {
                    float sSS = blr % 60;
                    float fwork = (float)Math.Floor((double)(blr / 60));
                    float sMM = fwork % 60;
                    float sHH = fwork / 60;
                    sMessage = "�o�b�e���[�c�莞�Ԃ́A" + sHH + "����" + sMM + "��" + sSS + "�b�ł�";
                }
                else
                {
                    //AC�d�����I�����C���̎��Ȃ�
                    sMessage = "�o�b�e���[�c�莞�Ԃ́A�s���ł�";
                }
                
                //MessageBox.Show(blr + "�b");

                return sMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "�G���[�yGetBatteryLifeRemaining�z", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryLifeRemaining()�ŃG���[���������܂���";
            }
        }

        /// <summary>
        /// �o�b�e���[���t���[�d���ꂽ���̎������ԁi�o�b�e���[�쓮���ԁj���擾����
        /// </summary>
        /// <returns></returns>
        private static string GetBatteryFullLifetime()
        {
            string sMessage;
            try
            {
                // �o�b�e���[���t���[�d���ꂽ���̎������ԁi�o�b�e���[�쓮���ԁj
                int bfl = SystemInformation.PowerStatus.BatteryFullLifetime;
                if (-1 < bfl)
                {
                    sMessage = "�o�b�e���[�쓮���Ԃ́A" + bfl + "�b�ł�";
                }
                else
                {
                    sMessage = "�o�b�e���[�쓮���Ԃ́A�s���ł�";
                }
                return sMessage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "�G���[�yGetBatteryFullLifetime�z", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "GetBatteryFullLifetime()�ŃG���[���������܂���";
            }
        }

        /// <summary>
        /// �S�Ẵo�b�e���[�̏�Ԃ��擾����
        /// </summary>
        private void GetBatteryStatus()
        {
            // �o�b�e���[�̏[�d��Ԃ��擾����
            label0.Text = GetBatteryChargeStatus();
            // AC�d���̏�Ԃ��擾����
            label1.Text = GetPowerLineStatus();
            // �o�b�e���[�c�ʁi�����j���擾����
            label2.Text = GetBatteryLifePercent();
            // �o�b�e���[�c�ʁi���ԁj���擾����
            label3.Text = GetBatteryLifeRemaining();
            // �o�b�e���[���t���[�d���ꂽ���̎������ԁi�o�b�e���[�쓮���ԁj���擾����
            label4.Text = GetBatteryFullLifetime();
        }

        /// <summary>
        /// ���Ԋu�Ńo�b�e���[�̏󋵂��m�F����
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
                    sTime += ts.Hours.ToString("00") + "���� ";
                }

                if (ts.Minutes > 0)
                {
                    sTime += ts.Minutes.ToString("00") + "�� ";
                }

                if (ts.Seconds > 0)
                {
                    sTime += ts.Seconds.ToString("00") + "�b ";
                }

                if (ts.Milliseconds > 0)
                {
                    sTime += ts.Milliseconds.ToString("000") + "�~���b";
                }

                //LblTime.Text = $"�@{ts.Hours}���� {ts.Minutes}�� {ts.Seconds}�b {ts.Milliseconds}�~���b";
                LblTime.Text = "�o�ߎ��ԁF" + sTime;
            }
            else
            {
                LblTime.Text = "";
            }
        }

        /// <summary>
        /// �u�X�V�v�{�^���̃N���b�N����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            GetBatteryStatus();
        }
    }
}