using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpInjector
{
    public class Injector
    {
        [Flags]
        private enum MemProtection
        {
            PageExecuteReadWrite = 0x040
        }
        
        [Flags]
        private enum MemoryAllocation
        {
            Commit = 0x01000,
            Reserve = 0x02000,
            Release = 0x08000
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr VirtualAllocEx(IntPtr handle, IntPtr address, int size, MemoryAllocation allocType, MemProtection protect);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, byte[] buffer, int size, int bytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateRemoteThread(IntPtr handle, IntPtr threadAttributes, uint stackSize, IntPtr startAddress, IntPtr parameter, uint creationFlags, IntPtr threadId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr moduleHandle, string name);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualFreeEx(IntPtr handle, IntPtr address, int size, MemoryAllocation freeType);

        public bool Inject(string dllPath, int pid)
        {
            bool result = false;

            // Get a handle to the process
            
            var processHandle = Process.GetProcessById(pid).Handle;
            
            if (processHandle == IntPtr.Zero)
            {
                MessageBox.Show("Failed to open valid process handle!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            // Get the address of load library
            
            var loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryW");

            if (loadLibraryAddr == IntPtr.Zero)
            {
                MessageBox.Show("Failed to get load library address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            
            // Allocate memory for the dll path in the process
            
            var remoteAddress = VirtualAllocEx(processHandle, IntPtr.Zero, dllPath.Length, MemoryAllocation.Commit | MemoryAllocation.Reserve, MemProtection.PageExecuteReadWrite);

            if (remoteAddress == IntPtr.Zero)
            {
                MessageBox.Show("Failed to allocate memory for the dll!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            
            // Write the dll path into the process

            var dllPathBuffer = Encoding.Unicode.GetBytes(dllPath + "\0");
            
            if(!WriteProcessMemory(processHandle, remoteAddress, dllPathBuffer, dllPathBuffer.Length, 0))
            {
                MessageBox.Show("Failed to write memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            
            // Create a remote thread to call the dll
            
            var remoteThread = CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddr, remoteAddress, 0, IntPtr.Zero);

            if (remoteThread == IntPtr.Zero)
            {
                MessageBox.Show("Failed to create remote thread!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            // Close the handle
            
            CloseHandle(remoteThread);

            // Free the memory
            
            VirtualFreeEx(processHandle, remoteAddress, dllPath.Length, MemoryAllocation.Release);
            
            result = true;

            return result;
        }
    }
}