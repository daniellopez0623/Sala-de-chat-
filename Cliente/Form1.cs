using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Cliente
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		#region " Variables "
		System.Net.Sockets.TcpClient Cliente;
		System.Net.Sockets.NetworkStream Stream;
		#endregion
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;
		#region " Constructors "
		public Form1()
		{
			InitializeComponent();
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(672, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "localhost";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(784, 24);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Connect";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(656, 320);
			this.label1.TabIndex = 2;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(8, 336);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(656, 20);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "";
			this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(704, 336);
			this.button2.Name = "button2";
			this.button2.TabIndex = 4;
			this.button2.Text = "button2";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(864, 362);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		private void Form1_Load(object sender, System.EventArgs e)
		{
			Stream= null;
			this.Text="Desconectado";
		}
		private void button1_Click(object sender, System.EventArgs e)
		{	
			Cliente = new System.Net.Sockets.TcpClient();
			Cliente.Connect(textBox1.Text , 2020);
			Stream = Cliente.GetStream();
			this.Text= "Conectado";
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if (Stream.CanWrite && Stream!=null)
			{
				byte [] buff = new byte[256];
				buff = System.Text.Encoding.ASCII.GetBytes(textBox2.Text);
				Stream.Write(buff,0,(int)buff.Length);
				label1.Text+= "\n" + textBox2.Text;
				textBox2.Text= "";
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if (Stream != null)
			if (Stream.CanRead && Stream.DataAvailable)
			{
				byte []buff = new byte[256]; 
				string str=" ";
				try
				{
					Stream.Read(buff,0,256);
					str = System.Text.Encoding.ASCII.GetString(buff);
				}
				catch(System.Exception X)
				{
					this.label1.Text = "\n" + X.Message+"\n";
				}
				finally
				{
					label1.Text += "\n" + str;
				}
			}
		}
		private void textBox2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
				button2_Click(null,null);						
		}
	}
}
