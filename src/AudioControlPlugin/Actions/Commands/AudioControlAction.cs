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

    internal class AudioControlAction
    {
        public const string COMMUNICATIONS_NAME = "defaultCommunications";
        public const string COMMUNICATIONS_DISPLAY_NAME = "Default communications";

        public const string MULTIMEDIA_NAME = "defaultMultimedia";
        public const string MULTIMEDIA_DISPLAY_NAME = "Default multimedia";

        public const string FOREGROUND_NAME = "foregroundApplication";
        public const string FOREGROUND_DISPLAY_NAME = "Foreground application";

        public static string ChannelA { get; set; }
        public static string ChannelB { get; set; }
        public static string ChannelC { get; set; }

        private static bool IsHighlighted(string actionParametersString, ActionChannel channel)
        {
            bool highlighted = false;
            if (channel == ActionChannel.A)
            {
                highlighted = actionParametersString == ChannelA;
            }
            else if (channel == ActionChannel.B)
            {
                highlighted = actionParametersString == ChannelB;
            }
            else if (channel == ActionChannel.C)
            {
                highlighted = actionParametersString == ChannelC;
            }
            return highlighted;
        }

        private enum ActionEditorControl
        {
            Channel,
            Type,
            Endpoint,
            ToggleDefaultEndpointMode,
            ToggleDefaultEndpoint1,
            ToggleDefaultEndpoint2,
            LongPressAction
        }

        private enum ActionChannel
        {
            None,
            A,
            B,
            C
        }

        private enum EndpointType
        {
            Capture,
            Render,
            Application
        }

        private enum ToggleDefaultEndpointMode
        {
            None,
            Communication,
            Multimedia,
            Both
        }

        private enum LongPressAction
        {
            None,
            MuteAll,
            ToggleDefaultEndpoint
        }

        private IActionEditorAction Parent { get; }
        private ConcurrentDictionary<string, string> EndpointDisplayNames { get; }
        private ActionImageStore<AudioImageData> ActionImageStore { get; }

        public AudioControlAction(IActionEditorAction action)
        {
            this.Parent = action;

            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.Channel.ToLower(), labelText: "Channel"));
            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.Type.ToLower(), labelText: "Type")).SetRequired();
            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.Endpoint.ToLower(), labelText: "Endpoint")).SetRequired();
            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.ToggleDefaultEndpointMode.ToLower(), labelText: "Toggle default endpoint mode"));
            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.ToggleDefaultEndpoint1.ToLower(), labelText: "Toggle default endpoint 1"));
            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.ToggleDefaultEndpoint2.ToLower(), labelText: "Toggle default endpoint 2"));
            this.Parent.ActionEditor.AddControlEx(new ActionEditorListbox(name: ActionEditorControl.LongPressAction.ToLower(), labelText: "Long-press action"));

            this.Parent.ActionEditor.ControlsStateRequested += this.OnControlsStateRequested;
            this.Parent.ActionEditor.ListboxItemsRequested += this.OnListboxItemsRequested;
            this.Parent.ActionEditor.ControlValueChanged += this.OnControlValueChanged;

            this.EndpointDisplayNames = new ConcurrentDictionary<string, string>();
            this.ActionImageStore = new ActionImageStore<AudioImageData>(new AudioImageFactory());
        }

        private void ApplyControlStates(ActionEditorState actionEditorState)
        {
            ActionEditorControlState channelState = actionEditorState.GetControlState(ActionEditorControl.Channel.ToLower());
            ActionEditorControlState typeState = actionEditorState.GetControlState(ActionEditorControl.Type.ToLower());
            ActionEditorControlState endpointState = actionEditorState.GetControlState(ActionEditorControl.Endpoint.ToLower());
            ActionEditorControlState toggleDefaultEndpointMode = actionEditorState.GetControlState(ActionEditorControl.ToggleDefaultEndpointMode.ToLower());
            ActionEditorControlState toggleDefaultEndpoint1State = actionEditorState.GetControlState(ActionEditorControl.ToggleDefaultEndpoint1.ToLower());
            ActionEditorControlState toggleDefaultEndpoint2State = actionEditorState.GetControlState(ActionEditorControl.ToggleDefaultEndpoint2.ToLower());
            ActionEditorControlState longPressActionState = actionEditorState.GetControlState(ActionEditorControl.LongPressAction.ToLower());

            //

            channelState.IsEnabled = true;
            channelState.Value = string.IsNullOrEmpty(channelState.Value) ? ActionChannel.None.ToLower() : channelState.Value;

            typeState.IsEnabled = true;

            endpointState.IsEnabled = !string.IsNullOrEmpty(typeState.Value);

            toggleDefaultEndpointMode.IsEnabled = endpointState.Value == COMMUNICATIONS_NAME || endpointState.Value == MULTIMEDIA_NAME;
            toggleDefaultEndpointMode.Value = toggleDefaultEndpointMode.IsEnabled ? toggleDefaultEndpointMode.Value : ToggleDefaultEndpointMode.None.ToLower();

            toggleDefaultEndpoint1State.IsEnabled = toggleDefaultEndpointMode.Value == ToggleDefaultEndpointMode.Communication.ToLower() || toggleDefaultEndpointMode.Value == ToggleDefaultEndpointMode.Multimedia.ToLower() || toggleDefaultEndpointMode.Value == ToggleDefaultEndpointMode.Both.ToLower();
            toggleDefaultEndpoint1State.Value = toggleDefaultEndpoint1State.IsEnabled ? toggleDefaultEndpoint1State.Value : "";

            toggleDefaultEndpoint2State.IsEnabled = toggleDefaultEndpoint1State.IsEnabled;
            toggleDefaultEndpoint2State.Value = toggleDefaultEndpoint2State.IsEnabled ? toggleDefaultEndpoint2State.Value : "";

            longPressActionState.IsEnabled = !string.IsNullOrEmpty(typeState.Value);
            longPressActionState.Value = string.IsNullOrEmpty(longPressActionState.Value) ? LongPressAction.None.ToLower() : longPressActionState.Value;

            //

            actionEditorState.SetEnabled(ActionEditorControl.Channel.ToLower(), channelState.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.Channel.ToLower(), channelState.Value);
            actionEditorState.SetEnabled(ActionEditorControl.Type.ToLower(), typeState.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.Type.ToLower(), typeState.Value);
            actionEditorState.SetEnabled(ActionEditorControl.Endpoint.ToLower(), endpointState.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.Endpoint.ToLower(), endpointState.Value);
            actionEditorState.SetEnabled(ActionEditorControl.ToggleDefaultEndpointMode.ToLower(), toggleDefaultEndpointMode.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.ToggleDefaultEndpointMode.ToLower(), toggleDefaultEndpointMode.Value);
            actionEditorState.SetEnabled(ActionEditorControl.ToggleDefaultEndpoint1.ToLower(), toggleDefaultEndpoint1State.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.ToggleDefaultEndpoint1.ToLower(), toggleDefaultEndpoint1State.Value);
            actionEditorState.SetEnabled(ActionEditorControl.ToggleDefaultEndpoint2.ToLower(), toggleDefaultEndpoint2State.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.ToggleDefaultEndpoint2.ToLower(), toggleDefaultEndpoint2State.Value);
            actionEditorState.SetEnabled(ActionEditorControl.LongPressAction.ToLower(), longPressActionState.IsEnabled);
            actionEditorState.SetValue(ActionEditorControl.LongPressAction.ToLower(), longPressActionState.Value);
        }

        private void OnControlsStateRequested(object sender, ActionEditorControlsStateRequestedEventArgs e)
        {
            this.ApplyControlStates(e.ActionEditorState);
        }

        private void OnListboxItemsRequested(object sender, ActionEditorListboxItemsRequestedEventArgs e)
        {
            if (e.ControlName == ActionEditorControl.Channel.ToLower())
            {
                e.AddItem(ActionChannel.None.ToLower(), "None", "");
                e.AddItem(ActionChannel.A.ToLower(), "A", "");
                e.AddItem(ActionChannel.B.ToLower(), "B", "");
                e.AddItem(ActionChannel.C.ToLower(), "C", "");
            }
            else if (e.ControlName == ActionEditorControl.Type.ToLower())
            {
                e.AddItem(EndpointType.Capture.ToLower(), "Capture", "");
                e.AddItem(EndpointType.Render.ToLower(), "Render", "");
                e.AddItem(EndpointType.Application.ToLower(), "Application", "");
            }
            else if (e.ControlName == ActionEditorControl.Endpoint.ToLower())
            {
                ActionEditorControlState typeState = e.ActionEditorState.GetControlState(ActionEditorControl.Type.ToLower());
                string endpointType = e.ActionEditorState.GetControlValue(ActionEditorControl.Type.ToLower());
                if (!string.IsNullOrEmpty(endpointType))
                {
                    this.EndpointDisplayNames.Clear();
                    if (endpointType == EndpointType.Capture.ToLower())
                    {
                        e.AddItem(COMMUNICATIONS_NAME, $"* {COMMUNICATIONS_DISPLAY_NAME}", "");
                        e.AddItem(MULTIMEDIA_NAME, $"* {MULTIMEDIA_DISPLAY_NAME}", "");
                        this.EndpointDisplayNames.TryAdd(COMMUNICATIONS_NAME, COMMUNICATIONS_DISPLAY_NAME);
                        this.EndpointDisplayNames.TryAdd(MULTIMEDIA_NAME, MULTIMEDIA_DISPLAY_NAME);
                        foreach (IAudioControlDevice device in AudioControl.MMAudio.CaptureDevices.Where(x => x.State == DeviceState.Active))
                        {
                            e.AddItem(device.Id, device.DisplayName, "");
                            this.EndpointDisplayNames.TryAdd(device.Id, device.DisplayName);
                        }
                    }
                    else if (endpointType == EndpointType.Render.ToLower())
                    {
                        e.AddItem(COMMUNICATIONS_NAME, $"* {COMMUNICATIONS_DISPLAY_NAME}", "");
                        e.AddItem(MULTIMEDIA_NAME, $"* {MULTIMEDIA_DISPLAY_NAME}", "");
                        this.EndpointDisplayNames.TryAdd(COMMUNICATIONS_NAME, COMMUNICATIONS_DISPLAY_NAME);
                        this.EndpointDisplayNames.TryAdd(MULTIMEDIA_NAME, MULTIMEDIA_DISPLAY_NAME);
                        foreach (IAudioControlDevice device in AudioControl.MMAudio.RenderDevices.Where(x => x.State == DeviceState.Active))
                        {
                            e.AddItem(device.Id, device.DisplayName, "");
                            this.EndpointDisplayNames.TryAdd(device.Id, device.DisplayName);
                        }
                    }
                    else if (endpointType == EndpointType.Application.ToLower())
                    {
                        e.AddItem(FOREGROUND_NAME, $"* {FOREGROUND_DISPLAY_NAME}", "");
                        this.EndpointDisplayNames.TryAdd(FOREGROUND_NAME, FOREGROUND_DISPLAY_NAME);
                        foreach (IAudioControlSession session in AudioControl.MMAudio.RenderSessions)
                        {
                            if (!string.IsNullOrEmpty(session.DisplayName))
                            {
                                if (!this.EndpointDisplayNames.Keys.Any(x => (AudioSessionInstanceIdentifier.FromString(x) is AudioSessionInstanceIdentifier asii) && ((asii.ExePath == session.ExePath) || (session.ExeId == Guid.Empty.ToString() && asii.ExeId == session.ExeId))))
                                {
                                    e.AddItem(session.Id, session.DisplayName, "");
                                    this.EndpointDisplayNames.TryAdd(session.Id, session.DisplayName);
                                }
                            }
                        }
                    }
                }
            }
            else if (e.ControlName == ActionEditorControl.ToggleDefaultEndpointMode.ToLower())
            {
                ActionEditorControlState typeState = e.ActionEditorState.GetControlState(ActionEditorControl.Type.ToLower());
                ActionEditorControlState endpointState = e.ActionEditorState.GetControlState(ActionEditorControl.Endpoint.ToLower());

                e.AddItem(ToggleDefaultEndpointMode.None.ToLower(), "None", "");

                if (endpointState.Value == COMMUNICATIONS_NAME || endpointState.Value == MULTIMEDIA_NAME)
                {
                    if (endpointState.Value == COMMUNICATIONS_NAME)
                    {
                        e.AddItem(ToggleDefaultEndpointMode.Communication.ToLower(), "Communication", "");
                    }
                    else if (endpointState.Value == MULTIMEDIA_NAME)
                    {
                        e.AddItem(ToggleDefaultEndpointMode.Multimedia.ToLower(), "Multimedia", "");
                    }

                    e.AddItem(ToggleDefaultEndpointMode.Both.ToLower(), "Communication and Multimedia", "");
                }
            }
            else if (e.ControlName == ActionEditorControl.ToggleDefaultEndpoint1.ToLower() || e.ControlName == ActionEditorControl.ToggleDefaultEndpoint2.ToLower())
            {
                ActionEditorControlState typeState = e.ActionEditorState.GetControlState(ActionEditorControl.Type.ToLower());
                if (!string.IsNullOrEmpty(typeState.Value))
                {
                    if (typeState.Value == EndpointType.Capture.ToLower())
                    {
                        foreach (IAudioControlDevice device in AudioControl.MMAudio.CaptureDevices.Where(x => x.State == DeviceState.Active))
                        {
                            e.AddItem(device.Id, device.DisplayName, "");
                        }
                    }
                    else if (typeState.Value == EndpointType.Render.ToLower())
                    {
                        foreach (IAudioControlDevice device in AudioControl.MMAudio.RenderDevices.Where(x => x.State == DeviceState.Active))
                        {
                            e.AddItem(device.Id, device.DisplayName, "");
                        }
                    }
                }
            }
            else if (e.ControlName == ActionEditorControl.LongPressAction.ToLower())
            {
                ActionEditorControlState typeState = e.ActionEditorState.GetControlState(ActionEditorControl.Type.ToLower());
                ActionEditorControlState endpointState = e.ActionEditorState.GetControlState(ActionEditorControl.Endpoint.ToLower());

                e.AddItem(LongPressAction.None.ToLower(), "None", "");

                if (typeState.Value == EndpointType.Capture.ToLower() || typeState.Value == EndpointType.Render.ToLower())
                {
                    e.AddItem(LongPressAction.MuteAll.ToLower(), "Mute/Unmute all", "");
                }

                if (endpointState.Value == COMMUNICATIONS_NAME || endpointState.Value == MULTIMEDIA_NAME)
                {
                    e.AddItem(LongPressAction.ToggleDefaultEndpoint.ToLower(), "Toggle default endpoint", "");
                }
            }
        }

        private void OnControlValueChanged(object sender, ActionEditorControlValueChangedEventArgs e)
        {
            this.ApplyControlStates(e.ActionEditorState);

            this.Parent.ActionEditor.ListboxItemsChanged(ActionEditorControl.Endpoint.ToLower());
            this.Parent.ActionEditor.ListboxItemsChanged(ActionEditorControl.ToggleDefaultEndpointMode.ToLower());
            this.Parent.ActionEditor.ListboxItemsChanged(ActionEditorControl.ToggleDefaultEndpoint1.ToLower());
            this.Parent.ActionEditor.ListboxItemsChanged(ActionEditorControl.ToggleDefaultEndpoint2.ToLower());
            this.Parent.ActionEditor.ListboxItemsChanged(ActionEditorControl.LongPressAction.ToLower());

            string displayName = string.Empty;

            ActionEditorControlState endpointState = e.ActionEditorState.GetControlState(ActionEditorControl.Endpoint.ToLower());
            this.EndpointDisplayNames.TryGetValue(endpointState.Value, out string endpointDisplayName);

            if (this.Parent is ActionEditorCommand)
            {
                displayName += "Touch - ";
            }
            else if (this.Parent is ActionEditorAdjustment)
            {
                displayName += "Dial - ";
            }

            string channel = Enum.Parse(typeof(ActionChannel), e.ActionEditorState.GetControlValue(ActionEditorControl.Channel.ToLower()), true).ToString();
            if (channel != ActionChannel.None.ToString())
            {
                displayName += $"{channel} - ";
            }

            displayName += $"{endpointDisplayName}";

            if (endpointState.Value == COMMUNICATIONS_NAME || endpointState.Value == MULTIMEDIA_NAME)
            {
                string type = Enum.Parse(typeof(EndpointType), e.ActionEditorState.GetControlValue(ActionEditorControl.Type.ToLower()), true).ToString();
                displayName += $" {type.ToLower()}";
            }

            if (this.Parent is ActionEditorAdjustment)
            {
                this.Parent.ResetDisplayName = $"{displayName} - Adjustment reset";
                displayName += " - Adjustment";
            }

            e.ActionEditorState.SetDisplayName(displayName);
        }

        private string StringifyActionParameters(ActionEditorActionParameters actionParameters)
        {
            string channel = actionParameters.Parameters.GetValue(ActionEditorControl.Channel.ToLower(), ActionChannel.None.ToLower());
            string type = actionParameters.Parameters.GetValue(ActionEditorControl.Type.ToLower(), "");
            string endpoint = actionParameters.Parameters.GetValue(ActionEditorControl.Endpoint.ToLower(), "");
            string toggleDefaultEndpointMode = actionParameters.Parameters.GetValue(ActionEditorControl.ToggleDefaultEndpointMode.ToLower(), ToggleDefaultEndpointMode.None.ToLower());
            string toggleDefaultEndpoint1 = actionParameters.Parameters.GetValue(ActionEditorControl.ToggleDefaultEndpoint1.ToLower(), "");
            string toggleDefaultEndpoint2 = actionParameters.Parameters.GetValue(ActionEditorControl.ToggleDefaultEndpoint2.ToLower(), "");
            string longPressAction = actionParameters.Parameters.GetValue(ActionEditorControl.LongPressAction.ToLower(), LongPressAction.None.ToLower());

            return $"{channel}+{type}+{endpoint}+{toggleDefaultEndpointMode}+{toggleDefaultEndpoint1}+{toggleDefaultEndpoint2}+{longPressAction}";
        }

        private bool TryDecodeActionParametersString(string actionParameters, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction)
        {
            try
            {
                string[] parameters = actionParameters.Split("+");

                string channelParam = parameters[0];
                string typeParam = parameters[1];
                string endpointParam = parameters[2];
                string toggleDefaultEndpointModeParam = parameters[3];
                string toggleDefaultEndpoint1Param = parameters[4];
                string toggleDefaultEndpoint2Param = parameters[5];
                string longPressActionParam = parameters[6];

                channel = (ActionChannel)Enum.Parse(typeof(ActionChannel), channelParam, true);
                type = (EndpointType)Enum.Parse(typeof(EndpointType), typeParam, true);
                endpointId = endpointParam;
                toggleDefaultEndpointMode = (ToggleDefaultEndpointMode)Enum.Parse(typeof(ToggleDefaultEndpointMode), toggleDefaultEndpointModeParam, true);
                toggleDefaultEndpoint1Id = toggleDefaultEndpoint1Param;
                toggleDefaultEndpoint2Id = toggleDefaultEndpoint2Param;
                longPressAction = (LongPressAction)Enum.Parse(typeof(LongPressAction), longPressActionParam, true);

                if (type == EndpointType.Capture)
                {
                    if (endpointId == COMMUNICATIONS_NAME && AudioControl.MMAudio.DefaultCommunicationsCapture != null)
                    {
                        endpointId = AudioControl.MMAudio.DefaultCommunicationsCapture.Id;
                    }
                    else if (endpointId == MULTIMEDIA_NAME && AudioControl.MMAudio.DefaultMultimediaCapture != null)
                    {
                        endpointId = AudioControl.MMAudio.DefaultMultimediaCapture.Id;
                    }
                }
                else if (type == EndpointType.Render)
                {
                    if (endpointId == COMMUNICATIONS_NAME && AudioControl.MMAudio.DefaultCommunicationsRender != null)
                    {
                        endpointId = AudioControl.MMAudio.DefaultCommunicationsRender.Id;
                    }
                    else if (endpointId == MULTIMEDIA_NAME && AudioControl.MMAudio.DefaultMultimediaRender != null)
                    {
                        endpointId = AudioControl.MMAudio.DefaultMultimediaRender.Id;
                    }
                }
                else if (type == EndpointType.Application)
                {
                    if (endpointId == FOREGROUND_NAME)
                    {
                        if (WindowEnumerator.TryGetForegroundProcessId(out int processId))
                        {
                            if (AppInfo.FromProcess(processId) is AppInfo appInfo)
                            {
                                AudioSessionInstanceIdentifier asii = new AudioSessionInstanceIdentifier($"{{0.0.0.00000000}}.{{{Guid.Empty}}}", appInfo.ExePath, $"{{{Guid.Empty}}}", 1, appInfo.ProcessId);
                                endpointId = asii.ToString();
                            }
                            else
                            {
                                throw new Exception("AppInfo not found.");
                            }
                        }
                        else
                        {
                            throw new Exception("Foreground process not found.");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PluginLog.Error($"Failed to decode action parameters string.|{ex.Message}");

                channel = ActionChannel.None;
                type = EndpointType.Capture;
                endpointId = null;
                toggleDefaultEndpointMode = ToggleDefaultEndpointMode.None;
                toggleDefaultEndpoint1Id = null;
                toggleDefaultEndpoint2Id = null;
                longPressAction = LongPressAction.None;
                return false;
            }
        }

        private bool TryDecodeActionParameters(ActionEditorActionParameters actionParameters, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction)
        {
            try
            {
                string channelParam = actionParameters.Parameters.GetValue(ActionEditorControl.Channel.ToLower(), ActionChannel.None.ToLower());
                channel = (ActionChannel)Enum.Parse(typeof(ActionChannel), channelParam, true);

                string typeParam = actionParameters.Parameters.GetValue(ActionEditorControl.Type.ToLower(), "");
                type = (EndpointType)Enum.Parse(typeof(EndpointType), typeParam, true);

                endpointId = actionParameters.Parameters.GetValue(ActionEditorControl.Endpoint.ToLower(), "");

                string toggleDefaultEndpointModeParam = actionParameters.Parameters.GetValue(ActionEditorControl.ToggleDefaultEndpointMode.ToLower(), ToggleDefaultEndpointMode.None.ToLower());
                toggleDefaultEndpointMode = (ToggleDefaultEndpointMode)Enum.Parse(typeof(ToggleDefaultEndpointMode), toggleDefaultEndpointModeParam, true);

                toggleDefaultEndpoint1Id = actionParameters.Parameters.GetValue(ActionEditorControl.ToggleDefaultEndpoint1.ToLower(), "");
                toggleDefaultEndpoint2Id = actionParameters.Parameters.GetValue(ActionEditorControl.ToggleDefaultEndpoint2.ToLower(), "");

                string longPressActionParam = actionParameters.Parameters.GetValue(ActionEditorControl.LongPressAction.ToLower(), LongPressAction.None.ToLower());
                longPressAction = (LongPressAction)Enum.Parse(typeof(LongPressAction), longPressActionParam, true);

                return true;
            }
            catch
            {
                PluginLog.Error("Failed to decode action parameters.");

                channel = ActionChannel.None;
                type = EndpointType.Capture;
                endpointId = null;
                toggleDefaultEndpointMode = ToggleDefaultEndpointMode.None;
                toggleDefaultEndpoint1Id = null;
                toggleDefaultEndpoint2Id = null;
                longPressAction = LongPressAction.None;

                return false;
            }
        }

        private void RefreshActionImage(string actionParametersString)
        {
            if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction))
            {
                AudioImageData audioImageData = null;
                bool highlighted = AudioControlAction.IsHighlighted(actionParametersString, channel);
                if (AudioControl.TryGetAudioControl(endpointId, out IAudioControl audioControl))
                {
                    audioImageData = AudioControl.CreateAudioData(audioControl, highlighted);
                }
                else
                {
                    if (type == EndpointType.Application)
                    {
                        string displayName = string.Empty;
                        string iconPath = string.Empty;
                        AudioSessionInstanceIdentifier asii = AudioSessionInstanceIdentifier.FromString(endpointId);
                        if (AppInfo.FromProcess(asii.ProcessId) is AppInfo appInfoFromProcess)
                        {
                            displayName = appInfoFromProcess.DisplayName;
                            iconPath = appInfoFromProcess.LogoPath;
                        }
                        else if (AppInfo.FromPath(asii.ExePath) is AppInfo appInfoFromPath)
                        {
                            displayName = appInfoFromPath.DisplayName;
                            iconPath = appInfoFromPath.LogoPath;
                        }
                        audioImageData = new AudioImageData();
                        audioImageData.Id = actionParametersString;
                        audioImageData.DisplayName = displayName;
                        audioImageData.UnmutedIconPath = iconPath;
                        audioImageData.IsActive = false;
                        audioImageData.Highlighted = highlighted;
                    }
                    else
                    {
                        audioImageData = new AudioImageData();
                        audioImageData.Id = actionParametersString;
                        audioImageData.DataFlow = type == EndpointType.Capture ? DataFlow.Capture : DataFlow.Render;
                        audioImageData.NotFound = true;
                        audioImageData.Highlighted = highlighted;
                    }
                }
                if (this.ActionImageStore.UpdateImage(actionParametersString, audioImageData))
                {
                    this.Parent.ActionImageChanged();
                }
            }
        }

        private void Plugin_OnElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (string imageId in this.ActionImageStore.ActionImageIds)
            {
                this.RefreshActionImage(imageId);
            }
        }

        public bool OnLoad()
        {
            this.Parent.ActionImageChanged();
            AudioControlPlugin.RefreshTimer.Elapsed += this.Plugin_OnElapsed;
            return true;
        }

        public bool OnUnload()
        {
            AudioControlPlugin.RefreshTimer.Elapsed -= this.Plugin_OnElapsed;
            return true;
        }

        public string GetDisplayName(ActionEditorActionParameters actionParameters)
        {
            return this.Parent.DisplayName;
        }

        public BitmapImage GetImage(ActionEditorActionParameters actionParameters, int imageWidth, int imageHeight)
        {
            if (this.TryDecodeActionParameters(actionParameters, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction))
            {
                string actionParametersString = this.StringifyActionParameters(actionParameters);
                if (this.ActionImageStore.TryGetImage(actionParametersString, PluginImage.GetImageSize(imageWidth, imageHeight), out BitmapImage bitmapImage))
                {
                    return bitmapImage;
                }
                if (this.Parent is ActionEditorAdjustment)
                {
                    if (channel == ActionChannel.A && string.IsNullOrEmpty(ChannelA))
                    {
                        ChannelA = actionParametersString;
                    }
                    else if (channel == ActionChannel.B && string.IsNullOrEmpty(ChannelB))
                    {
                        ChannelB = actionParametersString;
                    }
                    else if (channel == ActionChannel.C && string.IsNullOrEmpty(ChannelC))
                    {
                        ChannelC = actionParametersString;
                    }
                }
                this.RefreshActionImage(actionParametersString);
            }
            return PluginImage.DrawBlackImage(PluginImage.GetImageSize(imageWidth, imageHeight));
        }

        public bool ProcessButtonEvent2(ActionEditorActionParameters actionParameters, DeviceButtonEvent2 buttonEvent)
        {
            string actionParametersString = this.StringifyActionParameters(actionParameters);
            if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction))
            {
                if (buttonEvent.EventType == DeviceButtonEventType.Press)
                {
                    if (channel == ActionChannel.None)
                    {
                        if (AudioControl.TryGetAudioControl(endpointId, out IAudioControl audioControl))
                        {
                            AudioControl.ToggleMute(audioControl);
                        }
                    }
                    else if (channel == ActionChannel.A)
                    {
                        if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channelA, out EndpointType typeA, out string endpointIdA, out ToggleDefaultEndpointMode toggleDefaultEndpointModeA, out string toggleDefaultEndpoint1IdA, out string toggleDefaultEndpoint2IdA, out LongPressAction longPressActionA))
                        {
                            if (AudioControl.TryGetAudioControl(endpointIdA, out IAudioControl audioControlA))
                            {
                                AudioControl.ToggleMute(audioControlA);
                            }
                        }
                    }
                    else if (channel == ActionChannel.B)
                    {
                        if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channelB, out EndpointType typeB, out string endpointIdB, out ToggleDefaultEndpointMode toggleDefaultEndpointModeB, out string toggleDefaultEndpoint1IdB, out string toggleDefaultEndpoint2IdB, out LongPressAction longPressActionB))
                        {
                            if (AudioControl.TryGetAudioControl(endpointIdB, out IAudioControl audioControlB))
                            {
                                AudioControl.ToggleMute(audioControlB);
                            }
                        }
                    }
                    else if (channel == ActionChannel.C)
                    {
                        if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channelC, out EndpointType typeC, out string endpointIdC, out ToggleDefaultEndpointMode toggleDefaultEndpointModeC, out string toggleDefaultEndpoint1IdC, out string toggleDefaultEndpoint2IdC, out LongPressAction longPressActionC))
                        {
                            if (AudioControl.TryGetAudioControl(endpointIdC, out IAudioControl audioControlC))
                            {
                                AudioControl.ToggleMute(audioControlC);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool ProcessTouchEvent(ActionEditorActionParameters actionParameters, DeviceTouchEvent touchEvent)
        {
            string actionParametersString = this.StringifyActionParameters(actionParameters);
            if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction))
            {
                if (touchEvent.EventType == DeviceTouchEventType.Tap)
                {
                    if (channel == ActionChannel.A)
                    {
                        ChannelA = actionParametersString;
                    }
                    else if (channel == ActionChannel.B)
                    {
                        ChannelB = actionParametersString;
                    }
                    else if (channel == ActionChannel.C)
                    {
                        ChannelC = actionParametersString;
                    }
                }
                else if (touchEvent.EventType == DeviceTouchEventType.DoubleTap)
                {
                    if (AudioControl.TryGetAudioControl(endpointId, out IAudioControl audioControl))
                    {
                        AudioControl.ToggleMute(audioControl);
                    }
                }
                else if (touchEvent.EventType == DeviceTouchEventType.LongPress)
                {
                    if (AudioControl.TryGetAudioControl(endpointId, out IAudioControl audioControl))
                    {
                        if (longPressAction == LongPressAction.MuteAll)
                        {
                            if (audioControl is IAudioControlDevice audioControlDevice)
                            {
                                bool muted = !audioControlDevice.Muted;
                                foreach (IAudioControlDevice device in AudioControl.MMAudio.Devices.Where(x => x.State == DeviceState.Active && x.DataFlow == audioControlDevice.DataFlow))
                                {
                                    device.Muted = muted;
                                }
                            }
                        }
                        else if (longPressAction == LongPressAction.ToggleDefaultEndpoint)
                        {
                            string endpointTarget = toggleDefaultEndpoint1Id;
                            if (audioControl.Id == endpointTarget)
                            {
                                endpointTarget = toggleDefaultEndpoint2Id;
                            }
                            if (toggleDefaultEndpointMode == ToggleDefaultEndpointMode.Communication)
                            {
                                AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Communications);
                            }
                            else if (toggleDefaultEndpointMode == ToggleDefaultEndpointMode.Multimedia)
                            {
                                AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Multimedia);
                            }
                            else if (toggleDefaultEndpointMode == ToggleDefaultEndpointMode.Both)
                            {
                                AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Communications);
                                AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Multimedia);
                            }
                        }
                    }
                }
                else if (touchEvent.EventType == DeviceTouchEventType.Move)
                {
                    if (AudioControl.TryGetAudioControl(endpointId, out IAudioControl audioControl))
                    {
                        if (touchEvent.DeltaX != 0)
                        {
                            if (toggleDefaultEndpointMode != ToggleDefaultEndpointMode.None)
                            {
                                string endpointTarget = toggleDefaultEndpoint1Id;
                                if (audioControl.Id == endpointTarget)
                                {
                                    endpointTarget = toggleDefaultEndpoint2Id;
                                }
                                if (toggleDefaultEndpointMode == ToggleDefaultEndpointMode.Communication)
                                {
                                    AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Communications);
                                }
                                else if (toggleDefaultEndpointMode == ToggleDefaultEndpointMode.Multimedia)
                                {
                                    AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Multimedia);
                                }
                                else if (toggleDefaultEndpointMode == ToggleDefaultEndpointMode.Both)
                                {
                                    AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Communications);
                                    AudioControl.MMAudio.SetDefaultAudioEndpoint(endpointTarget, Role.Multimedia);
                                }
                            }
                        }
                        else if(touchEvent.DeltaY != 0)
                        {
                            AudioControl.SetRelativeVolume(audioControl, (touchEvent.DeltaY < 0 ? 1 : -1) * 10);
                        }
                    }
                }
            }
            return true;
        }

        public bool ProcessEncoderEvent(ActionEditorActionParameters actionParameters, DeviceEncoderEvent encoderEvent)
        {
            string actionParametersString = this.StringifyActionParameters(actionParameters);
            if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channel, out EndpointType type, out string endpointId, out ToggleDefaultEndpointMode toggleDefaultEndpointMode, out string toggleDefaultEndpoint1Id, out string toggleDefaultEndpoint2Id, out LongPressAction longPressAction))
            {
                if (channel == ActionChannel.None)
                {
                    if (AudioControl.TryGetAudioControl(endpointId, out IAudioControl audioControl))
                    {
                        AudioControl.SetRelativeVolume(audioControl, encoderEvent.Clicks);
                    }
                }
                else if (channel == ActionChannel.A)
                {
                    if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channelA, out EndpointType typeA, out string endpointIdA, out ToggleDefaultEndpointMode toggleDefaultEndpointModeA, out string toggleDefaultEndpoint1IdA, out string toggleDefaultEndpoint2IdA, out LongPressAction longPressActionA))
                    {
                        if (AudioControl.TryGetAudioControl(endpointIdA, out IAudioControl audioControlA))
                        {
                            AudioControl.SetRelativeVolume(audioControlA, encoderEvent.Clicks);
                        }
                    }
                }
                else if (channel == ActionChannel.B)
                {
                    if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channelB, out EndpointType typeB, out string endpointIdB, out ToggleDefaultEndpointMode toggleDefaultEndpointModeB, out string toggleDefaultEndpoint1IdB, out string toggleDefaultEndpoint2IdB, out LongPressAction longPressActionB))
                    {
                        if (AudioControl.TryGetAudioControl(endpointIdB, out IAudioControl audioControlB))
                        {
                            AudioControl.SetRelativeVolume(audioControlB, encoderEvent.Clicks);
                        }
                    }
                }
                else if (channel == ActionChannel.C)
                {
                    if (this.TryDecodeActionParametersString(actionParametersString, out ActionChannel channelC, out EndpointType typeC, out string endpointIdC, out ToggleDefaultEndpointMode toggleDefaultEndpointModeC, out string toggleDefaultEndpoint1IdC, out string toggleDefaultEndpoint2IdC, out LongPressAction longPressActionC))
                    {
                        if (AudioControl.TryGetAudioControl(endpointIdC, out IAudioControl audioControlC))
                        {
                            AudioControl.SetRelativeVolume(audioControlC, encoderEvent.Clicks);
                        }
                    }
                }
            }
            return true;
        }
    }
}
