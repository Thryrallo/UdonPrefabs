using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Thry.Udon
{
    public static class Patcher
    {
        static MethodInfo _src;

        [InitializeOnLoadMethod]
        static void Patch()
        {
            // find this method: internal static void LogWarning(this object obj, string message)
            Type type = FindTypeByFullName("VRC.SDK3.ClientSim.ClientSimExtensions");
            Debug.Log(type.Name);
            _src = type.GetMethod("LogWarning", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(object), typeof(string) }, null);
            Debug.Log(_src.Name);
            MethodInfo dst = typeof(Patcher).GetMethod("PatchedLogWarning", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(object), typeof(string) }, null);
            Debug.Log($"Patching {nameof(Patcher)}.{nameof(PatchedLogWarning)} to {type.FullName}.{_src.Name}");
            TryDetourFromTo(_src, dst);
        }

        internal static void PatchedLogWarning(object obj, string message)
        {
            if(message.StartsWith("Destroying uninitialized Helper", StringComparison.OrdinalIgnoreCase))
            {
                // This is a warning that we don't want to see in the console.
                return;
            }
            _src.Invoke(obj, new object[] { obj, message });
        }

        public static Type FindTypeByFullName(string fullname)
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where type.FullName == fullname
                    select type).FirstOrDefault();
        }

        // Start of Detour methods
        // Modified from: https://github.com/apkd/UnityStaticBatchingSortingPatch/blob/e83bed8cf31fc98097586c4e47af77fa79d9bed5/StaticBatchingSortingPatch.cs
        // Modified by Behemoth/hill
        static Dictionary<MethodInfo, byte[]> s_patchedData = new Dictionary<MethodInfo, byte[]>();
        public static unsafe void TryDetourFromTo(MethodInfo src, MethodInfo dst)
        {
#if UNITY_EDITOR_WIN
            try
            {
                if (IntPtr.Size == sizeof(Int64))
                {
                    // 64-bit systems use 64-bit absolute address and jumps
                    // 12 byte destructive

                    // Get function pointers
                    long Source_Base = src.MethodHandle.GetFunctionPointer().ToInt64();
                    long Destination_Base = dst.MethodHandle.GetFunctionPointer().ToInt64();

                    // Backup Source Data
                    IntPtr Source_IntPtr = src.MethodHandle.GetFunctionPointer();
                    var backup = new byte[0xC];
                    Marshal.Copy(Source_IntPtr, backup, 0, 0xC);
                    s_patchedData.Add(src, backup);

                    // Native source address
                    byte* Pointer_Raw_Source = (byte*)Source_Base;

                    // Pointer to insert jump address into native code
                    long* Pointer_Raw_Address = (long*)(Pointer_Raw_Source + 0x02);

                    // Insert 64-bit absolute jump into native code (address in rax)
                    // mov rax, immediate64
                    // jmp [rax]
                    *(Pointer_Raw_Source + 0x00) = 0x48;
                    *(Pointer_Raw_Source + 0x01) = 0xB8;
                    *Pointer_Raw_Address = Destination_Base; // ( Pointer_Raw_Source + 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 )
                    *(Pointer_Raw_Source + 0x0A) = 0xFF;
                    *(Pointer_Raw_Source + 0x0B) = 0xE0;
                }
                else
                {
                    // 32-bit systems use 32-bit relative offset and jump
                    // 5 byte destructive

                    // Get function pointers
                    int Source_Base = src.MethodHandle.GetFunctionPointer().ToInt32();
                    int Destination_Base = dst.MethodHandle.GetFunctionPointer().ToInt32();

                    // Backup Source Data
                    IntPtr Source_IntPtr = src.MethodHandle.GetFunctionPointer();
                    var backup = new byte[0x5];
                    Marshal.Copy(Source_IntPtr, backup, 0, 0x5);
                    s_patchedData.Add(src, backup);

                    // Native source address
                    byte* Pointer_Raw_Source = (byte*)Source_Base;

                    // Pointer to insert jump address into native code
                    int* Pointer_Raw_Address = (int*)(Pointer_Raw_Source + 1);

                    // Jump offset (less instruction size)
                    int offset = (Destination_Base - Source_Base) - 5;

                    // Insert 32-bit relative jump into native code
                    *Pointer_Raw_Source = 0xE9;
                    *Pointer_Raw_Address = offset;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to detour: {src?.Name ?? "UnknownSrc"} -> {dst?.Name ?? "UnknownDst"}\n{ex}");
                throw;
            }
#endif
        }
    }
}