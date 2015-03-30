Alter PROCEDURE [dbo].[AgentTree]
	@AgentCode	 varchar(50) 
as
begin

WITH TreeReport (AgentCode,SponsorCode,IntroducerCode,Position,Level)
AS
(
	--Create base record T(0)
    SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,0 AS Level
		FROM Agent AS a where a.Code=@AgentCode		
    UNION ALL 
	-- Recurse till T(n)
	SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,
		Level + 1
		FROM Agent AS a 
		INNER JOIN TreeReport AS d ON a.SponsorCode = d.AgentCode
)

---- Executes the CTE

SELECT 'SrNo' = ROW_NUMBER() OVER(ORDER BY Level asc ),
	AgentCode,SponsorCode,IntroducerCode,Position,Level FROM TreeReport
end