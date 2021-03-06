DROP Function [dbo].[ufn_AgentLeftDepthCode]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create Function [dbo].[ufn_AgentLeftDepthCode]
	(@AgentCode	 varchar(50))
	
	returns varchar(50)
as

begin

declare @DLeftCode varchar(50);
declare @DownLeftagent varchar(50)
set @DownLeftagent = (select LeftAgent from Agent where code = @AgentCode);

WITH TreeReport (AgentCode,SponsorCode,IntroducerCode,Position,Level)
AS
(
	--Create base record T(0)
    SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,0 AS Level
		FROM Agent AS a where a.Code=@DownLeftagent		
    UNION ALL 
	-- Recurse till T(n)
	SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,
		Level + 1
		FROM Agent AS a 
		INNER JOIN TreeReport AS d ON a.SponsorCode = d.AgentCode and a.Position = 1
)

---- Executes the CTE

 select @DLeftCode =(SELECT AgentCode FROM TreeReport where Level = (select MAX(level) from TreeReport ))

return @DLeftCode
end	