using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;

namespace ControllerTest
{
    public partial class Debug : Form
    {
        private XInputController _controller;

        public Debug(XInputController controller)
        {
            InitializeComponent();

            _controller = controller;
        }

        private void Debug_Load(object sender, EventArgs e)
        {
            Logger.TS_StatusLabel = statusLabel;
            UpdateValues();
        }

        bool prevDisconnected = false;
        private async void UpdateValues()
        {
            while (true)
            {
                await Task.Delay(100);

                if (_controller.Connected == false)
                {
                    Logger.Log("Controller not connected. Reconnecting...", LogStyle.Error);
                    UpdateValuesWithText("No connection");
                    actionsGroup.Enabled = false;
                    _controller.TryConnect();
                    prevDisconnected = true;
                    Text = "Debug - Disconnected";
                    continue;
                }
                else
                {
                    Text = "Debug - Connected";
                }

                if (prevDisconnected)
                {
                    Logger.Log("Connected.");
                    prevDisconnected = false;
                    actionsGroup.Enabled = true;
                }

                _controller.Update();

                try
                {
                    rightThumbX.Text = _controller.rightThumb.X.ToString();
                    rightThumbY.Text = _controller.rightThumb.Y.ToString();
                    leftThumbX.Text = _controller.leftThumb.X.ToString();
                    leftThumbY.Text = _controller.leftThumb.Y.ToString();
                    rightTrigger.Text = _controller.RightTrigger.ToString();
                    leftTrigger.Text = _controller.LeftTrigger.ToString();
                    buttons.Text = _controller.Gamepad.Buttons.ToString();
                    batteryLevel.Text = _controller.Controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel.ToString();
                }
                catch { }
            }
        }

        private void UpdateValuesWithText(string txt)
        {
            rightThumbX.Text = txt;
            rightThumbY.Text = txt;
            leftThumbX.Text = txt;
            leftThumbY.Text = txt;
            rightTrigger.Text = txt;
            leftTrigger.Text = txt;
            buttons.Text = txt;
            batteryLevel.Text = txt;
        }

        private void vibrateRightButton_Click(object sender, EventArgs e)
        {
            if (lockVibration.Checked)
            {
                Logger.Log("Vibration currently locked. Set 'Lock Vibration' to false.", LogStyle.Warning);
                return;
            }

            Vibrate(false, true);
        }
        private void vibrateLeftButton_Click(object sender, EventArgs e)
        {
            if (lockVibration.Checked)
            {
                Logger.Log("Vibration currently locked. Set 'Lock Vibration' to false.", LogStyle.Warning);
                return;
            }

            Vibrate(true, false);
        }
        private void vibrateButton_Click(object sender, EventArgs e)
        {
            if (lockVibration.Checked)
            {
                Logger.Log("Vibration currently locked. Set 'Lock Vibration' to false.", LogStyle.Warning);
                return;
            }

            Vibrate(true, true);
        }
        private void vibrateSetZero_Click(object sender, EventArgs e)
        {
            if (lockVibration.Checked)
            {
                Logger.Log("Vibration currently locked. Set 'Lock Vibration' to false.", LogStyle.Warning);
                return;
            }

            Vibrate(false, false);
        }
        private void vibrationLevel_Scroll(object sender, EventArgs e)
        {
            if (lockVibration.Checked)
            {
                Vibrate(true, true);
            }
        }
        private void lockVibration_CheckedChanged(object sender, EventArgs e)
        {
            if (lockVibration.Checked)
            {
                Vibrate(true, true);
            }
            else
            {
                Vibrate(false, false);
            }
        }
       
        private void Vibrate(bool left, bool right)
        {
            Vibration vibration = new Vibration();
            double speed = (double)ushort.MaxValue * ((double)vibrationLevel.Value / (double)100);

            if (left) 
                vibration.LeftMotorSpeed = (ushort)speed;
            else
                vibration.LeftMotorSpeed = 0;

            if (right)
                vibration.RightMotorSpeed = (ushort)speed;
            else
                vibration.RightMotorSpeed = 0;

            _controller.Controller.SetVibration(vibration);
        }
    }
}
