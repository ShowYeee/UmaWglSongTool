using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace UmaWglSongTool.Utility
{
    public class OcrHelper
    {
        private TesseractEngine _ocr;

        public OcrHelper()
        {
            _ocr  = new TesseractEngine("./tessdata", "jpn");
        }

        public string GetTextByImage(string fileName)
        {
            var img = Pix.LoadFromFile(fileName);
            Page page = _ocr.Process(img);
            string str = page.GetText();
            page.Dispose();
            return str;
        }
    }
}
