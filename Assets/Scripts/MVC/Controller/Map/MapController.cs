using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapController : IDisposable
{
    readonly IMapModel model;
    readonly MapView view;
    readonly MapTileSpriteDatabase spriteDatabase;

    public MapController (IMapModel model, MapView view, MapTileSpriteDatabase spriteDatabase)
    {
        this.model = model;
        this.view = view;
        this.spriteDatabase = spriteDatabase;
    }

    public void Initialize ()
    {
        int width = model.Width;
        int height = model.Height;

        view.Initialize(new Vector2Int(width, height));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
                view.SetTileSprite(new Vector2Int(x, y), spriteDatabase.GetSprite(model[x, y]));
        }
    }

    public void Dispose ()
    {
        Object.Destroy(view.gameObject);
    }
}
