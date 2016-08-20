using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System;

[Serializable]
public class Conversation
{
    [XmlAttribute("id")]
    public int id;

    [XmlElement("Dialogue")]
    public List<Dialogue> dialogue { get; set; }

    [XmlElement("InitialImages")]
    public InitialImages initialImages { get; set; }
}

[Serializable]
public class InitialImages
{
    [XmlAttribute("imageA")]
    public int imageA;

    [XmlAttribute("imageB")]
    public int imageB;
}

[Serializable]
public class Dialogue
{
    [XmlAttribute("id")]
    public int id;

    [XmlAttribute("name")]
    public string name;

    [XmlElement("image")]
    public CharacterImage image;

    [XmlElement("Speach")]
    public string speach;
}

[Serializable]
public class CharacterImage
{
    [XmlAttribute("imageIndex")]
    public int imageIndex;

    public string image;
}