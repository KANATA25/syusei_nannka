using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using System.Windows.Forms;

namespace Form_Test
{
    public class Test_Button
    {
        //ランダム関数
        private static Random random = new Random();
        private bool Boolrandom;
        private Color _onCoLOr = Color.LightGreen;
        private Color _ofCoLOr = Color.LightGray;
        private bool _eneble;
        private Form1 _form1;
        //<summary>横位置<summary>
        private int _x;
        //<summary>縦位置<summary>
        private int _y;

        public Point Location { get; }

        public Test_Button(Form1 form1, int x, int y, Point potision, Size size, string text)
        {
            Boolrandom = random.Next(0, 2) == 0;

            //Form1の参照を補完

            _form1 = form1;

            //横位置を参照

            _x = x;

            //縦位置を参照

            _y = y;

            //ボタンの位置を設定

            Location = potision;

            //ボタンの大きさを設定

            Size = size;

            Text = text;

         


            Click += hogehogwClick;

        }
    }
}

