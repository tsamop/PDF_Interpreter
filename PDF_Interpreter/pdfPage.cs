using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System.Collections.Generic;
using System.Reflection;

namespace PDF_Interpreter
{
    class pdfPage
    {
        private string msPageText = string.Empty;
        private List<pdfArticle> maoArticles = new List<pdfArticle>();


        public string PageText { get => msPageText; }
        public List<pdfArticle> Articles { get => maoArticles; }

        public pdfPage(PDDocument oDoc, int iPageNo)
        {

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
                    maoArticles.Add(new pdfArticle(oArticle));
            }
        }
        

    }
}
