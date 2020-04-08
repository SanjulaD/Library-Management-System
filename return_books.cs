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
    public partial class return_books : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-AHCNDUA9\MSSQLSERVER02;Initial Catalog=library_management_system;Integrated Security=True;Pooling=False");
        public return_books()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            fill_grid(textbox1.Text);
        }

        private void return_books_Load(object sender, EventArgs e)
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
        }

        public void fill_grid(string enrollment)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from issue_books where student_enrollment = '" + enrollment.ToString() + "' and books_return_date = '' ";
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
       }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panel2.Visible = true;
            int i = 0;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from issue_books where id = " + i + " ";
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            
            foreach(DataRow dr in dt.Rows)
            {
                lbl_booksName.Text = dr["books_name"].ToString();
                lbl_issueDate.Text = Convert.ToString(dr["books_issue_date"].ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update issue_books set books_return_date = '" + dateTimePicker1.Value.ToString() + "' where id = "+ i +" ";
            cmd.ExecuteNonQuery();

            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "update books_info set available_qty = available_qty+1 where books_name = '" + lbl_booksName.Text + "' ";
            cmd1.ExecuteNonQuery();

            MessageBox.Show("Books Return Successfully!");

            panel2.Visible = true;

            fill_grid(textbox1.Text);
        }
    }
}
