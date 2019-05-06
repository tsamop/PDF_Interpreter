using org.apache.pdfbox.util;
using System;

namespace PDF_Interpreter
{
    class pdfCharacter
    {
        private object maoPDFboxCharacter;

        private string msThisText;
        private double mfX;
        private double mfY;
        private double mfXend;
        private double mfYend;
        private double mfWidth0;
        private double mfMaxTextHeight;
        private double mfPageHeight;
        private double mfPageWidth;
        private int miRotation;

        public string Text { get => msThisText; }
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
                //GET THE GLIPH IN QUESTION.
                msThisText = maoPDFboxCharacter.GetField("str").ToString();

                miRotation = Convert.ToInt32(maoPDFboxCharacter.GetField("rot"));

                //==============================================================================================================
                //==============================================================================================================
                //I don't know where the character GLYPH widths and heights are stored.
                // This whole section is a big hack to try and get some valid width and height
                // information to properly place the character
                Matrix oPos = (Matrix)maoPDFboxCharacter.GetField("textPos");
                float[][] aoPos = oPos.getValues();
                double fWidthFromMatrix = Math.Abs(aoPos[0][1]);
                double fHeightFromMatrix = Math.Abs(aoPos[1][0]);


                //the Y's appear to be the bottom of the char?
                mfMaxTextHeight = fHeightFromMatrix > 0
                                ? fHeightFromMatrix
                                : Math.Abs(Convert.ToDouble(maoPDFboxCharacter.GetField("maxTextHeight")));

                mfX = Convert.ToDouble(maoPDFboxCharacter.GetField("x"));
                mfY = Convert.ToDouble(maoPDFboxCharacter.GetField("y")) - mfMaxTextHeight;

                //CALCULATE THE OTHER SIDE OF THE GLIPH
                mfWidth0 = fWidthFromMatrix > 0 
                         ? fWidthFromMatrix
                         : Math.Abs(((Single[])maoPDFboxCharacter.GetField("widths"))[0]);
                if (mfWidth0 == 0)
                    mfWidth0 = Convert.ToDouble(maoPDFboxCharacter.GetField("endY")) - mfX;//2nd attempt..no really, i think the API has endX and endY revesedd

                //X position is mid character, so scoot over to right corner.
                mfX -= mfWidth0 / 2;
                
                mfXend = mfX + mfWidth0; 

                //CALCULATE THE BOTTOM OF THE GLIPH.
                mfYend = mfY + mfMaxTextHeight;
                //==============================================================================================================
                //==============================================================================================================


                mfPageHeight = Convert.ToDouble(maoPDFboxCharacter.GetField("pageHeight"));
                mfPageWidth = Convert.ToDouble(maoPDFboxCharacter.GetField("pageWidth"));
            }

        }

    }
}
