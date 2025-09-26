using System;
using ScriptPortal.Vegas;

namespace YTPMVScript.Helpers
{
    public static class PitchHelper
    {
        /// <summary>
        /// Shift pitch of selected audio events by semitones.
        /// TODO: Implement the exact pitch-shift logic using your Vegas version's API.
        /// Typical approaches:
        ///  - Add an audio FX/plug-in that does pitch shift and set its parameters.
        ///  - Use any available Take.PitchShift or AudioEvent properties if present.
        /// Note: API names differ between versions; replace placeholders below with actual calls.
        /// </summary>
        public static void ShiftSelectedEventsPitch(Vegas vegas, int semitones)
        {
            // Iterate over selected events and apply pitch changes.
            foreach (Track track in vegas.Project.Tracks)
            {
                foreach (TrackEvent ev in track.Events)
                {
                    if (!ev.Selected) continue;
                    // If the event has audio takes, adjust pitch on the take
                    try
                    {
                        var take = ev.ActiveTake;
                        if (take == null) continue;

                        // Example placeholder: if there is a Pitch property, modify it.
                        // Replace with the actual API you have available.
                        dynamic dynTake = take;
                        if (HasMember(dynTake, "Pitch"))
                        {
                            dynTake.Pitch += semitones;
                        }
                        else if (HasMember(dynTake, "SetPitchShift"))
                        {
                            // hypothetical method
                            dynTake.SetPitchShift(semitones);
                        }
                        else
                        {
                            // Alternative: add a pitch shift plugin effect to the take or event and set parameters
                            // TODO: create and configure pitch shift plugin instance
                        }
                    }
                    catch
                    {
                        // swallow and continue; leave explicit errors to the caller if you prefer
                    }
                }
            }

            // Note: You may want to refresh UI or undo checkpoint here using Vegas API:
            // vegas.UndoRedo.BeginBlock(); ... vegas.UndoRedo.EndBlock("Pitch shift");
        }

        private static bool HasMember(dynamic obj, string member)
        {
            try
            {
                var _ = obj.GetType().GetProperty(member);
                return _ != null;
            }
            catch
            {
                return false;
            }
        }
    }
}