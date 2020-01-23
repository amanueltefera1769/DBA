using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;

namespace PhoneDiary
{
    public partial class MainForm : MaterialSkin.Controls.MaterialForm
    {
        public MainForm()
        {
            InitializeComponent();
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.DARK;
            skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey300,Primary.LightBlue500,Primary.LightBlue400,Accent.LightGreen400,TextShade.WHITE);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Edit(false);
            phoneBooksBindingSource.ResetBindings(false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appData.PhoneBooks' table. You can move, or remove it, as needed.
            this.phoneBooksTableAdapter.Fill(this.appData.PhoneBooks);
            Edit(false);

        }

        private void Edit(bool value)
        {
            txtPhoneNumber.Enabled = value;
            txtFullName.Enabled = value;
            txtEmail.Enabled = value;
            txtAddress.Enabled = value;

        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                Edit(true);
                appData.PhoneBooks.AddPhoneBooksRow(appData.PhoneBooks.NewPhoneBooksRow());
                phoneBooksBindingSource.MoveLast();
                txtPhoneNumber.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Message", MessageBoxButtons.OK,MessageBoxIcon.Error);
                appData.PhoneBooks.RejectChanges();
              
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Edit(true);
            txtPhoneNumber.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Edit(false);
                phoneBooksBindingSource.EndEdit();
                phoneBooksTableAdapter.Update(appData.PhoneBooks);
                txtPhoneNumber.Focus();
                dataGridView.Refresh();
                MessageBox.Show("Your data has been succefully saved.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appData.PhoneBooks.RejectChanges();

            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                    phoneBooksBindingSource.Filter = string.Format("PhoneNumber = '{0}' OR FullName LIKE '*{1}*' OR Email = '{2}' OR Address LIKE '*{3}*'", txtPhoneNumber.Text, txtFullName.Text, txtEmail.Text, txtAddress.Text);
                else
                    phoneBooksBindingSource.Filter = string.Empty;
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode ==Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this record.", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    phoneBooksBindingSource.RemoveCurrent();
            }
        }
    }
}
