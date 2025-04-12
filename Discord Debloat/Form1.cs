using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Discord_Debloat
{
    public partial class Form1 : Form
    {
        // Constants
        private const string DISCORD_PROCESS_NAME = "discord";
        private const string APP_PREFIX = "app-";
        private const string MODULE_SUFFIX1 = "-1";
        private const string MODULE_SUFFIX2 = "-2";

        // Module name constants
        private const string DISCORD_DESKTOP_CORE = "discord_desktop_core";
        private const string DISCORD_KRISP = "discord_krisp";
        private const string DISCORD_MODULES = "discord_modules";
        private const string DISCORD_UTILS = "discord_utils";
        private const string DISCORD_VOICE = "discord_voice";
        private const string DISCORD_CLOUDSYNC = "discord_cloudsync";
        private const string DISCORD_RPC = "discord_rpc";
        private const string DISCORD_DESKTOP_OVERLAY = "discord_desktop_overlay";
        private const string DISCORD_MEDIA = "discord_media";
        private const string DISCORD_GAME_UTILS = "discord_game_utils";
        private const string DISCORD_ERLPACK = "discord_erlpack";
        private const string DISCORD_DISPATCH = "discord_dispatch";
        private const string DISCORD_SPELLCHECK = "discord_spellcheck";
        private const string DISCORD_ZSTD = "discord_zstd";

        // Path variables
        private readonly string discordFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Discord");
        private string modulesFolderPath;

        // Module to checkbox mapping
        private Dictionary<string, CheckBox> moduleCheckboxMap;
        private Dictionary<string, Label> moduleLabelMap;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeMappings();
            FindModulesPath();
            RefreshAvailableOptions();
            CheckAndKillDiscord();
        }

        private void InitializeMappings()
        {
            // Initialize module to checkbox mapping
            moduleCheckboxMap = new Dictionary<string, CheckBox>
            {
                { DISCORD_DESKTOP_CORE, checkBox1 },
                { DISCORD_KRISP, checkBox2 },
                { DISCORD_MODULES, checkBox3 },
                { DISCORD_UTILS, checkBox4 },
                { DISCORD_VOICE, checkBox5 },
                { DISCORD_CLOUDSYNC, checkBox6 },
                { DISCORD_RPC, checkBox7 },
                { DISCORD_DESKTOP_OVERLAY, checkBox8 },
                { DISCORD_MEDIA, checkBox9 },
                { DISCORD_GAME_UTILS, checkBox10 },
                { DISCORD_ERLPACK, checkBox11 },
                { DISCORD_DISPATCH, checkBox12 },
                { DISCORD_SPELLCHECK, checkBox13 },
                { DISCORD_ZSTD, checkBox14 }
            };

            // Initialize module to label mapping
            moduleLabelMap = new Dictionary<string, Label>
            {
                { DISCORD_DESKTOP_CORE, label1 },
                { DISCORD_KRISP, label2 },
                { DISCORD_MODULES, label3 },
                { DISCORD_UTILS, label4 },
                { DISCORD_VOICE, label5 },
                { DISCORD_CLOUDSYNC, label6 },
                { DISCORD_RPC, label7 },
                { DISCORD_DESKTOP_OVERLAY, label11 },
                { DISCORD_MEDIA, label9 },
                { DISCORD_GAME_UTILS, label10 },
                { DISCORD_ERLPACK, label8 },
                { DISCORD_DISPATCH, label12 },
                { DISCORD_SPELLCHECK, label13 },
                { DISCORD_ZSTD, label14 }
            };
        }

        private void FindModulesPath()
        {
            string[] appDirectories = Directory.GetDirectories(discordFolderPath, $"{APP_PREFIX}*.*.*");

            // Take the first app directory (most likely the latest version)
            if (appDirectories.Length > 0)
            {
                modulesFolderPath = Path.Combine(appDirectories[0], "modules");
            }
        }

        public void CheckAndKillDiscord()
        {
            var discordProcesses = Process.GetProcessesByName(DISCORD_PROCESS_NAME);

            if (discordProcesses.Any() &&
                MessageBox.Show("Discord is running. Do you want to kill the process to proceed?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var process in discordProcesses)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to kill Discord process: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshAvailableOptions()
        {
            // Update checkbox states
            foreach (var moduleEntry in moduleCheckboxMap)
            {
                string moduleName = moduleEntry.Key;
                CheckBox checkBox = moduleEntry.Value;

                ToggleCheckBox(moduleName, checkBox);


                //Disable Protected Discord Functions
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
            }

            // Update label colors
            foreach (var labelEntry in moduleLabelMap)
            {
                string moduleName = labelEntry.Key;
                Label label = labelEntry.Value;

                UpdateLabelColor(label, moduleName);
            }

            UpdateDebloatButtonState();
        }

        private void ToggleCheckBox(string baseModuleName, CheckBox checkBox)
        {
            string modulePath1 = Path.Combine(modulesFolderPath, baseModuleName + MODULE_SUFFIX1);
            string modulePath2 = Path.Combine(modulesFolderPath, baseModuleName + MODULE_SUFFIX2);

            bool exists1 = Directory.Exists(modulePath1);
            bool exists2 = Directory.Exists(modulePath2);

            checkBox.Checked = exists1 || exists2;
            checkBox.Enabled = exists1 || exists2;
        }

        private void UpdateLabelColor(Label label, string moduleName)
        {
            string modulePath1 = Path.Combine(modulesFolderPath, moduleName + MODULE_SUFFIX1);
            string modulePath2 = Path.Combine(modulesFolderPath, moduleName + MODULE_SUFFIX2);

            bool exists = Directory.Exists(modulePath1) || Directory.Exists(modulePath2);
            label.ForeColor = exists ? Color.Green : Color.Red;
        }

        private void UpdateDebloatButtonState()
        {
            // Only enable debloat button if at least one removable module is checked
            bool anyRemovableModuleEnabled = false;

            // List of modules that can be safely removed
            var removableModules = new[]
            {
                DISCORD_CLOUDSYNC, DISCORD_DISPATCH, DISCORD_ERLPACK,
                DISCORD_GAME_UTILS, DISCORD_MEDIA, DISCORD_DESKTOP_OVERLAY,
                DISCORD_RPC, DISCORD_SPELLCHECK, DISCORD_ZSTD
            };

            foreach (string module in removableModules)
            {
                if (moduleCheckboxMap[module].Enabled)
                {
                    anyRemovableModuleEnabled = true;
                    break;
                }
            }

            button1.Enabled = anyRemovableModuleEnabled;
        }

        private void DeleteDirectoryIfChecked(string moduleName)
        {
            CheckBox checkBox = moduleCheckboxMap[moduleName];

            if (checkBox.CheckState != CheckState.Checked)
                return;

            string modulePath1 = Path.Combine(modulesFolderPath, moduleName + MODULE_SUFFIX1);
            string modulePath2 = Path.Combine(modulesFolderPath, moduleName + MODULE_SUFFIX2);

            if (Directory.Exists(modulePath1))
                Directory.Delete(modulePath1, true);

            if (Directory.Exists(modulePath2))
                Directory.Delete(modulePath2, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Only allow removal of safe-to-remove modules
            DeleteDirectoryIfChecked(DISCORD_CLOUDSYNC);
            DeleteDirectoryIfChecked(DISCORD_DISPATCH);
            DeleteDirectoryIfChecked(DISCORD_ERLPACK);
            DeleteDirectoryIfChecked(DISCORD_GAME_UTILS);
            DeleteDirectoryIfChecked(DISCORD_MEDIA);
            DeleteDirectoryIfChecked(DISCORD_DESKTOP_OVERLAY);
            DeleteDirectoryIfChecked(DISCORD_RPC);
            DeleteDirectoryIfChecked(DISCORD_SPELLCHECK);
            DeleteDirectoryIfChecked(DISCORD_ZSTD);

            RefreshAvailableOptions();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The first five directories should not be deleted to retain normal Discord functionality.\n\n" +
                "Red directories do not exist, and green ones exist.\n\n" +
                "Check each box next to the corresponding folder name and then click 'Debloat Discord' to delete each folder.",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string GITHUB_URL = "https://github.com/NicholasBly";

            DialogResult result = MessageBox.Show(
                "Credits to Nicholas Bly on GitHub. Open the GitHub page now?",
                "Credits", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Process.Start(GITHUB_URL);
            }
        }
    }
}
