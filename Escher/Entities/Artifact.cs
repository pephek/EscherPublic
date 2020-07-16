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
        public Color ForeColor;
        public SolidBrush TextColor;
        public string Text;
        public Image Image;
        public string Overprint;
        public Font Font;
        public Appearance Appearance;
        public bool RoundedCorners;
        public string Number;
        public RotateFlipType RotateFlipType;
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

        public Artifact GetScaledCopy(float scale)
        {
            if (scale == 1)
            {
                return this;
            }
            else
            {
                Artifact artifact = (Artifact)this.MemberwiseClone();

                artifact.X *= scale;
                artifact.Y *= scale;
                artifact.Width *= scale;
                artifact.Height *= scale;

                return artifact;
            }
        }

        public bool Overlaps(Point location)
        {
            return (location.X >= X && location.X <= Right() && location.Y >= Y && location.Y <= Bottom()) ;
        }
    }
}
