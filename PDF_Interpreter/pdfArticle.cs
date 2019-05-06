using System.Collections.Generic;

namespace PDF_Interpreter
{
    class pdfArticle
    {
        private const double MC_LINE_HEIGHT_TRIGGER_PCT = 0.8; // percent of char height below or above top of previous char necessary to trigger new line/word
        private const double MC_WORD_END_SAFETY_MARGIN = 0.5; //fixed value added to end of word X when checking if next char is of the word or not.
        private List<pdfCharacter> maoCharacters = new List<pdfCharacter>(); //I don't really need this, but it does not hurt to keep around.
        private List<pdfWord> maoWords = new List<pdfWord>();
        
        public List<pdfCharacter> Characters { get => maoCharacters; }
        public List<pdfWord> Words { get => maoWords; }

        public pdfArticle(object thisPDFboxArticle, bool bCoordsFlipped)
        {
            //THE CHARACTERS within the article
            object[] aoCharacters = (object[])thisPDFboxArticle.GetField("elementData");

            int iLineNo = 1;
            int iWordNo = 1;
            pdfWord oCurrentWord = new pdfWord(bCoordsFlipped);
            pdfWord oPreviousWord = null;
            foreach (object oChar in aoCharacters)
            {
                pdfCharacter oPChar = new pdfCharacter(oChar);
                if (oPChar.IsValid)
                {
                    maoCharacters.Add(oPChar);

                    if (oPChar.Text == " ")
                    {
                        //spaces don't get added to words, they terminate words
                        if (oCurrentWord.IsValid)
                        {
                            maoWords.Add(oCurrentWord);

                            oPreviousWord = oCurrentWord;
                            oCurrentWord = new pdfWord(bCoordsFlipped);

                        }
                    }
                    else //we have a character to put in a word.
                    {
                        //if this character is someplace other than on the end of the word, then quick start a new word
                        // this reads
                        // if character came before start of word
                        // if there is a gap between the end of the word and the character
                        // if the character is well below the word
                        // if the character is well above the word.
                        //  ...then start a new line.
                        if (oCurrentWord.IsValid && (oPChar.X < oCurrentWord.StartX
                                                        || oPChar.X > oCurrentWord.EndX + MC_WORD_END_SAFETY_MARGIN
                                                        || oCurrentWord.StartY + ((oCurrentWord.EndY - oCurrentWord.StartY) * MC_LINE_HEIGHT_TRIGGER_PCT) < oPChar.Y
                                                        || oCurrentWord.StartY > oPChar.Y + (oPChar.MaxTextHeight * MC_LINE_HEIGHT_TRIGGER_PCT)))
                        {
                            maoWords.Add(oCurrentWord);
                            oPreviousWord = oCurrentWord;
                            oCurrentWord = new pdfWord(bCoordsFlipped);
                        }

                        //ON THE FIRST CHARACTER OF A WORD, ASSIGN THE WORD POSITION IN THE ARTICLE 
                        if (oCurrentWord.CharCount == 0)
                        {
                            //if the new word is below OR ABOVE!! the previous word, then start a new line.
                            if (oPreviousWord != null && (oPreviousWord.StartY + ((oPreviousWord.EndY - oPreviousWord.StartY) * MC_LINE_HEIGHT_TRIGGER_PCT) < oPChar.Y
                                                            ||
                                                          oPreviousWord.StartY > oPChar.Y + (oPChar.MaxTextHeight * MC_LINE_HEIGHT_TRIGGER_PCT)))
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

                        //add the character to the correct word
                        oCurrentWord.Push(oPChar);
                    }
                }
            }

            //if a hanging word, add it.
            if(oCurrentWord.IsValid)
                maoWords.Add(oCurrentWord);
            
        }
        
    }
}
