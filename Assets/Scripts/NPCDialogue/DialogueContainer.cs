using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System;

[XmlRoot("DialogueHolder")]
public class DialogueContainer
{
    [XmlArray("Dialogues")]
    [XmlArrayItem("Conversation")]
    public List<Conversation> conversations = new List<Conversation>();

    public static DialogueContainer Load(string path)
    {
        XmlSerializer serialiser = new XmlSerializer(typeof(DialogueContainer));

        FileStream reader = new FileStream(path, FileMode.Open);

        DialogueContainer items = serialiser.Deserialize(reader) as DialogueContainer;

        reader.Close();

        return items;
    }


    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static DialogueContainer LoadFromText(string text)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueContainer));
        return serializer.Deserialize(new StringReader(text)) as DialogueContainer;
    }
}