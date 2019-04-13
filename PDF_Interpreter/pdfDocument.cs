using org.apache.pdfbox.pdmodel;
using System.Collections.Generic;

namespace PDF_Interpreter
{
    class pdfDocument
    {
        Dictionary<int, pdfPage> maoPages = new Dictionary<int, pdfPage>();


        public Dictionary<int, pdfPage> Pages { get => maoPages; }

        public pdfDocument(string sFilename)
        {
            
            PDDocument oDoc = PDDocument.load(sFilename);
            object[] oPages = oDoc.getDocumentCatalog().getAllPages().toArray();

            int iPageNo = 1; //1's based!!
            foreach (object oPage in oPages)
            {
                PDPage pdPage = (PDPage)oPage;
                
                maoPages.Add(iPageNo, new pdfPage(oDoc, iPageNo));


                iPageNo++;
            }
            
            oDoc.close();
        }

    }
}
