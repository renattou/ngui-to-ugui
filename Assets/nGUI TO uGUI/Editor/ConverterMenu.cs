﻿using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ConverterMenu : MonoBehaviour {

	#region Convert Atlas Selected
	[MenuItem ("nGUI TO uGUI/Atlas Convert/Selected")]
	static void OnConvertAtlasSelected () {	
		if (Selection.activeGameObject != null){
			foreach(GameObject selectedObject in Selection.gameObjects){
				if (selectedObject.GetComponent<UIAtlas>()){
					UIAtlas tempNguiAtlas;
					tempNguiAtlas = selectedObject.GetComponent<UIAtlas>();
					if (File.Exists("Assets/CONVERSION_DATA/"+tempNguiAtlas.name+".png")){
						Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"CONVERSION_DATA\" </color>Directory");
					}else{
						ConvertAtlas(tempNguiAtlas);
					}
				}
			}
		}else{
			Debug.LogError ("<Color=red>NO ATLASES SELECTED</Color>, <Color=yellow>Please select something to convert</Color>");
		}
	}
	#endregion

	#region Convert Atlases In Scene
	[MenuItem ("nGUI TO uGUI/Atlas Convert/Current Scene")]
	static void OnConvertAtlasesInScene () {
		UISprite[] FoundAtlasesList;
		FoundAtlasesList = GameObject.FindObjectsOfType<UISprite>();
		for (int c=0; c<FoundAtlasesList.Length; c++){
			UIAtlas tempNguiAtlas;
			tempNguiAtlas = FoundAtlasesList[c].atlas;
			if (File.Exists("Assets/CONVERSION_DATA/"+tempNguiAtlas.name+".png")){
				Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"CONVERSION_DATA\" </color>Directory");
			}else{
				ConvertAtlas(tempNguiAtlas);
			}
		}
	}
	#endregion

	#region Convert Atlas From Selected
	[MenuItem ("nGUI TO uGUI/Atlas Convert/Related To Selected")]
	static void OnConvertAtlasesFromSelected () {
		if (Selection.activeGameObject != null){
			foreach(GameObject selectedObject in Selection.gameObjects){
				if (selectedObject.GetComponent<UISprite>()){
					UIAtlas tempNguiAtlas;
					tempNguiAtlas = selectedObject.GetComponent<UISprite>().atlas;
					if (File.Exists("Assets/CONVERSION_DATA/"+tempNguiAtlas.name+".png")){
						Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"CONVERSION_DATA\" </color>Directory");
					}else{
						ConvertAtlas(tempNguiAtlas);
					}
				}
			}
		}
	}
	#endregion

	#region PROCEDURALS Convert Atlas
	static void ConvertAtlas(UIAtlas theAtlas){
		if(!Directory.Exists("Assets/CONVERSION_DATA")){
			AssetDatabase.CreateFolder ("Assets", "CONVERSION_DATA");
		}else{
			
		}
		AssetDatabase.CopyAsset (AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(theAtlas.name)[0]), "Assets/CONVERSION_DATA/"+theAtlas.name+".png");
		AssetDatabase.Refresh();
		//Debug.Log(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(theAtlas.name)[0]) + "\n" + "Assets/CONVERSION_DATA/"+theAtlas.name+".png");
		
		string conversionPath = "Assets/CONVERSION_DATA/"+theAtlas.name+".png";
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(conversionPath);
		importer.textureType = TextureImporterType.Sprite;
		importer.mipmapEnabled = false;
		importer.spriteImportMode = SpriteImportMode.Multiple;
		
		List <UISpriteData> theNGUISpritesList = theAtlas.spriteList;
		SpriteMetaData[] theSheet = new SpriteMetaData[theNGUISpritesList.Count];
		
		for (int c=0; c<theNGUISpritesList.Count; c++){
			float theY = theAtlas.texture.height - (theNGUISpritesList[c].y + theNGUISpritesList[c].height);
			theSheet[c].name = theNGUISpritesList[c].name;
			theSheet[c].pivot = new Vector2(theNGUISpritesList[c].paddingLeft, theNGUISpritesList[c].paddingBottom);
			theSheet[c].rect = new Rect (theNGUISpritesList[c].x, theY, theNGUISpritesList[c].width, theNGUISpritesList[c].height);
			theSheet[c].border = new Vector4(theNGUISpritesList[c].borderLeft, theNGUISpritesList[c].borderBottom, theNGUISpritesList[c].borderRight, theNGUISpritesList[c].borderTop);
			theSheet[c].alignment = 0;
			Debug.Log (theSheet[c].name + "       " + theSheet[c].pivot);
		}
		importer.spritesheet = theSheet;
		AssetDatabase.ImportAsset(conversionPath, ImportAssetOptions.ForceUpdate);
	}
	#endregion

	#region Convert Wedgit Selected
	[MenuItem ("nGUI TO uGUI/Wedgit Convert/Selected")]
	static void OnConvertWedgitSelected () {
		GameObject inProgressObject;
		if (Selection.activeGameObject != null){
			foreach(GameObject selectedObject in Selection.gameObjects){

				inProgressObject = (GameObject) Instantiate (selectedObject, selectedObject.transform.position, selectedObject.transform.rotation);

				if (selectedObject.GetComponent<UIWidget>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUIWidget (inProgressObject, false);
				}

				if (selectedObject.GetComponent<UISprite>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUISprite (inProgressObject, false);
				}

				if (selectedObject.GetComponent<UILabel>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUILabel (inProgressObject, false);
				}

				if (selectedObject.GetComponent<UIButton>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUIButton (inProgressObject, false);
				}

				if (selectedObject.GetComponent<UIToggle>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUIToggle (inProgressObject, false);
				}

				//OnCleanConvertedItem(inProgressObject);

				UIWidget[] UIWidgetsOnChilderens = inProgressObject.GetComponentsInChildren<UIWidget>();
				UISprite[] UISpritesOnChilderens = inProgressObject.GetComponentsInChildren<UISprite>();
				UILabel[] UILablesOnChilderens = inProgressObject.GetComponentsInChildren<UILabel>();
				UIButton[] UIButtonsOnChilderens = inProgressObject.GetComponentsInChildren<UIButton>();
				UIToggle[] UITogglesOnChilderens = inProgressObject.GetComponentsInChildren<UIToggle>();

				for (int a=0; a<UIWidgetsOnChilderens.Length; a++){
					OnConvertUIWidget (UIWidgetsOnChilderens[a].gameObject, true);
				}

				for (int b=0; b<UISpritesOnChilderens.Length; b++){
					OnConvertUISprite (UISpritesOnChilderens[b].gameObject, true);
				}

				for (int c=0; c<UILablesOnChilderens.Length; c++){
					OnConvertUILabel (UILablesOnChilderens[c].gameObject, true);
				}

				for (int d=0; d<UIButtonsOnChilderens.Length; d++){
					OnConvertUIButton (UIButtonsOnChilderens[d].gameObject, true);
				}

				for (int e=0; e<UITogglesOnChilderens.Length; e++){
					OnConvertUIToggle (UITogglesOnChilderens[e].gameObject, true);
				}

				OnCleanConvertedItem(GameObject.FindObjectOfType<Canvas>().gameObject);
			}
		}else{
			Debug.LogError ("<Color=red>NO NGUI-Wedgits SELECTED</Color>, <Color=yellow>Please select at least one wedgit to convert</Color>");
		}
	}
	#endregion

	#region UIWidgets Converter
	static void OnConvertUIWidget(GameObject selectedObject, bool isSubConvert){
		GameObject tempObject;

		tempObject = selectedObject;

		tempObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (GameObject.FindObjectOfType<Canvas>()){
				tempObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyImmediate (tempObject.gameObject);
				return;
			}
		}

		tempObject.name = selectedObject.name;
		tempObject.transform.position = selectedObject.transform.position;

		RectTransform addedRectT;

		addedRectT = tempObject.AddComponent<RectTransform>();

		tempObject.GetComponent<RectTransform>().sizeDelta = tempObject.GetComponent<UIWidget>().localSize;
		tempObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}
	#endregion

	#region UISprites Converter
	static void OnConvertUISprite(GameObject selectedObject, bool isSubConvert){
		GameObject tempObject;

		UIAtlas tempNguiAtlas;
		tempNguiAtlas = selectedObject.GetComponent<UISprite>().atlas;
		if (File.Exists("Assets/CONVERSION_DATA/"+tempNguiAtlas.name+".png")){
			Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"CONVERSION_DATA\" </color>Directory");
		}else{
			ConvertAtlas(tempNguiAtlas);
		}

		tempObject = selectedObject;
		tempObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (GameObject.FindObjectOfType<Canvas>()){
				tempObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyImmediate (tempObject.gameObject);
				return;
			}
		}
		tempObject.name = selectedObject.name;
		tempObject.transform.position = selectedObject.transform.position;
		
		//to easliy control the old and the new sprites and buttons
		Image addedImage;
		UISprite originalSprite;

		//define the objects of the previous variables
		if (tempObject.GetComponent<Image>()){
			addedImage = tempObject.GetComponent<Image>();
		}else{
			addedImage = tempObject.AddComponent<Image>();
		}
		originalSprite = selectedObject.GetComponent<UISprite>();

		//adjust the rect transform to fit the original one's size
		tempObject.GetComponent<RectTransform>().sizeDelta = originalSprite.localSize;
		tempObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
		
		Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/CONVERSION_DATA/" + originalSprite.atlas.name + ".png").OfType<Sprite>().ToArray();
		for (int c=0; c<sprites.Length; c++){
			if (sprites[c].name == originalSprite.spriteName){
				addedImage.sprite = sprites[c];
			}

		}
		
		// set the image sprite color
		if (addedImage.gameObject.GetComponent<UIButton>()){
			addedImage.color = Color.white;
		}else{
			addedImage.color = originalSprite.color;
		}
		
		//set the type of the sprite (with a button it will be usually sliced)
		if (originalSprite.type == UIBasicSprite.Type.Simple){
			addedImage.type = Image.Type.Simple;
		}else if (originalSprite.type == UIBasicSprite.Type.Sliced){
			addedImage.type = Image.Type.Sliced;
		}else if (originalSprite.type == UIBasicSprite.Type.Tiled){
			addedImage.type = Image.Type.Tiled;
		}else if (originalSprite.type == UIBasicSprite.Type.Filled){
			addedImage.type = Image.Type.Filled;
		}
	}
	#endregion


	#region UILabels Converter
	static void OnConvertUILabel(GameObject selectedObject, bool isSubConvert){
		GameObject tempObject;
		Text tempText;

		tempObject = selectedObject;
		tempObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (GameObject.FindObjectOfType<Canvas>()){
				tempObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyImmediate (tempObject.gameObject);
				return;
			}
		}
		tempText = tempObject.AddComponent<Text>();
		tempObject.name = selectedObject.name;
		//to adjust the text issue
		if (tempObject.GetComponent <UILabel>().overflowMethod == UILabel.Overflow.ResizeHeight){
			tempObject.GetComponent<RectTransform>().pivot = new Vector2(tempObject.GetComponent<RectTransform>().pivot.x, 1.0f);
		}
		tempObject.transform.position = selectedObject.transform.position;
		tempObject.GetComponent<RectTransform>().sizeDelta = tempObject.GetComponent<UILabel>().localSize;
		tempObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);


		UILabel originalText = tempObject.GetComponent <UILabel>();
		//tempText = originalText.gameObject.AddComponent<Text>();
		tempText.text = originalText.text;
		tempText.color = originalText.color;
		tempText.gameObject.GetComponent<RectTransform>().sizeDelta = originalText.localSize;
		tempText.font = (Font)AssetDatabase.LoadAssetAtPath("Assets/CONVERSION_DATA/FONTS/"+"FONT.ttf", typeof(Font));
		tempText.fontSize = originalText.fontSize;
		if (originalText.spacingY != 0){
			tempText.lineSpacing = originalText.spacingY;
		}
		
		if (originalText.alignment == NGUIText.Alignment.Automatic){
			tempText.alignment = TextAnchor.MiddleCenter;
		}else if (originalText.alignment == NGUIText.Alignment.Center){
			tempText.alignment = TextAnchor.MiddleCenter;
		}else if (originalText.alignment == NGUIText.Alignment.Justified){
			tempText.alignment = TextAnchor.MiddleLeft;
		}else if (originalText.alignment == NGUIText.Alignment.Left){
			tempText.alignment = TextAnchor.UpperLeft;
		}else if (originalText.alignment == NGUIText.Alignment.Right){
			tempText.alignment = TextAnchor.UpperRight;
		}

	}
	#endregion

	#region UIButtons Converter
	static void OnConvertUIButton(GameObject selectedObject, bool isSubConvert){
		GameObject tempObject;

		/*
		UIAtlas tempNguiAtlas;
		tempNguiAtlas = selectedObject.GetComponent<UISprite>().atlas;
		if (File.Exists("Assets/CONVERSION_DATA/"+tempNguiAtlas.name+".png")){
			Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"CONVERSION_DATA\" </color>Directory");
		}else{
			ConvertAtlas(tempNguiAtlas);
		}
		*/
		tempObject = selectedObject;
		tempObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (GameObject.FindObjectOfType<Canvas>()){
				tempObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyImmediate (tempObject.gameObject);
				return;
			}
		}
		tempObject.transform.position = selectedObject.transform.position;
		
		//to easliy control the old and the new sprites and buttons
		Button addedButton;
		UIButton originalButton;
		
		//define the objects of the previous variables
		if (tempObject.GetComponent<Button>()){
			addedButton = tempObject.GetComponent<Button>();
		}else{
			addedButton = tempObject.AddComponent<Button>();
		}
		originalButton = selectedObject.GetComponent<UIButton>();
		
		//adjust the rect transform to fit the original one's size..If it have no sprite, then it must had a widget
		if (originalButton.GetComponent<UISprite>()){
			tempObject.GetComponent<RectTransform>().sizeDelta = originalButton.GetComponent<UISprite>().localSize;
		}else{
			tempObject.GetComponent<RectTransform>().sizeDelta = originalButton.GetComponent<UIWidget>().localSize;
		}
		tempObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

		//if the object ahve no UISprites, then a sub object must have!
		Sprite[] sprites;
		if (originalButton.GetComponent<UISprite>()){
			sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/CONVERSION_DATA/" + originalButton.GetComponent<UISprite>().atlas.name + ".png").OfType<Sprite>().ToArray();
		}else{
			sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/CONVERSION_DATA/" + originalButton.gameObject.GetComponentInChildren<UISprite>().atlas.name + ".png").OfType<Sprite>().ToArray();
		}

		SpriteState tempState = addedButton.spriteState;
		for (int c=0; c<sprites.Length; c++){
			//Apply the sprite swap option, just in case the user have it.
			// Used several If statement, just in case a user using the same sprite to define more than one state
			if (sprites[c].name == originalButton.hoverSprite){
				tempState.highlightedSprite = sprites[c];
			}
			if (sprites[c].name == originalButton.pressedSprite){
				tempState.pressedSprite = sprites[c];
			}
			if (sprites[c].name == originalButton.disabledSprite){
				tempState.disabledSprite = sprites[c];
			}
			addedButton.spriteState = tempState;
		}
		
		//set the button colors and the fade duration
		if (originalButton.GetComponent<UISprite>()){
			ColorBlock tempColor = addedButton.colors;
			tempColor.normalColor = originalButton.GetComponent<UISprite>().color;
			tempColor.highlightedColor = originalButton.hover;
			tempColor.pressedColor = originalButton.pressed;
			tempColor.disabledColor = originalButton.disabledColor;
			tempColor.fadeDuration = originalButton.duration;
			addedButton.colors = tempColor;
		}
		
		//if the button is using some sprites, then switch the transitons into the swap type. otherwise, keep it with the color tint!
		if (originalButton.hoverSprite != "" &&
		    originalButton.pressedSprite != "" &&
		    originalButton.disabledSprite != ""){
			//addedButton.transition = Selectable.Transition.SpriteSwap;
			addedButton.transition = Selectable.Transition.ColorTint;
		}else{
			addedButton.transition = Selectable.Transition.ColorTint;
		}
	}
	#endregion

	#region UIToggles Converter
	static void OnConvertUIToggle (GameObject selectedObject, bool isSubConvert){

	}
	#endregion

	#region Cleaner
	static void OnCleanConvertedItem (GameObject selectedObject){

		UIWidget[] UIWidgetsOnChilderens = selectedObject.GetComponentsInChildren<UIWidget>();
		UISprite[] UISpritesOnChilderens = selectedObject.GetComponentsInChildren<UISprite>();
		UILabel[] UILablesOnChilderens = selectedObject.GetComponentsInChildren<UILabel>();
		UIButton[] UIButtonsOnChilderens = selectedObject.GetComponentsInChildren<UIButton>();
		UIToggle[] UITogglesOnChilderens = selectedObject.GetComponentsInChildren<UIToggle>();

		Collider[] CollidersOnChilderens = selectedObject.GetComponentsInChildren<Collider>();

		for (int a=0; a<UIWidgetsOnChilderens.Length; a++){
			if (UIWidgetsOnChilderens[a]){
				DestroyImmediate (UIWidgetsOnChilderens[a]);
			}
		}
		
		for (int b=0; b<UISpritesOnChilderens.Length; b++){
			if (UISpritesOnChilderens[b]){
				DestroyImmediate (UISpritesOnChilderens[b]);
			}
		}
		
		for (int c=0; c<UILablesOnChilderens.Length; c++){
			if (UILablesOnChilderens[c]){
				DestroyImmediate (UILablesOnChilderens[c]);
			}
		}
		
		for (int d=0; d<UIButtonsOnChilderens.Length; d++){
			if (UIButtonsOnChilderens[d]){
				DestroyImmediate (UIButtonsOnChilderens[d]);
			}
		}
		
		for (int e=0; e<UITogglesOnChilderens.Length; e++){
			if(UITogglesOnChilderens[e]){
				DestroyImmediate (UITogglesOnChilderens[e]);
			}
		}

		for (int z=0; z<CollidersOnChilderens.Length; z++){
			if (CollidersOnChilderens[z]){
				DestroyImmediate (CollidersOnChilderens[z]);
			}
		}

	}
	#endregion

}

// when everything done, the canvas needs to be UnParented, and have the scale of 1, and finally moved to Zero to be viewed by the camera.
