using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Library_MS
{
    public partial class view_student_info : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-AHCNDUA9\MSSQLSERVER02;Initial Catalog=library_management_system;Integrated Security=True;Pooling=False");
        int i = 0;
        string wanted_path;
        DialogResult result;
        string pwd = Class1.GetRandomPassword(20);
        public view_student_info()
        {
            InitializeComponent();
        }

        private void view_student_info_Load(object sender, EventArgs e)
        {

            string pwd = Class1.GetRandomPassword(20);
            DialogResult result;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            fillGrid();
        }
            public void fillGrid()
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                int i = 0;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from student_info";
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                Bitmap img;
                DataGridViewImageColumn imagecol = new DataGridViewImageColumn();

                imagecol.HeaderText = "Student Image";
                imagecol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imagecol.Width = 100;
                dataGridView1.Columns.Add(imagecol);

                foreach (DataRow dr in dt.Rows)
                {
                    img = new Bitmap(@"..\..\" + dr["student_image"].ToString());
                    dataGridView1.Rows[i].Cells[8].Value = img;
                    dataGridView1.Rows[i].Height = 100;
                    i++;
                }
            }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                int i = 0;

                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from student_info where students_name like '%"+ textBox1.Text +"%'";
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                Bitmap img;
                DataGridViewImageColumn imagecol = new DataGridViewImageColumn();

                imagecol.HeaderText = "Student Image";
                imagecol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imagecol.Width = 100;
                dataGridView1.Columns.Add(imagecol);

                foreach (DataRow dr in dt.Rows)
                {
                    img = new Bitmap(@"..\..\" + dr["student_image"].ToString());
                    dataGridView1.Rows[i].Cells[8].Value = img;
                    dataGridView1.Rows[i].Height = 100;
                    i++;
                }
            }

            catch( Exception ex )
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from student_info where id = "+ i +"";
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach( DataRow dr in dt.Rows )
            {
                student_name.Text = dr["students_name"].ToString();
                student_enrollment.Text = dr["student_enrollment_no"].ToString();
                student_dept.Text = dr["student_department"].ToString();
                student_sem.Text = dr["student_sem"].ToString();
                student_contact.Text = dr["student_contact"].ToString();
                student_email.Text = dr["student_email"].ToString();
            }
        }

        private void Select_Click(object sender, EventArgs e)
        {
            wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            result = openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files(.*jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (result == DialogResult.OK)
            {
                int i;
                i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

                string img_path;
                File.Copy(openFileDialog1.FileName, wanted_path + "\\student_images\\" + pwd + ".jpg");
                img_path = "\\student_images\\" + pwd + ".jpg";

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update student_info set students_name='" + student_name.Text + "',student_image='" + img_path.ToString() + "',student_enrollment_no='"+ student_enrollment.Text +"',student_department='"+ student_dept.Text +"',student_sem='"+ student_sem.Text +"',student_contact='"+ student_contact.Text +"',student_email='"+ student_email.Text +"' where id = "+ i +" ";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Updated Successfullly!");
            }
            else if(result == DialogResult.Cancel)
            {
                int i;
                i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update student_info set students_name='" + student_name.Text + "',student_enrollment_no='" + student_enrollment.Text + "',student_department='" + student_dept.Text + "',student_sem='" + student_sem.Text + "',student_contact='" + student_contact.Text + "',student_email='" + student_email.Text + "' where id = " + i + " ";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Updated Successfullly!");
            }
        }
    }
}
