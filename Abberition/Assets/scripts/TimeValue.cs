using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TimeValue
{
    public float value;
    public TimeType type;

    public float GetSeconds ()
    {
        if (type == TimeType.seconds) return value;
        else
        {
            var seconds = value * 60;
            if (type == TimeType.minutes) return seconds;
            else
            {
                seconds *= 60;
                if (type == TimeType.hours) return seconds;
                else
                {
                    seconds *= 24;
                    if (type == TimeType.days) return seconds;
                    else
                    {
                        seconds *= 365;
                        return seconds;
                    }
                }
            }
        }
    }

    //public float SecondsToUnit (float seconds)
    //{
    //    if (type == TimeType.seconds) return seconds;
    //    else
    //    {
    //        var value = seconds / 60;
    //        if (type == TimeType.minutes) return value;
    //        else
    //        {
    //            value /= 60;
    //            if (type == TimeType.hours) return value;
    //            else
    //            {
    //                value /= 24;
    //                if (type == TimeType.days) return value;
    //                else
    //                {
    //                    value /= 365;
    //                    return value;
    //                }
    //            }
    //        }
    //    }
    //}

    //public float SecondsToValue (float seconds)
    //{
    //    return SecondsToValue(seconds);
    //}
}

public enum TimeType
{ 
    seconds, minutes, hours, days, years
}