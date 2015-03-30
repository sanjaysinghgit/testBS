--[usp_AgentTree] '1900010101'
/****** Object:  StoredProcedure [dbo].[usp_AgentGenerationTree]    Script Date: 3/22/2015 1:25:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[usp_AgentTree]
	@AgentCode	 varchar(50) 
as
begin

WITH TreeReport (AgentCode,SponsorCode,IntroducerCode,Position,Status)
AS
(
	--Create base record T(0)
    SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position, a.Status
		FROM Agent AS a where a.Code=@AgentCode		
    UNION ALL 
	-- Recurse till T(n)
	SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,
		a.Status
		FROM Agent AS a 
		INNER JOIN TreeReport AS d ON a.IntroducerCode = d.AgentCode
)

---- Executes the CTE

SELECT 'SrNo' = ROW_NUMBER() OVER(ORDER BY AgentCode asc ),
	AgentCode,SponsorCode,IntroducerCode,Position,status FROM TreeReport
end