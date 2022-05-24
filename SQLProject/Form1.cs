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

namespace SQLProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = currentTable;
            LoadTable(currentTable);
        }

        SqlConnection conn = new SqlConnection("Data Source = COMPUTER; Initial Catalog = LibraryDB; User Id = admin; Password = admin12");
        SqlCommand cmd;
        SqlDataReader read;

        string id;
        bool mode = true;
        bool editPressed = false;
        string sql;
        string currentTable = "Books";

        public void LoadTable(string name)
        {
            try
            {
                sql = "select * from " + name;
                cmd = new SqlCommand(sql, conn);
                conn.Open();

                read = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }

                read.Close();
                conn.Close();

            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void getValuesByID(string ID)
        {
            sql = "select * from Books where ID = '" + ID + "'";
            cmd = new SqlCommand(sql, conn);
            conn.Open();
            read = cmd.ExecuteReader();
            read.Read();
            while (read.HasRows)
            {
                txtName.Text = read[1].ToString(); //name
                txtEmail.Text = read[2].ToString(); //author
                txtBookName.Text = read[3].ToString(); //stock
                break;
            }

            conn.Close();
        }

        public void clearFields()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtBookName.Clear();
            txtName.Focus();
        }

        private void Button2_Click(object sender, EventArgs e) //clear button
        {

            clearFields();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //save button
        {
            if (txtName.Text != "" && txtEmail.Text != "" && txtBookName.Text != "")
            {
                string name = txtName.Text;
                string email = txtEmail.Text;
                string bookName = txtBookName.Text;
                string author = "";
                int BookStock = -1;

                if (mode)
                {

                    sql = "select Stock from Books where Name = '" + bookName + "'";
                    conn.Open();
                    cmd = new SqlCommand(sql, conn);
                    read = cmd.ExecuteReader();
                    read.Read();

                    while (read.HasRows)
                    {
                        BookStock = Int32.Parse(read["Stock"].ToString());
                        break;
                    }

                    read.Close();

                    if (BookStock > 0)
                    {
                        sql = "insert into Clients(Name, Email, BookRented) values(@Name, @Email, @BookRented)";
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@BookRented", bookName);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Book rented!");

                        sql = "select Author from Books where Name = '" + bookName + "'";
                        cmd = new SqlCommand(sql, conn);
                        read = cmd.ExecuteReader();
                        read.Read();

                        while (read.HasRows)
                        {
                            author = read["Author"].ToString();
                            break;
                        }
                        read.Close();

                        sql = "insert into RentedBooks(Name, Author, Renter) values(@Name, @Author, @Renter)";

                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@Name", bookName);
                        cmd.Parameters.AddWithValue("@Author", author);
                        cmd.Parameters.AddWithValue("@Renter", name);
                        cmd.ExecuteNonQuery();

                        BookStock--;
                        sql = "update Books set Stock = " + BookStock + " where Name = '" + bookName + "'";
                        cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        clearFields();
                    }
                    else
                    {
                        MessageBox.Show("Book not in stock, sorry!");
                    }

                }
                else
                {

                    id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    sql = "update Books set Name = @Name, Author = @Author, Stock = @Stock where id = @id";
                    conn.Open();
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Author", email);
                    cmd.Parameters.AddWithValue("@Stock", bookName);
                    cmd.Parameters.AddWithValue("@id", id);

                    groupBox1.Text = "Rent a book";
                    NameLabel1.Text = "Name";
                    EmailLabel2.Text = "Email Adress";
                    BookLabel3.Text = "Book Name";

                    MessageBox.Show("Record updated!");
                    cmd.ExecuteNonQuery();

                    clearFields();
                    SaveButton.Text = "Save";
                    mode = true;

                }

                conn.Close();
            }
            else
            {
                MessageBox.Show("No empty fields!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                if (currentTable == "Books")
                {
                    groupBox1.Text = "Edit Book";
                    NameLabel1.Text = "Book Name";
                    EmailLabel2.Text = "Author";
                    BookLabel3.Text = "Stock";

                    mode = false;
                    id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    getValuesByID(id);
                    SaveButton.Text = "Edit";

                    if (editPressed == false)
                    {
                        editPressed = true;
                    }
                }
                else
                {
                    MessageBox.Show("You can only edit books!");
                }

            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {

                mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from " + currentTable + " where id = '" + id + "'";

                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record deleted!");
                conn.Close();

                if (editPressed)
                {
                    mode = true;

                    groupBox1.Text = "Rent a book";
                    NameLabel1.Text = "Name";
                    EmailLabel2.Text = "Email Adress";
                    BookLabel3.Text = "Book Name";
                    SaveButton.Text = "Save";
                    editPressed = false;

                    clearFields();
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e) //refresh button
        {
            LoadTable(currentTable);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e) //clients button
        {
            if (TableButton1.Text == "Books")
            {
                dataGridView1.Columns[1].HeaderText = "Book Name";
                dataGridView1.Columns[2].HeaderText = "Author";
                dataGridView1.Columns[3].HeaderText = "Stock";
            }
            else
            {
                dataGridView1.Columns[1].HeaderText = "Name";
                dataGridView1.Columns[2].HeaderText = "Email";
                dataGridView1.Columns[3].HeaderText = "Book rented";
            }
            string aux = currentTable;
            currentTable = TableButton1.Text.Trim();
            TableButton1.Text = aux;
            LoadTable(currentTable);
            this.Text = currentTable;
        }

        private void TableButton2_Click(object sender, EventArgs e) // rented books button
        {
            if (TableButton2.Text == "Books")
            {
                dataGridView1.Columns[1].HeaderText = "Book Name";
                dataGridView1.Columns[2].HeaderText = "Author";
                dataGridView1.Columns[3].HeaderText = "Stock";
            }
            else
            {
                dataGridView1.Columns[1].HeaderText = "Book Name";
                dataGridView1.Columns[2].HeaderText = "Author";
                dataGridView1.Columns[3].HeaderText = "Renter";
            }

            string aux = currentTable;
            currentTable = TableButton2.Text.Replace(" ", string.Empty);
            TableButton2.Text = aux;
            LoadTable(currentTable);
            this.Text = currentTable;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}