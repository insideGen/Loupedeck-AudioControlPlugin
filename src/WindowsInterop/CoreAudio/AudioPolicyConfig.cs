namespace WindowsInterop.CoreAudio
{
    using System;
    using System.Runtime.InteropServices;

    using WindowsInterop.CoreAudio.Interfaces;

    public class AudioPolicyConfig : IAudioPolicyConfig
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate HRESULT QueryInterface(nint audioPolicyConfigFactory, nint guid, ref nint audioPolicyConfig);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate HRESULT SetPersistedDefaultAudioEndpointDelegate(nint audioPolicyConfigFactory, uint processId, DataFlow flow, Role role, HSTRING deviceId);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate HRESULT GetPersistedDefaultAudioEndpointDelegate(nint audioPolicyConfigFactory, uint processId, DataFlow flow, Role role, ref HSTRING deviceId);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate HRESULT ClearAllPersistedApplicationDefaultEndpointsDelegate(nint audioPolicyConfigFactory);

        private const int GUID_SIZE = 16;
        private const string MMDEVAPI_TOKEN = @"\\?\SWD#MMDEVAPI#";
        private const string DEVINTERFACE_AUDIO_RENDER = "#{e6327cad-dcec-4949-ae8a-991e976a79d2}";
        private const string DEVINTERFACE_AUDIO_CAPTURE = "#{2eef81be-33fa-4800-9670-1cd474972c3f}";

        private readonly string[] _knownGuids =
        {
            // Pre-H1H2
            "2a59116d-6c4f-45e0-a74f-707e3fef9258",
            // H1H2/Win11
            "ab3d4648-e242-459f-b02f-541c70306324",
            // Windows 10.0.16299
            "32aa8e18-6496-4e24-9f94-b800e7eccc45"
        };

        private readonly int _ptrSize;
        private readonly nint _factory;
        private readonly nint _vfTable;

        public AudioPolicyConfig()
        {
            this._ptrSize = Marshal.SizeOf<nint>();
            using HSTRING name = HSTRING.FromString("Windows.Media.Internal.AudioPolicyConfig");
            AudioSes.DllGetActivationFactory(name, out this._factory);
            IInspectableSlim iinspectable = (IInspectableSlim)Marshal.GetObjectForIUnknown(this._factory);
            nint count = Marshal.AllocHGlobal(this._ptrSize);
            nint iids = Marshal.AllocHGlobal(this._ptrSize);

            try
            {
                HRESULT result = iinspectable.GetIids(count, ref iids);
                if (result != HRESULT.S_OK)
                {
                    throw new InvalidComObjectException($"Can't get IIDS for {nameof(AudioPolicyConfig)}: {result}");
                }

                int totalGuids = Marshal.ReadInt32(count);
                if (totalGuids <= 0)
                {
                    throw new InvalidComObjectException($"Invalid number of GUIDs found: {totalGuids}");
                }

                nint audioPolicyConfigGuidPtr = iids + GUID_SIZE * (totalGuids - 1);
                string audioPolicyConfigGuid = GuidPtrToString(audioPolicyConfigGuidPtr);
                if (!this._knownGuids.Contains(audioPolicyConfigGuid))
                {
                    throw new InvalidComObjectException($"Unknown AudioPolicyConfig GUID: {audioPolicyConfigGuid}");
                }

                nint vftable = Marshal.PtrToStructure<nint>(this._factory);
                nint queryInterfacePtr = Marshal.PtrToStructure<nint>(vftable);
                QueryInterface queryInterface = Marshal.GetDelegateForFunctionPointer<QueryInterface>(queryInterfacePtr);

                result = queryInterface(this._factory, audioPolicyConfigGuidPtr, ref this._factory);
                if (result != HRESULT.S_OK)
                {
                    throw new InvalidComObjectException($"{nameof(QueryInterface)} for {nameof(AudioPolicyConfig)} failed: {result}");
                }
                this._vfTable = Marshal.PtrToStructure<nint>(this._factory);
            }
            finally
            {
                Marshal.FreeHGlobal(count);
                Marshal.FreeHGlobal(iids);
            }
        }

        private static string GuidPtrToString(nint guidPtr)
        {
            byte[] buffer = new byte[GUID_SIZE];
            Marshal.Copy(guidPtr, buffer, 0, GUID_SIZE);
            GCHandle gchandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                Guid guid = (Guid)(Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(Guid)) ?? throw new Exception($"Unable to convert GUID"));
                return guid.ToString("D");
            }
            finally
            {
                gchandle.Free();
            }
        }

        public void SetPersistedDefaultAudioEndpoint(uint processId, DataFlow flow, Role role, string deviceId)
        {
            string deviceLongId = deviceId is null ? null : $"{MMDEVAPI_TOKEN}{deviceId}{(flow == DataFlow.Render ? DEVINTERFACE_AUDIO_RENDER : DEVINTERFACE_AUDIO_CAPTURE)}";
            using HSTRING deviceIdHString = HSTRING.FromString(deviceLongId);
            nint setPersistedDefaultAudioEndpointPtr = Marshal.PtrToStructure<nint>(this._vfTable + this._ptrSize * 25);
            SetPersistedDefaultAudioEndpointDelegate setPersistedDefaultAudioEndpoint = Marshal.GetDelegateForFunctionPointer<SetPersistedDefaultAudioEndpointDelegate>(setPersistedDefaultAudioEndpointPtr);
            HRESULT result = setPersistedDefaultAudioEndpoint(this._factory, processId, flow, role, deviceIdHString);
            if (result != HRESULT.S_OK && result != HRESULT.PROCESS_NO_AUDIO)
            {
                throw new InvalidComObjectException($"Can't set the persistent audio endpoint: {result}");
            }
        }

        public string GetPersistedDefaultAudioEndpoint(uint processId, DataFlow flow, Role role)
        {
            nint getPersistedDefaultAudioEndpointPtr = Marshal.PtrToStructure<nint>(this._vfTable + this._ptrSize * 26);
            GetPersistedDefaultAudioEndpointDelegate getPersistedDefaultAudioEndpoint = Marshal.GetDelegateForFunctionPointer<GetPersistedDefaultAudioEndpointDelegate>(getPersistedDefaultAudioEndpointPtr);

            HSTRING deviceIdPtr = new HSTRING();
            try
            {
                HRESULT result = getPersistedDefaultAudioEndpoint(this._factory, processId, flow, role, ref deviceIdPtr);
                if (result != HRESULT.S_OK)
                {
                    if (result != HRESULT.PROCESS_NO_AUDIO)
                    {
                        throw new InvalidComObjectException($"Can't get the persisted audio endpoint: {result}");
                    }
                    return null;
                }
                string deviceId = deviceIdPtr.ToString();
                if (!string.IsNullOrEmpty(deviceId))
                {
                    if (deviceId.StartsWith(MMDEVAPI_TOKEN))
                    {
                        deviceId = deviceId.Remove(0, MMDEVAPI_TOKEN.Length);
                    }
                    if (deviceId.EndsWith(DEVINTERFACE_AUDIO_RENDER))
                    {
                        deviceId = deviceId.Remove(deviceId.Length - DEVINTERFACE_AUDIO_RENDER.Length);
                    }
                    if (deviceId.EndsWith(DEVINTERFACE_AUDIO_CAPTURE))
                    {
                        deviceId = deviceId.Remove(deviceId.Length - DEVINTERFACE_AUDIO_CAPTURE.Length);
                    }
                }
                return deviceId;
            }
            finally
            {
                deviceIdPtr.Dispose();
            }
        }

        public void ClearAllPersistedApplicationDefaultEndpoints()
        {
            nint clearAllPersistedDefaultAudioEndpointsPtr = Marshal.PtrToStructure<nint>(this._vfTable + this._ptrSize * 27);
            ClearAllPersistedApplicationDefaultEndpointsDelegate clearAllPersistedDefaultAudioEndpoints = Marshal.GetDelegateForFunctionPointer<ClearAllPersistedApplicationDefaultEndpointsDelegate>(clearAllPersistedDefaultAudioEndpointsPtr);
            HRESULT result = clearAllPersistedDefaultAudioEndpoints(this._factory);
            if (result != HRESULT.S_OK)
            {
                throw new InvalidComObjectException($"Reset audio endpoints: {result}");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Marshal.Release(this._factory);
            Marshal.Release(this._vfTable);
        }

        ~AudioPolicyConfig()
        {
            this.Dispose();
        }
    }
}
