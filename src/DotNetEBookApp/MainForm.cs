﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Reflection;
using System.Resources;
using EPubBook = Bzway.DotNetBook.ePub.EBook;
using NavPoint = Bzway.DotNetBook.ePub.NavPoint;

namespace Bzway.DotNetBook.ePub.Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            var epub = new EPubBook();

            epub.AddAuthor(textboxAuthor.Text);
            epub.AddTitle(textboxTitle.Text);
            epub.AddLanguage("en");

            String css = Bzway.DotNetBook.ePub.Test.Properties.Resources.style;

            if (checkBoxFonts.Checked)
            {
                css += "\nbody { font-family: LiberationSerif; }\n";
                css += "@font-face { font-family : LiberationSerif; font-weight : normal; font-style: normal; src : url(LiberationSerif-Regular.ttf); }\n";
                css += "@font-face { font-family : LiberationSerif; font-weight : normal; font-style: italic; src : url(LiberationSerif-Italic.ttf); }\n";
                css += "@font-face { font-family : LiberationSerif; font-weight : bold; font-style: normal; src : url(LiberationSerif-Bold.ttf); }\n";
                css += "@font-face { font-family : LiberationSerif; font-weight : bold; font-style: italic; src : url(LiberationSerif-BoldItalic.ttf); }\n";

                epub.AddData("LiberationSerif-Regular.ttf", Bzway.DotNetBook.ePub.Test.Properties.Resources.LiberationSerif_Regular, "application/octet-stream");
                epub.AddData("LiberationSerif-Bold.ttf", Bzway.DotNetBook.ePub.Test.Properties.Resources.LiberationSerif_Bold, "application/octet-stream");
                epub.AddData("LiberationSerif-Italic.ttf", Bzway.DotNetBook.ePub.Test.Properties.Resources.LiberationSerif_Italic, "application/octet-stream");
                epub.AddData("LiberationSerif-BoldItalic.ttf", Bzway.DotNetBook.ePub.Test.Properties.Resources.LiberationSerif_BoldItalic, "application/octet-stream");
            }

            epub.AddStylesheetData("style.css", css);
            Bitmap coverImg = Bzway.DotNetBook.ePub.Test.Properties.Resources.pngSample;
            MemoryStream coverData = new MemoryStream();
            coverImg.Save(coverData, ImageFormat.Png);
            String coverId = epub.AddImageData("cover.png", coverData.GetBuffer());
            epub.AddMetaItem("cover", coverId);

            String page_template = Encoding.UTF8.GetString(Bzway.DotNetBook.ePub.Test.Properties.Resources.page);

            int navCounter = 1;
            int pageCounter = 1;

            string content = "<h1>Header 1</h1>";
            content += "<h2>Header 2</h2>";
            content += "<h3>Header 3</h3>";
            content += "<h4>Regular paragraph</h4>";
            content += "<p>" + Bzway.DotNetBook.ePub.Test.Properties.Resources.lorem + "<a href=\"notes.xhtml#n1\">[1]</a></p>";

            content += "<h4>Bold paragraph</h4>";
            content += "<p class=\"bold\">" + Bzway.DotNetBook.ePub.Test.Properties.Resources.lorem + "</p>";

            content += "<h4>Italic paragraph</h4>";
            content += "<p class=\"italic\">" + Bzway.DotNetBook.ePub.Test.Properties.Resources.lorem + "</p>";

            content += "<h4>Bold+Italic paragraph</h4>";
            content += "<p class=\"bolditalic\">" + Bzway.DotNetBook.ePub.Test.Properties.Resources.lorem + "</p>";

            String page = page_template.Replace("%%CONTENT%%", content);

            String pageName = String.Format("page{0}.xhtml", pageCounter);
            epub.AddXhtmlData(pageName, page);
            epub.AddNavPoint("Lorem ipsum", pageName, navCounter++);
            pageCounter++;
            pageName = String.Format("page{0}.xhtml", pageCounter);
            if (checkBoxGif.Checked || checkBoxPng.Checked || checkBoxJpeg.Checked || checkBoxSvg.Checked)
            {

                var navImageTests = epub.AddNavPoint("Image tests", pageName, navCounter);

                if (checkBoxPng.Checked)
                {
                    Bitmap img = Bzway.DotNetBook.ePub.Test.Properties.Resources.pngSample;
                    MemoryStream memImg = new MemoryStream();
                    img.Save(memImg, ImageFormat.Png);
                    epub.AddImageData("sample.png", memImg.GetBuffer());
                    page = page_template.Replace("%%CONTENT%%", "<img src=\"sample.png\" alt=\"\"/>");
                    epub.AddXhtmlData(pageName, page);
                    navImageTests.AddNavPoint("PNG Test", pageName, navCounter++);
                    pageCounter++;
                    pageName = String.Format("page{0}.xhtml", pageCounter);
                }

                if (checkBoxJpeg.Checked)
                {
                    Bitmap img = Bzway.DotNetBook.ePub.Test.Properties.Resources.jpegSample;
                    MemoryStream memImg = new MemoryStream();
                    img.Save(memImg, ImageFormat.Jpeg);
                    epub.AddImageData("sample.jpg", memImg.GetBuffer());
                    page = page_template.Replace("%%CONTENT%%", "<img src=\"sample.jpg\" alt=\"\"/>");
                    epub.AddXhtmlData(pageName, page);
                    navImageTests.AddNavPoint("JPEG Test", pageName, navCounter++);
                    pageCounter++;
                    pageName = String.Format("page{0}.xhtml", pageCounter);
                }

                if (checkBoxGif.Checked)
                {
                    Bitmap img = Bzway.DotNetBook.ePub.Test.Properties.Resources.gifSample;
                    MemoryStream memImg = new MemoryStream();
                    img.Save(memImg, ImageFormat.Gif);
                    epub.AddImageData("sample.gif", memImg.GetBuffer());
                    page = page_template.Replace("%%CONTENT%%", "<img src=\"sample.gif\" alt=\"\"/>");
                    epub.AddXhtmlData(pageName, page);
                    navImageTests.AddNavPoint("GIF Test", pageName, navCounter++);
                    pageCounter++;
                    pageName = String.Format("page{0}.xhtml", pageCounter);
                }

                if (checkBoxSvg.Checked)
                {
                    byte[] svg = Bzway.DotNetBook.ePub.Test.Properties.Resources.svgSample;

                    epub.AddImageData("sample.svg", svg);
                    page = page_template.Replace("%%CONTENT%%", "<img width=\"500\" height=\"500\" src=\"sample.svg\" alt=\"\"/>");
                    epub.AddXhtmlData(pageName, page);
                    navImageTests.AddNavPoint("SVG Test", pageName, navCounter++);
                    pageCounter++;
                    pageName = String.Format("page{0}.xhtml", pageCounter);
                }
            }

            page = page_template.Replace("%%CONTENT%%", richtextSampleContent.Text);

            epub.AddXhtmlData(pageName, page);
            epub.AddNavPoint("Your sample content", pageName, navCounter++);
            pageCounter++;
            pageName = String.Format("page{0}.xhtml", pageCounter);

            page = page_template.Replace("%%CONTENT%%", "<h1>Notes</h1><a id=\"n1\">1</a> Just note sample");

            epub.AddXhtmlData("notes.xhtml", page);
            SaveFileDialog saveFileDialog = new SaveFileDialog();


            saveFileDialog.Filter = "epub files (*)|*|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                epub.Generate(saveFileDialog.FileName);
            }
        }
    }
}
