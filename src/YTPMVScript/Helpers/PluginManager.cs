using System;
using System.Linq;
using ScriptPortal.Vegas;

namespace YTPMVScript.Helpers
{
    public static class PluginManager
    {
        /// <summary>
        /// Apply a plugin (by partial name) to selected tracks or events.
        /// TODO: use exact Vegas API to find Audio/VideoFX and add them to track/event FX chains.
        /// Placeholder demonstrates intent and error handling.
        /// </summary>
        public static void ApplyPluginToSelectedTracksOrEvents(Vegas vegas, string pluginPartialName)
        {
            // Example flow (replace with proper API calls):
            // - Search available FX types in vegas.AudioFx/VideoFx
            // - Find matching plugin(s)
            // - For each selected track/event, add effect instance and set default params

            // PSEUDO:
            // var availableFx = vegas.AudioFx; // or appropriate collection
            // var match = availableFx.FirstOrDefault(f => f.Name.Contains(pluginPartialName));
            // foreach selected track/event: track.EffectChain.Add(match.CreateInstance());

            // TODO: implement above using your Vegas SDK. Below is a protective skeleton.

            // Throw if plugin name is empty
            if (string.IsNullOrEmpty(pluginPartialName)) throw new ArgumentException("pluginPartialName is empty");

            // Iterate tracks and selected events
            foreach (Track track in vegas.Project.Tracks)
            {
                if (track.Selected)
                {
                    // TODO: add FX to track
                }

                foreach (TrackEvent ev in track.Events)
                {
                    if (ev.Selected)
                    {
                        // TODO: add FX to event or its take
                    }
                }
            }
        }
    }
}