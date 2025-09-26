using System;
using ScriptPortal.Vegas;

namespace YTPMVScript.Helpers
{
    public static class Mixer
    {
        /// <summary>
        /// Set a track's volume in dB. Replace example code with the correct property if your Vegas API exposes a different member (e.g., Track.Volume or Track.MixerStrip.Gain).
        /// </summary>
        public static void SetTrackVolume(Vegas vegas, Track track, double db)
        {
            // TODO: Replace with correct property on Track or MixerStrip depending on Vegas version.
            try
            {
                dynamic dynTrack = track;
                if (HasMember(dynTrack, "Volume"))
                {
                    dynTrack.Volume = db;
                }
                else if (HasMember(dynTrack, "MixerStrip"))
                {
                    if (HasMember(dynTrack.MixerStrip, "Volume"))
                    {
                        dynTrack.MixerStrip.Volume = db;
                    }
                }
                else
                {
                    // Alternative approach: insert a volume envelope or apply track FX to adjust gain
                    // TODO: implement fallback approach
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to set track volume: " + ex.Message, ex);
            }
        }

        private static bool HasMember(dynamic obj, string member)
        {
            try
            {
                return obj.GetType().GetProperty(member) != null;
            }
            catch
            {
                return false;
            }
        }
    }
}