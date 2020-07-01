using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;

public class ScoreDisplayer : MonoBehaviour
{
    public GameObject canvas;
    public Score scr;
    public bool wasExecuted = false;
    public bool faseActive = true;
    string JsonScores = string.Empty;
    public string path = string.Empty;
    public bool isRefresh = false;

    private void Start()
    {
        try
        {
            path = Application.dataPath + "/StreamingAssets/JsonChallenge.json";
            ShowScore();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void CommenceDelete()
    {
        isRefresh = true;
        Debug.Log("isRefresh: " + isRefresh);
    }

    public void Update()
    {
        if(isRefresh)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in canvas.transform)
            {
                children.Add(child.gameObject);
            }
            children.Where(c => c.name != "btnRefrescar").ToList().ForEach(child => Destroy(child));
            isRefresh = false;
        }
        else if(!isRefresh && canvas.transform.childCount <= 1)
        {
            ShowScore();
        }
    }

    public void ShowScore()
    {
        try
        {
            //Transform tempTransform = canvas.transform;
            JsonScores = File.ReadAllText(path);
            scr = JsonConvert.DeserializeObject<Score>(JsonScores);

            float columnXd = (610 / (scr.ColumnHeaders.Length + 1)) - 297;
            float yBase = (380 / (scr.Data.Length + 2));
            float columnYd = yBase + 100;
            //- 252f
            int k = 0;
            int i = 0;
            string parameterName = string.Empty;
            GameObject go = new GameObject("Title");
            GameObject tempGo = go;
            go.transform.parent = canvas.gameObject.transform;

            go.AddComponent<Text>();
            go.GetComponent<Text>().text = scr.Title;
            go.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            go.GetComponent<Text>().color = Color.black;
            go.GetComponent<Text>().fontStyle = FontStyle.Bold;
            go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150f);

            go.transform.localPosition = new Vector3(5f, columnYd + (yBase / 3), 0f);

            do
            {
                if (faseActive)
                {
                    if (tempGo.transform.localPosition.y > 160)
                    {
                        columnYd = yBase;
                    }
                    else
                    {
                        columnYd = yBase + 100;
                    }

                    go = new GameObject("Header_" + k);
                    go.transform.parent = canvas.gameObject.transform;
                    go.AddComponent<Text>();
                    parameterName = scr.ColumnHeaders[k];
                    go.GetComponent<Text>().text = parameterName;
                    go.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    go.GetComponent<Text>().color = Color.black;
                    go.GetComponent<Text>().fontStyle = FontStyle.Bold;
                    go.transform.localPosition = new Vector3(columnXd, columnYd, 0f);
                    faseActive = false;
                }

                if (i <= scr.Data.Length - 1)
                {
                    columnYd -= yBase;
                    GameObject go_Data = new GameObject("DataText_" + k + "_" + i);
                    go_Data.transform.parent = canvas.gameObject.transform;
                    go_Data.AddComponent<Text>();

                    switch (parameterName)
                    {
                        case "ID":
                            go_Data.GetComponent<Text>().text = Convert.ToString(scr.Data[i].ID);
                            break;
                        case "Name":
                            go_Data.GetComponent<Text>().text = Convert.ToString(scr.Data[i].Name);
                            break;
                        case "Role":
                            go_Data.GetComponent<Text>().text = Convert.ToString(scr.Data[i].Role);
                            break;
                        case "Nickname":
                            go_Data.GetComponent<Text>().text = Convert.ToString(scr.Data[i].Nickname);
                            break;
                        default:
                            break;
                    }

                    go_Data.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    go_Data.GetComponent<Text>().color = Color.black;
                    go_Data.transform.localPosition = new Vector3(columnXd, columnYd, 0f);
                    i++;
                }
                else
                {
                    if (k < scr.ColumnHeaders.Length - 1)
                    {
                        k++;
                        faseActive = true;
                        columnXd += 126f;
                    }
                    else
                    {
                        wasExecuted = !wasExecuted;
                    }
                    i = 0;

                    if (tempGo.transform.localPosition.y > 160)
                    {
                        columnYd = yBase;
                    }
                    else
                    {
                        columnYd = yBase + 100;
                    }

                    //columnXd = (610 / (scr.ColumnHeaders.Length + 1)) - 297;
                }

            }
            while (!wasExecuted);

            if (tempGo.transform.localPosition.y > 160)
            {
                columnYd = yBase;
            }
            else
            {
                columnYd = yBase + 100;
            }
            tempGo.transform.localPosition = new Vector3(5f, columnYd + (yBase / 3), 0f);
            faseActive = true;
            wasExecuted = !wasExecuted;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}
