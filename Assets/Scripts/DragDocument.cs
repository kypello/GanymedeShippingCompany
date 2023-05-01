using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct WorldDocumentSpacePosition {
    public Vector3 worldSpacePosition;
    public Vector2Int documentSpacePosition;
}

public class DragDocument : MonoBehaviour
{
    public UIManager uiManager;

    public Document documentPrefab;
    public Camera cam;
    public Document documentBeingDragged;
    public Transform placePreview;
    public RectTransform placePreviewCanvas;
    public Package package;
    public Palette palette;
    public RectTransform noSymbol;

    public Transform documentPlacePlane;

    public bool draggingDocument = false;
    public bool clickedThisFrame = false;

    public AudioSource buttonSound;
    public AudioSource documentPickupSound;
    public AudioSource documentPlaceSound;
    public AudioSource documentDeleteSound;

    void Update() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!draggingDocument && uiManager.state == UIManager.State.SelectDocument) {
            Physics.Raycast(ray, out hit, 20f, 1<<6);
            
            Vector2Int highlightedTile = package.WorldToTileSpace(hit.point);

            if (package.TileIsWithinBounds(highlightedTile) && package.TileIsOccupied(highlightedTile) && !DropdownMenu.instance.IsActive() && !TextInputMonitor.instance.IsMonitoring()) {
                Document highlightedDocument = package.GetDocumentAtTile(highlightedTile);

                if (highlightedDocument.CheckHighlightableSegments() != null) {
                    placePreview.gameObject.SetActive(false);

                    if (Input.GetMouseButtonDown(0)) {
                        Debug.Log("Click");
                        highlightedDocument.CheckHighlightableSegments().GetClicked();
                        buttonSound.Play();
                    }
                }
                else {
                    placePreview.gameObject.SetActive(true);

                    placePreview.transform.position = package.DocumentCornerToCenter(package.TileToWorldSpace(highlightedDocument.bottomLeftPosition), highlightedDocument) + Vector3.forward * documentPlacePlane.position.z;
                    placePreview.transform.rotation = highlightedDocument.transform.rotation;
                    placePreviewCanvas.sizeDelta = new Vector2(highlightedDocument.width * 200, highlightedDocument.height * 200);

                    if (Input.GetMouseButtonDown(0) && !clickedThisFrame) {
                        documentPickupSound.Play();
                        documentBeingDragged = highlightedDocument;
                        documentBeingDragged.transform.rotation = Quaternion.identity;
                        draggingDocument = true;
                        package.RemoveDocument(highlightedDocument, false);
                        clickedThisFrame = true;
                        uiManager.EnterDraggingDocumentState();
                    }
                }
            }
            else {
                placePreview.gameObject.SetActive(false);
            }
        }

        if (draggingDocument) {
            Physics.Raycast(ray, out hit, 20f, 1<<7);
            documentBeingDragged.transform.position = hit.point;

            Physics.Raycast(ray, out hit, 20f, 1<<6);
            
            Vector3 documentCenterPosition = SnapInputToDocumentCenter(hit.point, documentBeingDragged);
            Vector2Int documentCornerTile = package.WorldToTileSpace(package.DocumentCenterToCorner(documentCenterPosition, documentBeingDragged));

            if (package.DocumentPositionIsValid(documentBeingDragged, documentCornerTile)) {
                documentBeingDragged.SetTransparentState(Document.TransparentState.SemiTransparent);
                noSymbol.gameObject.SetActive(false);
                placePreview.gameObject.SetActive(true);
                placePreview.position = documentCenterPosition;
                placePreview.rotation = Quaternion.identity;
                placePreviewCanvas.sizeDelta = new Vector2(documentBeingDragged.width * 200, documentBeingDragged.height * 200);

                if (Input.GetMouseButtonDown(0) && !clickedThisFrame) {
                    documentPlaceSound.Play();
                    documentBeingDragged.SetTransparentState(Document.TransparentState.Opaque);
                    package.PlaceDocument(documentBeingDragged, documentCornerTile);
                    placePreview.gameObject.SetActive(false);
                    draggingDocument = false;
                    uiManager.EnterSelectDocumentState();
                }
                else if (Input.GetMouseButtonDown(1) && documentBeingDragged.deleteAllowed) {
                    documentDeleteSound.Play();
                    placePreview.gameObject.SetActive(false);
                    draggingDocument = false;
                    Destroy(documentBeingDragged.gameObject);
                    uiManager.EnterSelectDocumentState();
                }
            }
            else {
                documentBeingDragged.SetTransparentState(Document.TransparentState.Transparent);
                placePreview.gameObject.SetActive(false);

                if (!documentBeingDragged.deleteAllowed) {
                    noSymbol.gameObject.SetActive(true);
                    noSymbol.anchoredPosition = new Vector2(Input.mousePosition.x / Screen.width * 2560f - 80f - 160f * (documentBeingDragged.width - 1), Input.mousePosition.y / Screen.height * 1440f - 80f - 160f * (documentBeingDragged.height - 1));
                }

                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !clickedThisFrame && documentBeingDragged.deleteAllowed) {
                    documentDeleteSound.Play();
                    draggingDocument = false;
                    Destroy(documentBeingDragged.gameObject);
                    noSymbol.gameObject.SetActive(false);
                    uiManager.EnterSelectDocumentState();
                }
            }
        }

        clickedThisFrame = false;
    }

    public void SpawnNewDocument(Document prefab) {
        documentBeingDragged = Instantiate(prefab);
        draggingDocument = true;
        clickedThisFrame = true;
    }

    Vector3 SnapInputToDocumentCenter(Vector3 position, Document document) {
        float x = 0;
        float y = 0;

        if (document.width % 2 == 0) {
            x = RoundToEven(position.x);
        }
        else {
            x = RoundToOdd(position.x);
        }

        if (document.height % 2 == 0) {
            y = RoundToEven(position.y);
        }
        else {
            y = RoundToOdd(position.y);
        }

        return new Vector3(x, y, documentPlacePlane.position.z);
    }

    float RoundToEven(float f) {
        return Mathf.Floor(f / 2f + 0.5f) * 2f;
    }

    float RoundToOdd(float f) {
        return Mathf.Floor((f + 1f) / 2f + 0.5f) * 2f - 1f;
    }
}
