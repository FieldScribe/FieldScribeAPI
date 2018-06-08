CREATE VIEW [dbo].[ResultType] AS
	SELECT e.EventID, e.EventType, m.MeasurementType
	FROM Events e JOIN Meets m 
	ON e.MeetID = m.MeetId