using System;

namespace PDF_Interpreter
{
    internal class pdfWord
    {
        private int miCharCount = 0;
        private int miLineNo = -1;
        private int miWordNo = -1;
        private string msVal = string.Empty;
        private double mfStartX = -1;
        private double mfStartY = -1;
        private double mfEndX = -1;
        private double mfEndY = -1;
        private double mfPageTotalX = -1;
        private double mfPageTotalY = -1;

        public int CharCount { get => miCharCount; }
        public int LineNo { get => miLineNo; } //line within article.  allows phrases to be found.
        public int WordNo { get => miWordNo; } //word no, within Line, within article.  allows phrases to be found.
        public string Val { get => msVal; }
        public double StartX { get => mfStartX; }
        public double StartY { get => mfStartY; }
        public double EndX { get => mfEndX; }
        public double EndY { get => mfEndY; }
        public double PageTotalX { get => mfPageTotalX; }
        public double PageTotalY { get => mfPageTotalY; }

        //better coordinates for drawing rectangles.
        public double Xpct { get => mfStartX/ mfPageTotalX; }
        public double WidthPct { get => (EndX - mfStartX) / mfPageTotalX; }
        public double Ypct { get => mfStartY / mfPageTotalY; }
        public double HeightPct { get => (EndY - mfStartY) / mfPageTotalY; }
        
        public void SetWordPosition(int iLineNo, int iWordNo)
        {
            miLineNo = iLineNo;
            miWordNo = iWordNo;
        }

        public void Push(pdfCharacter oChar)
        {
            if (miCharCount == 0)
            {
                mfStartX = oChar.X;
                mfStartY = oChar.Y;
            }
            //always, because we may have a 1-character word.
            mfEndX = oChar.Xend;
            mfEndY = oChar.Yend;

            //Pull the total page size into the word aas well and validate.
            if (mfPageTotalX < 0)
                mfPageTotalX = oChar.PageWidth;
            else if (mfPageTotalX != oChar.PageWidth)
                throw new Exception("Page width varied within a word!");
            
            if (mfPageTotalY < 0)
                mfPageTotalY = oChar.PageHeight;
            else if (mfPageTotalY != oChar.PageHeight)
                throw new Exception("Page height varied within a word!");
            
            //form the word.
            msVal += oChar.Val;
            
            miCharCount++;

        }

    }
}