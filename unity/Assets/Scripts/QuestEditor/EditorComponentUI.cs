﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Assets.Scripts.Content;
using Assets.Scripts.UI;

public class EditorComponentUI : EditorComponentEvent
{
    QuestData.UI uiComponent;
    EditorSelectionList imageList;
    DialogBoxEditable locXDBE;
    DialogBoxEditable locYDBE;
    DialogBoxEditable sizeDBE;
    DialogBoxEditable aspectDBE;
    PaneledDialogBoxEditable textDBE;
    DialogBoxEditable textSizeDBE;
    EditorSelectionList colorList;

    private readonly StringKey SELECT_IMAGE = new StringKey("val", "SELECT_IMAGE");

    public EditorComponentUI(string nameIn) : base(nameIn)
    {
    }

    override public float AddPosition(float offset)
    {
        return offset;
    }

    override public void Highlight()
    {
    }

    override public float AddSubEventComponents(float offset)
    {
        uiComponent = component as QuestData.UI;

        UIElement ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(0, offset, 4.5f, 1);
        ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "IMAGE")));

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(4.5f, offset, 15, 1);
        ui.SetText(uiComponent.imageName);
        ui.SetButton(delegate { SetImage(); });
        new UIElementBorder(ui);
        offset += 2;

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(0, offset, 6, 1);
        ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "UNITS")));

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(6, offset, 6, 1);
        ui.SetButton(delegate { ChangeUnits(); });
        new UIElementBorder(ui);
        if (uiComponent.verticalUnits)
        {
            ui.SetText(new StringKey("val", "VERTICAL"));
        }
        else
        {
            ui.SetText(new StringKey("val", "HORIZONTAL"));
        }
        offset += 2;

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(0, offset++, 4, 1);
        ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "ALIGN")));

        DrawAlignSelection(offset, -1, -1, "┏");
        DrawAlignSelection(offset, 0, -1, "━");
        DrawAlignSelection(offset, 1, -1, "┓");

        DrawAlignSelection(offset, -1, 0, "┃");
        DrawAlignSelection(offset, 0, 0, "╋");
        DrawAlignSelection(offset, 1, 0, "┃");

        DrawAlignSelection(offset, -1, 1, "┗");
        DrawAlignSelection(offset, 0, 1, "━");
        DrawAlignSelection(offset, 1, 1, "┛");
        offset += 3;

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(0, offset++, 10, 1);
        ui.SetText(new StringKey("val", "POSITION"));

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(0, offset, 2, 1);
        ui.SetText("X:");

        locXDBE = new DialogBoxEditable(new Vector2(2, offset), new Vector2(3, 1),
            uiComponent.location.x.ToString(), false, delegate { UpdateNumbers(); });
        locXDBE.background.transform.SetParent(scrollArea.transform);
        locXDBE.ApplyTag(Game.EDITOR);
        locXDBE.AddBorder();

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(5, offset, 2, 1);
        ui.SetText("Y:");

        locYDBE = new DialogBoxEditable(new Vector2(7, offset), new Vector2(3, 1),
            uiComponent.location.y.ToString(), false, delegate { UpdateNumbers(); });
        locYDBE.background.transform.SetParent(scrollArea.transform);
        locYDBE.ApplyTag(Game.EDITOR);
        locYDBE.AddBorder();
        offset += 2;

        ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(0, offset, 5, 1);
        ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "SIZE")));

        sizeDBE = new DialogBoxEditable(new Vector2(5, offset), new Vector2(3, 1),
            uiComponent.size.ToString(), false, delegate { UpdateNumbers(); });
        sizeDBE.background.transform.SetParent(scrollArea.transform);
        sizeDBE.ApplyTag(Game.EDITOR);
        sizeDBE.AddBorder();

        if (uiComponent.imageName.Length == 0)
        {
            ui = new UIElement(Game.EDITOR, scrollArea.transform);
            ui.SetLocation(10, offset, 5, 1);
            ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "ASPECT")));

            aspectDBE = new DialogBoxEditable(new Vector2(15, offset), new Vector2(3, 1),
                uiComponent.aspect.ToString(), false, delegate { UpdateNumbers(); });
            aspectDBE.background.transform.SetParent(scrollArea.transform);
            aspectDBE.ApplyTag(Game.EDITOR);
            aspectDBE.AddBorder();
            offset += 2;

            textDBE = new PaneledDialogBoxEditable(
                new Vector2(0.5f, offset), new Vector2(19, 8),
                uiComponent.uiText.Translate(true),
                delegate { UpdateUIText(); });
            textDBE.background.transform.SetParent(scrollArea.transform);
            textDBE.ApplyTag(Game.EDITOR);
            textDBE.AddBorder();
            offset += 9;

            ui = new UIElement(Game.EDITOR, scrollArea.transform);
            ui.SetLocation(0, offset, 7, 1);
            ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "TEXT_SIZE")));

            textSizeDBE = new DialogBoxEditable(new Vector2(7, offset), new Vector2(3, 1),
                uiComponent.textSize.ToString(), false, delegate { UpdateTextSize(); });
            textSizeDBE.background.transform.SetParent(scrollArea.transform);
            textSizeDBE.ApplyTag(Game.EDITOR);
            textSizeDBE.AddBorder();

            ui = new UIElement(Game.EDITOR, scrollArea.transform);
            ui.SetLocation(10, offset, 4.5f, 1);
            ui.SetText(new StringKey("val", "X_COLON", new StringKey("val", "COLOR")));

            ui = new UIElement(Game.EDITOR, scrollArea.transform);
            ui.SetLocation(14.5f, offset, 5, 1);
            ui.SetText(uiComponent.textColor);
            ui.SetButton(delegate { SetColour(); });
            new UIElementBorder(ui);
            offset += 2;

            ui = new UIElement(Game.EDITOR, scrollArea.transform);
            ui.SetLocation(0.5f, offset, 8, 1);
            ui.SetText(uiComponent.textColor);
            ui.SetButton(delegate { ToggleBorder(); });
            new UIElementBorder(ui);
            if (uiComponent.border)
            {
                ui.SetText(new StringKey("val", "BORDER"));
            }
            else
            {
                ui.SetText(new StringKey("val", "NO_BORDER"));
            }
        }
        offset += 2;

        DrawUIComponent();

        return offset;
    }

    public void DrawAlignSelection(float offset, int x, int y, string label)
    {
        Color selected = (uiComponent.hAlign == x && uiComponent.vAlign == y) ? Color.white : new Color(0.3f, 0.3f, 0.3f);
        UIElement ui = new UIElement(Game.EDITOR, scrollArea.transform);
        ui.SetLocation(5 + x, offset + y, 1, 1);
        ui.SetText(label, selected);
        ui.SetButton(delegate { SetAlign(x, y); });
        new UIElementBorder(ui, selected);
    }

    public void DrawUIComponent()
    {
        game.quest.ChangeAlpha(uiComponent.sectionName, 1f);

        // Create a grey zone outside of the 16x9 boundary
        // Find quest UI panel
        GameObject panel = GameObject.Find("QuestUIPanel");
        if (panel == null)
        {
            // Create UI Panel
            panel = new GameObject("QuestUIPanel");
            panel.tag = Game.BOARD;
            panel.transform.SetParent(game.uICanvas.transform);
            panel.transform.SetAsFirstSibling();
            panel.AddComponent<RectTransform>();
            panel.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, Screen.height);
            panel.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, Screen.width);
        }

        // Create objects
        GameObject unityObject = new GameObject("greyzonea");
        unityObject.tag = Game.EDITOR;
        unityObject.transform.SetParent(panel.transform);
        UnityEngine.UI.Image panela = unityObject.AddComponent<UnityEngine.UI.Image>();
        panela.color = new Color(1f, 1f, 1f, 0.3f);
        unityObject = new GameObject("greyzoneb");
        unityObject.tag = Game.EDITOR;
        unityObject.transform.SetParent(panel.transform);
        UnityEngine.UI.Image panelb = unityObject.AddComponent<UnityEngine.UI.Image>();
        panelb.color = new Color(1f, 1f, 1f, 0.3f);
        panelb.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        panelb.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);

        if (uiComponent.verticalUnits)
        {
            // Size bars for wider screens
            // Position and Scale assume a 16x9 aspect
            float templateWidth = (float)Screen.height * 16f / 10f;
            float hOffset = (float)Screen.width - templateWidth;
            panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, Screen.height);

            if (uiComponent.hAlign < 0)
            {
                panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, hOffset);
            }
            else if (uiComponent.hAlign > 0)
            {
                panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, hOffset);
            }
            else
            {
                panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, hOffset / 2);
                panelb.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, hOffset / 2);
                panelb.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, Screen.height);
            }
        }
        else
        {
            // letterboxing for taller screens
            // Position and Scale assume a 16x9 aspect
            float templateHeight = (float)Screen.width * 9f / 16f;
            float vOffset = (float)Screen.height - templateHeight;
            panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, Screen.width);

            if (uiComponent.vAlign < 0)
            {
                panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, vOffset);
            }
            else if (uiComponent.vAlign > 0)
            {
                panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, vOffset);
            }
            else
            {
                panela.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, vOffset / 2);
                panelb.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, vOffset / 2);
                panelb.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, Screen.width);
            }
        }
    }

    override public float AddEventTrigger(float offset)
    {
        return offset;
    }

    public void SetImage()
    {
        string relativePath = new FileInfo(Path.GetDirectoryName(Game.Get().quest.qd.questPath)).FullName;
        List<EditorSelectionList.SelectionListEntry> list = new List<EditorSelectionList.SelectionListEntry>();
        list.Add(new EditorSelectionList.SelectionListEntry(""));
        foreach (string s in Directory.GetFiles(relativePath, "*.png", SearchOption.AllDirectories))
        {
            list.Add(new EditorSelectionList.SelectionListEntry(s.Substring(relativePath.Length + 1), "File"));
        }
        foreach (string s in Directory.GetFiles(relativePath, "*.jpg", SearchOption.AllDirectories))
        {
            list.Add(new EditorSelectionList.SelectionListEntry(s.Substring(relativePath.Length + 1), "File"));
        }
        foreach (KeyValuePair<string, ImageData> kv in Game.Get().cd.images)
        {
            list.Add(new EditorSelectionList.SelectionListEntry(kv.Key, "FFG"));
        }
        imageList = new EditorSelectionList(SELECT_IMAGE, list, delegate { SelectImage(); });
        imageList.SelectItem();
    }

    public void SelectImage()
    {
        uiComponent.imageName = imageList.selection;
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        if (uiComponent.imageName.Length > 0)
        {
            LocalizationRead.scenarioDict.Remove(uiComponent.uitext_key);
            uiComponent.border = false;
            uiComponent.aspect = 1;
        }
        else
        {
            LocalizationRead.updateScenarioText(uiComponent.uitext_key, "");
        }
        Update();
    }

    public void ChangeUnits()
    {
        uiComponent.verticalUnits = !uiComponent.verticalUnits;
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        Update();
    }

    public void SetAlign(int x, int y)
    {
        uiComponent.hAlign = x;
        uiComponent.vAlign = y;
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        Update();
    }

    public void UpdateNumbers()
    {
        if (!locXDBE.Text.Equals(""))
        {
            float.TryParse(locXDBE.Text, out uiComponent.location.x);
        }
        if (!locYDBE.Text.Equals(""))
        {
            float.TryParse(locYDBE.Text, out uiComponent.location.y);
        }
        if (!sizeDBE.Text.Equals(""))
        {
            float.TryParse(sizeDBE.Text, out uiComponent.size);
        }
        if (aspectDBE != null && !aspectDBE.Text.Equals(""))
        {
            float.TryParse(aspectDBE.Text, out uiComponent.aspect);
        }
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        Update();
    }

    public void UpdateUIText()
    {
        Game game = Game.Get();

        if (textDBE.CheckTextChangedAndNotEmpty())
        {
            LocalizationRead.updateScenarioText(uiComponent.uitext_key, textDBE.Text);
        }
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        Update();
    }

    public void UpdateTextSize()
    {
        float.TryParse(textSizeDBE.Text, out uiComponent.textSize);
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        Update();
    }

    public void SetColour()
    {
        List<EditorSelectionList.SelectionListEntry> colours = new List<EditorSelectionList.SelectionListEntry>();
        foreach (KeyValuePair<string, string> kv in ColorUtil.LookUp())
        {
            colours.Add(EditorSelectionList.SelectionListEntry.BuildNameKeyItem(kv.Key));
        }
        colorList = new EditorSelectionList(CommonStringKeys.SELECT_ITEM, colours, delegate { SelectColour(); });
        colorList.SelectItem();
    }

    public void SelectColour()
    {
        uiComponent.textColor = colorList.selection;
        Game.Get().quest.Remove(uiComponent.sectionName);
        Game.Get().quest.Add(uiComponent.sectionName);
        Update();
    }

    public void ToggleBorder()
    {
        uiComponent.border = !uiComponent.border;
        Update();
    }
}
