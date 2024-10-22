# WiZ Bulb Control API

This document provides an overview of the methods and parameters available for controlling WiZ smart bulbs via their UDP protocol.

## Table of Contents

- [Overview](#overview)
- [Discovery](#discovery)
- [Control Methods](#control-methods)
  - [1. getSystemConfig](#1-getSystemConfig)
  - [2. setPilot](#2-setPilot)
  - [3. getPilot](#3-getPilot)
  - [4. setSystemConfig](#4-setSystemConfig)
- [Example Usage](#example-usage)
  
## Overview

WiZ bulbs communicate over a custom UDP protocol, allowing users to control various functions such as turning the bulb on/off, adjusting brightness, and changing colors. This document covers the available methods and the parameters you can send in your requests.

## Discovery

To discover available WiZ bulbs on your network, send the following broadcast message:

```json
{
    "method": "getSystemConfig"
}
```

This request retrieves the configuration of the bulb, including its current state and available features.

## Control Methods

### 1. getSystemConfig

- **Method**: `getSystemConfig`
- **Description**: Retrieves the current configuration of the bulb, including its power state and settings.

**Example Request**:
```json
{
    "method": "getSystemConfig"
}
```

### 2. setPilot

- **Method**: `setPilot`
- **Description**: Controls the power state, brightness, and color of the bulb.
  
- **Parameters**:
  - `state` (boolean): Turn the bulb on (`true`) or off (`false`).
  - `dimming` (integer, 0-100): Set the brightness level (0 for off, 100 for full brightness).
  - `r` (integer, 0-255): Red component of the RGB color (optional).
  - `g` (integer, 0-255): Green component of the RGB color (optional).
  - `b` (integer, 0-255): Blue component of the RGB color (optional).
  - `colorTemp` (integer, 0-100): Color temperature (optional, for white light).
  
**Example Request**:
```json
{
    "method": "setPilot",
    "params": {
        "state": true,
        "dimming": 75,
        "r": 255,
        "g": 0,
        "b": 0
    }
}
```

### 3. getPilot

- **Method**: `getPilot`
- **Description**: Retrieves the current state of the bulb (power state, brightness, color).

**Example Request**:
```json
{
    "method": "getPilot"
}
```

### 4. setSystemConfig

- **Method**: `setSystemConfig`
- **Description**: Sets system configurations such as module name and home/room IDs.
  
- **Parameters**:
  - `moduleName` (string): The name of the module (e.g., "WiZ").
  - `homeId` (integer): The ID of the home (optional).
  - `roomId` (integer): The ID of the room (optional).
  - `groupId` (integer): The ID of the group (optional).

**Example Request**:
```json
{
    "method": "setSystemConfig",
    "params": {
        "moduleName": "WiZ",
        "homeId": 123456,
        "roomId": 654321,
        "groupId": 1
    }
}
```

## Example Usage

Here is a simple example of how to discover WiZ bulbs and turn one on using C#:

```csharp
public static async Task DiscoverAndControlWizBulbsAsync()
{
    // Discovery phase
    await DiscoverWizBulbsAsync();

    // Control phase
    string controlMessage = "{\"method\":\"setPilot\",\"params\":{\"state\":true,\"dimming\":100}}";
    byte[] controlBytes = Encoding.UTF8.GetBytes(controlMessage);

    // Send control message to a specific bulb IP address
    // (assuming you have the IP address of the bulb)
    using (var udpClient = new UdpClient())
    {
        await udpClient.SendAsync(controlBytes, controlBytes.Length, new IPEndPoint(IPAddress.Parse("192.168.1.110"), 38899));
        Console.WriteLine("Control message sent to turn on the bulb.");
    }
}
```
