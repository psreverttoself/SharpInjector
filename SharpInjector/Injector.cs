using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpInjector
{
    public class Injector
    {
        private const uint MEM_COMMIT = 0x00001000;
        private const uint MEM_RESERVE = 0x00002000;
        
        private const uint MEM_RELEASE = 0x8000;

        private const uint PAGE_EXECUTE_READWRITE = 0x40;

        private const uint PROCESS_VM_OPERATION = 0x0008;
        private const uint PROCESS_CREATE_THREAD = 0x0002;
        private const uint PROCESS_VM_WRITE = 0x0020;

        [DllImport("kernel32.dll")]
        private static extern IntPtr VirtualAllocEx(IntPtr handle, IntPtr address, uint size, uint allocType, uint protect);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, uint size, out uint bytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateRemoteThread(IntPtr handle, IntPtr threadAttributes, uint stackSize, IntPtr startAddress, IntPtr parameter, uint creationFlags, IntPtr threadId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibraryA(IntPtr name);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr moduleHandle, IntPtr name);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint access, bool inheritHandle, uint pid);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualFreeEx(IntPtr handle, IntPtr address, uint size, uint freeType);

        public bool Inject(string dllPath, int pid)
        {
            bool result = false;

            IntPtr processHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION | PROCESS_VM_WRITE, false, (uint) pid);

            if (processHandle == IntPtr.Zero)
            {
                MessageBox.Show("Failed to open valid process handle!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            byte[] asciiDllPath = Encoding.ASCII.GetBytes((dllPath + "\0"));
            byte[] asciiKernel32DLL = Encoding.ASCII.GetBytes("kernel32.dll\0");
            byte[] asciiLoadLibraryA = Encoding.ASCII.GetBytes("LoadLibraryA\0");
            
            IntPtr dllPathBuffer = Marshal.AllocHGlobal(asciiDllPath.Length);
            IntPtr kernel32Buffer = Marshal.AllocHGlobal(asciiKernel32DLL.Length);
            IntPtr loadLibraryABuffer = Marshal.AllocHGlobal(asciiLoadLibraryA.Length);

            Marshal.Copy(asciiDllPath, 0, dllPathBuffer, asciiDllPath.Length);
            Marshal.Copy(asciiKernel32DLL, 0, kernel32Buffer, asciiKernel32DLL.Length);
            Marshal.Copy(asciiLoadLibraryA, 0, loadLibraryABuffer, asciiLoadLibraryA.Length);

            IntPtr kernel32Base = LoadLibraryA(kernel32Buffer);

            if (kernel32Base != IntPtr.Zero)
            {
                IntPtr loadLibraryAddr = GetProcAddress(kernel32Base, loadLibraryABuffer);

                if (loadLibraryAddr != IntPtr.Zero)
                {
                    IntPtr remoteAddress = VirtualAllocEx(processHandle, IntPtr.Zero, (uint) asciiDllPath.Length, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                    if (remoteAddress != IntPtr.Zero)
                    {
                        uint written;

                        if (WriteProcessMemory(processHandle, remoteAddress, dllPathBuffer, (uint) asciiDllPath.Length, out written))
                        {
                            IntPtr remoteThread = CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddr, remoteAddress, 0, IntPtr.Zero);
                            
                            if (remoteThread != IntPtr.Zero)
                            {
                                CloseHandle(remoteThread);

                                VirtualFreeEx(processHandle, remoteAddress, (uint) asciiDllPath.Length, MEM_RELEASE);

                                result = true;
                            }
                            else
                            {
                                MessageBox.Show("Failed to create remote thread!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to write data to remote process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } else
                    {
                        MessageBox.Show("Failed to allocate memory in remote process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to get kernel32.dll!LoadLibraryA address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Failed to get kernel32.dll base address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CloseHandle(processHandle);

            Marshal.FreeHGlobal(dllPathBuffer);
            Marshal.FreeHGlobal(kernel32Buffer);
            Marshal.FreeHGlobal(loadLibraryABuffer);

            return result;
        }
    }
}