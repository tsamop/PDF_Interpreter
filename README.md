# PDF_Interpreter
Using PDFBox 1.8.9 and Ghostscript to extract &amp; view Word and Character coords from PDF

The trick is using System.Reflection to expose hidden (private) properties of the PDFbox Page object.

Program creates 1 image for each page of a PDF, computes word locations (if PDF is OCR'ed) then generates SVG into HTML files to display the word locations over an image of the PDF.  The program exposes Pages, Articles within Pages, Words within Articles.  For words, the program notes the line number within the article, and word number within the line.  Word and character coordinates are also exposed.

Example code.  
To use this you need:
1). PDFbox.NET, either the NuGet version (https://www.nuget.org/packages/Pdfbox-IKVM) or just download it(http://www.squarepdf.net/pdfbox-in-net).  You Only need to include refrences to IKVM.OpenJDK.Core, IKVM.OpenJDK.SwingAWT, and pdfbox-1.8.9 to make this work, so the NuGet way is a bit overkill, I just included it to make it easier for the user.

2). GhostScript https://www.ghostscript.com/download/gsdnld.html
I was using the Windows x64 version.
LINE 22 of Form1.cs has a hard-coded path to the GhostScript executable.  You will need to set this. eg. C:\Program Files\gs\gs9.27\bin\gswin64.exe

Took me a whole week, full time, hacking around with all sorts of different API's to finally find one that worked. I really hope this saves someoen else time and is useful. 



