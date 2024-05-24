using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadanie4_lab3
{
    public partial class Form1 : Form
    {
        private RSACryptoServiceProvider rsaProvider;

        public Form1()
        {
            InitializeComponent();
            rsaProvider = new RSACryptoServiceProvider();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                try
                {
                    byte[] fileBytes = File.ReadAllBytes(txtFilePath.Text);
                    byte[] encryptedBytes = rsaProvider.Encrypt(fileBytes, true);

                    string encryptedFilePath = Path.Combine(Path.GetDirectoryName(txtFilePath.Text),
                        Path.GetFileNameWithoutExtension(txtFilePath.Text) + "_encrypted" +
                        Path.GetExtension(txtFilePath.Text));

                    using (FileStream fs = new FileStream(encryptedFilePath, FileMode.Create))
                    {
                        fs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }

                    MessageBox.Show("Szyfrowanie przebiegło pomyślnie");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Encryption failed: " + ex.Message, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Wybierz plik do zaszyfriwania.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                try
                {
                    byte[] encryptedBytes = File.ReadAllBytes(txtFilePath.Text);
                    // Deszyfrowanie
                    byte[] decryptedBytes = rsaProvider.Decrypt(encryptedBytes, true);

                    string decryptedFilePath = Path.Combine(Path.GetDirectoryName(txtFilePath.Text),
                        Path.GetFileNameWithoutExtension(txtFilePath.Text) + "_decrypted" +
                        Path.GetExtension(txtFilePath.Text));

                    using (FileStream fs = new FileStream(decryptedFilePath, FileMode.Create))
                    {
                        fs.Write(decryptedBytes, 0, decryptedBytes.Length);
                    }

                    MessageBox.Show("Dzeszyfrowanie przebiegło pomyślnie");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd Deszyfriowania" + ex.Message, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Wybierz plik do dezaszyfriwania.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}