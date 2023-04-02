using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HeroesOnGridDataListWrapper
{
    public List<HeroesOnGridData> list;
}

[System.Serializable]
public struct HeroesCounterListWraper
{
    public List<HeroCounter> list;
}

public static class JsonUtilityHelper
{
    // Heroes on grid
    public static string SerializeHeroesOnGridDataArray(HeroesOnGridData[] array)
    {
        HeroesOnGridDataListWrapper wrapper = new HeroesOnGridDataListWrapper();
        wrapper.list = new List<HeroesOnGridData>(array);
        return JsonUtility.ToJson(wrapper);
    }

    public static HeroesOnGridData[] DeserializeHeroesOnGridDataArray(string json)
    {
        HeroesOnGridDataListWrapper wrapper = JsonUtility.FromJson<HeroesOnGridDataListWrapper>(json);
        return wrapper.list.ToArray();
    }

    //Heroes counter

    public static string SerializerHeroesCounterArray(HeroCounter[] arry)
    {
        HeroesCounterListWraper wrapper = new HeroesCounterListWraper();
        wrapper.list = new List<HeroCounter>(arry);
        return JsonUtility.ToJson(wrapper);
    }

    public static HeroCounter[] DeserializeHeroesCouner(string json)
    {
        HeroesCounterListWraper wrapper = JsonUtility.FromJson<HeroesCounterListWraper>(json);
        return wrapper.list.ToArray();
    }
}
