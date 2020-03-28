using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Escher
{
    public class Artifacts
    {
        private List<Artifact> artifacts;

        private Graphics graphics;
        private float scaleX;
        private float scaleY;

        public Artifacts(Graphics graphics, float scaleX, float scaleY)
        {
            this.graphics = graphics;
            this.scaleX = scaleX;
            this.scaleY = scaleY;

            this.artifacts = new List<Artifact>();
        }

        public List<Artifact> Get()
        {
            return artifacts;
        }

        public int Count()
        {
            return artifacts.Count();
        }

        public Artifact Last()
        {
            return artifacts[artifacts.Count() - 1];
        }

        private void AddCursor(float x, float y)
        {
            Artifact artifact = new Artifact(ArtifactType.Cursor);

            artifact.X = x;
            artifact.Y = y;

            artifacts.Add(artifact);
        }

        public void AddMove(float width, float height, Pen foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular, bool screenOnly = false)
        {
            ArtifactType type;

            if (frameStyle == FrameStyle.Thick)
            {
                type = ArtifactType.Rectangle;
            }
            else if (frameStyle == FrameStyle.ThinSolid)
            {
                type = ArtifactType.MoveSolid;
            }
            else
            {
                type = ArtifactType.MoveDotted;
            }

            Artifact artifact = new Artifact(type);

            artifact.Width = width;
            artifact.Height = height;
            artifact.ForeColor = foreColor;
            artifact.Appearance = appearance;
            artifact.screenOnly = screenOnly;

            artifacts.Add(artifact);
        }

        public void AddRectangle(float x, float y, float width, float height, Pen foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular, bool screenOnly = false)
        {
            AddCursor(x, y);

            if (frameStyle == FrameStyle.ThinSolid || frameStyle == FrameStyle.ThinDotted)
            {
                AddMove(width, 0, foreColor, frameStyle, appearance, screenOnly);
                AddMove(0, height, foreColor, frameStyle, appearance, screenOnly);
                AddMove(-width, 0, foreColor, frameStyle, appearance, screenOnly);
                AddMove(0, -height, foreColor, frameStyle, appearance, screenOnly);
            }
            else // frameStyle == FrameStyle.ThickFrame
            {
                AddMove(width, height, foreColor, frameStyle, appearance, screenOnly);
            }
        }

        public void AddText(float x, float y, string text, SolidBrush textColor, string fontName, float fontSize, bool fontBold = false, bool fontItalic = false, bool screenOnly = false)
        {
            Artifact artifact = new Artifact(ArtifactType.Text);
            
            artifact.X = x;
            artifact.Y = y;
            artifact.Text = text;
            artifact.TextColor = textColor;
            artifact.Font = new Font(fontName, fontSize, (fontBold ? FontStyle.Bold: 0) | (fontItalic ? FontStyle.Italic : 0));
            artifact.screenOnly = screenOnly;

            SizeF size = graphics.MeasureString(artifact.Text, artifact.Font);
            artifact.Width = size.Width;
            artifact.Height = size.Height;

            artifacts.Add(artifact);
        }
    }
}
