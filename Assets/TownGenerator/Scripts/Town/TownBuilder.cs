using System.Collections;
using System.Collections.Generic;
using Town;
using UnityEngine;
using UnityEngine.Profiling;

public class TownBuilder : MonoBehaviour
{
	public TownOptions townOptions = new TownOptions ();
	public TownMeshRendererOptions rendererOptions = new TownMeshRendererOptions ();

	public void Clear ()
	{
		if (rendererOptions.Root == null)
		{
			rendererOptions.Root = new GameObject ("TownRoot").transform;
		}
		for (int i = rendererOptions.Root.childCount - 1; i > -1; i--)
		{
			DestroyImmediate (rendererOptions.Root.GetChild (i).gameObject);
		}
	}

	public void GenerateRandom ()
	{
		townOptions.Seed = Random.Range (1, 999999);
		Generate ();
	}

	public void Generate ()
	{
		Clear ();
		Profiler.BeginSample ("TownGenerator");

		var town = new Town.Town (townOptions);

		var renderer = new TownMeshRenderer (town, townOptions, rendererOptions);

		renderer.Generate ();

		Profiler.EndSample ();
	}
}