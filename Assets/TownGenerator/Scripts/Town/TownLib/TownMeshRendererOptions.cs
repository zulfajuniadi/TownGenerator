using UnityEngine;

namespace Town
{
    [System.Serializable]
    public class TownMeshRendererOptions
    {
        public Transform Root;
        public Material BuildingMaterial;
        public Material RoadMaterial;
        public Material WallMaterial;
        public Material TowerMaterial;
        public Material GateMaterial;
        public Material OuterCityGroundMaterial;
        public Material CityCenterGround;
        public Material WithinWallsGroundMaterial;
        public Material CastleGroundMaterial;
        public Material WaterMaterial;
        public Material PoorArea;
        public Material RichArea;
        public Material FarmArea;
    }
}