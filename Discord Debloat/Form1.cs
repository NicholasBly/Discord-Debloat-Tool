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
using System.Text.RegularExpressions;
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

        void UpdateLabelColor(System.Windows.Forms.Label label, string folderNamePattern)
        {
            string searchPattern = ReplaceNumbersWithWildcard(folderNamePattern);
            string[] matchingDirectories = Directory.GetDirectories(modulesFolderPath, searchPattern);

            if (matchingDirectories.Length > 0)
            {
                label.ForeColor = Color.Green;
                label.Text = Path.GetFileName(matchingDirectories[0]); // Update label to the first matching directory name
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
            foreach (CheckBox checkBox in new CheckBox[] { checkBox6, checkBox12, checkBox11, checkBox10, checkBox9, checkBox8, checkBox7, checkBox13, checkBox14, checkBox1, checkBox5, checkBox3, checkBox4 })
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
        string ReplaceNumbersWithWildcard(string input)
        {
            return Regex.Replace(input, @"\d", "*");
        }

        void refreshAvailableOptions()
        {
            ToggleCheckBox("discord_cloudsync-*", checkBox6);
            ToggleCheckBox("discord_dispatch-*", checkBox12);
            ToggleCheckBox("discord_erlpack-*", checkBox11);
            ToggleCheckBox("discord_game_utils-*", checkBox10);
            ToggleCheckBox("discord_media-*", checkBox9);
            ToggleCheckBox("discord_overlay2-*", checkBox8);
            ToggleCheckBox("discord_rpc-*", checkBox7);
            ToggleCheckBox("discord_spellcheck-*", checkBox13);
            ToggleCheckBox("discord_zstd-*", checkBox14);

            UpdateLabelColor(label1, ReplaceNumbersWithWildcard(label1.Text));
            UpdateLabelColor(label2, ReplaceNumbersWithWildcard(label2.Text));
            UpdateLabelColor(label3, ReplaceNumbersWithWildcard(label3.Text));
            UpdateLabelColor(label4, ReplaceNumbersWithWildcard(label4.Text));
            UpdateLabelColor(label5, ReplaceNumbersWithWildcard(label5.Text));
            UpdateLabelColor(label6, ReplaceNumbersWithWildcard(label6.Text));
            UpdateLabelColor(label7, ReplaceNumbersWithWildcard(label7.Text));
            UpdateLabelColor(label8, ReplaceNumbersWithWildcard(label8.Text));
            UpdateLabelColor(label9, ReplaceNumbersWithWildcard(label9.Text));
            UpdateLabelColor(label10, ReplaceNumbersWithWildcard(label10.Text));
            UpdateLabelColor(label11, ReplaceNumbersWithWildcard(label11.Text));
            UpdateLabelColor(label12, ReplaceNumbersWithWildcard(label12.Text));
            UpdateLabelColor(label13, ReplaceNumbersWithWildcard(label13.Text));
            UpdateLabelColor(label14, ReplaceNumbersWithWildcard(label14.Text));

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
            DeleteDirectoryIfChecked(checkBox6, "discord_cloudsync-*");
            DeleteDirectoryIfChecked(checkBox12, "discord_dispatch-*");
            DeleteDirectoryIfChecked(checkBox11, "discord_erlpack-*");
            DeleteDirectoryIfChecked(checkBox10, "discord_game_utils-*");
            DeleteDirectoryIfChecked(checkBox9, "discord_media-*");
            DeleteDirectoryIfChecked(checkBox8, "discord_overlay*-*");
            DeleteDirectoryIfChecked(checkBox7, "discord_rpc-*");
            DeleteDirectoryIfChecked(checkBox13, "discord_spellcheck-*");
            DeleteDirectoryIfChecked(checkBox14, "discord_zstd-*");

            refreshAvailableOptions();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The first four directories should not be deleted to retain normal Discord functionality.\n\nRed directories do not exist, and green ones exist.\n\nCheck each box next to the corresponding folder name and then click 'Debloat Discord' to delete each folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Credits to Nicholas Bly on github. Open the github page now?", "Credits", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if ((result == DialogResult.Yes))
            {
                Process.Start("https://github.com/NicholasBly");
            }
        }

        private void applyRecommendedOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox1.CheckState = CheckState.Unchecked;
            checkBox5.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Unchecked;
            checkBox4.CheckState = CheckState.Unchecked;

            checkBox1.Enabled = false;
            checkBox5.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;

            checkBox14.CheckState = CheckState.Checked;
            checkBox13.CheckState = CheckState.Checked;
            checkBox7.CheckState = CheckState.Checked;
            checkBox8.CheckState = CheckState.Checked;
            checkBox9.CheckState = CheckState.Checked;
            checkBox10.CheckState = CheckState.Checked;
            checkBox11.CheckState = CheckState.Checked;
            checkBox12.CheckState = CheckState.Checked;
            checkBox6.CheckState = CheckState.Checked;

            CheckBoxState();
        }

        private void enableNecessaryFoldersNotRecommendedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            checkBox5.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;

            CheckBoxState();
        }

        private void openFolderDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = modulesFolderPath,
                    FileName = "explorer.exe"
                };

            Process.Start(startInfo);
        }
    }
}
