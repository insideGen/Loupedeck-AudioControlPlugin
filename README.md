# Loupedeck plugin to control Windows audio

This plugin allows you to control the volume of render and capture devices and display peak meter on each. As well as audio sessions associated with render devices (audio stream between applications and audio endpoint devices).

![Peak meter example](./media/peak-meter-example.gif "Peak meter example")

It also allows you to set default communication and multimedia devices and input/output device for each application.

## Installation

- Download the [latest release](https://github.com/insideGen/Loupedeck-AudioControl-OpenPlugin/releases/latest) of the `.lplug4` asset;
- Double-click on the file and follow the instructions in the Loupedeck application.

Only available for Windows, tested with Loupedeck Live S.

## Actions

### Audio touch action [Command]

Displays volume and peak meter and allows volume control.

![Unmuted capture icon](./media/capture-unmuted.png "Unmuted capture icon")
![Muted capture icon](./media/capture-muted.png "Muted capture icon")
![Unmuted render icon](./media/render-unmuted.png "Unmuted render icon")
![Muted render icon](./media/render-muted.png "Muted render icon")
![Unmuted application icon](./media/application-unmuted.png "Unmuted application icon")
![Muted application icon](./media/application-muted.png "Muted application icon")

**`Configuration`**

| Parameter                    | Type    | Value                                          | Description                                                                                                                                                                                  |
| ---------------------------- | ------- | ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Channel                      | Listbox | None, A, B, C                                  | Channel allows to bind this `Audio touch action` to an `Audio encoder action`.                                                                                                               |
| Type                         | Listbox | Capture, Render, Application                   | Capture and Render: lists all active devices. Application: lists all running apps currently emitting sound.                                                                                  |
| Endpoint                     | Listbox | *                                              | Endpoint name. **Default communications** and **Default multimedia** are dynamic endpoints for Capture and Render type. **Foreground application** is dynamic endpoint for Application type. |
| Toggle default endpoint mode | Listbox | None, Communication, Multimedia, Both          | The default mode to apply to the endpoint.                                                                                                                                                   |
| Toggle default endpoint 1    | Listbox | *                                              | Default endpoint 1 name.                                                                                                                                                                     |
| Toggle default endpoint 2    | Listbox | *                                              | Default endpoint 2 name.                                                                                                                                                                     |
| Long-press action            | Listbox | None, Mute/Unmute all, Toggle default endpoint | Mute/Unmute all endpoint of this type (Capture, Render). Toggle the default one between endpoint 1 and endpoint 2.                                                                           |

**`Touch screen`**

| Gesture          | Description                                                                                                               |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------- |
| Single tap       | Select the endpoint for the channel defined in the configuration if Channel is not set to None, otherwise nothing.        |
| Double tap       | Mute/Unmute endpoint defined in the configuration.                                                                        |
| Long press       | Execute action defined in the configuration.                                                                              |
| Horizontal swipe | If toggle default endpoint mode is Communication or Multimedia or Both, toggles endpoint 1 / endpoint 2.                  |
| Vertical swipe   | Increases or decreases the volume by 10.                                                                                  |

### Audio encoder action [Adjustment with reset]

Displays volume and peak meter and allows volume control.

**`Configuration`**

| Parameter                    | Type    | Value                                          | Description                                                                                                                                                                                  |
| ---------------------------- | ------- | ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Channel                      | Listbox | None, A, B, C                                  | Channel allows to bind this `Audio encoder action` to multiple `Audio touch action`.                                                                                                         |
| Type                         | Listbox | Capture, Render, Application                   | Capture and Render: lists all active devices. Application: lists all running apps currently emitting sound.                                                                                  |
| Endpoint                     | Listbox | *                                              | Endpoint name. **Default communications** and **Default multimedia** are dynamic endpoints for Capture and Render type. **Foreground application** is dynamic endpoint for Application type. |
| Toggle default endpoint mode | Listbox | None, Communication, Multimedia, Both          | The default mode to apply to the endpoint.                                                                                                                                                   |
| Toggle default endpoint 1    | Listbox | *                                              | Default endpoint 1 name.                                                                                                                                                                     |
| Toggle default endpoint 2    | Listbox | *                                              | Default endpoint 2 name.                                                                                                                                                                     |
| Long-press action            | Listbox | None, Mute/Unmute all, Toggle default endpoint | Mute/Unmute all endpoint of this type (Capture, Render). Toggle the default one between endpoint 1 and endpoint 2.                                                                           |

**`Dial`**

| Gesture    | Description                                                                                                               |
| ---------- | ------------------------------------------------------------------------------------------------------------------------- |
| Rotate     | Adjust volume of endpoint selected in channel if Channel is not set to None, otherwise endpoint defined in configuration. |
| Push       | Mute/unmute endpoint selected in channel if Channel is not set to None, otherwise endpoint defined in configuration.      |

**`Touch screen` of reset action**

| Gesture          | Description                                                                                                               |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------- |
| Single tap       | Select the endpoint for the channel defined in the configuration if Channel is not set to None, otherwise nothing.        |
| Double tap       | Mute/Unmute endpoint defined in the configuration.                                                                        |
| Long press       | Execute action defined in the configuration.                                                                              |
| Horizontal swipe | If toggle default endpoint mode is Communication or Multimedia or Both, toggles endpoint 1 / endpoint 2.                  |
| Vertical swipe   | Increases or decreases the volume by 10.                                                                                  |

### Mute/unmute all capture devices [Command]

Allows to mute/unmute all capture devices.

![Mute/unmute all capture devices icon](./media/un-mute-all-capture.png "Mute/unmute all capture devices icon")

**`Touch screen`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Single tap | Mute all capture devices.            |
| Long press | Unmute all capture devices.          |

### Mute/unmute all render devices [Command]

Allows to mute/unmute all render devices.

![Mute/unmute all render devices icon](./media/un-mute-all-render.png "Mute/unmute all render devices icon")

**`Touch screen`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Single tap | Mute all render devices.             |
| Long press | Unmute all render devices.           |

### All-in-one [Folder]

Gathers all the folders.

![All-in-one folder icon](./media/folder-all-in-one.png "All-in-one folder icon")

#### Sub-folders

![Capture page icon](./media/page-capture.png "Capture page icon")
![Render page icon](./media/page-render.png "Render page icon")
![Applications page icon](./media/page-applications.png "Applications page icon")
![Multimedia page icon](./media/page-multimedia.png "Multimedia page icon")
![Communication page icon](./media/page-communication.png "Communication page icon")
![Settings page icon](./media/page-settings.png "Settings page icon")

- Capture devices
- Render devices
- Applications
- Multimedia (Multimedia capture device + Multimedia render device + Applications)
- Communication (Communication capture device + Communication render device + Applications)
- Settings

### Capture devices [Folder]

![Capture folder icon](./media/folder-capture.png "Capture folder icon")

#### Capture devices page

Displays all active capture devices and their peak meters (display peak meter only when an audio stream exists).

**`Touch screen`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Single tap | Select the device.                   |
| Double tap | Mute/unmute device.                  |
| Swipe up   | Set device as default communication. |
| Swipe down | Set device as default multimedia.    |

**`Dial`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Rotate     | Adjust selected device volume.       |
| Push       | Mute/unmute selected device.         |

### Render devices [Folder]

![Render folder icon](./media/folder-render.png "Render folder icon")

#### Render devices page

Displays all active render devices and their peak meters.

**`Touch screen`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Single tap | Select the device.                   |
| Double tap | Mute/unmute device.                  |
| Swipe up   | Set device as default communication. |
| Swipe down | Set device as default multimedia.    |
| Long press | Open render applications page.       |

**`Dial`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Rotate     | Adjust selected device volume.       |
| Push       | Mute/unmute selected device.         |

#### Render applications page

Displays all active applications of a specific render device and their peak meters.

**`Touch screen`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Single tap | Select the application.              |
| Double tap | Mute/unmute application.             |
| Long press | Open input/output device page.       |

**`Dial`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Rotate     | Adjust selected application volume.  |
| Push       | Mute/unmute selected application.    |

#### Input/output device page

Configure input/output device for application.

### Applications [Folder]

![Applications folder icon](./media/folder-applications.png "Applications folder icon")

#### Applications page

Displays all active applications and their peak meters.

**`Touch screen`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Single tap | Select the application.              |
| Double tap | Mute/unmute application.             |
| Long press | Open input/output device page.       |

**`Dial`**

| Gesture    | Description                          |
| ---------- | ------------------------------------ |
| Rotate     | Adjust selected application volume.  |
| Push       | Mute/unmute selected application.    |

#### Input/output device page

Configure input/output device for application.

### Settings [Folder]

![Settings folder icon](./media/folder-settings.png "Settings folder icon")

#### Settings page

| Setting           | Value               | Description                                                                          |
| ----------------- | ------------------- | ------------------------------------------------------------------------------------ |
| FPS               | 10, **20**, 30      | Select refresh rate (impacts the smoothness of the peak meter and processor usage).  |
| Peak meter        | **Yes**, No         | Enable/disable peek meter.                                                           |
| Device icon style | **Windows**, Custom | Switch between Windows icon style and custom icons.                                  |
| Folder decoration | Yes, **No**         | Enable/disable folder icon decoration (a restart is required to apply this setting). |
| Blue light filter | **Yes**, No         | Enable/disable blue light filter (a restart is required to apply this setting).      |

*Default values are highlighted in bold.*
