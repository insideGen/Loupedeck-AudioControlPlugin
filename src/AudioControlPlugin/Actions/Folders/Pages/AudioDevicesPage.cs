namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Timers;

    using WindowsInterop.CoreAudio;

    using static Loupedeck.AudioControlPlugin.PluginExtension;

    internal class AudioDevicesPage : FolderPage
    {
        private readonly DataFlow _dataFlow;
        private readonly ActionImageStore<AudioImageData> _actionImageStore;

        private string _selectedActionName = string.Empty;

        public AudioDevicesPage(Folder parent, DataFlow dataFlow) : base(parent)
        {
            this._dataFlow = dataFlow;
            this._actionImageStore = new ActionImageStore<AudioImageData>(new AudioImageFactory());
        }

        private void Plugin_OnElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (string actionName in base.ButtonActionNames)
            {
                this.RefreshActionImage(actionName);
            }
        }

        private void MMAudio_OnDeviceStateChanged(object sender, DeviceStateEventArgs e)
        {
            base.ButtonActionNamesChanged();
        }

        private void MMAudio_OnDevicePropertyChanged(object sender, string e)
        {
            base.ButtonActionNamesChanged();
        }

        private void MMAudio_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ButtonActionNamesChanged();
        }

        public override void Enter()
        {
            AudioControlPlugin.RefreshTimer.Elapsed += this.Plugin_OnElapsed;
            AudioControl.MMAudio.DeviceStateChanged += this.MMAudio_OnDeviceStateChanged;
            AudioControl.MMAudio.DevicePropertyChanged += this.MMAudio_OnDevicePropertyChanged;
            AudioControl.MMAudio.Devices.CollectionChanged += this.MMAudio_OnCollectionChanged;
        }

        public override void Leave()
        {
            AudioControlPlugin.RefreshTimer.Elapsed -= this.Plugin_OnElapsed;
            AudioControl.MMAudio.DeviceStateChanged -= this.MMAudio_OnDeviceStateChanged;
            AudioControl.MMAudio.DevicePropertyChanged -= this.MMAudio_OnDevicePropertyChanged;
            AudioControl.MMAudio.Devices.CollectionChanged -= this.MMAudio_OnCollectionChanged;
        }

        public override void Unload()
        {
            this._selectedActionName = string.Empty;
        }

        public void RefreshActionImage(string actionParameter)
        {
            if (AudioControl.TryGetAudioControl(actionParameter, null, out IAudioControl audioControl))
            {
                bool highlighted = this._selectedActionName == actionParameter;
                AudioImageData audioImageData = AudioControl.CreateAudioData(audioControl, highlighted);
                if (this._actionImageStore.UpdateImage(actionParameter, audioImageData))
                {
                    base.CommandImageChanged(actionParameter);
                }
            }
        }

        public BitmapImage GetImage(string actionParameter, PluginImageSize imageSize)
        {
            if (this._actionImageStore.TryGetImage(actionParameter, imageSize, out BitmapImage bitmapImage))
            {
                return bitmapImage;
            }
            this.RefreshActionImage(actionParameter);
            return PluginImage.DrawBlackImage(imageSize);
        }

        public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
        {
            List<string> actionNames = new List<string>();
            IEnumerable<IAudioControlDevice> devices = null;
            if (this._dataFlow == DataFlow.Capture)
            {
                devices = AudioControl.MMAudio.CaptureDevices;
            }
            else if (this._dataFlow == DataFlow.Render)
            {
                devices = AudioControl.MMAudio.RenderDevices;
            }
            if (devices != null)
            {
                foreach (IAudioControlDevice device in devices.Where(x => x.State == DeviceState.Active))
                {
                    actionNames.Add(device.Id);
                }
            }
            return actionNames;
        }

        public override IEnumerable<string> GetEncoderRotateActionNames(DeviceType deviceType)
        {
            return new string[] { "encoder-rotate" };
        }

        public override IEnumerable<string> GetEncoderPressActionNames(DeviceType deviceType)
        {
            return new string[] { "encoder-press" };
        }

        public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            return this.GetImage(actionParameter, imageSize);
        }

        public override BitmapImage GetAdjustmentImage(string actionParameter, PluginImageSize imageSize)
        {
            return PluginImage.DrawBlackImage(imageSize);
        }

        public override bool ProcessEncoderEvent(string actionParameter, DeviceEncoderEvent encoderEvent)
        {
            if (actionParameter == "encoder-rotate")
            {
                if (AudioControl.TryGetAudioControl(this._selectedActionName, null, out IAudioControl audioControl))
                {
                    AudioControl.SetRelativeVolume(audioControl, encoderEvent.Clicks);
                }
            }
            return false;
        }

        public override bool ProcessButtonEvent2(string actionParameter, DeviceButtonEvent2 buttonEvent)
        {
            if (actionParameter == "encoder-press")
            {
                if (buttonEvent.EventType == DeviceButtonEventType.Press)
                {
                    if (AudioControl.TryGetAudioControl(this._selectedActionName, null, out IAudioControl audioControl))
                    {
                        AudioControl.ToggleMute(audioControl);
                    }
                }
            }
            return false;
        }

        public override bool ProcessTouchEvent(string actionParameter, DeviceTouchEvent touchEvent)
        {
            if (AudioControl.TryGetAudioControl(actionParameter, null, out IAudioControl audioControl))
            {
                if (touchEvent.EventType == DeviceTouchEventType.Tap)
                {
                    this._selectedActionName = actionParameter;
                }
                else if (touchEvent.EventType == DeviceTouchEventType.DoubleTap)
                {
                    if (actionParameter == this._selectedActionName)
                    {
                        AudioControl.ToggleMute(audioControl);
                    }
                    this._selectedActionName = actionParameter;
                }
                else if (touchEvent.EventType == DeviceTouchEventType.LongPress)
                {
                    this._selectedActionName = actionParameter;
                    if (audioControl is IAudioControlDevice audioControlDevice && audioControlDevice.DataFlow == DataFlow.Render)
                    {
                        this.NavigateTo(new AudioSessionsPage(base.Folder, audioControlDevice));
                    }
                }
                else if (touchEvent.EventType == DeviceTouchEventType.Move)
                {
                    if (touchEvent.GetOrientation() == DeviceTouchEventOrientation.Horizontal)
                    {
                        if (touchEvent.DeltaX > 0)
                        {
                            AudioControl.MMAudio.SetDefaultAudioEndpoint(audioControl.Id, Role.Multimedia);
                        }
                        else if (touchEvent.DeltaX < 0)
                        {
                            AudioControl.MMAudio.SetDefaultAudioEndpoint(audioControl.Id, Role.Communications);
                        }
                    }
                    else if (touchEvent.GetOrientation() == DeviceTouchEventOrientation.Vertical)
                    {
                        AudioControl.SetRelativeVolume(audioControl, (touchEvent.DeltaY < 0 ? 1 : -1) * 10);
                    }
                }
            }
            return false;
        }
    }
}
