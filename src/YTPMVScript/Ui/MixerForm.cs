using System;
using System.Linq;
using System.Windows.Forms;
using ScriptPortal.Vegas;
using YTPMVScript.Helpers;

namespace YTPMVScript.Ui
{
    public class MixerForm : Form
    {
        private readonly Vegas _vegas;
        private ListBox _lbTracks;
        private Track _selectedTrack;
        private TrackVolumeControl _volumeControl;

        public MixerForm(Vegas vegas)
        {
            _vegas = vegas;
            InitializeComponents();
            LoadTracks();
        }

        private void InitializeComponents()
        {
            this.Text = "Mixer — YTPMVScript";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterParent;

            _lbTracks = new ListBox { Left = 12, Top = 12, Width = 220, Height = 300 };
            _lbTracks.SelectedIndexChanged += LbTracks_SelectedIndexChanged;
            this.Controls.Add(_lbTracks);

            _volumeControl = new TrackVolumeControl { Left = 250, Top = 12 };
            this.Controls.Add(_volumeControl);

            var btnApply = new Button { Left = 250, Top = 260, Width = 200, Text = "Apply Volume to Track" };
            btnApply.Click += (s, e) =>
            {
                if (_selectedTrack == null) return;
                Mixer.SetTrackVolume(_vegas, _selectedTrack, _volumeControl.VolumeDb);
                MessageBox.Show("Applied volume (see TODO for exact API behavior).", "Mixer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            this.Controls.Add(btnApply);
        }

        private void LoadTracks()
        {
            _lbTracks.Items.Clear();
            foreach (Track t in _vegas.Project.Tracks)
            {
                var label = string.IsNullOrEmpty(t.Name) ? "Unnamed track" : t.Name;
                _lbTracks.Items.Add(new { Label = label, Track = t });
            }
            _lbTracks.DisplayMember = "Label";
        }

        private void LbTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = _lbTracks.SelectedItem;
            if (obj == null) return;
            dynamic dyn = obj;
            _selectedTrack = dyn.Track as Track;
            // If you can read current gain/volume, populate volume control:
            // TODO: read actual track volume from API and set _volumeControl.VolumeDb
        }
    }

    // Minimal custom control for volume (placeholder)
    public class TrackVolumeControl : UserControl
    {
        private NumericUpDown _num;
        public TrackVolumeControl()
        {
            this.Width = 200;
            this.Height = 60;
            _num = new NumericUpDown { Left = 0, Top = 0, Width = 120, Minimum = -60, Maximum = 12, DecimalPlaces = 1 };
            this.Controls.Add(_num);
        }

        public double VolumeDb
        {
            get => (double)_num.Value;
            set => _num.Value = (decimal)value;
        }
    }
}