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

namespace Library_MS
{
    public partial class issue_books : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-AHCNDUA9\MSSQLSERVER02;Initial Catalog=library_management_system;Integrated Security=True;Pooling=False");
        public issue_books()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from student_info where student_enrollment_no = '" + txt_enrollment.Text + "'";
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            i = Convert.ToInt32(dt.Rows.Count.ToString());
            if (i == 0)
            {
                MessageBox.Show("This is enrollment not found!");
            }
            else
            {
                
                foreach (DataRow dr in dt.Rows)
                {
                    txt_studentName.Text = dr["students_name"].ToString();
                    txt_dept.Text = dr["student_department"].ToString();
                    txt_sem.Text = dr["student_sem"].ToString();
                    txt_contact.Text = dr["student_contact"].ToString();
                    txt_email.Text = dr["student_email"].ToString();
                }
            }
        }

        private void issue_books_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
        }

        private void txt_books_KeyUp(object sender, KeyEventArgs e)
        {
            int count = 0;

            if(e.KeyCode != Keys.Enter)
            {
                listBox1.Items.Clear();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from books_info where books_name like ('%" + txt_books.Text +"%')";
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                count = Convert.ToInt32(dt.Rows.Count.ToString());

                if (count > 0)
                {
                    listBox1.Visible = true;
                    foreach (DataRow dr in dt.Rows)
                    {
                        listBox1.Items.Add(dr["books_name"].ToString());
                    }
                }
            }
            
        }

        private void txt_books_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_books_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                listBox1.Show();
                listBox1.SelectedIndex = 0;
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                txt_books.Text = listBox1.SelectedItem.ToString();
                listBox1.Visible = false;
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            txt_books.Text = listBox1.SelectedItem.ToString();
            listBox1.Visible = false; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int books_qty = 0;

            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select * from books_info where books_name = '" + txt_books.Text + "'";
            cmd2.ExecuteNonQuery();

            if(books_qty > 0)
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into issue_books values( '" + txt_enrollment.Text + "' , '" + txt_studentName.Text + "' , '" + txt_dept.Text + "' , '" + txt_sem.Text + "' , '" + txt_contact.Text + "' , '" + txt_email.Text + "' , '" + txt_books.Text + "' , '" + dateTimePicker1.Value.ToShortDateString() + "', '' )";
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = con.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "update books_info set available_qty = available_qty-1 where books_name = '" + txt_books.Text + "'";
                cmd1.ExecuteNonQuery();

                MessageBox.Show("Books Issued Successfully!");
            }
            
            else
            {
                MessageBox.Show("Books not available");
            }
        }
    }
}
