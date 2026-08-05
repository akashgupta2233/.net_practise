using System;
using System.Collections.Generic;
using System.Linq;

public class TravelSummary
{
    public long lastEntryStation;
    public long lastExitStation;
    public long lastEntryTime;
    public long lastExitTime;
    public double totalFarePaid;
    public int totalTrips;
    public double averageFarePerTrip;
}

public class Commuter
{
    public int cardNumber;
    public string commuterName;
    public string commuterType;
    public TravelSummary travelSummary;
}

public class Station
{
    public int stationId;
    public string stationName;
    public int zone;
    public double latitude;
    public double longitude;
}

public interface MetroOperations
{
    void issueCard(int cardNumber, string commuterName, string commuterType);
    bool tapIn(int cardNumber, int stationId, long epochTime);
    bool tapOut(int cardNumber, int stationId, long epochTime);
    Commuter getCommuterInfo(int cardNumber);
    List<double> fareHistory(int cardNumber);
    Dictionary<string, double> getZoneWiseRevenue(long startTime, long endTime);
    List<string> getFrequentRoute(int cardNumber);
    double getDailyPassSavings(int cardNumber, long date);
}

public class MetroCardManager : MetroOperations
{
    private readonly Dictionary<int, Commuter> commuters =
        new Dictionary<int, Commuter>();

    private readonly Dictionary<int, Station> stations =
        new Dictionary<int, Station>();

    private readonly Dictionary<int, (int stationId, long time)> activeJourneys =
        new Dictionary<int, (int, long)>();

    private readonly Dictionary<int, List<double>> fareHistories =
        new Dictionary<int, List<double>>();

    private readonly Dictionary<int, Dictionary<string, int>> routeCount =
        new Dictionary<int, Dictionary<string, int>>();

    private readonly Dictionary<int, Dictionary<long, double>> dailyTotals =
        new Dictionary<int, Dictionary<long, double>>();

    private readonly List<(long time, string zonePair, double fare)> revenueLog =
        new List<(long, string, double)>();

    private readonly double baseFare;
    private readonly double perKmRate;
    private readonly double maxDailyCap;

    public MetroCardManager(
        List<Station> stationList,
        double baseFare,
        double perKmRate,
        double maxDailyCap)
    {
        foreach (var s in stationList)
            stations[s.stationId] = s;

        this.baseFare = baseFare;
        this.perKmRate = perKmRate;
        this.maxDailyCap = maxDailyCap;
    }

    public void issueCard(int cardNumber, string commuterName, string commuterType)
    {
        if (commuters.ContainsKey(cardNumber))
            return;

        commuters[cardNumber] = new Commuter
        {
            cardNumber = cardNumber,
            commuterName = commuterName,
            commuterType = commuterType,
            travelSummary = new TravelSummary()
        };

        fareHistories[cardNumber] = new List<double>();
        routeCount[cardNumber] = new Dictionary<string, int>();
        dailyTotals[cardNumber] = new Dictionary<long, double>();
    }

    public bool tapIn(int cardNumber, int stationId, long epochTime)
    {
        if (!commuters.ContainsKey(cardNumber) ||
            !stations.ContainsKey(stationId) ||
            activeJourneys.ContainsKey(cardNumber))
            return false;

        activeJourneys[cardNumber] = (stationId, epochTime);

        commuters[cardNumber].travelSummary.lastEntryStation = stationId;
        commuters[cardNumber].travelSummary.lastEntryTime = epochTime;

        return true;
    }

