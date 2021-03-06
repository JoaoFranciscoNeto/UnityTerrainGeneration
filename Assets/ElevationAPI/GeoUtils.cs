﻿using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeoUtils
{

    public static UTMCoords ConvertToUTM(GeoCoords geoCoords)
    {
        int zone = GetZone(geoCoords.latitude, geoCoords.longitude);
        string band = GetBand(geoCoords.latitude);
        //Transform to UTM
        CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
        ICoordinateSystem wgs84geo = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
        ICoordinateSystem utm = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WGS84_UTM(zone, geoCoords.latitude > 0);
        ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84geo, utm);
        double[] pUtm = trans.MathTransform.Transform(new double[] { geoCoords.longitude, geoCoords.latitude });

        double easting = pUtm[0];
        double northing = pUtm[1];
        return new UTMCoords(zone, band, easting, northing);
    }

    public static GeoCoords ConvertToGeo(UTMCoords utmCoords)
    {
        int zone = utmCoords.zone;
        //Transform to latlong
        CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
        ICoordinateSystem wgs84geo = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
        ICoordinateSystem utm = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WGS84_UTM(zone, utmCoords.isNorthernHemisphere());
        ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(utm, wgs84geo);
        double[] pUtm = trans.MathTransform.Transform(new double[] { utmCoords.easting, utmCoords.northing });
        return new GeoCoords(pUtm[1], pUtm[0]);
    }

    private static int GetZone(double latitude, double longitude)
    {
        // Norway
        if (latitude >= 56 && latitude < 64 && longitude >= 3 && longitude < 13)
            return 32;

        // Spitsbergen
        if (latitude >= 72 && latitude < 84)
        {
            if (longitude >= 0 && longitude < 9)
                return 31;
            else if (longitude >= 9 && longitude < 21)
                return 33;
            if (longitude >= 21 && longitude < 33)
                return 35;
            if (longitude >= 33 && longitude < 42)
                return 37;
        }

        return (int)Mathf.Ceil(((float)longitude + 180f) / 6f);
    }
    private static string GetBand(double latitude)
    {
        if (latitude <= 84 && latitude >= 72)
            return "X";
        else if (latitude < 72 && latitude >= 64)
            return "W";
        else if (latitude < 64 && latitude >= 56)
            return "V";
        else if (latitude < 56 && latitude >= 48)
            return "U";
        else if (latitude < 48 && latitude >= 40)
            return "T";
        else if (latitude < 40 && latitude >= 32)
            return "S";
        else if (latitude < 32 && latitude >= 24)
            return "R";
        else if (latitude < 24 && latitude >= 16)
            return "Q";
        else if (latitude < 16 && latitude >= 8)
            return "P";
        else if (latitude < 8 && latitude >= 0)
            return "N";
        else if (latitude < 0 && latitude >= -8)
            return "M";
        else if (latitude < -8 && latitude >= -16)
            return "L";
        else if (latitude < -16 && latitude >= -24)
            return "K";
        else if (latitude < -24 && latitude >= -32)
            return "J";
        else if (latitude < -32 && latitude >= -40)
            return "H";
        else if (latitude < -40 && latitude >= -48)
            return "G";
        else if (latitude < -48 && latitude >= -56)
            return "F";
        else if (latitude < -56 && latitude >= -64)
            return "E";
        else if (latitude < -64 && latitude >= -72)
            return "D";
        else if (latitude < -72 && latitude >= -80)
            return "C";
        else
            return null;
    }
}



public class UTMCoords
{
    public int zone;
    public string band;
    public double easting;
    public double northing;

    public UTMCoords(int zone, string band, double easting, double northing)
    {
        this.zone = zone;
        this.band = band;
        this.easting = easting;
        this.northing = northing;
    }

    public UTMCoords(UTMCoords zoneband, double easting, double northing)
    {
        this.zone = zoneband.zone;
        this.band = zoneband.band;
        this.easting = easting;
        this.northing = northing;
    }

    public static float Distance(UTMCoords c1, UTMCoords c2)
    {
        return Vector2.Distance(new Vector2((float)c1.easting, (float)c1.northing), new Vector2((float)c2.easting, (float)c2.northing));
    }

    public override string ToString()
    {
        return (zone + " " + band + " " + easting + " " + northing);
    }

    public bool isNorthernHemisphere()
    {
        return string.Compare(band, "N") >= 0;
    }
}

public class GeoCoords
{
    public double latitude;
    public double longitude;

    public GeoCoords(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public GeoCoords(Vector2 coords)
    {
        latitude = coords.x;
        longitude = coords.y;
    }
}

public class GeoArea
{
    // Corners of the region, CCW, from SW
    public UTMCoords sw;
    public UTMCoords se;
    public UTMCoords ne;
    public UTMCoords nw;

    public double width;
    public double length;

    public double area;

    public Vector2 center;

    public GeoArea(UTMCoords SWcorner, UTMCoords NEcorner)
    {
        if (SWcorner.zone != NEcorner.zone || SWcorner.band != NEcorner.band)
            Debug.LogError("Corners of the area must be in the same zone and band");

        sw = new UTMCoords(SWcorner.zone, SWcorner.band, SWcorner.easting, SWcorner.northing);
        se = new UTMCoords(SWcorner.zone, SWcorner.band, NEcorner.easting, SWcorner.northing);
        ne = new UTMCoords(SWcorner.zone, SWcorner.band, NEcorner.easting, NEcorner.northing);
        nw = new UTMCoords(SWcorner.zone, SWcorner.band, SWcorner.easting, NEcorner.northing);

        width = UTMCoords.Distance(sw, se);
        length = UTMCoords.Distance(sw, nw);

        area = width * length;

        center = new Vector2((float)(sw.easting + width / 2f), (float)(sw.northing + length / 2f));
    }

    public List<GeoArea> SubdivideArea(int cols, int rows)
    {
        List<GeoArea> newAreas = new List<GeoArea>();

        double cellWidth = width / cols;
        double cellLength = length / rows;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                newAreas.Add(new GeoArea(
                    new UTMCoords(sw,
                    sw.easting + x * cellWidth,
                    sw.northing + y * cellLength),
                    new UTMCoords(sw,
                    sw.easting + (x+1) * cellWidth,
                    sw.northing + (y+1) * cellLength)
                    ));
            }
        }

        return newAreas;
    }

    public override string ToString()
    {
        return ("Area width " + width + " length " + length + "\tSW " + sw + "\tNE" + ne);
    }


}