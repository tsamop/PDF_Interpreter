using System;

namespace PDF_Interpreter
{
    class pdfCharacter
    {
        private object maoPDFboxCharacter;

        private char mcThisChar;
        private double mfX;
        private double mfY;
        private double mfXend;
        private double mfYend;
        private double mfWidth0;
        private double mfMaxTextHeight;
        private double mfPageHeight;
        private double mfPageWidth;

        public char Val { get => mcThisChar; }
        public double X { get => mfX; }
        public double Y { get => mfY; }
        public double Xend { get => mfXend; }
        public double Yend { get => mfYend; }
        public double Width { get => mfWidth0; }
        public double MaxTextHeight { get => mfMaxTextHeight; }
        public double PageHeight { get => mfPageHeight; }
        public double PageWidth { get => mfPageWidth; }
        

        public pdfCharacter(object thisPDFboxCharacter)
        {

            /*properties I caulght using reflection:
             * endX, endY, font, fontSize, fontSizePt, maxTextHeight, pageHeight, pageWidth, rot, str textPos, unicodCP, widthOfSpace, widths, wordSpacing, x, y
             * 
             */

            maoPDFboxCharacter = thisPDFboxCharacter;
            if (maoPDFboxCharacter != null)
            {
                //the Y's appear to be the bottom of the char?
                mfMaxTextHeight = Convert.ToDouble(maoPDFboxCharacter.GetField("maxTextHeight"));

                mcThisChar = maoPDFboxCharacter.GetField("str").ToString().ToCharArray()[0];
                mfX = Convert.ToDouble(maoPDFboxCharacter.GetField("x"));
                mfY = Convert.ToDouble(maoPDFboxCharacter.GetField("y")) - mfMaxTextHeight;

                //CALCULATE THE OTHER SIDE OF THE GLIPH
                mfWidth0 = ((Single[])maoPDFboxCharacter.GetField("widths"))[0];
                mfXend = mfX + mfWidth0; // Convert.ToDouble(maoPDFboxCharacter.GetField("endX"));
                
                //CALCULATE THE BOTTOM OF THE GLIPH.
                mfYend = mfY + mfMaxTextHeight; //  Convert.ToDouble(maoPDFboxCharacter.GetField("endY"));
                
                mfPageHeight = Convert.ToDouble(maoPDFboxCharacter.GetField("pageHeight"));
                mfPageWidth = Convert.ToDouble(maoPDFboxCharacter.GetField("pageWidth"));

             }

        }

    }
}
