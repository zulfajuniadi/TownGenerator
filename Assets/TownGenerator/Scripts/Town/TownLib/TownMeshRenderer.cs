using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeshUtils;
using Town.Geom;
using UnityEngine;

namespace Town
{
    public class TownMeshRenderer
    {
        private Town town;
        private TownOptions options;
        private TownMeshRendererOptions rendererOptions;

        private Transform child;
        public Transform root;
        public GameObject Waters;
        public GameObject Buildings;
        public GameObject BuildingsMesh;
        public GameObject Walls;
        public GameObject WallsMesh;
        public GameObject Roads;

        public TownMeshRenderer (Town town, TownOptions options, TownMeshRendererOptions rendererOptions)
        {
            this.town = town;
            this.options = options;
            root = rendererOptions.Root;
            this.rendererOptions = rendererOptions;
        }

        public GameObject Generate ()
        {
            var go = new GameObject ("Town");
            go.transform.parent = root;
            child = go.transform;
            child.transform.localPosition = Vector3.zero;
            var bounds = town.GetCityWallsBounds ().Expand (100);

            var geometry = town.GetTownGeometry (options);
            MeshUtils.Polygon poly;

            List<Vector3> vertices = new List<Vector3> ();
            UnityEngine.Random.InitState (options.Seed.GetHashCode ());

            if (options.Water)
            {
                Waters = new GameObject ("Waters");
                Waters.transform.parent = child;
                Waters.transform.localPosition = Vector3.zero;
                foreach (var water in geometry.Water)
                {
                    foreach (var vertex in water.Vertices)
                    {
                        vertices.Add (new Vector3 (vertex.x, -3f, vertex.y));
                    }
                    poly = new MeshUtils.Polygon ("Water", vertices, 3f, rendererOptions.WaterMaterial, Waters.transform);
                    poly.Transform.localPosition = Vector3.zero;
                    vertices.Clear ();
                }
            }
            else
            {
                Waters = null;
            }

            BuildingsMesh = new GameObject ("BuildingsMesh");
            BuildingsMesh.transform.parent = child;
            BuildingsMesh.transform.localPosition = Vector3.zero;
            Buildings = new GameObject ("Buildings");
            Buildings.transform.parent = child;
            Buildings.transform.localPosition = Vector3.zero;
            foreach (var building in geometry.Buildings)
            {
                foreach (var vertex in building.Shape.Vertices)
                {
                    vertices.Add (new Vector3 (vertex.x, 0, vertex.y));
                }
                poly = new MeshUtils.Polygon (building.Description + "Base", vertices, 0.1f, rendererOptions.BuildingMaterial, Buildings.transform, true);
                poly.Transform.localPosition = Vector3.zero;
                poly = new MeshUtils.Polygon (building.Description, vertices, UnityEngine.Random.Range (2f, 4f), rendererOptions.BuildingMaterial, BuildingsMesh.transform, false);
                poly.Transform.localPosition = Vector3.zero;
                vertices.Clear ();
            }
            DrawRoads (geometry, null);
            if (options.Walls)
            {
                DrawWalls (geometry, null);
            }
            else
            {
                Walls = null;
            }

            if (options.Overlay)
            {
                DrawOverlay (geometry, null);
            }

            return go;
        }

        private void DrawOverlay (TownGeometry geometry, StringBuilder sb)
        {
            MeshUtils.Polygon poly;
            List<Vector3> vertices = new List<Vector3> ();
            var overlays = new GameObject ("Overlays");

            overlays.transform.parent = child;
            overlays.transform.localPosition = Vector3.zero;
            foreach (var patch in geometry.Overlay)
            {
                if (patch.Water) continue;
                var type = patch.Area.ToString ();
                foreach (var vertex in patch.Shape.Vertices)
                {
                    vertices.Add (new Vector3 (vertex.x, -3f, vertex.y));
                }
                float offset = 3f;
                Material mat;
                if (patch.HasCastle)
                {
                    offset = 3.4f;
                    mat = rendererOptions.CastleGroundMaterial;
                }
                else if (patch.IsCityCenter)
                {
                    offset = 3.2f;
                    mat = rendererOptions.CityCenterGround;
                }
                else if (patch.WithinWalls)
                {
                    if (type == "Town.RichArea")
                    {
                        mat = rendererOptions.RichArea;
                    }
                    else if (type == "Town.PoorArea")
                    {
                        mat = rendererOptions.PoorArea;
                    }
                    else
                    {
                        mat = rendererOptions.WithinWallsGroundMaterial;
                    }
                }
                else
                {
                    if (type == "Town.FarmArea")
                    {
                        mat = rendererOptions.FarmArea;
                    }
                    else
                    {
                        mat = rendererOptions.OuterCityGroundMaterial;
                    }
                }
                poly = new MeshUtils.Polygon ("Patch", vertices, offset, mat, overlays.transform, true);
                poly.Transform.localPosition = Vector3.zero;
                vertices.Clear ();
            }
        }

