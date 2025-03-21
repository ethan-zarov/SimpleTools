﻿using UnityEngine;
using System.Collections.Generic;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
       
        public static void GizmoSquare(this Vector2 position, float size)
        {
            var hs = size / 2f;
            Vector2 ul = new Vector2(position.x - hs, position.y + hs);
            Vector2 ur = new Vector2(position.x + hs, position.y + hs);
            Vector2 bl = new Vector2(position.x - hs, position.y - hs);
            Vector2 br = new Vector2(position.x + hs, position.y - hs);

            Gizmos.DrawLine(ul, ur);
            Gizmos.DrawLine(ur, br);
            Gizmos.DrawLine(br, bl);
            Gizmos.DrawLine(bl, ul);
        }
        
        public static void GizmoArrow(this Vector2 value, Vector2 direction)
        {
            value.GizmoArrow(direction, direction.magnitude * .12f, 25);
        }
        public static void GizmoArrow(this Vector2 value, Vector2 direction, float distance)
        {
            value.GizmoArrow(direction.normalized * distance);
        }
        public static void GizmoArrow(this Vector2 position, Vector2 direction, float arrowSize, float arrowSpread)
        {
            
            var endPosition = position + direction;
            Gizmos.DrawLine(position, endPosition);
            var leftDir = -direction.Rotate(-arrowSpread).normalized * arrowSize;
            var rightDir = -direction.Rotate(arrowSpread).normalized * arrowSize;

            Gizmos.DrawLine(endPosition, endPosition + leftDir);
            Gizmos.DrawLine(endPosition, endPosition + rightDir);
        }


        private static void GenerateSightlineMesh(this Mesh mesh, Vector2 position, Vector2 sightVector, float angleSpread)
        {
            //Get total number of points. There should be a point every 3 degrees.
            //If angle spread is 28 then from an edge of the sightline to the center, there should be 9 intermediary points.
            var intermediaryPoints = Mathf.FloorToInt(angleSpread / 3f);
            var totalPoints = 4 + intermediaryPoints * 2; //The four additional points are the origin, far left, center, and far right.

            Vector3[] vertices = new Vector3[totalPoints];
            Vector3[] normals = new Vector3[totalPoints];
            
            //Set triangles. If there are 5 points, there are 3 triangles.
            int[] triangles = new int[(totalPoints - 2) * 3];
            
            ////////////////////////////////////////////////////////////////////
            
            //Origin
            vertices[0] = position;
            normals[0] = Vector3.forward;
            
            
            //Counter-clockwise-most point
            vertices[1] = position + sightVector.Rotate(angleSpread);
            normals[1] = Vector3.forward;
            
            //Set left side points
            float tDiv = intermediaryPoints + 1;
            for (var i = 0; i < intermediaryPoints; i++)
            {
                var t = (intermediaryPoints - i) / tDiv;
                vertices[i+2] = position + sightVector.Rotate(angleSpread * t);
                normals[i+2] = Vector3.forward;
            }
            
            //Center sightline point
            vertices[intermediaryPoints+2] = position + sightVector; 
            normals[intermediaryPoints+2] = Vector3.forward;
            //Set right side points
            for (var i = 0; i < intermediaryPoints; i++)
            {
                var t = (i+1) / tDiv;
                vertices[i+intermediaryPoints + 3] = position + sightVector.Rotate(-angleSpread * t);
                normals[i+intermediaryPoints + 3] = Vector3.forward;
            }

            vertices[totalPoints - 1] = position + sightVector.Rotate(-angleSpread);
            normals[totalPoints - 1] = Vector3.forward;
            
            for (int i = 0; i < totalPoints-2; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
        }
        public static void GizmoSightline(this Mesh mesh, Vector2 position, Vector2 sightVector, float angleSpread)
        {
            mesh.GenerateSightlineMesh(position, sightVector, angleSpread);

            Gizmos.color = Gizmos.color.SetAlpha(.25f);
            Gizmos.DrawMesh(mesh);

            Gizmos.color = Gizmos.color.SetAlpha(1);

        }

        public static void GizmoSightline(this Mesh mesh, Vector2 position, Vector2 sightVector, float angleSpread, float distance)
        {
            mesh.GizmoSightline(position, sightVector.normalized * distance, angleSpread);
        }
        
        public static void GizmoSightline(this Mesh mesh, Vector2 position, float angle, float angleSpread)
        {
            mesh.GizmoSightline(position, angle.DegToVec2(), angleSpread);
        }
        
        public static void GizmoWireSightline(this Mesh mesh, Vector2 position, Vector2 sightVector, float angleSpread)
        {
            mesh.GenerateSightlineMesh(position, sightVector, angleSpread);

            Gizmos.color = Gizmos.color.SetAlpha(.25f);
            Gizmos.DrawWireMesh(mesh);

            Gizmos.color = Gizmos.color.SetAlpha(1);

        }

        public static void GizmoWireSightline(this Mesh mesh, Vector2 position, Vector2 sightVector, float angleSpread, float distance)
        {
            mesh.GizmoWireSightline(position, sightVector.normalized * distance, angleSpread);
        }

        public static void GizmoWireSightline(this Mesh mesh, Vector2 position, float angle, float angleSpread)
        {
            mesh.GizmoWireSightline(position, angle.DegToVec2(), angleSpread);
        }
    }

}