using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System;

public class TextWriter : MonoBehaviour
{
    private static TextWriter Instance;
    private List<TextWriterSingle> m_textWriterSingleList;

    private void Awake()
    {
        Instance = this;
        m_textWriterSingleList = new List<TextWriterSingle>();
    }

    //TO USE CEB:
    public static void AddWriter_Static(TextMeshPro uiText, TextWritterLinesRefSO linesToWriteRefSO)
    {
        Instance.RemoveWriter(uiText);
        Instance.AddWriter(uiText, linesToWriteRefSO.textWriterLines);
    }
    
    //TO USE CEB:
    public static bool IsWriterDone_Static(TextMeshPro uiText)
    {
        return Instance.IsWriterDone(uiText);
    }

    private bool IsWriterDone(TextMeshPro uiText)
    {
        foreach(TextWriterSingle textWriterSingle in m_textWriterSingleList.ToList<TextWriterSingle>())
        {
            if(textWriterSingle.GetUIText() == uiText)
            {
                return false;
            }
        }
        return true;
    }

    public void AddWriter(TextMeshPro uiText, List<TextWriterLine> linesToWrite)
    {
        m_textWriterSingleList.Add(new TextWriterSingle(uiText, linesToWrite));
    }

    private void RemoveWriter(TextMeshPro uiText)
    {
        foreach(TextWriterSingle textWriterSingle in m_textWriterSingleList.ToList<TextWriterSingle>())
        {
            if(textWriterSingle.GetUIText() == uiText)
            {
                m_textWriterSingleList.Remove(textWriterSingle);
            }
        }
    }

    private void Update()
    {
        foreach(TextWriterSingle textWriterSingle in m_textWriterSingleList.ToList<TextWriterSingle>())
        {
            if(textWriterSingle.Update())
            {
                m_textWriterSingleList.Remove(textWriterSingle);
            }
        }
    }

    [Serializable]
    public class TextWriterLine
    {
        public string line;
        public float timePerChar;
        public bool addNewLineAfter;
    }

    public class TextWriterSingle
    {
        private TextMeshPro m_uiText;
        private string m_textToWrite;
        private int m_charIndex;
        private List<CharLinesAndSpeed> m_timerPerCharVect;
        private float m_timer;
        private int m_lineIdx = 0;
        private int m_charFromPreviousLines = 0;
        public TextWriterSingle(TextMeshPro uiText, List<TextWriterLine> linesToWrite)
        {
            m_uiText = uiText;
            m_charIndex = 0;
            m_timerPerCharVect = new List<CharLinesAndSpeed>();
            int lineIdx = 0;
            foreach(TextWriterLine lineString in linesToWrite)
            {
                m_textToWrite += lineString.line;
                int lengthToAdd = lineString.line.Length;
                if(lineString.addNewLineAfter)
                {
                    if(linesToWrite.Count <= lineIdx+1)
                    {
                        Debug.LogError("Le dernier element de la liste 'Lines' ne doit jamais ajouter une nouvelle ligne.");
                    }
                    m_textToWrite+= "\n";
                    lengthToAdd++;
                }
                m_timerPerCharVect.Add(new CharLinesAndSpeed(lineString.timePerChar, lengthToAdd));
                lineIdx++;
            }
        }

        public bool Update()
        {
            if(m_uiText != null)
            {
                m_timer -= Time.deltaTime;
                //var listOfTimerPerChar = m_timerPerCharVect.ToList();
                while(m_timer <= 0f)
                {
                    m_timer += m_timerPerCharVect[m_lineIdx].speed;

                    m_charIndex++;

                    if(m_timerPerCharVect.Count > 1)
                    {
                        if(m_charIndex + 1 >= m_charFromPreviousLines + m_timerPerCharVect[m_lineIdx].idxCharToNextLine)
                        {
                            m_charFromPreviousLines += m_timerPerCharVect[m_lineIdx].idxCharToNextLine;
                            if(m_lineIdx < m_timerPerCharVect.Count - 1)
                            {
                                m_lineIdx++;
                            }
                        }
                    }

                    if(m_textToWrite[m_charIndex-1] == '\n')
                    {
                        m_charIndex++;
                    }
                    if(m_textToWrite[m_charIndex-1] == '<')
                    {
                        while(m_textToWrite[m_charIndex-1] != '>' && m_charIndex + 1 <= m_textToWrite.Length)
                        {
                            m_charIndex++;
                        }
                        if(m_charIndex >= m_textToWrite.Length)
                        {
                            //Entire string displayed;
                            if(m_textToWrite[m_charIndex-1] != '>')
                            {
                                Debug.LogError("Le caractère '<' a été entré sans que le caractère '>' ne le suive plus loin");
                                m_uiText = null;
                                return true;
                            }
                            else
                            {
                                m_uiText.text = m_textToWrite;                        
                                m_uiText = null;
                                return true;
                            }
                        }
                    }
                    if(m_textToWrite[m_charIndex-1] == '>')
                    {
                        m_charIndex++;
                    }
                    string text = m_textToWrite.Substring(0, m_charIndex);
                    text += "<color=#000000>" + m_textToWrite.Substring(m_charIndex) + "</color>";
                    m_uiText.text = text;
                    if(m_charIndex >= m_textToWrite.Length)
                    {
                        //Entire string displayed;
                        m_uiText = null;
                        return true;
                    }
                }
            }
            return false;
        }

        public TextMeshPro GetUIText()
        {
            return m_uiText;
        }

        private class CharLinesAndSpeed
        {
            public float speed;
            public int idxCharToNextLine;

            public CharLinesAndSpeed(float speed, int idxCharToNextLine)
            {
                this.speed = speed;
                this.idxCharToNextLine = idxCharToNextLine;
            }
        }
    }
}