        private void DrawRoads (TownGeometry geometry, StringBuilder sb)
        {
            Roads = new GameObject ("Roads");
            Roads.transform.parent = child;
            Roads.transform.localPosition = Vector3.zero;
            Cube cube;
            foreach (var road in geometry.Roads)
            {
                Geom.Vector2 last = new Geom.Vector2 (0, 0);
                foreach (var current in road)
                {
                    if (last.x != 0 && last.y != 0)
                    {
                        cube = new Cube ("Road", GetLineVertices (
                            last.x,
                            current.x,
                            last.y,
                            current.y,
                            2
                        ), 0.2f, rendererOptions.RoadMaterial, Roads.transform);
                        cube.Transform.localPosition = Vector3.zero;
                    }
                    last = current;
                }
            }
        }

        private void DrawWalls (TownGeometry geometry, StringBuilder sb)
        {
            Cube cube;
            WallsMesh = new GameObject ("WallsMesh");
            WallsMesh.transform.parent = child;
            WallsMesh.transform.localPosition = Vector3.zero;
            Walls = new GameObject ("Walls");
            Walls.transform.parent = child;
            Walls.transform.localPosition = Vector3.zero;
            var replacedGates = new List<Geom.Vector2> ();
            foreach (var wall in geometry.Walls)
            {
                var start = wall.A;
                var end = wall.B;

                if (geometry.Gates.Contains (start))
                {
                    replacedGates.Add (start);
                    start = start + Geom.Vector2.Scale (end - start, 0.3f);
                    wall.A = start;
                    geometry.Gates.Add (start);
                }

                if (geometry.Gates.Contains (end))
                {
                    replacedGates.Add (end);
                    end = end - Geom.Vector2.Scale (end - start, 0.3f);
                    wall.B = end;
                    geometry.Gates.Add (end);
                }
                cube = new Cube ("Wall", GetLineVertices (
                    start.x,
                    end.x,
                    start.y,
                    end.y
                ), 0.1f, rendererOptions.WallMaterial, Walls.transform);
                cube.Transform.localPosition = Vector3.zero;
                cube = new Cube ("WallMesh", GetLineVertices (
                    start.x,
                    end.x,
                    start.y,
                    end.y
                ), 4, rendererOptions.WallMaterial, WallsMesh.transform, false);
                cube.Transform.localPosition = Vector3.zero;
            }

            foreach (var replacedGate in replacedGates.Distinct ())
            {
                geometry.Gates.Remove (replacedGate);
            }

            foreach (var tower in geometry.Towers)
            {
                cube = new Cube ("Tower", GetVertices (4, 4, tower.x - 2, tower.y - 2), 0.1f, rendererOptions.TowerMaterial, Walls.transform);
                cube.Transform.localPosition = Vector3.zero;
                cube = new Cube ("TowerMesh", GetVertices (4, 4, tower.x - 2, tower.y - 2), 8, rendererOptions.TowerMaterial, WallsMesh.transform, false);
                cube.Transform.localPosition = Vector3.zero;
            }

            foreach (var gate in geometry.Gates)
            {
                cube = new Cube ("Gate", GetVertices (4, 4, gate.x - 2, gate.y - 2), 0.1f, rendererOptions.GateMaterial, Walls.transform);
                cube.Transform.localPosition = Vector3.zero;
                cube = new Cube ("GateMesh", GetVertices (4, 4, gate.x - 2, gate.y - 2), 6, rendererOptions.GateMaterial, WallsMesh.transform, false);
                cube.Transform.localPosition = Vector3.zero;
            }
        }

        private Vector3[] GetLineVertices (float startX, float endX, float startY, float endY, float thickness = 1f)
        {
            var p1 = new Vector3 (startX, 0, startY);
            var p2 = new Vector3 (endX, 0, endY);
            var dir = (p1 - p2).normalized;
            var norm = Vector3.Cross (dir, Vector3.up);
            var halfThickness = (norm * thickness) / 2;
            var p3 = p2 + halfThickness;
            var p4 = p1 + halfThickness + dir / 2;
            p1 = p1 - halfThickness + dir / 2;
            p2 = p2 - halfThickness;
            return new Vector3[]
            {
                p1,
                p2,
                p3,
                p4
            };
        }

        private Vector3[] GetVertices (int width, int length, float offsetX, float offsetZ)
        {
            return new Vector3[]
            {
                new Vector3 (offsetX, 0, offsetZ),
                    new Vector3 (offsetX, 0, offsetZ + length),
                    new Vector3 (offsetX + width, 0, offsetZ + length),
                    new Vector3 (offsetX + width, 0, offsetZ)
            };
        }
    }
}