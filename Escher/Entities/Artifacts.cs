using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Escher
{
    public class Artifacts : List<Artifact>
    {
        public static Artifact CreateCursor(float x, float y)
        {
            Artifact artifact = new Artifact(ArtifactType.Cursor);

            artifact.X = x;
            artifact.Y = y;

            return artifact;
        }

        public static Artifact CreateMove(float width, float height, Pen foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular)
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

            return artifact;
        }

        public static List<Artifact> CreateRectangle(float x, float y, float width, float height, Pen foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular)
        {
            List<Artifact> artifacts = new List<Artifact>();

            artifacts.Add(CreateCursor(x, y));

            if (frameStyle == FrameStyle.ThinSolid || frameStyle == FrameStyle.ThinDotted)
            {
                artifacts.Add(CreateMove(width, 0, foreColor, frameStyle));
                artifacts.Add(CreateMove(0, height, foreColor, frameStyle));
                artifacts.Add(CreateMove(-width, 0, foreColor, frameStyle));
                artifacts.Add(CreateMove(0, -height, foreColor, frameStyle));
            }
            else // frmeStyle == FrameStyle.ThickFrame
            {
                artifacts.Add(CreateMove(width, height, foreColor, frameStyle, appearance));
            }

            return artifacts;
        }
    }

    public static class ArtifactsExtensionMethods
    {
        public static void AddRectangle(this Artifacts artifacts, float x, float y, float width, float height, Pen foreColor, FrameStyle frameStyle = FrameStyle.ThinSolid, Appearance appearance = Appearance.Singular)
        {
            artifacts.AddRange(Artifacts.CreateRectangle(x, y, width, height, foreColor, frameStyle, appearance));
        }
    }
}
