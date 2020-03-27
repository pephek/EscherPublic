using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public class Artifact
    {
        public readonly ArtifactType Type;
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public Pen ForeColor;
        public Color BackColor;
        public string Text;
        public string Filename;
        public PictureBox Picture;
        public string FontName;
        public float FontSize;
        public bool FontBold;
        public bool FontItalic;
        public Appearance Appearance;

        public Artifact(ArtifactType type)
        {
            Type = type;
        }
    }
}
