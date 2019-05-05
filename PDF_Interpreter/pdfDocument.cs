using System;
using org.apache.pdfbox.pdmodel;
using System.Collections.Generic;

namespace PDF_Interpreter
{
    class pdfDocument:IDisposable
    {
        PDDocument moDoc = null;
        Dictionary<int, pdfPage> maoPages = new Dictionary<int, pdfPage>();
        private int miPageCount = -1;

        public int PageCount { get => miPageCount; }
        public Dictionary<int, pdfPage> Pages { get => maoPages; }

        public pdfDocument(string sFilename)
        {
            moDoc = PDDocument.load(sFilename);
            miPageCount = moDoc.getNumberOfPages();
           
        }

        public void EvaluateAllPages()
        {
            //optional callback function that can be used to get an event when a page is done being processed.
            Func<pdfPage, bool> examplePageEvalRoutine = delegate (pdfPage oPG)
            {
                //stuff to do with each page as it is processed goes here, like progress or whatever

                return true;
            };

            EvaluateAllPages(examplePageEvalRoutine);

        }

        public void EvaluateAllPages(Func<pdfPage, bool> pageEvalRoutine)
        {
            object[] oPages = moDoc.getDocumentCatalog().getAllPages().toArray();

            miPageCount = oPages.Length;

            int iPageNo = 1; //1's based!!
            foreach (object oPage in oPages)
            {

                //the PDF box object
                PDPage pdPage = (PDPage)oPage;

                //my object from the PDFbox object
                pdfPage oThisPage = new pdfPage(moDoc, pdPage, iPageNo);

                //post inflation event
                pageEvalRoutine(oThisPage);

                //save for later
                maoPages.Add(iPageNo, oThisPage);
                
                iPageNo++;
            }
            
        }

        public void Dispose()
        {
            moDoc.close();
        }

    }
}
