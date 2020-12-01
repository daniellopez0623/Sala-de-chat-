using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Servidor
{
	public class Form1 : System.Windows.Forms.Form
	{
		#region " Propiedades "
			System.Net.Sockets.TcpListener Listener;	//Para habilitaru puerto
			System.Net.Sockets.TcpClient Cliente;		//Para obtener el Cliente
			System.Net.Sockets.NetworkStream Stream;	//Para el fichero de intercambio
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.ComponentModel.IContainer components;
		#endregion
		#region " Constructor "
			public Form1()
			{
				InitializeComponent();
			}
		#endregion
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(744, 240);
			this.label1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(592, 272);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Send";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(24, 272);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(544, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "";
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(760, 306);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
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
			label1.Text="";
			Listener = new System.Net.Sockets.TcpListener(2020);	//Inicializo el TCPListener en el puerto 2020
			this.Text = "Sensai.Net ChatServer... >;)";				
			Listener.Start();										//Pongo el puerto en Escucha
			Cliente = Listener.AcceptTcpClient();					//Acepto la conexion
			Stream = Cliente.GetStream();							//Obtengo el flujo de Datos
			byte []buff = new byte[256];							//Declaro arreglo de bytes para almacenar lo recibido
			buff = System.Text.Encoding.ASCII.GetBytes("Hola ya te conectaste");//Convierto la cadena de Saludo a Bytes	
			if (Stream.CanWrite)									//Si puedo escribir Escribo
				Stream.Write(buff,0,buff.Length);					
		}
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if (Stream != null) //Si hay conexion (existe el cliente)
				if (Stream.CanRead && Stream.DataAvailable) //Si  puedo leer y hay informacion disponible
				{
					byte []buff = new byte[256];	//Declaro un arreglo de bytes
					string str=" ";					//Declaro una cadena
					try
					{
						Stream.Read(buff,0,256);	//Leo los bytes del Stream
						str = System.Text.Encoding.ASCII.GetString(buff);//Convierto los bytes a Cadena
					}
					catch(System.Exception X)
					{
						this.label1.Text = "\n" + X.Message+"\n";	//Encaso de error lo muestro
					}
					finally
					{
						label1.Text += "\n" + str;	//Concateno lo que obtuve del Stream en mi label1
					}
				}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			byte []buff = new byte[256];								//Declaro un arreglo de bytes a transmitir
			buff = System.Text.Encoding.ASCII.GetBytes(textBox1.Text);	//Convierto la cadena en arreglo de bytes
			
			if (Stream.CanWrite)										//Su puedo escribir en mi NetWorkStream
				Stream.Write(buff,0,buff.Length);						//Escribe el arreglo de bytes
			label1.Text+= "\n" + textBox1.Text;
			textBox1.Text = "";
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
				button1_Click(null,null);						
		}
	}
}
