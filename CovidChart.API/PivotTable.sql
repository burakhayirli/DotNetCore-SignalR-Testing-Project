SELECT CovidDate,[1],[2],[3],[4],[5]
FROM (SELECT [City],[Count],CAST([CovidDate] as DATE) as CovidDate  FROM Covids) as covidT
PIVOT(
SUM([Count]) For City IN([1],[2],[3],[4],[5])
)
AS PVT
ORDER BY CovidDate ASC