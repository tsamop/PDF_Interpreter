using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.pdmodel.common;
using org.apache.pdfbox.util;
using System.Collections.Generic;
using System.Reflection;

namespace PDF_Interpreter
{
    class pdfPage
    {
        private int miThisPageNo = -1;
        private string msPageText = string.Empty;
        private List<pdfArticle> maoArticles = new List<pdfArticle>();
        private PDPage moThisPage = null;
        private bool mbCoordinatesFlipped = false;


        public int ThisPageNo { get => miThisPageNo; }
        public string PageText { get => msPageText; }
        public List<pdfArticle> Articles { get => maoArticles; }


        public pdfPage(PDDocument oDoc, PDPage oPage, int iPageNo)
        {
            //pick a page.
            miThisPageNo = iPageNo;
            moThisPage = oPage;

            //https://stackoverflow.com/questions/38919551/how-to-find-pdf-is-portrait-or-landscape-using-pdfbox-library-in-java
            PDRectangle mediaBox = oPage.findMediaBox();
            bool isLandscape = mediaBox.getWidth() > mediaBox.getHeight();
            
            // however...the page could be rotated:
            int iRotation = oPage.findRotation();
            if (iRotation == 90 || iRotation == 270)
            {
                isLandscape = !isLandscape;
                mbCoordinatesFlipped = true;
            }
            
            System.Diagnostics.Debug.Print("{0}, {1}, {2}, {3}", iPageNo, mediaBox.getWidth(), mediaBox.getHeight(), iRotation);
            
            //feed the stripper a page.
            PDFTextStripper tStripper = new PDFTextStripper();
            tStripper.setStartPage(iPageNo);
            tStripper.setEndPage(iPageNo);
            msPageText = tStripper.getText(oDoc);

            //This gets the "charactersByArticle" private object in PDF Box.
            FieldInfo charactersByArticleInfo = typeof(PDFTextStripper).GetField("charactersByArticle", BindingFlags.NonPublic | BindingFlags.Instance);
            object charactersByArticle = charactersByArticleInfo.GetValue(tStripper);
            
            object[] aoArticles = (object[])charactersByArticle.GetField("elementData");

            foreach (object oArticle in aoArticles)
            {
                if(oArticle != null)
                    maoArticles.Add(new pdfArticle(oArticle, mbCoordinatesFlipped));
            }
        }
        

    }
}
