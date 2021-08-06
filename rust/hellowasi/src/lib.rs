
use unicode_segmentation::UnicodeSegmentation;
//use wasm_bindgen::prelude::*;
witx_bindgen_rust::export!("hellowasi.witx");

struct Hellowasi;

// mod w1 {
//     #[export_name = "reverse"]
//     unsafe extern "C" fn __witx_bindgen_reverse(arg0: i32, arg1: i32, ) -> i32{
//       //println!("__witx_bindgen_reverse");
//       let len0 = arg1 as usize;
//       let result1 = <super::W1 as W1>::reverse(String::from_utf8(Vec::from_raw_parts(arg0 as *mut _, len0, len0)).unwrap());
//       let vec2 = (result1.into_bytes()).into_boxed_slice();
//       let ptr2 = vec2.as_ptr() as i32;
//       let len2 = vec2.len() as i32;
//       core::mem::forget(vec2);
//       let ptr3 = RET_AREA.as_mut_ptr() as i32;
//       *((ptr3 + 8) as *mut i32) = len2;
//       *((ptr3 + 0) as *mut i32) = ptr2;
//       ptr3
//     }
//     pub trait W1 {
//       fn reverse(s: String,) -> String;
//     }
//     static mut RET_AREA: [i64; 2] = [0; 2];
//   }
  
impl hellowasi::Hellowasi for Hellowasi {

    fn reverse(s : String) -> String {
        let drow: String = s
            // Copied from SO:
            // Split the string into an Iterator of &strs, where each element is an
            // extended grapheme cluster.
            .graphemes(true)
            // Reverse the order of the grapheme iterator.
            .rev()
            // Collect all the chars into a new owned String.
            .collect();
        
            return drow;
    }
}





