using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace PDF_Interpreter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PdfToJpg(string inputPDFFile, string outputImagesPath)
        {
            //https://stackoverflow.com/questions/11517659/how-to-use-ghostscript-for-converting-pdf-to-image

            string ghostScriptPath = @"C:\Program Files\gs\gs9.27\bin\gswin64.exe";

            // -- %03d is a placeholder for page number.
            // -dFirstPage=12 -dLastPage=12  can be used to print a selection

            if (!outputImagesPath.EndsWith(@"\"))
                outputImagesPath += @"\";


            string ars = "-dNOPAUSE -dSAFER -sDEVICE=jpeg -r150 -dTextAlphaBits=4 -o" + outputImagesPath + "p-%03d.jpg -sPAPERSIZE=a4 -dJPEGQ=30 \"" + inputPDFFile + "\"";
            Process proc = new Process();
            proc.StartInfo.FileName = ghostScriptPath;
            proc.StartInfo.Arguments = ars;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            

        }
        
        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            FileInfo oFile = new FileInfo( txtFile.Text.Trim());

            if (!oFile.Exists)
                throw new Exception("that File does not exist.");
            else if (oFile.Extension != ".pdf")
                throw new Exception("that File is not a pdf.");
            else
            {
                WriteTestHTML(oFile.FullName, oFile.DirectoryName);
            }





        }

        private void WriteTestHTML(string sFile, string sDestDirectory)
        {

            if (!sDestDirectory.EndsWith(@"\"))
                sDestDirectory += @"\";

            PdfToJpg(sFile, sDestDirectory);

            //THIS HOLDS PDFbox and does all the work of recognizing characters/words.
            pdfDocument oDoc = new pdfDocument(sFile);

            foreach(KeyValuePair<int, pdfPage> oKVP in oDoc.Pages)
            {
                int iPageNo = oKVP.Key;
                pdfPage oPage = oKVP.Value;

                string sImageFile = string.Format("{0}p-{1:000}.jpg", sDestDirectory, iPageNo);

                int iImageWidthPx = -1;
                int iImageHeightPx = -1;
                using (Image oPageImage = Image.FromFile(sImageFile))
                {
                    iImageWidthPx = oPageImage.Width;
                    iImageHeightPx = oPageImage.Height;
                }

                StringBuilder oSB = new StringBuilder(@"<html>
                                                            <head>
                                                                <style type=""text/css"">

	                                                            .wordBox{
			                                                            fill:blue;
			                                                            stroke:blue;
			                                                            stroke-width:2px;
			                                                            fill-opacity:0.1;
			                                                            stroke-opacity:0.7;	
                                                                        }
                                                                 
                                                                .wordBox:hover{
			                                                                    fill:pink;
			                                                                    stroke:pink;}
                                                                svg{
                                                                        border:3px solid red;
                                                                        background-color:lightblue;
                                                                    }
                                                            </style>
                                                            </head>
                                                            <body>");

                oSB.AppendFormat(@"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink""  width=""{0}"" height=""{1}"">
                    <image xlink:href=""file:///{2}""  width=""{0}"" height=""{1}""/>", iImageWidthPx, iImageHeightPx, sImageFile.Replace('\\','/'));

                foreach(pdfArticle oArticle in oPage.Articles)
                {
                    foreach(pdfWord oWD in oArticle.Words)
                        oSB.AppendFormat(@"<rect x=""{0}"" y=""{1}"" width=""{2}"" height=""{3}"" class=""wordBox""/>", oWD.Xpct * iImageWidthPx, oWD.Ypct * iImageHeightPx, oWD.WidthPct * iImageWidthPx, oWD.HeightPct * iImageHeightPx);
                }

                oSB.Append(@"</svg>
                            </body>
                            </html>");

                //put the example to file as HTML.
                string sPageAndExt = string.Format("-pp{0:000}.html", iPageNo);
                string sHTMLOutputFile = sFile.ToLower().Replace(".pdf", sPageAndExt);
                File.WriteAllText(sHTMLOutputFile, oSB.ToString());


            }
            


        }
    }
}
