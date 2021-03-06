



ALTER Function [dbo].[ufn_AgentTotalLeftRight]
	(@AgentCode	 varchar(50))
	returns @t table (TotLeft int, TotRight int) 
	--returns varchar(50)	
as
begin

Declare @TotLeft int;
--Declare @TotRight int;


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

insert into @t values(
(select count(*) from TreeReport where agentCode <> @AgentCode and Position=1),
(select count(*) from TreeReport where agentCode <> @AgentCode and Position=2)
)

return 
end











