using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Transform : Component
    {
        Vector2 position;
        float width;
        float height;
        float radians;
        Vector2 scale = new Vector2(1,1);
        Transform parent;
        float degreesFromParent = 0f;
        float startingRotation = 0f;
        float distanceToParent = 0f;
        byte layer;

        public Vector2 Position { get {
                if (parent == null)
                    return position;
                else
                    return hf_Math.getRotationPosition(hf_Math.RadiansToDegres(degreesFromParent + parent.radians), distanceToParent, parent.position) + position;
            } }    
        public float Width { get { return width; } }
        public float Height { get { return height; } }
        public float Radians { get {
                if (parent == null)
                    return radians;
                else
                    return radians + parent.radians - startingRotation;
            }
            set { radians = value; }
        }
        public Vector2 Scale { get { return scale; } }
        public float Radius { get { return (float)Math.Sqrt(Math.Pow(Height / 2, 2) + Math.Pow(Width / 2, 2)); } }
        public Transform Parent { get { return parent; } }
        public byte Layer { get { return layer; } set { layer = value; } }
        public Transform(GameObject go, Vector2 pos, float w, float h, float r, byte l) : base(go, "transform")
        {
            radians = r;
            position = pos;
            width = w;
            height = h;
            layer = l;
        }

        public void SetScale(float x, float y)
        {
            scale = new Vector2(x, y);
        }

        public void Resize(float x, float y)
        {
            width = x;
            height = y;
        }

        public void Move(Vector2 dist)
        {
            position += dist;
        }

        public void Place(Vector2 pos)
        {
            position = pos;
        }

        public void Rotate(float r)
        {
            radians += r;
        }

        public Vector2 WorldPosition(Vector2 offSet)
        {
            return (Position + offSet) * new Vector2(1,-1);
                //+ hf_Math.getRotationPosition(hf_Math.RadiansToDegres(Radians), (float)Math.Sqrt(Math.Pow(offSet.X, 2) + (float)Math.Pow(offSet.Y, 2)), new Vector2()) * Scale) * RenderingManager.GameScale * new Vector2(1,-1);
        }

        public void AttachToTransform(Transform t)
        {
            parent = t;
            startingRotation = t.radians;
            degreesFromParent = hf_Math.getAngle(t.position, position) - t.radians;
            distanceToParent = Vector2.Distance(position, t.position);
            position = position - t.position;
        }

        public bool ContainsPoint(Vector2 v)
        {
            return v.X > Position.X - (Width * scale.X) / 2 &&
                    v.X < Position.X + (Width * scale.X) / 2 &&
                    v.Y > Position.Y - (Height * scale.Y) / 2 &&
                    v.Y < Position.Y + (Height * scale.Y) / 2;
        }

        public void DetachFromParent()
        {
            position = Position;
            radians = Radians;
            parent = null;
            degreesFromParent = 0;
            startingRotation = 0;          
        }
    }
}
