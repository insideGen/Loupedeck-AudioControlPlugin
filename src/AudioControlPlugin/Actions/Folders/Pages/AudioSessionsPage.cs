namespace Loupedeck.AudioControlPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Timers;

    using WindowsInterop.CoreAudio;

    using static Loupedeck.AudioControlPlugin.PluginExtension;

    internal class AudioSessionsPage : FolderPage
    {
        public enum DefaultType
        {
            Communications,
            Multimedia
        }

        private readonly ActionImageStore<AudioImageData> _actionImageStore = new ActionImageStore<AudioImageData>(new AudioImageFactory());
        private readonly bool _isDefaultDevices;
        private readonly DefaultType _defaultType;

        private IAudioControlDevice _render = null;
        private IAudioControlDevice _capture = null;
        private IEnumerable<IAudioControlSession> _sessions = null;
        private string _selectedActionName = null;

        public AudioSessionsPage(Folder parent) : base(parent)
        {
            this._isDefaultDevices = false;
        }

        public AudioSessionsPage(Folder parent, IAudioControlDevice render) : base(parent)
        {
            this._isDefaultDevices = false;
            this._render = render;
        }

        public AudioSessionsPage(Folder parent, IAudioControlDevice render, IAudioControlDevice capture) : base(parent)
        {
            this._isDefaultDevices = false;
            this._render = render;
            this._capture = capture;
        }

        public AudioSessionsPage(Folder parent, DefaultType defaultType) : base(parent)
        {
            this._isDefaultDevices = true;
            this._defaultType = defaultType;
        }

        private void Plugin_OnElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (string actionName in base.ButtonActionNames)
            {
                this.RefreshActionImage(actionName);
            }
        }

        private void MMAudio_OnSessionStateChanged(object sender, AudioSessionState e)
        {
            base.ButtonActionNamesChanged();
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
            AudioControl.MMAudio.SessionStateChanged += this.MMAudio_OnSessionStateChanged;
            AudioControl.MMAudio.DeviceStateChanged += this.MMAudio_OnDeviceStateChanged;
            AudioControl.MMAudio.DevicePropertyChanged += this.MMAudio_OnDevicePropertyChanged;
            AudioControl.MMAudio.Devices.CollectionChanged += this.MMAudio_OnCollectionChanged;
        }

        public override void Leave()
        {
            AudioControlPlugin.RefreshTimer.Elapsed -= this.Plugin_OnElapsed;
            AudioControl.MMAudio.SessionStateChanged -= this.MMAudio_OnSessionStateChanged;
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
            if (this._isDefaultDevices)
            {
                if (this._defaultType == DefaultType.Communications)
                {
                    this._render = AudioControl.MMAudio.DefaultCommunicationsRender;
                    this._capture = AudioControl.MMAudio.DefaultCommunicationsCapture;
                }
                else if (this._defaultType == DefaultType.Multimedia)
                {
                    this._render = AudioControl.MMAudio.DefaultMultimediaRender;
                    this._capture = AudioControl.MMAudio.DefaultMultimediaCapture;
                }
                this._sessions = this._render.Sessions.Where(s => s.IsSystemSoundsSession || s.State == AudioSessionState.Active);
            }
            else if (this._render is null)
            {
                this._sessions = AudioControl.MMAudio.RenderSessions.Where(s => !s.IsSystemSoundsSession && s.State == AudioSessionState.Active);
            }
            else
            {
                this._sessions = this._render.Sessions.Where(s => s.IsSystemSoundsSession || s.State == AudioSessionState.Active);
            }
            foreach (IAudioControlSession session in this._sessions)
            {
                if (session.IsSystemSoundsSession)
                {
                    actionNames.Insert(0, session.InstanceId);
                }
                else
                {
                    actionNames.Add(session.InstanceId);
                }
            }
            if (this._render != null)
            {
                actionNames.Insert(0, this._render.Id);
            }
            if (this._capture != null)
            {
                actionNames.Insert(0, this._capture.Id);
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
                    if (audioControl is IAudioControlSession audioControlSession && !audioControlSession.IsSystemSoundsSession)
                    {
                        this.NavigateTo(new AudioInOutSessionPage(base.Folder, audioControlSession));
                    }
                    this._selectedActionName = actionParameter;
                }
                else if (touchEvent.EventType == DeviceTouchEventType.Move)
                {
                    if (touchEvent.GetOrientation() == DeviceTouchEventOrientation.Vertical)
                    {
                        AudioControl.SetRelativeVolume(audioControl, (touchEvent.DeltaY < 0 ? 1 : touchEvent.DeltaY > 0 ? -1 : 0) * 10);
                    }
                }
            }
            return false;
        }
    }
}
