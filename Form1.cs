using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace DBBYTST
{
    public partial class dababy : Form
    {

	// start of move & resize borderless window code
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void dababy_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private const int
    HTLEFT = 10,
    HTRIGHT = 11,
    HTTOP = 12,
    HTTOPLEFT = 13,
    HTTOPRIGHT = 14,
    HTBOTTOM = 15,
    HTBOTTOMLEFT = 16,
    HTBOTTOMRIGHT = 17;

        const int _ = 10; // you can rename this variable if you like

        Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }


        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }
	//end of borderless window resize & move
        public dababy()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            Thread dbby = new Thread(Baby);
            dbby.IsBackground = true;
            dbby.Start();
        }
        int counter = 0;
        string prevgo = string.Empty;
        bool keepR = true;

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        int mbtncount = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if (mbtncount == 0)
            {
                this.WindowState = FormWindowState.Maximized;
                mbtncount = 1;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                mbtncount = 0;
            }
            resizeevent();
        }
        
        private void resizeevent()
        {
            button1.Left = this.Width - button1.Width - 10;
            button2.Left = button1.Left - button2.Width - 10;
            button3.Left = button2.Left - button3.Width - 10;
            label1.Left = (int)this.Width / 2 - label1.Width;
        }

        private void dababy_Resize(object sender, EventArgs e)
        {
            resizeevent();
        }

        bool keepL = false;
        bool keepT = false;
        bool keepB = true;
        Random rnd = new Random();
        private void Baby()
        {
            while (true)
            {
                int increm = rnd.Next(1, 3);
                int lPos = dababypic.Left;
                int tPos = dababypic.Top;
                int bPos = this.Height - dababypic.Top - 100;
                int rPos = this.Width - dababypic.Left - 100;
                bool hitBot = false;
                bool hitRight = false;
                bool hitLeft = false;
                bool hitTop = false;
                if (lPos == 0)
                {
                    hitLeft = true;
                        //playSimpleSound();
				//uncomment the above line to play a sound when it hits left
                }
                if (tPos == 0)
                {
                    hitTop = true;
                        //playSimpleSound();
                }
                if (bPos == 0)
                {
                    hitBot = true;
                    //playSimpleSound();
                }
                if (rPos == 0)
                {
                    hitRight = true;
                        //playSimpleSound();
                }
                if (hitLeft != true && keepL == true)
                    lPos-=increm;
                else
                {
                    keepL = false;
                    keepR = true;
                    lPos+=increm;
                }
                if (hitTop != true && keepT == true)
                    tPos-=increm;
                else
                {
                    keepT = false;
                    keepB = true;
                    tPos+=increm;
                }
                if (hitRight != true && keepR == true)
                    lPos+=increm;
                else
                {
                    keepR = false;
                    keepL = true;
                    lPos-=increm;
                }
                if (hitBot != true && keepB == true)
                    tPos+=increm;
                else
                {
                    keepB = false;
                    keepT = true;
                    tPos-=increm;
                }
                // handle maximum
                if (lPos > this.Width - 100)
                    lPos = this.Width - 100;
                if (tPos > this.Height - 100)
                    tPos = this.Height - 100;
                if (lPos < 0)
                    lPos = 0;
                if (tPos < 0)
                    tPos = 0;
                if ((tPos == 0 && lPos == 0) || (tPos == 0 && rPos == 0) || (bPos == 0 && lPos == 0) || (bPos == 0 && rPos == 0))
                {
                    counter++;
                    playGO(); //play corner sound
                }
                try
                {
                    Invoke((MethodInvoker)delegate
                       {
                           dababypic.Top = tPos;
                           dababypic.Left = lPos;
                           label1.Text = "Corner hits: " + counter;
                       });
                }
                catch
                {
                    MessageBox.Show("Invoke error.",  "Exception thrown!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Thread.Sleep(40); //interval (aka speed) of the image
            }
        }
        private void playSimpleSound()
        {
            SoundPlayer yeye = new SoundPlayer(Properties.Resources.yeye); //replace with path to your sound or add to resources.resx
            yeye.Play();
        }
        private void playGO()
        {
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.lesgo); //replace with path to your sound or add to resources.resx
            simpleSound.Play();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
