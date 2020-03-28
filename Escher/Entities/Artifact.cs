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
        public SolidBrush TextColor;
        public string Text;
        public string Filename;
        public PictureBox Picture;
        public Font Font;
        public Appearance Appearance;
        public bool screenOnly;

        public Artifact(ArtifactType type)
        {
            Type = type;
        }

        public float Right()
        {
            return X + Width;
        }

        public float Bottom(float margin = 0)
        {
            return Y + Height + margin;
        }

        public Artifact GetScaledCopy(float scaleX, float scaleY)
        {
            if (scaleX == 1 && scaleY == 1)
            {
                return this;
            }
            else
            {
                Artifact artifact = (Artifact)this.MemberwiseClone();

                artifact.X *= scaleX;
                artifact.Y *= scaleY;
                artifact.Width *= scaleX;
                artifact.Height *= scaleY;

                return artifact;
            }
        }
    }
}
