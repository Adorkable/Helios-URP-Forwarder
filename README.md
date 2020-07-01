# Helios URP Forwarder

Scriptable Renderer that forwards render data to [Helios](https://assetstore.unity.com/packages/tools/camera/helios-63643), the 360 video capturer for Unity

1. Make `Helios::OnRenderImage` in **Helios.cs** `public`

    ![Code change](./Tutorial/Step1.png)

1. Add **Helios Scriptable Renderer Forwarder** as **Renderer Feature** to Scriptable Pipeline

    ![Add Forwarder](./Tutorial/Step2.png)

1. Give **Forwarder** a handy name you'll remember
  
    ![Give it a name](./Tutorial/Step3.png)

1. ⚡️
