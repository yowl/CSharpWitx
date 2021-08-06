using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CsharpWitx
{
    class Program
    {
        // [DllImport("*")]
        // private static extern int doubles(int i);

        static void Main(string[] args)
        {
            PrintLine("start");

            PrintLine(WitxBindings.Reverse("Rust can reverse dotnet strings"));

            PrintLine("end");

        }

        private static unsafe void PrintString(string s)
        {
            int length = s.Length;
            fixed (char* curChar = s)
            {
                for (int i = 0; i < length; i++)
                {
                    TwoByteStr curCharStr = new TwoByteStr();
                    curCharStr.first = (byte)(*(curChar + i));
                    printf((byte*)&curCharStr, null);
                }
            }
        }

        public static void PrintLine(string s)
        {
            PrintString(s);
            PrintString("\n");
        }

        [DllImport("*")]
        private static unsafe extern int printf(byte* str, byte* unused);
    }

    public struct TwoByteStr
    {
        public byte first;
        public byte second;
    }

    unsafe class WitxBindings
    {
        [DllImport("*")]
        private static extern void reverse(byte* utf8Bytes, int len, IntPtr returnArea);

        internal static string Reverse(string input)
        {
            // witx  - interface type canonical abi, string are done as per this rust
            /*
mod input {
  #[export_name = "double"]
  unsafe extern "C" fn __witx_bindgen_double(arg0: i32, arg1: i32, ) -> i32{
    let len0 = arg1 as usize;
    let result1 = <super::Input as Input>::double(String::from_utf8(Vec::from_raw_parts(arg0 as *mut _, len0, len0)).unwrap());
    let vec2 = (result1.into_bytes()).into_boxed_slice();
    let ptr2 = vec2.as_ptr() as i32;
    let len2 = vec2.len() as i32;
    core::mem::forget(vec2);
    let ptr3 = RET_AREA.as_mut_ptr() as i32;
    *((ptr3 + 8) as *mut i32) = len2;
    *((ptr3 + 0) as *mut i32) = ptr2;
    ptr3
  }
  pub trait Input {
    fn double(i: String,) -> String;
  }
  static mut RET_AREA: [i64; 2] = [0; 2];
}
             */
            var utf8Bytes = Encoding.UTF8.GetBytes(input);

            GCHandle pinnedArray = GCHandle.Alloc(utf8Bytes, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();

            var returnedArea = new byte[16]; // struct {IntPtr ut8Ptr, [offset=8] int len; int padding; } better?
            GCHandle pinnedReturnedArray = GCHandle.Alloc(returnedArea, GCHandleType.Pinned);
            IntPtr pointerReturnArea = pinnedArray.AddrOfPinnedObject();

            reverse((byte*)pointer.ToPointer(), utf8Bytes.Length, pointerReturnArea);
            
            var utf8BytesReturned = *((byte**)pointerReturnArea.ToPointer());

            var lenPtr = new IntPtr(pointerReturnArea.ToInt32() + 8);
            var len = BitConverter.ToInt32(new ReadOnlySpan<byte>((void*)lenPtr, 4));

            pinnedArray.Free();

            var s = new char[len];
            Encoding.UTF8.GetChars(new ReadOnlySpan<byte>(utf8BytesReturned, len), new Span<char>(s));
            pinnedReturnedArray.Free();

            return new string(s);
        }
    }
}
