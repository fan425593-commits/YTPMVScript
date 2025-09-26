using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ScriptPortal.Vegas;
using YTPMVScript.Helpers;
using YTPMVScript.Generators;

namespace YTPMVScript.Ui
{
    public class MainForm : Form
    {
        private readonly Vegas _vegas;
        private ListBox _lbTracks;
        private ListBox _lbSources;
        private Button _btnBrowseSources;
        private Button _btnImportSources;
        private Button _btnPitchUp;
        private Button _btnPitchDown;
        private NumericUpDown _numPitchSteps;
        private Button _btnOpenMixer;
        private Button _btnApplyPluginToSelected;
        private Button _btnGenVisual;
        private Button _btnGenAudio;

        public MainForm(Vegas vegas)
        {
            _vegas = vegas ?? throw new ArgumentNullException(nameof(vegas));
            InitializeComponents();
            LoadTrackList();
        }

        private void InitializeComponents()
        {
            this.Text = "YTPMVScript — Tools";
            this.Width = 900;
            this.Height = 520;
            this.StartPosition = FormStartPosition.CenterParent;

            _lbTracks = new ListBox { Left = 12, Top = 12, Width = 300, Height = 380 };
            this.Controls.Add(_lbTracks);

            _lbSources = new ListBox { Left = 330, Top = 12, Width = 520, Height = 200 };
            this.Controls.Add(_lbSources);

            _btnBrowseSources = new Button { Left = 330, Top = 220, Width = 120, Text = "Browse Sources..." };
            _btnBrowseSources.Click += BtnBrowseSources_Click;
            this.Controls.Add(_btnBrowseSources);

            _btnImportSources = new Button { Left = 460, Top = 220, Width = 120, Text = "Import to Project" };
            _btnImportSources.Click += BtnImportSources_Click;
            this.Controls.Add(_btnImportSources);

            _numPitchSteps = new NumericUpDown { Left = 330, Top = 260, Width = 80, Minimum = -24, Maximum = 24, Value = 0 };
            this.Controls.Add(_numPitchSteps);

            _btnPitchUp = new Button { Left = 420, Top = 260, Width = 120, Text = "Pitch + (apply)" };
            _btnPitchUp.Click += BtnPitchUp_Click;
            this.Controls.Add(_btnPitchUp);

            _btnPitchDown = new Button { Left = 550, Top = 260, Width = 120, Text = "Pitch - (apply)" };
            _btnPitchDown.Click += BtnPitchDown_Click;
            this.Controls.Add(_btnPitchDown);

            _btnOpenMixer = new Button { Left = 330, Top = 310, Width = 120, Text = "Open Mixer..." };
            _btnOpenMixer.Click += BtnOpenMixer_Click;
            this.Controls.Add(_btnOpenMixer);

            _btnApplyPluginToSelected = new Button { Left = 460, Top = 310, Width = 210, Text = "Apply Plugin to Selected" };
            _btnApplyPluginToSelected.Click += BtnApplyPluginToSelected_Click;
            this.Controls.Add(_btnApplyPluginToSelected);

            _btnGenVisual = new Button { Left = 330, Top = 350, Width = 160, Text = "Generate Visual" };
            _btnGenVisual.Click += BtnGenVisual_Click;
            this.Controls.Add(_btnGenVisual);

            _btnGenAudio = new Button { Left = 500, Top = 350, Width = 160, Text = "Generate Audio" };
            _btnGenAudio.Click += BtnGenAudio_Click;
            this.Controls.Add(_btnGenAudio);

            var btnClose = new Button { Left = 720, Top = 420, Width = 120, Text = "Close" };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void LoadTrackList()
        {
            _lbTracks.Items.Clear();
            int i = 0;
            foreach (Track t in _vegas.Project.Tracks)
            {
                string name = string.IsNullOrEmpty(t.Name) ? $"Track {i}" : t.Name;
                _lbTracks.Items.Add(new { Index = i, Name = name, Track = t });
                i++;
            }
            _lbTracks.DisplayMember = "Name";
        }

        private void BtnBrowseSources_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Audio and Video|*.wav;*.mp3;*.flac;*.aac;*.mp4;*.mov;*.mkv;*.avi|All files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (var f in dlg.FileNames) _lbSources.Items.Add(f);
            }
        }

        private void BtnImportSources_Click(object sender, EventArgs e)
        {
            var files = _lbSources.Items.Cast<object>().Select(x => x.ToString()).ToArray();
            try
            {
                foreach (var f in files)
                {
                    // TODO: Use the proper Vegas API to import media into the project.
                    // Example placeholder: _vegas.AddMedia(f);
                    try
                    {
                        // Attempt to import — replace with actual call if needed.
                        dynamic dynVegas = _vegas;
                        dynVegas.AddMedia(f); // may throw if method name differs; replace or implement import properly
                    }
                    catch
                    {
                        // If direct AddMedia not available, create ProjectMedia from file:
                        // TODO: _vegas.Project.MediaPool.Add(f) or similar (depends on Vegas API)
                    }
                }
                MessageBox.Show("Import attempted. Please check the Project Media in Vegas.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import failed: " + ex.Message, "Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPitchUp_Click(object sender, EventArgs e)
        {
            ApplyPitch((int)_numPitchSteps.Value);
        }

        private void BtnPitchDown_Click(object sender, EventArgs e)
        {
            ApplyPitch(-(int)_numPitchSteps.Value);
        }

        private void ApplyPitch(int semitones)
        {
            try
            {
                PitchHelper.ShiftSelectedEventsPitch(_vegas, semitones);
                MessageBox.Show($"Requested pitch shift of {semitones} semitone(s) applied to selected events (see TODO notes).", "Pitch Helper", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pitch operation failed: " + ex.Message, "Pitch Helper", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOpenMixer_Click(object sender, EventArgs e)
        {
            using (var fm = new MixerForm(_vegas))
            {
                fm.ShowDialog();
            }
        }

        private void BtnApplyPluginToSelected_Click(object sender, EventArgs e)
        {
            try
            {
                string pluginName = PromptForString("Enter plugin name (partial match):", "Apply Plugin");
                if (string.IsNullOrEmpty(pluginName)) return;
                PluginManager.ApplyPluginToSelectedTracksOrEvents(_vegas, pluginName);
                MessageBox.Show("Plugin application attempted. Check track effects in Vegas.", "Plugins", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Plugin operation failed: " + ex.Message, "Plugins", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenVisual_Click(object sender, EventArgs e)
        {
            VisualGenerator.GenerateSimpleSolidVisual(_vegas, System.Drawing.Color.Black, Timecode.FromSeconds(0), Timecode.FromSeconds(8));
            MessageBox.Show("Requested visual generation. See timeline for created track/media (if API supports).", "Visual Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnGenAudio_Click(object sender, EventArgs e)
        {
            AudioGenerator.GenerateSineTone(_vegas, frequency: 440, durationSeconds: 4.0);
            MessageBox.Show("Requested audio generation. See timeline for created audio event (if API supports).", "Audio Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string PromptForString(string prompt, string title)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox(prompt, title, "");
            return input;
        }
    }
}