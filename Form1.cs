using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excel = Microsoft.Office.Interop.Excel;

namespace Cari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                {

                    double cell1 = Convert.ToSingle(dataGridView4.CurrentRow.Cells[2].Value);

                    double cell2 = Convert.ToSingle(dataGridView4.CurrentRow.Cells[3].Value);

                    if (cell1.ToString() != "" && cell2.ToString() != "")
                    {
                        dataGridView4.CurrentRow.Cells[4].Value = cell1 * cell2;

                    }

                }
                label1.Visible = true;
                decimal tot = 0;
                for (int i = 0; i <= dataGridView4.RowCount - 1; i++)
                {
                    tot += Convert.ToDecimal(dataGridView4.Rows[i].Cells[4].Value);
                }
                if (tot == 0)
                {
                    //MessageBox.Show("Kayıt Bulunamadı.");
                }
                txt_netamount.Text = tot.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True");
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from Musteri ", con);
            da.Fill(dt);
            comboBox1.ValueMember = "musteri_id";
            comboBox1.DisplayMember = "musteri_unvan";
            comboBox1.DataSource = dt;
        }

        private void button3x_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView4.Rows.Count > 1)
                {

                    for (int i = 0; i <= dataGridView4.Rows.Count - 2; i++)
                    {

                        //string col1 = dataGridView4.Rows[i].Cells[0].Value.ToString();
                        string col1 = dataGridView4.Rows[i].Cells[1].Value.ToString();
                        string col2 = dataGridView4.Rows[i].Cells[2].Value.ToString();
                        string col3 = dataGridView4.Rows[i].Cells[3].Value.ToString();
                        //string col4 = dataGridView4.Rows[i].Cells[4].Value.ToString();
                        
                        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True");
                        SqlCommand cmd = new SqlCommand();
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_InsertSevk"; //Stored Procedure' ümüzün ismi
                        cmd.Parameters.AddWithValue("@musteri_id", comboBox1.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@urun_adi", col1.ToString());
                        cmd.Parameters.AddWithValue("@urun_miktar", col2.ToString());
                        cmd.Parameters.AddWithValue("@urun_birim", "Adet");
                        cmd.Parameters.AddWithValue("@urun_birimFiyat", col3.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();
                        

                    }

                }

            }

            catch (Exception ex)
            {

                ex.Message.ToString();

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 1)
            {

                for (int i = 0; i <= dataGridView4.Rows.Count - 2; i++)
                {
                    dataGridView4.Rows[i].Cells[1].Value = "";
                    dataGridView4.Rows[i].Cells[2].Value = "";
                    dataGridView4.Rows[i].Cells[3].Value = "";
                    dataGridView4.Rows[i].Cells[4].Value = "";
                    comboBox1.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertMusteri"; //Stored Procedure' ümüzün ismi
            cmd.Parameters.AddWithValue("@musteri_unvan", textBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@musteri_adres", textBox2.Text.ToString());
            cmd.Parameters.AddWithValue("@musteri_not", textBox11.Text.ToString());

            cmd.ExecuteNonQuery();
            con.Close();

            var select1 = "SELECT * FROM Musteri";
            var c1 = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True"); // Your Connection String here
            var dataAdapter1 = new SqlDataAdapter(select1, c1);

            var commandBuilder1 = new SqlCommandBuilder(dataAdapter1);
            var ds1 = new DataSet();
            dataAdapter1.Fill(ds1);
            dataGridView1.ReadOnly = false;
            dataGridView1.DataSource = ds1.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.Enabled = true;
            textBox3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox11.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView4.SelectedRows)
            {
                dataGridView4.Rows.RemoveAt(row.Index);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'cARI_DBDataSet2.Musteri' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.musteriTableAdapter1.Fill(this.cARI_DBDataSet2.Musteri);
            // TODO: Bu kod satırı 'cARI_DBDataSet.Musteri' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.musteriTableAdapter.Fill(this.cARI_DBDataSet.Musteri);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateMusteriKayit"; //Stored Procedure' ümüzün ismi
            cmd.Parameters.AddWithValue("@musteri_id", textBox3.Text.ToString());
            cmd.Parameters.AddWithValue("@musteri_unvan", textBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@musteri_adres", textBox2.Text.ToString());
            cmd.Parameters.AddWithValue("@musteri_not", textBox11.Text.ToString());
            cmd.ExecuteNonQuery();
            con.Close();

            var select1 = "SELECT * FROM Musteri";
            var c1 = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True"); // Your Connection String here
            var dataAdapter1 = new SqlDataAdapter(select1, c1);

            var commandBuilder1 = new SqlCommandBuilder(dataAdapter1);
            var ds1 = new DataSet();
            dataAdapter1.Fill(ds1);
            dataGridView1.ReadOnly = false;
            dataGridView1.DataSource = ds1.Tables[0];
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            SqlConnection conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SP_Satislar ", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@musteri_id", comboBox2.SelectedValue.ToString());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;

            int sum = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; ++i)
            {
                sum += Convert.ToInt32(dataGridView2.Rows[i].Cells[7].Value);
            }
            textBox4.Text = sum.ToString();
        }

        private void tabControl1_Enter(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=CARI_DB;Integrated Security=True");
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("\tSELECT M.musteri_id,M.musteri_unvan,M.musteri_adres,M.musteri_not,S.urun_adi,S.urun_miktar,S.urun_birim,\r\n\tS.urun_birimFiyat,S.urun_satir_toplam FROM Musteri M\r\n\tINNER JOIN SEVK S ON M.musteri_id=S.musteri_id\r\n\tWHERE  urun_satir_toplam is not null", con);
            da.Fill(dt1);
            comboBox2.ValueMember = "musteri_id";
            comboBox2.DisplayMember = "musteri_unvan";
            comboBox2.DataSource = dt1;
        }
        private void copyAlltoClipboard()
        {
            dataGridView2.SelectAll();
            DataObject dataObj = dataGridView2.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }
    }
}
