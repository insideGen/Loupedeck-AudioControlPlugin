namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Linq;

    using WindowsInterop.CoreAudio;

    internal abstract class AudioSwitchDeviceCommand : ActionEditorCommand
    {

        public const string DEVICE = "device";
        public const string TYPE = "type";

        private ConcurrentDictionary<string, string> KeyValuePairs { get; }

        public AudioSwitchDeviceCommand()
        {
            base.DisplayName = "Audio switch device";
            base.Description = "Select and switch the default audio render device.";
            base.GroupName = "";
            base.IsWidget = true;

            base.ActionEditor.AddControlEx(new ActionEditorListbox(name: DEVICE, labelText: "Select Audio Device"));
            base.ActionEditor.AddControlEx(new ActionEditorListbox(name: TYPE, labelText: "Select Type"));
            base.ActionEditor.ListboxItemsRequested += this.OnListboxItemsRequested;

            this.KeyValuePairs = new ConcurrentDictionary<string, string>();
        }

        protected override bool RunCommand(ActionEditorActionParameters actionParameters)
        {
            if (actionParameters.TryGetString(DEVICE, out string deviceId) &&
                actionParameters.TryGetString(TYPE, out string typeName))
            {
                var device = AudioControl.MMAudio.RenderDevices.FirstOrDefault(x => x.Id == deviceId);
                if (device != null)
                {
                    AudioControl.MMAudio.SetDefaultAudioEndpoint(device.Id, Enum.Parse<Role>(typeName));
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
            else if (e.ControlName.Equals(TYPE))
            {
                foreach (string role in Enum.GetNames(typeof(Role)))
                {
                    e.AddItem(name: role, displayName: role, description: $"Set as default {role.ToLower()} device");
                }
            }
        }
    }
}
