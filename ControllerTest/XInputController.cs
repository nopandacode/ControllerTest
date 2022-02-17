using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace ControllerTest
{
    public class XInputController
    {

        public bool Connected { get; set; } = false;
        public int DeadBand { get; set; } = 2500;
        public PointF leftThumb, rightThumb = new Point(0, 0);
        public float LeftTrigger { get; set; }
        public float RightTrigger { get; set; }
        public Gamepad Gamepad { get; set; }
        public Controller Controller { get; set; }

        public XInputController()
        {
            TryConnect();
        }

        public void TryConnect()
        {
            Controller = new Controller(UserIndex.One);
            Connected = Controller.IsConnected;
        }

        public void Update()
        {
            if (!Connected)
            {
                TryConnect();
            }

            try
            {
                Gamepad = Controller.GetState().Gamepad;
            }
            catch
            {
                Connected = false;
                return;
            }

            leftThumb.X = (Math.Abs((float)Gamepad.LeftThumbX) < DeadBand) ? 0 : (float)Gamepad.LeftThumbX / short.MinValue * -100;
            leftThumb.Y = (Math.Abs((float)Gamepad.LeftThumbY) < DeadBand) ? 0 : (float)Gamepad.LeftThumbY / short.MaxValue * 100;
            rightThumb.X = (Math.Abs((float)Gamepad.RightThumbX) < DeadBand) ? 0 : (float)Gamepad.RightThumbX / short.MaxValue * 100;
            rightThumb.Y = (Math.Abs((float)Gamepad.RightThumbY) < DeadBand) ? 0 : (float)Gamepad.RightThumbY / short.MaxValue * 100;

            LeftTrigger = Gamepad.LeftTrigger;
            RightTrigger = Gamepad.RightTrigger;
        }
    }
}
