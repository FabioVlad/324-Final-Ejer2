using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication5
{   
    
    public partial class Form1 : Form
    {
        int cR, cG, cB,cRt,cGt,cBt;
        SqlConnection cn = new SqlConnection("SERVER=localhost;DATABASE=COLORES;integrated security=true;");        public Form1()
        {
            InitializeComponent();
            cargar_datos();
        }
        public void cargar_datos()
        {
            cn.Open();
            SqlCommand cm = new SqlCommand("select codigo,nombre from color", cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cn.Close();

            DataRow fila = dt.NewRow();
            fila["nombre"] = "Color Elija";
            dt.Rows.InsertAt(fila, 0);

            comboBox1.ValueMember = "codigo";
            comboBox1.DisplayMember = "nombre";
            comboBox1.DataSource = dt;
        }
        public void cargar_datos2()
        {
            cn.Open();
            SqlCommand cm = new SqlCommand("select codigo,nombre from color", cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cn.Close();

            DataRow fila = dt.NewRow();
            fila["nombre"] = "Color Elija";
            dt.Rows.InsertAt(fila, 0);

            comboBox3.ValueMember = "codigo";
            comboBox3.DisplayMember = "nombre";
            comboBox3.DataSource = dt;
        }

        public void carga_varia(String codigo)
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("select codvar, rgb from variaciones where codigo= @codigo", cn);
            cmd.Parameters.AddWithValue("codigo", codigo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataRow dr = dt.NewRow();
            dr["rgb"] = "variacion";
            dt.Rows.InsertAt(dr, 0);
            cn.Close();

            comboBox2.ValueMember = "codvar";
            comboBox2.DisplayMember = "rgb";
            comboBox2.DataSource = dt;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            c = bmp.GetPixel(0, 0);
            textBox1.Text = c.R.ToString();
            textBox2.Text = c.G.ToString();
            textBox3.Text = c.B.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox4.Text))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(DISTINCT codigo) codigo FROM color;", cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int ve = 0;

                foreach (DataRow row in dt.Rows)
                {
                    ve = int.Parse(row["codigo"].ToString());
                }
                ve = ve + 1;
                String nu = ve.ToString();
                MessageBox.Show("INSERTADO CORRECTAMENTE CODIGO: "+ve + "");
                SqlCommand cm = new SqlCommand("INSERT INTO color VALUES(@nu,@nombre)", cn);
                cm.Parameters.AddWithValue("@nu", nu);
                cm.Parameters.AddWithValue("@nombre", textBox4.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                button10.Visible = false;
                textBox4.Visible =false;
            }
            else
            {
                MessageBox.Show("INGRESE NOMBRE");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button10.Visible = true;
            textBox4.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comboBox3.Visible = true;
            button5.Visible = true;
            cargar_datos2();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox3.SelectedValue.ToString()))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(DISTINCT codvar) codvar FROM variaciones;", cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int ve = 0;

                foreach (DataRow row in dt.Rows)
                {
                    ve = int.Parse(row["codvar"].ToString());
                }
                ve = ve + 1;
                String nu = ve.ToString();
                String vrgb = cR + "," + cB+ "," + cG ;
                MessageBox.Show("INSERTADO CORRECTAMENTE CODIGO: " + ve + "");
                SqlCommand cm = new SqlCommand("INSERT INTO variaciones VALUES(@cdv,@rgb,@cod);", cn);
                cm.Parameters.AddWithValue("@cdv", nu);
                cm.Parameters.AddWithValue("@rgb", vrgb);
                cm.Parameters.AddWithValue("@cod", comboBox3.SelectedValue.ToString());
                cm.ExecuteNonQuery();
                cn.Close();
                //MessageBox.Show(this.comboBox3.SelectedValue.ToString());
                comboBox3.Visible = false;
                button5.Visible = false;
            }
            else
            {
                MessageBox.Show("SELECCIONE UN COLOR");
            }
            
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cargar_datos();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            c = bmp.GetPixel(e.X,e.Y);
            
            cR = c.R; cG = c.G; cB = c.B;
            cRt = 0; cGt = 0; cBt = 0;
            for(int i = e.X; i < e.X + 10; i++)
                for (int j = e.Y; j < e.Y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    cRt = cRt+c.R; cGt = cGt+c.G; cBt = cBt+c.B;
                }
            cRt = cRt / 100;
            cGt = cGt / 100;
            cBt = cBt / 100;
            
            textBox1.Text = c.R.ToString() + " " +cRt.ToString();
            textBox2.Text = c.G.ToString() + " " +cGt.ToString();
            textBox3.Text = c.B.ToString() + " " +cBt.ToString();
            
        }



       

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() != null)
            {
                String codigo = comboBox1.SelectedValue.ToString();
                carga_varia(codigo);
            }
        }


        
        private void button8_Click(object sender, EventArgs e)
        {
            cRt = 134; cGt = 128; cBt =106;
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            int cRto, cGto, cBto;
            Color c = new Color();
            for (int i = 0; i < bmp.Width-20; i=i+10)
                for (int j = 0; j < bmp.Height-20; j=j+10)
                {

                    cRto = 0; cGto = 0; cBto = 0;
                    for (int k = i; k < i +10; k++)
                        for (int l = j; l < j + 10; l++)
                        {
                            c = bmp.GetPixel(k, l);
                            cRto = c.R + cRto; cGto = c.G + cGto; cBto = c.B + cBto;
                        }
                    cRto = cRto / 100;
                    cGto = cGto / 100;
                    cBto = cBto / 100;
                    c = bmp.GetPixel(i, j);
                    if (((cRt - 10 <= cRto) && (cRto <= cRt + 10)) && ((cGto - 10 <= cGt) && (cGt <= cGto + 10)) && ((cBto - 10 <= cBt) && (cBt <= cBto + 10)))
                        for (int k = i; k < i + 10; k++)
                            for (int l = j; l < j + 10; l++)
                            {
                                bmp2.SetPixel(k, l, Color.Black);
                            }
                    else
                        for (int k = i; k < i + 10; k++)
                            for (int l = j; l < j + 10; l++)
                            {
                                c = bmp.GetPixel(k, l);
                                bmp2.SetPixel(k, l, c);
                            }
                }
            pictureBox2.Image = bmp2;
        }


        private void button9_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox1.SelectedValue.ToString()))
            {
                String dato = comboBox1.SelectedValue.ToString();
                //MessageBox.Show(dato);
                cn.Open();
                SqlCommand cmd = new SqlCommand("select codvar, rgb from variaciones where codigo= '" + dato + "'", cn);
                cmd.Parameters.AddWithValue("codigo", dato);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);


                int sw = 0;
                foreach (DataRow row in dt.Rows)
                {
                    //MessageBox.Show(row["rgb"].ToString());

                    string s = row["rgb"].ToString();
                    string[] subs = s.Split(',');
                    int v1 = int.Parse(subs[0]);
                    int v2 = int.Parse(subs[1]);
                    int v3 = int.Parse(subs[2]);
                    //MessageBox.Show(v1 + ";" + v2 + ";"+v3);
                    cRt = v1; cGt = v3; cBt = v2;
                    //cRt = 12; cGt = 42; cBt = 63;
                    Bitmap bmp;
                    Bitmap bmp2;
                    if (sw == 0)
                    {
                        bmp = new Bitmap(pictureBox1.Image);
                        bmp2 = new Bitmap(bmp.Width, bmp.Height);
                        sw = 1;
                    }
                    else
                    {
                        bmp = new Bitmap(pictureBox2.Image);
                        bmp2 = new Bitmap(bmp.Width, bmp.Height);
                    }

                    //Bitmap bmp = new Bitmap(pictureBox1.Image);
                    //Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);

                    int cRto, cGto, cBto;
                    Color c = new Color();
                    for (int i = 0; i < bmp.Width - 20; i = i + 10)
                        for (int j = 0; j < bmp.Height - 20; j = j + 10)
                        {

                            cRto = 0; cGto = 0; cBto = 0;
                            for (int k = i; k < i + 10; k++)
                                for (int l = j; l < j + 10; l++)
                                {
                                    c = bmp.GetPixel(k, l);
                                    cRto = c.R + cRto; cGto = c.G + cGto; cBto = c.B + cBto;
                                }
                            cRto = cRto / 100;
                            cGto = cGto / 100;
                            cBto = cBto / 100;

                            if (((cRt - 10 <= cRto) && (cRto <= cRt + 10)) && ((cGto - 10 <= cGt) && (cGt <= cGto + 10)) && ((cBto - 10 <= cBt) && (cBt <= cBto + 10)))
                            {
                                for (int k = i; k < i + 10; k++)
                                    for (int l = j; l < j + 10; l++)
                                    {
                                        bmp2.SetPixel(k, l, Color.Black);
                                    }
                            }
                            else
                            {
                                for (int k = i; k < i + 10; k++)
                                    for (int l = j; l < j + 10; l++)
                                    {
                                        c = bmp.GetPixel(k, l);
                                        bmp2.SetPixel(k, l, c);
                                    }
                            }
                        }
                    pictureBox2.Image = bmp2;

                }



                cn.Close();
            }
            else
            {
                MessageBox.Show("DEBE ELEGIR ALGUN COLOR");
            }
                
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}
