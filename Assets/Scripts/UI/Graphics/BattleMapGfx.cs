using Rhynn.Engine;
using UnityEngine;
using Util;
using Util.Pathfinding;

namespace UI.Graphics
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class BattleMapGfx : MonoBehaviour
    {
        [SerializeField] private float tileSize = 1.0f;

        [Header("Tile Textures")] 
        [SerializeField] private Texture2D terrainTileSet;

        [SerializeField] private int tileResolution;
        [SerializeField] private FilterMode tileTextureFilterMode;
        [SerializeField] private TextureWrapMode tileTextureWrapMode;

        [SerializeField] private bool DebugMode;

        public float TileSize => tileSize;

        public void Init(Game game)
        {
            _game = game;
            GenerateGridGraphics();
        }

        private void GenerateGridGraphics()
        {
            BuildMesh();
            BuildTexture();
        }

        /// <summary>
        /// Constructs a plain mesh from the <see cref="Rhynn.Engine.BattleMap"/>.Bounds.Size data in the Game object.
        /// </summary>
        /// TODO: Consider changing mesh generation to create individual meshes for each tile instead of one giant mesh.
        private void BuildMesh()
        {
            _dimensions = _game.BattleMap.Bounds.Size;

            int numTiles = _dimensions.Area;
            int numTris = numTiles * 2;

            int verticesNumX = _dimensions.x + 1;
            int verticesNumZ = _dimensions.y + 1;
            int numVertices = verticesNumX * verticesNumZ;
            
            // Instantiate mesh data structures
            Vector3[] vertices = new Vector3[numVertices];
            Vector3[] normals = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];

            int[] triangles = new int[numTris * 3];
            
            // Populate vertices, normals, and uv data structures with mesh data
            int x, z;
            for (z = 0; z < verticesNumZ; z++)
            {
                for (x = 0; x < verticesNumX; x++)
                {
                    vertices[z * verticesNumX + x] = new Vector3(x * tileSize, 0, z * tileSize);
                    normals[z * verticesNumX + x] = Vector3.up;
                    uv[z * verticesNumX + x] = new Vector2((float)x / _dimensions.x, (float)z / _dimensions.y);
                }
            }
            
            // Do a second pass now that vertices are all instantiated to fill triangles data
            for (z = 0; z < _dimensions.y; z++)
            {
                for (x = 0; x < _dimensions.x; x++)
                {
                    int quadOffset = z * _dimensions.x + x;
                    int triOffset = quadOffset * 6;
                    triangles[triOffset + 0] = z * verticesNumX + x + 0;
                    triangles[triOffset + 1] = z * verticesNumX + x + verticesNumX + 0;
                    triangles[triOffset + 2] = z * verticesNumX + x + verticesNumX + 1;
                    
                    triangles[triOffset + 3] = z * verticesNumX + x + 0;
                    triangles[triOffset + 4] = z * verticesNumX + x + verticesNumX + 1;
                    triangles[triOffset + 5] = z * verticesNumX + x + 1;
                }
            }
            
            // Create a new mesh and populate it with the data
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;
            
            // Assign mesh to filter and collider
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshCollider meshCollider = GetComponent<MeshCollider>();

            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
        }

        /// <summary>
        /// Generates a texture to place on the mesh generated in <see cref="BuildMesh"/>.
        /// </summary>
        private void BuildTexture()
        {
            int textureWidth = _dimensions.x * tileResolution;
            int textureHeight = _dimensions.y * tileResolution;
            Texture2D texture = new Texture2D(textureWidth, textureHeight);
            
            // TODO: Each texture is currently dependent on each tile in the tileset having the same ordering as that
            // of the GridTileData.Type enum. This should probably be loaded from a reference file instead.
            Color[][] tiles = BuildTileTextures();

            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    Color[] pixelColors = tiles[_game.BattleMap.Tiles[x, y].Type.SpriteIndex];
                    texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, 
                        tileResolution, pixelColors);
                }
            }

            texture.filterMode = tileTextureFilterMode;
            texture.wrapMode = tileTextureWrapMode;
            texture.Apply();

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterials[0].mainTexture = texture;
        }

        private Color[][] BuildTileTextures()
        {
            int numTilesPerRow = terrainTileSet.width / tileResolution;
            int numRows = terrainTileSet.height / tileResolution;

            Color[][] tiles = new Color[numTilesPerRow * numRows][];

            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numTilesPerRow; x++)
                {
                    tiles[y * numTilesPerRow + x] = terrainTileSet.GetPixels(x * tileResolution, y * tileResolution,
                        tileResolution, tileResolution);
                }
            }

            return tiles;
        }

        public void OnDrawGizmos()
        {
            if (DebugMode)
            {
                foreach (var tileCoordinate in _game.BattleMap.Bounds)
                {
                    var x = tileCoordinate.x + 0.5f;
                    var z = tileCoordinate.y + 0.5f;
                    var tileWorldSpaceCoordinates = new Vector3(x, 1, z);

                    var tile = _game.BattleMap.Tiles[tileCoordinate];
                
                    Gizmos.color = tile.AllowsMotility(Motility.Land) ? Color.green : Color.red;
                
                    Gizmos.DrawWireSphere(tileWorldSpaceCoordinates, 0.15f);

                    foreach (var neighbor in tile.Neighbors)
                    {
                        var color = tile.CanEnter(neighbor, Motility.Land) ? Color.green : Color.red;

                        var neighborX = neighbor.Position.x + 0.5f;
                        var neighborZ = neighbor.Position.y + 0.5f;
                        var neighborWorldSpaceCoordinates = new Vector3(neighborX, 1, neighborZ);
                        
                        var arrowStartVector = Vector3.Lerp(tileWorldSpaceCoordinates,
                            neighborWorldSpaceCoordinates, 0.2f);
                        var arrowEndVector = Vector3.Lerp(tileWorldSpaceCoordinates,
                            neighborWorldSpaceCoordinates, 0.9f);
                        
                        DrawArrow.ForDebug(arrowStartVector, arrowEndVector - arrowStartVector, color);
                    }
                }
                
                var tile1 = _game.BattleMap.Tiles[0, 0];
                var x1 = tile1.Position.x + 0.5f;
                var z1 = tile1.Position.y + 0.5f;
                var tile1WorldSpaceCoordinates = new Vector3(x1, 1, z1);
                
                var tile2 = _game.BattleMap.Tiles[0, 1];
                var x2 = tile2.Position.x + 0.5f;
                var z2 = tile2.Position.y + 0.5f;
                var tile2WorldSpaceCoordinates = new Vector3(x2, 1, z2);

                var arrowColor = Color.green;

                var startCoordinates = Vector3.Lerp(tile1WorldSpaceCoordinates, tile2WorldSpaceCoordinates, 0.2f);
                var endCoordinates = Vector3.Lerp(tile1WorldSpaceCoordinates, tile2WorldSpaceCoordinates, 0.8f);
                
                DrawArrow.ForDebug(startCoordinates, endCoordinates - startCoordinates, arrowColor);
            }
        }

        private Game _game;
        private Vec2 _dimensions;
    }
}