    public bool tapOut(int cardNumber, int stationId, long epochTime)
    {
        if (!commuters.ContainsKey(cardNumber) ||
            !stations.ContainsKey(stationId) ||
            !activeJourneys.ContainsKey(cardNumber))
            return false;

        var journey = activeJourneys[cardNumber];

        if (stationId == journey.stationId || epochTime <= journey.time)
            return false;

        Station start = stations[journey.stationId];
        Station end = stations[stationId];

        double distance = CalculateDistance(start, end);

        double fare;

        long durationMinutes =
            (epochTime - journey.time) / (1000 * 60);

        if (durationMinutes > 120)
            fare = baseFare * 3;
        else
            fare = baseFare + distance * perKmRate;

        fare *= GetDiscountFactor(
            commuters[cardNumber].commuterType);

        long dayKey = epochTime / 86400000L;

        if (!dailyTotals[cardNumber].ContainsKey(dayKey))
            dailyTotals[cardNumber][dayKey] = 0;

        double today = dailyTotals[cardNumber][dayKey];

        if (today >= maxDailyCap)
            fare = 0;
        else if (today + fare > maxDailyCap)
            fare = maxDailyCap - today;

        dailyTotals[cardNumber][dayKey] += fare;

        var ts = commuters[cardNumber].travelSummary;

        ts.lastExitStation = stationId;
        ts.lastExitTime = epochTime;
        ts.totalFarePaid += fare;
        ts.totalTrips++;
        ts.averageFarePerTrip =
            ts.totalFarePaid / ts.totalTrips;

        fareHistories[cardNumber].Add(fare);

        string route =
            start.stationName + " to " + end.stationName;

        if (!routeCount[cardNumber].ContainsKey(route))
            routeCount[cardNumber][route] = 0;

        routeCount[cardNumber][route]++;

        string zonePair =
            "Zone" + start.zone + "-Zone" + end.zone;

        revenueLog.Add((epochTime, zonePair, fare));

        activeJourneys.Remove(cardNumber);

        return true;
    }

    public Commuter getCommuterInfo(int cardNumber)
    {
        return commuters.ContainsKey(cardNumber)
            ? commuters[cardNumber]
            : null;
    }

    public List<double> fareHistory(int cardNumber)
    {
        if (!fareHistories.ContainsKey(cardNumber))
            return new List<double>();

        return fareHistories[cardNumber]
            .OrderByDescending(x => x)
            .Take(5)
            .ToList();
    }

    public Dictionary<string, double> getZoneWiseRevenue(
        long startTime,
        long endTime)
    {
        return revenueLog
            .Where(x => x.time >= startTime &&
                        x.time <= endTime)
            .GroupBy(x => x.zonePair)
            .Select(g => new
            {
                Key = g.Key,
                Value = g.Sum(x => x.fare)
            })
            .Where(x => x.Value > 0)
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public List<string> getFrequentRoute(int cardNumber)
    {
        if (!routeCount.ContainsKey(cardNumber))
            return new List<string>();

        return routeCount[cardNumber]
            .OrderByDescending(x => x.Value)
            .Take(3)
            .Select(x => x.Key)
            .ToList();
    }

    public double getDailyPassSavings(
        int cardNumber,
        long date)
    {
        if (!dailyTotals.ContainsKey(cardNumber) ||
            !dailyTotals[cardNumber].ContainsKey(date))
            return 0;

        double spent = dailyTotals[cardNumber][date];
        double passCost = maxDailyCap * 0.8;

        return Math.Max(0, spent - passCost);
    }

    private double GetDiscountFactor(string type)
    {
        switch (type)
        {
            case "SENIOR": return 0.50;
            case "STUDENT": return 0.75;
            case "CHILD": return 0.25;
            default: return 1.00;
        }
    }

    private double CalculateDistance(
        Station s1,
        Station s2)
    {
        double lat1 = Math.PI * s1.latitude / 180.0;
        double lon1 = Math.PI * s1.longitude / 180.0;
        double lat2 = Math.PI * s2.latitude / 180.0;
        double lon2 = Math.PI * s2.longitude / 180.0;

        double dlat = lat2 - lat1;
        double dlon = lon2 - lon1;

        double a =
            Math.Pow(Math.Sin(dlat / 2), 2) +
            Math.Cos(lat1) * Math.Cos(lat2) *
            Math.Pow(Math.Sin(dlon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));

        return 6371 * c;
    }
}