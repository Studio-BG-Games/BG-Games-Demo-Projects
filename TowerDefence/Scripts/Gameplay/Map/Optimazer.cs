using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using LunarCatsStudio.SuperCombiner;
using UnityEngine;

namespace Gameplay.Map
{
    public class Optimazer
    {
        private List<UnitySector> _sectors = new List<UnitySector>();
        
        public void OptimazeSector(UnitySector secotr)
        {
            return;
            _sectors.Add(secotr);
            var gmSector = secotr.gameObject;
            var combine = gmSector.AddComponent<SuperCombiner>();
            SettingCombine(combine, secotr.ToString()+"combine");
            combine.Combine(GetMeshRenderBlocks(secotr), new List<SkinnedMeshRenderer>());

            foreach (var secotrChunk in secotr.chunks)
            {
                //((UnityChunk)secotrChunk).gameObject.SetActive(false);
                foreach (var block in secotrChunk.blocks)
                {
                    if(block==null)
                        continue;
                    ((UnityBlock)block).GetContent()?.GoOverBrick(x=>x.Off());
                }
            }
        }

        public void OptimazeWorld(UnityWorld world)
        {
            /*
            var worldGM = world.gameObject;
            var combine = worldGM.AddComponent<SuperCombiner>();
            SettingCombine(combine, "CombineWorld");
            IEnumerable<MeshRenderer> meshRendereres = new List<MeshRenderer>();
            foreach (var unitySector in _sectors)
            {
                meshRendereres = meshRendereres.Union(GetMeshRenderBlocks(unitySector));
            }
            combine.Combine(meshRendereres.ToList(), new List<SkinnedMeshRenderer>());*/
        }

        private List<MeshRenderer> GetMeshRenderBlocks(UnitySector secotr)
        {
            List< MeshRenderer> result =new List<MeshRenderer>();
            foreach (var chunk in secotr.chunks)
            {
                foreach (IBlock block in chunk.blocks)
                {
                    var content = block?.GetContent();
                    if(content==null)
                        continue;
                    content.GoOverBrick(x =>
                    {
                        x.MeshRenderers.ForEach(mesh =>
                        {
                            if(mesh)
                                result.Add(mesh);
                        });
                    });
                }
            }

            return result;
        }

        private static void SettingCombine(SuperCombiner combine, string name)
        {
            combine._sessionName = name;
            combine._textureAtlasSize = 512;
            combine._combineMeshes = true;
            combine._combineMaterials = true;
            combine._manageColliders = false;
        }
    }
}