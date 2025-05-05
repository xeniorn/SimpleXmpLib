using SE.Halligang.CsXmpToolkit;
using SimpleXmpLib.Exceptions;
using System;

namespace SimpleXmpLib;

public static class XmpHelper
{
    public static string GetUtcDateTimeXmpString(DateTime dateTime)
    {
        try
        {
            XmpUtils.ConvertFromDate(dateTime.ToUniversalTime(), out var xmpDateTime);
            return xmpDateTime;
        }
        catch (Exception ex)
        {
            throw new SimpleXmpLibCodeException($"Failed to convert datetime {dateTime} to a string", ex);
        }
        
    }

    public static DateTime AbsoluteTimeFromDateTimeXmpString(string dateTimeString)
    {
        try
        {
            XmpUtils.ConvertToDate(dateTimeString, out var xmpDateTime);
            return xmpDateTime.ToUniversalTime();
        }
        catch (Exception ex)
        {
            throw new SimpleXmpLibCodeException($"Failed to convert string {dateTimeString} to a DateTime", ex);
        }
    }

    public static DateTimeOffset ZonedTimeFromDateTimeXmpString(string dateTimeString)
    {
        try
        {
            XmpUtils.ConvertToDateTimeOffset(dateTimeString, out var xmpDateTime);
            return xmpDateTime;
        }
        catch (Exception ex)
        {
            throw new SimpleXmpLibCodeException($"Failed to convert string {dateTimeString} to a DateTimeOffset", ex);
        }
    }

    public static string GetZonedDateTimeXmpString(DateTimeOffset zonedDateTime)
    {
        try
        {
            XmpUtils.ConvertFromDate(zonedDateTime, out var xmpDateTime);
            return xmpDateTime;
        }
        catch (Exception ex)
        {
            throw new SimpleXmpLibCodeException($"Failed to convert datetime {zonedDateTime} to a string", ex);
        }
    }
}