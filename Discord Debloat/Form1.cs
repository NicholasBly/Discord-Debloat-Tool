using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace Discord_Debloat
{
    public partial class Form1 : Form
    {
        string discordFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Discord");
        string modulesFolderPath;
        public Form1()
        {
            InitializeComponent();
        }

        public void CheckAndKillDiscord()
        {
            var discordProcesses = Process.GetProcessesByName("discord");

            if (discordProcesses.Any() && MessageBox.Show("Discord is running. Do you want to kill the process to proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var process in discordProcesses)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to kill Discord process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        void ToggleCheckBox(string moduleName, CheckBox checkBox)
        {
            string moduleFolderPath = Path.Combine(modulesFolderPath, moduleName);

            if (!Directory.Exists(moduleFolderPath))
            {
                checkBox.Checked = false;
                checkBox.Enabled = false;
            }
            else
            {
                checkBox.Checked = true;
                checkBox.Enabled = true;
            }
        }
        void DeleteDirectoryIfChecked(CheckBox checkBox, string folderName)
        {
            if (checkBox.CheckState == CheckState.Checked && Directory.Exists(modulesFolderPath + @"\" + folderName))
            {
                Directory.Delete(modulesFolderPath + @"\" + folderName, true);
            }
        }

        void UpdateLabelColor(System.Windows.Forms.Label label, string folderName)
        {
            if (Directory.Exists(modulesFolderPath + @"\" + folderName))
            {
                label.ForeColor = Color.Green;
            }
            else
            {
                label.ForeColor = Color.Red;
            }
        }
        void CheckBoxState()
        {
            // Iterate through all checkboxes
            bool anyCheckBoxEnabled = false;
            foreach (CheckBox checkBox in new CheckBox[] { checkBox6, checkBox12, checkBox11, checkBox10, checkBox9, checkBox8, checkBox7, checkBox13, checkBox14 })
            {
                if (checkBox.Enabled)
                {
                    anyCheckBoxEnabled = true;
                    break;
                }
            }

            // Disable button1 if all checkboxes are disabled
            button1.Enabled = anyCheckBoxEnabled;
        }

        void refreshAvailableOptions()
        {
            ToggleCheckBox("discord_cloudsync-1", checkBox6);
            ToggleCheckBox("discord_dispatch-1", checkBox12);
            ToggleCheckBox("discord_erlpack-1", checkBox11);
            ToggleCheckBox("discord_game_utils-1", checkBox10);
            ToggleCheckBox("discord_media-1", checkBox9);
            ToggleCheckBox("discord_overlay2-1", checkBox8);
            ToggleCheckBox("discord_rpc-1", checkBox7);
            ToggleCheckBox("discord_spellcheck-1", checkBox13);
            ToggleCheckBox("discord_zstd-1", checkBox14);

            UpdateLabelColor(label1, label1.Text);
            UpdateLabelColor(label2, label2.Text);
            UpdateLabelColor(label3, label3.Text);
            UpdateLabelColor(label4, label4.Text);
            UpdateLabelColor(label5, label5.Text);
            UpdateLabelColor(label6, label6.Text);
            UpdateLabelColor(label7, label7.Text);
            UpdateLabelColor(label8, label8.Text);
            UpdateLabelColor(label9, label9.Text);
            UpdateLabelColor(label10, label10.Text);
            UpdateLabelColor(label11, label11.Text);
            UpdateLabelColor(label12, label12.Text);
            UpdateLabelColor(label13, label13.Text);
            UpdateLabelColor(label14, label14.Text);

            CheckBoxState();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] appDirectories = Directory.GetDirectories(discordFolderPath, "app-*.*.*");
            foreach (string appDirectory in appDirectories)
            {
                modulesFolderPath = Path.Combine(appDirectory, "modules");
            }
            refreshAvailableOptions();
            CheckAndKillDiscord();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteDirectoryIfChecked(checkBox6, "discord_cloudsync-1");
            DeleteDirectoryIfChecked(checkBox12, "discord_dispatch-1");
            DeleteDirectoryIfChecked(checkBox11, "discord_erlpack-1");
            DeleteDirectoryIfChecked(checkBox10, "discord_game_utils-1");
            DeleteDirectoryIfChecked(checkBox9, "discord_media-1");
            DeleteDirectoryIfChecked(checkBox8, "discord_overlay2-1");
            DeleteDirectoryIfChecked(checkBox7, "discord_rpc-1");
            DeleteDirectoryIfChecked(checkBox13, "discord_spellcheck-1");
            DeleteDirectoryIfChecked(checkBox14, "discord_zstd-1");

            refreshAvailableOptions();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The first five directories should not be deleted to retain normal Discord functionality.\n\nRed directories do not exist, and green ones exist.\n\nCheck each box next to the corresponding folder name and then click 'Debloat Discord' to delete each folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Credits to Nicholas Bly on github. Open the github page now?", "Credits", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if ((result == DialogResult.Yes))
            {
                Process.Start("https://github.com/NicholasBly");
            }
        }
    }
}
