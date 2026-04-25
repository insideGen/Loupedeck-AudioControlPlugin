namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using WindowsInterop.CoreAudio;

    internal static class AudioControl
    {
        public static MMAudio MMAudio { get; } = CreateMMAudio();

        private static MMAudio CreateMMAudio()
        {
            var mmAudio = new MMAudio();
            mmAudio.SessionLoadError += (sender, ex) => PluginLog.Error(ex, $"[AudioSessionCollection] Failed to load audio session: {ex.Message}");
            return mmAudio;
        }

        public enum EndpointType
        {
            Device,
            Session
        }

        public static bool TryGetEndpointType(string endpointId, out EndpointType? type)
        {
            if (!string.IsNullOrEmpty(endpointId))
            {
                type = endpointId.Contains('|') ? EndpointType.Session : EndpointType.Device;
                return true;
            }
            type = null;
            return false;
        }

        public static bool TryGetAudioControl(string endpointId, out IAudioControl audioControl)
        {
            if (TryGetEndpointType(endpointId, out EndpointType? type))
            {
                if (type == EndpointType.Device)
                {
                    if (AudioControl.MMAudio.Devices.FirstOrDefault(x => x.Id == endpointId) is IAudioControlDevice device)
                    {
                        audioControl = device;
                        return true;
                    }
                }
                else if (type == EndpointType.Session)
                {
                    IEnumerable<IAudioControlSession> sessions = AudioControl.MMAudio.RenderSessions.Where(x => x.IsSystemSoundsSession == true || x.State != AudioSessionState.Expired).OrderByDescending(x => x.State);
                    AudioSessionInstanceIdentifier asii = AudioSessionInstanceIdentifier.FromString(endpointId);
                    if (asii.ExeId == null && !asii.ExeId.Contains(Guid.Empty.ToString()) && asii.ProcessId == -1)
                    {
                        if (sessions.FirstOrDefault(x => x.DeviceId == asii.DeviceId && x.ExeId == asii.ExeId) is IAudioControlSession session1)
                        {
                            audioControl = session1;
                            return true;
                        }
                    }
                    if(!asii.ExeId.Contains(Guid.Empty.ToString()))
                    {
                        if (sessions.FirstOrDefault(x => x.ExeId == asii.ExeId) is IAudioControlSession session1)
                        {
                            audioControl = session1;
                            return true;
                        }
                    }
                    if (asii.ExePath != null)
                    {
                        if (sessions.FirstOrDefault(x => x.ExePath == asii.ExePath && x.ProcessId == asii.ProcessId) is IAudioControlSession session2)
                        {
                            audioControl = session2;
                            return true;
                        }
                        if (sessions.FirstOrDefault(x => x.ExePath == asii.ExePath) is IAudioControlSession session3)
                        {
                            audioControl = session3;
                            return true;
                        }
                        if(sessions.FirstOrDefault(x => x.ProcessId == asii.ProcessId) is IAudioControlSession session4)
                        {
                            audioControl = session4;
                            return true;
                        }
                    }
                }
            }
            audioControl = null;
            return false;
        }

        public static AudioImageData CreateAudioData(IAudioControl audioControl, bool highlighted = false)
        {
            AudioImageData audioData = new AudioImageData();

            audioData.DisplayName = audioControl.DisplayName;
            audioData.UnmutedIconPath = audioControl.IconPath;

            audioData.Highlighted = highlighted;

            audioData.Muted = audioControl.Muted;
            audioData.VolumeScalar = audioControl.VolumeScalar;

            if (audioControl is IAudioControlDevice device)
            {
                audioData.Id = device.Id;
                if (audioControl is AudioControl)
                {
                    audioData.IsActive = false;
                }
                else
                {
                    audioData.IsActive = device.State == DeviceState.Active;
                }
                if (PluginSettings.IsWindowsIconStyle)
                {
                    if (device.DataFlow == DataFlow.Capture)
                    {
                        audioData.UnmutedIconPath = "@" + CaptureDevice.GetUnmutedIconPath(audioControl.VolumeScalar) + "," + Color.LimeGreen.Name;
                        audioData.MutedIconPath = "@" + CaptureDevice.GetMutedIconPath() + "," + Color.Red.Name;
                    }
                    else if (device.DataFlow == DataFlow.Render)
                    {
                        audioData.UnmutedIconPath = "@" + RenderDevice.GetUnmutedIconPath(audioControl.VolumeScalar) + "," + Color.LimeGreen.Name;
                        audioData.MutedIconPath = "@" + RenderDevice.GetMutedIconPath() + "," + Color.Red.Name;
                    }
                }
                if (audioData.IsActive)
                {
                    if (device.DataFlow == DataFlow.Capture)
                    {
                        if (AudioControl.MMAudio.DefaultCommunicationsCapture != null)
                        {
                            audioData.IsCommunicationsDefault = device.Id == AudioControl.MMAudio.DefaultCommunicationsCapture.Id;
                        }
                        if (AudioControl.MMAudio.DefaultMultimediaCapture != null)
                        {
                            audioData.IsMultimediaDefault = device.Id == AudioControl.MMAudio.DefaultMultimediaCapture.Id;
                        }
                    }
                    else if (device.DataFlow == DataFlow.Render)
                    {
                        if (AudioControl.MMAudio.DefaultCommunicationsRender != null)
                        {
                            audioData.IsCommunicationsDefault = device.Id == AudioControl.MMAudio.DefaultCommunicationsRender.Id;
                        }
                        if (AudioControl.MMAudio.DefaultMultimediaRender != null)
                        {
                            audioData.IsMultimediaDefault = device.Id == AudioControl.MMAudio.DefaultMultimediaRender.Id;
                        }
                    }
                    audioData.Volume = (float)Math.Round(device.Volume, 2);
                }
            }
            else if (audioControl is IAudioControlSession session)
            {
                audioData.Id = session.InstanceId;
                audioData.IsActive = session.State != AudioSessionState.Expired || session.IsSystemSoundsSession;
            }

            if (audioData.IsActive && PluginSettings.PeakMeterEnabled)
            {
                float[] peakValues = audioControl.PeakValues.ToArray();
                if (peakValues.Length >= 2)
                {
                    audioData.PeakL = (float)Math.Round(peakValues[0], 2);
                    audioData.PeakR = (float)Math.Round(peakValues[1], 2);
                }
                else if (peakValues.Length == 1)
                {
                    audioData.PeakL = (float)Math.Round(peakValues[0], 2);
                    audioData.PeakR = (float)Math.Round(peakValues[0], 2);
                }
            }

            return audioData;
        }

        public static void ToggleMute(IAudioControl audioControl)
        {
            audioControl.Muted = !audioControl.Muted;
        }

        public static void SetRelativeVolume(IAudioControl audioControl, int diff)
        {
            bool isScalar = true;
            float fltDiff, targetVolume;
            if (audioControl is IAudioControlDevice device && device.DataFlow == DataFlow.Capture)
            {
                float range = Math.Abs(device.MinDecibels) + Math.Abs(device.MaxDecibels);
                float steps = range / device.IncrementDecibels;
                if (steps <= 100.0f)
                {
                    isScalar = false;
                }
            }

            if (isScalar)
            {
                fltDiff = diff / 100.0f;
                targetVolume = (float)Math.Round(audioControl.VolumeScalar + fltDiff, 2);
                if (targetVolume < 0.0f)
                {
                    audioControl.VolumeScalar = 0.0f;
                }
                else if (targetVolume > 1.0f)
                {
                    audioControl.VolumeScalar = 1.0f;
                }
                else
                {
                    audioControl.VolumeScalar = targetVolume;
                }
            }
            else
            {
                IAudioControlDevice audioControlDevice = audioControl as IAudioControlDevice;
                fltDiff = audioControlDevice.IncrementDecibels * diff;
                targetVolume = audioControlDevice.Volume + fltDiff;
                if (targetVolume < audioControlDevice.MinDecibels)
                {
                    audioControlDevice.Volume = audioControlDevice.MinDecibels;
                }
                else if (targetVolume > audioControlDevice.MaxDecibels)
                {
                    audioControlDevice.Volume = audioControlDevice.MaxDecibels;
                }
                else
                {
                    audioControlDevice.Volume = targetVolume;
                }
            }
        }
    }
}
