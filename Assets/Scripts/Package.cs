using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Tile {
    public bool occupied;
    public Document document;
}

public class Package : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public Transform documentPlacePlane;
    public Transform documentsParent;
    List<Document> documents = new List<Document>();
    public ShipButton shipButton;

    public List<Document> defaultDocumentPrefabs = new List<Document>();

    Tile[,] tiles = new Tile[6,6];

    void Awake() {
        tiles = new Tile[width, height];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GeneratePackage();
        }
    }

    public void Clear() {
        for (int i = documents.Count - 1; i >= 0; i--) {
            RemoveDocument(documents[i], true);
        }
    }

    public void GeneratePackage() {
        foreach (Document documentPrefab in defaultDocumentPrefabs) {
            PlaceDocument(Instantiate(documentPrefab));
        }
    }

    public Document Find(Document.Type type) {
        foreach (Document document in documents) {
            if (document.type == type) {
                return document;
            }
        }
        return null;
    }

    public bool ResultStampPresent() {
        foreach (Document document in documents) {
            if (document.documentClass == Document.DocumentClass.ResultStamp) {
                return true;
            }
        }
        return false;
    }

    public bool DocumentPositionIsValid(Document document, Vector2Int bottomLeftPosition) {
        if (bottomLeftPosition.x < 0) {
            return false;
        }
        if (bottomLeftPosition.y < 0) {
            return false;
        }
        if (bottomLeftPosition.x + document.width - 1 >= width) {
            return false;
        }
        if (bottomLeftPosition.y + document.height - 1 >= height) {
            return false;
        }

        for (int y = bottomLeftPosition.y; y < bottomLeftPosition.y + document.height; y++) {
            for (int x = bottomLeftPosition.x; x < bottomLeftPosition.x + document.width; x++) {
                if (tiles[x, y].occupied) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool TileIsWithinBounds(Vector2Int tile) {
        if (tile.x < 0) {
            return false;
        }
        if (tile.y < 0) {
            return false;
        }
        if (tile.x >= width) {
            return false;
        }
        if (tile.y >= height) {
            return false;
        }

        return true;
    }

    public bool TileIsOccupied(Vector2Int tile) {
        return tiles[tile.x, tile.y].occupied;
    }

    public Document GetDocumentAtTile(Vector2Int tile) {
        return tiles[tile.x, tile.y].document;
    }

    public void PlaceDocument(Document document) {
        Vector2Int position;
        do {
            position = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (!DocumentPositionIsValid(document, position));

        PlaceDocument(document, position);
    }

    public void PlaceDocument(Document document, Vector2Int bottomLeftPosition) {
        if (document.documentClass != Document.DocumentClass.Free) {
            for (int i = documents.Count - 1; i >= 0; i--) {
                if (documents[i].documentClass == document.documentClass) {
                    RemoveDocument(documents[i], true);
                }
            }
        }

        for (int y = bottomLeftPosition.y; y < bottomLeftPosition.y + document.height; y++) {
            for (int x = bottomLeftPosition.x; x < bottomLeftPosition.x + document.width; x++) {
                tiles[x, y].document = document;
                tiles[x, y].occupied = true;
            }
        }

        documents.Add(document);
        document.bottomLeftPosition = bottomLeftPosition;

        document.transform.SetParent(documentsParent);
        document.transform.localPosition = DocumentCornerToCenter(TileToWorldSpace(bottomLeftPosition), document);
        document.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f));
    }

    public void RemoveDocument(Document document, bool destroy) {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (tiles[x, y].occupied && tiles[x, y].document == document) {
                    tiles[x, y].occupied = false;
                }
            }
        }

        documents.Remove(document);

        if (destroy) {
            Destroy(document.gameObject);
        }
        else {
            document.transform.SetParent(null);
        }
    }

    public Vector3 DocumentCenterToCorner(Vector3 position, Document document) {
        float x = position.x - document.width + 1f;
        float y = position.y - document.height + 1f;

        return new Vector3(x, y, 0f);
    }

    public Vector3 DocumentCornerToCenter(Vector3 position, Document document) {
        float x = position.x + document.width - 1f;
        float y = position.y + document.height - 1f;

        return new Vector3(x, y, 0f);
    }

    public Vector3 TileToWorldSpace(Vector2Int tile) {
        float worldX = tile.x * 2f - width + 1f;
        float worldY = tile.y * 2f - height + 1f;
        return new Vector3(worldX, worldY, 0f);
    }

    public Vector2Int WorldToTileSpace(Vector3 position) {
        int tileX = Mathf.FloorToInt((position.x + width - 1f) / 2f + 0.5f);
        int tileY = Mathf.FloorToInt((position.y + height - 1f) / 2f + 0.5f);
        return new Vector2Int(tileX, tileY);
    }
}
