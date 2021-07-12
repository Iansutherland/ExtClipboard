using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtClipboardWPF.Services
{
    //credit to https://gist.github.com/Ciantic/471698
    class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private Action<char, int> innerCallback;

        public InterceptKeys(Action<char, int> action)
        {
            //accept an action<int> to call inside the HookCallback
            this.innerCallback = action;
            this._proc = this.HookCallback;
        }

        public void StartHook()
        {
            _hookID = SetHook(_proc);
        }

        public void EndHook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        //Delegate called when registered with "SetWindowsHookEx"
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                //get the keyCode
                int vkCode = Marshal.ReadInt32(lParam);
                uint uint_vkCode = (uint)vkCode;
                //get the character string
                int nonVirtualKey = MapVirtualKey(uint_vkCode, 2);
                //TODO: IT DOESN'T GET CHARACTERS FOR CTRL BECAUSE THAT DOESN'T HAVE A CHAR DUH, YOU GOTTA CHECK THE VKCODE FOR THAT I GUESS 
                char mappedChar = Convert.ToChar(nonVirtualKey);

                //Handle special case where CTRL is hit in combination
                //if (this.comboKeyList.Contains(vkKey))
                //{

                //}

                //call the innercallback given by ctor
                 this.innerCallback(mappedChar, vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(uint uCode, uint uMapType);
    }
}