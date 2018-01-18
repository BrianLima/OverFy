using System;
using System.Runtime.InteropServices;

namespace OverFy
{
    public static class RivaTuner
    {
        [DllImport("kernel32")]
        private unsafe static extern void* LoadLibrary(string dllname);
        [DllImport("kernel32")]
        private unsafe static extern void FreeLibrary(void* handle);

        private sealed unsafe class LibraryUnloader
        {
            internal LibraryUnloader(void* handle)
            {
                this.handle = handle;
            }

            ~LibraryUnloader()
            {
                if (handle != null)
                    FreeLibrary(handle);
            }

            private void* handle;

        } // LibraryUnloader

        private static readonly LibraryUnloader unloader;

        static RivaTuner()
        {
            string path;

            if (IntPtr.Size == 4)
                path = "x86/rivatuner.dll";
            else
                path = "x64/rivatuner.dll";

            unsafe
            {
                void* handle = LoadLibrary(path);

                if (handle == null)
                    throw new DllNotFoundException("Unable to find the native rivatuner library: " + path);

                unloader = new LibraryUnloader(handle);
            }
        }

        [DllImport("rivatuner", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool print(string text);

    }
}
