namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Linq;
    using System.Timers;

    using WindowsInterop;
    using WindowsInterop.CoreAudio;
    using WindowsInterop.Win32;

    internal class AudioSwitchDeviceCommand : ActionEditorCommand
    {
        
        public const string DEVICE = "device";
        private ConcurrentDictionary<string, string> KeyValuePairs { get; }

        public AudioSwitchDeviceCommand()
        {
            base.DisplayName = "Audio switch device";
            base.Description = "Select and switch the default audio render device.";
            base.GroupName   = "";
            base.IsWidget    = true;

            base.ActionEditor.AddControlEx(new ActionEditorListbox(name: DEVICE, labelText: "Select Audio Device"));
            base.ActionEditor.ListboxItemsRequested += this.OnListboxItemsRequested;

            this.KeyValuePairs = new ConcurrentDictionary<string, string>();
        }

        protected override bool RunCommand(ActionEditorActionParameters actionParameters)
        {
            if(actionParameters.TryGetString(DEVICE, out var deviceId))
            {
                var device = AudioControl.MMAudio.RenderDevices.FirstOrDefault(x => x.Id == deviceId);
                if (device != null)
                {
                    AudioControl.MMAudio.SetDefaultAudioEndpoint(device.Id, Role.Multimedia);
                    return true;
                }
            }
            return false;
        }

        private void OnListboxItemsRequested(object sender, ActionEditorListboxItemsRequestedEventArgs e)
        {
            if (e.ControlName.Equals(DEVICE))
            {
                foreach (IAudioControlDevice device in AudioControl.MMAudio.RenderDevices.Where(x => x.State == DeviceState.Active))
                {
                    e.AddItem(name: device.Id, displayName: device.DisplayName, description: "");
                    this.KeyValuePairs.TryAdd(device.Id, device.DisplayName);
                }
            }
        }
    }
}
