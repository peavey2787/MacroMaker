using System;
using System.Collections.Generic;
using NAudio.CoreAudioApi;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace MacroMaker.Classes
{    public class AudioDeviceManager
    {
        private NotificationManager notificationManager;
        public AudioDeviceManager()
        {
            notificationManager = new NotificationManager();
        }
        public List<MMDevice> GetAudioPlaybackDevices()
        {
            List<MMDevice> playbackDevices = new List<MMDevice>();

            try
            {
                using (var enumerator = new MMDeviceEnumerator())
                {
                    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

                    foreach (var device in devices)
                    {
                        playbackDevices.Add(device);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return playbackDevices;
        }
        public MMDevice GetDefaultAudioPlaybackDevice()
        {
            try
            {
                using (var enumerator = new MMDeviceEnumerator())
                {
                    return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public MMDevice GetDefaultAudioRecordingDevice()
        {
            try
            {
                using (var enumerator = new MMDeviceEnumerator())
                {
                    return enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<MMDevice> GetAudioRecordingDevices()
        {
            List<MMDevice> recordingDevices = new List<MMDevice>();

            try
            {
                using (var enumerator = new MMDeviceEnumerator())
                {
                    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

                    foreach (var device in devices)
                    {
                        recordingDevices.Add(device);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }

            return recordingDevices;
        }


        /// <summary>
        /// If successMessage is found in the output it returns true; If errorMessage is found in the error-output it returns false; Waiting for input will press 'Y' after short delay
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        /// <param name="successMessage"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool RunPowerShellCommand(string command, string arguments, string successMessage, string errorMessage, bool waitForInput = false)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"{command} {arguments}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                bool hasSucessMessage = string.IsNullOrWhiteSpace(successMessage) ? true : false;
                bool hasErrorMessage = false;

                process.OutputDataReceived += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.Data)) return;

                    if (e.Data.Contains(successMessage))
                    {
                        hasSucessMessage = true;
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(e.Data)) return;

                    if (e.Data.Contains(errorMessage))
                    {
                        hasErrorMessage = true;
                    }
                };

                process.Start();

                // Begin asynchronous reading of output and error streams
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to exit
                process.WaitForExit();

                if (waitForInput)
                {
                    System.Threading.Thread.Sleep(3500);
                    Keyboard.Type("Y \r\n");

                    hasSucessMessage = true;
                    hasErrorMessage = false;
                }

                if (process.ExitCode == 0 && hasSucessMessage && !hasErrorMessage)
                {
                    // PowerShell command succeeded
                    return true;
                }

                // PowerShell command failed
                return false;
            }
        }


        public bool SetDefaultAudioPlaybackDevice(MMDevice device)
        {
            string command = "Set-AudioDevice";
            string arguments = $"-ID '{device.ID}'";
            string successMessage = "";
            string errorMessage = "is not recognized";

            if (RunPowerShellCommand(command, arguments, successMessage, errorMessage))
            {
                notificationManager.ShowNotification($"Default audio device switched to {device.FriendlyName}");
                return true;
            }

            // Handle additional cases specific to SetDefaultAudioPlaybackDevice
            if (InstallAudioDeviceCmdlets())
            {
                // Retry the Set-AudioDevice command after installing the module
                return SetDefaultAudioPlaybackDevice(device);
            }
            else
            {
                notificationManager.ShowNotification("Please run app as admin to install missing package: AudioDeviceCmdlets");
            }

            return false;
        }
        public bool SetDefaultAudioRecordingDevice(MMDevice device)
        {
            string command = "Set-AudioDevice";
            string arguments = $"-ID '{device.ID}' -Role Capture";
            string successMessage = "";
            string errorMessage = "is not recognized";

            if (RunPowerShellCommand(command, arguments, successMessage, errorMessage))
            {
                notificationManager.ShowNotification($"Default recording device switched to {device.FriendlyName}");
                return true;
            }

            // Handle additional cases specific to SetDefaultAudioRecordingDevice
            if (InstallAudioDeviceCmdlets())
            {
                // Retry the Set-AudioDevice command after installing the module
                return SetDefaultAudioRecordingDevice(device);
            }
            else
            {
                notificationManager.ShowNotification("Please run app as admin to install missing package: AudioDeviceCmdlets");
            }

            return false;
        }


        private bool InstallAudioDeviceCmdlets()
        {
            string command = "Install-Module";
            string arguments = "-Name AudioDeviceCmdlets -Force -Confirm:$false -Scope AllUsers";
            string successMessage = "AudioDeviceCmdlets installed";
            string errorMessage = "administrator rights are required";

            return RunPowerShellCommand(command, arguments, successMessage, errorMessage, true);
        }

    }
}
