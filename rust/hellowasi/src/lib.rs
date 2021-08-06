use unicode_segmentation::UnicodeSegmentation;
witx_bindgen_rust::export!("hellowasi.witx");

struct Hellowasi;
  
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