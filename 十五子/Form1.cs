using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiftyNum
{
    public partial class Form1 : Form
    {
        const int N = 4; //行列数
        Button[,] buttons = new Button[N, N]; //按钮阵列
        public int swapCounter = 0;
        public int gametime = 0;

        public Form1()
        {
            InitializeComponent();
            button1.Text = "开始";
            Text = "十五子";
            label1.Text = "次数:0";
            label2.Text = "0:00";
            timer1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //打乱按钮阵列顺序
            StarGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化按钮阵列
            InitializeButtonsArray();
        }

        void InitializeButtonsArray()
        {
            int x0 = 10, y0 = 10, w = 45, d = 50;
            for (int r = 0; r < N; r++)
                for (int c = 0; c < N; c++)
                {
                    int num = r * N + c;
                    Button btn = new Button();
                    btn.Text = (num + 1).ToString();

                    btn.Top = y0 + r * d;
                    btn.Left = x0 + c * d;

                    btn.Width = w;
                    btn.Height = w;

                    btn.Visible = true;
                    btn.Enabled = false;

                    btn.Tag = num; //按钮序号

                    //注册事件
                    btn.Click += new EventHandler(Btn_Click);

                    buttons[r, c] = btn; //新按钮放入按钮阵列
                    this.Controls.Add(btn); //加载到界面
                }
            buttons[N - 1, N - 1].Visible = false; //留一个空位
        }

        void StarGame()
        {
            //多次随机交换两个按钮
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                int r1 = rnd.Next(N);
                int r2 = rnd.Next(N);
                int r3 = rnd.Next(N);
                int r4 = rnd.Next(N);
                Swap(buttons[r1, r2], buttons[r3, r4]);
            }
            for (int r = 0; r < N; r++)
                for (int c = 0; c < N; c++)
                {
                    buttons[r, c].Enabled = true;
                }

            //开启计时器
            timer1.Enabled = true;
        }

        //交换两个按钮
        void Swap(Button b1, Button b2)
        {
            string tmpText = b1.Text;
            b1.Text = b2.Text;
            b2.Text = tmpText;

            bool tmpVisible = b1.Visible;
            b1.Visible = b2.Visible;
            b2.Visible = tmpVisible;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button current = sender as Button; //当前点中按钮
            Button blank = FindeHiddenButton(); //空白按钮

            if (IsNeighbor(current, blank))
            {
                Swap(current, blank);
                swapCounter += 1;
                label1.Text = "次数:" + swapCounter.ToString();
                blank.Focus();
            }

            if (Victory())
            {
                timer1.Enabled = false;
                MessageBox.Show("胜利！\n\t用时"+label2.Text+"\n\t点击"+swapCounter+"次","游戏结束！");
                Close();
            }
        }

        Button FindeHiddenButton()
        {
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (!buttons[r, c].Visible)
                    {
                        return buttons[r, c];
                    }
                }
            }
            return null;
        }

        bool IsNeighbor(Button b1, Button b2)
        {
            int t1 = (int)b1.Tag;
            int t2 = (int)b2.Tag;
            //求行列号
            int r1 = t1 / N, c1 = t1 % N;
            int r2 = t2 / N, c2 = t2 % N;

            if (r1 == r2 && (c1 == c2 + 1 || c1 == c2 - 1) || c1 == c2 && (r1 == r2 + 1 || r1 == r2 - 1))
            {
                return true;
            }
            return false;
        }

        bool Victory()
        {
            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    if (buttons[r, c].Text != (r * N + c + 1).ToString())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime t = new DateTime();
            gametime += 1;
            label2.Text = t.AddSeconds(gametime).ToString("m:ss");
        }
    }
}
