using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapController : IDisposable
{
    readonly IMapModel model;
    readonly MapView view;
    readonly MapTileSpriteDatabase spriteDatabase;
    readonly ContentFitterCamera contentFitter;

    public MapController (
        IMapModel model,
        MapView view,
        MapTileSpriteDatabase spriteDatabase,
        ContentFitterCamera contentFitter
    )
    {
        this.model = model;
        this.view = view;
        this.spriteDatabase = spriteDatabase;
        this.contentFitter = contentFitter;
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

        SetupCamera();
    }

    void SetupCamera ()
    {
        const float TILE_PIVOT_OFFSET = 0.5f;
        contentFitter.SetViewContent(
            new CameraViewContent(
                new Vector3(-TILE_PIVOT_OFFSET, -TILE_PIVOT_OFFSET, 0),
                new Vector3(model.Width - TILE_PIVOT_OFFSET, model.Height - TILE_PIVOT_OFFSET, 0)
            )
        );
    }

    public void Dispose ()
    {
        Object.Destroy(view.gameObject);
    }
}
