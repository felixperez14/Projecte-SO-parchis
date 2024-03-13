using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        public string correo, nombreUsuario, contraseña;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse(IP.Text);
            IPEndPoint ipep = new IPEndPoint(direc, 9080);
            

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (consulta1.Checked)
            {
                string mensaje = "1/" + nombreUsuario + "/" + contraseña + "/" + correo;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];
                MessageBox.Show("La respuesta a la consulta 1 és: " + mensaje);
            }
            else if (consulta2.Checked)
            {
                string mensaje = "1/" + nombreUsuario + "/" + contraseña + "/" + correo;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split (',')[0];
                MessageBox.Show("La respuesta a la consulta 2 és: " + mensaje);
            }
            else if (consulta3.Checked)
            {
                string mensaje = "1/" + nombreUsuario + "/" + contraseña + "/" + correo;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                MessageBox.Show("La respuesta a la consulta 3 és: " + mensaje);
            }
            else
            {
                MessageBox.Show("Seleccione una consulta.");
            }

            // Se terminó el servicio. 
            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

        }

        private void signupButton_Click(object sender, EventArgs e)
        {
            consultasBox.Visible = false;
            signupLabel.Visible = true;
            signupCorreoLabel.Visible = true;
            signupUsuarioLabel.Visible = true;
            signupContraseñaLabel.Visible = true;
            signupCorreotextBox.Visible = true;
            signupUsuariotextBox.Visible = true;
            signupContraseñatextBox.Visible = true;
            signupRegistrarButton.Visible = true;

            loginLabel.Visible = false;
            loginUsuarioLabel.Visible = false;
            loginContraseñaLabel.Visible = false;
            loginUsuariotextBox.Visible = false;
            loginContraseñatextBox.Visible = false;
            loginEntrarButton.Visible = false;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            consultasBox.Visible = false;
            loginLabel.Visible = true;
            loginUsuarioLabel.Visible = true;
            loginContraseñaLabel.Visible = true;
            loginUsuariotextBox.Visible = true;
            loginContraseñatextBox.Visible = true;
            loginEntrarButton.Visible = true;

            signupLabel.Visible = false;
            signupCorreoLabel.Visible = false;
            signupUsuarioLabel.Visible = false;
            signupContraseñaLabel.Visible = false;
            signupCorreotextBox.Visible = false;
            signupUsuariotextBox.Visible = false;
            signupContraseñatextBox.Visible = false;
            signupRegistrarButton.Visible = false;
        }

        private void loginEntrarButton_Click(object sender, EventArgs e)
        {
            if (loginUsuariotextBox.Text != "" && loginContraseñatextBox.Text != "")
            {
                //envio en la base de datos los datos intorduciodos;
                string mensaje = $"2/{loginUsuariotextBox.Text}/{loginContraseñatextBox.Text}";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //recivimos la respuesta del Server.
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];

                if(mensaje == "0")
                {
                    MessageBox.Show("Bienvenido de nuevo, " + loginUsuariotextBox.Text + "!");
                    consultasBox.Visible = true;
                    consultasNombretextBox.Text = loginUsuariotextBox.Text;
                    nombreUsuario = consultasNombretextBox.Text;
                }
                else
                {
                    MessageBox.Show("Te has equivocado de contraseña o usuario.");
                }

                
            }
            else
            {
                MessageBox.Show("Debes rellenar todos los campos.");
            }
        }

        private void signupRegistrarButton_Click(object sender, EventArgs e)
        {
            if (signupCorreotextBox.Text != "" && signupUsuariotextBox.Text != "" && signupContraseñatextBox.Text != "")
            {
                 

                MessageBox.Show("Usuario registrado con éxito.");
                consultasBox.Visible = true;
                consultasNombretextBox.Text = signupUsuariotextBox.Text;
                nombreUsuario = consultasNombretextBox.Text;
            }
            else
            {
                MessageBox.Show("Debes rellenar todos los campos.");
            }
        }
    }
}
