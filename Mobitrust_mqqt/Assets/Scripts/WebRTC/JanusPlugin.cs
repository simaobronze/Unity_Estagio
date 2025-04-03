using System;
using System.Collections;
using UnityEngine;

public abstract class JanusPlugin
{
    public long id;
    public string name;

    public static JanusPlugin From(string plugin, long id, Janus janus)
    {
        switch (plugin)
        {
            case "janus.plugin.streaming":
                return new StreamingPlugin(id, janus);
            case "janus.plugin.videoroom":
                return new VideoroomPlugin(id);
            default:
                throw new InvalidOperationException("Plugin not supported/invalid.");
        }
    }



}
