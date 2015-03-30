--[usp_AgentDetails] '1900010101'
/****** Object:  StoredProcedure [dbo].[PosionWiseAgentCount]    Script Date: 03/05/2015 01:41:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[usp_AgentDetails]
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


select 
(select count(*) from TreeReport where agentCode <> @AgentCode and Position=1) as leftcount,
(select count(*) from TreeReport where agentCode <> @AgentCode and Position=2) as rightcount

end
