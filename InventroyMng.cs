using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManage
{
    public partial class Form1 : Form
    {
        string strServerName;
        string strDatabase;
        string strUserID;
        string strUserPW;

        MySqlConnection connection;
        Boolean dbConnectionResult = false;

        string PNo, PName, PEa, Pdate;

        public Form1()
        {   
            InitializeComponent();
            DBConnection("127.0.0.1", "factorydb", "root", "0000");
            dbConnectionResult = DBOpen();
            if (dbConnectionResult)
            {
                MessageBox.Show("DB 오픈 성공");
            }
            else
            {
                MessageBox.Show("DB 오픈 실패");
            }
            DBClose();


        }

        public Boolean DBClose()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        private bool DBOpen()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        break;
                    case 1045:
                        break;
                }
                return false;
            }

        }

        private void DBConnection(string v1, string v2, string v3, string v4)
        {
            this.strServerName = v1;
            this.strDatabase = v2;
            this.strUserID = v3;
            this.strUserPW = v4;
            string connectionString;
            connectionString = "SERVER=" + strServerName + ";" + "DATABASE=" + strDatabase + ";" + "UID=" + strUserID + ";" + "PASSWORD=" + strUserPW + ";";
            connection = new MySqlConnection(connectionString);

        }

        public Boolean Sql(string strSql)
        {
            DBOpen();
            try
            {
                MySqlCommand cmd = new MySqlCommand(strSql, connection);
                cmd.ExecuteNonQuery();
                DBClose();
                return true;
            }
            catch (Exception ex)
            {
                DBClose();
                return false;
            }
        }

        public void SqlSelect(string strSqlQuery)
        {
            if (DBOpen() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(strSqlQuery, connection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);
                datGridViewProduct.DataSource = dt;

                DBClose();
            }
            return;
        }

        public void SqlSelect(string strSqlQuery, DataGridView str)
        {
            if (DBOpen() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(strSqlQuery, connection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);
                str.DataSource = dt;

                DBClose();
            }
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            string strSql;
            strSql = "select * from inventorytb";
            SqlSelect(strSql);
        }

        private void btnProductNoSelect_Click(object sender, EventArgs e)
        {
            string strSql;
            strSql = $"select * from inventorytb where ProductNo like '%{txtBoxSearch.Text}%'";
            SqlSelect(strSql);
        }

        private void btnProductNameSelect_Click(object sender, EventArgs e)
        {
            string strSql;
            strSql = $"select * from inventorytb where ProductName like '%{txtBoxSearch.Text}%'";
            SqlSelect(strSql);

        }

        private void btnCompanySelect_Click(object sender, EventArgs e)
        {
            string strSql;
            strSql = $"select * from inventorytb where Company like '%{txtBoxSearch.Text}%'";
            SqlSelect(strSql);
        }

        private void btnProductInsert_Click(object sender, EventArgs e)
        {
            string strSql;
            strSql = $"INSERT INTO inventorytb values ('{txtBoxProductNo.Text}'" +
                                                     $",'{txtBoxProductName.Text}'" +
                                                     $",{txtBoxEa.Text}" +
                                                     $",{txtBoxCost.Text}" +
                                                     $",'{txtBoxCompany.Text}'" +
                                                     $",'{txtBoxEtc.Text}')";
            Sql(strSql);
            strSql = "select * from inventorytb";
            SqlSelect(strSql);
        }

        private void button4_Click(object sender, EventArgs e) //btnInputSelect
        {
            if (txtBoxProductNoSelect.Text.Length == 0)
            {
                MessageBox.Show("제품 번호를 입력해 주세요.");

                return;
            }

            string strSql;
            strSql = $"select * from inventorytb where ProductNo like '%{txtBoxProductNoSelect.Text}%'";
            SqlSelect(strSql, dataGridViewInputInven);

            txtBoxInputProductNo.ReadOnly = false;
        }

        private void dataGridViewInputInven_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
 
        }

        private void datGridViewProduct_Click(object sender, EventArgs e)
        {
            string strSql;
            strSql = "select * from inventorytb";
            SqlSelect(strSql);
        }

        private void btnOutputSelect_Click(object sender, EventArgs e)
        {
            if (txtBoxProductNoSelectOutput.Text.Length == 0)
            {
                MessageBox.Show("제품 번호를 입력해 주세요.");

                return;
            }

            string strSql;
            strSql = $"select * from inventorytb where ProductNo like '%{txtBoxProductNoSelectOutput.Text}%'";
            SqlSelect(strSql, dataGridViewOutputInven);

            txtBoxOutputProductNo.ReadOnly = false;
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            string strSql;

            PNo = txtBoxInputProductNo.Text;
            PEa = txtBoxInputProductEa.Text;
            Pdate = DateTime.Now.ToString("yyyy-MM-dd");
            Console.WriteLine(PNo, PName, PEa, Pdate);
            strSql = $"INSERT INTO inputtb(ProductNo, ProductName, EA, InputDate) values ('{PNo}'"+ $",{PName}" + $",{PEa}" + $",'{Pdate}')";
            Sql(strSql);
            strSql = "select * from inputtb";
            SqlSelect(strSql, dataGridViewInput);
        }

        private void dataGridViewInputInven_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PNo = txtBoxInputProductNo.Text = dataGridViewInputInven.Rows[e.RowIndex].Cells[0].Value.ToString();
            PName = dataGridViewInputInven.Rows[e.RowIndex].Cells[1].Value.ToString();
            PEa = txtBoxInputProductEa.Text = dataGridViewInputInven.Rows[e.RowIndex].Cells[2].Value.ToString();
            Pdate = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

}
