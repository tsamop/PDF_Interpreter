using System.Collections.Generic;

namespace PDF_Interpreter
{
    class pdfArticle
    {
        private const double MC_LINE_HEIGHT_TRIGGER_PCT = 0.8;
        private List<pdfCharacter> maoCharacters = new List<pdfCharacter>(); //I don't really need this, but it does not hurt to keep around.
        private List<pdfWord> maoWords = new List<pdfWord>();
        
        public List<pdfCharacter> Characters { get => maoCharacters; }
        public List<pdfWord> Words { get => maoWords; }

        public pdfArticle(object thisPDFboxArticle)
        {
            //THE CHARACTERS within the article
            object[] aoCharacters = (object[])thisPDFboxArticle.GetField("elementData");

            int iLineNo = 1;
            int iWordNo = 1;
            pdfWord oCurrentWord = new pdfWord();
            pdfWord oPreviousWord = null;
            foreach (object oChar in aoCharacters)
            {
                pdfCharacter oPChar = new pdfCharacter(oChar);
                maoCharacters.Add(oPChar);

                if (oPChar.Val == ' ')
                {
                    //spaces don't get added to words, they terminate words
                    if (oCurrentWord.CharCount > 0)
                    {
                        maoWords.Add(oCurrentWord);

                        oPreviousWord = oCurrentWord;
                        oCurrentWord = new pdfWord();

                    }
                }
                else
                {
                    //someone snuck in a linefeed!!
                    if (oPChar.X < oCurrentWord.StartX && oCurrentWord.CharCount > 0)
                    {
                        maoWords.Add(oCurrentWord);
                        oPreviousWord = oCurrentWord;
                        oCurrentWord = new pdfWord();
                    }

                    //ON THE FIRST CHARACTER OF A WORD, ASSIGN THE WORD POSITION IN THE ARTICLE 
                    if(oCurrentWord.CharCount == 0)
                    {
                        //if the new word is below the previous word, then start a new line.
                        if (oPreviousWord != null && oPreviousWord.StartY + (oPreviousWord.EndY * MC_LINE_HEIGHT_TRIGGER_PCT) < oPChar.Y)
                        {
                            iLineNo++;
                            iWordNo = 1;
                        }
                        else
                        {
                            iWordNo++;
                        }
                        
                        oCurrentWord.SetWordPosition(iLineNo, iWordNo);
                    }

                    //add the chearcter to the correct word
                    oCurrentWord.Push(oPChar);
                }
            }

            //if a hanging word, add it.
            if(oCurrentWord.CharCount > 0)
                maoWords.Add(oCurrentWord);
            
        }
        
    }
}
